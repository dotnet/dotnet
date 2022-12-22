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
[assembly: AssemblyTitle("System.Globalization.Extensions")]
[assembly: AssemblyDescription("System.Globalization.Extensions")]
[assembly: AssemblyDefaultAlias("System.Globalization.Extensions")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.1.0")]




namespace System
{
    public static partial class StringNormalizationExtensions
    {
        public static bool IsNormalized(this string value) { throw null; }
        public static bool IsNormalized(this string value, System.Text.NormalizationForm normalizationForm) { throw null; }
        public static string Normalize(this string value) { throw null; }
        public static string Normalize(this string value, System.Text.NormalizationForm normalizationForm) { throw null; }
    }
}
namespace System.Globalization
{
    public static partial class GlobalizationExtensions
    {
        public static System.StringComparer GetStringComparer(this System.Globalization.CompareInfo compareInfo, System.Globalization.CompareOptions options) { throw null; }
    }
    public sealed partial class IdnMapping
    {
        public IdnMapping() { }
        public bool AllowUnassigned { get { throw null; } set { } }
        public bool UseStd3AsciiRules { get { throw null; } set { } }
        public override bool Equals(object obj) { throw null; }
        public string GetAscii(string unicode) { throw null; }
        public string GetAscii(string unicode, int index) { throw null; }
        public string GetAscii(string unicode, int index, int count) { throw null; }
        public override int GetHashCode() { throw null; }
        public string GetUnicode(string ascii) { throw null; }
        public string GetUnicode(string ascii, int index) { throw null; }
        public string GetUnicode(string ascii, int index, int count) { throw null; }
    }
}
namespace System.Text
{
    public enum NormalizationForm
    {
        FormC = 1,
        FormD = 2,
        FormKC = 5,
        FormKD = 6,
    }
}
