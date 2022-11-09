// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.Deployment.Utilities
{
    internal static class ResourceUpdater
    {
        /// <summary>
        /// Sets the filename of application to launch, in the custom resource.
        /// </summary>
        /// <param name="path">Path to the file in which to set the resource.</param>
        /// <param name="filename">Filename of application to launch.</param>
        /// <returns>Success or failure</returns>
        public static bool SetApplicationFilename(string path, string filename)
        {
            return WriteString(path, NativeMethods.Launcher_CustomResourceTypePtr, NativeMethods.Launcher_ResourceName, filename);
        }

        /// <summary>
        /// Writes resource string with specific resource type and value.
        /// </summary>
        /// <param name="path">Path to a file in which to write the resource</param>
        /// <param name="type">Resource type</param>
        /// <param name="name">Resource name</param>
        /// <param name="value">Resource value</param>
        /// <returns>Success or failure</returns>
        private static bool WriteString(string path, IntPtr type, string name, string value)
        {
            bool returnValue = true;
            int beginUpdateRetries = 20;
            const int beginUpdateRetryInterval = 100; // In milliseconds
            bool endUpdate = false;

            IntPtr hUpdate = IntPtr.Zero;

            try
            {
                hUpdate = NativeMethods.Kernel32.BeginUpdateResourceW(path, false);
                while (IntPtr.Zero == hUpdate &&
                       Marshal.GetHRForLastWin32Error() == NativeMethods.Kernel32.ERROR_SHARING_VIOLATION &&
                       beginUpdateRetries > 0)
                {
                    hUpdate = NativeMethods.Kernel32.BeginUpdateResourceW(path, false);
                    beginUpdateRetries--;
                    Thread.Sleep(beginUpdateRetryInterval);
                }

                if (IntPtr.Zero == hUpdate)
                {
                    return false;
                }

                endUpdate = true;

                if (hUpdate != IntPtr.Zero)
                {
                    byte[] dataBytes = GetZeroTerminatedByteArray(value);

                    if (!NativeMethods.Kernel32.UpdateResourceW(hUpdate, type, name, 0, dataBytes, dataBytes.Length))
                    {
                        return false;
                    }
                }
            }
            finally
            {
                if (endUpdate && !NativeMethods.Kernel32.EndUpdateResource(hUpdate, false))
                {
                    returnValue = false;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Gets a zero-terminated byte array from the input string.
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>Byte array</returns>
        private static byte[] GetZeroTerminatedByteArray(string str)
        {
            // Append zero-termination to match MSBuild code.

            byte[] strBytes = System.Text.Encoding.Unicode.GetBytes(str);
            byte[] data = new byte[strBytes.Length + 2];
            strBytes.CopyTo(data, 0);
            data[data.Length - 2] = 0;
            data[data.Length - 1] = 0;
            return data;
        }
    }
}
