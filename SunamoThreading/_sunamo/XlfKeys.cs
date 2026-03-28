namespace SunamoThreading._sunamo;

/// <summary>
/// Contains localization key constants used throughout the threading library.
/// </summary>
internal class XlfKeys
{
    /// <summary>
    /// Key for the message indicating the pool is being disposed.
    /// </summary>
    internal static readonly string ThisPoolInstanceIsInTheProcessOfBeingDisposedCanTAddAnymore = "ThisPoolInstanceIsInTheProcessOfBeingDisposedCanTAddAnymore";

    /// <summary>
    /// Key for the message indicating the pool has already been disposed.
    /// </summary>
    internal static readonly string ThisPoolInstanceHasAlreadyBeenDisposed = "ThisPoolInstanceHasAlreadyBeenDisposed";

    /// <summary>
    /// Key for the worker thread name prefix.
    /// </summary>
    internal static readonly string Worker = "Worker";
}
