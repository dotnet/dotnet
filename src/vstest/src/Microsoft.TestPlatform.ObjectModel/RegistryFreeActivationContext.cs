// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <remarks>
/// Enable registry-free COM context
/// </remarks>
internal class RegistryFreeActivationContext : IDisposable
{
    private IntPtr _cookie = IntPtr.Zero;

    private IntPtr _hActCtx = IntPtr.Zero;

    private bool _disposed;

    private readonly string _manifestFilePath = string.Empty;

    /// <summary>
    /// Initializes a new instance of RegistryFreeActivationContext class.
    /// </summary>
    ///<param name="manifest">Manifest file path.</param>
    public RegistryFreeActivationContext(string manifest)
    {
        _manifestFilePath = manifest;
    }

    /// <summary>
    /// Finalize an instance of RegistryFreeActivationContext class.
    /// </summary>
    ~RegistryFreeActivationContext()
    {
        Dispose(false);
    }

    /// <summary>
    /// Load manifest to enable registry-free COM context.
    /// </summary>
    public void ActivateContext()
    {
        if (_cookie != IntPtr.Zero || _hActCtx != IntPtr.Zero)
        {
            return;
        }

        ActivationContextNativeMethods.ACTCTX context = new();
        context.cbSize = Marshal.SizeOf(typeof(ActivationContextNativeMethods.ACTCTX));

        context.lpSource = _manifestFilePath;

        _hActCtx = ActivationContextNativeMethods.CreateActCtx(ref context);
        if (_hActCtx == (IntPtr)(-1))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error(), "Fail to create registry-free COM context");
        }
        if (!ActivationContextNativeMethods.ActivateActCtx(_hActCtx, out _cookie))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error(), "Fail to activate registry-free COM context");
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            // No managed resources to release
        }

        DeactivateContext();
        _disposed = true;
    }

    /// <summary>
    /// Disable registry-free COM context.
    /// </summary>
    private void DeactivateContext()
    {
        if (_cookie == IntPtr.Zero && _hActCtx == IntPtr.Zero)
        {
            return;
        }

        try
        {
            ActivationContextNativeMethods.DeactivateActCtx(0, _cookie);
            ActivationContextNativeMethods.ReleaseActCtx(_hActCtx);
            _cookie = IntPtr.Zero;
            _hActCtx = IntPtr.Zero;
        }
        catch (Exception ex)
        {
            // Log any exceptions during deactivation.
            EqtTrace.Error(ex);
        }
    }
}

/// ActivationContextNativeMethods class needed for registry-free context
/// </summary>
internal static class ActivationContextNativeMethods
{
    // Activation Context API Functions
    [DllImport("Kernel32.dll", SetLastError = true, EntryPoint = "CreateActCtxW")]
    public extern static IntPtr CreateActCtx(ref ACTCTX actctx);

    [DllImport("Kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ActivateActCtx(IntPtr hActCtx, out IntPtr lpCookie);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeactivateActCtx(int dwFlags, IntPtr lpCookie);

    [DllImport("Kernel32.dll", SetLastError = true)]
    public static extern void ReleaseActCtx(IntPtr hActCtx);

    // Activation context structure
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Layout struct")]
    public struct ACTCTX
    {
        public Int32 cbSize;
        public UInt32 dwFlags;
        public string lpSource;
        public UInt16 wProcessorArchitecture;
        public UInt16 wLangId;
        public string lpAssemblyDirectory;
        public string lpResourceName;
        public string lpApplicationName;
        public IntPtr hModule;
    }
}

#endif
