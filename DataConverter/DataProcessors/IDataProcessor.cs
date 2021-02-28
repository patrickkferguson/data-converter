namespace DataConverter.DataProcessors
{
    public interface IDataProcessor
    {
        bool CanProcess(string headerLine);

        bool ProcessLine(string dataLine);

        object GetSummary();
    }
}
