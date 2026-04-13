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
[assembly: System.Reflection.AssemblyTitle("System.Diagnostics.FileVersionInfo")]
[assembly: System.Reflection.AssemblyDescription("System.Diagnostics.FileVersionInfo")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Diagnostics.FileVersionInfo")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Diagnostics
{
    public sealed partial class FileVersionInfo
    {
        internal FileVersionInfo() { }

        public string Comments { get { throw null; } }

        public string CompanyName { get { throw null; } }

        public int FileBuildPart { get { throw null; } }

        public string FileDescription { get { throw null; } }

        public int FileMajorPart { get { throw null; } }

        public int FileMinorPart { get { throw null; } }

        public string FileName { get { throw null; } }

        public int FilePrivatePart { get { throw null; } }

        public string FileVersion { get { throw null; } }

        public string InternalName { get { throw null; } }

        public bool IsDebug { get { throw null; } }

        public bool IsPatched { get { throw null; } }

        public bool IsPreRelease { get { throw null; } }

        public bool IsPrivateBuild { get { throw null; } }

        public bool IsSpecialBuild { get { throw null; } }

        public string Language { get { throw null; } }

        public string LegalCopyright { get { throw null; } }

        public string LegalTrademarks { get { throw null; } }

        public string OriginalFilename { get { throw null; } }

        public string PrivateBuild { get { throw null; } }

        public int ProductBuildPart { get { throw null; } }

        public int ProductMajorPart { get { throw null; } }

        public int ProductMinorPart { get { throw null; } }

        public string ProductName { get { throw null; } }

        public int ProductPrivatePart { get { throw null; } }

        public string ProductVersion { get { throw null; } }

        public string SpecialBuild { get { throw null; } }

        public static FileVersionInfo GetVersionInfo(string fileName) { throw null; }

        public override string ToString() { throw null; }
    }
}