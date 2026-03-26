using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvProcessor.Core.Interfaces;
using CsvProcessor.Core.Models;

namespace CsvProcessor.Core.Services;

public class MultipleFileReader : ICsvReader
{
    private readonly InputData _inputData;
    public MultipleFileReader(InputData inputData)
    {
        _inputData = inputData;
    }

    public async IAsyncEnumerable<string[]> ReadFileAsync()
    {
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = _inputData.HasHeader,
            Delimiter = _inputData.Delimeter
        };
        var csvFiles = ValidateCsvFiles(_inputData.FilePaths);

        foreach (var file in csvFiles)
        {
            using var reader = new StreamReader(file);
            using var csv = new CsvReader(reader, configuration);

            while (csv.Read())
            {
                var record = csv.Parser.Record;
                if (record != null)
                    yield return record;
            }
        }
    }

    //helper validate method to check if files exist and are csv
    /// <summary>
    /// Validates the file paths and checks if the files exist and are csv files
    /// </summary>
    /// <param name="filePaths">The list of file paths to validate</param>
    /// <returns>The list of validated file paths</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public List<string> ValidateCsvFiles(List<string> filePaths)
    {
        if (filePaths == null || !filePaths.Any())
        {
            throw new ArgumentNullException("File paths cannot be null or empty");
        }

        foreach (var file in filePaths)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException("Csv file not found", nameof(file));
            }
            if (!string.Equals(Path.GetExtension(file), ".csv", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("File extension must .csv");
            }
        }
        return filePaths;
    }
}
