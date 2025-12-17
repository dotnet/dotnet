// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

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

        public static bool IsWindows
        {
            get => _isWindows.Value;
        }

        private static bool GetIsWindows()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
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
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);
        }

        public static bool IsLinux
        {
            get => _IsLinux.Value;
        }

        private static bool GetIsLinux()
        {
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
        }
    }
}
