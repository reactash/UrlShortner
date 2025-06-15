using UrlShortener.API.Models;
using UrlShortener.API.Models.Analytics;


namespace UrlShortener.API.Repository.Interface
{
    public interface IUrlRepository
    {
        Task CreateAsync(UrlMapping mapping);
        Task<UrlMapping> GetByShortUrlAsync(string shortUrl);
        Task StoreAnalyticsDataAsync(UrlAnalytics urlAnalytics);
    }
    
}