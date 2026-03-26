using System;
using CsvProcessor.Core.Models;

namespace CsvProcessor.Core.Interfaces;

/// <summary>
/// Defines methods for processing CSV input data, applying filters,
/// and producing match results based on configuration.
/// </summary>
public interface ICsvProcessor
{
    /// <summary>
    /// Processes the CSV input data according to the provided filter configuration
    ///  and returns the matching results. This method orchestrates the reading of CSV data,
    ///  applying the specified filters, and compiling the results into a structured format that
    ///  can be easily consumed by other components or services.
    /// </summary>
    /// <param name="csvMatchFilter">The CSV match filter configuration.</param>
    /// <returns>The task representing the asynchronous operation and the resulting matches.</returns>
    public Task<CsvMatchFilterResult> CsvInputResult(CsvMatchFilterConfig csvMatchFilter);
}
