// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace NuGet.Common
{
    public static class RuntimeEnvironmentHelper
    {
        private static readonly string[] VisualStudioProcesses = { "DEVENV", "BLEND" };

        private static Lazy<bool> _isMono = new Lazy<bool>(() => Type.GetType("Mono.Runtime") != null);

        private static Lazy<bool> _isWindows = new Lazy<bool>(() => GetIsWindows());

        private static Lazy<bool> _IsMacOSX = new Lazy<bool>(() => GetIsMacOSX());

        private static Lazy<bool> _IsLinux = new Lazy<bool>(() => GetIsLinux());

        private static Lazy<bool> _isRunningInVisualStudio = new Lazy<bool>(() =>
        {
            if (!IsWindows)
            {
                return false;
            }

            var currentProcessName = Path.GetFileNameWithoutExtension(GetCurrentProcessFilePath());

            return VisualStudioProcesses.Any(
                process => process.Equals(currentProcessName, StringComparison.OrdinalIgnoreCase));
        });

        [DllImport("libc")]
        static extern int uname(IntPtr buf);

        public static bool IsWindows
        {
            get => _isWindows.Value;
        }

        private static bool GetIsWindows()
        {
#if IS_CORECLR
            // This API does work on full framework but it requires a newer nuget client (RID aware)
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                return true;
            }

            return false;
#else
            var platform = (int)Environment.OSVersion.Platform;
            return (platform != 4) && (platform != 6) && (platform != 128);
#endif
        }

        public static bool IsMono
        {
            get
            {
                if (IsRunningInVisualStudio)
                {
                    // skip Mono type check if current process is Devenv
                    return false;
                }

                return _isMono.Value;
            }
        }

        public static bool IsRunningInVisualStudio
        {
            get
            {
                return _isRunningInVisualStudio.Value;
            }
        }

        private static string GetCurrentProcessFilePath()
        {
            using (var process = Process.GetCurrentProcess())
            {
                return process.MainModule!.FileName;
            }
        }

        public static bool IsMacOSX
        {
            get => _IsMacOSX.Value;
        }

        private static bool GetIsMacOSX()
        {
#if IS_CORECLR
            // This API does work on full framework but it requires a newer nuget client (RID aware)
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
            {
                return true;
            }

            return false;
#else
            var buf = IntPtr.Zero;

            try
            {
                buf = Marshal.AllocHGlobal(8192);

                // This is a hacktastic way of getting sysname from uname ()
                if (uname(buf) == 0)
                {
                    var os = Marshal.PtrToStringAnsi(buf);

                    if (os == "Darwin")
                    {
                        return true;
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (buf != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(buf);
                }
            }

            return false;
#endif
        }

        public static bool IsLinux
        {
            get => _IsLinux.Value;
        }

        private static bool GetIsLinux()
        {
#if IS_CORECLR
            // This API does work on full framework but it requires a newer nuget client (RID aware)
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
            {
                return true;
            }

            // The OSPlatform.FreeBSD property only exists in .NET Core 3.1 and higher, whereas this project is also
            // compiled for .NET Standard and .NET Framework, where an OSPlatform for FreeBSD must be created manually
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Create("FREEBSD")))
            {
                return true;
            }

            return false;
#else
            var platform = (int)Environment.OSVersion.Platform;
            return platform == 4;
#endif
        }
    }
}
