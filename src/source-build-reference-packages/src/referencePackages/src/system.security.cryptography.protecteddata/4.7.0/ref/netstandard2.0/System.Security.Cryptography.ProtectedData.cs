// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Security.Cryptography.ProtectedData")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("System.Security.Cryptography.ProtectedData")]
[assembly: System.Reflection.AssemblyFileVersion("4.700.19.56404")]
[assembly: System.Reflection.AssemblyInformationalVersion("3.1.0+0f7f38c4fd323b26da10cce95f857f77f0f09b48")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Core")]
[assembly: System.Reflection.AssemblyTitle("System.Security.Cryptography.ProtectedData")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.5.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Security.Cryptography
{
    public enum DataProtectionScope
    {
        CurrentUser = 0,
        LocalMachine = 1
    }

    public static partial class ProtectedData
    {
        public static byte[] Protect(byte[] userData, byte[] optionalEntropy, DataProtectionScope scope) { throw null; }

        public static byte[] Unprotect(byte[] encryptedData, byte[] optionalEntropy, DataProtectionScope scope) { throw null; }
    }
}