using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvProcessor.Core.Interfaces;
using CsvProcessor.Core.Models;

namespace CsvProcessor.Core.Services;

public class StreamCsvReader : ICsvReader
{
    private readonly InputData _inputData;
    public StreamCsvReader(InputData inputData)
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

        using var reader = new StreamReader(_inputData.Stream);
        using var csv = new CsvReader(reader, configuration);
        while (csv.Read())
        {
            var record = csv.Parser.Record;
            if (record != null)
                yield return record;
        }
    }
}
