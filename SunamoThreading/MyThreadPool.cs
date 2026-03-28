namespace SunamoThreading;

/// <summary>
/// Implements a simple thread pool that allows dynamic change of the number of working threads.
/// Pool size is not fixed and can have more elements than the configured pool size.
/// </summary>
public class MyThreadPool : IThreadPool
{
    /// <summary>
    /// Maximum size of the pool.
    /// </summary>
    private int poolSize;
    /// <summary>
    /// Threads currently running in the pool.
    /// </summary>
    private List<Thread> threads = new List<Thread>();
    /// <summary>
    /// Queue of pending jobs to be processed.
    /// </summary>
    private Queue<WaitCallback> jobs = new Queue<WaitCallback>();

    /// <summary>
    /// Adds a work item to the job queue and signals waiting threads.
    /// </summary>
    /// <param name="callBack">The callback method to be queued for execution.</param>
    /// <returns>True if the work item was successfully queued.</returns>
    public bool QueueUserWorkItem(WaitCallback callBack)
    {
        if (callBack == null)
            throw new Exception("  callback method cannot be null");
        lock (jobs)
        {
            jobs.Enqueue(callBack);
            Monitor.Pulse(jobs);
        }
        return true;
    }

    /// <summary>
    /// Sets the pool size. After reducing the number of working threads,
    /// currently working threads are allowed to finish their jobs (they are not interrupted).
    /// </summary>
    /// <param name="size">The desired number of threads in the pool.</param>
    /// <returns>True if the pool size was successfully updated.</returns>
    public bool SetPoolSize(int size)
    {
        lock (threads)
        {
            poolSize = size;
            if (poolSize > threads.Count)
                spawnThreads();
            else if (poolSize < threads.Count)
            {
                lock (jobs) Monitor.PulseAll(jobs);
            }
        }
        return true;
    }

    /// <summary>
    /// Spawns new threads up to the configured pool size.
    /// </summary>
    private void spawnThreads()
    {
        while (threads.Count < poolSize)
        {
            Thread thread = new Thread(consumeJobs);
            threads.Add(thread);
            thread.Start();
        }
    }

    /// <summary>
    /// Runner method for worker threads that continuously dequeues and executes jobs.
    /// </summary>
    private void consumeJobs()
    {
        WaitCallback job;
        while (true)
        {
            if (killThreadIfNeeded()) return;
            lock (jobs)
            {
                while (jobs.Count == 0 && !(poolSize < threads.Count))
                    Monitor.Wait(jobs);
                if (killThreadIfNeeded()) return;
                job = jobs.Dequeue();
            }
            job(null);
        }
    }

    /// <summary>
    /// Checks if there are more running threads than the pool size and removes the current thread if needed.
    /// </summary>
    /// <returns>True if the invoking thread should terminate, false otherwise.</returns>
    private bool killThreadIfNeeded()
    {
        if (poolSize < threads.Count)
        {
            lock (threads)
            {
                if (poolSize < threads.Count)
                {
                    threads.Remove(Thread.CurrentThread);
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Gets the most recently set size of the pool.
    /// </summary>
    public int PoolSize { get { return poolSize; } }

    /// <summary>
    /// Gets the actual number of threads in the pool. It might not equal PoolSize when
    /// the number of threads is stabilizing after a pool size change.
    /// </summary>
    public int ActualPoolSize { get { return threads.Count; } }
}
