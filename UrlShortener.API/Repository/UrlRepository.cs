using MongoDB.Driver;
using UrlShortener.API.Models;
using UrlShortener.Repository.Interface;

namespace UrlShortener.Repository
{
    public class UrlRepository : IUrlRepository
    {
        private readonly IMongoCollection<UrlMapping> _urlCollection;

        public UrlRepository(IMongoCollection<UrlMapping> urlCollection)
        {
            _urlCollection = urlCollection;
        }

        public async Task CreateAsync(UrlMapping mapping)
        {
            await _urlCollection.InsertOneAsync(mapping);
        }

        public async Task<UrlMapping> GetByShortUrlAsync(string shortUrl)
        {
            return await _urlCollection.Find(x => x.ShortCode == shortUrl).FirstOrDefaultAsync();
        }
        
    }
    
}