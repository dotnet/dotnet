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
[assembly: System.Reflection.AssemblyTitle("System.Globalization.Extensions")]
[assembly: System.Reflection.AssemblyDescription("System.Globalization.Extensions")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Globalization.Extensions")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.1.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System
{
    public static partial class StringNormalizationExtensions
    {
        public static bool IsNormalized(this string value, Text.NormalizationForm normalizationForm) { throw null; }

        public static bool IsNormalized(this string value) { throw null; }

        public static string Normalize(this string value, Text.NormalizationForm normalizationForm) { throw null; }

        public static string Normalize(this string value) { throw null; }
    }
}

namespace System.Globalization
{
    public static partial class GlobalizationExtensions
    {
        public static StringComparer GetStringComparer(this CompareInfo compareInfo, CompareOptions options) { throw null; }
    }

    public sealed partial class IdnMapping
    {
        public bool AllowUnassigned { get { throw null; } set { } }

        public bool UseStd3AsciiRules { get { throw null; } set { } }

        public override bool Equals(object obj) { throw null; }

        public string GetAscii(string unicode, int index, int count) { throw null; }

        public string GetAscii(string unicode, int index) { throw null; }

        public string GetAscii(string unicode) { throw null; }

        public override int GetHashCode() { throw null; }

        public string GetUnicode(string ascii, int index, int count) { throw null; }

        public string GetUnicode(string ascii, int index) { throw null; }

        public string GetUnicode(string ascii) { throw null; }
    }
}

namespace System.Text
{
    public enum NormalizationForm
    {
        FormC = 1,
        FormD = 2,
        FormKC = 5,
        FormKD = 6
    }
}