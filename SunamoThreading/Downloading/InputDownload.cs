namespace SunamoThreading.Downloading;

/// <summary>
/// Contains a URI to download. Each URI has a unique identification number.
/// </summary>
/// <param name="uri">The URI to download.</param>
/// <param name="id">The unique identification number for this download.</param>
public class InputDownload(string uri, int id) : IInputDownload
{
    /// <summary>
    /// Gets or sets the unique identification number for this download.
    /// </summary>
    public int ID { get; set; } = id;

    /// <summary>
    /// Gets or sets the URI to download.
    /// </summary>
    public string Uri
    {
        get;
        set;
    } = uri;
}
