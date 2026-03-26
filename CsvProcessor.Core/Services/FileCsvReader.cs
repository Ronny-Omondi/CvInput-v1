using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvProcessor.Core.Interfaces;
using CsvProcessor.Core.Models;

namespace CsvProcessor.Core.Services;

public class FileCsvReader : ICsvReader
{
    private readonly InputData _inputData;
    public FileCsvReader(InputData inputData)
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

        using var reader = new StreamReader(ValidateFilePath(_inputData.FilePath));
        using var csv = new CsvReader(reader, configuration);

        // if (_inputData.HasHeader)
        // {
        //     await csv.ReadAsync();
        //     csv.ReadHeader();
        // }
        while (csv.Read())
        {
            var record = csv.Parser.Record;
            if (record != null)
                yield return record;
        }
    }

    //helper method to validate file path and check if file exists and is csv
    /// <summary>
    /// Validates the file path and checks if the file exists and is a csv file
    /// </summary>
    /// <param name="filePath">The file path to validate</param>
    /// <returns>The validated file path </returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public string ValidateFilePath(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException("File path not found", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException("Csv file not found", nameof(filePath));
        if (!string.Equals(Path.GetExtension(filePath), ".csv", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("File extension must be .csv");

        return filePath;
    }
}
