
using UAParser;

public class BrowserInfo
{
    public string Browser { get; set; }
    public string Platform { get; set;}
}

public static class UserAgentParser
{
    public static readonly Parser _parser = Parser.GetDefault();

    public static BrowserInfo ParserUserAgent(string userAgent)
    {

        var clientInfo = _parser.Parse(userAgent);
        return new BrowserInfo
        {
            Browser = $"{clientInfo.UA.Family} {clientInfo.UA.Major}",
            Platform = clientInfo.OS.Family
        };
    } 
}