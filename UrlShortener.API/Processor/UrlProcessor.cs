using System.Text.RegularExpressions;
using UrlShortener.API.Models;
using UrlShortener.API.Processor.Inteface;
using UrlShortener.Models.Url;
using UrlShortener.Repository.Interface;

namespace UrlShortener.Processor
{
    public class UrlProcessor : IUrlProcessor
    {
        private readonly IUrlRepository urlRepository;
        private const int ShortCodeLength = 6;
        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";


        public UrlProcessor(IUrlRepository urlRepository)
        {
            this.urlRepository = urlRepository;
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

            return $"https://urlshortner-ziaw.onrender.com/s/{shorturl}";
        }

        public async Task<string> RedirectToLongUrl(string shortCode)
        {
            if (string.IsNullOrEmpty(shortCode))
            {
                return string.Empty;
            }

            var originalurl = await this.urlRepository.GetByShortUrlAsync(shortCode);

            return originalurl?.LongUrl;
        }
    }
}