using System.Text.RegularExpressions;
using UrlShortener.API.Models;
using UrlShortener.API.Processor.Inteface;
using UrlShortener.API.Repository.Interface;
using UrlShortener.API.Service;
using UrlShortener.Models.Url;


namespace UrlShortener.Processor
{
    public class UrlProcessor : IUrlProcessor
    {
        private readonly IUrlRepository urlRepository;
        private readonly IRedisCacheService redisCacheService;
        private const int ShortCodeLength = 6;
        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";


        public UrlProcessor(IUrlRepository urlRepository, IRedisCacheService redisCacheService)
        {
            this.urlRepository = urlRepository;
            this.redisCacheService = redisCacheService;
        }

        private string GenerateShortCode()
        {
            var shorturl = new char[ShortCodeLength];
            var random = new Random();
            for (int i = 0; i < ShortCodeLength; i++)
            {
                var elemIndex = random.Next(AllowedChars.Length);
                shorturl[i] = AllowedChars[elemIndex];
            }
            return new string(shorturl);
        }

        private bool IsValidUrl(string longUrl)
        {
            if (!Uri.TryCreate(longUrl, UriKind.Absolute, out var uriResult))
                return false;

            if (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
                return false;

            var domainPattern = @"^([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$";

            return Regex.IsMatch(uriResult.Host, domainPattern);

        }



        public async Task<string> ShortenUrlAsync(ShortenUrlRequest shortenUrlRequest)
        {
            var longUrl = shortenUrlRequest.LongUrl;
            if (!IsValidUrl(longUrl))
                throw new ArgumentException("Invalid URL format.");

            string shorturl;

            do
            {
                shorturl = GenerateShortCode();

            }
            while (await this.urlRepository.GetByShortUrlAsync(shorturl) != null);


            var mapping = new UrlMapping
            {
                LongUrl = longUrl,
                ShortCode = shorturl,
                CreatedAt = DateTime.UtcNow
            };


            await this.urlRepository.CreateAsync(mapping);

             // ✅ Cache the new short URL in Redis
            redisCacheService.Set(shorturl, longUrl, TimeSpan.FromHours(1));

            return $"https://urlshortner-ziaw.onrender.com/s/{shorturl}";
        }

        public async Task<string> RedirectToLongUrl(string shortCode)
        {
            if (string.IsNullOrEmpty(shortCode))
                return string.Empty;

            // check if it exists in redis
            var cachedLongUrl = redisCacheService.Get(shortCode);
            if (!string.IsNullOrEmpty(cachedLongUrl))
            {
                return cachedLongUrl;
            }

            //fetch from db
            var originalurl = await urlRepository.GetByShortUrlAsync(shortCode);
            if (originalurl == null)
                return string.Empty;

            //set it in our redis
            redisCacheService.Set(shortCode, originalurl.LongUrl, TimeSpan.FromHours(1));

            return originalurl.LongUrl;
        }

    }
}