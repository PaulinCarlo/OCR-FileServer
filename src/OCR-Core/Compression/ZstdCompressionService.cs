using ZstdSharp;

namespace OCR_Core.Compression;

public class ZstdCompressionService
{
    private readonly int _compressionLevel;

    public ZstdCompressionService(int compressionLevel = 3)
    {
        _compressionLevel = compressionLevel;
    }

    public byte[] Compress(byte[] data)
    {
        using var compressor = new Compressor(_compressionLevel);
        return compressor.Wrap(data).ToArray();
    }

    public byte[] Decompress(byte[] compressedData)
    {
        using var decompressor = new Decompressor();
        return decompressor.Unwrap(compressedData).ToArray();
    }
}
