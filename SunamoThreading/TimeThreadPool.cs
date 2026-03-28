namespace SunamoThreading;

/// <summary>
/// Runs threads on a timed interval, starting a new thread each time the timer elapses.
/// </summary>
public class TimeThreadPool
{
    private Timer? timer = null;
    private Dictionary<int, Thread> threads = new Dictionary<int, Thread>();
    private Stack<int> threadIndexStack = new Stack<int>();
    private int remainingCount = 0;
    private string[]? arguments = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeThreadPool"/> class.
    /// Third parameter cannot be params.
    /// </summary>
    /// <param name="threadStart">The method each thread will execute.</param>
    /// <param name="maxConcurrentThreads">The maximum number of threads allowed to run at the same time.</param>
    /// <param name="arguments">The arguments to pass to each thread, one per thread.</param>
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

    /// <summary>
    /// Callback invoked each time the timer elapses to start the next pending thread.
    /// </summary>
    /// <param name="state">Timer callback state (unused).</param>
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

    /// <summary>
    /// Stops all running threads and disposes the timer.
    /// </summary>
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

    /// <summary>
    /// Determines whether the specified thread entry is in a running state.
    /// </summary>
    /// <param name="threadEntry">The thread entry to check.</param>
    /// <returns>True if the thread is actively running, false otherwise.</returns>
    private bool isThreadTurnedOn(KeyValuePair<int, Thread> threadEntry)
    {
        return threadEntry.Value.ThreadState != ThreadState.Stopped && threadEntry.Value.ThreadState != ThreadState.StopRequested && threadEntry.Value.ThreadState != ThreadState.WaitSleepJoin;
    }

    /// <summary>
    /// Disposes the internal timer and releases its resources.
    /// </summary>
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
