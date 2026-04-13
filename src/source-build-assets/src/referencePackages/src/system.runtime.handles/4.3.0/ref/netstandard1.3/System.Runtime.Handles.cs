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
[assembly: System.Reflection.AssemblyTitle("System.Runtime.Handles")]
[assembly: System.Reflection.AssemblyDescription("System.Runtime.Handles")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Runtime.Handles")]
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
namespace Microsoft.Win32.SafeHandles
{
    public sealed partial class SafeWaitHandle : System.Runtime.InteropServices.SafeHandle
    {
        public SafeWaitHandle(System.IntPtr existingHandle, bool ownsHandle) : base(default, default) { }

        public override bool IsInvalid { get { throw null; } }

        protected override bool ReleaseHandle() { throw null; }
    }
}

namespace System.IO
{
    public enum HandleInheritability
    {
        None = 0,
        Inheritable = 1
    }
}

namespace System.Runtime.InteropServices
{
    public abstract partial class CriticalHandle : IDisposable
    {
        protected IntPtr handle;
        protected CriticalHandle(IntPtr invalidHandleValue) { }

        public bool IsClosed { get { throw null; } }

        public abstract bool IsInvalid { get; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        ~CriticalHandle() {
        }

        protected abstract bool ReleaseHandle();
        protected void SetHandle(IntPtr handle) { }

        public void SetHandleAsInvalid() { }
    }

    public abstract partial class SafeHandle : IDisposable
    {
        protected IntPtr handle;
        protected SafeHandle(IntPtr invalidHandleValue, bool ownsHandle) { }

        public bool IsClosed { get { throw null; } }

        public abstract bool IsInvalid { get; }

        public void DangerousAddRef(ref bool success) { }

        public IntPtr DangerousGetHandle() { throw null; }

        public void DangerousRelease() { }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        ~SafeHandle() {
        }

        protected abstract bool ReleaseHandle();
        protected void SetHandle(IntPtr handle) { }

        public void SetHandleAsInvalid() { }
    }
}

namespace System.Threading
{
    public static partial class WaitHandleExtensions
    {
        public static Microsoft.Win32.SafeHandles.SafeWaitHandle GetSafeWaitHandle(this WaitHandle waitHandle) { throw null; }

        public static void SetSafeWaitHandle(this WaitHandle waitHandle, Microsoft.Win32.SafeHandles.SafeWaitHandle value) { }
    }
}