// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Reflection.AssemblyTitle("System.IO.dll")]
[assembly: System.Reflection.AssemblyDescription("System.IO.dll")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.IO.dll")]
[assembly: System.Reflection.AssemblyFileVersion("4.0.30319.17929")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.0.30319.17929")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.IO
{
    public partial class BinaryReader : IDisposable
    {
        public BinaryReader(Stream input, Text.Encoding encoding, bool leaveOpen) { }

        public BinaryReader(Stream input, Text.Encoding encoding) { }

        public BinaryReader(Stream input) { }

        public virtual Stream BaseStream { get { throw null; } }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        protected virtual void FillBuffer(int numBytes) { }

        public virtual int PeekChar() { throw null; }

        public virtual int Read() { throw null; }

        public virtual int Read(byte[] buffer, int index, int count) { throw null; }

        public virtual int Read(char[] buffer, int index, int count) { throw null; }

        protected internal int Read7BitEncodedInt() { throw null; }

        public virtual bool ReadBoolean() { throw null; }

        public virtual byte ReadByte() { throw null; }

        public virtual byte[] ReadBytes(int count) { throw null; }

        public virtual char ReadChar() { throw null; }

        public virtual char[] ReadChars(int count) { throw null; }

        public virtual decimal ReadDecimal() { throw null; }

        public virtual double ReadDouble() { throw null; }

        public virtual short ReadInt16() { throw null; }

        public virtual int ReadInt32() { throw null; }

        public virtual long ReadInt64() { throw null; }

        [CLSCompliant(false)]
        public virtual sbyte ReadSByte() { throw null; }

        public virtual float ReadSingle() { throw null; }

        public virtual string ReadString() { throw null; }

        [CLSCompliant(false)]
        public virtual ushort ReadUInt16() { throw null; }

        [CLSCompliant(false)]
        public virtual uint ReadUInt32() { throw null; }

        [CLSCompliant(false)]
        public virtual ulong ReadUInt64() { throw null; }
    }

    public partial class BinaryWriter : IDisposable
    {
        public static readonly BinaryWriter Null;
        protected Stream OutStream;
        protected BinaryWriter() { }

        public BinaryWriter(Stream output, Text.Encoding encoding, bool leaveOpen) { }

        public BinaryWriter(Stream output, Text.Encoding encoding) { }

        public BinaryWriter(Stream output) { }

        public virtual Stream BaseStream { get { throw null; } }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public virtual void Flush() { }

        public virtual long Seek(int offset, SeekOrigin origin) { throw null; }

        public virtual void Write(bool value) { }

        public virtual void Write(byte value) { }

        public virtual void Write(byte[] buffer, int index, int count) { }

        public virtual void Write(byte[] buffer) { }

        public virtual void Write(char ch) { }

        public virtual void Write(char[] chars, int index, int count) { }

        public virtual void Write(char[] chars) { }

        public virtual void Write(decimal value) { }

        public virtual void Write(double value) { }

        public virtual void Write(short value) { }

        public virtual void Write(int value) { }

        public virtual void Write(long value) { }

        [CLSCompliant(false)]
        public virtual void Write(sbyte value) { }

        public virtual void Write(float value) { }

        public virtual void Write(string value) { }

        [CLSCompliant(false)]
        public virtual void Write(ushort value) { }

        [CLSCompliant(false)]
        public virtual void Write(uint value) { }

        [CLSCompliant(false)]
        public virtual void Write(ulong value) { }

        protected void Write7BitEncodedInt(int value) { }
    }

    public partial class EndOfStreamException : IOException
    {
        public EndOfStreamException() { }

        public EndOfStreamException(string message, Exception innerException) { }

        public EndOfStreamException(string message) { }
    }

    public partial class FileNotFoundException : IOException
    {
        public FileNotFoundException() { }

        public FileNotFoundException(string message, Exception innerException) { }

        public FileNotFoundException(string message, string fileName, Exception innerException) { }

        public FileNotFoundException(string message, string fileName) { }

        public FileNotFoundException(string message) { }

        public string FileName { get { throw null; } }

        public override string Message { get { throw null; } }

        public override string ToString() { throw null; }
    }

    public sealed partial class InvalidDataException : Exception
    {
        public InvalidDataException() { }

        public InvalidDataException(string message, Exception innerException) { }

        public InvalidDataException(string message) { }
    }

    public partial class IOException : Exception
    {
        public IOException() { }

        public IOException(string message, Exception innerException) { }

        public IOException(string message, int hresult) { }

        public IOException(string message) { }
    }

    public partial class MemoryStream : Stream
    {
        public MemoryStream() { }

        public MemoryStream(byte[] buffer, bool writable) { }

        public MemoryStream(byte[] buffer, int index, int count, bool writable) { }

        public MemoryStream(byte[] buffer, int index, int count) { }

        public MemoryStream(byte[] buffer) { }

        public MemoryStream(int capacity) { }

        public override bool CanRead { get { throw null; } }

        public override bool CanSeek { get { throw null; } }

        public override bool CanWrite { get { throw null; } }

        public virtual int Capacity { get { throw null; } set { } }

        public override long Length { get { throw null; } }

        public override long Position { get { throw null; } set { } }

        protected override void Dispose(bool disposing) { }

        public override void Flush() { }

        public override Threading.Tasks.Task FlushAsync(Threading.CancellationToken cancellationToken) { throw null; }

        public override int Read(byte[] buffer, int offset, int count) { throw null; }

        public override Threading.Tasks.Task<int> ReadAsync(byte[] buffer, int offset, int count, Threading.CancellationToken cancellationToken) { throw null; }

        public override int ReadByte() { throw null; }

        public override long Seek(long offset, SeekOrigin loc) { throw null; }

        public override void SetLength(long value) { }

        public virtual byte[] ToArray() { throw null; }

        public override void Write(byte[] buffer, int offset, int count) { }

        public override Threading.Tasks.Task WriteAsync(byte[] buffer, int offset, int count, Threading.CancellationToken cancellationToken) { throw null; }

        public override void WriteByte(byte value) { }

        public virtual void WriteTo(Stream stream) { }
    }

    public enum SeekOrigin
    {
        Begin = 0,
        Current = 1,
        End = 2
    }

    public abstract partial class Stream : IDisposable
    {
        public static readonly Stream Null;
        public abstract bool CanRead { get; }
        public abstract bool CanSeek { get; }

        public virtual bool CanTimeout { get { throw null; } }

        public abstract bool CanWrite { get; }
        public abstract long Length { get; }
        public abstract long Position { get; set; }

        public virtual int ReadTimeout { get { throw null; } set { } }

        public virtual int WriteTimeout { get { throw null; } set { } }

        public void CopyTo(Stream destination, int bufferSize) { }

        public void CopyTo(Stream destination) { }

        public virtual Threading.Tasks.Task CopyToAsync(Stream destination, int bufferSize, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task CopyToAsync(Stream destination, int bufferSize) { throw null; }

        public Threading.Tasks.Task CopyToAsync(Stream destination) { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public abstract void Flush();
        public Threading.Tasks.Task FlushAsync() { throw null; }

        public virtual Threading.Tasks.Task FlushAsync(Threading.CancellationToken cancellationToken) { throw null; }

        public abstract int Read(byte[] buffer, int offset, int count);
        public virtual Threading.Tasks.Task<int> ReadAsync(byte[] buffer, int offset, int count, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task<int> ReadAsync(byte[] buffer, int offset, int count) { throw null; }

        public virtual int ReadByte() { throw null; }

        public abstract long Seek(long offset, SeekOrigin origin);
        public abstract void SetLength(long value);
        public abstract void Write(byte[] buffer, int offset, int count);
        public virtual Threading.Tasks.Task WriteAsync(byte[] buffer, int offset, int count, Threading.CancellationToken cancellationToken) { throw null; }

        public Threading.Tasks.Task WriteAsync(byte[] buffer, int offset, int count) { throw null; }

        public virtual void WriteByte(byte value) { }
    }

    public partial class StreamReader : TextReader
    {
        public new static readonly StreamReader Null;
        public StreamReader(Stream stream, bool detectEncodingFromByteOrderMarks) { }

        public StreamReader(Stream stream, Text.Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize, bool leaveOpen) { }

        public StreamReader(Stream stream, Text.Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize) { }

        public StreamReader(Stream stream, Text.Encoding encoding, bool detectEncodingFromByteOrderMarks) { }

        public StreamReader(Stream stream, Text.Encoding encoding) { }

        public StreamReader(Stream stream) { }

        public virtual Stream BaseStream { get { throw null; } }

        public virtual Text.Encoding CurrentEncoding { get { throw null; } }

        public bool EndOfStream { get { throw null; } }

        public void DiscardBufferedData() { }

        protected override void Dispose(bool disposing) { }

        public override int Peek() { throw null; }

        public override int Read() { throw null; }

        public override int Read(char[] buffer, int index, int count) { throw null; }

        public override Threading.Tasks.Task<int> ReadAsync(char[] buffer, int index, int count) { throw null; }

        public override int ReadBlock(char[] buffer, int index, int count) { throw null; }

        public override Threading.Tasks.Task<int> ReadBlockAsync(char[] buffer, int index, int count) { throw null; }

        public override string ReadLine() { throw null; }

        public override Threading.Tasks.Task<string> ReadLineAsync() { throw null; }

        public override string ReadToEnd() { throw null; }

        public override Threading.Tasks.Task<string> ReadToEndAsync() { throw null; }
    }

    public partial class StreamWriter : TextWriter
    {
        public new static readonly StreamWriter Null;
        public StreamWriter(Stream stream, Text.Encoding encoding, int bufferSize, bool leaveOpen) { }

        public StreamWriter(Stream stream, Text.Encoding encoding, int bufferSize) { }

        public StreamWriter(Stream stream, Text.Encoding encoding) { }

        public StreamWriter(Stream stream) { }

        public virtual bool AutoFlush { get { throw null; } set { } }

        public virtual Stream BaseStream { get { throw null; } }

        public override Text.Encoding Encoding { get { throw null; } }

        protected override void Dispose(bool disposing) { }

        public override void Flush() { }

        public override Threading.Tasks.Task FlushAsync() { throw null; }

        public override void Write(char value) { }

        public override void Write(char[] buffer, int index, int count) { }

        public override void Write(char[] buffer) { }

        public override void Write(string value) { }

        public override Threading.Tasks.Task WriteAsync(char value) { throw null; }

        public override Threading.Tasks.Task WriteAsync(char[] buffer, int index, int count) { throw null; }

        public override Threading.Tasks.Task WriteAsync(string value) { throw null; }

        public override Threading.Tasks.Task WriteLineAsync() { throw null; }

        public override Threading.Tasks.Task WriteLineAsync(char value) { throw null; }

        public override Threading.Tasks.Task WriteLineAsync(char[] buffer, int index, int count) { throw null; }

        public override Threading.Tasks.Task WriteLineAsync(string value) { throw null; }
    }

    public partial class StringReader : TextReader
    {
        public StringReader(string s) { }

        protected override void Dispose(bool disposing) { }

        public override int Peek() { throw null; }

        public override int Read() { throw null; }

        public override int Read(char[] buffer, int index, int count) { throw null; }

        public override Threading.Tasks.Task<int> ReadAsync(char[] buffer, int index, int count) { throw null; }

        public override Threading.Tasks.Task<int> ReadBlockAsync(char[] buffer, int index, int count) { throw null; }

        public override string ReadLine() { throw null; }

        public override Threading.Tasks.Task<string> ReadLineAsync() { throw null; }

        public override string ReadToEnd() { throw null; }

        public override Threading.Tasks.Task<string> ReadToEndAsync() { throw null; }
    }

    public partial class StringWriter : TextWriter
    {
        public StringWriter() { }

        public StringWriter(IFormatProvider formatProvider) { }

        public StringWriter(Text.StringBuilder sb, IFormatProvider formatProvider) { }

        public StringWriter(Text.StringBuilder sb) { }

        public override Text.Encoding Encoding { get { throw null; } }

        protected override void Dispose(bool disposing) { }

        public override Threading.Tasks.Task FlushAsync() { throw null; }

        public virtual Text.StringBuilder GetStringBuilder() { throw null; }

        public override string ToString() { throw null; }

        public override void Write(char value) { }

        public override void Write(char[] buffer, int index, int count) { }

        public override void Write(string value) { }

        public override Threading.Tasks.Task WriteAsync(char value) { throw null; }

        public override Threading.Tasks.Task WriteAsync(char[] buffer, int index, int count) { throw null; }

        public override Threading.Tasks.Task WriteAsync(string value) { throw null; }

        public override Threading.Tasks.Task WriteLineAsync(char value) { throw null; }

        public override Threading.Tasks.Task WriteLineAsync(char[] buffer, int index, int count) { throw null; }

        public override Threading.Tasks.Task WriteLineAsync(string value) { throw null; }
    }

    public abstract partial class TextReader : IDisposable
    {
        public static readonly TextReader Null;
        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public virtual int Peek() { throw null; }

        public virtual int Read() { throw null; }

        public virtual int Read(char[] buffer, int index, int count) { throw null; }

        public virtual Threading.Tasks.Task<int> ReadAsync(char[] buffer, int index, int count) { throw null; }

        public virtual int ReadBlock(char[] buffer, int index, int count) { throw null; }

        public virtual Threading.Tasks.Task<int> ReadBlockAsync(char[] buffer, int index, int count) { throw null; }

        public virtual string ReadLine() { throw null; }

        public virtual Threading.Tasks.Task<string> ReadLineAsync() { throw null; }

        public virtual string ReadToEnd() { throw null; }

        public virtual Threading.Tasks.Task<string> ReadToEndAsync() { throw null; }
    }

    public abstract partial class TextWriter : IDisposable
    {
        protected char[] CoreNewLine;
        public static readonly TextWriter Null;
        protected TextWriter() { }

        protected TextWriter(IFormatProvider formatProvider) { }

        public abstract Text.Encoding Encoding { get; }

        public virtual IFormatProvider FormatProvider { get { throw null; } }

        public virtual string NewLine { get { throw null; } set { } }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public virtual void Flush() { }

        public virtual Threading.Tasks.Task FlushAsync() { throw null; }

        public virtual void Write(bool value) { }

        public abstract void Write(char value);
        public virtual void Write(char[] buffer, int index, int count) { }

        public virtual void Write(char[] buffer) { }

        public virtual void Write(decimal value) { }

        public virtual void Write(double value) { }

        public virtual void Write(int value) { }

        public virtual void Write(long value) { }

        public virtual void Write(object value) { }

        public virtual void Write(float value) { }

        public virtual void Write(string format, params object[] arg) { }

        public virtual void Write(string value) { }

        [CLSCompliant(false)]
        public virtual void Write(uint value) { }

        [CLSCompliant(false)]
        public virtual void Write(ulong value) { }

        public virtual Threading.Tasks.Task WriteAsync(char value) { throw null; }

        public virtual Threading.Tasks.Task WriteAsync(char[] buffer, int index, int count) { throw null; }

        public Threading.Tasks.Task WriteAsync(char[] buffer) { throw null; }

        public virtual Threading.Tasks.Task WriteAsync(string value) { throw null; }

        public virtual void WriteLine() { }

        public virtual void WriteLine(bool value) { }

        public virtual void WriteLine(char value) { }

        public virtual void WriteLine(char[] buffer, int index, int count) { }

        public virtual void WriteLine(char[] buffer) { }

        public virtual void WriteLine(decimal value) { }

        public virtual void WriteLine(double value) { }

        public virtual void WriteLine(int value) { }

        public virtual void WriteLine(long value) { }

        public virtual void WriteLine(object value) { }

        public virtual void WriteLine(float value) { }

        public virtual void WriteLine(string format, params object[] arg) { }

        public virtual void WriteLine(string value) { }

        [CLSCompliant(false)]
        public virtual void WriteLine(uint value) { }

        [CLSCompliant(false)]
        public virtual void WriteLine(ulong value) { }

        public virtual Threading.Tasks.Task WriteLineAsync() { throw null; }

        public virtual Threading.Tasks.Task WriteLineAsync(char value) { throw null; }

        public virtual Threading.Tasks.Task WriteLineAsync(char[] buffer, int index, int count) { throw null; }

        public Threading.Tasks.Task WriteLineAsync(char[] buffer) { throw null; }

        public virtual Threading.Tasks.Task WriteLineAsync(string value) { throw null; }
    }
}