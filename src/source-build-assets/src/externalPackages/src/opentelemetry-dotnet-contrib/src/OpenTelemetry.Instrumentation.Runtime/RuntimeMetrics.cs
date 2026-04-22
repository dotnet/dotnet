// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.Metrics;
using System.Reflection;
using OpenTelemetry.Internal;
#if NET
using JitInfo = System.Runtime.JitInfo;
#endif

namespace OpenTelemetry.Instrumentation.Runtime;

/// <summary>
/// .NET runtime instrumentation.
/// </summary>
internal sealed class RuntimeMetrics
{
    internal static readonly Assembly Assembly = typeof(RuntimeMetrics).Assembly;
    internal static readonly AssemblyName AssemblyName = Assembly.GetName();
    internal static readonly Meter MeterInstance = new(AssemblyName.Name!, Assembly.GetPackageVersion());

#if NET
    private const long NanosecondsPerTick = 100;
#endif
    private const int NumberOfGenerations = 3;

    private static readonly string[] GenNames = ["gen0", "gen1", "gen2", "loh", "poh"];
#if NET
    private static bool isGcInfoAvailable;
#endif

    static RuntimeMetrics()
    {
        MeterInstance.CreateObservableCounter(
            "process.runtime.dotnet.gc.collections.count",
            GetGarbageCollectionCounts,
            description: "Number of garbage collections that have occurred since process start.");

        MeterInstance.CreateObservableUpDownCounter(
            "process.runtime.dotnet.gc.objects.size",
            () => GC.GetTotalMemory(false),
            unit: "bytes",
            description: "Count of bytes currently in use by objects in the GC heap that haven't been collected yet. Fragmentation and other GC committed memory pools are excluded.");

#if NET
        MeterInstance.CreateObservableCounter(
            "process.runtime.dotnet.gc.allocations.size",
            () => GC.GetTotalAllocatedBytes(),
            unit: "bytes",
            description: "Count of bytes allocated on the managed GC heap since the process start. .NET objects are allocated from this heap. Object allocations from unmanaged languages such as C/C++ do not use this heap.");

        MeterInstance.CreateObservableUpDownCounter(
            "process.runtime.dotnet.gc.committed_memory.size",
            () =>
            {
                return !IsGcInfoAvailable ? Array.Empty<Measurement<long>>() : [new(GC.GetGCMemoryInfo().TotalCommittedBytes)];
            },
            unit: "bytes",
            description: "The amount of committed virtual memory for the managed GC heap, as observed during the latest garbage collection. Committed virtual memory may be larger than the heap size because it includes both memory for storing existing objects (the heap size) and some extra memory that is ready to handle newly allocated objects in the future. The value will be unavailable until at least one garbage collection has occurred.");

        MeterInstance.CreateObservableUpDownCounter(
            "process.runtime.dotnet.gc.heap.size",
            () =>
            {
                if (!IsGcInfoAvailable)
                {
                    return [];
                }

                var generationInfo = GC.GetGCMemoryInfo().GenerationInfo;
                var measurements = new Measurement<long>[generationInfo.Length];
                var maxSupportedLength = Math.Min(generationInfo.Length, GenNames.Length);
                for (var i = 0; i < maxSupportedLength; ++i)
                {
                    measurements[i] = new(generationInfo[i].SizeAfterBytes, new KeyValuePair<string, object?>("generation", GenNames[i]));
                }

                return measurements;
            },
            unit: "bytes",
            description: "The heap size (including fragmentation), as observed during the latest garbage collection. The value will be unavailable until at least one garbage collection has occurred.");

        MeterInstance.CreateObservableUpDownCounter(
            "process.runtime.dotnet.gc.heap.fragmentation.size",
            () =>
            {
                if (!IsGcInfoAvailable)
                {
                    return [];
                }

                var generationInfo = GC.GetGCMemoryInfo().GenerationInfo;
                var measurements = new Measurement<long>[generationInfo.Length];
                var maxSupportedLength = Math.Min(generationInfo.Length, GenNames.Length);
                for (var i = 0; i < maxSupportedLength; ++i)
                {
                    measurements[i] = new(generationInfo[i].FragmentationAfterBytes, new KeyValuePair<string, object?>("generation", GenNames[i]));
                }

                return measurements;
            },
            unit: "bytes",
            description: "The heap fragmentation, as observed during the latest garbage collection. The value will be unavailable until at least one garbage collection has occurred.");

        var mi = typeof(GC).GetMethod("GetTotalPauseDuration", BindingFlags.Public | BindingFlags.Static);
        var getTotalPauseDuration = mi?.CreateDelegate<Func<TimeSpan>>();
        if (getTotalPauseDuration != null)
        {
            MeterInstance.CreateObservableCounter(
                "process.runtime.dotnet.gc.duration",
                () => getTotalPauseDuration().Ticks * NanosecondsPerTick,
                unit: "ns",
                description: "The total amount of time paused in GC since the process start.");
        }

        MeterInstance.CreateObservableCounter(
            "process.runtime.dotnet.jit.il_compiled.size",
            () => JitInfo.GetCompiledILBytes(),
            unit: "bytes",
            description: "Count of bytes of intermediate language that have been compiled since the process start.");

        MeterInstance.CreateObservableCounter(
            "process.runtime.dotnet.jit.methods_compiled.count",
            () => JitInfo.GetCompiledMethodCount(),
            description: "The number of times the JIT compiler compiled a method since the process start. The JIT compiler may be invoked multiple times for the same method to compile with different generic parameters, or because tiered compilation requested different optimization settings.");

        MeterInstance.CreateObservableCounter(
            "process.runtime.dotnet.jit.compilation_time",
            () => JitInfo.GetCompilationTime().Ticks * NanosecondsPerTick,
            unit: "ns",
            description: "The amount of time the JIT compiler has spent compiling methods since the process start.");

        MeterInstance.CreateObservableCounter(
            "process.runtime.dotnet.monitor.lock_contention.count",
            () => Monitor.LockContentionCount,
            description: "The number of times there was contention when trying to acquire a monitor lock since the process start. Monitor locks are commonly acquired by using the lock keyword in C#, or by calling Monitor.Enter() and Monitor.TryEnter().");

        MeterInstance.CreateObservableUpDownCounter(
            "process.runtime.dotnet.thread_pool.threads.count",
            () => (long)ThreadPool.ThreadCount,
            description: "The number of thread pool threads that currently exist.");

        MeterInstance.CreateObservableCounter(
            "process.runtime.dotnet.thread_pool.completed_items.count",
            () => ThreadPool.CompletedWorkItemCount,
            description: "The number of work items that have been processed by the thread pool since the process start.");

        MeterInstance.CreateObservableUpDownCounter(
            "process.runtime.dotnet.thread_pool.queue.length",
            () => ThreadPool.PendingWorkItemCount,
            description: "The number of work items that are currently queued to be processed by the thread pool.");

        MeterInstance.CreateObservableUpDownCounter(
            "process.runtime.dotnet.timer.count",
            () => Timer.ActiveCount,
            description: "The number of timer instances that are currently active. Timers can be created by many sources such as System.Threading.Timer, Task.Delay, or the timeout in a CancellationSource. An active timer is registered to tick at some point in the future and has not yet been canceled.");
#endif

        MeterInstance.CreateObservableUpDownCounter(
            "process.runtime.dotnet.assemblies.count",
            () => (long)AppDomain.CurrentDomain.GetAssemblies().Length,
            description: "The number of .NET assemblies that are currently loaded.");

        var exceptionCounter = MeterInstance.CreateCounter<long>(
            "process.runtime.dotnet.exceptions.count",
            description: "Count of exceptions that have been thrown in managed code, since the observation started. The value will be unavailable until an exception has been thrown after OpenTelemetry.Instrumentation.Runtime initialization.");

        AppDomain.CurrentDomain.FirstChanceException += (source, e) =>
        {
            exceptionCounter.Add(1);
        };
    }
#pragma warning disable SA1313
    /// <summary>
    /// Initializes a new instance of the <see cref="RuntimeMetrics"/> class.
    /// </summary>
    /// <param name="_1">The options to define the metrics.</param>
    public RuntimeMetrics(RuntimeInstrumentationOptions _1)
#pragma warning restore SA1313
    {
    }

#if NET
    private static bool IsGcInfoAvailable
    {
        get
        {
            if (isGcInfoAvailable)
            {
                return true;
            }

            if (GC.CollectionCount(0) > 0)
            {
                isGcInfoAvailable = true;
            }

            return isGcInfoAvailable;
        }
    }
#endif

    private static IEnumerable<Measurement<long>> GetGarbageCollectionCounts()
    {
        long collectionsFromHigherGeneration = 0;

        for (var gen = NumberOfGenerations - 1; gen >= 0; --gen)
        {
            long collectionsFromThisGeneration = GC.CollectionCount(gen);

            yield return new(collectionsFromThisGeneration - collectionsFromHigherGeneration, new KeyValuePair<string, object?>("generation", GenNames[gen]));

            collectionsFromHigherGeneration = collectionsFromThisGeneration;
        }
    }
}
