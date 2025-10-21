// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
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
