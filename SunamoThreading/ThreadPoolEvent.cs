namespace SunamoThreading;
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
