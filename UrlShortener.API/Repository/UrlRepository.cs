using MongoDB.Driver;
using UrlShortener.API.Models;
using UrlShortener.API.Models.Analytics;
using UrlShortener.API.Repository.Interface;

namespace UrlShortener.Repository
{
    public class UrlRepository : IUrlRepository
    {
        private readonly IMongoCollection<UrlMapping> _urlCollection;
        private readonly IMongoCollection<UrlAnalytics> _urlAnalytics;

        public UrlRepository(IMongoDatabase db)
        {

            _urlCollection = db.GetCollection<UrlMapping>("UrlMappings");
            _urlAnalytics  = db.GetCollection<UrlAnalytics>("UrlAnalytics");
        }

        public async Task CreateAsync(UrlMapping mapping)
        {
            await _urlCollection.InsertOneAsync(mapping);
        }

        public async Task<UrlMapping> GetByShortUrlAsync(string shortUrl)
        {
            return await _urlCollection.Find(x => x.ShortCode == shortUrl).FirstOrDefaultAsync();
        }
            
        public async Task StoreAnalyticsDataAsync(UrlAnalytics urlAnalytics)
        {
             await _urlAnalytics.InsertOneAsync(urlAnalytics);
        }

    }
    
}