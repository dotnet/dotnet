// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Threading.Tasks.Extensions")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("System.Threading.Tasks.Extensions")]
[assembly: System.Reflection.AssemblyFileVersion("4.600.225.16908")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.6.2+6b84308c9ad012f53240d72c1d716d7e42546483")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Threading.Tasks.Extensions")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/maintenance-packages")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.2.1.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false, AllowMultiple = false)]
    public sealed partial class AsyncMethodBuilderAttribute : Attribute
    {
        public AsyncMethodBuilderAttribute(Type builderType) { }

        public Type BuilderType { get { throw null; } }
    }

    public partial struct AsyncValueTaskMethodBuilder
    {
        private int _dummyPrimitive;
        public Threading.Tasks.ValueTask Task { get { throw null; } }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine { }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine { }

        public static AsyncValueTaskMethodBuilder Create() { throw null; }

        public void SetException(Exception exception) { }

        public void SetResult() { }

        public void SetStateMachine(IAsyncStateMachine stateMachine) { }

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine { }
    }

    public partial struct AsyncValueTaskMethodBuilder<TResult>
    {
        private AsyncTaskMethodBuilder<TResult> _methodBuilder;
        private TResult _result;
        private int _dummyPrimitive;
        public Threading.Tasks.ValueTask<TResult> Task { get { throw null; } }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine { }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine { }

        public static AsyncValueTaskMethodBuilder<TResult> Create() { throw null; }

        public void SetException(Exception exception) { }

        public void SetResult(TResult result) { }

        public void SetStateMachine(IAsyncStateMachine stateMachine) { }

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine { }
    }

    public readonly partial struct ConfiguredValueTaskAwaitable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public readonly ConfiguredValueTaskAwaiter GetAwaiter() { throw null; }

        public readonly partial struct ConfiguredValueTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
        {
            private readonly object _dummy;
            private readonly int _dummyPrimitive;
            public bool IsCompleted { get { throw null; } }

            public readonly void GetResult() { }

            public readonly void OnCompleted(Action continuation) { }

            public readonly void UnsafeOnCompleted(Action continuation) { }
        }
    }

    public readonly partial struct ConfiguredValueTaskAwaitable<TResult>
    {
        private readonly Threading.Tasks.ValueTask<TResult> _value;
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public readonly ConfiguredValueTaskAwaiter GetAwaiter() { throw null; }

        public readonly partial struct ConfiguredValueTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
        {
            private readonly Threading.Tasks.ValueTask<TResult> _value;
            private readonly object _dummy;
            private readonly int _dummyPrimitive;
            public bool IsCompleted { get { throw null; } }

            public readonly TResult GetResult() { throw null; }

            public readonly void OnCompleted(Action continuation) { }

            public readonly void UnsafeOnCompleted(Action continuation) { }
        }
    }

    public readonly partial struct ValueTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public bool IsCompleted { get { throw null; } }

        public readonly void GetResult() { }

        public readonly void OnCompleted(Action continuation) { }

        public readonly void UnsafeOnCompleted(Action continuation) { }
    }

    public readonly partial struct ValueTaskAwaiter<TResult> : ICriticalNotifyCompletion, INotifyCompletion
    {
        private readonly Threading.Tasks.ValueTask<TResult> _value;
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public bool IsCompleted { get { throw null; } }

        public readonly TResult GetResult() { throw null; }

        public readonly void OnCompleted(Action continuation) { }

        public readonly void UnsafeOnCompleted(Action continuation) { }
    }
}

namespace System.Threading.Tasks
{
    [Runtime.CompilerServices.AsyncMethodBuilder(typeof(Runtime.CompilerServices.AsyncValueTaskMethodBuilder))]
    public readonly partial struct ValueTask : IEquatable<ValueTask>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ValueTask(Sources.IValueTaskSource source, short token) { }

        public ValueTask(Task task) { }

        public bool IsCanceled { get { throw null; } }

        public bool IsCompleted { get { throw null; } }

        public bool IsCompletedSuccessfully { get { throw null; } }

        public bool IsFaulted { get { throw null; } }

        public readonly Task AsTask() { throw null; }

        public readonly Runtime.CompilerServices.ConfiguredValueTaskAwaitable ConfigureAwait(bool continueOnCapturedContext) { throw null; }

        public override readonly bool Equals(object obj) { throw null; }

        public readonly bool Equals(ValueTask other) { throw null; }

        public readonly Runtime.CompilerServices.ValueTaskAwaiter GetAwaiter() { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(ValueTask left, ValueTask right) { throw null; }

        public static bool operator !=(ValueTask left, ValueTask right) { throw null; }

        public readonly ValueTask Preserve() { throw null; }
    }

    [Runtime.CompilerServices.AsyncMethodBuilder(typeof(Runtime.CompilerServices.AsyncValueTaskMethodBuilder<>))]
    public readonly partial struct ValueTask<TResult> : IEquatable<ValueTask<TResult>>
    {
        private readonly TResult _result;
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ValueTask(TResult result) { }

        public ValueTask(Sources.IValueTaskSource<TResult> source, short token) { }

        public ValueTask(Task<TResult> task) { }

        public bool IsCanceled { get { throw null; } }

        public bool IsCompleted { get { throw null; } }

        public bool IsCompletedSuccessfully { get { throw null; } }

        public bool IsFaulted { get { throw null; } }

        public TResult Result { get { throw null; } }

        public readonly Task<TResult> AsTask() { throw null; }

        public readonly Runtime.CompilerServices.ConfiguredValueTaskAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext) { throw null; }

        public override readonly bool Equals(object obj) { throw null; }

        public readonly bool Equals(ValueTask<TResult> other) { throw null; }

        public readonly Runtime.CompilerServices.ValueTaskAwaiter<TResult> GetAwaiter() { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(ValueTask<TResult> left, ValueTask<TResult> right) { throw null; }

        public static bool operator !=(ValueTask<TResult> left, ValueTask<TResult> right) { throw null; }

        public readonly ValueTask<TResult> Preserve() { throw null; }

        public override readonly string ToString() { throw null; }
    }
}

namespace System.Threading.Tasks.Sources
{
    public partial interface IValueTaskSource
    {
        void GetResult(short token);
        ValueTaskSourceStatus GetStatus(short token);
        void OnCompleted(Action<object> continuation, object state, short token, ValueTaskSourceOnCompletedFlags flags);
    }

    public partial interface IValueTaskSource<out TResult>
    {
        TResult GetResult(short token);
        ValueTaskSourceStatus GetStatus(short token);
        void OnCompleted(Action<object> continuation, object state, short token, ValueTaskSourceOnCompletedFlags flags);
    }

    [Flags]
    public enum ValueTaskSourceOnCompletedFlags
    {
        None = 0,
        UseSchedulingContext = 1,
        FlowExecutionContext = 2
    }

    public enum ValueTaskSourceStatus
    {
        Pending = 0,
        Succeeded = 1,
        Faulted = 2,
        Canceled = 3
    }
}