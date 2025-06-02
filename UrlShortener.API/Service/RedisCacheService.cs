using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using UrlShortener.API.Service;

public class RedisCacheService : IRedisCacheService
{
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _db;

    public RedisCacheService(string redisConnectionString)
    {
       // var redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION");
        _redis = ConnectionMultiplexer.Connect(redisConnectionString);
        _db = _redis.GetDatabase();
    }

    // Save cache
    public void Set(string key, string value, TimeSpan? expiry = null)
    {
        _db.StringSet(key, value, expiry);
    }

    // Get cache
    public string Get(string key)
    {
        return _db.StringGet(key);
    }
}
