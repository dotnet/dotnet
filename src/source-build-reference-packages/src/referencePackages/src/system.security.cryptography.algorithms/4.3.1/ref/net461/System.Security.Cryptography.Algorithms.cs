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
using System.Security.Cryptography;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Security.Cryptography.Algorithms")]
[assembly: AssemblyDescription("System.Security.Cryptography.Algorithms")]
[assembly: AssemblyDefaultAlias("System.Security.Cryptography.Algorithms")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.25815.03")]
[assembly: AssemblyInformationalVersion("4.6.25815.03 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.1.0.0")]

[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Aes))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.DeriveBytes))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.ECDsa))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.HMACMD5))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.HMACSHA1))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.HMACSHA256))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.HMACSHA384))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.HMACSHA512))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.MD5))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.RandomNumberGenerator))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Rfc2898DeriveBytes))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.RSA))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.RSAEncryptionPadding))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.RSAEncryptionPaddingMode))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.RSAParameters))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.RSASignaturePadding))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.RSASignaturePaddingMode))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.SHA1))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.SHA256))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.SHA384))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.SHA512))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.TripleDES))]



namespace System.Security.Cryptography
{
    public sealed partial class IncrementalHash : System.IDisposable
    {
        internal IncrementalHash() { }
        public System.Security.Cryptography.HashAlgorithmName AlgorithmName { get { throw null; } }
        public void AppendData(byte[] data) { }
        public void AppendData(byte[] data, int offset, int count) { }
        public static System.Security.Cryptography.IncrementalHash CreateHash(System.Security.Cryptography.HashAlgorithmName hashAlgorithm) { throw null; }
        public static System.Security.Cryptography.IncrementalHash CreateHMAC(System.Security.Cryptography.HashAlgorithmName hashAlgorithm, byte[] key) { throw null; }
        public void Dispose() { }
        public byte[] GetHashAndReset() { throw null; }
    }
}
