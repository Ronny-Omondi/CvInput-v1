using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvProcessor.Core.Interfaces;
using CsvProcessor.Core.Models;

namespace CsvProcessor.Core.Services;

public class FolderCsvReader : ICsvReader
{
    private readonly InputData _inputData;
    public FolderCsvReader(InputData inputData)
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

        var csvFiles = GetAllCsvPath(_inputData.Pattern, _inputData.SearchDir);

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

    //helper method to get all csv files in a directory
    /// <summary>
    /// Gets all csv files in a directory based on a search pattern
    /// </summary>
    /// <param name="pattern">The search pattern</param>
    /// <param name="searchDir">The directory to search in</param>
    /// <returns>The list of csv file paths</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    public List<string> GetAllCsvPath(string pattern, string searchDir)
    {
        if (string.IsNullOrWhiteSpace(pattern))
        {
            throw new ArgumentNullException("Pattern cannot be null or empty");
        }
        if (string.IsNullOrWhiteSpace(searchDir))
        {
            throw new ArgumentNullException("Search directory cannot be null or empty");
        }
        if (!Directory.Exists(searchDir))
        {
            throw new DirectoryNotFoundException("Directory does not exist");
        }

        var files = Directory.GetFiles(searchDir, pattern, SearchOption.TopDirectoryOnly).Where(f => string.Equals(Path.GetExtension(f), ".csv", StringComparison.OrdinalIgnoreCase)).ToList();

        return files;
    }
}
