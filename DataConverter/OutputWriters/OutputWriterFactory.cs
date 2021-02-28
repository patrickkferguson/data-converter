using System;
using System.Collections.Generic;
using System.Linq;

namespace DataConverter.OutputWriters
{
    public class OutputWriterFactory : IOutputWriterFactory
    {
        private readonly IEnumerable<IOutputWriter> _outputWriters;

        public OutputWriterFactory(IEnumerable<IOutputWriter> outputWriters)
        {
            _outputWriters = outputWriters;
        }

        public IOutputWriter Create(string outputType)
        {
            var writer = _outputWriters.FirstOrDefault(c => c.Supports(outputType));

            if (writer == null)
            {
                throw new InvalidOperationException($"No output writer registered for output type '{outputType}'.");
            }

            return writer;
        }
    }
}
