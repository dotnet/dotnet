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
[assembly: System.Reflection.AssemblyTitle("System.Threading.Tasks")]
[assembly: System.Reflection.AssemblyDescription("System.Threading.Tasks")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Threading.Tasks")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.10.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System
{
    public partial class AggregateException : Exception
    {
        public AggregateException() { }

        public AggregateException(Collections.Generic.IEnumerable<Exception> innerExceptions) { }

        public AggregateException(params Exception[] innerExceptions) { }

        public AggregateException(string message, Collections.Generic.IEnumerable<Exception> innerExceptions) { }

        public AggregateException(string message, Exception innerException) { }

        public AggregateException(string message, params Exception[] innerExceptions) { }

        public AggregateException(string message) { }

        public Collections.ObjectModel.ReadOnlyCollection<Exception> InnerExceptions { get { throw null; } }

        public AggregateException Flatten() { throw null; }

        public override Exception GetBaseException() { throw null; }

        public void Handle(Func<Exception, bool> predicate) { }

        public override string ToString() { throw null; }
    }

    public partial class OperationCanceledException : Exception
    {
        public OperationCanceledException() { }

        public OperationCanceledException(string message, Exception innerException, Threading.CancellationToken token) { }

        public OperationCanceledException(string message, Exception innerException) { }

        public OperationCanceledException(string message, Threading.CancellationToken token) { }

        public OperationCanceledException(string message) { }

        public OperationCanceledException(Threading.CancellationToken token) { }

        public Threading.CancellationToken CancellationToken { get { throw null; } }
    }
}

namespace System.Runtime.CompilerServices
{
    public partial struct AsyncTaskMethodBuilder
    {
        public Threading.Tasks.Task Task { get { throw null; } }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine { }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine { }

        public static AsyncTaskMethodBuilder Create() { throw null; }

        public void SetException(Exception exception) { }

        public void SetResult() { }

        public void SetStateMachine(IAsyncStateMachine stateMachine) { }

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine { }
    }

    public partial struct AsyncTaskMethodBuilder<TResult>
    {
        public Threading.Tasks.Task<TResult> Task { get { throw null; } }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine { }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine { }

        public static AsyncTaskMethodBuilder<TResult> Create() { throw null; }

        public void SetException(Exception exception) { }

        public void SetResult(TResult result) { }

        public void SetStateMachine(IAsyncStateMachine stateMachine) { }

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine { }
    }

    public partial struct AsyncVoidMethodBuilder
    {
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine { }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine { }

        public static AsyncVoidMethodBuilder Create() { throw null; }

        public void SetException(Exception exception) { }

        public void SetResult() { }

        public void SetStateMachine(IAsyncStateMachine stateMachine) { }

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine { }
    }

    public partial struct ConfiguredTaskAwaitable
    {
        public ConfiguredTaskAwaiter GetAwaiter() { throw null; }

        public partial struct ConfiguredTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
        {
            public bool IsCompleted { get { throw null; } }

            public void GetResult() { }

            public void OnCompleted(Action continuation) { }

            public void UnsafeOnCompleted(Action continuation) { }
        }
    }

    public partial struct ConfiguredTaskAwaitable<TResult>
    {
        public ConfiguredTaskAwaiter GetAwaiter() { throw null; }

        public partial struct ConfiguredTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
        {
            public bool IsCompleted { get { throw null; } }

            public TResult GetResult() { throw null; }

            public void OnCompleted(Action continuation) { }

            public void UnsafeOnCompleted(Action continuation) { }
        }
    }

    public partial interface IAsyncStateMachine
    {
        void MoveNext();
        void SetStateMachine(IAsyncStateMachine stateMachine);
    }

    public partial interface ICriticalNotifyCompletion : INotifyCompletion
    {
        void UnsafeOnCompleted(Action continuation);
    }

    public partial interface INotifyCompletion
    {
        void OnCompleted(Action continuation);
    }

    public partial struct TaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
        public bool IsCompleted { get { throw null; } }

        public void GetResult() { }

        public void OnCompleted(Action continuation) { }

        public void UnsafeOnCompleted(Action continuation) { }
    }

    public partial struct TaskAwaiter<TResult> : ICriticalNotifyCompletion, INotifyCompletion
    {
        public bool IsCompleted { get { throw null; } }

        public TResult GetResult() { throw null; }

        public void OnCompleted(Action continuation) { }

        public void UnsafeOnCompleted(Action continuation) { }
    }

    public partial struct YieldAwaitable
    {
        public YieldAwaiter GetAwaiter() { throw null; }

        public partial struct YieldAwaiter : ICriticalNotifyCompletion, INotifyCompletion
        {
            public bool IsCompleted { get { throw null; } }

            public void GetResult() { }

            public void OnCompleted(Action continuation) { }

            public void UnsafeOnCompleted(Action continuation) { }
        }
    }
}

namespace System.Threading
{
    public partial struct CancellationToken
    {
        public CancellationToken(bool canceled) { }

        public bool CanBeCanceled { get { throw null; } }

        public bool IsCancellationRequested { get { throw null; } }

        public static CancellationToken None { get { throw null; } }

        public WaitHandle WaitHandle { get { throw null; } }

        public override bool Equals(object other) { throw null; }

        public bool Equals(CancellationToken other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(CancellationToken left, CancellationToken right) { throw null; }

        public static bool operator !=(CancellationToken left, CancellationToken right) { throw null; }

        public CancellationTokenRegistration Register(Action callback, bool useSynchronizationContext) { throw null; }

        public CancellationTokenRegistration Register(Action callback) { throw null; }

        public CancellationTokenRegistration Register(Action<object> callback, object state, bool useSynchronizationContext) { throw null; }

        public CancellationTokenRegistration Register(Action<object> callback, object state) { throw null; }

        public void ThrowIfCancellationRequested() { }
    }

    public partial struct CancellationTokenRegistration : IDisposable, IEquatable<CancellationTokenRegistration>
    {
        public void Dispose() { }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(CancellationTokenRegistration other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(CancellationTokenRegistration left, CancellationTokenRegistration right) { throw null; }

        public static bool operator !=(CancellationTokenRegistration left, CancellationTokenRegistration right) { throw null; }
    }

    public partial class CancellationTokenSource : IDisposable
    {
        public CancellationTokenSource() { }

        public CancellationTokenSource(int millisecondsDelay) { }

        public CancellationTokenSource(TimeSpan delay) { }

        public bool IsCancellationRequested { get { throw null; } }

        public CancellationToken Token { get { throw null; } }

        public void Cancel() { }

        public void Cancel(bool throwOnFirstException) { }

        public void CancelAfter(int millisecondsDelay) { }

        public void CancelAfter(TimeSpan delay) { }

        public static CancellationTokenSource CreateLinkedTokenSource(CancellationToken token1, CancellationToken token2) { throw null; }

        public static CancellationTokenSource CreateLinkedTokenSource(params CancellationToken[] tokens) { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }
    }
}

namespace System.Threading.Tasks
{
    public partial class ConcurrentExclusiveSchedulerPair
    {
        public ConcurrentExclusiveSchedulerPair() { }

        public ConcurrentExclusiveSchedulerPair(TaskScheduler taskScheduler, int maxConcurrencyLevel, int maxItemsPerTask) { }

        public ConcurrentExclusiveSchedulerPair(TaskScheduler taskScheduler, int maxConcurrencyLevel) { }

        public ConcurrentExclusiveSchedulerPair(TaskScheduler taskScheduler) { }

        public Task Completion { get { throw null; } }

        public TaskScheduler ConcurrentScheduler { get { throw null; } }

        public TaskScheduler ExclusiveScheduler { get { throw null; } }

        public void Complete() { }
    }

    public partial class Task : IAsyncResult
    {
        public Task(Action action, CancellationToken cancellationToken, TaskCreationOptions creationOptions) { }

        public Task(Action action, CancellationToken cancellationToken) { }

        public Task(Action action, TaskCreationOptions creationOptions) { }

        public Task(Action action) { }

        public Task(Action<object> action, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions) { }

        public Task(Action<object> action, object state, CancellationToken cancellationToken) { }

        public Task(Action<object> action, object state, TaskCreationOptions creationOptions) { }

        public Task(Action<object> action, object state) { }

        public object AsyncState { get { throw null; } }

        public static Task CompletedTask { get { throw null; } }

        public TaskCreationOptions CreationOptions { get { throw null; } }

        public static int? CurrentId { get { throw null; } }

        public AggregateException Exception { get { throw null; } }

        public static TaskFactory Factory { get { throw null; } }

        public int Id { get { throw null; } }

        public bool IsCanceled { get { throw null; } }

        public bool IsCompleted { get { throw null; } }

        public bool IsFaulted { get { throw null; } }

        public TaskStatus Status { get { throw null; } }

        WaitHandle IAsyncResult.AsyncWaitHandle { get { throw null; } }

        bool IAsyncResult.CompletedSynchronously { get { throw null; } }

        public Runtime.CompilerServices.ConfiguredTaskAwaitable ConfigureAwait(bool continueOnCapturedContext) { throw null; }

        public Task ContinueWith(Action<Task, object> continuationAction, object state, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task ContinueWith(Action<Task, object> continuationAction, object state, CancellationToken cancellationToken) { throw null; }

        public Task ContinueWith(Action<Task, object> continuationAction, object state, TaskContinuationOptions continuationOptions) { throw null; }

        public Task ContinueWith(Action<Task, object> continuationAction, object state, TaskScheduler scheduler) { throw null; }

        public Task ContinueWith(Action<Task, object> continuationAction, object state) { throw null; }

        public Task ContinueWith(Action<Task> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task ContinueWith(Action<Task> continuationAction, CancellationToken cancellationToken) { throw null; }

        public Task ContinueWith(Action<Task> continuationAction, TaskContinuationOptions continuationOptions) { throw null; }

        public Task ContinueWith(Action<Task> continuationAction, TaskScheduler scheduler) { throw null; }

        public Task ContinueWith(Action<Task> continuationAction) { throw null; }

        public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, CancellationToken cancellationToken) { throw null; }

        public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions) { throw null; }

        public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction, TaskScheduler scheduler) { throw null; }

        public Task<TResult> ContinueWith<TResult>(Func<Task, TResult> continuationFunction) { throw null; }

        public Task<TResult> ContinueWith<TResult>(Func<Task, object, TResult> continuationFunction, object state, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task<TResult> ContinueWith<TResult>(Func<Task, object, TResult> continuationFunction, object state, CancellationToken cancellationToken) { throw null; }

        public Task<TResult> ContinueWith<TResult>(Func<Task, object, TResult> continuationFunction, object state, TaskContinuationOptions continuationOptions) { throw null; }

        public Task<TResult> ContinueWith<TResult>(Func<Task, object, TResult> continuationFunction, object state, TaskScheduler scheduler) { throw null; }

        public Task<TResult> ContinueWith<TResult>(Func<Task, object, TResult> continuationFunction, object state) { throw null; }

        public static Task Delay(int millisecondsDelay, CancellationToken cancellationToken) { throw null; }

        public static Task Delay(int millisecondsDelay) { throw null; }

        public static Task Delay(TimeSpan delay, CancellationToken cancellationToken) { throw null; }

        public static Task Delay(TimeSpan delay) { throw null; }

        public static Task FromCanceled(CancellationToken cancellationToken) { throw null; }

        public static Task<TResult> FromCanceled<TResult>(CancellationToken cancellationToken) { throw null; }

        public static Task FromException(Exception exception) { throw null; }

        public static Task<TResult> FromException<TResult>(Exception exception) { throw null; }

        public static Task<TResult> FromResult<TResult>(TResult result) { throw null; }

        public Runtime.CompilerServices.TaskAwaiter GetAwaiter() { throw null; }

        public static Task Run(Action action, CancellationToken cancellationToken) { throw null; }

        public static Task Run(Action action) { throw null; }

        public static Task Run(Func<Task> function, CancellationToken cancellationToken) { throw null; }

        public static Task Run(Func<Task> function) { throw null; }

        public static Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken) { throw null; }

        public static Task<TResult> Run<TResult>(Func<TResult> function) { throw null; }

        public static Task<TResult> Run<TResult>(Func<Task<TResult>> function, CancellationToken cancellationToken) { throw null; }

        public static Task<TResult> Run<TResult>(Func<Task<TResult>> function) { throw null; }

        public void RunSynchronously() { }

        public void RunSynchronously(TaskScheduler scheduler) { }

        public void Start() { }

        public void Start(TaskScheduler scheduler) { }

        public void Wait() { }

        public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken) { throw null; }

        public bool Wait(int millisecondsTimeout) { throw null; }

        public void Wait(CancellationToken cancellationToken) { }

        public bool Wait(TimeSpan timeout) { throw null; }

        public static bool WaitAll(Task[] tasks, int millisecondsTimeout, CancellationToken cancellationToken) { throw null; }

        public static bool WaitAll(Task[] tasks, int millisecondsTimeout) { throw null; }

        public static void WaitAll(Task[] tasks, CancellationToken cancellationToken) { }

        public static bool WaitAll(Task[] tasks, TimeSpan timeout) { throw null; }

        public static void WaitAll(params Task[] tasks) { }

        public static int WaitAny(Task[] tasks, int millisecondsTimeout, CancellationToken cancellationToken) { throw null; }

        public static int WaitAny(Task[] tasks, int millisecondsTimeout) { throw null; }

        public static int WaitAny(Task[] tasks, CancellationToken cancellationToken) { throw null; }

        public static int WaitAny(Task[] tasks, TimeSpan timeout) { throw null; }

        public static int WaitAny(params Task[] tasks) { throw null; }

        public static Task WhenAll(Collections.Generic.IEnumerable<Task> tasks) { throw null; }

        public static Task WhenAll(params Task[] tasks) { throw null; }

        public static Task<TResult[]> WhenAll<TResult>(Collections.Generic.IEnumerable<Task<TResult>> tasks) { throw null; }

        public static Task<TResult[]> WhenAll<TResult>(params Task<TResult>[] tasks) { throw null; }

        public static Task<Task> WhenAny(Collections.Generic.IEnumerable<Task> tasks) { throw null; }

        public static Task<Task> WhenAny(params Task[] tasks) { throw null; }

        public static Task<Task<TResult>> WhenAny<TResult>(Collections.Generic.IEnumerable<Task<TResult>> tasks) { throw null; }

        public static Task<Task<TResult>> WhenAny<TResult>(params Task<TResult>[] tasks) { throw null; }

        public static Runtime.CompilerServices.YieldAwaitable Yield() { throw null; }
    }

    public partial class TaskCanceledException : OperationCanceledException
    {
        public TaskCanceledException() { }

        public TaskCanceledException(string message, Exception innerException) { }

        public TaskCanceledException(string message) { }

        public TaskCanceledException(Task task) { }

        public Task Task { get { throw null; } }
    }

    public partial class TaskCompletionSource<TResult>
    {
        public TaskCompletionSource() { }

        public TaskCompletionSource(object state, TaskCreationOptions creationOptions) { }

        public TaskCompletionSource(object state) { }

        public TaskCompletionSource(TaskCreationOptions creationOptions) { }

        public Task<TResult> Task { get { throw null; } }

        public void SetCanceled() { }

        public void SetException(Collections.Generic.IEnumerable<Exception> exceptions) { }

        public void SetException(Exception exception) { }

        public void SetResult(TResult result) { }

        public bool TrySetCanceled() { throw null; }

        public bool TrySetCanceled(CancellationToken cancellationToken) { throw null; }

        public bool TrySetException(Collections.Generic.IEnumerable<Exception> exceptions) { throw null; }

        public bool TrySetException(Exception exception) { throw null; }

        public bool TrySetResult(TResult result) { throw null; }
    }

    [Flags]
    public enum TaskContinuationOptions
    {
        None = 0,
        PreferFairness = 1,
        LongRunning = 2,
        AttachedToParent = 4,
        DenyChildAttach = 8,
        HideScheduler = 16,
        LazyCancellation = 32,
        RunContinuationsAsynchronously = 64,
        NotOnRanToCompletion = 65536,
        NotOnFaulted = 131072,
        OnlyOnCanceled = 196608,
        NotOnCanceled = 262144,
        OnlyOnFaulted = 327680,
        OnlyOnRanToCompletion = 393216,
        ExecuteSynchronously = 524288
    }

    [Flags]
    public enum TaskCreationOptions
    {
        None = 0,
        PreferFairness = 1,
        LongRunning = 2,
        AttachedToParent = 4,
        DenyChildAttach = 8,
        HideScheduler = 16,
        RunContinuationsAsynchronously = 64
    }

    public static partial class TaskExtensions
    {
        public static Task Unwrap(this Task<Task> task) { throw null; }

        public static Task<TResult> Unwrap<TResult>(this Task<Task<TResult>> task) { throw null; }
    }

    public partial class TaskFactory
    {
        public TaskFactory() { }

        public TaskFactory(CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { }

        public TaskFactory(CancellationToken cancellationToken) { }

        public TaskFactory(TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions) { }

        public TaskFactory(TaskScheduler scheduler) { }

        public CancellationToken CancellationToken { get { throw null; } }

        public TaskContinuationOptions ContinuationOptions { get { throw null; } }

        public TaskCreationOptions CreationOptions { get { throw null; } }

        public TaskScheduler Scheduler { get { throw null; } }

        public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction, CancellationToken cancellationToken) { throw null; }

        public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction, TaskContinuationOptions continuationOptions) { throw null; }

        public Task ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction) { throw null; }

        public Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken) { throw null; }

        public Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction, TaskContinuationOptions continuationOptions) { throw null; }

        public Task<TResult> ContinueWhenAll<TResult>(Task[] tasks, Func<Task[], TResult> continuationFunction) { throw null; }

        public Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction, CancellationToken cancellationToken) { throw null; }

        public Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction, TaskContinuationOptions continuationOptions) { throw null; }

        public Task ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>[]> continuationAction) { throw null; }

        public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, CancellationToken cancellationToken) { throw null; }

        public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, TaskContinuationOptions continuationOptions) { throw null; }

        public Task<TResult> ContinueWhenAll<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction) { throw null; }

        public Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction, CancellationToken cancellationToken) { throw null; }

        public Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction, TaskContinuationOptions continuationOptions) { throw null; }

        public Task ContinueWhenAny(Task[] tasks, Action<Task> continuationAction) { throw null; }

        public Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken) { throw null; }

        public Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions) { throw null; }

        public Task<TResult> ContinueWhenAny<TResult>(Task[] tasks, Func<Task, TResult> continuationFunction) { throw null; }

        public Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction, CancellationToken cancellationToken) { throw null; }

        public Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction, TaskContinuationOptions continuationOptions) { throw null; }

        public Task ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Action<Task<TAntecedentResult>> continuationAction) { throw null; }

        public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, CancellationToken cancellationToken) { throw null; }

        public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, TaskContinuationOptions continuationOptions) { throw null; }

        public Task<TResult> ContinueWhenAny<TAntecedentResult, TResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction) { throw null; }

        public Task FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, object state, TaskCreationOptions creationOptions) { throw null; }

        public Task FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, object state) { throw null; }

        public Task FromAsync(IAsyncResult asyncResult, Action<IAsyncResult> endMethod, TaskCreationOptions creationOptions, TaskScheduler scheduler) { throw null; }

        public Task FromAsync(IAsyncResult asyncResult, Action<IAsyncResult> endMethod, TaskCreationOptions creationOptions) { throw null; }

        public Task FromAsync(IAsyncResult asyncResult, Action<IAsyncResult> endMethod) { throw null; }

        public Task FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, object state, TaskCreationOptions creationOptions) { throw null; }

        public Task FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, object state) { throw null; }

        public Task<TResult> FromAsync<TResult>(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state, TaskCreationOptions creationOptions) { throw null; }

        public Task<TResult> FromAsync<TResult>(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state) { throw null; }

        public Task<TResult> FromAsync<TResult>(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions, TaskScheduler scheduler) { throw null; }

        public Task<TResult> FromAsync<TResult>(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions) { throw null; }

        public Task<TResult> FromAsync<TResult>(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod) { throw null; }

        public Task FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, object state, TaskCreationOptions creationOptions) { throw null; }

        public Task FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, object state) { throw null; }

        public Task<TResult> FromAsync<TArg1, TResult>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state, TaskCreationOptions creationOptions) { throw null; }

        public Task<TResult> FromAsync<TArg1, TResult>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state) { throw null; }

        public Task FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, TaskCreationOptions creationOptions) { throw null; }

        public Task FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Action<IAsyncResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state) { throw null; }

        public Task<TResult> FromAsync<TArg1, TArg2, TResult>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state, TaskCreationOptions creationOptions) { throw null; }

        public Task<TResult> FromAsync<TArg1, TArg2, TResult>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state) { throw null; }

        public Task<TResult> FromAsync<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, TaskCreationOptions creationOptions) { throw null; }

        public Task<TResult> FromAsync<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state) { throw null; }

        public Task StartNew(Action action, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler) { throw null; }

        public Task StartNew(Action action, CancellationToken cancellationToken) { throw null; }

        public Task StartNew(Action action, TaskCreationOptions creationOptions) { throw null; }

        public Task StartNew(Action action) { throw null; }

        public Task StartNew(Action<object> action, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler) { throw null; }

        public Task StartNew(Action<object> action, object state, CancellationToken cancellationToken) { throw null; }

        public Task StartNew(Action<object> action, object state, TaskCreationOptions creationOptions) { throw null; }

        public Task StartNew(Action<object> action, object state) { throw null; }

        public Task<TResult> StartNew<TResult>(Func<TResult> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler) { throw null; }

        public Task<TResult> StartNew<TResult>(Func<TResult> function, CancellationToken cancellationToken) { throw null; }

        public Task<TResult> StartNew<TResult>(Func<TResult> function, TaskCreationOptions creationOptions) { throw null; }

        public Task<TResult> StartNew<TResult>(Func<TResult> function) { throw null; }

        public Task<TResult> StartNew<TResult>(Func<object, TResult> function, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler) { throw null; }

        public Task<TResult> StartNew<TResult>(Func<object, TResult> function, object state, CancellationToken cancellationToken) { throw null; }

        public Task<TResult> StartNew<TResult>(Func<object, TResult> function, object state, TaskCreationOptions creationOptions) { throw null; }

        public Task<TResult> StartNew<TResult>(Func<object, TResult> function, object state) { throw null; }
    }

    public partial class TaskFactory<TResult>
    {
        public TaskFactory() { }

        public TaskFactory(CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { }

        public TaskFactory(CancellationToken cancellationToken) { }

        public TaskFactory(TaskCreationOptions creationOptions, TaskContinuationOptions continuationOptions) { }

        public TaskFactory(TaskScheduler scheduler) { }

        public CancellationToken CancellationToken { get { throw null; } }

        public TaskContinuationOptions ContinuationOptions { get { throw null; } }

        public TaskCreationOptions CreationOptions { get { throw null; } }

        public TaskScheduler Scheduler { get { throw null; } }

        public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction, CancellationToken cancellationToken) { throw null; }

        public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction, TaskContinuationOptions continuationOptions) { throw null; }

        public Task<TResult> ContinueWhenAll(Task[] tasks, Func<Task[], TResult> continuationFunction) { throw null; }

        public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, CancellationToken cancellationToken) { throw null; }

        public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction, TaskContinuationOptions continuationOptions) { throw null; }

        public Task<TResult> ContinueWhenAll<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>[], TResult> continuationFunction) { throw null; }

        public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction, CancellationToken cancellationToken) { throw null; }

        public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction, TaskContinuationOptions continuationOptions) { throw null; }

        public Task<TResult> ContinueWhenAny(Task[] tasks, Func<Task, TResult> continuationFunction) { throw null; }

        public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, CancellationToken cancellationToken) { throw null; }

        public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction, TaskContinuationOptions continuationOptions) { throw null; }

        public Task<TResult> ContinueWhenAny<TAntecedentResult>(Task<TAntecedentResult>[] tasks, Func<Task<TAntecedentResult>, TResult> continuationFunction) { throw null; }

        public Task<TResult> FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state, TaskCreationOptions creationOptions) { throw null; }

        public Task<TResult> FromAsync(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, object state) { throw null; }

        public Task<TResult> FromAsync(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions, TaskScheduler scheduler) { throw null; }

        public Task<TResult> FromAsync(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod, TaskCreationOptions creationOptions) { throw null; }

        public Task<TResult> FromAsync(IAsyncResult asyncResult, Func<IAsyncResult, TResult> endMethod) { throw null; }

        public Task<TResult> FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state, TaskCreationOptions creationOptions) { throw null; }

        public Task<TResult> FromAsync<TArg1>(Func<TArg1, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, object state) { throw null; }

        public Task<TResult> FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state, TaskCreationOptions creationOptions) { throw null; }

        public Task<TResult> FromAsync<TArg1, TArg2>(Func<TArg1, TArg2, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, object state) { throw null; }

        public Task<TResult> FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state, TaskCreationOptions creationOptions) { throw null; }

        public Task<TResult> FromAsync<TArg1, TArg2, TArg3>(Func<TArg1, TArg2, TArg3, AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, TResult> endMethod, TArg1 arg1, TArg2 arg2, TArg3 arg3, object state) { throw null; }

        public Task<TResult> StartNew(Func<TResult> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler) { throw null; }

        public Task<TResult> StartNew(Func<TResult> function, CancellationToken cancellationToken) { throw null; }

        public Task<TResult> StartNew(Func<TResult> function, TaskCreationOptions creationOptions) { throw null; }

        public Task<TResult> StartNew(Func<TResult> function) { throw null; }

        public Task<TResult> StartNew(Func<object, TResult> function, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions, TaskScheduler scheduler) { throw null; }

        public Task<TResult> StartNew(Func<object, TResult> function, object state, CancellationToken cancellationToken) { throw null; }

        public Task<TResult> StartNew(Func<object, TResult> function, object state, TaskCreationOptions creationOptions) { throw null; }

        public Task<TResult> StartNew(Func<object, TResult> function, object state) { throw null; }
    }

    public abstract partial class TaskScheduler
    {
        public static TaskScheduler Current { get { throw null; } }

        public static TaskScheduler Default { get { throw null; } }

        public int Id { get { throw null; } }

        public virtual int MaximumConcurrencyLevel { get { throw null; } }

        public static event EventHandler<UnobservedTaskExceptionEventArgs> UnobservedTaskException { add { } remove { } }

        public static TaskScheduler FromCurrentSynchronizationContext() { throw null; }

        protected abstract Collections.Generic.IEnumerable<Task> GetScheduledTasks();
        protected internal abstract void QueueTask(Task task);
        protected internal virtual bool TryDequeue(Task task) { throw null; }

        protected bool TryExecuteTask(Task task) { throw null; }

        protected abstract bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued);
    }

    public partial class TaskSchedulerException : Exception
    {
        public TaskSchedulerException() { }

        public TaskSchedulerException(Exception innerException) { }

        public TaskSchedulerException(string message, Exception innerException) { }

        public TaskSchedulerException(string message) { }
    }

    public enum TaskStatus
    {
        Created = 0,
        WaitingForActivation = 1,
        WaitingToRun = 2,
        Running = 3,
        WaitingForChildrenToComplete = 4,
        RanToCompletion = 5,
        Canceled = 6,
        Faulted = 7
    }

    public partial class Task<TResult> : Task
    {
        public Task(Func<TResult> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(default!) { }

        public Task(Func<TResult> function, CancellationToken cancellationToken) : base(default!) { }

        public Task(Func<TResult> function, TaskCreationOptions creationOptions) : base(default!) { }

        public Task(Func<TResult> function) : base(default!) { }

        public Task(Func<object, TResult> function, object state, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(default!) { }

        public Task(Func<object, TResult> function, object state, CancellationToken cancellationToken) : base(default!) { }

        public Task(Func<object, TResult> function, object state, TaskCreationOptions creationOptions) : base(default!) { }

        public Task(Func<object, TResult> function, object state) : base(default!) { }

        public new static TaskFactory<TResult> Factory { get { throw null; } }

        public TResult Result { get { throw null; } }

        public new Runtime.CompilerServices.ConfiguredTaskAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext) { throw null; }

        public Task ContinueWith(Action<Task<TResult>, object> continuationAction, object state, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task ContinueWith(Action<Task<TResult>, object> continuationAction, object state, CancellationToken cancellationToken) { throw null; }

        public Task ContinueWith(Action<Task<TResult>, object> continuationAction, object state, TaskContinuationOptions continuationOptions) { throw null; }

        public Task ContinueWith(Action<Task<TResult>, object> continuationAction, object state, TaskScheduler scheduler) { throw null; }

        public Task ContinueWith(Action<Task<TResult>, object> continuationAction, object state) { throw null; }

        public Task ContinueWith(Action<Task<TResult>> continuationAction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task ContinueWith(Action<Task<TResult>> continuationAction, CancellationToken cancellationToken) { throw null; }

        public Task ContinueWith(Action<Task<TResult>> continuationAction, TaskContinuationOptions continuationOptions) { throw null; }

        public Task ContinueWith(Action<Task<TResult>> continuationAction, TaskScheduler scheduler) { throw null; }

        public Task ContinueWith(Action<Task<TResult>> continuationAction) { throw null; }

        public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, TNewResult> continuationFunction, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, TNewResult> continuationFunction, CancellationToken cancellationToken) { throw null; }

        public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, TNewResult> continuationFunction, TaskContinuationOptions continuationOptions) { throw null; }

        public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, TNewResult> continuationFunction, TaskScheduler scheduler) { throw null; }

        public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, TNewResult> continuationFunction) { throw null; }

        public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, object, TNewResult> continuationFunction, object state, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions, TaskScheduler scheduler) { throw null; }

        public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, object, TNewResult> continuationFunction, object state, CancellationToken cancellationToken) { throw null; }

        public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, object, TNewResult> continuationFunction, object state, TaskContinuationOptions continuationOptions) { throw null; }

        public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, object, TNewResult> continuationFunction, object state, TaskScheduler scheduler) { throw null; }

        public Task<TNewResult> ContinueWith<TNewResult>(Func<Task<TResult>, object, TNewResult> continuationFunction, object state) { throw null; }

        public new Runtime.CompilerServices.TaskAwaiter<TResult> GetAwaiter() { throw null; }
    }

    public partial class UnobservedTaskExceptionEventArgs : EventArgs
    {
        public UnobservedTaskExceptionEventArgs(AggregateException exception) { }

        public AggregateException Exception { get { throw null; } }

        public bool Observed { get { throw null; } }

        public void SetObserved() { }
    }
}