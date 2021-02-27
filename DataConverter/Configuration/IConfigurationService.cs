namespace DataConverter.Configuration
{
    public interface IConfigurationService
    {
        string GetInputPath();

        string GetOutput();

        string GetOutputFormat();

        string GetOutputType();
    }
}
