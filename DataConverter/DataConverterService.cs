using System;
using System.IO;
using System.Threading.Tasks;
using DataConverter.Configuration;
using DataConverter.DataProcessors;
using DataConverter.FileReaders;
using DataConverter.OutputConverters;
using DataConverter.OutputWriters;

namespace DataConverter
{
    public class DataConverterService : IDataConverterService
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFileReader _fileReader;
        private readonly IDataProcessorFactory _dataProcessorFactory;
        private readonly IOutputConverterFactory _outputConverterFactory;
        private readonly IOutputWriterFactory _outputWriterFactory;

        public DataConverterService(IConfigurationService configurationService,
            IFileReader fileReader,
            IDataProcessorFactory dataProcessorFactory,
            IOutputConverterFactory outputConverterFactory,
            IOutputWriterFactory outputWriterFactory)
        {
            _configurationService = configurationService;
            _fileReader = fileReader;
            _dataProcessorFactory = dataProcessorFactory;
            _outputConverterFactory = outputConverterFactory;
            _outputWriterFactory = outputWriterFactory;
        }

        public async Task RunAsync()
        {
            var inputPath = _configurationService.GetInputPath();

            var processedData = await ProcessData(inputPath);

            var outputString = ConvertData(processedData);

            await WriteOutput(outputString);
        }
        
        private async Task<object> ProcessData(string inputPath)
        {
            Console.WriteLine($"Reading data from {inputPath}");

            await using var fileStream = File.OpenRead(inputPath);

            var totalLineCount = 0;
            var validLineCount = 0;
            
            IDataProcessor dataProcessor = null;

            await foreach (var dataLine in _fileReader.Read(fileStream, 0))
            {
                if (totalLineCount == 0)
                {
                    dataProcessor = _dataProcessorFactory.Create(dataLine);
                }
                else
                {
                    if (dataProcessor.ProcessLine(dataLine))
                    {
                        validLineCount++;
                    }
                }

                totalLineCount++;
            }

            Console.WriteLine($"Finished reading data - {totalLineCount} total lines; {validLineCount} valid lines");

            return dataProcessor.GetSummary();
        }

        private string ConvertData(object processedData)
        {
            string outputFormat = _configurationService.GetOutputFormat();

            var outputConverter = _outputConverterFactory.Create(outputFormat);

            var outputString = outputConverter.Convert(outputFormat, processedData);

            return outputString;
        }

        private async Task WriteOutput(string outputString)
        {
            var outputType = _configurationService.GetOutputType();

            var outputWriter = _outputWriterFactory.Create(outputType);

            await outputWriter.WriteAsync(outputString);
        }
    }
}
