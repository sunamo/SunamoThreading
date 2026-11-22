namespace SunamoThreading.Interfaces;

/// <summary>
/// Represents the i thread pool type.
/// </summary>
public interface IThreadPool
{
    /// <summary>
    /// Queue user work item.
    /// </summary>
    /// <param name="callBack">The call back.</param>
    /// <returns>The queue user work item result.</returns>
    bool QueueUserWorkItem(WaitCallback callBack);

    /// <summary>
    /// Set pool size.
    /// </summary>
    /// <param name="size">The size.</param>
    /// <returns>The set pool size result.</returns>
    bool SetPoolSize(int size);

    /// <summary>
    /// Gets the pool size.
    /// </summary>
    int PoolSize { get; }
}
