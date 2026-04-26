using Microsoft.AspNetCore.Mvc;
using OCR_Core.Models;

namespace OCR_Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentController : ControllerBase
{
    private readonly ILogger<DocumentController> _logger;

    public DocumentController(ILogger<DocumentController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Accepts scanned PDF documents from printers via multipart/form-data.
    /// </summary>
    [HttpPost("upload")]
    [RequestSizeLimit(50 * 1024 * 1024)] // 50 MB
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ScannedDocument), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadDocument(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file was provided.");

        if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase)
            && !file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Only PDF files are accepted.");
        }

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var document = new ScannedDocument
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            Content = ms.ToArray(),
            UploadedAt = DateTime.UtcNow
        };

        _logger.LogInformation("Received scanned document: {FileName}, Size: {Size} bytes",
            document.FileName, document.Content.Length);

        // TODO: persist to DB and enqueue for OCR processing
        return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, document);
    }

    /// <summary>
    /// Gets a previously uploaded document by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ScannedDocument), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetDocument(int id)
    {
        // TODO: retrieve from DB
        return NotFound();
    }
}
