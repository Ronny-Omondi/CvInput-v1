using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvProcessor.Core.Enum;
using CsvProcessor.Core.Interfaces;
using CsvProcessor.Core.Models;

namespace CsvProcessor.Core.Services;

public class Filter : IFilter
{
    public async Task<HashSet<string>> BuildFilterAsync(FilterData filterData)
    {
        if (filterData == null)
            throw new ArgumentNullException("Filter data must be provided");

        return FilterSetBuilder(await ExtractColumnValues(filterData), filterData.JoinBy);
    }

    /// <summary>
    /// Extracts the column values from the specified CSV file based on the provided filter data.
    /// </summary>
    /// <param name="filterData">The filter data</param>
    /// <returns>The list of column values</returns>
    /// <exception cref="ArgumentNullException"></exception>
    private async Task<IEnumerable<IEnumerable<string>>> ExtractColumnValues(FilterData filterData)
    {
        if (filterData == null) throw new ArgumentNullException("Filter data cannot be null");
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = filterData.HasHeader,
            Delimiter = filterData.Delimeter,
        };

        using var reader = new StreamReader(filterData.FilePath);
        using var csvReader = new CsvReader(reader, config);
        if (filterData.HasHeader)
        {
            await csvReader.ReadAsync();
            csvReader.ReadHeader();
        }

        var columnList = new List<HashSet<string>>();
        for (int i = 0; i < filterData.Columns.Count; i++)
        {
            columnList.Add(new HashSet<string>());
        }
        while (await csvReader.ReadAsync())
        {
            for (int i = 0; i < filterData.Columns.Count; i++)
            {
                var col = filterData.Columns[i];
                var value = csvReader.GetField(col);
                if (!string.IsNullOrEmpty(value))
                    columnList[i].Add(value);
            }
        }
        return columnList;
    }

    /// <summary>
    /// Builds a filter set based on the provided sets and the specified join action (union or intersection).
    /// </summary>
    /// <param name="sets">The sets to combine</param>
    /// <param name="mode">The join action to perform</param>
    /// <returns>The combined filter set</returns>
    /// <exception cref="ArgumentNullException"></exception>
    private HashSet<string> FilterSetBuilder(IEnumerable<IEnumerable<string>> sets, JoinAction mode)
    {
        HashSet<string> filtered = null!;
        if (sets == null)
            throw new ArgumentNullException("Filter set cannot be null");
        foreach (var set in sets)
        {
            if (filtered == null)
            {
                filtered = new HashSet<string>(set, StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                if (mode == JoinAction.Union)
                {
                    filtered.UnionWith(set);
                }
                else
                {
                    filtered.IntersectWith(set);
                }
            }
        }

        return filtered;
    }
}
