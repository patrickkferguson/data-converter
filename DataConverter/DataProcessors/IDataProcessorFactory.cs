namespace DataConverter.DataProcessors
{
    public interface IDataProcessorFactory
    {
        IDataProcessor Create(string headerLine);
    }
}
