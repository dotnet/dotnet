// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName = ".NET Standard 2.1")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Threading.Tasks.Dataflow")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("TPL Dataflow promotes actor/agent-oriented designs through primitives for in-process message passing, dataflow, and pipelining. TDF builds upon the APIs and scheduling infrastructure provided by the Task Parallel Library (TPL), and integrates with the language support for asynchrony provided by C#, Visual Basic, and F#.\r\n\r\nCommonly Used Types:\r\nSystem.Threading.Tasks.Dataflow.ActionBlock<TInput>\r\nSystem.Threading.Tasks.Dataflow.BatchBlock<T>\r\nSystem.Threading.Tasks.Dataflow.BatchedJoinBlock<T1, T2>\r\nSystem.Threading.Tasks.Dataflow.BatchedJoinBlock<T1, T2, T3>\r\nSystem.Threading.Tasks.Dataflow.BroadcastBlock<T>\r\nSystem.Threading.Tasks.Dataflow.BufferBlock<T>\r\nSystem.Threading.Tasks.Dataflow.DataflowBlock\r\nSystem.Threading.Tasks.Dataflow.JoinBlock<T1, T2>\r\nSystem.Threading.Tasks.Dataflow.JoinBlock<T1, T2, T3>\r\nSystem.Threading.Tasks.Dataflow.TransformBlock<TInput, TOutput>\r\nSystem.Threading.Tasks.Dataflow.TransformManyBlock<TInput, TOutput>\r\nSystem.Threading.Tasks.Dataflow.WriteOnceBlock<T>")]
[assembly: System.Reflection.AssemblyFileVersion("8.0.23.53103")]
[assembly: System.Reflection.AssemblyInformationalVersion("8.0.0+5535e31a712343a63f5d7d796cd874e563e5ac14")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Threading.Tasks.Dataflow")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("8.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Threading.Tasks.Dataflow
{
    public sealed partial class ActionBlock<TInput> : ITargetBlock<TInput>, IDataflowBlock
    {
        public ActionBlock(Action<TInput> action, ExecutionDataflowBlockOptions dataflowBlockOptions) { }

        public ActionBlock(Action<TInput> action) { }

        public ActionBlock(Func<TInput, Task> action, ExecutionDataflowBlockOptions dataflowBlockOptions) { }

        public ActionBlock(Func<TInput, Task> action) { }

        public Task Completion { get { throw null; } }

        public int InputCount { get { throw null; } }

        public void Complete() { }

        public bool Post(TInput item) { throw null; }

        void IDataflowBlock.Fault(Exception exception) { }

        DataflowMessageStatus ITargetBlock<TInput>.OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput> source, bool consumeToAccept) { throw null; }

        public override string ToString() { throw null; }
    }

    public sealed partial class BatchBlock<T> : IPropagatorBlock<T, T[]>, ITargetBlock<T>, IDataflowBlock, ISourceBlock<T[]>, IReceivableSourceBlock<T[]>
    {
        public BatchBlock(int batchSize, GroupingDataflowBlockOptions dataflowBlockOptions) { }

        public BatchBlock(int batchSize) { }

        public int BatchSize { get { throw null; } }

        public Task Completion { get { throw null; } }

        public int OutputCount { get { throw null; } }

        public void Complete() { }

        public IDisposable LinkTo(ITargetBlock<T[]> target, DataflowLinkOptions linkOptions) { throw null; }

        void IDataflowBlock.Fault(Exception exception) { }

        T[] ISourceBlock<T[]>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T[]> target, out bool messageConsumed) { throw null; }

        void ISourceBlock<T[]>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T[]> target) { }

        bool ISourceBlock<T[]>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T[]> target) { throw null; }

        DataflowMessageStatus ITargetBlock<T>.OfferMessage(DataflowMessageHeader messageHeader, T messageValue, ISourceBlock<T> source, bool consumeToAccept) { throw null; }

        public override string ToString() { throw null; }

        public void TriggerBatch() { }

        public bool TryReceive(Predicate<T[]>? filter, out T[]? item) { throw null; }

        public bool TryReceiveAll(out Collections.Generic.IList<T[]>? items) { throw null; }
    }

    public sealed partial class BatchedJoinBlock<T1, T2> : IReceivableSourceBlock<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>>>, ISourceBlock<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>>>, IDataflowBlock
    {
        public BatchedJoinBlock(int batchSize, GroupingDataflowBlockOptions dataflowBlockOptions) { }

        public BatchedJoinBlock(int batchSize) { }

        public int BatchSize { get { throw null; } }

        public Task Completion { get { throw null; } }

        public int OutputCount { get { throw null; } }

        public ITargetBlock<T1> Target1 { get { throw null; } }

        public ITargetBlock<T2> Target2 { get { throw null; } }

        public void Complete() { }

        public IDisposable LinkTo(ITargetBlock<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>>> target, DataflowLinkOptions linkOptions) { throw null; }

        void IDataflowBlock.Fault(Exception exception) { }

        Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>> ISourceBlock<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>>>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>>> target, out bool messageConsumed) { throw null; }

        void ISourceBlock<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>>>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>>> target) { }

        bool ISourceBlock<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>>>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>>> target) { throw null; }

        public override string ToString() { throw null; }

        public bool TryReceive(Predicate<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>>>? filter, out Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>>? item) { throw null; }

        public bool TryReceiveAll(out Collections.Generic.IList<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>>>? items) { throw null; }
    }

    public sealed partial class BatchedJoinBlock<T1, T2, T3> : IReceivableSourceBlock<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>, Collections.Generic.IList<T3>>>, ISourceBlock<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>, Collections.Generic.IList<T3>>>, IDataflowBlock
    {
        public BatchedJoinBlock(int batchSize, GroupingDataflowBlockOptions dataflowBlockOptions) { }

        public BatchedJoinBlock(int batchSize) { }

        public int BatchSize { get { throw null; } }

        public Task Completion { get { throw null; } }

        public int OutputCount { get { throw null; } }

        public ITargetBlock<T1> Target1 { get { throw null; } }

        public ITargetBlock<T2> Target2 { get { throw null; } }

        public ITargetBlock<T3> Target3 { get { throw null; } }

        public void Complete() { }

        public IDisposable LinkTo(ITargetBlock<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>, Collections.Generic.IList<T3>>> target, DataflowLinkOptions linkOptions) { throw null; }

        void IDataflowBlock.Fault(Exception exception) { }

        Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>, Collections.Generic.IList<T3>> ISourceBlock<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>, Collections.Generic.IList<T3>>>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>, Collections.Generic.IList<T3>>> target, out bool messageConsumed) { throw null; }

        void ISourceBlock<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>, Collections.Generic.IList<T3>>>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>, Collections.Generic.IList<T3>>> target) { }

        bool ISourceBlock<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>, Collections.Generic.IList<T3>>>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>, Collections.Generic.IList<T3>>> target) { throw null; }

        public override string ToString() { throw null; }

        public bool TryReceive(Predicate<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>, Collections.Generic.IList<T3>>>? filter, out Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>, Collections.Generic.IList<T3>>? item) { throw null; }

        public bool TryReceiveAll(out Collections.Generic.IList<Tuple<Collections.Generic.IList<T1>, Collections.Generic.IList<T2>, Collections.Generic.IList<T3>>>? items) { throw null; }
    }

    public sealed partial class BroadcastBlock<T> : IPropagatorBlock<T, T>, ITargetBlock<T>, IDataflowBlock, ISourceBlock<T>, IReceivableSourceBlock<T>
    {
        public BroadcastBlock(Func<T, T>? cloningFunction, DataflowBlockOptions dataflowBlockOptions) { }

        public BroadcastBlock(Func<T, T>? cloningFunction) { }

        public Task Completion { get { throw null; } }

        public void Complete() { }

        public IDisposable LinkTo(ITargetBlock<T> target, DataflowLinkOptions linkOptions) { throw null; }

        void IDataflowBlock.Fault(Exception exception) { }

        bool IReceivableSourceBlock<T>.TryReceiveAll(out Collections.Generic.IList<T> items) { throw null; }

        T ISourceBlock<T>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target, out bool messageConsumed) { throw null; }

        void ISourceBlock<T>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T> target) { }

        bool ISourceBlock<T>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target) { throw null; }

        DataflowMessageStatus ITargetBlock<T>.OfferMessage(DataflowMessageHeader messageHeader, T messageValue, ISourceBlock<T> source, bool consumeToAccept) { throw null; }

        public override string ToString() { throw null; }

        public bool TryReceive(Predicate<T>? filter, out T item) { throw null; }
    }

    public sealed partial class BufferBlock<T> : IPropagatorBlock<T, T>, ITargetBlock<T>, IDataflowBlock, ISourceBlock<T>, IReceivableSourceBlock<T>
    {
        public BufferBlock() { }

        public BufferBlock(DataflowBlockOptions dataflowBlockOptions) { }

        public Task Completion { get { throw null; } }

        public int Count { get { throw null; } }

        public void Complete() { }

        public IDisposable LinkTo(ITargetBlock<T> target, DataflowLinkOptions linkOptions) { throw null; }

        void IDataflowBlock.Fault(Exception exception) { }

        T ISourceBlock<T>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target, out bool messageConsumed) { throw null; }

        void ISourceBlock<T>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T> target) { }

        bool ISourceBlock<T>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target) { throw null; }

        DataflowMessageStatus ITargetBlock<T>.OfferMessage(DataflowMessageHeader messageHeader, T messageValue, ISourceBlock<T> source, bool consumeToAccept) { throw null; }

        public override string ToString() { throw null; }

        public bool TryReceive(Predicate<T>? filter, out T item) { throw null; }

        public bool TryReceiveAll(out Collections.Generic.IList<T>? items) { throw null; }
    }

    public static partial class DataflowBlock
    {
        public static IObservable<TOutput> AsObservable<TOutput>(this ISourceBlock<TOutput> source) { throw null; }

        public static IObserver<TInput> AsObserver<TInput>(this ITargetBlock<TInput> target) { throw null; }

        public static Task<int> Choose<T1, T2>(ISourceBlock<T1> source1, Action<T1> action1, ISourceBlock<T2> source2, Action<T2> action2, DataflowBlockOptions dataflowBlockOptions) { throw null; }

        public static Task<int> Choose<T1, T2>(ISourceBlock<T1> source1, Action<T1> action1, ISourceBlock<T2> source2, Action<T2> action2) { throw null; }

        public static Task<int> Choose<T1, T2, T3>(ISourceBlock<T1> source1, Action<T1> action1, ISourceBlock<T2> source2, Action<T2> action2, ISourceBlock<T3> source3, Action<T3> action3, DataflowBlockOptions dataflowBlockOptions) { throw null; }

        public static Task<int> Choose<T1, T2, T3>(ISourceBlock<T1> source1, Action<T1> action1, ISourceBlock<T2> source2, Action<T2> action2, ISourceBlock<T3> source3, Action<T3> action3) { throw null; }

        public static IPropagatorBlock<TInput, TOutput> Encapsulate<TInput, TOutput>(ITargetBlock<TInput> target, ISourceBlock<TOutput> source) { throw null; }

        public static IDisposable LinkTo<TOutput>(this ISourceBlock<TOutput> source, ITargetBlock<TOutput> target, Predicate<TOutput> predicate) { throw null; }

        public static IDisposable LinkTo<TOutput>(this ISourceBlock<TOutput> source, ITargetBlock<TOutput> target, DataflowLinkOptions linkOptions, Predicate<TOutput> predicate) { throw null; }

        public static IDisposable LinkTo<TOutput>(this ISourceBlock<TOutput> source, ITargetBlock<TOutput> target) { throw null; }

        public static ITargetBlock<TInput> NullTarget<TInput>() { throw null; }

        public static Task<bool> OutputAvailableAsync<TOutput>(this ISourceBlock<TOutput> source, CancellationToken cancellationToken) { throw null; }

        public static Task<bool> OutputAvailableAsync<TOutput>(this ISourceBlock<TOutput> source) { throw null; }

        public static bool Post<TInput>(this ITargetBlock<TInput> target, TInput item) { throw null; }

        public static TOutput Receive<TOutput>(this ISourceBlock<TOutput> source, CancellationToken cancellationToken) { throw null; }

        public static TOutput Receive<TOutput>(this ISourceBlock<TOutput> source, TimeSpan timeout, CancellationToken cancellationToken) { throw null; }

        public static TOutput Receive<TOutput>(this ISourceBlock<TOutput> source, TimeSpan timeout) { throw null; }

        public static TOutput Receive<TOutput>(this ISourceBlock<TOutput> source) { throw null; }

        public static Collections.Generic.IAsyncEnumerable<TOutput> ReceiveAllAsync<TOutput>(this IReceivableSourceBlock<TOutput> source, CancellationToken cancellationToken = default) { throw null; }

        public static Task<TOutput> ReceiveAsync<TOutput>(this ISourceBlock<TOutput> source, CancellationToken cancellationToken) { throw null; }

        public static Task<TOutput> ReceiveAsync<TOutput>(this ISourceBlock<TOutput> source, TimeSpan timeout, CancellationToken cancellationToken) { throw null; }

        public static Task<TOutput> ReceiveAsync<TOutput>(this ISourceBlock<TOutput> source, TimeSpan timeout) { throw null; }

        public static Task<TOutput> ReceiveAsync<TOutput>(this ISourceBlock<TOutput> source) { throw null; }

        public static Task<bool> SendAsync<TInput>(this ITargetBlock<TInput> target, TInput item, CancellationToken cancellationToken) { throw null; }

        public static Task<bool> SendAsync<TInput>(this ITargetBlock<TInput> target, TInput item) { throw null; }

        public static bool TryReceive<TOutput>(this IReceivableSourceBlock<TOutput> source, out TOutput item) { throw null; }
    }

    public partial class DataflowBlockOptions
    {
        public const int Unbounded = -1;
        public int BoundedCapacity { get { throw null; } set { } }

        public CancellationToken CancellationToken { get { throw null; } set { } }

        public bool EnsureOrdered { get { throw null; } set { } }

        public int MaxMessagesPerTask { get { throw null; } set { } }

        public string NameFormat { get { throw null; } set { } }

        public TaskScheduler TaskScheduler { get { throw null; } set { } }
    }

    public partial class DataflowLinkOptions
    {
        public bool Append { get { throw null; } set { } }

        public int MaxMessages { get { throw null; } set { } }

        public bool PropagateCompletion { get { throw null; } set { } }
    }

    public readonly partial struct DataflowMessageHeader : IEquatable<DataflowMessageHeader>
    {
        private readonly int _dummyPrimitive;
        public DataflowMessageHeader(long id) { }

        public long Id { get { throw null; } }

        public bool IsValid { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(DataflowMessageHeader other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(DataflowMessageHeader left, DataflowMessageHeader right) { throw null; }

        public static bool operator !=(DataflowMessageHeader left, DataflowMessageHeader right) { throw null; }
    }

    public enum DataflowMessageStatus
    {
        Accepted = 0,
        Declined = 1,
        Postponed = 2,
        NotAvailable = 3,
        DecliningPermanently = 4
    }

    public partial class ExecutionDataflowBlockOptions : DataflowBlockOptions
    {
        public int MaxDegreeOfParallelism { get { throw null; } set { } }

        public bool SingleProducerConstrained { get { throw null; } set { } }
    }

    public partial class GroupingDataflowBlockOptions : DataflowBlockOptions
    {
        public bool Greedy { get { throw null; } set { } }

        public long MaxNumberOfGroups { get { throw null; } set { } }
    }

    public partial interface IDataflowBlock
    {
        Task Completion { get; }

        void Complete();
        void Fault(Exception exception);
    }

    public partial interface IPropagatorBlock<in TInput, out TOutput> : ITargetBlock<TInput>, IDataflowBlock, ISourceBlock<TOutput>
    {
    }

    public partial interface IReceivableSourceBlock<TOutput> : ISourceBlock<TOutput>, IDataflowBlock
    {
        bool TryReceive(Predicate<TOutput>? filter, out TOutput item);
        bool TryReceiveAll(out Collections.Generic.IList<TOutput>? items);
    }

    public partial interface ISourceBlock<out TOutput> : IDataflowBlock
    {
        TOutput? ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target, out bool messageConsumed);
        IDisposable LinkTo(ITargetBlock<TOutput> target, DataflowLinkOptions linkOptions);
        void ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target);
        bool ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target);
    }

    public partial interface ITargetBlock<in TInput> : IDataflowBlock
    {
        DataflowMessageStatus OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput>? source, bool consumeToAccept);
    }

    public sealed partial class JoinBlock<T1, T2> : IReceivableSourceBlock<Tuple<T1, T2>>, ISourceBlock<Tuple<T1, T2>>, IDataflowBlock
    {
        public JoinBlock() { }

        public JoinBlock(GroupingDataflowBlockOptions dataflowBlockOptions) { }

        public Task Completion { get { throw null; } }

        public int OutputCount { get { throw null; } }

        public ITargetBlock<T1> Target1 { get { throw null; } }

        public ITargetBlock<T2> Target2 { get { throw null; } }

        public void Complete() { }

        public IDisposable LinkTo(ITargetBlock<Tuple<T1, T2>> target, DataflowLinkOptions linkOptions) { throw null; }

        void IDataflowBlock.Fault(Exception exception) { }

        Tuple<T1, T2> ISourceBlock<Tuple<T1, T2>>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<T1, T2>> target, out bool messageConsumed) { throw null; }

        void ISourceBlock<Tuple<T1, T2>>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<T1, T2>> target) { }

        bool ISourceBlock<Tuple<T1, T2>>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<T1, T2>> target) { throw null; }

        public override string ToString() { throw null; }

        public bool TryReceive(Predicate<Tuple<T1, T2>>? filter, out Tuple<T1, T2>? item) { throw null; }

        public bool TryReceiveAll(out Collections.Generic.IList<Tuple<T1, T2>>? items) { throw null; }
    }

    public sealed partial class JoinBlock<T1, T2, T3> : IReceivableSourceBlock<Tuple<T1, T2, T3>>, ISourceBlock<Tuple<T1, T2, T3>>, IDataflowBlock
    {
        public JoinBlock() { }

        public JoinBlock(GroupingDataflowBlockOptions dataflowBlockOptions) { }

        public Task Completion { get { throw null; } }

        public int OutputCount { get { throw null; } }

        public ITargetBlock<T1> Target1 { get { throw null; } }

        public ITargetBlock<T2> Target2 { get { throw null; } }

        public ITargetBlock<T3> Target3 { get { throw null; } }

        public void Complete() { }

        public IDisposable LinkTo(ITargetBlock<Tuple<T1, T2, T3>> target, DataflowLinkOptions linkOptions) { throw null; }

        void IDataflowBlock.Fault(Exception exception) { }

        Tuple<T1, T2, T3> ISourceBlock<Tuple<T1, T2, T3>>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<T1, T2, T3>> target, out bool messageConsumed) { throw null; }

        void ISourceBlock<Tuple<T1, T2, T3>>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<T1, T2, T3>> target) { }

        bool ISourceBlock<Tuple<T1, T2, T3>>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<Tuple<T1, T2, T3>> target) { throw null; }

        public override string ToString() { throw null; }

        public bool TryReceive(Predicate<Tuple<T1, T2, T3>>? filter, out Tuple<T1, T2, T3>? item) { throw null; }

        public bool TryReceiveAll(out Collections.Generic.IList<Tuple<T1, T2, T3>>? items) { throw null; }
    }

    public sealed partial class TransformBlock<TInput, TOutput> : IPropagatorBlock<TInput, TOutput>, ITargetBlock<TInput>, IDataflowBlock, ISourceBlock<TOutput>, IReceivableSourceBlock<TOutput>
    {
        public TransformBlock(Func<TInput, TOutput> transform, ExecutionDataflowBlockOptions dataflowBlockOptions) { }

        public TransformBlock(Func<TInput, TOutput> transform) { }

        public TransformBlock(Func<TInput, Task<TOutput>> transform, ExecutionDataflowBlockOptions dataflowBlockOptions) { }

        public TransformBlock(Func<TInput, Task<TOutput>> transform) { }

        public Task Completion { get { throw null; } }

        public int InputCount { get { throw null; } }

        public int OutputCount { get { throw null; } }

        public void Complete() { }

        public IDisposable LinkTo(ITargetBlock<TOutput> target, DataflowLinkOptions linkOptions) { throw null; }

        void IDataflowBlock.Fault(Exception exception) { }

        TOutput ISourceBlock<TOutput>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target, out bool messageConsumed) { throw null; }

        void ISourceBlock<TOutput>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target) { }

        bool ISourceBlock<TOutput>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target) { throw null; }

        DataflowMessageStatus ITargetBlock<TInput>.OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput> source, bool consumeToAccept) { throw null; }

        public override string ToString() { throw null; }

        public bool TryReceive(Predicate<TOutput>? filter, out TOutput item) { throw null; }

        public bool TryReceiveAll(out Collections.Generic.IList<TOutput>? items) { throw null; }
    }

    public sealed partial class TransformManyBlock<TInput, TOutput> : IPropagatorBlock<TInput, TOutput>, ITargetBlock<TInput>, IDataflowBlock, ISourceBlock<TOutput>, IReceivableSourceBlock<TOutput>
    {
        public TransformManyBlock(Func<TInput, Collections.Generic.IAsyncEnumerable<TOutput>> transform, ExecutionDataflowBlockOptions dataflowBlockOptions) { }

        public TransformManyBlock(Func<TInput, Collections.Generic.IAsyncEnumerable<TOutput>> transform) { }

        public TransformManyBlock(Func<TInput, Collections.Generic.IEnumerable<TOutput>> transform, ExecutionDataflowBlockOptions dataflowBlockOptions) { }

        public TransformManyBlock(Func<TInput, Collections.Generic.IEnumerable<TOutput>> transform) { }

        public TransformManyBlock(Func<TInput, Task<Collections.Generic.IEnumerable<TOutput>>> transform, ExecutionDataflowBlockOptions dataflowBlockOptions) { }

        public TransformManyBlock(Func<TInput, Task<Collections.Generic.IEnumerable<TOutput>>> transform) { }

        public Task Completion { get { throw null; } }

        public int InputCount { get { throw null; } }

        public int OutputCount { get { throw null; } }

        public void Complete() { }

        public IDisposable LinkTo(ITargetBlock<TOutput> target, DataflowLinkOptions linkOptions) { throw null; }

        void IDataflowBlock.Fault(Exception exception) { }

        TOutput ISourceBlock<TOutput>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target, out bool messageConsumed) { throw null; }

        void ISourceBlock<TOutput>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target) { }

        bool ISourceBlock<TOutput>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<TOutput> target) { throw null; }

        DataflowMessageStatus ITargetBlock<TInput>.OfferMessage(DataflowMessageHeader messageHeader, TInput messageValue, ISourceBlock<TInput> source, bool consumeToAccept) { throw null; }

        public override string ToString() { throw null; }

        public bool TryReceive(Predicate<TOutput>? filter, out TOutput item) { throw null; }

        public bool TryReceiveAll(out Collections.Generic.IList<TOutput>? items) { throw null; }
    }

    public sealed partial class WriteOnceBlock<T> : IPropagatorBlock<T, T>, ITargetBlock<T>, IDataflowBlock, ISourceBlock<T>, IReceivableSourceBlock<T>
    {
        public WriteOnceBlock(Func<T, T>? cloningFunction, DataflowBlockOptions dataflowBlockOptions) { }

        public WriteOnceBlock(Func<T, T>? cloningFunction) { }

        public Task Completion { get { throw null; } }

        public void Complete() { }

        public IDisposable LinkTo(ITargetBlock<T> target, DataflowLinkOptions linkOptions) { throw null; }

        void IDataflowBlock.Fault(Exception exception) { }

        bool IReceivableSourceBlock<T>.TryReceiveAll(out Collections.Generic.IList<T> items) { throw null; }

        T ISourceBlock<T>.ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target, out bool messageConsumed) { throw null; }

        void ISourceBlock<T>.ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T> target) { }

        bool ISourceBlock<T>.ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target) { throw null; }

        DataflowMessageStatus ITargetBlock<T>.OfferMessage(DataflowMessageHeader messageHeader, T messageValue, ISourceBlock<T> source, bool consumeToAccept) { throw null; }

        public override string ToString() { throw null; }

        public bool TryReceive(Predicate<T>? filter, out T item) { throw null; }
    }
}