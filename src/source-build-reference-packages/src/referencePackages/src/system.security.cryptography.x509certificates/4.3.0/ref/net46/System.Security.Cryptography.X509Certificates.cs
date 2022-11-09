// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Security.Cryptography.X509Certificates")]
[assembly: AssemblyDescription("System.Security.Cryptography.X509Certificates")]
[assembly: AssemblyDefaultAlias("System.Security.Cryptography.X509Certificates")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.24705.01")]
[assembly: AssemblyInformationalVersion("4.6.24705.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.0.0")]

[assembly: TypeForwardedTo(typeof(Microsoft.Win32.SafeHandles.SafeX509ChainHandle))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.OpenFlags))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.PublicKey))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.RSACertificateExtensions))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.StoreLocation))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.StoreName))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X500DistinguishedName))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X500DistinguishedNameFlags))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509BasicConstraintsExtension))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509Certificate))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509Certificate2))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509Certificate2Collection))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509Certificate2Enumerator))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509CertificateCollection))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509Chain))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509ChainElement))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509ChainElementCollection))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509ChainElementEnumerator))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509ChainPolicy))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509ChainStatus))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509ChainStatusFlags))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509ContentType))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509Extension))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509ExtensionCollection))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509ExtensionEnumerator))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509FindType))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509KeyStorageFlags))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509KeyUsageExtension))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509KeyUsageFlags))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509NameType))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509RevocationFlag))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509RevocationMode))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509Store))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509SubjectKeyIdentifierExtension))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509SubjectKeyIdentifierHashAlgorithm))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.X509Certificates.X509VerificationFlags))]



