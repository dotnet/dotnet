// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Threading.Tasks.Extensions")]
[assembly: AssemblyDescription("System.Threading.Tasks.Extensions")]
[assembly: AssemblyDefaultAlias("System.Threading.Tasks.Extensions")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.28619.01")]
[assembly: AssemblyInformationalVersion("4.6.28619.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.2.0.1")]




namespace System.Runtime.CompilerServices
{
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Delegate | System.AttributeTargets.Enum | System.AttributeTargets.Interface | System.AttributeTargets.Struct, Inherited=false, AllowMultiple=false)]
    public sealed partial class AsyncMethodBuilderAttribute : System.Attribute
    {
        public AsyncMethodBuilderAttribute(System.Type builderType) { }
        public System.Type BuilderType { get { throw null; } }
    }
    public partial struct AsyncValueTaskMethodBuilder
    {
        private int _dummyPrimitive;
        public System.Threading.Tasks.ValueTask Task { get { throw null; } }
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : System.Runtime.CompilerServices.INotifyCompletion where TStateMachine : System.Runtime.CompilerServices.IAsyncStateMachine { }
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : System.Runtime.CompilerServices.ICriticalNotifyCompletion where TStateMachine : System.Runtime.CompilerServices.IAsyncStateMachine { }
        public static System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder Create() { throw null; }
        public void SetException(System.Exception exception) { }
        public void SetResult() { }
        public void SetStateMachine(System.Runtime.CompilerServices.IAsyncStateMachine stateMachine) { }
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : System.Runtime.CompilerServices.IAsyncStateMachine { }
    }
    public partial struct AsyncValueTaskMethodBuilder<TResult>
    {
        private TResult _result;
        private int _dummyPrimitive;
        public System.Threading.Tasks.ValueTask<TResult> Task { get { throw null; } }
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : System.Runtime.CompilerServices.INotifyCompletion where TStateMachine : System.Runtime.CompilerServices.IAsyncStateMachine { }
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : System.Runtime.CompilerServices.ICriticalNotifyCompletion where TStateMachine : System.Runtime.CompilerServices.IAsyncStateMachine { }
        public static System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder<TResult> Create() { throw null; }
        public void SetException(System.Exception exception) { }
        public void SetResult(TResult result) { }
        public void SetStateMachine(System.Runtime.CompilerServices.IAsyncStateMachine stateMachine) { }
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : System.Runtime.CompilerServices.IAsyncStateMachine { }
    }
    public readonly partial struct ConfiguredValueTaskAwaitable
    {
        private readonly object _dummy;
        public System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable.ConfiguredValueTaskAwaiter GetAwaiter() { throw null; }
        public readonly partial struct ConfiguredValueTaskAwaiter : System.Runtime.CompilerServices.ICriticalNotifyCompletion, System.Runtime.CompilerServices.INotifyCompletion
        {
            private readonly object _dummy;
            public bool IsCompleted { get { throw null; } }
            public void GetResult() { }
            public void OnCompleted(System.Action continuation) { }
            public void UnsafeOnCompleted(System.Action continuation) { }
        }
    }
    public readonly partial struct ConfiguredValueTaskAwaitable<TResult>
    {
        private readonly object _dummy;
        public System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable<TResult>.ConfiguredValueTaskAwaiter GetAwaiter() { throw null; }
        public readonly partial struct ConfiguredValueTaskAwaiter : System.Runtime.CompilerServices.ICriticalNotifyCompletion, System.Runtime.CompilerServices.INotifyCompletion
        {
            private readonly object _dummy;
            public bool IsCompleted { get { throw null; } }
            public TResult GetResult() { throw null; }
            public void OnCompleted(System.Action continuation) { }
            public void UnsafeOnCompleted(System.Action continuation) { }
        }
    }
    public readonly partial struct ValueTaskAwaiter : System.Runtime.CompilerServices.ICriticalNotifyCompletion, System.Runtime.CompilerServices.INotifyCompletion
    {
        private readonly object _dummy;
        public bool IsCompleted { get { throw null; } }
        public void GetResult() { }
        public void OnCompleted(System.Action continuation) { }
        public void UnsafeOnCompleted(System.Action continuation) { }
    }
    public readonly partial struct ValueTaskAwaiter<TResult> : System.Runtime.CompilerServices.ICriticalNotifyCompletion, System.Runtime.CompilerServices.INotifyCompletion
    {
        private readonly object _dummy;
        public bool IsCompleted { get { throw null; } }
        public TResult GetResult() { throw null; }
        public void OnCompleted(System.Action continuation) { }
        public void UnsafeOnCompleted(System.Action continuation) { }
    }
}
namespace System.Threading.Tasks
{
    [System.Runtime.CompilerServices.AsyncMethodBuilderAttribute(typeof(System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder))]
    public readonly partial struct ValueTask : System.IEquatable<System.Threading.Tasks.ValueTask>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ValueTask(System.Threading.Tasks.Sources.IValueTaskSource source, short token) { throw null; }
        public ValueTask(System.Threading.Tasks.Task task) { throw null; }
        public bool IsCanceled { get { throw null; } }
        public bool IsCompleted { get { throw null; } }
        public bool IsCompletedSuccessfully { get { throw null; } }
        public bool IsFaulted { get { throw null; } }
        public System.Threading.Tasks.Task AsTask() { throw null; }
        public System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable ConfigureAwait(bool continueOnCapturedContext) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Threading.Tasks.ValueTask other) { throw null; }
        public System.Runtime.CompilerServices.ValueTaskAwaiter GetAwaiter() { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Threading.Tasks.ValueTask left, System.Threading.Tasks.ValueTask right) { throw null; }
        public static bool operator !=(System.Threading.Tasks.ValueTask left, System.Threading.Tasks.ValueTask right) { throw null; }
        public System.Threading.Tasks.ValueTask Preserve() { throw null; }
    }
    [System.Runtime.CompilerServices.AsyncMethodBuilderAttribute(typeof(System.Runtime.CompilerServices.AsyncValueTaskMethodBuilder<>))]
    public readonly partial struct ValueTask<TResult> : System.IEquatable<System.Threading.Tasks.ValueTask<TResult>>
    {
        internal readonly TResult _result;
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ValueTask(System.Threading.Tasks.Sources.IValueTaskSource<TResult> source, short token) { throw null; }
        public ValueTask(System.Threading.Tasks.Task<TResult> task) { throw null; }
        public ValueTask(TResult result) { throw null; }
        public bool IsCanceled { get { throw null; } }
        public bool IsCompleted { get { throw null; } }
        public bool IsCompletedSuccessfully { get { throw null; } }
        public bool IsFaulted { get { throw null; } }
        public TResult Result { get { throw null; } }
        public System.Threading.Tasks.Task<TResult> AsTask() { throw null; }
        public System.Runtime.CompilerServices.ConfiguredValueTaskAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Threading.Tasks.ValueTask<TResult> other) { throw null; }
        public System.Runtime.CompilerServices.ValueTaskAwaiter<TResult> GetAwaiter() { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Threading.Tasks.ValueTask<TResult> left, System.Threading.Tasks.ValueTask<TResult> right) { throw null; }
        public static bool operator !=(System.Threading.Tasks.ValueTask<TResult> left, System.Threading.Tasks.ValueTask<TResult> right) { throw null; }
        public System.Threading.Tasks.ValueTask<TResult> Preserve() { throw null; }
        public override string ToString() { throw null; }
    }
}
namespace System.Threading.Tasks.Sources
{
    public partial interface IValueTaskSource
    {
        void GetResult(short token);
        System.Threading.Tasks.Sources.ValueTaskSourceStatus GetStatus(short token);
        void OnCompleted(System.Action<object> continuation, object state, short token, System.Threading.Tasks.Sources.ValueTaskSourceOnCompletedFlags flags);
    }
    public partial interface IValueTaskSource<out TResult>
    {
        TResult GetResult(short token);
        System.Threading.Tasks.Sources.ValueTaskSourceStatus GetStatus(short token);
        void OnCompleted(System.Action<object> continuation, object state, short token, System.Threading.Tasks.Sources.ValueTaskSourceOnCompletedFlags flags);
    }
    [System.FlagsAttribute]
    public enum ValueTaskSourceOnCompletedFlags
    {
        FlowExecutionContext = 2,
        None = 0,
        UseSchedulingContext = 1,
    }
    public enum ValueTaskSourceStatus
    {
        Canceled = 3,
        Faulted = 2,
        Pending = 0,
        Succeeded = 1,
    }
}
