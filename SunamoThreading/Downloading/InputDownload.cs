namespace SunamoThreading.Downloading;

public class InputDownload(string uri, int id) : IInputDownload
{
    public int ID { get; set; } = id;

    public string Uri
    {
        get;
        set;
    } = uri;
}
