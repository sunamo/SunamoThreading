namespace SunamoThreading.Downloading;

/// <summary>
/// Downloads multiple strings concurrently using a timed thread pool.
/// </summary>
/// <typeparam name="T">The input type, should implement <see cref="IInputDownload"/>.</typeparam>
public class MultiStringDownloader<T>
    where T : IInputDownload
{
    private TimeThreadPool? timeThreadPool = null;
    private Action<T, object>? evaluationMethod = null;
    private Action<T, Exception>? passExceptionMethod = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiStringDownloader{T}"/> class.
    /// Third parameter cannot be params.
    /// </summary>
    /// <param name="evaluationMethod">The method to call with the downloaded content.</param>
    /// <param name="passExceptionMethod">The method to call when a download fails.</param>
    /// <param name="toDownload">The URIs to download.</param>
    public MultiStringDownloader(Action<T, object> evaluationMethod, Action<T, Exception> passExceptionMethod, string[] toDownload)
    {
        timeThreadPool = new TimeThreadPool(download, 5, toDownload);
        this.evaluationMethod = evaluationMethod;
        this.passExceptionMethod = passExceptionMethod;
    }

    /// <summary>
    /// Downloads content from the URI specified in the input object.
    /// </summary>
    /// <param name="state">The input object containing the URI to download.</param>
    private void download(object? state)
    {
        T input = (T)state!;
#pragma warning disable SYSLIB0014
        WebClient webClient = new WebClient();
#pragma warning restore SYSLIB0014
        try
        {
            evaluationMethod!.Invoke(input, webClient.DownloadString(input.Uri));
        }
        catch (Exception exception)
        {
            passExceptionMethod!.Invoke(input, exception);
        }
    }
}
