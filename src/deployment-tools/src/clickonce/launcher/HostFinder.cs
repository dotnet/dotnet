// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Deployment.Utilities;

namespace Microsoft.Deployment.Launcher
{
    internal static class HostFinder
    {
        internal static readonly string ArchMsil = "msil";
        internal static readonly string ArchX86   = "x86";
        internal static readonly string ArchX64   = "amd64";
        internal static readonly string ArchArm64 = "arm64";

        /// <summary>
        /// Gets full path to .NET host appropriate for activating the application.
        /// 
        /// .NET host's location can be obtained from multiple locations.
        ///
        /// Current code searches in default, global shared runtime, location only:
        /// %ProgramFiles%\dotnet and %ProgramFiles(x86)%\dotnet
        /// 
        /// Consider adding support for other non-standard registrations of host location.
        /// 1) Environment variables: DOTNET_ROOT or DOTNET_ROOT(x86)
        /// 2) Registry: HKLM\SOFTWARE\dotnet\Setup\InstalledVersions\{arch}\[InstallLocation]
        /// 
        /// Order of search should eventually be:
        /// 1) Environment variables
        /// 2) Registry
        /// 3) Global location
        /// </summary>
        /// <param name="applicationFilePath">Full path to application</param>
        /// <returns></returns>
        internal static string GetHost(string applicationFilePath)
        {
            string arch = GetProcessorArchitectureFromAssembly(applicationFilePath);

            if (arch == ArchX86)
            {
                return GetGlobalHost(arch);
            }
            else if (arch == ArchX64 || arch == ArchArm64)
            {
                return Environment.Is64BitOperatingSystem ? GetGlobalHost(arch) : string.Empty;
            }
            else if (arch == "msil")
            {
                string host = string.Empty;

                // On arm64 systems, first look for arm64 host
                if (IsArm64System)
                {
                    host = GetGlobalHost(ArchArm64);
                }

                // Fallback for arm64 systems, and native x64 systems
                if (string.IsNullOrEmpty(host))
                {
                    host = GetGlobalHost(ArchX64);
                }

                return string.IsNullOrEmpty(host) ? GetGlobalHost(ArchX86) : host;
            }

            return string.Empty;
        }

        /// <summary>
        /// Checks if running on Arm64 system
        /// </summary>
        private static bool IsArm64System
        {
            get
            {
                try
                {
                    if (NativeMethods.Kernel32.IsWow64Process2(new IntPtr(-1), out _, out ushort nativeMachine))
                    {
                        if (nativeMachine == NativeMethods.Kernel32.IMAGE_FILE_MACHINE_ARM64)
                        {
                            return true;
                        }
                    }
                }
                catch (EntryPointNotFoundException)
                {
                    // kernel32.dll does not export IsWow64Process2 on systems before Win10
                    // API is available on all systems that support arm64.
                }

                return false;
            }
        }

        /// <summary>
        /// Gets ProgramFiles folder for the specified bitness.
        /// </summary>
        /// <param name="is64bit">If 64-bit bitness is required</param>
        /// <returns></returns>
        private static string GetProgramFilesFolder(bool is64bit = false)
        {
            return is64bit ?
                (Environment.Is64BitProcess ?
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) :
                    Environment.GetEnvironmentVariable("ProgramW6432")) :
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        }

        /// <summary>
        /// Get global host if it exists, for the specified bitness.
        /// </summary>
        /// <param name="arch">Architecture</param>
        /// <returns></returns>
        private static string GetGlobalHost(string arch)
        {
            bool is64bit = arch == ArchX86 ? false : true;
            string folder = GetProgramFilesFolder(is64bit);
            string relativeHostPath = "dotnet\\dotnet.exe";

            // On Arm64 systems, x64 host is in "x64" sub-folder
            if (is64bit && arch == ArchX64 && IsArm64System)
            {
                relativeHostPath = "dotnet\\x64\\dotnet.exe";
            }

            string host = !string.IsNullOrEmpty(folder) ? Path.Combine(folder, relativeHostPath) : string.Empty;
            return File.Exists(host) ? host : string.Empty;
        }

        /// <summary>
        /// Gets processor architecture from assembly metadata.
        /// </summary>
        /// <param name="path">Assembly path</param>
        /// <returns></returns>
        private static string GetProcessorArchitectureFromAssembly(string path)
        {
            string processorArchitecture = string.Empty;

            try
            {
                Guid riid = GetGuidOfType(typeof(NativeMethods.Clr.IReferenceIdentity));
                NativeMethods.Clr.IReferenceIdentity refid = (NativeMethods.Clr.IReferenceIdentity)NativeMethods.Clr.GetAssemblyIdentityFromFile(path, ref riid);
                if (refid != null)
                {
                    processorArchitecture = refid.GetAttribute(null, "processorArchitecture");
                }
            }
            catch (Exception)
            {
                // GetAssemblyIdentityFromFile throws an exception for architectures that don't exist in .NET FX, i.e. arm64.
            }

            // Default to "msil", to support failure scenarios, like GetAssemblyIdentityFromFile returning null
            // or encountering an unknown architecture.
            return string.IsNullOrEmpty(processorArchitecture) ?
                        ArchMsil :
                        processorArchitecture.ToLowerInvariant();
        }

        private static Guid GetGuidOfType(Type type)
        {
            var guidAttr = (GuidAttribute)Attribute.GetCustomAttribute(type, typeof(GuidAttribute), false);
            return new Guid(guidAttr.Value);
        }
    }
}
