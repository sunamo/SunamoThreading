namespace SunamoThreading;

public sealed class Pool : IDisposable
{
    private readonly List<Thread> workers;
    private readonly LinkedList<Action> tasks = new LinkedList<Action>();
    private bool disallowAdd;
    private bool disposed;

    public Pool(int size)
    {
        workers = new List<Thread>();
        for (var i = 0; i < size; ++i)
        {
            var workerThread = new Thread(worker) { Name = string.Concat(Translate.FromKey(XlfKeys.Worker) + " ", i) };
            workerThread.Start();
            workers.Add(workerThread);
        }
    }

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
