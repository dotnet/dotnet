// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Deployment.Utilities;

namespace Microsoft.Deployment.Launcher
{
    internal static class ResourceReader
    {
        /// <summary>
        /// Gets the name of the application, from the custom resource.
        /// </summary>
        /// <param name="path">Path to file from which to read the resource.</param>
        /// <returns>Application filename</returns>
        public static string GetApplicationFilename(string path)
        {
            return ReadString(path, NativeMethods.Launcher_CustomResourceTypePtr, NativeMethods.Launcher_ResourceName);
        }

        /// <summary>
        /// Reads resource string from a resource type.
        /// </summary>
        /// <param name="path">Path to file from which to read the resource</param>
        /// <param name="type">Resource type</param>
        /// <param name="name">Resource name</param>
        /// <returns>Resource value</returns>
        private static string ReadString(string path, IntPtr type, string name)
        {
            IntPtr hModule = IntPtr.Zero;
            string binaryToLaunch = string.Empty;

            try
            {
                hModule = NativeMethods.Kernel32.LoadLibraryExW(path, IntPtr.Zero, NativeMethods.Kernel32.LOAD_LIBRARY_AS_DATAFILE);
                if (hModule != IntPtr.Zero)
                {
                    binaryToLaunch = GetResourceString(hModule, type, name);
                }
            }
            finally
            {
                if (hModule != IntPtr.Zero)
                {
                    NativeMethods.Kernel32.FreeLibrary(hModule);
                }
            }

            return binaryToLaunch;
        }

        /// <summary>
        /// Gets the value of a resource string from module.
        /// </summary>
        /// <param name="hModule">Module handle</param>
        /// <param name="pType">Resource type</param>
        /// <param name="pName">Resource name</param>
        /// <returns>Resource value</returns>
        private static string GetResourceString(IntPtr hModule, IntPtr pType, string pName)
        {
            string value = string.Empty;
            IntPtr hResInfo = NativeMethods.Kernel32.FindResource(hModule, pName, pType);
            if (hResInfo == IntPtr.Zero)
            {
                throw new LauncherException(Constants.ErrorResourceMissing, pName);
            }

            IntPtr hResource = NativeMethods.Kernel32.LoadResource(hModule, hResInfo);
            if (hResource == IntPtr.Zero)
            {
                throw new LauncherException(Constants.ErrorResourceFailedToLoad, pName);
            }

            IntPtr hResLock = NativeMethods.Kernel32.LockResource(hResource);
            if (hResLock == IntPtr.Zero)
            {
                throw new LauncherException(Constants.ErrorResourceFailedToLock, pName);
            }

            uint bufsize = NativeMethods.Kernel32.SizeofResource(hModule, hResInfo);
            byte[] buffer = new byte[bufsize];

            Marshal.Copy(hResource, buffer, 0, buffer.Length);
            value = Encoding.Unicode.GetString(buffer, 0, buffer.Length);


            // Strip all leading or trailing '\0' characters.
            // Trailing '\0' is always added by Visual Studio and Mage tool.

            return value.Trim('\0');
        }
    }
}
