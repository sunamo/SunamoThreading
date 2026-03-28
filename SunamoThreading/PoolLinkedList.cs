namespace SunamoThreading;

/// <summary>
/// A thread pool implementation using a linked-list-based worker queue.
/// Workers process actions in FIFO order and are joined on disposal.
/// </summary>
public sealed class PoolLinkedList : IDisposable
{
    private readonly LinkedList<Thread> workers;
    private readonly LinkedList<Action> tasks = new LinkedList<Action>();
    private bool disallowAdd;
    private bool disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="PoolLinkedList"/> class with the specified number of worker threads.
    /// </summary>
    /// <param name="size">The number of worker threads to create.</param>
    public PoolLinkedList(int size)
    {
        workers = new LinkedList<Thread>();
        for (var i = 0; i < size; ++i)
        {
            var workerThread = new Thread(worker) { Name = string.Concat(Translate.FromKey(XlfKeys.Worker) + " ", i) };
            workerThread.Start();
            workers.AddLast(workerThread);
        }
    }

    /// <summary>
    /// Disposes the pool by waiting for all pending tasks to complete and joining all worker threads.
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
    /// Adds a new task to the queue for processing by a worker thread.
    /// </summary>
    /// <param name="task">The action to be queued for execution.</param>
    public void QueueTask(Action task)
    {
        lock (tasks)
        {
            if (disallowAdd) { throw new Exception(Translate.FromKey(XlfKeys.ThisPoolInstanceIsInTheProcessOfBeingDisposedCanTAddAnymore)); }
            if (disposed) { throw new Exception(Translate.FromKey(XlfKeys.ThisPoolInstanceHasAlreadyBeenDisposed)); }
            tasks.AddLast(task);
            Monitor.PulseAll(tasks);
        }
    }

    /// <summary>
    /// Worker loop that continuously dequeues and executes tasks.
    /// </summary>
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
                    if (null != workers.First && ReferenceEquals(Thread.CurrentThread, workers.First.Value) && tasks.Count > 0)
                    {
                        task = tasks.First!.Value;
                        tasks.RemoveFirst();
                        workers.RemoveFirst();
                        Monitor.PulseAll(tasks);
                        break;
                    }
                    Monitor.Wait(tasks);
                }
            }
            task();
            lock (tasks)
            {
                workers.AddLast(Thread.CurrentThread);
            }
            task = null;
        }
    }
}
