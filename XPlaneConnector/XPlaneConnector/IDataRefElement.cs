namespace XPlaneConnector
{
    public interface IDataRefElement
    {
        string DataRef { get; set; }
        int Frequency { get; set; }
        int IsInitialized { get; set; }
        string Units { get; set; }
        string Description { get; set; }
        bool Update<T>(int id, T value);
    }
}