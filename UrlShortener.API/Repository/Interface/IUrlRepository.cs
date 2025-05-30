
using UrlShortener.API.Models;
using UrlShortener.Models.Url;

namespace UrlShortener.Repository.Interface
{
    public interface IUrlRepository
    {
        Task CreateAsync(UrlMapping mapping);
        Task<UrlMapping> GetByShortUrlAsync(string shortUrl);
    }
    
}