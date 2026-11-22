namespace SunamoThreading;

/// <summary>
/// Represents the pool type.
/// </summary>
public sealed class Pool : IDisposable
{
    private readonly List<Thread> workers;
    private readonly LinkedList<Action> tasks = new LinkedList<Action>();
    private bool disallowAdd;
    private bool disposed;

    /// <summary>
    /// Initializes a new instance of the pool class.
    /// </summary>
    /// <param name="size">The size.</param>
    public Pool(int size)
    {
        workers = new List<Thread>();
        for (var i = 0; i < size; ++i)
        {
            var workerThread = new Thread(worker) { Name = string.Concat("Worker" + " ", i) };
            workerThread.Start();
            workers.Add(workerThread);
        }
    }

    /// <summary>
    /// Dispose.
    /// </summary>
    public void Dispose()
    {
        var isWaitingForThreads = false;
        lock (tasks)
        {
            if (!disposed)
            {
                GC.SuppressFinalize(this);
                disallowAdd = true;
                while (tasks.Count > 0)
                {
                    Monitor.Wait(tasks);
                }
                disposed = true;
                Monitor.PulseAll(tasks);
                isWaitingForThreads = true;
            }
        }
        if (isWaitingForThreads)
        {
            foreach (var workerThread in workers)
            {
                workerThread.Join();
            }
        }
    }

    /// <summary>
    /// Queue task.
    /// </summary>
    /// <param name="task">The task.</param>
    public void QueueTask(Action task)
    {
        lock (tasks)
        {
            if (disallowAdd) { throw new Exception("This pool instance is in the process of being disposed can t add anymore"); }
            if (disposed) { throw new Exception("This pool instance has already been disposed"); }
            tasks.AddLast(task);
            Monitor.PulseAll(tasks);
        }
    }

    private void worker()
    {
        Action? task = null;
        while (true)
        {
            lock (tasks)
            {
                while (true)
                {
                    if (disposed)
                    {
                        return;
                    }
                    if (workers.Count > 0 && null != workers[0] && ReferenceEquals(Thread.CurrentThread, workers[0]) && tasks.Count > 0)
                    {
                        task = tasks.First!.Value;
                        tasks.RemoveFirst();
                        workers.RemoveAt(0);
                        Monitor.PulseAll(tasks);
                        break;
                    }
                    Monitor.Wait(tasks);
                }
            }
            task();
            lock (tasks)
            {
                workers.Add(Thread.CurrentThread);
            }
            task = null;
        }
    }
}
