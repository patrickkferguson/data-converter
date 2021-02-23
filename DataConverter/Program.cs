using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DataConverter.Models;
using Microsoft.Extensions.Configuration;

namespace DataConverter
{
    internal class Program
    {
        internal static async Task Main(string[] args)
        {
            var switchMappings = new Dictionary<string, string>()
            {
                { "-i", "input" },
                { "-o", "output" },
            };

            var builder = new ConfigurationBuilder();
            builder.AddCommandLine(args, switchMappings);

            var config = builder.Build();

            var inputPath = string.IsNullOrEmpty(config["input"])
                ? "C:\\Temp\\IDCJAC0009_066062_1800_Data.csv"
                : config["input"];

            if (!File.Exists(inputPath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"No file found at the specified path {inputPath}");
                Console.ResetColor();
                return;
            }

            Console.WriteLine($"Reading data from {inputPath}");

            await using var fileStream = File.OpenRead(inputPath);

            var fileReader = new FileReader();
            var totalLineCount = 0;
            var validLineCount = 0;

            var aggregator = new WeatherDataAggregator();

            await foreach (var line in fileReader.Read(fileStream, 1))
            {
                if (BomRainfallData.TryParse(line, out var data))
                {
                    aggregator.Aggregate(data);
                    validLineCount++;
                }

                totalLineCount++;
            }

            Console.WriteLine($"Finished reading data - {totalLineCount} total lines; {validLineCount} valid lines");

            var summary = aggregator.GetSummary();

            var jsonString = JsonSerializer.Serialize(summary);

            var outputPath = config["output"];

            if (string.IsNullOrEmpty(outputPath))
            {
                outputPath = Path.Combine(
                    Path.GetDirectoryName(inputPath),
                    $"{Path.GetFileNameWithoutExtension(inputPath)}.json");
            }

            Console.WriteLine($"Writing JSON output to {outputPath}");

            await File.WriteAllTextAsync(outputPath, jsonString);
        }
    }
}
