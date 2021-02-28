using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataConverter.Configuration;
using DataConverter.DataProcessors;
using DataConverter.Extensions;
using DataConverter.FileReaders;
using DataConverter.OutputConverters;
using DataConverter.OutputWriters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DataConverter
{
    internal class Program
    {
        internal static async Task Main(string[] args)
        {
            var switchMappings = new Dictionary<string, string>()
            {
                { "-i", "inputPath" },
                { "-o", "output" },
                { "-ot", "outputType" },
                { "-of", "outputFormat" },
            };

            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration(configurationBuilder =>
                    {
                        configurationBuilder.AddCommandLine(args, switchMappings);
                    })
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton<IConfigurationService, ConfigurationService>();
                    services.AddSingleton<IDataConverterService, DataConverterService>();
                    services.AddSingleton<IFileReader, FileReader>();
                    AddDataProcessors(services);
                    AddOutputConverters(services);
                    AddOutputWriters(services);
                });

            try
            {
                var host = hostBuilder.Build();
                var converterService = host.Services.GetRequiredService<IDataConverterService>();

                await converterService.RunAsync();
            }
            catch (Exception exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(exception.Message);
                Console.ResetColor();
            }
        }

        private static void AddDataProcessors(IServiceCollection services)
        {
            services.AddSingleton<IDataProcessorFactory, DataProcessorFactory>();
            services.AddTransientAllImplementations<IDataProcessor>();
        }

        private static void AddOutputConverters(IServiceCollection services)
        {
            services.AddSingleton<IOutputConverterFactory, OutputConverterFactory>();
            services.AddTransientAllImplementations<IOutputConverter>();
        }

        private static void AddOutputWriters(IServiceCollection services)
        {
            services.AddSingleton<IOutputWriterFactory, OutputWriterFactory>();
            services.AddTransientAllImplementations<IOutputWriter>();
        }
    }
}
