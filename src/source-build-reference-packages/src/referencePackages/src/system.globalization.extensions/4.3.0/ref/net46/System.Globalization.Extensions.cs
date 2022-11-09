// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Globalization.Extensions")]
[assembly: AssemblyDescription("System.Globalization.Extensions")]
[assembly: AssemblyDefaultAlias("System.Globalization.Extensions")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.24705.01")]
[assembly: AssemblyInformationalVersion("4.6.24705.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.2.0")]

[assembly: TypeForwardedTo(typeof(System.Globalization.IdnMapping))]
[assembly: TypeForwardedTo(typeof(System.Text.NormalizationForm))]



namespace System
{
    public static partial class StringNormalizationExtensions
    {
        public static bool IsNormalized(this string strInput) { throw null; }
        public static bool IsNormalized(this string strInput, System.Text.NormalizationForm normalizationForm) { throw null; }
        public static string Normalize(this string strInput) { throw null; }
        public static string Normalize(this string strInput, System.Text.NormalizationForm normalizationForm) { throw null; }
    }
}
namespace System.Globalization
{
    public static partial class GlobalizationExtensions
    {
        public static System.StringComparer GetStringComparer(this System.Globalization.CompareInfo compareInfo, System.Globalization.CompareOptions options) { throw null; }
    }
}
