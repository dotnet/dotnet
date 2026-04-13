// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("System.IO.Pipelines.Tests, PublicKey=002400000480000094000000060200000024000052534131000400000100010015c01ae1f50e8cc09ba9eac9147cf8fd9fce2cfe9f8dce4f7301c4132ca9fb50ce8cbf1df4dc18dd4d210e4345c744ecb3365ed327efdbc52603faa5e21daa11234c8c4a73e51f03bf192544581ebe107adee3a34928e39d04e524a9ce729d5090bfd7dad9d10c722c0def9ccc08ff0a03790e48bcd1f9b6c476063e1966a1c4")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyTitle("System.IO.Pipelines")]
[assembly: System.Reflection.AssemblyDescription("System.IO.Pipelines")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.IO.Pipelines")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.6.27129.04")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.6.27129.04 @BuiltBy: dlab14-DDVSOWINAGE083 @Branch: release/2.1-MSRC @SrcCode: https://github.com/dotnet/corefx/tree/99ce22c306b07f99ddae60f443d23a983ae78f7b")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.1")]
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
        public PipeOptions(Buffers.MemoryPool<byte> pool = null, PipeScheduler readerScheduler = null, PipeScheduler writerScheduler = null, long pauseWriterThreshold = 32768, long resumeWriterThreshold = 16384, int minimumSegmentSize = 2048, bool useSynchronizationContext = true) { }

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
        public abstract void CancelPendingRead();
        public abstract void Complete(Exception exception = null);
        public abstract void OnWriterCompleted(Action<Exception, object> callback, object state);
        public abstract Threading.Tasks.ValueTask<ReadResult> ReadAsync(Threading.CancellationToken cancellationToken = default);
        public abstract bool TryRead(out ReadResult result);
    }

    public abstract partial class PipeScheduler
    {
        public static PipeScheduler Inline { get { throw null; } }

        public static PipeScheduler ThreadPool { get { throw null; } }

        public abstract void Schedule(Action<object> action, object state);
    }

    public abstract partial class PipeWriter : Buffers.IBufferWriter<byte>
    {
        public abstract void Advance(int bytes);
        public abstract void CancelPendingFlush();
        public abstract void Complete(Exception exception = null);
        public abstract Threading.Tasks.ValueTask<FlushResult> FlushAsync(Threading.CancellationToken cancellationToken = default);
        public abstract Memory<byte> GetMemory(int sizeHint = 0);
        public abstract Span<byte> GetSpan(int sizeHint = 0);
        public abstract void OnReaderCompleted(Action<Exception, object> callback, object state);
        public virtual Threading.Tasks.ValueTask<FlushResult> WriteAsync(ReadOnlyMemory<byte> source, Threading.CancellationToken cancellationToken = default) { throw null; }
    }

    public partial struct ReadResult
    {
        private int _dummyPrimitive;
        public ReadResult(Buffers.ReadOnlySequence<byte> buffer, bool isCanceled, bool isCompleted) { }

        public Buffers.ReadOnlySequence<byte> Buffer { get { throw null; } }

        public bool IsCanceled { get { throw null; } }

        public bool IsCompleted { get { throw null; } }
    }
}