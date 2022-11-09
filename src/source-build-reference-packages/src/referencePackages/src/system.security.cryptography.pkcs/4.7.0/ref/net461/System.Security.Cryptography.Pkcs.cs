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
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.Xml;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Security.Cryptography.Pkcs")]
[assembly: AssemblyDescription("System.Security.Cryptography.Pkcs")]
[assembly: AssemblyDefaultAlias("System.Security.Cryptography.Pkcs")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.700.19.56404")]
[assembly: AssemblyInformationalVersion("4.700.19.56404 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.1.1.0")]

[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CryptographicAttributeObject))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CryptographicAttributeObjectCollection))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.CryptographicAttributeObjectEnumerator))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Xml.X509IssuerSerial))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.AlgorithmIdentifier))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.CmsRecipient))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.CmsRecipientCollection))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.CmsRecipientEnumerator))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.CmsSigner))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.ContentInfo))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.EnvelopedCms))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.KeyAgreeRecipientInfo))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.KeyTransRecipientInfo))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.Pkcs9AttributeObject))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.Pkcs9ContentType))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.Pkcs9DocumentDescription))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.Pkcs9DocumentName))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.Pkcs9MessageDigest))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.Pkcs9SigningTime))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.PublicKeyInfo))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.RecipientInfo))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.RecipientInfoCollection))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.RecipientInfoEnumerator))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.RecipientInfoType))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.SignedCms))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.SignerInfo))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.SignerInfoCollection))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.SignerInfoEnumerator))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.SubjectIdentifier))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.SubjectIdentifierOrKey))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.SubjectIdentifierOrKeyType))]
[assembly: TypeForwardedTo(typeof(System.Security.Cryptography.Pkcs.SubjectIdentifierType))]



