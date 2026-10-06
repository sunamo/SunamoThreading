namespace SunamoThreading;

public class TimeThreadPool
{
    private Timer? timer = null;
    private Dictionary<int, Thread> threads = new Dictionary<int, Thread>();
    private Stack<int> threadIndexStack = new Stack<int>();
    private int remainingCount = 0;
    private string[]? arguments = null;

    // Third parameter cannot be params.
    public TimeThreadPool(ParameterizedThreadStart threadStart, int maxConcurrentThreads, string[] arguments)
    {
        if (arguments.Length < maxConcurrentThreads)
        {
            maxConcurrentThreads = 0;
        }
        remainingCount = arguments.Length;
        this.arguments = arguments;
        for (int i = 0; i < arguments.Length; i++)
        {
            threadIndexStack.Push(i);
            Thread thread = new Thread(threadStart);
            threads.Add(i, thread);
        }
        timer = new Timer(timerElapsed, null, 0, 1000);
    }

    private void timerElapsed(object? state)
    {
        if (remainingCount != 0)
        {
            remainingCount--;
            int threadIndex = threadIndexStack.Pop();
            threads[threadIndex].Start(arguments![threadIndex]);
        }
        else
        {
            disposeTimer();
        }
    }

    public void StopAll()
    {
        disposeTimer();
        foreach (KeyValuePair<int, Thread> item in threads)
        {
            if (isThreadTurnedOn(item))
            {
                item.Value.Interrupt();
            }
        }
    }

    private bool isThreadTurnedOn(KeyValuePair<int, Thread> threadEntry)
    {
        return threadEntry.Value.ThreadState != ThreadState.Stopped && threadEntry.Value.ThreadState != ThreadState.StopRequested && threadEntry.Value.ThreadState != ThreadState.WaitSleepJoin;
    }

    private void disposeTimer()
    {
        if (timer != null)
        {
            timer.Change(Timeout.Infinite, 0);
            timer.Dispose();
            timer = null;
        }
    }
}
