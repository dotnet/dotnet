// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.CLSCompliant(false)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.Default | System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Reflection.AssemblyDescription("System.Runtime.CompilerServices.Unsafe")]
[assembly: System.Reflection.AssemblyFileVersion("6.100.125.10404")]
[assembly: System.Reflection.AssemblyInformationalVersion("6.1.1")]
[assembly: System.Reflection.AssemblyTitle("System.Runtime.CompilerServices.Unsafe")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyVersionAttribute("6.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Runtime.CompilerServices
{
    public static partial class Unsafe
    {
        public static ref T Add<T>(ref T source, int elementOffset) { throw null; }

        public static ref T Add<T>(ref T source, IntPtr elementOffset) { throw null; }

        public static ref T Add<T>(ref T source, nuint elementOffset) { throw null; }

        public static unsafe void* Add<T>(void* source, int elementOffset) { throw null; }

        public static ref T AddByteOffset<T>(ref T source, IntPtr byteOffset) { throw null; }

        public static ref T AddByteOffset<T>(ref T source, nuint byteOffset) { throw null; }

        public static bool AreSame<T>(ref T left, ref T right) { throw null; }

        public static T As<T>(object o)
            where T : class { throw null; }

        public static ref TTo As<TFrom, TTo>(ref TFrom source) { throw null; }

        public static unsafe void* AsPointer<T>(ref T value) { throw null; }

        public static ref T AsRef<T>(in T source) { throw null; }

        public static unsafe ref T AsRef<T>(void* source) { throw null; }

        public static IntPtr ByteOffset<T>(ref T origin, ref T target) { throw null; }

        public static unsafe void Copy<T>(ref T destination, void* source) { }

        public static unsafe void Copy<T>(void* destination, ref T source) { }

        public static void CopyBlock(ref byte destination, ref byte source, uint byteCount) { }

        public static unsafe void CopyBlock(void* destination, void* source, uint byteCount) { }

        public static void CopyBlockUnaligned(ref byte destination, ref byte source, uint byteCount) { }

        public static unsafe void CopyBlockUnaligned(void* destination, void* source, uint byteCount) { }

        public static void InitBlock(ref byte startAddress, byte value, uint byteCount) { }

        public static unsafe void InitBlock(void* startAddress, byte value, uint byteCount) { }

        public static void InitBlockUnaligned(ref byte startAddress, byte value, uint byteCount) { }

        public static unsafe void InitBlockUnaligned(void* startAddress, byte value, uint byteCount) { }

        public static bool IsAddressGreaterThan<T>(ref T left, ref T right) { throw null; }

        public static bool IsAddressLessThan<T>(ref T left, ref T right) { throw null; }

        public static bool IsNullRef<T>(ref T source) { throw null; }

        public static ref T NullRef<T>() { throw null; }

        public static unsafe T Read<T>(void* source) { throw null; }

        public static T ReadUnaligned<T>(ref byte source) { throw null; }

        public static unsafe T ReadUnaligned<T>(void* source) { throw null; }

        public static int SizeOf<T>() { throw null; }

        public static void SkipInit<T>(out T value) { throw null; }

        public static ref T Subtract<T>(ref T source, int elementOffset) { throw null; }

        public static ref T Subtract<T>(ref T source, IntPtr elementOffset) { throw null; }

        public static ref T Subtract<T>(ref T source, nuint elementOffset) { throw null; }

        public static unsafe void* Subtract<T>(void* source, int elementOffset) { throw null; }

        public static ref T SubtractByteOffset<T>(ref T source, IntPtr byteOffset) { throw null; }

        public static ref T SubtractByteOffset<T>(ref T source, nuint byteOffset) { throw null; }

        public static ref T Unbox<T>(object box)
            where T : struct { throw null; }

        public static unsafe void Write<T>(void* destination, T value) { }

        public static void WriteUnaligned<T>(ref byte destination, T value) { }

        public static unsafe void WriteUnaligned<T>(void* destination, T value) { }
    }
}