using System;
using System.Text.Json;
using CsvProcessor.Core.Interfaces;
using CsvProcessor.Core.Models;
using YamlDotNet.Serialization.NamingConventions;

namespace CsvProcessor.Core.Services;

public class ConfigurationLoader : IConfigurationLoader
{
    public async Task<CsvMatchFilterConfig> LoadConfigurations(Stream stream)
    {
        if (stream == null)
            throw new ArgumentNullException("Stream cannot be null");
        using var reader = new StreamReader(stream);
        var text = await reader.ReadToEndAsync();
        return LoadStinConfigurations(text);
    }

    /// <summary>
    /// Loads the configuration from a file path. This method reads the entire file content and then processes it
    ///  to extract the configuration settings. It supports both JSON and YAML formats, automatically determining 
    /// the format based on the content of the file. If the file does not exist, it throws a FileNotFoundException.
    /// </summary>
    /// <param name="path">The path to the configuration file</param>
    /// <returns>The loaded configuration</returns>
    /// <exception cref="FileNotFoundException"></exception>
    public async Task<CsvMatchFilterConfig> LoadFromFileAsync(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Config file not found: {path}");
        var text = await File.ReadAllTextAsync(path);
        return LoadStinConfigurations(text);
    }
    public CsvMatchFilterConfig LoadStinConfigurations(string configText)
    {
        if (string.IsNullOrWhiteSpace(configText))
            throw new ArgumentException("Configuration text cannot be null or empty");
            
        if (configText.TrimStart().StartsWith("{"))
        {
            var config = JsonSerializer.Deserialize<CsvMatchFilterConfig>(configText,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return config!;
        }
        else
        {
            var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var config = deserializer.Deserialize<CsvMatchFilterConfig>(configText);
            return config;
        }
    }
}
