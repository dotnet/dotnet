// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.1", FrameworkDisplayName = "")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Threading.Channels")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides types for passing data between producers and consumers.\r\n\r\nCommonly Used Types:\r\nSystem.Threading.Channel\r\nSystem.Threading.Channel<T>")]
[assembly: System.Reflection.AssemblyFileVersion("6.0.21.52210")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.0.0+4822e3c3aa77eb82b2fb33c9321f923cf11ddde6")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Threading.Channels")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Threading.Channels
{
    public enum BoundedChannelFullMode
    {
        Wait = 0,
        DropNewest = 1,
        DropOldest = 2,
        DropWrite = 3
    }

    public sealed partial class BoundedChannelOptions : ChannelOptions
    {
        public BoundedChannelOptions(int capacity) { }

        public int Capacity { get { throw null; } set { } }

        public BoundedChannelFullMode FullMode { get { throw null; } set { } }
    }

    public static partial class Channel
    {
        public static Channel<T> CreateBounded<T>(int capacity) { throw null; }

        public static Channel<T> CreateBounded<T>(BoundedChannelOptions options, Action<T>? itemDropped) { throw null; }

        public static Channel<T> CreateBounded<T>(BoundedChannelOptions options) { throw null; }

        public static Channel<T> CreateUnbounded<T>() { throw null; }

        public static Channel<T> CreateUnbounded<T>(UnboundedChannelOptions options) { throw null; }
    }

    public partial class ChannelClosedException : InvalidOperationException
    {
        public ChannelClosedException() { }

        public ChannelClosedException(Exception? innerException) { }

        protected ChannelClosedException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public ChannelClosedException(string? message, Exception? innerException) { }

        public ChannelClosedException(string? message) { }
    }

    public abstract partial class ChannelOptions
    {
        protected ChannelOptions() { }

        public bool AllowSynchronousContinuations { get { throw null; } set { } }

        public bool SingleReader { get { throw null; } set { } }

        public bool SingleWriter { get { throw null; } set { } }
    }

    public abstract partial class ChannelReader<T>
    {
        protected ChannelReader() { }

        public virtual bool CanCount { get { throw null; } }

        public virtual bool CanPeek { get { throw null; } }

        public virtual Tasks.Task Completion { get { throw null; } }

        public virtual int Count { get { throw null; } }

        public virtual Collections.Generic.IAsyncEnumerable<T> ReadAllAsync(CancellationToken cancellationToken = default) { throw null; }

        public virtual Tasks.ValueTask<T> ReadAsync(CancellationToken cancellationToken = default) { throw null; }

        public virtual bool TryPeek(out T item) { throw null; }

        public abstract bool TryRead(out T item);
        public abstract Tasks.ValueTask<bool> WaitToReadAsync(CancellationToken cancellationToken = default);
    }

    public abstract partial class ChannelWriter<T>
    {
        protected ChannelWriter() { }

        public void Complete(Exception? error = null) { }

        public virtual bool TryComplete(Exception? error = null) { throw null; }

        public abstract bool TryWrite(T item);
        public abstract Tasks.ValueTask<bool> WaitToWriteAsync(CancellationToken cancellationToken = default);
        public virtual Tasks.ValueTask WriteAsync(T item, CancellationToken cancellationToken = default) { throw null; }
    }

    public abstract partial class Channel<T> : Channel<T, T>
    {
        protected Channel() { }
    }

    public abstract partial class Channel<TWrite, TRead>
    {
        protected Channel() { }

        public ChannelReader<TRead> Reader { get { throw null; } protected set { } }

        public ChannelWriter<TWrite> Writer { get { throw null; } protected set { } }

        public static implicit operator ChannelReader<TRead>(Channel<TWrite, TRead> channel) { throw null; }

        public static implicit operator ChannelWriter<TWrite>(Channel<TWrite, TRead> channel) { throw null; }
    }

    public sealed partial class UnboundedChannelOptions : ChannelOptions
    {
        public UnboundedChannelOptions() { }
    }
}