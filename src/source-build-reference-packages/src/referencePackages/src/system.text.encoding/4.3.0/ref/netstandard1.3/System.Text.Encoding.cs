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
[assembly: System.Reflection.AssemblyTitle("System.Text.Encoding")]
[assembly: System.Reflection.AssemblyDescription("System.Text.Encoding")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Text.Encoding")]
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
namespace System.Text
{
    public abstract partial class Decoder
    {
        public DecoderFallback Fallback { get { throw null; } set { } }

        public DecoderFallbackBuffer FallbackBuffer { get { throw null; } }

        public virtual void Convert(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, int charCount, bool flush, out int bytesUsed, out int charsUsed, out bool completed) { throw null; }

        public virtual int GetCharCount(byte[] bytes, int index, int count, bool flush) { throw null; }

        public abstract int GetCharCount(byte[] bytes, int index, int count);
        public virtual int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, bool flush) { throw null; }

        public abstract int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex);
        public virtual void Reset() { }
    }

    public sealed partial class DecoderExceptionFallback : DecoderFallback
    {
        public override int MaxCharCount { get { throw null; } }

        public override DecoderFallbackBuffer CreateFallbackBuffer() { throw null; }

        public override bool Equals(object value) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public abstract partial class DecoderFallback
    {
        public static DecoderFallback ExceptionFallback { get { throw null; } }

        public abstract int MaxCharCount { get; }

        public static DecoderFallback ReplacementFallback { get { throw null; } }

        public abstract DecoderFallbackBuffer CreateFallbackBuffer();
    }

    public abstract partial class DecoderFallbackBuffer
    {
        public abstract int Remaining { get; }

        public abstract bool Fallback(byte[] bytesUnknown, int index);
        public abstract char GetNextChar();
        public abstract bool MovePrevious();
        public virtual void Reset() { }
    }

    public sealed partial class DecoderFallbackException : ArgumentException
    {
        public DecoderFallbackException() { }

        public DecoderFallbackException(string message, byte[] bytesUnknown, int index) { }

        public DecoderFallbackException(string message, Exception innerException) { }

        public DecoderFallbackException(string message) { }

        public byte[] BytesUnknown { get { throw null; } }

        public int Index { get { throw null; } }
    }

    public sealed partial class DecoderReplacementFallback : DecoderFallback
    {
        public DecoderReplacementFallback() { }

        public DecoderReplacementFallback(string replacement) { }

        public string DefaultString { get { throw null; } }

        public override int MaxCharCount { get { throw null; } }

        public override DecoderFallbackBuffer CreateFallbackBuffer() { throw null; }

        public override bool Equals(object value) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public abstract partial class Encoder
    {
        public EncoderFallback Fallback { get { throw null; } set { } }

        public EncoderFallbackBuffer FallbackBuffer { get { throw null; } }

        public virtual void Convert(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, int byteCount, bool flush, out int charsUsed, out int bytesUsed, out bool completed) { throw null; }

        public abstract int GetByteCount(char[] chars, int index, int count, bool flush);
        public abstract int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush);
        public virtual void Reset() { }
    }

    public sealed partial class EncoderExceptionFallback : EncoderFallback
    {
        public override int MaxCharCount { get { throw null; } }

        public override EncoderFallbackBuffer CreateFallbackBuffer() { throw null; }

        public override bool Equals(object value) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public abstract partial class EncoderFallback
    {
        public static EncoderFallback ExceptionFallback { get { throw null; } }

        public abstract int MaxCharCount { get; }

        public static EncoderFallback ReplacementFallback { get { throw null; } }

        public abstract EncoderFallbackBuffer CreateFallbackBuffer();
    }

    public abstract partial class EncoderFallbackBuffer
    {
        public abstract int Remaining { get; }

        public abstract bool Fallback(char charUnknownHigh, char charUnknownLow, int index);
        public abstract bool Fallback(char charUnknown, int index);
        public abstract char GetNextChar();
        public abstract bool MovePrevious();
        public virtual void Reset() { }
    }

    public sealed partial class EncoderFallbackException : ArgumentException
    {
        public EncoderFallbackException() { }

        public EncoderFallbackException(string message, Exception innerException) { }

        public EncoderFallbackException(string message) { }

        public char CharUnknown { get { throw null; } }

        public char CharUnknownHigh { get { throw null; } }

        public char CharUnknownLow { get { throw null; } }

        public int Index { get { throw null; } }

        public bool IsUnknownSurrogate() { throw null; }
    }

    public sealed partial class EncoderReplacementFallback : EncoderFallback
    {
        public EncoderReplacementFallback() { }

        public EncoderReplacementFallback(string replacement) { }

        public string DefaultString { get { throw null; } }

        public override int MaxCharCount { get { throw null; } }

        public override EncoderFallbackBuffer CreateFallbackBuffer() { throw null; }

        public override bool Equals(object value) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public abstract partial class Encoding
    {
        protected Encoding() { }

        protected Encoding(int codePage, EncoderFallback encoderFallback, DecoderFallback decoderFallback) { }

        protected Encoding(int codePage) { }

        public static Encoding ASCII { get { throw null; } }

        public static Encoding BigEndianUnicode { get { throw null; } }

        public virtual int CodePage { get { throw null; } }

        public DecoderFallback DecoderFallback { get { throw null; } }

        public EncoderFallback EncoderFallback { get { throw null; } }

        public virtual string EncodingName { get { throw null; } }

        public virtual bool IsSingleByte { get { throw null; } }

        public static Encoding Unicode { get { throw null; } }

        public static Encoding UTF32 { get { throw null; } }

        public static Encoding UTF7 { get { throw null; } }

        public static Encoding UTF8 { get { throw null; } }

        public virtual string WebName { get { throw null; } }

        public virtual object Clone() { throw null; }

        public static byte[] Convert(Encoding srcEncoding, Encoding dstEncoding, byte[] bytes, int index, int count) { throw null; }

        public static byte[] Convert(Encoding srcEncoding, Encoding dstEncoding, byte[] bytes) { throw null; }

        public override bool Equals(object value) { throw null; }

        public abstract int GetByteCount(char[] chars, int index, int count);
        public virtual int GetByteCount(char[] chars) { throw null; }

        [CLSCompliant(false)]
        public virtual unsafe int GetByteCount(char* chars, int count) { throw null; }

        public virtual int GetByteCount(string s) { throw null; }

        public abstract int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex);
        public virtual byte[] GetBytes(char[] chars, int index, int count) { throw null; }

        public virtual byte[] GetBytes(char[] chars) { throw null; }

        [CLSCompliant(false)]
        public virtual unsafe int GetBytes(char* chars, int charCount, byte* bytes, int byteCount) { throw null; }

        public virtual int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex) { throw null; }

        public virtual byte[] GetBytes(string s) { throw null; }

        public abstract int GetCharCount(byte[] bytes, int index, int count);
        public virtual int GetCharCount(byte[] bytes) { throw null; }

        [CLSCompliant(false)]
        public virtual unsafe int GetCharCount(byte* bytes, int count) { throw null; }

        public abstract int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex);
        public virtual char[] GetChars(byte[] bytes, int index, int count) { throw null; }

        public virtual char[] GetChars(byte[] bytes) { throw null; }

        [CLSCompliant(false)]
        public virtual unsafe int GetChars(byte* bytes, int byteCount, char* chars, int charCount) { throw null; }

        public virtual Decoder GetDecoder() { throw null; }

        public virtual Encoder GetEncoder() { throw null; }

        public static Encoding GetEncoding(int codepage, EncoderFallback encoderFallback, DecoderFallback decoderFallback) { throw null; }

        public static Encoding GetEncoding(int codepage) { throw null; }

        public static Encoding GetEncoding(string name, EncoderFallback encoderFallback, DecoderFallback decoderFallback) { throw null; }

        public static Encoding GetEncoding(string name) { throw null; }

        public override int GetHashCode() { throw null; }

        public abstract int GetMaxByteCount(int charCount);
        public abstract int GetMaxCharCount(int byteCount);
        public virtual byte[] GetPreamble() { throw null; }

        public virtual string GetString(byte[] bytes, int index, int count) { throw null; }

        public virtual string GetString(byte[] bytes) { throw null; }

        [CLSCompliant(false)]
        public unsafe string GetString(byte* bytes, int byteCount) { throw null; }

        public static void RegisterProvider(EncodingProvider provider) { }
    }

    public abstract partial class EncodingProvider
    {
        public EncodingProvider() { }

        public virtual Encoding GetEncoding(int codepage, EncoderFallback encoderFallback, DecoderFallback decoderFallback) { throw null; }

        public abstract Encoding GetEncoding(int codepage);
        public virtual Encoding GetEncoding(string name, EncoderFallback encoderFallback, DecoderFallback decoderFallback) { throw null; }

        public abstract Encoding GetEncoding(string name);
    }
}