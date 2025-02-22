using System.Reflection;

namespace AI.Facial.Emotion.Helpers;

public static class Utils
{
    public static byte[] LoadEmbeddedResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
            throw new FileNotFoundException($"Embedded resource {resourceName} not found.");

        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
    public static string ExtractEmbeddedResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
            throw new Exception($"Not found resource: {resourceName}");

        string tempPath = Path.Combine(Path.GetTempPath(), resourceName);

        using var fileStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write);
        stream.CopyTo(fileStream);

        return tempPath;
    }
}