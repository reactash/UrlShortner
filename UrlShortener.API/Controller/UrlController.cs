using Microsoft.AspNetCore.Mvc;
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
        return Ok("API is working");
    }

    [HttpPost("/shorten")] // optional: make this public-facing too
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
            return StatusCode(500, new { error = "Internal Server Error" });
        }
    }

    [HttpGet("/s/{shortCode}")] // fix: override route to match public URL
    public async Task<IActionResult> RedirectToLongUrl(string shortCode)
    {
        var originalUrl = await urlProcessor.RedirectToLongUrl(shortCode);
        if (string.IsNullOrEmpty(originalUrl))
            return NotFound();

        return Redirect(originalUrl);
    }
}

}