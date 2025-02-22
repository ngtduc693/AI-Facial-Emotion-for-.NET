using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace WinForms;

public static class ImageProcessor
{
    public static async Task<byte[]> LoadImageFromUrlAsync(string imageUrl)
    {
        using var httpClient = new HttpClient();
        return await httpClient.GetByteArrayAsync(imageUrl);
    }

    public static byte[] LoadImageFromBase64(string base64Image)
    {
        return Convert.FromBase64String(base64Image);
    }

    public static async Task<byte[]> LoadImageFromStreamAsync(Stream fileStream)
    {
        using var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}