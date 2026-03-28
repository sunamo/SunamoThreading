// variables names: ok
namespace SunamoThreading.Tests;

/// <summary>
/// Unit tests for SunamoThreading library.
/// </summary>
public class UnitTest1
{
    /// <summary>
    /// Tests that ThreadPoolEvent fires Done event after the expected number of partial completions.
    /// </summary>
    [Fact]
    public void ThreadPoolEvent_FiresDone_WhenAllPartsComplete()
    {
        bool isDone = false;
        var threadPoolEvent = new ThreadPoolEvent(3);
        threadPoolEvent.Done += () => isDone = true;

        threadPoolEvent.PartiallyDone();
        Assert.False(isDone);

        threadPoolEvent.PartiallyDone();
        Assert.False(isDone);

        threadPoolEvent.PartiallyDone();
        Assert.True(isDone);
    }

    /// <summary>
    /// Tests that MyThreadPool can queue and execute work items.
    /// </summary>
    [Fact]
    public void MyThreadPool_QueuesAndExecutesWorkItems()
    {
        var threadPool = new MyThreadPool();
        threadPool.SetPoolSize(2);

        var resetEvent = new ManualResetEventSlim(false);
        bool wasExecuted = false;

        threadPool.QueueUserWorkItem(_ =>
        {
            wasExecuted = true;
            resetEvent.Set();
        });

        bool isSignaled = resetEvent.Wait(TimeSpan.FromSeconds(5));
        Assert.True(isSignaled);
        Assert.True(wasExecuted);

        threadPool.SetPoolSize(0);
    }

    /// <summary>
    /// Tests that Pool can queue and execute tasks.
    /// </summary>
    [Fact]
    public void Pool_QueuesAndExecutesTasks()
    {
        using var pool = new Pool(2);

        var resetEvent = new ManualResetEventSlim(false);
        bool wasExecuted = false;

        pool.QueueTask(() =>
        {
            wasExecuted = true;
            resetEvent.Set();
        });

        bool isSignaled = resetEvent.Wait(TimeSpan.FromSeconds(5));
        Assert.True(isSignaled);
        Assert.True(wasExecuted);
    }

    /// <summary>
    /// Tests that PoolLinkedList can queue and execute tasks.
    /// </summary>
    [Fact]
    public void PoolLinkedList_QueuesAndExecutesTasks()
    {
        using var pool = new PoolLinkedList(2);

        var resetEvent = new ManualResetEventSlim(false);
        bool wasExecuted = false;

        pool.QueueTask(() =>
        {
            wasExecuted = true;
            resetEvent.Set();
        });

        bool isSignaled = resetEvent.Wait(TimeSpan.FromSeconds(5));
        Assert.True(isSignaled);
        Assert.True(wasExecuted);
    }
}