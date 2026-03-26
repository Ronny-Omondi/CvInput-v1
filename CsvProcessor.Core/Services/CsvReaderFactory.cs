using System;
using CsvProcessor.Core.Interfaces;
using CsvProcessor.Core.Models;

namespace CsvProcessor.Core.Services;

/// <summary>
/// Factory class to create instances of ICsvReader based on the input data provided. 
/// It checks the properties of the input data and returns the appropriate reader implementation
///  (StreamCsvReader, FileCsvReader, MultipleFileReader, FolderCsvReader, MultiPartCsvReader)
///  based on the available data. If the input data is invalid or does not match any reader type,
///  it throws an ArgumentException. This factory pattern allows for flexibility and separation of concerns
///  when creating different types of CSV readers.
/// </summary>
public class CsvReaderFactory
{
    /// <summary>
    /// Creates an instance of ICsvReader based on the properties of the provided InputData.
    ///  It checks for the presence of a stream, file path, multiple file paths,
    ///  search directory and pattern, or multipart files to determine which reader implementation to return.
    ///  If the input data is invalid or does not match any reader type, it throws an ArgumentException.
    /// </summary>
    /// <param name="inputData">The input data for the CSV reader</param>
    /// <returns>The created CSV reader instance</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static ICsvReader Create(InputData inputData)
    {
        if (inputData == null)
        {
            throw new ArgumentNullException(nameof(inputData));
        }

        if (inputData.Stream != null && inputData.Stream.CanRead)
        {
            return new StreamCsvReader(inputData);
        }
        if (!string.IsNullOrWhiteSpace(inputData.FilePath))
        {
            return new FileCsvReader(inputData);
        }
        if (inputData.FilePaths != null && inputData.FilePaths.Count > 0)
        {
            return new MultipleFileReader(inputData);
        }
        if (!string.IsNullOrWhiteSpace(inputData.SearchDir) && !string.IsNullOrWhiteSpace(inputData.Pattern))
        {
            return new FolderCsvReader(inputData);
        }
        if (inputData.Files != null && inputData.Files.Count > 0 )
        {
            return new MultiPartCsvReader(inputData);
        }

        throw new ArgumentException("Invalid input data", nameof(inputData)); 
    }
}
