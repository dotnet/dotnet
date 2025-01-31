// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Reflection.AssemblyTitle("System.Threading.Tasks.Extensions")]
[assembly: System.Reflection.AssemblyDescription("System.Threading.Tasks.Extensions")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Threading.Tasks.Extensions")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.6.24705.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.6.24705.01. Commit Hash: 4d1af962ca0fede10beb01d197367c2f90e92c97")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.1.0.0")]
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

    public partial struct ConfiguredValueTaskAwaitable<TResult>
    {
        private Threading.Tasks.ValueTask<TResult> _value;
        private int _dummyPrimitive;
        public ConfiguredValueTaskAwaiter GetAwaiter() { throw null; }

        public partial struct ConfiguredValueTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
        {
            private Threading.Tasks.ValueTask<TResult> _value;
            private int _dummyPrimitive;
            public bool IsCompleted { get { throw null; } }

            public TResult GetResult() { throw null; }

            public void OnCompleted(Action continuation) { }

            public void UnsafeOnCompleted(Action continuation) { }
        }
    }

    public partial struct ValueTaskAwaiter<TResult> : ICriticalNotifyCompletion, INotifyCompletion
    {
        private Threading.Tasks.ValueTask<TResult> _value;
        public bool IsCompleted { get { throw null; } }

        public TResult GetResult() { throw null; }

        public void OnCompleted(Action continuation) { }

        public void UnsafeOnCompleted(Action continuation) { }
    }
}

namespace System.Threading.Tasks
{
    [Runtime.CompilerServices.AsyncMethodBuilder(typeof(Runtime.CompilerServices.AsyncValueTaskMethodBuilder<>))]
    public partial struct ValueTask<TResult> : IEquatable<ValueTask<TResult>>
    {
        private Task<TResult> _task;
        private TResult _result;
        private object _dummy;
        private int _dummyPrimitive;
        public ValueTask(TResult result) { }

        public ValueTask(Task<TResult> task) { }

        public bool IsCanceled { get { throw null; } }

        public bool IsCompleted { get { throw null; } }

        public bool IsCompletedSuccessfully { get { throw null; } }

        public bool IsFaulted { get { throw null; } }

        public TResult Result { get { throw null; } }

        public Task<TResult> AsTask() { throw null; }

        public Runtime.CompilerServices.ConfiguredValueTaskAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext) { throw null; }

        public static Runtime.CompilerServices.AsyncValueTaskMethodBuilder<TResult> CreateAsyncMethodBuilder() { throw null; }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(ValueTask<TResult> other) { throw null; }

        public Runtime.CompilerServices.ValueTaskAwaiter<TResult> GetAwaiter() { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(ValueTask<TResult> left, ValueTask<TResult> right) { throw null; }

        public static bool operator !=(ValueTask<TResult> left, ValueTask<TResult> right) { throw null; }

        public override string ToString() { throw null; }
    }
}