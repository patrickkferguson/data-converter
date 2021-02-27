using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DataConverter.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetInputPath()
        {
            var inputPath = _configuration.GetValue<string>("inputPath");

            if (string.IsNullOrEmpty(inputPath))
            {
                throw new InvalidOperationException("Required argument 'inputPath' was not provided.");
            }

            if (!File.Exists(inputPath))
            {
                throw new InvalidOperationException($"No file found at the specified input path '{inputPath}'.");
            }

            return inputPath;
        }

        public string GetOutput()
        {
            var outputPath = _configuration.GetValue<string>("output");
            return outputPath;
        }

        public string GetOutputFormat()
        {
            var outputFormat = _configuration.GetValue<string>("outputFormat");

            return string.IsNullOrEmpty(outputFormat) ? "json" : outputFormat;
        }

        public string GetOutputType()
        {
            var outputType = _configuration.GetValue<string>("outputType");

            return string.IsNullOrEmpty(outputType) ? "file" : outputType;
        }
    }
}
