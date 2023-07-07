// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Reflection.AssemblyTitle("System.Text.Encoding.dll")]
[assembly: System.Reflection.AssemblyDescription("System.Text.Encoding.dll")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Text.Encoding.dll")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.0.30319.17929")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.0.30319.17929")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Text
{
    public abstract partial class Decoder
    {
        public virtual void Convert(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, int charCount, bool flush, out int bytesUsed, out int charsUsed, out bool completed) { throw null; }

        public virtual int GetCharCount(byte[] bytes, int index, int count, bool flush) { throw null; }

        public abstract int GetCharCount(byte[] bytes, int index, int count);
        public virtual int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, bool flush) { throw null; }

        public abstract int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex);
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

    public abstract partial class Encoder
    {
        public virtual void Convert(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, int byteCount, bool flush, out int charsUsed, out int bytesUsed, out bool completed) { throw null; }

        public abstract int GetByteCount(char[] chars, int index, int count, bool flush);
        public abstract int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush);
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
    }

    public abstract partial class Encoding
    {
        public static Encoding BigEndianUnicode { get { throw null; } }

        public static Encoding Unicode { get { throw null; } }

        public static Encoding UTF8 { get { throw null; } }

        public virtual string WebName { get { throw null; } }

        public static byte[] Convert(Encoding srcEncoding, Encoding dstEncoding, byte[] bytes, int index, int count) { throw null; }

        public static byte[] Convert(Encoding srcEncoding, Encoding dstEncoding, byte[] bytes) { throw null; }

        public override bool Equals(object value) { throw null; }

        public abstract int GetByteCount(char[] chars, int index, int count);
        public virtual int GetByteCount(char[] chars) { throw null; }

        public virtual int GetByteCount(string s) { throw null; }

        public abstract int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex);
        public virtual byte[] GetBytes(char[] chars, int index, int count) { throw null; }

        public virtual byte[] GetBytes(char[] chars) { throw null; }

        public virtual int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex) { throw null; }

        public virtual byte[] GetBytes(string s) { throw null; }

        public abstract int GetCharCount(byte[] bytes, int index, int count);
        public virtual int GetCharCount(byte[] bytes) { throw null; }

        public abstract int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex);
        public virtual char[] GetChars(byte[] bytes, int index, int count) { throw null; }

        public virtual char[] GetChars(byte[] bytes) { throw null; }

        public virtual Decoder GetDecoder() { throw null; }

        public virtual Encoder GetEncoder() { throw null; }

        public static Encoding GetEncoding(string name) { throw null; }

        public override int GetHashCode() { throw null; }

        public abstract int GetMaxByteCount(int charCount);
        public abstract int GetMaxCharCount(int byteCount);
        public virtual byte[] GetPreamble() { throw null; }

        public virtual string GetString(byte[] bytes, int index, int count) { throw null; }
    }
}