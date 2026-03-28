# SunamoThreading

Various types of thread pools and multithreaded utilities for .NET.

## Overview

SunamoThreading is part of the Sunamo package ecosystem, providing modular, platform-independent thread pool implementations for .NET development.

## Main Components

### Thread Pool Implementations

- **MyThreadPool** - A simple thread pool with dynamic pool size adjustment. Implements `IThreadPool`.
- **Pool** - A list-based thread pool where workers process actions in FIFO order. Implements `IDisposable`.
- **PoolLinkedList** - A linked-list-based thread pool variant. Implements `IDisposable`.
- **TimeThreadPool** - Starts threads on a timed interval using an internal timer.

### Events

- **ThreadPoolEvent** - Tracks partial completion of multiple operations and fires a `Done` event when all are complete.

### Downloading

- **MultiStringDownloader\<T\>** - Downloads multiple strings concurrently using a timed thread pool.
- **InputDownload** - Represents a download input with a URI and unique ID.

### Interfaces

- **IThreadPool** - Contract for thread pools that can queue work items and resize dynamically.
- **IInputDownload** - Represents a download input providing both a URI and an ID.
- **IUri** - Provides access to a URI string property.

## Installation

```bash
dotnet add package SunamoThreading
```

## Target Frameworks

- .NET 10.0
- .NET 9.0
- .NET 8.0

## Dependencies

- **Microsoft.Extensions.Logging.Abstractions**

## License

MIT
