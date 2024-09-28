namespace GenericHttpClient.Contracts;
public sealed class HttpClientOptions
{
    public string BaseAddress { get; set; }
    public int TimeoutSeconds { get; set; }
    public Dictionary<string, string> DefaultHeaders { get; set; }
    public int HttpClientLifeTime { get; set; } // default is 5 minutes.
}