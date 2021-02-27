using System.Threading.Tasks;

namespace DataConverter.OutputWriters
{
    public interface IOutputWriter
    {
        bool Supports(string outputType);

        Task WriteAsync(string output);
    }
}
