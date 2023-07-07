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
[assembly: System.Reflection.AssemblyTitle("System.Runtime.InteropServices.RuntimeInformation")]
[assembly: System.Reflection.AssemblyDescription("System.Runtime.InteropServices.RuntimeInformation")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Runtime.InteropServices.RuntimeInformation")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.6.24705.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.6.24705.01. Commit Hash: 4d1af962ca0fede10beb01d197367c2f90e92c97")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.1.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Runtime.InteropServices
{
    public enum Architecture
    {
        X86 = 0,
        X64 = 1,
        Arm = 2,
        Arm64 = 3
    }

    public partial struct OSPlatform : IEquatable<OSPlatform>
    {
        public static OSPlatform Linux { get { throw null; } }

        public static OSPlatform OSX { get { throw null; } }

        public static OSPlatform Windows { get { throw null; } }

        public static OSPlatform Create(string osPlatform) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(OSPlatform other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(OSPlatform left, OSPlatform right) { throw null; }

        public static bool operator !=(OSPlatform left, OSPlatform right) { throw null; }

        public override string ToString() { throw null; }
    }

    public static partial class RuntimeInformation
    {
        public static string FrameworkDescription { get { throw null; } }

        public static Architecture OSArchitecture { get { throw null; } }

        public static string OSDescription { get { throw null; } }

        public static Architecture ProcessArchitecture { get { throw null; } }

        public static bool IsOSPlatform(OSPlatform osPlatform) { throw null; }
    }
}