using System.Text;

namespace NugetPackage.MiddlewareLogs.NewFolder;
public sealed class RequestResponseData
{
    public string? Application { get; set; }
    public string? ApplicationVersion { get; set; }
    public DateTime? RequestTimestamp { get; set; }
    public string RequestUri { get; set; }
    public string RequestMethod { get; set; }
    public string? RequestBody { get; set; }
    public string IpAddress { get; set; }
    public string RequestHeaders { get; set; }
    public string RequestContentType { get; set; }
    public DateTime? ResponseTimestamp { get; set; }
    public string ResponseBody { get; set; }
    public int ResponseStatusCode { get; set; }
    public string Machine { get; set; }
    public string? User { get; set; }
    public override string ToString()
    {
        string separator = string.Concat(Enumerable.Repeat("*", 50));
        var lb = "\r\n";
        StringBuilder sb = new();

        sb.Append($"{lb}{separator}{lb}");
        sb.Append($"Http Request Information: {lb}");
        sb.Append($"Application: {Application}{lb}");
        sb.Append($"Application-Version: {ApplicationVersion}{lb}");
        sb.Append($"Machine: {Machine}{lb}");
        sb.Append($"User: {User}{lb}");
        sb.Append($"Request Timestamp: {RequestTimestamp}{lb}");
        sb.Append($"Request IpAddress: {IpAddress}{lb}");
        sb.Append($"Request Uri: {RequestUri}{lb}");
        sb.Append($"Request Method: {RequestMethod}{lb}");
        sb.Append($"RequestHeaders: {RequestHeaders}{lb}");
        sb.Append($"Request ContentType: {RequestContentType}{lb}");
        sb.Append($"Request ContentBody: {RequestBody}{lb}");
        sb.Append($"RequestHeaders: {RequestHeaders}{lb}");

        sb.Append($"{Environment.NewLine}Http Response Information: {Environment.NewLine}");
        sb.Append($"ResponseStatusCode: {ResponseStatusCode}{lb}");
        sb.Append($"ResponseContentBody: {ResponseBody}{lb}");
        sb.Append($"{lb}{separator}{lb}{Environment.NewLine}");

        return sb.ToString();
    }
}