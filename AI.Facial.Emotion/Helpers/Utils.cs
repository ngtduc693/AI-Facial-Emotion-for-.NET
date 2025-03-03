using System.Reflection;

namespace AI.Facial.Emotion.Helpers;

public static class Utils
{
    public static byte[] LoadEmbeddedResource(string resourceName)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        using Stream stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new FileNotFoundException($"Embedded resource {resourceName} not found.");
        }

        using MemoryStream memoryStream = new();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
    public static string ExtractEmbeddedResource(string resourceName)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        using Stream stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new Exception($"Not found resource: {resourceName}");
        }

        string tempPath = Path.Combine(Path.GetTempPath(), resourceName);

        using FileStream fileStream = new(tempPath, FileMode.Create, FileAccess.Write);
        stream.CopyTo(fileStream);

        return tempPath;
    }
}