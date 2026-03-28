namespace SunamoThreading;

/// <summary>
/// Tracks partial completion of multiple thread pool operations and fires an event when all are done.
/// </summary>
/// <param name="expectedCount">The total number of partial completions expected before firing the Done event.</param>
public class ThreadPoolEvent(int expectedCount)
{
    private int finished = 0;

    /// <summary>
    /// Occurs when all expected partial operations have completed.
    /// </summary>
    public event Action? Done;

    /// <summary>
    /// Signals that one partial operation has completed. Fires <see cref="Done"/> when all expected operations finish.
    /// </summary>
    public void PartiallyDone()
    {
        finished++;
        if (finished == expectedCount)
        {
            Done?.Invoke();
        }
    }
}
