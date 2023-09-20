// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Text.RegularExpressions;

#nullable enable

namespace Microsoft.Build.Locator
{
    internal static class DotNetSdkLocationHelper
    {
        private static readonly Regex VersionRegex = new Regex(@"^(\d+)\.(\d+)\.(\d+)", RegexOptions.Multiline);
        private static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        private static readonly string ExeName = IsWindows ? "dotnet.exe" : "dotnet";
        private static readonly Lazy<string> DotnetPath = new(() => ResolveDotnetPath());

        public static VisualStudioInstance? GetInstance(string dotNetSdkPath)
        {
            if (string.IsNullOrWhiteSpace(dotNetSdkPath) || !File.Exists(Path.Combine(dotNetSdkPath, "Microsoft.Build.dll")))
            {
                return null;
            }

            string versionPath = Path.Combine(dotNetSdkPath, ".version");
            if (!File.Exists(versionPath))
            {
                return null;
            }

            // Preview versions contain a hyphen after the numeric part of the version. Version.TryParse doesn't accept that.
            Match versionMatch = VersionRegex.Match(File.ReadAllText(versionPath));

            if (!versionMatch.Success)
            {
                return null;
            }

            if (!int.TryParse(versionMatch.Groups[1].Value, out int major) ||
                !int.TryParse(versionMatch.Groups[2].Value, out int minor) ||
                !int.TryParse(versionMatch.Groups[3].Value, out int patch))
            {
                return null;
            }

            // Components of the SDK often have dependencies on the runtime they shipped with, including that several tasks that shipped
            // in the .NET 5 SDK rely on the .NET 5.0 runtime. Assuming the runtime that shipped with a particular SDK has the same version,
            // this ensures that we don't choose an SDK that doesn't work with the runtime of the chosen application. This is not guaranteed
            // to always work but should work for now.
            if (major > Environment.Version.Major ||
                (major == Environment.Version.Major && minor > Environment.Version.Minor))
            {
                return null;
            }

            return new VisualStudioInstance(
                name: ".NET Core SDK",
                path: dotNetSdkPath,
                version: new Version(major, minor, patch),
                discoveryType: DiscoveryType.DotNetSdk);
        }

        public static IEnumerable<VisualStudioInstance> GetInstances(string workingDirectory)
        {            
            foreach (var basePath in GetDotNetBasePaths(workingDirectory))
            {
                var dotnetSdk = GetInstance(basePath);
                if (dotnetSdk != null)
                {
                    yield return dotnetSdk;
                }
            }
        }

        private static IEnumerable<string> GetDotNetBasePaths(string workingDirectory)
        {
            try
            {
                AddUnmanagedDllResolver();

                string? bestSDK = GetSdkFromGlobalSettings(workingDirectory);
                if (!string.IsNullOrEmpty(bestSDK))
                {
                    yield return bestSDK;
                }

                string[] dotnetPaths = GetAllAvailableSDKs();
                // We want to return the newest SDKs first, however, so iterate over the list in reverse order.
                // If basePath is disqualified because it was later
                // than the runtime version, this ensures that RegisterDefaults will return the latest valid
                // SDK instead of the earliest installed.
                for (int i = dotnetPaths.Length - 1; i >= 0; i--)
                {
                    if (dotnetPaths[i] != bestSDK)
                    {
                        yield return dotnetPaths[i];
                    }
                }
            }
            finally
            {
                RemoveUnmanagedDllResolver();
            }
        }

        private static void AddUnmanagedDllResolver() => ModifyUnmanagedDllResolver(loadContext => loadContext.ResolvingUnmanagedDll += HostFxrResolver);

        private static void RemoveUnmanagedDllResolver() => ModifyUnmanagedDllResolver(loadContext => loadContext.ResolvingUnmanagedDll -= HostFxrResolver);

        private static void ModifyUnmanagedDllResolver(Action<AssemblyLoadContext> resolverAction)
        {
            // For Windows hostfxr is loaded in the process.
            if (!IsWindows)
            {
                var loadContext = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly());
                if (loadContext != null)
                {
                    resolverAction(loadContext);
                }
            }
        }

        private static IntPtr HostFxrResolver(Assembly assembly, string libraryName)
        {
            // the DllImport hardcoded the name as hostfxr.
            if (!libraryName.Equals(NativeMethods.HostFxrName, StringComparison.Ordinal))
            {
                return IntPtr.Zero;
            }

            string hostFxrLibName =
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                "hostfxr.dll" :
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "libhostfxr.dylib" : "libhostfxr.so";

            string hostFxrRoot = Path.Combine(DotnetPath.Value, "host", "fxr");
            if (Directory.Exists(hostFxrRoot))
            {
                var fileEnumerable = new FileSystemEnumerable<SemanticVersion?>(
                    directory: hostFxrRoot,
                    transform: static (ref FileSystemEntry entry) => SemanticVersionParser.TryParse(entry.FileName.ToString(), out var version) ? version : null)
                {
                    ShouldIncludePredicate = static (ref FileSystemEntry entry) => entry.IsDirectory
                };

                // Load hostfxr from the highest version, because it should be backward-compatible
                if (fileEnumerable.Max() is SemanticVersion hostFxrVersion)
                {
                    string hostFxrAssembly = Path.Combine(hostFxrRoot, hostFxrVersion.OriginalValue, hostFxrLibName);
                    return NativeLibrary.Load(hostFxrAssembly);
                }
            }

            string error = $".NET SDK cannot be resolved, because {hostFxrLibName} cannot be found inside {hostFxrRoot}." +
                Environment.NewLine +
                $"This might indicate a corrupted SDK installation on the machine.";

            throw new InvalidOperationException(error);
        }

        private static string SdkResolutionExceptionMessage(string methodName) => $"Failed to find all versions of .NET Core MSBuild. Call to {methodName}. There may be more details in stderr.";
        
        /// <summary>
        /// Determines the directory location of the SDK accounting for
        /// global.json and multi-level lookup policy.
        /// </summary>
        private static string? GetSdkFromGlobalSettings(string workingDirectory)
        {
            string? resolvedSdk = null;
            int rc = NativeMethods.hostfxr_resolve_sdk2(exe_dir: DotnetPath.Value, working_dir: workingDirectory, flags: 0, result: (key, value) =>
            {
                if (key == NativeMethods.hostfxr_resolve_sdk2_result_key_t.resolved_sdk_dir)
                {
                    resolvedSdk = value;
                }
            });

            if (rc != 0)
            {
                throw new InvalidOperationException(SdkResolutionExceptionMessage(nameof(NativeMethods.hostfxr_resolve_sdk2)));
            }

            return resolvedSdk;
        }

        private static string ResolveDotnetPath()
        {
            string? dotnetPath = GetDotnetPathFromROOT();

            if (string.IsNullOrEmpty(dotnetPath))
            {
                string? dotnetExePath = GetCurrentProcessPath();
                var isRunFromDotnetExecutable = !string.IsNullOrEmpty(dotnetExePath) 
                    && Path.GetFileName(dotnetExePath).Equals(ExeName, StringComparison.OrdinalIgnoreCase);
                
                if (isRunFromDotnetExecutable)
                {
                    dotnetPath = Path.GetDirectoryName(dotnetExePath);
                }
                else
                {
                    // DOTNET_HOST_PATH is pointing to the file, DOTNET_ROOT is the path of the folder
                    string? hostPath = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH");
                    if (!string.IsNullOrEmpty(hostPath) && File.Exists(hostPath))
                    {
                        if (!IsWindows)
                        {
                            hostPath = realpath(hostPath) ?? hostPath;
                        }

                        dotnetPath = Path.GetDirectoryName(hostPath);
                        if (dotnetPath is not null)
                        {
                            // don't overwrite DOTNET_HOST_PATH, if we use it.
                            return dotnetPath;
                        }
                    }

                    dotnetPath = FindDotnetPathFromEnvVariable("DOTNET_MSBUILD_SDK_RESOLVER_CLI_DIR")
                        ?? GetDotnetPathFromPATH();
                }
            }

            if (string.IsNullOrEmpty(dotnetPath))
            {
                throw new InvalidOperationException("Could not find the dotnet executable. Is it set on the DOTNET_ROOT?");
            }

            SetEnvironmentVariableIfEmpty("DOTNET_HOST_PATH", Path.Combine(dotnetPath, ExeName));

            return dotnetPath;
        }

        private static string? GetDotnetPathFromROOT()
        {
            // 32-bit architecture has (x86) suffix
            string envVarName = (IntPtr.Size == 4) ? "DOTNET_ROOT(x86)" : "DOTNET_ROOT";
            var dotnetPath = FindDotnetPathFromEnvVariable(envVarName);
            
            return dotnetPath;
        }

        private static string? GetCurrentProcessPath() => Environment.ProcessPath;

        private static string? GetDotnetPathFromPATH()
        {
            string? dotnetPath = null;
            // We will generally find the dotnet exe on the path, but on linux, it is often just a 'dotnet' symlink (possibly even to more symlinks) that we have to resolve
            // to the real dotnet executable.
            // This will work as often as just invoking dotnet from the command line, but we can be more confident in finding a dotnet executable by following
            // https://github.com/dotnet/designs/blob/main/accepted/2021/install-location-per-architecture.md
            // This could be done using the nethost library, but this is currently shipped as metadata package (Microsoft.NETCore.DotNetAppHost) and requires the customers
            // to specify <RuntimeIdentifier> for resolving runtime assembly.
            var paths = Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator) ?? Array.Empty<string>();
            foreach (string dir in paths)
            {
                string? filePath = ValidatePath(dir);
                if (!string.IsNullOrEmpty(filePath))
                {
                    dotnetPath = filePath;
                    break;
                }
            }

            return dotnetPath;
        }

        /// <summary>
        /// Returns the list of all available SDKs ordered by ascending version.
        /// </summary>
        private static string[] GetAllAvailableSDKs()
        {
            string[]? resolvedPaths = null;
            int rc = NativeMethods.hostfxr_get_available_sdks(exe_dir: DotnetPath.Value, result: (key, value) => resolvedPaths = value);

            // Errors are automatically printed to stderr. We should not continue to try to output anything if we failed.
            if (rc != 0)
            {
                throw new InvalidOperationException(SdkResolutionExceptionMessage(nameof(NativeMethods.hostfxr_get_available_sdks)));
            }

            return resolvedPaths ?? Array.Empty<string>();
        }

        /// <summary>
        /// This native method call determines the actual location of path, including
        /// resolving symbolic links.
        /// </summary>
        private static string? realpath(string path)
        {
            IntPtr ptr = NativeMethods.realpath(path, IntPtr.Zero);
            string? result = Marshal.PtrToStringAuto(ptr);
            NativeMethods.free(ptr);

            return result;
        }

        private static string? FindDotnetPathFromEnvVariable(string environmentVariable)
        {
            string? dotnetPath = Environment.GetEnvironmentVariable(environmentVariable);
            
            return string.IsNullOrEmpty(dotnetPath) ? null : ValidatePath(dotnetPath);
        }

        private static void SetEnvironmentVariableIfEmpty(string name, string value)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(name)))
            {
                Environment.SetEnvironmentVariable(name, value);
            }
        }

        private static string? ValidatePath(string dotnetPath)
        {
            string fullPathToDotnetFromRoot = Path.Combine(dotnetPath, ExeName);
            if (File.Exists(fullPathToDotnetFromRoot))
            {
                if (!IsWindows)
                {
                    fullPathToDotnetFromRoot = realpath(fullPathToDotnetFromRoot) ?? fullPathToDotnetFromRoot;
                    return File.Exists(fullPathToDotnetFromRoot) ? Path.GetDirectoryName(fullPathToDotnetFromRoot) : null;
                }

                return dotnetPath;
            }

            return null;
        }
    }
}
#endif
