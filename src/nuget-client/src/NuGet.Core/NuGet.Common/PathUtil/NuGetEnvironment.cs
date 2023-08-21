// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// BCL annotations on Environment.GetEnvironmentVariable makes this file difficult to annotate in .NET 5+
#nullable disable

using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace NuGet.Common
{
    public static class NuGetEnvironment
    {
        private const string DotNet = "dotnet";
        private const string DotNetExe = "dotnet.exe";
        private const string Home = "HOME";
        private const string UserProfile = "USERPROFILE";
#if IS_CORECLR
        private const string DotNetHome = "DOTNET_CLI_HOME";
#endif

        private static readonly Lazy<string> _getHome = new Lazy<string>(() => GetHome());

        private static string _nuGetTempDirectory = null;
        internal static string NuGetTempDirectory
        {
            get { return _nuGetTempDirectory ??= GetNuGetTempDirectory(); }
        }

        private static string GetNuGetTempDirectory()
        {
            var nuGetScratch = Environment.GetEnvironmentVariable("NUGET_SCRATCH");
            if (string.IsNullOrEmpty(nuGetScratch))
            {
#pragma warning disable RS0030 // Do not used banned APIs
                // This is the only place in the product code we can use GetTempPath().
                var tempPath = Path.GetTempPath();
#pragma warning restore RS0030 // Do not used banned APIs

                // On Windows and Mac the temp directories are per-user, but on Linux it's /tmp for everyone, so append the username on Linux.
                nuGetScratch = Path.Combine(tempPath,
                    RuntimeEnvironmentHelper.IsLinux ? "NuGetScratch" + Environment.UserName : "NuGetScratch");

                if (RuntimeEnvironmentHelper.IsLinux)
                {
                    Directory.CreateDirectory(nuGetScratch);
                    if (chmod(nuGetScratch, 0b111_000_000) != 0)   //0b111_000_000 = 700 permissions
                    {
                        // Another user created a folder pretending to be us! 
                        var errno = Marshal.GetLastWin32Error(); // fetch the errno before running any other operation
                        throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                            Strings.UnableToSetNuGetTempFolderPermission,
                            nuGetScratch,
                            errno));
                    }
                }
            }
            return nuGetScratch;
        }

        public static string GetFolderPath(NuGetFolderPath folder)
        {
            switch (folder)
            {
                case NuGetFolderPath.MachineWideSettingsBaseDirectory:
                    string machineWideBaseDir;
                    if (RuntimeEnvironmentHelper.IsWindows)
                    {
                        machineWideBaseDir = GetFolderPath(SpecialFolder.ProgramFilesX86);
                        if (string.IsNullOrEmpty(machineWideBaseDir))
                        {
                            // On 32-bit Windows
                            machineWideBaseDir = GetFolderPath(SpecialFolder.ProgramFiles);
                        }
                    }
                    else
                    {
                        machineWideBaseDir = GetFolderPath(SpecialFolder.CommonApplicationData);
                    }
                    return Path.Combine(machineWideBaseDir,
                        "NuGet");

                case NuGetFolderPath.MachineWideConfigDirectory:
                    return Path.Combine(
                        GetFolderPath(NuGetFolderPath.MachineWideSettingsBaseDirectory),
                        "Config");

                case NuGetFolderPath.UserSettingsDirectory:
                    return Path.Combine(
                        GetFolderPath(SpecialFolder.ApplicationData),
                        "NuGet");

                case NuGetFolderPath.NuGetHome:
                    return Path.Combine(
                        GetFolderPath(SpecialFolder.UserProfile),
                        ".nuget");

                case NuGetFolderPath.HttpCacheDirectory:
                    if (RuntimeEnvironmentHelper.IsWindows)
                    {
                        return Path.Combine(
                            GetFolderPath(SpecialFolder.LocalApplicationData),
                            "NuGet",
                            "v3-cache");
                    }
                    else
                    {
                        return Path.Combine(
                            GetFolderPath(SpecialFolder.LocalApplicationData),
                            "NuGet",
                            "http-cache");
                    }

                case NuGetFolderPath.NuGetPluginsCacheDirectory:
                    if (RuntimeEnvironmentHelper.IsWindows)
                    {
                        return Path.Combine(
                            GetFolderPath(SpecialFolder.LocalApplicationData),
                            "NuGet",
                            "plugins-cache");
                    }
                    else
                    {
                        return Path.Combine(
                            GetFolderPath(SpecialFolder.LocalApplicationData),
                            "NuGet",
                            "plugin-cache");
                    }

                case NuGetFolderPath.DefaultMsBuildPath:
                    var programFilesPath = GetFolderPath(SpecialFolder.ProgramFilesX86);
                    if (string.IsNullOrEmpty(programFilesPath))
                    {
                        // On 32-bit Windows
                        programFilesPath = GetFolderPath(SpecialFolder.ProgramFiles);
                    }

                    return Path.Combine(programFilesPath, "MSBuild", "14.0", "Bin", "MSBuild.exe");

                case NuGetFolderPath.Temp:
                    {
                        return NuGetTempDirectory;
                    }

                default:
                    return null;
            }
        }

        /// <summary>Only to be used for setting permissions of directories under /tmp on Linux. Do not use elsewhere.</summary>
        [DllImport("libc", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern int chmod(string pathname, int mode);


#if IS_CORECLR

        internal static string GetFolderPath(SpecialFolder folder)
        {
            switch (folder)
            {
                case SpecialFolder.ProgramFilesX86:
                    return Environment.GetEnvironmentVariable("PROGRAMFILES(X86)");

                case SpecialFolder.ProgramFiles:
                    return Environment.GetEnvironmentVariable("PROGRAMFILES");

                case SpecialFolder.UserProfile:
                    return _getHome.Value;

                case SpecialFolder.CommonApplicationData:
                    if (RuntimeEnvironmentHelper.IsWindows)
                    {
                        var programData = Environment.GetEnvironmentVariable("PROGRAMDATA");

                        if (!string.IsNullOrEmpty(programData))
                        {
                            return programData;
                        }

                        return Environment.GetEnvironmentVariable("ALLUSERSPROFILE");
                    }
                    else if (RuntimeEnvironmentHelper.IsMacOSX)
                    {
                        return @"/Library/Application Support";
                    }
                    else
                    {
                        var commonApplicationDataOverride = Environment.GetEnvironmentVariable("NUGET_COMMON_APPLICATION_DATA");

                        if (!string.IsNullOrEmpty(commonApplicationDataOverride))
                        {
                            return commonApplicationDataOverride;
                        }

                        return @"/etc/opt";
                    }

                case SpecialFolder.ApplicationData:
                    if (RuntimeEnvironmentHelper.IsWindows)
                    {
                        return Environment.GetEnvironmentVariable("APPDATA");
                    }
                    else
                    {
                        return Path.Combine(_getHome.Value, ".nuget");
                    }

                case SpecialFolder.LocalApplicationData:
                    if (RuntimeEnvironmentHelper.IsWindows)
                    {
                        return Environment.GetEnvironmentVariable("LOCALAPPDATA");
                    }
                    else
                    {
                        var xdgDataHome = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
                        if (!string.IsNullOrEmpty(xdgDataHome))
                        {
                            return xdgDataHome;
                        }

                        return Path.Combine(_getHome.Value, ".local", "share");
                    }

                default:
                    return null;
            }
        }

        private static string GetDotNetHome()
        {
            return Environment.GetEnvironmentVariable(DotNetHome);
        }

#else

        internal static string GetFolderPath(SpecialFolder folder)
        {
            // Convert the private enum to the .NET Framework enum
            Environment.SpecialFolder converted;
            switch (folder)
            {
                case SpecialFolder.ProgramFilesX86:
                    converted = Environment.SpecialFolder.ProgramFilesX86;
                    break;

                case SpecialFolder.ProgramFiles:
                    converted = Environment.SpecialFolder.ProgramFiles;
                    break;

                case SpecialFolder.UserProfile:
                    var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                    // On Kudu this might return null
                    if (!string.IsNullOrEmpty(userProfile))
                    {
                        return userProfile;
                    }

                    return _getHome.Value;

                case SpecialFolder.CommonApplicationData:
                    converted = Environment.SpecialFolder.CommonApplicationData;
                    break;

                case SpecialFolder.ApplicationData:
                    converted = Environment.SpecialFolder.ApplicationData;
                    break;

                case SpecialFolder.LocalApplicationData:
                    converted = Environment.SpecialFolder.LocalApplicationData;
                    break;

                default:
                    return null;
            }

            return Environment.GetFolderPath(converted);
        }

#endif

        private static string GetHome()
        {
#if IS_CORECLR
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                return GetValueOrThrowMissingEnvVarsDotnet(() => GetDotNetHome() ?? GetHomeWindows(), UserProfile, DotNetHome);
            }
            else
            {
                return GetValueOrThrowMissingEnvVarsDotnet(() => GetDotNetHome() ?? Environment.GetEnvironmentVariable(Home), Home, DotNetHome);
            }
#else
            if (RuntimeEnvironmentHelper.IsWindows)
            {
                return GetValueOrThrowMissingEnvVar(() => GetHomeWindows(), UserProfile);
            }
            else
            {
                return GetValueOrThrowMissingEnvVar(() => Environment.GetEnvironmentVariable(Home), Home);
            }
#endif
        }

        private static string GetHomeWindows()
        {
            var userProfile = Environment.GetEnvironmentVariable(UserProfile);
            if (!string.IsNullOrEmpty(userProfile))
            {
                return userProfile;
            }
            else
            {
                return Environment.GetEnvironmentVariable("HOMEDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH");
            }
        }

        /// <summary>
        /// Throw a helpful message if the required env vars are not set.
        /// </summary>
        private static string GetValueOrThrowMissingEnvVarsDotnet(Func<string> getValue, string home, string dotnetHome)
        {
            var value = getValue();

            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.MissingRequiredEnvVarsDotnet, home, dotnetHome));
            }

            return value;
        }

        /// <summary>
        /// Throw a helpful message if a required env var is not set.
        /// </summary>
        private static string GetValueOrThrowMissingEnvVar(Func<string> getValue, string name)
        {
            var value = getValue();

            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.MissingRequiredEnvVar, name));
            }

            return value;
        }

        public static string GetDotNetLocation()
        {
            var path = Environment.GetEnvironmentVariable("PATH");
            var isWindows = RuntimeEnvironmentHelper.IsWindows;
            var splitChar = isWindows ? ';' : ':';
            var executable = isWindows ? DotNetExe : DotNet;

            foreach (var dir in path.Split(splitChar))
            {
                var fullPath = Path.Combine(dir, executable);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }

            if (isWindows)
            {
                var programFiles = GetFolderPath(SpecialFolder.ProgramFiles);
                if (!string.IsNullOrEmpty(programFiles))
                {
                    var fullPath = Path.Combine(programFiles, DotNet, DotNetExe);
                    if (File.Exists(fullPath))
                    {
                        return fullPath;
                    }
                }

                programFiles = GetFolderPath(SpecialFolder.ProgramFilesX86);
                if (!string.IsNullOrEmpty(programFiles))
                {
                    var fullPath = Path.Combine(programFiles, DotNet, DotNetExe);
                    if (File.Exists(fullPath))
                    {
                        return fullPath;
                    }
                }
            }
            else
            {
                var localBin = "/usr/local/bin";
                if (!string.IsNullOrEmpty(localBin))
                {
                    var fullPath = Path.Combine(localBin, DotNet);
                    if (File.Exists(fullPath))
                    {
                        return fullPath;
                    }
                }
            }

            return DotNet;
        }

        /// <summary>
        /// Since <see cref="Environment.SpecialFolder"/> is not available on .NET Core, we have to
        /// make our own and re-implement the functionality. On .NET Framework, we can use the
        /// built-in functionality.
        /// </summary>
        internal enum SpecialFolder
        {
            ProgramFilesX86,
            ProgramFiles,
            UserProfile,
            CommonApplicationData,
            ApplicationData,
            LocalApplicationData
        }
    }
}
