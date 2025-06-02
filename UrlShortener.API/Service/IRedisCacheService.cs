

namespace UrlShortener.API.Service
{
    public interface IRedisCacheService
    {
        string Get(string key);
        void Set(string key, string value, TimeSpan? expiry = null);

    }

}