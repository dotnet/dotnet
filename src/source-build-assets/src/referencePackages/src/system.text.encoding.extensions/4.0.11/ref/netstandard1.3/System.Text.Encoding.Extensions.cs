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
[assembly: System.Reflection.AssemblyTitle("System.Text.Encoding.Extensions")]
[assembly: System.Reflection.AssemblyDescription("System.Text.Encoding.Extensions")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Text.Encoding.Extensions")]
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
    public partial class ASCIIEncoding : Encoding
    {
        public override bool IsSingleByte { get { throw null; } }

        public override int GetByteCount(char[] chars, int index, int count) { throw null; }

        [CLSCompliant(false)]
        public override unsafe int GetByteCount(char* chars, int count) { throw null; }

        public override int GetByteCount(string chars) { throw null; }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) { throw null; }

        [CLSCompliant(false)]
        public override unsafe int GetBytes(char* chars, int charCount, byte* bytes, int byteCount) { throw null; }

        public override int GetBytes(string chars, int charIndex, int charCount, byte[] bytes, int byteIndex) { throw null; }

        public override int GetCharCount(byte[] bytes, int index, int count) { throw null; }

        [CLSCompliant(false)]
        public override unsafe int GetCharCount(byte* bytes, int count) { throw null; }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) { throw null; }

        [CLSCompliant(false)]
        public override unsafe int GetChars(byte* bytes, int byteCount, char* chars, int charCount) { throw null; }

        public override Decoder GetDecoder() { throw null; }

        public override Encoder GetEncoder() { throw null; }

        public override int GetMaxByteCount(int charCount) { throw null; }

        public override int GetMaxCharCount(int byteCount) { throw null; }

        public override string GetString(byte[] bytes, int byteIndex, int byteCount) { throw null; }
    }

    public partial class UnicodeEncoding : Encoding
    {
        public UnicodeEncoding() { }

        public UnicodeEncoding(bool bigEndian, bool byteOrderMark, bool throwOnInvalidBytes) { }

        public UnicodeEncoding(bool bigEndian, bool byteOrderMark) { }

        public override bool Equals(object value) { throw null; }

        public override int GetByteCount(char[] chars, int index, int count) { throw null; }

        public override int GetByteCount(string s) { throw null; }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) { throw null; }

        public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex) { throw null; }

        public override int GetCharCount(byte[] bytes, int index, int count) { throw null; }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) { throw null; }

        public override Decoder GetDecoder() { throw null; }

        public override Encoder GetEncoder() { throw null; }

        public override int GetHashCode() { throw null; }

        public override int GetMaxByteCount(int charCount) { throw null; }

        public override int GetMaxCharCount(int byteCount) { throw null; }

        public override byte[] GetPreamble() { throw null; }

        public override string GetString(byte[] bytes, int index, int count) { throw null; }
    }

    public sealed partial class UTF32Encoding : Encoding
    {
        public UTF32Encoding() { }

        public UTF32Encoding(bool bigEndian, bool byteOrderMark, bool throwOnInvalidCharacters) { }

        public UTF32Encoding(bool bigEndian, bool byteOrderMark) { }

        public override bool Equals(object value) { throw null; }

        public override int GetByteCount(char[] chars, int index, int count) { throw null; }

        [CLSCompliant(false)]
        public override unsafe int GetByteCount(char* chars, int count) { throw null; }

        public override int GetByteCount(string s) { throw null; }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) { throw null; }

        [CLSCompliant(false)]
        public override unsafe int GetBytes(char* chars, int charCount, byte* bytes, int byteCount) { throw null; }

        public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex) { throw null; }

        public override int GetCharCount(byte[] bytes, int index, int count) { throw null; }

        [CLSCompliant(false)]
        public override unsafe int GetCharCount(byte* bytes, int count) { throw null; }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) { throw null; }

        [CLSCompliant(false)]
        public override unsafe int GetChars(byte* bytes, int byteCount, char* chars, int charCount) { throw null; }

        public override Decoder GetDecoder() { throw null; }

        public override Encoder GetEncoder() { throw null; }

        public override int GetHashCode() { throw null; }

        public override int GetMaxByteCount(int charCount) { throw null; }

        public override int GetMaxCharCount(int byteCount) { throw null; }

        public override byte[] GetPreamble() { throw null; }

        public override string GetString(byte[] bytes, int index, int count) { throw null; }
    }

    public partial class UTF7Encoding : Encoding
    {
        public UTF7Encoding() { }

        public UTF7Encoding(bool allowOptionals) { }

        public override bool Equals(object value) { throw null; }

        public override int GetByteCount(char[] chars, int index, int count) { throw null; }

        [CLSCompliant(false)]
        public override unsafe int GetByteCount(char* chars, int count) { throw null; }

        public override int GetByteCount(string s) { throw null; }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) { throw null; }

        [CLSCompliant(false)]
        public override unsafe int GetBytes(char* chars, int charCount, byte* bytes, int byteCount) { throw null; }

        public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex) { throw null; }

        public override int GetCharCount(byte[] bytes, int index, int count) { throw null; }

        [CLSCompliant(false)]
        public override unsafe int GetCharCount(byte* bytes, int count) { throw null; }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) { throw null; }

        [CLSCompliant(false)]
        public override unsafe int GetChars(byte* bytes, int byteCount, char* chars, int charCount) { throw null; }

        public override Decoder GetDecoder() { throw null; }

        public override Encoder GetEncoder() { throw null; }

        public override int GetHashCode() { throw null; }

        public override int GetMaxByteCount(int charCount) { throw null; }

        public override int GetMaxCharCount(int byteCount) { throw null; }

        public override string GetString(byte[] bytes, int index, int count) { throw null; }
    }

    public partial class UTF8Encoding : Encoding
    {
        public UTF8Encoding() { }

        public UTF8Encoding(bool encoderShouldEmitUTF8Identifier, bool throwOnInvalidBytes) { }

        public UTF8Encoding(bool encoderShouldEmitUTF8Identifier) { }

        public override bool Equals(object value) { throw null; }

        public override int GetByteCount(char[] chars, int index, int count) { throw null; }

        [CLSCompliant(false)]
        public override unsafe int GetByteCount(char* chars, int count) { throw null; }

        public override int GetByteCount(string chars) { throw null; }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) { throw null; }

        [CLSCompliant(false)]
        public override unsafe int GetBytes(char* chars, int charCount, byte* bytes, int byteCount) { throw null; }

        public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex) { throw null; }

        public override int GetCharCount(byte[] bytes, int index, int count) { throw null; }

        [CLSCompliant(false)]
        public override unsafe int GetCharCount(byte* bytes, int count) { throw null; }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) { throw null; }

        [CLSCompliant(false)]
        public override unsafe int GetChars(byte* bytes, int byteCount, char* chars, int charCount) { throw null; }

        public override Decoder GetDecoder() { throw null; }

        public override Encoder GetEncoder() { throw null; }

        public override int GetHashCode() { throw null; }

        public override int GetMaxByteCount(int charCount) { throw null; }

        public override int GetMaxCharCount(int byteCount) { throw null; }

        public override byte[] GetPreamble() { throw null; }

        public override string GetString(byte[] bytes, int index, int count) { throw null; }
    }
}