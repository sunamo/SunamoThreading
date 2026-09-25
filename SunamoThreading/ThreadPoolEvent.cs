namespace SunamoThreading;

public class ThreadPoolEvent(int expectedCount)
{
    private int finished = 0;

    public event Action? Done;

    public void PartiallyDone()
    {
        finished++;
        if (finished == expectedCount)
        {
            Done?.Invoke();
        }
    }
}
