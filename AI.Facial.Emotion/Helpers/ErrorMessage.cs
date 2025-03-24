namespace AI.Facial.Emotion.Helpers;

public static class ErrorMessage
{
    public const string IMG_COULD_LOAD = "Image could not be loaded, possibly due to permissions or image error";
    public const string IMG_INVALID_RATIO = "Image must be square (1:1 ratio). Current size";
    public const string IMG_NO_FACE = "No faces detected";
    public const string IMG_UNSUPPORTED = "Unsupported image format";
}
