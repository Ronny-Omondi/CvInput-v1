using System;
using CsvProcessor.Core.Models;

namespace CsvProcessor.Core.Interfaces;

/// <summary>
/// Defines methods for reading CSV data from streams and returning
/// rows of values for further processing.
/// </summary>
public interface ICsvReader
{
    /// <summary>
    /// Asynchronously reads CSV data from the specified source and yields each row as an array of strings.
    ///  This method is designed to handle large CSV files efficiently by 
    /// streaming the data and processing it row by row, allowing for better
    ///  memory management and performance when dealing with extensive datasets.
    /// </summary>
    /// <returns>An asynchronous enumerable of string arrays representing the CSV rows.</returns>
    IAsyncEnumerable<string[]> ReadFileAsync();
}
