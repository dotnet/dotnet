// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Threading.Overlapped")]
[assembly: AssemblyDescription("System.Threading.Overlapped")]
[assembly: AssemblyDefaultAlias("System.Threading.Overlapped")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.1.0")]




namespace System.Threading
{
    [System.CLSCompliantAttribute(false)]
    public unsafe delegate void IOCompletionCallback(uint errorCode, uint numBytes, System.Threading.NativeOverlapped* pOVERLAP);
    public partial struct NativeOverlapped
    {
        public System.IntPtr EventHandle;
        public System.IntPtr InternalHigh;
        public System.IntPtr InternalLow;
        public int OffsetHigh;
        public int OffsetLow;
    }
    public sealed partial class PreAllocatedOverlapped : System.IDisposable
    {
        [System.CLSCompliantAttribute(false)]
        public PreAllocatedOverlapped(System.Threading.IOCompletionCallback callback, object state, object pinData) { }
        public void Dispose() { }
    }
    public sealed partial class ThreadPoolBoundHandle : System.IDisposable
    {
        internal ThreadPoolBoundHandle() { }
        public System.Runtime.InteropServices.SafeHandle Handle { get { throw null; } }
        [System.CLSCompliantAttribute(false)]
        public unsafe System.Threading.NativeOverlapped* AllocateNativeOverlapped(System.Threading.IOCompletionCallback callback, object state, object pinData) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public unsafe System.Threading.NativeOverlapped* AllocateNativeOverlapped(System.Threading.PreAllocatedOverlapped preAllocated) { throw null; }
        public static System.Threading.ThreadPoolBoundHandle BindHandle(System.Runtime.InteropServices.SafeHandle handle) { throw null; }
        public void Dispose() { }
        [System.CLSCompliantAttribute(false)]
        public unsafe void FreeNativeOverlapped(System.Threading.NativeOverlapped* overlapped) { }
        [System.CLSCompliantAttribute(false)]
        public unsafe static object GetNativeOverlappedState(System.Threading.NativeOverlapped* overlapped) { throw null; }
    }
}
