// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETCOREAPP

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        private static IEnumerable<string> GetDotNetBasePaths(string workingDirectory)
        {
            const string DOTNET_CLI_UI_LANGUAGE = nameof(DOTNET_CLI_UI_LANGUAGE);
            
            Process process;
            var lines = new List<string>();
            try
            {
                process = new Process()
                { 
                    StartInfo = new ProcessStartInfo("dotnet", "--info")
                    {
                        WorkingDirectory = workingDirectory,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };

                // Ensure that we set the DOTNET_CLI_UI_LANGUAGE environment variable to "en-US" before
                // running 'dotnet --info'. Otherwise, we may get localized results.
                process.StartInfo.EnvironmentVariables[DOTNET_CLI_UI_LANGUAGE] = "en-US";

                process.OutputDataReceived += (_, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {
                        lines.Add(e.Data);
                    }
                };

                process.Start();
            }
            catch
            {
                // when error running dotnet command, consider dotnet as not available
                yield break;
            }

            process.BeginOutputReadLine();

            process.WaitForExit();

            var outputString = string.Join(Environment.NewLine, lines);

            var matched = DotNetBasePathRegex.Match(outputString);
            string basePath = null;
            if (matched.Success)
            {
                basePath = matched.Groups[1].Value.Trim();
                yield return basePath;
            }

            var lineSdkIndex = lines.FindIndex(line => line.Contains("SDKs installed"));

            List<string> paths = new List<string>();
            if (lineSdkIndex != -1)
            {
                lineSdkIndex++;

                while (lineSdkIndex < lines.Count && !string.IsNullOrEmpty(lines[lineSdkIndex]))
                {
                    var sdkMatch = SdkRegex.Match(lines[lineSdkIndex]);

                    if (!sdkMatch.Success)
                        break;

                    var version = sdkMatch.Groups[1].Value.Trim();
                    var path = sdkMatch.Groups[2].Value.Trim();
                    
                    path = Path.Combine(path, version) + Path.DirectorySeparatorChar;

                    if (!path.Equals(basePath))
                        paths.Add(path); 
                                    
                    lineSdkIndex++;
                }
            }

            // The paths are sorted in increasing order. We want to return the newest SDKs first, however,
            // so iterate over the list in reverse order. If basePath is disqualified because it was later
            // than the runtime version, this ensures that RegisterDefaults will return the latest valid
            // SDK instead of the earliest installed.
            for (int i = paths.Count - 1; i >= 0; i--)
            {
                yield return paths[i];
            }
        }
    }
}
#endif
