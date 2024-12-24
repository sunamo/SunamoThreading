namespace SunamoThreading.Downloading;
/// <summary>
/// Obsahuje URI ke stažení - každá URI má jedinečné identifikační číslo
/// </summary>
public class InputDownload(string uri, int id) : IInputDownload
{
    public int ID = id;

    public string Uri
    {
        get;
        set;
    } = uri;
}
