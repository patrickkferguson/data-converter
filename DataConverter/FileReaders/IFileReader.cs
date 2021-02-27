using System.Collections.Generic;
using System.IO;

namespace DataConverter.FileReaders
{
    public interface IFileReader
    {
        IAsyncEnumerable<string> Read(Stream fileStream, int skipLines);
    }
}
