// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyTitle("System.Threading.Tasks.Parallel")]
[assembly: System.Reflection.AssemblyDescription("System.Threading.Tasks.Parallel")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Threading.Tasks.Parallel")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Threading.Tasks
{
    public static partial class Parallel
    {
        public static ParallelLoopResult For(int fromInclusive, int toExclusive, Action<int, ParallelLoopState> body) { throw null; }

        public static ParallelLoopResult For(int fromInclusive, int toExclusive, Action<int> body) { throw null; }

        public static ParallelLoopResult For(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Action<int, ParallelLoopState> body) { throw null; }

        public static ParallelLoopResult For(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Action<int> body) { throw null; }

        public static ParallelLoopResult For(long fromInclusive, long toExclusive, Action<long, ParallelLoopState> body) { throw null; }

        public static ParallelLoopResult For(long fromInclusive, long toExclusive, Action<long> body) { throw null; }

        public static ParallelLoopResult For(long fromInclusive, long toExclusive, ParallelOptions parallelOptions, Action<long, ParallelLoopState> body) { throw null; }

        public static ParallelLoopResult For(long fromInclusive, long toExclusive, ParallelOptions parallelOptions, Action<long> body) { throw null; }

        public static ParallelLoopResult For<TLocal>(int fromInclusive, int toExclusive, Func<TLocal> localInit, Func<int, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally) { throw null; }

        public static ParallelLoopResult For<TLocal>(int fromInclusive, int toExclusive, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<int, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally) { throw null; }

        public static ParallelLoopResult For<TLocal>(long fromInclusive, long toExclusive, Func<TLocal> localInit, Func<long, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally) { throw null; }

        public static ParallelLoopResult For<TLocal>(long fromInclusive, long toExclusive, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<long, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally) { throw null; }

        public static ParallelLoopResult ForEach<TSource>(Collections.Concurrent.OrderablePartitioner<TSource> source, Action<TSource, ParallelLoopState, long> body) { throw null; }

        public static ParallelLoopResult ForEach<TSource>(Collections.Concurrent.OrderablePartitioner<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState, long> body) { throw null; }

        public static ParallelLoopResult ForEach<TSource>(Collections.Concurrent.Partitioner<TSource> source, Action<TSource, ParallelLoopState> body) { throw null; }

        public static ParallelLoopResult ForEach<TSource>(Collections.Concurrent.Partitioner<TSource> source, Action<TSource> body) { throw null; }

        public static ParallelLoopResult ForEach<TSource>(Collections.Concurrent.Partitioner<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState> body) { throw null; }

        public static ParallelLoopResult ForEach<TSource>(Collections.Concurrent.Partitioner<TSource> source, ParallelOptions parallelOptions, Action<TSource> body) { throw null; }

        public static ParallelLoopResult ForEach<TSource>(Collections.Generic.IEnumerable<TSource> source, Action<TSource, ParallelLoopState, long> body) { throw null; }

        public static ParallelLoopResult ForEach<TSource>(Collections.Generic.IEnumerable<TSource> source, Action<TSource, ParallelLoopState> body) { throw null; }

        public static ParallelLoopResult ForEach<TSource>(Collections.Generic.IEnumerable<TSource> source, Action<TSource> body) { throw null; }

        public static ParallelLoopResult ForEach<TSource>(Collections.Generic.IEnumerable<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState, long> body) { throw null; }

        public static ParallelLoopResult ForEach<TSource>(Collections.Generic.IEnumerable<TSource> source, ParallelOptions parallelOptions, Action<TSource, ParallelLoopState> body) { throw null; }

        public static ParallelLoopResult ForEach<TSource>(Collections.Generic.IEnumerable<TSource> source, ParallelOptions parallelOptions, Action<TSource> body) { throw null; }

        public static ParallelLoopResult ForEach<TSource, TLocal>(Collections.Concurrent.OrderablePartitioner<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally) { throw null; }

        public static ParallelLoopResult ForEach<TSource, TLocal>(Collections.Concurrent.OrderablePartitioner<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally) { throw null; }

        public static ParallelLoopResult ForEach<TSource, TLocal>(Collections.Concurrent.Partitioner<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally) { throw null; }

        public static ParallelLoopResult ForEach<TSource, TLocal>(Collections.Concurrent.Partitioner<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally) { throw null; }

        public static ParallelLoopResult ForEach<TSource, TLocal>(Collections.Generic.IEnumerable<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally) { throw null; }

        public static ParallelLoopResult ForEach<TSource, TLocal>(Collections.Generic.IEnumerable<TSource> source, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally) { throw null; }

        public static ParallelLoopResult ForEach<TSource, TLocal>(Collections.Generic.IEnumerable<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, TLocal, TLocal> body, Action<TLocal> localFinally) { throw null; }

        public static ParallelLoopResult ForEach<TSource, TLocal>(Collections.Generic.IEnumerable<TSource> source, ParallelOptions parallelOptions, Func<TLocal> localInit, Func<TSource, ParallelLoopState, long, TLocal, TLocal> body, Action<TLocal> localFinally) { throw null; }

        public static void Invoke(params Action[] actions) { }

        public static void Invoke(ParallelOptions parallelOptions, params Action[] actions) { }
    }

    public partial struct ParallelLoopResult
    {
        public bool IsCompleted { get { throw null; } }

        public long? LowestBreakIteration { get { throw null; } }
    }

    public partial class ParallelLoopState
    {
        public bool IsExceptional { get { throw null; } }

        public bool IsStopped { get { throw null; } }

        public long? LowestBreakIteration { get { throw null; } }

        public bool ShouldExitCurrentIteration { get { throw null; } }

        public void Break() { }

        public void Stop() { }
    }

    public partial class ParallelOptions
    {
        public ParallelOptions() { }

        public CancellationToken CancellationToken { get { throw null; } set { } }

        public int MaxDegreeOfParallelism { get { throw null; } set { } }

        public TaskScheduler TaskScheduler { get { throw null; } set { } }
    }
}