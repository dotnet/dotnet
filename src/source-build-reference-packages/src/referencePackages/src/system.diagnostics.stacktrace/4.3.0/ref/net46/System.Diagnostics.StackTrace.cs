// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Reflection.AssemblyTitle("System.Diagnostics.StackTrace")]
[assembly: System.Reflection.AssemblyDescription("System.Diagnostics.StackTrace")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Diagnostics.StackTrace")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.6.24705.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.6.24705.01. Commit Hash: 4d1af962ca0fede10beb01d197367c2f90e92c97")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.3.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Diagnostics.StackFrame))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Diagnostics.StackTrace))]
namespace System.Diagnostics
{
    public static partial class StackFrameExtensions
    {
        public static IntPtr GetNativeImageBase(this StackFrame stackFrame) { throw null; }

        public static IntPtr GetNativeIP(this StackFrame stackFrame) { throw null; }

        public static bool HasILOffset(this StackFrame stackFrame) { throw null; }

        public static bool HasMethod(this StackFrame stackFrame) { throw null; }

        public static bool HasNativeImage(this StackFrame stackFrame) { throw null; }

        public static bool HasSource(this StackFrame stackFrame) { throw null; }
    }
}