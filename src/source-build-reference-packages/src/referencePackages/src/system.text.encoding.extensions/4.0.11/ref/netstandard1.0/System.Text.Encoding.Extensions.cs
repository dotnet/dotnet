// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Reflection.AssemblyTitle("System.Text.Encoding.Extensions.dll")]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Reflection.AssemblyDescription("System.Text.Encoding.Extensions.dll")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Text.Encoding.Extensions.dll")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.0.30319.17929")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.0.30319.17929")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Text
{
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

    public partial class UTF8Encoding : Encoding
    {
        public UTF8Encoding() { }

        public UTF8Encoding(bool encoderShouldEmitUTF8Identifier, bool throwOnInvalidBytes) { }

        public UTF8Encoding(bool encoderShouldEmitUTF8Identifier) { }

        public override bool Equals(object value) { throw null; }

        public override int GetByteCount(char[] chars, int index, int count) { throw null; }

        public override int GetByteCount(string chars) { throw null; }

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
}