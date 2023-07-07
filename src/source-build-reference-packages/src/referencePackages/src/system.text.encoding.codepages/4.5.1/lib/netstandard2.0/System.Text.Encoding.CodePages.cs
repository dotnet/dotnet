// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyTitle("System.Text.Encoding.CodePages")]
[assembly: System.Reflection.AssemblyDescription("System.Text.Encoding.CodePages")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Text.Encoding.CodePages")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.6.27129.04")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.6.27129.04 @BuiltBy: dlab14-DDVSOWINAGE083 @Branch: release/2.1-MSRC @SrcCode: https://github.com/dotnet/corefx/tree/99ce22c306b07f99ddae60f443d23a983ae78f7b")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyVersionAttribute("4.1.1.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Text
{
    public sealed partial class CodePagesEncodingProvider : EncodingProvider
    {
        internal CodePagesEncodingProvider() { }

        public static EncodingProvider Instance { get { throw null; } }

        public override Encoding GetEncoding(int codepage) { throw null; }

        public override Encoding GetEncoding(string name) { throw null; }
    }
}