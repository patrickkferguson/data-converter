using System.Collections.Generic;
using System.IO;

namespace DataConverter
{
    public class FileReader
    {
        public async IAsyncEnumerable<string> Read(Stream fileStream, int skipLines)
        {
            await using (fileStream)
            {
                using var reader = new StreamReader(fileStream);

                for (var i = 0; i < skipLines; i++)
                {
                    await reader.ReadLineAsync();
                }

                var line = await reader.ReadLineAsync();
                while (line != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        yield return line;
                    }

                    line = await reader.ReadLineAsync();
                }
            }
        }
    }
}
