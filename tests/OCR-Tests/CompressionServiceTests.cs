using OCR_Core.Compression;
using Xunit;

namespace OCR_Tests;

public class CompressionServiceTests
{
    private readonly ZstdCompressionService _service = new();

    [Fact]
    public void CompressAndDecompress_ReturnsOriginalData()
    {
        var original = System.Text.Encoding.UTF8.GetBytes("Hello, OCR File Server!");
        var compressed = _service.Compress(original);
        var decompressed = _service.Decompress(compressed);
        Assert.Equal(original, decompressed);
    }

    [Fact]
    public void Compress_ReducesSize_ForRepetitiveData()
    {
        var original = System.Text.Encoding.UTF8.GetBytes(new string('A', 1000));
        var compressed = _service.Compress(original);
        Assert.True(compressed.Length < original.Length);
    }
}
