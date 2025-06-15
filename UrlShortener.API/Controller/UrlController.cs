using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UrlShortener.API.Models;
using UrlShortener.API.Processor.Inteface;
using UrlShortener.Models.Url;

namespace UrlShortener.API.Controller
{
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly IUrlProcessor urlProcessor;

        public UrlController(IUrlProcessor urlProcessor)
        {
            this.urlProcessor = urlProcessor;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            var ip =  HttpContext.Connection.RemoteIpAddress?.ToString();
            var forwardFor = Request.Headers["X-Forward-For"].FirstOrDefault();
            ip = !string.IsNullOrEmpty(forwardFor) ? forwardFor : ip;

            var userAgent = Request.Headers["User-Agent"].ToString();
            var browserInfo = UserAgentParser.ParserUserAgent(userAgent); 

            return Ok($"API is working on ip - {ip}: {browserInfo.Browser}:{browserInfo.Platform}");
        }
        [HttpGet("test-mongo")]
        public IActionResult TestMongo([FromServices] IMongoClient mongoClient, [FromServices] IOptions<MongoDbSettings> settings)
        {
            try
            {
                var db = mongoClient.GetDatabase(settings.Value.DatabaseName);
                var collections = db.ListCollectionNames().ToList();
                return Ok(new { status = "Connected", collections });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("shorten")] // optional: make this public-facing too
        public async Task<IActionResult> ShortenUrlAsync([FromBody] ShortenUrlRequest shortenUrlRequest)
        {
            try
            {
                var result = await this.urlProcessor.ShortenUrlAsync(shortenUrlRequest);
                return this.Ok(result);
            }
            catch (ArgumentException arg)
            {
                return BadRequest(new { error = arg.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled exception: {ex.Message}");
                return StatusCode(500, new { error = "Internal Server Error" });
            }

        }

        [HttpGet("s/{shortCode}")] // fix: override route to match public URL
        public async Task<IActionResult> RedirectToLongUrl(string shortCode)
        {
             var ip =  HttpContext.Connection.RemoteIpAddress?.ToString();
            /*  var forwardFor = Request.Headers["X-Forward-For"].FirstOrDefault();
              ip = !string.IsNullOrEmpty(forwardFor) ? forwardFor : ip;

              */

            var userAgent = Request.Headers["User-Agent"].ToString();
  
            var originalUrl = await urlProcessor.RedirectToLongUrl(shortCode,ip,userAgent);
            if (string.IsNullOrEmpty(originalUrl))
                return NotFound();

            return Redirect(originalUrl);
        }
    }

}