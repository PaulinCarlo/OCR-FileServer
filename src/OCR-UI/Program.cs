using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;
using OCR_Core.Telemetry;

var builder = WebApplication.CreateBuilder(args);

// --- Configuration ---
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// --- Kestrel ---
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5002);          // HTTP
    options.ListenAnyIP(5003, listenOptions =>
    {
        listenOptions.UseHttps();       // HTTPS
    });
});

// --- Services ---
builder.Services.AddControllersWithViews();

// Response Compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
    options.Level = CompressionLevel.Fastest);
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
    options.Level = CompressionLevel.Fastest);

// OpenTelemetry
builder.Services.AddOcrTelemetry("OCR-UI");

var app = builder.Build();

app.UseResponseCompression();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
