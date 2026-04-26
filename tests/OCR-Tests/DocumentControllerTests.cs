using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace OCR_Tests;

public class DocumentControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public DocumentControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UploadDocument_WithNoPdf_ReturnsBadRequest()
    {
        using var content = new MultipartFormDataContent();
        var response = await _client.PostAsync("/api/document/upload", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UploadDocument_WithPdf_ReturnsCreated()
    {
        var pdfBytes = CreateMinimalPdf();
        using var content = new MultipartFormDataContent();
        using var fileContent = new ByteArrayContent(pdfBytes);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        content.Add(fileContent, "file", "test.pdf");

        var response = await _client.PostAsync("/api/document/upload", content);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetDocument_WithUnknownId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/document/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private static byte[] CreateMinimalPdf()
    {
        // Minimal valid PDF
        var pdf = "%PDF-1.4\n1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n"
                + "2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n"
                + "3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] >>\nendobj\n"
                + "xref\n0 4\n0000000000 65535 f \n0000000009 00000 n \n0000000058 00000 n \n"
                + "0000000115 00000 n \ntrailer\n<< /Size 4 /Root 1 0 R >>\nstartxref\n190\n%%EOF";
        return System.Text.Encoding.ASCII.GetBytes(pdf);
    }
}
