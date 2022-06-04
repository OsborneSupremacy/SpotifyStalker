
namespace Spotify.Utility;

public static class StringFunctions
{
    public static string Left(this string? input, int length)
    {
        if (input == null) return string.Empty;
        if (input.Length > length) return input.Substring(0, length);
        return input;
    }
}
