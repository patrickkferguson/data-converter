namespace DataConverter.OutputConverters
{
    public interface IOutputConverter
    {
        bool Supports(string outputFormat);

        string Convert(string outputFormat, object data);

        string GetFileExtension();
    }
}
