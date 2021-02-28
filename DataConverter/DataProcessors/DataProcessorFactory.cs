using System;
using System.Collections.Generic;
using System.Linq;

namespace DataConverter.DataProcessors
{
    public class DataProcessorFactory : IDataProcessorFactory
    {
        private readonly IEnumerable<IDataProcessor> _dataProcessors;

        public DataProcessorFactory(IEnumerable<IDataProcessor> dataProcessors)
        {
            _dataProcessors = dataProcessors;
        }

        public IDataProcessor Create(string headerLine)
        {
            var converter = _dataProcessors.FirstOrDefault(c => c.CanProcess(headerLine));

            if (converter == null)
            {
                throw new InvalidOperationException($"No registered data processor could recognise the header line from the input file.");
            }

            return converter;
        }
    }
}
