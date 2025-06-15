using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UrlShortener.API.Models.Analytics
{
    public class UrlAnalytics
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string Url { get; set; }
        public string ShortUrl { get; set; }
        public string Browser { get; set; }
        public string Platform { get; set; }
        public string Ip { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public Location Location { get; set; }
    }

}