// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.Bcl.AsyncInterfaces")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Microsoft.Bcl.AsyncInterfaces")]
[assembly: System.Reflection.AssemblyFileVersion("5.0.20.51904")]
[assembly: System.Reflection.AssemblyInformationalVersion("5.0.0+cf258a14b70ad9069470a108f13765e0e5988f51")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Bcl.AsyncInterfaces")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "git://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("5.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System
{
    public partial interface IAsyncDisposable
    {
        Threading.Tasks.ValueTask DisposeAsync();
    }
}

namespace System.Collections.Generic
{
    public partial interface IAsyncEnumerable<out T>
    {
        IAsyncEnumerator<T> GetAsyncEnumerator(Threading.CancellationToken cancellationToken = default);
    }

    public partial interface IAsyncEnumerator<out T> : IAsyncDisposable
    {
        T Current { get; }

        Threading.Tasks.ValueTask<bool> MoveNextAsync();
    }
}

namespace System.Runtime.CompilerServices
{
    public partial struct AsyncIteratorMethodBuilder
    {
        private object _dummy;
        private int _dummyPrimitive;
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine { }
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine { }
        public void Complete() { }
        public static AsyncIteratorMethodBuilder Create() { throw null; }
        public void MoveNext<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine { }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed partial class AsyncIteratorStateMachineAttribute : StateMachineAttribute
    {
        public AsyncIteratorStateMachineAttribute(Type stateMachineType) : base(default!) { }
    }

    public readonly partial struct ConfiguredAsyncDisposable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public readonly ConfiguredValueTaskAwaitable DisposeAsync() { throw null; }
    }

    public readonly partial struct ConfiguredCancelableAsyncEnumerable<T>
    {
        private readonly Collections.Generic.IAsyncEnumerable<T> _enumerable;
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public readonly ConfiguredCancelableAsyncEnumerable<T> ConfigureAwait(bool continueOnCapturedContext) { throw null; }
        public readonly Enumerator GetAsyncEnumerator() { throw null; }
        public readonly ConfiguredCancelableAsyncEnumerable<T> WithCancellation(Threading.CancellationToken cancellationToken) { throw null; }
        public readonly partial struct Enumerator
        {
            private readonly Collections.Generic.IAsyncEnumerator<T> _enumerator;
            private readonly object _dummy;
            private readonly int _dummyPrimitive;
            public T Current { get { throw null; } }

            public readonly ConfiguredValueTaskAwaitable DisposeAsync() { throw null; }
            public readonly ConfiguredValueTaskAwaitable<bool> MoveNextAsync() { throw null; }
        }
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed partial class EnumeratorCancellationAttribute : Attribute
    {
    }
}

namespace System.Threading.Tasks
{
    public static partial class TaskAsyncEnumerableExtensions
    {
        public static Runtime.CompilerServices.ConfiguredAsyncDisposable ConfigureAwait(this IAsyncDisposable source, bool continueOnCapturedContext) { throw null; }
        public static Runtime.CompilerServices.ConfiguredCancelableAsyncEnumerable<T> ConfigureAwait<T>(this Collections.Generic.IAsyncEnumerable<T> source, bool continueOnCapturedContext) { throw null; }
        public static Runtime.CompilerServices.ConfiguredCancelableAsyncEnumerable<T> WithCancellation<T>(this Collections.Generic.IAsyncEnumerable<T> source, CancellationToken cancellationToken) { throw null; }
    }
}

namespace System.Threading.Tasks.Sources
{
    public partial struct ManualResetValueTaskSourceCore<TResult>
    {
        private TResult _result;
        private object _dummy;
        private int _dummyPrimitive;
        public bool RunContinuationsAsynchronously { get { throw null; } set { } }
        public short Version { get { throw null; } }

        public TResult GetResult(short token) { throw null; }
        public ValueTaskSourceStatus GetStatus(short token) { throw null; }
        public void OnCompleted(Action<object> continuation, object state, short token, ValueTaskSourceOnCompletedFlags flags) { }
        public void Reset() { }
        public void SetException(Exception error) { }
        public void SetResult(TResult result) { }
    }
}