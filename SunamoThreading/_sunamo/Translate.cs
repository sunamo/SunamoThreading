namespace SunamoThreading._sunamo;

/// <summary>
/// Provides translation lookup for localization keys.
/// </summary>
internal class Translate
{
    /// <summary>
    /// Returns the translated string for the specified key.
    /// </summary>
    /// <param name="key">The localization key to look up.</param>
    /// <returns>The translated string.</returns>
    internal static string? FromKey(object key)
    {
        return key?.ToString();
    }
}
