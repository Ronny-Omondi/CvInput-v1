using System;
using CsvProcessor.Core.Models;

namespace CsvProcessor.Core.Interfaces;

/// <summary>
/// Defines methods for loading configuration data used by the CSV
/// filtering and matching services.
/// </summary>
public interface IConfigurationLoader
{
    /// <summary>
    /// Loads the configuration from a file path.
    /// </summary>
    /// <param name="configurationFile">The path to the configuration file.</param>
    /// <returns>The loaded configuration.</returns>
    public CsvMatchFilterConfig LoadStinConfigurations(string configurationFile);

    /// <summary>
    /// Loads the configuration from a stream. This is useful for scenarios where
    ///  the configuration is uploaded as a file or provided as a stream from another source.
    ///  The method reads the entire stream and then processes it to extract the configuration settings.
    /// </summary>
    /// <param name="stream">The stream containing the configuration data.</param>
    /// <returns>The loaded configuration.</returns>
    public Task<CsvMatchFilterConfig> LoadConfigurations(Stream stream);
}
