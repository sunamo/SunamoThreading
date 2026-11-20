namespace SunamoThreading;

/// <summary>
/// Thread pool event.
/// </summary>
/// <param name="expectedCount">The expected count.</param>
/// <returns>The thread pool event result.</returns>
public class ThreadPoolEvent(int expectedCount)
{
    private int finished = 0;

    /// <summary>
    /// The done.
    /// </summary>
    public event Action? Done;

    /// <summary>
    /// Partially done.
    /// </summary>
    /// <returns>The partially done result.</returns>
    public void PartiallyDone()
    {
        finished++;
        if (finished == expectedCount)
        {
            Done?.Invoke();
        }
    }
}
