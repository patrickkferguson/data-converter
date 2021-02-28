namespace DataConverter.OutputWriters
{
    public interface IOutputWriterFactory
    {
        IOutputWriter Create(string outputType);
    }
}
