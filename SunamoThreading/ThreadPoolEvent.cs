namespace SunamoThreading;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public class ThreadPoolEvent(int n)
{
    int finished = 0;
    public event Action Done;

    public void PartiallyDone()
    {
        finished++;
        if (finished == n)
        {
            Done();
        }
    }
}