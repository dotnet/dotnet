// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Text.Encoding.Extensions")]
[assembly: AssemblyDescription("System.Text.Encoding.Extensions")]
[assembly: AssemblyDefaultAlias("System.Text.Encoding.Extensions")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.0.30319.17929")]
[assembly: AssemblyInformationalVersion("4.0.30319.17929 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.0.0")]




namespace System.Text
{
    public partial class UnicodeEncoding : System.Text.Encoding
    {
        public UnicodeEncoding() { }
        public UnicodeEncoding(bool bigEndian, bool byteOrderMark) { }
        public UnicodeEncoding(bool bigEndian, bool byteOrderMark, bool throwOnInvalidBytes) { }
        public override bool Equals(object value) { throw null; }
        public override int GetByteCount(char[] chars, int index, int count) { throw null; }
        public override int GetByteCount(string s) { throw null; }
        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) { throw null; }
        public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex) { throw null; }
        public override int GetCharCount(byte[] bytes, int index, int count) { throw null; }
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) { throw null; }
        public override System.Text.Decoder GetDecoder() { throw null; }
        public override System.Text.Encoder GetEncoder() { throw null; }
        public override int GetHashCode() { throw null; }
        public override int GetMaxByteCount(int charCount) { throw null; }
        public override int GetMaxCharCount(int byteCount) { throw null; }
        public override byte[] GetPreamble() { throw null; }
        public override string GetString(byte[] bytes, int index, int count) { throw null; }
    }
    public partial class UTF8Encoding : System.Text.Encoding
    {
        public UTF8Encoding() { }
        public UTF8Encoding(bool encoderShouldEmitUTF8Identifier) { }
        public UTF8Encoding(bool encoderShouldEmitUTF8Identifier, bool throwOnInvalidBytes) { }
        public override bool Equals(object value) { throw null; }
        public override int GetByteCount(char[] chars, int index, int count) { throw null; }
        public override int GetByteCount(string chars) { throw null; }
        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) { throw null; }
        public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex) { throw null; }
        public override int GetCharCount(byte[] bytes, int index, int count) { throw null; }
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) { throw null; }
        public override System.Text.Decoder GetDecoder() { throw null; }
        public override System.Text.Encoder GetEncoder() { throw null; }
        public override int GetHashCode() { throw null; }
        public override int GetMaxByteCount(int charCount) { throw null; }
        public override int GetMaxCharCount(int byteCount) { throw null; }
        public override byte[] GetPreamble() { throw null; }
        public override string GetString(byte[] bytes, int index, int count) { throw null; }
    }
}
