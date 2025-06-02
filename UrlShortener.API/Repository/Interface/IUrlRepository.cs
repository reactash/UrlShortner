using UrlShortener.API.Models;


namespace UrlShortener.API.Repository.Interface
{
    public interface IUrlRepository
    {
        Task CreateAsync(UrlMapping mapping);
        Task<UrlMapping> GetByShortUrlAsync(string shortUrl);
    }
    
}