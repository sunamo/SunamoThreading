namespace SunamoThreading.Downloading;

/// <summary>
/// Input download.
/// </summary>
/// <param name="uri">The uri.</param>
/// <param name="id">The id.</param>
/// <returns>The input download result.</returns>
public class InputDownload(string uri, int id) : IInputDownload
{
    /// <summary>
    /// Gets or sets the ID.
    /// </summary>
    public int ID { get; set; } = id;

    /// <summary>
    /// The uri.
    /// </summary>
    public string Uri
    {
        get;
        set;
    } = uri;
}
