namespace SunamoThreading;

public class MyThreadPool : IThreadPool
{
    private int poolSize;
    private List<Thread> threads = new List<Thread>();
    private Queue<WaitCallback> jobs = new Queue<WaitCallback>();

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

    private void spawnThreads()
    {
        while (threads.Count < poolSize)
        {
            Thread thread = new Thread(consumeJobs);
            threads.Add(thread);
            thread.Start();
        }
    }

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

    public int PoolSize { get { return poolSize; } }

    public int ActualPoolSize { get { return threads.Count; } }
}
