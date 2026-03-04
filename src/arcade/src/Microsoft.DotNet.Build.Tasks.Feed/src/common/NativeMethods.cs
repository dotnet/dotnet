// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    internal partial class NativeMethods
    {
        [LibraryImport("kernel32.dll", EntryPoint = "CreateHardLinkW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool CreateHardLink(string newFileName, string exitingFileName, IntPtr securityAttributes);

        [LibraryImport("libc", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial int link(string oldpath, string newpath);

        internal static bool MakeHardLink(string newFileName, string exitingFileName, ref string errorMessage)
        {
            bool hardLinkCreated;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                hardLinkCreated = CreateHardLink(newFileName, exitingFileName, IntPtr.Zero /* reserved, must be NULL */);
                errorMessage = hardLinkCreated ? null : Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error()).Message;
            }
            else
            {
                hardLinkCreated = link(exitingFileName, newFileName) == 0;
                errorMessage = hardLinkCreated ? null : $"The link() library call failed with the following error code: {Marshal.GetLastWin32Error()}.";
            }

            return hardLinkCreated;
        }
    }
}
