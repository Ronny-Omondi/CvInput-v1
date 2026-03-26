using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvProcessor.Core.Interfaces;
using CsvProcessor.Core.Models;
using Microsoft.AspNetCore.Http;

namespace CsvProcessor.Core.Services;

public class MultiPartCsvReader : ICsvReader
{
    private readonly InputData _inputData;
    public MultiPartCsvReader(InputData inputData)
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

        var csvFiles = GetAllMultipartCsvPath(_inputData.Files);

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

    //helper method to save the uploaded files to a temporary location and return their paths
    /// <summary>
    /// Saves the uploaded files to a temporary location and returns their paths. This is necessary because CsvHelper works with file streams, and we need to save the uploaded files to disk before we can read them.
    /// </summary>
    /// <param name="uploadedFiles">The list of uploaded files.</param>
    /// <returns>A list of paths to the saved files.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public List<string> GetAllMultipartCsvPath(List<IFormFile> uploadedFiles)
    {
        var files = new List<string>();

        if (uploadedFiles == null)
        {
            throw new ArgumentNullException("Files cannot be null");
        }

        foreach (var file in uploadedFiles)
        {
            if (!string.Equals(Path.GetExtension(file.FileName), ".csv", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("File extension must be .csv");
            var temp = Path.Combine(Path.GetTempPath(), file.FileName);

            using (var stream = new FileStream(temp, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            files.Add(temp);
        }

        return files;
    }
}
