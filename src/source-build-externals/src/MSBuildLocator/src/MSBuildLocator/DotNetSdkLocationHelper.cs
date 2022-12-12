// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Microsoft.Build.Locator
{
    internal static class DotNetSdkLocationHelper
    {
        private static readonly Regex DotNetBasePathRegex = new Regex("Base Path:(.*)$", RegexOptions.Multiline);
        private static readonly Regex VersionRegex = new Regex(@"^(\d+)\.(\d+)\.(\d+)", RegexOptions.Multiline);
        private static readonly Regex SdkRegex = new Regex(@"(\S+) \[(.*?)]$", RegexOptions.Multiline);

        public static VisualStudioInstance GetInstance(string dotNetSdkPath)
        {            
            if (string.IsNullOrWhiteSpace(dotNetSdkPath))
            {
                return null;
            }

            if (!File.Exists(Path.Combine(dotNetSdkPath, "Microsoft.Build.dll")))
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
                    yield return dotnetSdk;
            }
        }

        private static string realpath(string path)
        {
            IntPtr ptr = NativeMethods.realpath(path, IntPtr.Zero);
            string result = Marshal.PtrToStringAuto(ptr);
            NativeMethods.free(ptr);
            return result;
        }

        private static IEnumerable<string> GetDotNetBasePaths(string workingDirectory)
        {
            string dotnetPath = null;
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            string exeName = isWindows ? "dotnet.exe" : "dotnet";

            // We will generally find the dotnet exe on the path, but on linux, it is often just a 'dotnet' symlink (possibly even to more symlinks) that we have to resolve
            // to the real dotnet executable.
            // This will work as often as just invoking dotnet from the command line, but we can be more confident in finding a dotnet executable by following
            // https://github.com/dotnet/designs/blob/main/accepted/2021/install-location-per-architecture.md
            // This can be done using the nethost library. We didn't do this previously, so I did not implement this extension.
            foreach (string dir in Environment.GetEnvironmentVariable("PATH").Split(Path.PathSeparator))
            {
                string filePath = Path.Combine(dir, exeName);
                if (File.Exists(Path.Combine(dir, exeName)))
                {
                    dotnetPath = filePath;
                    break;
                }
            }

            if (dotnetPath != null)
            {
                dotnetPath = Path.GetDirectoryName(isWindows ? dotnetPath : realpath(dotnetPath) ?? dotnetPath);
            }

            string bestSDK = null;
            int rc = NativeMethods.hostfxr_resolve_sdk2(exe_dir: dotnetPath, working_dir: workingDirectory, flags: 0, result: (key, value) =>
            {
                if (key == NativeMethods.hostfxr_resolve_sdk2_result_key_t.resolved_sdk_dir)
                {
                    bestSDK = value;
                }
            });

            if (rc == 0 && bestSDK != null)
            {
                yield return bestSDK;
            }
            else if (rc != 0)
            {
                throw new InvalidOperationException("Failed to find an appropriate version of .NET Core MSBuild. Call to hostfxr_resolve_sdk2 failed. There may be more details in stderr.");
            }

            string[] paths = null;
            rc = NativeMethods.hostfxr_get_available_sdks(exe_dir: dotnetPath, result: (key, value) =>
            {
                paths = value;
            });

            // Errors are automatically printed to stderr. We should not continue to try to output anything if we failed.
            if (rc != 0)
            {
                throw new InvalidOperationException("Failed to find all versions of .NET Core MSBuild. Call to hostfxr_get_available_sdks failed. There may be more details in stderr.");
            }

            // The paths are sorted in increasing order. We want to return the newest SDKs first, however,
            // so iterate over the list in reverse order. If basePath is disqualified because it was later
            // than the runtime version, this ensures that RegisterDefaults will return the latest valid
            // SDK instead of the earliest installed.
            for (int i = paths.Length - 1; i >= 0; i--)
            {
                if (paths[i] != bestSDK)
                {
                    yield return paths[i];
                }
            }
        }
    }
}
#endif
