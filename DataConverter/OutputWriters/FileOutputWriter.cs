 using System;
using System.IO;
using System.Threading.Tasks;
using DataConverter.Configuration;

namespace DataConverter.OutputWriters
{
    public class FileOutputWriter : IOutputWriter
    {
        private readonly IConfigurationService _configurationService;

        public FileOutputWriter(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public bool Supports(string outputType)
        {
            return outputType != null && outputType.ToLower() == "file";
        }

        public async Task WriteAsync(string output)
        {
            var outputPath = GetOutputPath();

            Console.WriteLine($"Writing output to {outputPath}");

            await File.WriteAllTextAsync(outputPath, output);
        }

        private string GetOutputPath()
        {
            var outputPath = _configurationService.GetOutput();

            if (string.IsNullOrEmpty(outputPath))
            {
                var inputPath = _configurationService.GetInputPath();

                outputPath = Path.Combine(
                    Path.GetDirectoryName(inputPath),
                    $"{Path.GetFileNameWithoutExtension(inputPath)}.{GetFileExtension()}");
            }

            return outputPath;
        }

        private string GetFileExtension()
        {
            var outputFormat = _configurationService.GetOutputFormat();

            if (outputFormat.ToLower().StartsWith("json"))
            {
                return "json";
            }

            throw new InvalidOperationException($"Unable to determine a default file extension for output format '{outputFormat}'. Please specify an output path using the --output argument.");
        }
    }
}
