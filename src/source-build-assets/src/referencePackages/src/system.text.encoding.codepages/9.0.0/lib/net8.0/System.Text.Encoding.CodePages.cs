// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v8.0", FrameworkDisplayName = ".NET 8.0")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Text.Encoding.CodePages")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides support for code-page based encodings, including Windows-1252, Shift-JIS, and GB2312.\r\n\r\nCommonly Used Types:\r\nSystem.Text.CodePagesEncodingProvider")]
[assembly: System.Reflection.AssemblyFileVersion("9.0.24.52809")]
[assembly: System.Reflection.AssemblyInformationalVersion("9.0.0+9d5a6a9aa463d6d10b0b0ba6d5982cc82f363dc3")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Text.Encoding.CodePages")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("9.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Text
{
    public sealed partial class CodePagesEncodingProvider : EncodingProvider
    {
        internal CodePagesEncodingProvider() { }
        public static EncodingProvider Instance { get { throw null; } }

        public override Encoding? GetEncoding(int codepage) { throw null; }
        public override Encoding? GetEncoding(string name) { throw null; }
        public override Collections.Generic.IEnumerable<EncodingInfo> GetEncodings() { throw null; }
    }
}