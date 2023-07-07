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
[assembly: System.Reflection.AssemblyTitle("System.IO.Compression.ZipFile")]
[assembly: System.Reflection.AssemblyDescription("System.IO.Compression.ZipFile")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.IO.Compression.ZipFile")]
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
namespace System.IO.Compression
{
    public static partial class ZipFile
    {
        public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, CompressionLevel compressionLevel, bool includeBaseDirectory, Text.Encoding entryNameEncoding) { }

        public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, CompressionLevel compressionLevel, bool includeBaseDirectory) { }

        public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName) { }

        public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName, Text.Encoding entryNameEncoding) { }

        public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName) { }

        public static ZipArchive Open(string archiveFileName, ZipArchiveMode mode, Text.Encoding entryNameEncoding) { throw null; }

        public static ZipArchive Open(string archiveFileName, ZipArchiveMode mode) { throw null; }

        public static ZipArchive OpenRead(string archiveFileName) { throw null; }
    }

    public static partial class ZipFileExtensions
    {
        public static ZipArchiveEntry CreateEntryFromFile(this ZipArchive destination, string sourceFileName, string entryName, CompressionLevel compressionLevel) { throw null; }

        public static ZipArchiveEntry CreateEntryFromFile(this ZipArchive destination, string sourceFileName, string entryName) { throw null; }

        public static void ExtractToDirectory(this ZipArchive source, string destinationDirectoryName) { }

        public static void ExtractToFile(this ZipArchiveEntry source, string destinationFileName, bool overwrite) { }

        public static void ExtractToFile(this ZipArchiveEntry source, string destinationFileName) { }
    }
}