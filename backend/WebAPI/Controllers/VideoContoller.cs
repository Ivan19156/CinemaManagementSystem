using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Application.Interfaces.BlobInterface;
using Infrastructure.Logging;
using CinemaManagementSystem.Infrastructure.Logging;
[ApiController]
[Route("api/[controller]")]
public class VideoController : ControllerBase
{
    private readonly IBlobStorageService _blobStorageService;
    
private readonly IAppLogger<VideoController> _logger;

public VideoController(
    IBlobStorageService blobStorageService,
    IAppLogger<VideoController> logger)
{
    _blobStorageService = blobStorageService;
    _logger = logger;
}

    [HttpPost("upload")]
    public async Task<IActionResult> UploadVideo(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        using var stream = file.OpenReadStream();
        var blobName = file.FileName; // Можна генерувати унікальне ім'я

        await _blobStorageService.UploadFileAsync(blobName, stream);

        var url = _blobStorageService.GetBlobUrl(blobName);

        return Ok(new { Url = url });
    }

    [HttpGet("trailers/{filmId}")]
public IActionResult GetTrailerUrl(int filmId)
{
    try
    {
        string blobName = $"film_{filmId}.mp4";
        string url = _blobStorageService.GetBlobUrl(blobName);
        
        _logger.LogInfo("Requested trailer for film {FilmId}, blob: {BlobName}, URL: {Url}", filmId, blobName, url);

        return Ok(new { url });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error fetching trailer for film {FilmId}", filmId);
        return StatusCode(500, "Internal server error. See logs for details.");
    }
}

    

}


