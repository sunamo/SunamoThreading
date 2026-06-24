namespace SunamoThreading.Downloading;

// Third parameter cannot be params.
public class MultiStringDownloader<T>
    where T : IInputDownload
{
    private TimeThreadPool? timeThreadPool = null;
    private Action<T, object>? evaluationMethod = null;
    private Action<T, Exception>? passExceptionMethod = null;

    public MultiStringDownloader(Action<T, object> evaluationMethod, Action<T, Exception> passExceptionMethod, string[] toDownload)
    {
        timeThreadPool = new TimeThreadPool(download, 5, toDownload);
        this.evaluationMethod = evaluationMethod;
        this.passExceptionMethod = passExceptionMethod;
    }

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
