# NuGet Alternatives to SunamoThreading

This document lists popular NuGet packages that provide similar functionality to SunamoThreading.

## Overview

Advanced threading utilities

## Alternative Packages

### System.Threading.Tasks.Dataflow
- **NuGet**: System.Threading.Tasks.Dataflow
- **Purpose**: Dataflow library
- **Key Features**: Actor model, pipelines, parallel processing

### Nito.AsyncEx
- **NuGet**: Nito.AsyncEx
- **Purpose**: Async primitives
- **Key Features**: Async-compatible synchronization

### System.Threading.Channels
- **NuGet**: System.Threading.Channels
- **Purpose**: High-performance channels
- **Key Features**: Producer-consumer patterns

### Parallel Extensions
- **NuGet**: System.Threading.Tasks.Parallel
- **Purpose**: Parallel operations
- **Key Features**: Parallel.For, Parallel.ForEach, PLINQ

## Comparison Notes

Use async/await + Channels for most scenarios. Dataflow for complex pipelines.

## Choosing an Alternative

Consider these alternatives based on your specific needs:
- **System.Threading.Tasks.Dataflow**: Dataflow library
- **Nito.AsyncEx**: Async primitives
- **System.Threading.Channels**: High-performance channels
