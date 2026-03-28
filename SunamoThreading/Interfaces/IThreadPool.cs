namespace SunamoThreading.Interfaces;

/// <summary>
/// Defines the contract for a thread pool that can queue work items and resize dynamically.
/// </summary>
public interface IThreadPool
{
    /// <summary>
    /// Queues a work item for execution by a thread in the pool.
    /// </summary>
    /// <param name="callBack">The callback method to be queued for execution.</param>
    /// <returns>True if the work item was successfully queued.</returns>
    bool QueueUserWorkItem(WaitCallback callBack);

    /// <summary>
    /// Sets the desired number of threads in the pool.
    /// </summary>
    /// <param name="size">The desired number of threads.</param>
    /// <returns>True if the pool size was successfully updated.</returns>
    bool SetPoolSize(int size);

    /// <summary>
    /// Gets the configured pool size.
    /// </summary>
    int PoolSize { get; }
}
