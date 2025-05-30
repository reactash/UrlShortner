using Microsoft.AspNetCore.Mvc;
using UrlShortener.API.Processor.Inteface;
using UrlShortener.Models.Url;

namespace UrlShortener.API.Controller
{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
            return Ok("API is working");
        }

        [HttpPost("shorten")]
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
            catch (Exception)
            {
                return StatusCode(500, new { error = "Interval Server Error" });
            }
        }

        [HttpGet("s/{shortCode}")]
        public async Task<IActionResult> RedirectToLongUrl(string shortCode)
        {
            var originalUrl = await urlProcessor.RedirectToLongUrl(shortCode);
            if (string.IsNullOrEmpty(originalUrl))
                return NotFound();

            return Redirect(originalUrl);
        }

    }

}