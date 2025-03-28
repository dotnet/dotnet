// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
    // This abstract base class represents a writer that can write a sequential
    // stream of characters. A subclass must minimally implement the
    // Write(char) method.
    //
    // This class is intended for character output, not bytes.
    // There are methods on the Stream class for writing bytes.
    public abstract partial class TextWriter : MarshalByRefObject, IDisposable, IAsyncDisposable
    {
        public static readonly TextWriter Null = new NullTextWriter();

        // We don't want to allocate on every TextWriter creation, so cache the char array.
        private static readonly char[] s_coreNewLine = Environment.NewLineConst.ToCharArray();

        /// <summary>
        /// This is the 'NewLine' property expressed as a char[].
        /// It is exposed to subclasses as a protected field for read-only
        /// purposes.  You should only modify it by using the 'NewLine' property.
        /// In particular you should never modify the elements of the array
        /// as they are shared among many instances of TextWriter.
        /// </summary>
        protected char[] CoreNewLine = s_coreNewLine;
        private string CoreNewLineStr = Environment.NewLineConst;

        // Can be null - if so, ask for the Thread's CurrentCulture every time.
        private readonly IFormatProvider? _internalFormatProvider;

        protected TextWriter()
        {
        }

        protected TextWriter(IFormatProvider? formatProvider)
        {
            _internalFormatProvider = formatProvider;
        }

        public virtual IFormatProvider FormatProvider
            => _internalFormatProvider ?? CultureInfo.CurrentCulture;

        public virtual void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual ValueTask DisposeAsync()
        {
            try
            {
                Dispose();
                return default;
            }
            catch (Exception exc)
            {
                return ValueTask.FromException(exc);
            }
        }

        // Clears all buffers for this TextWriter and causes any buffered data to be
        // written to the underlying device. This default method is empty, but
        // descendant classes can override the method to provide the appropriate
        // functionality.
        public virtual void Flush()
        {
        }

        public abstract Encoding Encoding
        {
            get;
        }

        /// <summary>
        /// Returns the line terminator string used by this TextWriter. The default line
        /// terminator string is Environment.NewLine, which is platform specific.
        /// On Windows this is a carriage return followed by a line feed ("\r\n").
        /// On OSX and Linux this is a line feed ("\n").
        /// </summary>
        /// <remarks>
        /// The line terminator string is written to the text stream whenever one of the
        /// WriteLine methods are called. In order for text written by
        /// the TextWriter to be readable by a TextReader, only one of the following line
        /// terminator strings should be used: "\r", "\n", or "\r\n".
        /// </remarks>
        [AllowNull]
        public virtual string NewLine
        {
            get => CoreNewLineStr;
            set
            {
                value ??= Environment.NewLineConst;

                CoreNewLineStr = value;
                CoreNewLine = value.ToCharArray();
            }
        }

        // Writes a character to the text stream. This default method is empty,
        // but descendant classes can override the method to provide the
        // appropriate functionality.
        //
        public virtual void Write(char value)
        {
        }

        // Writes a character array to the text stream. This default method calls
        // Write(char) for each of the characters in the character array.
        // If the character array is null, nothing is written.
        //
        public virtual void Write(char[]? buffer)
        {
            if (buffer != null)
            {
                Write(buffer, 0, buffer.Length);
            }
        }

        // Writes a range of a character array to the text stream. This method will
        // write count characters of data into this TextWriter from the
        // buffer character array starting at position index.
        //
        public virtual void Write(char[] buffer, int index, int count)
        {
            ArgumentNullException.ThrowIfNull(buffer);

            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            if (buffer.Length - index < count)
            {
                throw new ArgumentException(SR.Argument_InvalidOffLen);
            }

            for (int i = 0; i < count; i++) Write(buffer[index + i]);
        }

        // Writes a span of characters to the text stream.
        //
        public virtual void Write(ReadOnlySpan<char> buffer)
        {
            char[] array = ArrayPool<char>.Shared.Rent(buffer.Length);

            try
            {
                buffer.CopyTo(new Span<char>(array));
                Write(array, 0, buffer.Length);
            }
            finally
            {
                ArrayPool<char>.Shared.Return(array);
            }
        }

        // Writes the text representation of a boolean to the text stream. This
        // method outputs either bool.TrueString or bool.FalseString.
        //
        public virtual void Write(bool value)
        {
            Write(value ? "True" : "False");
        }

        // Writes the text representation of an integer to the text stream. The
        // text representation of the given value is produced by calling the
        // int.ToString() method.
        //
        public virtual void Write(int value)
        {
            Write(value.ToString(FormatProvider));
        }

        // Writes the text representation of an integer to the text stream. The
        // text representation of the given value is produced by calling the
        // uint.ToString() method.
        //
        [CLSCompliant(false)]
        public virtual void Write(uint value)
        {
            Write(value.ToString(FormatProvider));
        }

        // Writes the text representation of a long to the text stream. The
        // text representation of the given value is produced by calling the
        // long.ToString() method.
        //
        public virtual void Write(long value)
        {
            Write(value.ToString(FormatProvider));
        }

        // Writes the text representation of an unsigned long to the text
        // stream. The text representation of the given value is produced
        // by calling the ulong.ToString() method.
        //
        [CLSCompliant(false)]
        public virtual void Write(ulong value)
        {
            Write(value.ToString(FormatProvider));
        }

        // Writes the text representation of a float to the text stream. The
        // text representation of the given value is produced by calling the
        // float.ToString(float) method.
        //
        public virtual void Write(float value)
        {
            Write(value.ToString(FormatProvider));
        }

        // Writes the text representation of a double to the text stream. The
        // text representation of the given value is produced by calling the
        // double.ToString(double) method.
        //
        public virtual void Write(double value)
        {
            Write(value.ToString(FormatProvider));
        }

        public virtual void Write(decimal value)
        {
            Write(value.ToString(FormatProvider));
        }

        // Writes a string to the text stream. If the given string is null, nothing
        // is written to the text stream.
        //
        public virtual void Write(string? value)
        {
            if (value != null)
            {
                Write(value.ToCharArray());
            }
        }

        // Writes the text representation of an object to the text stream. If the
        // given object is null, nothing is written to the text stream.
        // Otherwise, the object's ToString method is called to produce the
        // string representation, and the resulting string is then written to the
        // output stream.
        //
        public virtual void Write(object? value)
        {
            if (value != null)
            {
                if (value is IFormattable f)
                {
                    Write(f.ToString(null, FormatProvider));
                }
                else
                    Write(value.ToString());
            }
        }

        /// <summary>
        /// Equivalent to Write(stringBuilder.ToString()) however it uses the
        /// StringBuilder.GetChunks() method to avoid creating the intermediate string
        /// </summary>
        /// <param name="value">The string (as a StringBuilder) to write to the stream</param>
        public virtual void Write(StringBuilder? value)
        {
            if (value != null)
            {
                foreach (ReadOnlyMemory<char> chunk in value.GetChunks())
                    Write(chunk.Span);
            }
        }

        // Writes out a formatted string.  Uses the same semantics as
        // string.Format.
        //
        public virtual void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0)
        {
            Write(string.Format(FormatProvider, format, arg0));
        }

        // Writes out a formatted string.  Uses the same semantics as
        // string.Format.
        //
        public virtual void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1)
        {
            Write(string.Format(FormatProvider, format, arg0, arg1));
        }

        // Writes out a formatted string.  Uses the same semantics as
        // string.Format.
        //
        public virtual void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2)
        {
            Write(string.Format(FormatProvider, format, arg0, arg1, arg2));
        }

        // Writes out a formatted string.  Uses the same semantics as
        // string.Format.
        //
        public virtual void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] arg)
        {
            Write(string.Format(FormatProvider, format, arg));
        }

        /// <summary>
        /// Writes a formatted string to the text stream, using the same semantics as <see cref="string.Format(string, ReadOnlySpan{object?})"/>.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">An object span that contains zero or more objects to format and write.</param>
        public virtual void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params ReadOnlySpan<object?> arg)
        {
            Write(string.Format(FormatProvider, format, arg));
        }

        // Writes a line terminator to the text stream. The default line terminator
        // is Environment.NewLine, but this value can be changed by setting the NewLine property.
        //
        public virtual void WriteLine()
        {
            Write(CoreNewLine);
        }

        // Writes a character followed by a line terminator to the text stream.
        //
        public virtual void WriteLine(char value)
        {
            Write(value);
            WriteLine();
        }

        // Writes an array of characters followed by a line terminator to the text
        // stream.
        //
        public virtual void WriteLine(char[]? buffer)
        {
            Write(buffer);
            WriteLine();
        }

        // Writes a range of a character array followed by a line terminator to the
        // text stream.
        //
        public virtual void WriteLine(char[] buffer, int index, int count)
        {
            Write(buffer, index, count);
            WriteLine();
        }

        public virtual void WriteLine(ReadOnlySpan<char> buffer)
        {
            char[] array = ArrayPool<char>.Shared.Rent(buffer.Length);

            try
            {
                buffer.CopyTo(new Span<char>(array));
                WriteLine(array, 0, buffer.Length);
            }
            finally
            {
                ArrayPool<char>.Shared.Return(array);
            }
        }

        // Writes the text representation of a boolean followed by a line
        // terminator to the text stream.
        //
        public virtual void WriteLine(bool value)
        {
            Write(value);
            WriteLine();
        }

        // Writes the text representation of an integer followed by a line
        // terminator to the text stream.
        //
        public virtual void WriteLine(int value)
        {
            Write(value);
            WriteLine();
        }

        // Writes the text representation of an unsigned integer followed by
        // a line terminator to the text stream.
        //
        [CLSCompliant(false)]
        public virtual void WriteLine(uint value)
        {
            Write(value);
            WriteLine();
        }

        // Writes the text representation of a long followed by a line terminator
        // to the text stream.
        //
        public virtual void WriteLine(long value)
        {
            Write(value);
            WriteLine();
        }

        // Writes the text representation of an unsigned long followed by
        // a line terminator to the text stream.
        //
        [CLSCompliant(false)]
        public virtual void WriteLine(ulong value)
        {
            Write(value);
            WriteLine();
        }

        // Writes the text representation of a float followed by a line terminator
        // to the text stream.
        //
        public virtual void WriteLine(float value)
        {
            Write(value);
            WriteLine();
        }

        // Writes the text representation of a double followed by a line terminator
        // to the text stream.
        //
        public virtual void WriteLine(double value)
        {
            Write(value);
            WriteLine();
        }

        public virtual void WriteLine(decimal value)
        {
            Write(value);
            WriteLine();
        }

        // Writes a string followed by a line terminator to the text stream.
        //
        public virtual void WriteLine(string? value)
        {
            if (value != null)
            {
                Write(value);
            }
            Write(CoreNewLineStr);
        }

        /// <summary>
        /// Equivalent to WriteLine(stringBuilder.ToString()) however it uses the
        /// StringBuilder.GetChunks() method to avoid creating the intermediate string
        /// </summary>
        public virtual void WriteLine(StringBuilder? value)
        {
            Write(value);
            WriteLine();
        }

        // Writes the text representation of an object followed by a line
        // terminator to the text stream.
        //
        public virtual void WriteLine(object? value)
        {
            if (value == null)
            {
                WriteLine();
            }
            else
            {
                // Call WriteLine(value.ToString), not Write(Object), WriteLine().
                // This makes calls to WriteLine(Object) atomic.
                if (value is IFormattable f)
                {
                    WriteLine(f.ToString(null, FormatProvider));
                }
                else
                {
                    WriteLine(value.ToString());
                }
            }
        }

        // Writes out a formatted string and a new line.  Uses the same
        // semantics as string.Format.
        //
        public virtual void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0)
        {
            WriteLine(string.Format(FormatProvider, format, arg0));
        }

        // Writes out a formatted string and a new line.  Uses the same
        // semantics as string.Format.
        //
        public virtual void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1)
        {
            WriteLine(string.Format(FormatProvider, format, arg0, arg1));
        }

        // Writes out a formatted string and a new line.  Uses the same
        // semantics as string.Format.
        //
        public virtual void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2)
        {
            WriteLine(string.Format(FormatProvider, format, arg0, arg1, arg2));
        }

        // Writes out a formatted string and a new line.  Uses the same
        // semantics as string.Format.
        //
        public virtual void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] arg)
        {
            WriteLine(string.Format(FormatProvider, format, arg));
        }

        /// <summary>
        /// Writes out a formatted string and a new line to the text stream, using the same semantics as <see cref="string.Format(string, ReadOnlySpan{object?})"/>.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">An object span that contains zero or more objects to format and write.</param>
        public virtual void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params ReadOnlySpan<object?> arg)
        {
            WriteLine(string.Format(FormatProvider, format, arg));
        }

        #region Task based Async APIs
        public virtual Task WriteAsync(char value) =>
            Task.Factory.StartNew(static state =>
            {
                var t = (TupleSlim<TextWriter, char>)state!;
                t.Item1.Write(t.Item2);
            }, new TupleSlim<TextWriter, char>(this, value), CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

        public virtual Task WriteAsync(string? value) =>
            Task.Factory.StartNew(static state =>
            {
                var t = (TupleSlim<TextWriter, string?>)state!;
                t.Item1.Write(t.Item2);
            }, new TupleSlim<TextWriter, string?>(this, value), CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

        /// <summary>
        /// Equivalent to WriteAsync(stringBuilder.ToString()) however it uses the
        /// StringBuilder.GetChunks() method to avoid creating the intermediate string
        /// </summary>
        /// <param name="value">The string (as a StringBuilder) to write to the stream</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public virtual Task WriteAsync(StringBuilder? value, CancellationToken cancellationToken = default)
        {
            return
                cancellationToken.IsCancellationRequested ? Task.FromCanceled(cancellationToken) :
                value == null ? Task.CompletedTask :
                WriteAsyncCore(value, cancellationToken);

            async Task WriteAsyncCore(StringBuilder sb, CancellationToken ct)
            {
                foreach (ReadOnlyMemory<char> chunk in sb.GetChunks())
                {
                    await WriteAsync(chunk, ct).ConfigureAwait(false);
                }
            }
        }

        public Task WriteAsync(char[]? buffer)
        {
            if (buffer == null)
            {
                return Task.CompletedTask;
            }

            return WriteAsync(buffer, 0, buffer.Length);
        }

        public virtual Task WriteAsync(char[] buffer, int index, int count) =>
            Task.Factory.StartNew(static state =>
            {
                var t = (TupleSlim<TextWriter, char[], int, int>)state!;
                t.Item1.Write(t.Item2, t.Item3, t.Item4);
            }, new TupleSlim<TextWriter, char[], int, int>(this, buffer, index, count), CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

        public virtual Task WriteAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default) =>
            cancellationToken.IsCancellationRequested ? Task.FromCanceled(cancellationToken) :
            MemoryMarshal.TryGetArray(buffer, out ArraySegment<char> array) ?
                WriteAsync(array.Array!, array.Offset, array.Count) :
                Task.Factory.StartNew(static state =>
                {
                    var t = (TupleSlim<TextWriter, ReadOnlyMemory<char>>)state!;
                    t.Item1.Write(t.Item2.Span);
                }, new TupleSlim<TextWriter, ReadOnlyMemory<char>>(this, buffer), cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

        public virtual Task WriteLineAsync(char value) =>
            Task.Factory.StartNew(static state =>
            {
                var t = (TupleSlim<TextWriter, char>)state!;
                t.Item1.WriteLine(t.Item2);
            }, new TupleSlim<TextWriter, char>(this, value), CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

        public virtual Task WriteLineAsync(string? value) =>
            Task.Factory.StartNew(static state =>
            {
                var t = (TupleSlim<TextWriter, string?>)state!;
                t.Item1.WriteLine(t.Item2);
            }, new TupleSlim<TextWriter, string?>(this, value), CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

        /// <summary>
        /// Equivalent to WriteLineAsync(stringBuilder.ToString()) however it uses the
        /// StringBuilder.GetChunks() method to avoid creating the intermediate string
        /// </summary>
        /// <param name="value">The string (as a StringBuilder) to write to the stream</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public virtual Task WriteLineAsync(StringBuilder? value, CancellationToken cancellationToken = default)
        {
            return
                cancellationToken.IsCancellationRequested ? Task.FromCanceled(cancellationToken) :
                value == null ? WriteAsync(CoreNewLine, cancellationToken) :
                WriteLineAsyncCore(value, cancellationToken);

            async Task WriteLineAsyncCore(StringBuilder sb, CancellationToken ct)
            {
                foreach (ReadOnlyMemory<char> chunk in sb.GetChunks())
                {
                    await WriteAsync(chunk, ct).ConfigureAwait(false);
                }
                await WriteAsync(CoreNewLine, ct).ConfigureAwait(false);
            }
        }

        public Task WriteLineAsync(char[]? buffer)
        {
            if (buffer == null)
            {
                return WriteLineAsync();
            }

            return WriteLineAsync(buffer, 0, buffer.Length);
        }

        public virtual Task WriteLineAsync(char[] buffer, int index, int count) =>
            Task.Factory.StartNew(static state =>
            {
                var t = (TupleSlim<TextWriter, char[], int, int>)state!;
                t.Item1.WriteLine(t.Item2, t.Item3, t.Item4);
            }, new TupleSlim<TextWriter, char[], int, int>(this, buffer, index, count), CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

        public virtual Task WriteLineAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default) =>
            cancellationToken.IsCancellationRequested ? Task.FromCanceled(cancellationToken) :
            MemoryMarshal.TryGetArray(buffer, out ArraySegment<char> array) ?
                WriteLineAsync(array.Array!, array.Offset, array.Count) :
                Task.Factory.StartNew(static state =>
                {
                    var t = (TupleSlim<TextWriter, ReadOnlyMemory<char>>)state!;
                    t.Item1.WriteLine(t.Item2.Span);
                }, new TupleSlim<TextWriter, ReadOnlyMemory<char>>(this, buffer), cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

        public virtual Task WriteLineAsync()
        {
            return WriteAsync(CoreNewLine);
        }

        public virtual Task FlushAsync()
        {
            return Task.Factory.StartNew(static state => ((TextWriter)state!).Flush(), this,
                CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }

        /// <summary>
        /// Asynchronously clears all buffers for the current writer and causes any buffered data to
        /// be written to the underlying device.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous flush operation.</returns>
        /// <exception cref="ObjectDisposedException">The text writer is disposed.</exception>
        /// <exception cref="InvalidOperationException">The writer is currently in use by a previous write operation.</exception>
        public virtual Task FlushAsync(CancellationToken cancellationToken) =>
            cancellationToken.IsCancellationRequested ? Task.FromCanceled(cancellationToken) :
            FlushAsync();
        #endregion

        private sealed class NullTextWriter : TextWriter
        {
            internal NullTextWriter() { }

            public override IFormatProvider FormatProvider => CultureInfo.InvariantCulture;
            public override Encoding Encoding => Encoding.Unicode;
            [AllowNull]
            public override string NewLine { get => base.NewLine; set { } }

            // To avoid all unnecessary overhead in the base, override all Flush/Write methods as pure nops.

            public override void Flush() { }
            public override Task FlushAsync() => Task.CompletedTask;
            public override Task FlushAsync(CancellationToken cancellationToken) => Task.CompletedTask;

            public override void Write(char value) { }
            public override void Write(char[]? buffer) { }
            public override void Write(char[] buffer, int index, int count) { }
            public override void Write(ReadOnlySpan<char> buffer) { }
            public override void Write(bool value) { }
            public override void Write(int value) { }
            public override void Write(uint value) { }
            public override void Write(long value) { }
            public override void Write(ulong value) { }
            public override void Write(float value) { }
            public override void Write(double value) { }
            public override void Write(decimal value) { }
            public override void Write(string? value) { }
            public override void Write(object? value) { }
            public override void Write(StringBuilder? value) { }
            public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0) { }
            public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1) { }
            public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2) { }
            public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] arg) { }
            public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params ReadOnlySpan<object?> arg) { }
            public override Task WriteAsync(char value) => Task.CompletedTask;
            public override Task WriteAsync(string? value) => Task.CompletedTask;
            public override Task WriteAsync(StringBuilder? value, CancellationToken cancellationToken = default) => Task.CompletedTask;
            public override Task WriteAsync(char[] buffer, int index, int count) => Task.CompletedTask;
            public override Task WriteAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default) => Task.CompletedTask;
            public override void WriteLine() { }
            public override void WriteLine(char value) { }
            public override void WriteLine(char[]? buffer) { }
            public override void WriteLine(char[] buffer, int index, int count) { }
            public override void WriteLine(ReadOnlySpan<char> buffer) { }
            public override void WriteLine(bool value) { }
            public override void WriteLine(int value) { }
            public override void WriteLine(uint value) { }
            public override void WriteLine(long value) { }
            public override void WriteLine(ulong value) { }
            public override void WriteLine(float value) { }
            public override void WriteLine(double value) { }
            public override void WriteLine(decimal value) { }
            public override void WriteLine(string? value) { }
            public override void WriteLine(StringBuilder? value) { }
            public override void WriteLine(object? value) { }
            public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0) { }
            public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1) { }
            public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2) { }
            public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] arg) { }
            public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params ReadOnlySpan<object?> arg) { }
            public override Task WriteLineAsync(char value) => Task.CompletedTask;
            public override Task WriteLineAsync(string? value) => Task.CompletedTask;
            public override Task WriteLineAsync(StringBuilder? value, CancellationToken cancellationToken = default) => Task.CompletedTask;
            public override Task WriteLineAsync(char[] buffer, int index, int count) => Task.CompletedTask;
            public override Task WriteLineAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default) => Task.CompletedTask;
            public override Task WriteLineAsync() => Task.CompletedTask;
        }

        public static TextWriter Synchronized(TextWriter writer)
        {
            ArgumentNullException.ThrowIfNull(writer);

#if (!TARGET_BROWSER && !TARGET_WASI) || FEATURE_WASM_MANAGED_THREADS
            return writer is SyncTextWriter ? writer : new SyncTextWriter(writer);
#else
            return writer;
#endif
        }

        internal sealed class SyncTextWriter : TextWriter, IDisposable
        {
            private readonly TextWriter _out;

            internal SyncTextWriter(TextWriter t)
            {
                _out = t;
            }

            public override Encoding Encoding => _out.Encoding;

            public override IFormatProvider FormatProvider => _out.FormatProvider;

            [AllowNull]
            public override string NewLine
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                get => _out.NewLine;
                [MethodImpl(MethodImplOptions.Synchronized)]
                set => _out.NewLine = value;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Close() => _out.Close();

            [MethodImpl(MethodImplOptions.Synchronized)]
            protected override void Dispose(bool disposing)
            {
                // Explicitly pick up a potentially methodimpl'ed Dispose
                if (disposing)
                    ((IDisposable)_out).Dispose();
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Flush() => _out.Flush();

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write(char value) => _out.Write(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write(char[]? buffer) => _out.Write(buffer);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write(char[] buffer, int index, int count) => _out.Write(buffer, index, count);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write(ReadOnlySpan<char> buffer) => _out.Write(buffer);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write(bool value) => _out.Write(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write(int value) => _out.Write(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write(uint value) => _out.Write(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write(long value) => _out.Write(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write(ulong value) => _out.Write(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write(float value) => _out.Write(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write(double value) => _out.Write(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write(decimal value) => _out.Write(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write(string? value) => _out.Write(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write(StringBuilder? value) => _out.Write(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write(object? value) => _out.Write(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0) => _out.Write(format, arg0);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1) => _out.Write(format, arg0, arg1);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2) => _out.Write(format, arg0, arg1, arg2);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object?[] arg) => _out.Write(format, arg);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void Write([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params ReadOnlySpan<object?> arg) => _out.Write(format, arg);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine() => _out.WriteLine();

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine(char value) => _out.WriteLine(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine(decimal value) => _out.WriteLine(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine(char[]? buffer) => _out.WriteLine(buffer);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine(char[] buffer, int index, int count) => _out.WriteLine(buffer, index, count);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine(ReadOnlySpan<char> buffer) => _out.WriteLine(buffer);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine(bool value) => _out.WriteLine(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine(int value) => _out.WriteLine(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine(uint value) => _out.WriteLine(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine(long value) => _out.WriteLine(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine(ulong value) => _out.WriteLine(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine(float value) => _out.WriteLine(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine(double value) => _out.WriteLine(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine(string? value) => _out.WriteLine(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine(StringBuilder? value) => _out.WriteLine(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine(object? value) => _out.WriteLine(value);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0) => _out.WriteLine(format, arg0);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1) => _out.WriteLine(format, arg0, arg1);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2) => _out.WriteLine(format, arg0, arg1, arg2);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object?[] arg) => _out.WriteLine(format, arg);

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override void WriteLine([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params ReadOnlySpan<object?> arg) => _out.WriteLine(format, arg);

            //
            // On SyncTextWriter all APIs should run synchronously, even the async ones.
            //

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override ValueTask DisposeAsync()
            {
                Dispose();
                return default;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override Task WriteAsync(char value)
            {
                Write(value);
                return Task.CompletedTask;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override Task WriteAsync(string? value)
            {
                Write(value);
                return Task.CompletedTask;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override Task WriteAsync(StringBuilder? value, CancellationToken cancellationToken = default)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return Task.FromCanceled(cancellationToken);
                }

                Write(value);
                return Task.CompletedTask;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override Task WriteAsync(char[] buffer, int index, int count)
            {
                Write(buffer, index, count);
                return Task.CompletedTask;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override Task WriteAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return Task.FromCanceled(cancellationToken);
                }

                Write(buffer.Span);
                return Task.CompletedTask;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override Task WriteLineAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return Task.FromCanceled(cancellationToken);
                }

                WriteLine(buffer.Span);
                return Task.CompletedTask;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override Task WriteLineAsync(char value)
            {
                WriteLine(value);
                return Task.CompletedTask;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override Task WriteLineAsync()
            {
                WriteLine();
                return Task.CompletedTask;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override Task WriteLineAsync(string? value)
            {
                WriteLine(value);
                return Task.CompletedTask;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override Task WriteLineAsync(StringBuilder? value, CancellationToken cancellationToken = default)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return Task.FromCanceled(cancellationToken);
                }

                WriteLine(value);
                return Task.CompletedTask;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override Task WriteLineAsync(char[] buffer, int index, int count)
            {
                WriteLine(buffer, index, count);
                return Task.CompletedTask;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override Task FlushAsync()
            {
                Flush();
                return Task.CompletedTask;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override Task FlushAsync(CancellationToken cancellationToken)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return Task.FromCanceled(cancellationToken);
                }

                Flush();
                return Task.CompletedTask;
            }
        }
    }
}
