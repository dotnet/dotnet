// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.NET.StringTools.UnitTests, PublicKey=002400000480000094000000060200000024000052534131000400000100010015c01ae1f50e8cc09ba9eac9147cf8fd9fce2cfe9f8dce4f7301c4132ca9fb50ce8cbf1df4dc18dd4d210e4345c744ecb3365ed327efdbc52603faa5e21daa11234c8c4a73e51f03bf192544581ebe107adee3a34928e39d04e524a9ce729d5090bfd7dad9d10c722c0def9ccc08ff0a03790e48bcd1f9b6c476063e1966a1c4")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.NET.StringTools.net35.UnitTests, PublicKey=002400000480000094000000060200000024000052534131000400000100010007d1fa57c4aed9f0a32e84aa0faefd0de9e8fd6aec8f87fb03766c834c99921eb23be79ad9d5dcc1dd9ad236132102900b723cf980957fc4e177108fc607774f29e8320e92ea05ece4e821c0a5efe8f1645c4c0c93c1ab99285d622caa652c1dfad63d745d6f2de5f17e5eaf0fc4963d261c8a12436518206dc093344d5ad293")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.NET.StringTools.Benchmark, PublicKey=002400000480000094000000060200000024000052534131000400000100010007d1fa57c4aed9f0a32e84aa0faefd0de9e8fd6aec8f87fb03766c834c99921eb23be79ad9d5dcc1dd9ad236132102900b723cf980957fc4e177108fc607774f29e8320e92ea05ece4e821c0a5efe8f1645c4c0c93c1ab99285d622caa652c1dfad63d745d6f2de5f17e5eaf0fc4963d261c8a12436518206dc093344d5ad293")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyConfiguration("Release")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Microsoft.NET.StringTools.dll")]
[assembly: System.Reflection.AssemblyFileVersion("17.12.6.51805")]
[assembly: System.Reflection.AssemblyInformationalVersion("17.12.6+db5f6012cb7f6e2dd7066c50c573c0d352713407")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® Build Tools®")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.NET.StringTools.dll")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/msbuild")]
[assembly: System.Reflection.AssemblyVersionAttribute("1.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.NET.StringTools
{
    public static partial class FowlerNollVo1aHash
    {
        public static long Combine64(long left, long right) { throw null; }

        public static int ComputeHash32(string text) { throw null; }

        public static int ComputeHash32Fast(string text) { throw null; }

        public static long ComputeHash64(string text) { throw null; }

        public static long ComputeHash64Fast(string text) { throw null; }
    }

    public partial class SpanBasedStringBuilder : System.IDisposable
    {
        public SpanBasedStringBuilder(int capacity = 4) { }

        public SpanBasedStringBuilder(string str) { }

        public int Capacity { get { throw null; } }

        public int Length { get { throw null; } }

        public void Append(System.ReadOnlyMemory<char> span) { }

        public void Append(string value, int startIndex, int count) { }

        public void Append(string? value) { }

        public void Clear() { }

        public void Dispose() { }

        public Enumerator GetEnumerator() { throw null; }

        public override string ToString() { throw null; }

        public void Trim() { }

        public void TrimEnd() { }

        public void TrimStart() { }

        public partial struct Enumerator
        {
            private object _dummy;
            private int _dummyPrimitive;
            public char Current { get { throw null; } }

            public bool MoveNext() { throw null; }
        }
    }

    public static partial class Strings
    {
        public static string CreateDiagnosticReport() { throw null; }

        public static void EnableDiagnostics() { }

        public static SpanBasedStringBuilder GetSpanBasedStringBuilder() { throw null; }

        public static string WeakIntern(System.ReadOnlySpan<char> str) { throw null; }

        public static string WeakIntern(string str) { throw null; }
    }
}