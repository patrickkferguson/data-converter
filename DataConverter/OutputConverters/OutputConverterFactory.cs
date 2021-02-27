using System;
using System.Collections.Generic;
using System.Linq;

namespace DataConverter.OutputConverters
{
    public class OutputConverterFactory : IOutputConverterFactory
    {
        private readonly IEnumerable<IOutputConverter> _outputConverters;

        public OutputConverterFactory(IEnumerable<IOutputConverter> outputConverters)
        {
            _outputConverters = outputConverters;
        }

        public IOutputConverter Create(string outputFormat)
        {
            var converter = _outputConverters.FirstOrDefault(c => c.Supports(outputFormat));

            if (converter == null)
            {
                throw new InvalidOperationException($"No output converter registered for output format '{outputFormat}'.");
            }

            return converter;
        }
    }
}
