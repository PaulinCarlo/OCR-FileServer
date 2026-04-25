using Microsoft.AspNetCore.Mvc;

namespace OCR_UI.Controllers;

[Controller]
public class HtmlController : Controller
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<HtmlController> _logger;
    private static readonly string[] AllowedPages = { "index", "upload", "documents" };

    public HtmlController(IWebHostEnvironment env, ILogger<HtmlController> logger)
    {
        _env = env;
        _logger = logger;
    }

    [Route("/")]
    [Route("/home")]
    public IActionResult Index()
    {
        return ServeHtml("Index.html");
    }

    [Route("/{page}")]
    public IActionResult Page(string page)
    {
        // Validate page name to prevent path injection
        if (string.IsNullOrEmpty(page) || !IsValidPageName(page))
        {
            _logger.LogWarning("Invalid page requested");
            return NotFound();
        }

        var fileName = $"{page}.html";
        var filePath = Path.Combine(_env.ContentRootPath, "Html", fileName);

        if (!System.IO.File.Exists(filePath))
        {
            _logger.LogWarning("HTML page not found");
            return NotFound();
        }

        return ServeHtml(fileName);
    }

    private IActionResult ServeHtml(string fileName)
    {
        var filePath = Path.Combine(_env.ContentRootPath, "Html", fileName);

        if (!System.IO.File.Exists(filePath))
            return NotFound();

        var content = System.IO.File.ReadAllText(filePath);
        return Content(content, "text/html");
    }

    private static bool IsValidPageName(string page)
    {
        return !string.IsNullOrEmpty(page) &&
               !page.Contains("..") &&
               !page.Contains("/") &&
               !page.Contains("\\") &&
               AllowedPages.Any(p => p.Equals(page, StringComparison.OrdinalIgnoreCase));
    }
}

