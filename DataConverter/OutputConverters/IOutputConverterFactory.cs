namespace DataConverter.OutputConverters
{
    public interface IOutputConverterFactory
    {
        IOutputConverter Create(string outputFormat);
    }
}
