namespace Master.Utilities.Services.Abstraction.MessageBus;

public class Parcel
{
    public string Id { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public Dictionary<string, object> Headers { get; set; } = new();
    public string Body { get; set; } = String.Empty;
    public string Route { get; set; } = String.Empty;
    public string CorrelationId { get; set; } = String.Empty;
}