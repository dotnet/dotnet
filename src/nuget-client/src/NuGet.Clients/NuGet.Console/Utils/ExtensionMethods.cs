// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using NuGet.PackageManagement;

namespace NuGetConsole
{
    internal static class ExtensionMethods
    {
        public static SnapshotPoint GetEnd(this ITextSnapshot snapshot)
        {
            return new SnapshotPoint(snapshot, snapshot.Length);
        }

        /// <summary>
        /// Removes a ReadOnlyRegion and clears the reference (set to null).
        /// </summary>
        public static void ClearReadOnlyRegion(this IReadOnlyRegionEdit readOnlyRegionEdit, ref IReadOnlyRegion readOnlyRegion)
        {
            if (readOnlyRegion != null)
            {
                readOnlyRegionEdit.RemoveReadOnlyRegion(readOnlyRegion);
                readOnlyRegion = null;
            }
        }

        public static void Raise<T>(this EventHandler<NuGetEventArgs<T>> ev, object sender, T arg) where T : class
        {
            if (ev != null)
            {
                ev(sender, new NuGetEventArgs<T>(arg));
            }
        }

        /// <summary>
        /// Execute a VS command on the wpfTextView CommandTarget.
        /// </summary>
        public static void Execute(this IOleCommandTarget target, Guid guidCommand, uint idCommand, object args = null)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            IntPtr varIn = IntPtr.Zero;
            try
            {
                if (args != null)
                {
                    varIn = Marshal.AllocHGlobal(NativeMethods.VariantSize);
                    Marshal.GetNativeVariantForObject(args, varIn);
                }

                int hr = target.Exec(ref guidCommand, idCommand, (uint)OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, varIn, IntPtr.Zero);
                ErrorHandler.ThrowOnFailure(hr);
            }
            finally
            {
                if (varIn != IntPtr.Zero)
                {
                    NativeMethods.VariantClear(varIn);
                    Marshal.FreeHGlobal(varIn);
                }
            }
        }

        /// <summary>
        /// Execute a default VSStd2K command.
        /// </summary>
        public static void Execute(this IOleCommandTarget target, VSConstants.VSStd2KCmdID idCommand, object args = null)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            target.Execute(VSConstants.VSStd2K, (uint)idCommand, args);
        }
    }
}
