

using UrlShortener.Models.Url;

namespace UrlShortener.API.Processor.Inteface
{
    public interface IUrlProcessor
    {
        Task<string> ShortenUrlAsync(ShortenUrlRequest shortenUrlRequest);
        Task<string> RedirectToLongUrl(string shortCode);
    }    
}
