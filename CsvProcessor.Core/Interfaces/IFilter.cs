using System;
using CsvProcessor.Core.Enum;
using CsvProcessor.Core.Models;

namespace CsvProcessor.Core.Interfaces;

/// <summary>
/// Defines methods for building and applying filters to CSV data,
/// including set operations and matching strategies.
/// </summary>
public interface IFilter
{
    /// <summary>
    /// Asynchronously builds a filter set based on the provided filter data,
    ///  which includes the source of the filter values, the column to extract from,
    ///  and the join strategy to apply when combining multiple filter sets. 
    /// The method processes the input data according to the specified 
    /// configuration and returns a hash set of strings that represent 
    /// the unique filter values to be used in matching operations against CSV data.
    /// </summary>
    /// <param name="filterData"></param>
    /// <returns></returns>
    public Task<HashSet<string>> BuildFilterAsync(FilterData filterData);
}
