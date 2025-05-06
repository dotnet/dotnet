// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("System.IO.Pipelines.Tests, PublicKey=00240000048000009400000006020000002400005253413100040000010001004b86c4cb78549b34bab61a3b1800e23bfeb5b3ec390074041536a7e3cbd97f5f04cf0f857155a8928eaa29ebfd11cfbbad3ba70efea7bda3226c6a8d370a4cd303f714486b6ebc225985a638471e6ef571cc92a4613c00b8fa65d61ccee0cbe5f36330c9a01f4183559f1bef24cc2917c6d913e3a541333a1d05d9bed22b38cb")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.IO.Pipelines")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("System.IO.Pipelines")]
[assembly: System.Reflection.AssemblyFileVersion("5.0.120.57516")]
[assembly: System.Reflection.AssemblyInformationalVersion("5.0.1+b02e13abab3d6f499751af2ad1ae6333fb4e633d")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.IO.Pipelines")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "git://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("5.0.0.1")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.IO.Pipelines
{
    public partial struct FlushResult
    {
        private int _dummyPrimitive;
        public FlushResult(bool isCanceled, bool isCompleted) { }

        public bool IsCanceled { get { throw null; } }

        public bool IsCompleted { get { throw null; } }
    }

    public partial interface IDuplexPipe
    {
        PipeReader Input { get; }

        PipeWriter Output { get; }
    }

    public sealed partial class Pipe
    {
        public Pipe() { }

        public Pipe(PipeOptions options) { }

        public PipeReader Reader { get { throw null; } }

        public PipeWriter Writer { get { throw null; } }

        public void Reset() { }
    }

    public partial class PipeOptions
    {
        public PipeOptions(Buffers.MemoryPool<byte>? pool = null, PipeScheduler? readerScheduler = null, PipeScheduler? writerScheduler = null, long pauseWriterThreshold = -1, long resumeWriterThreshold = -1, int minimumSegmentSize = -1, bool useSynchronizationContext = true) { }

        public static PipeOptions Default { get { throw null; } }

        public int MinimumSegmentSize { get { throw null; } }

        public long PauseWriterThreshold { get { throw null; } }

        public Buffers.MemoryPool<byte> Pool { get { throw null; } }

        public PipeScheduler ReaderScheduler { get { throw null; } }

        public long ResumeWriterThreshold { get { throw null; } }

        public bool UseSynchronizationContext { get { throw null; } }

        public PipeScheduler WriterScheduler { get { throw null; } }
    }

    public abstract partial class PipeReader
    {
        public abstract void AdvanceTo(SequencePosition consumed, SequencePosition examined);
        public abstract void AdvanceTo(SequencePosition consumed);
        public virtual Stream AsStream(bool leaveOpen = false) { throw null; }

        public abstract void CancelPendingRead();
        public abstract void Complete(Exception? exception = null);
        public virtual Threading.Tasks.ValueTask CompleteAsync(Exception? exception = null) { throw null; }

        public virtual Threading.Tasks.Task CopyToAsync(PipeWriter destination, Threading.CancellationToken cancellationToken = default) { throw null; }

        public virtual Threading.Tasks.Task CopyToAsync(Stream destination, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static PipeReader Create(Stream stream, StreamPipeReaderOptions? readerOptions = null) { throw null; }

        [Obsolete("OnWriterCompleted may not be invoked on all implementations of PipeReader. This will be removed in a future release.")]
        public virtual void OnWriterCompleted(Action<Exception?, object?> callback, object? state) { }

        public abstract Threading.Tasks.ValueTask<ReadResult> ReadAsync(Threading.CancellationToken cancellationToken = default);
        public abstract bool TryRead(out ReadResult result);
    }

    public abstract partial class PipeScheduler
    {
        public static PipeScheduler Inline { get { throw null; } }

        public static PipeScheduler ThreadPool { get { throw null; } }

        public abstract void Schedule(Action<object?> action, object? state);
    }

    public abstract partial class PipeWriter : Buffers.IBufferWriter<byte>
    {
        public abstract void Advance(int bytes);
        public virtual Stream AsStream(bool leaveOpen = false) { throw null; }

        public abstract void CancelPendingFlush();
        public abstract void Complete(Exception? exception = null);
        public virtual Threading.Tasks.ValueTask CompleteAsync(Exception? exception = null) { throw null; }

        protected internal virtual Threading.Tasks.Task CopyFromAsync(Stream source, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static PipeWriter Create(Stream stream, StreamPipeWriterOptions? writerOptions = null) { throw null; }

        public abstract Threading.Tasks.ValueTask<FlushResult> FlushAsync(Threading.CancellationToken cancellationToken = default);
        public abstract Memory<byte> GetMemory(int sizeHint = 0);
        public abstract Span<byte> GetSpan(int sizeHint = 0);
        [Obsolete("OnReaderCompleted may not be invoked on all implementations of PipeWriter. This will be removed in a future release.")]
        public virtual void OnReaderCompleted(Action<Exception?, object?> callback, object? state) { }

        public virtual Threading.Tasks.ValueTask<FlushResult> WriteAsync(ReadOnlyMemory<byte> source, Threading.CancellationToken cancellationToken = default) { throw null; }
    }

    public readonly partial struct ReadResult
    {
        private readonly int _dummyPrimitive;
        public ReadResult(Buffers.ReadOnlySequence<byte> buffer, bool isCanceled, bool isCompleted) { }

        public Buffers.ReadOnlySequence<byte> Buffer { get { throw null; } }

        public bool IsCanceled { get { throw null; } }

        public bool IsCompleted { get { throw null; } }
    }

    public static partial class StreamPipeExtensions
    {
        public static Threading.Tasks.Task CopyToAsync(this Stream source, PipeWriter destination, Threading.CancellationToken cancellationToken = default) { throw null; }
    }

    public partial class StreamPipeReaderOptions
    {
        public StreamPipeReaderOptions(Buffers.MemoryPool<byte>? pool = null, int bufferSize = -1, int minimumReadSize = -1, bool leaveOpen = false) { }

        public int BufferSize { get { throw null; } }

        public bool LeaveOpen { get { throw null; } }

        public int MinimumReadSize { get { throw null; } }

        public Buffers.MemoryPool<byte> Pool { get { throw null; } }
    }

    public partial class StreamPipeWriterOptions
    {
        public StreamPipeWriterOptions(Buffers.MemoryPool<byte>? pool = null, int minimumBufferSize = -1, bool leaveOpen = false) { }

        public bool LeaveOpen { get { throw null; } }

        public int MinimumBufferSize { get { throw null; } }

        public Buffers.MemoryPool<byte> Pool { get { throw null; } }
    }
}