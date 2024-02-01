namespace PubSubTester;

public class AddMessageRequest
{
    public string EmulatorPort { get; set; } = string.Empty;
    public string ProjectId { get; set; } = string.Empty;
    public string TopicName { get; set; } = string.Empty;
    public object Data { get; set; } = null!;
    public Dictionary<string, string> Attributes { get; set; } = [];
}