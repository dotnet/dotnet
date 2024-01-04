// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace Microsoft.DotNet.Arcade.Sdk
{
#if NET472
    [LoadInSeparateAppDomain]
    public class InstallDotNetCore : AppDomainIsolatedTask
    {
        static InstallDotNetCore() => AssemblyResolution.Initialize();
#else
    public class InstallDotNetCore : Microsoft.Build.Utilities.Task
    {
#endif
        public string VersionsPropsPath { get; set; }

        [Required]
        public string DotNetInstallScript { get; set; }
        [Required]
        public string GlobalJsonPath { get; set; }
        [Required]
        public string Platform { get; set; }

        public string RuntimeSourceFeed { get; set; }
        
        public string RuntimeSourceFeedKey { get; set; }

        public override bool Execute()
        {
            if (!File.Exists(GlobalJsonPath))
            {
                Log.LogWarning($"Unable to find global.json file '{GlobalJsonPath} exiting");
                return true;
            }
            if (!File.Exists(DotNetInstallScript))
            {
                Log.LogError($"Unable to find dotnet install script '{DotNetInstallScript} exiting");
                return !Log.HasLoggedErrors;
            }

            var jsonContent = File.ReadAllText(GlobalJsonPath);
            var bytes = Encoding.UTF8.GetBytes(jsonContent);

            using (JsonDocument jsonDocument = JsonDocument.Parse(bytes))
            {
                if (jsonDocument.RootElement.TryGetProperty("tools", out JsonElement toolsElement))
                {
                    if (toolsElement.TryGetProperty("runtimes", out JsonElement dotnetLocalElement))
                    {
                        var runtimeItems = new Dictionary<string, IEnumerable<KeyValuePair<string, string>>>();
                        foreach (var runtime in dotnetLocalElement.EnumerateObject())
                        {
                            var items = GetItemsFromJsonElementArray(runtime, out string runtimeName);
                            if (runtimeItems.ContainsKey(runtimeName))
                            {
                                runtimeItems[runtimeName] = runtimeItems[runtimeName].Concat(items);
                            }
                            else
                            {
                                runtimeItems.Add(runtimeName, items);
                            }
                        }
                        if (runtimeItems.Count > 0)
                        {
                            System.Linq.ILookup<string, ProjectProperty> properties = null;
                            // Only load Versions.props if there's a need to look for a version identifier (ie, there's a value listed that's not a parsable version).
                            if (runtimeItems.SelectMany(r => r.Value).Select(r => r.Key).FirstOrDefault(f => !SemanticVersion.TryParse(f, out SemanticVersion version)) != null)
                            {
                                if (!File.Exists(VersionsPropsPath))
                                {
                                    Log.LogError($"Unable to find translation file {VersionsPropsPath}");
                                    return !Log.HasLoggedErrors;
                                }
                                else
                                {
                                    var proj = Project.FromFile(VersionsPropsPath, new Build.Definition.ProjectOptions() { ProjectCollection = new ProjectCollection() });
                                    properties = proj.AllEvaluatedProperties.ToLookup(p => p.Name, StringComparer.OrdinalIgnoreCase);
                                }
                            }

                            foreach (var runtimeItem in runtimeItems)
                            {
                                foreach (var item in runtimeItem.Value)
                                {
                                    string architecture = GetArchitecture(item.Value);

                                    if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && string.Equals("x86", architecture, StringComparison.OrdinalIgnoreCase))
                                    {
                                        Log.LogMessage(MessageImportance.Low, "Skipping installing x86 runtimes because this is a non-Windows platform and .NET Core x86 is not currently supported on any non-Windows platform.");
                                        continue;
                                    }

                                    SemanticVersion version = null;
                                    // Try to parse version
                                    if (!SemanticVersion.TryParse(item.Key, out version))
                                    {
                                        var propertyName = item.Key.Trim('$', '(', ')');

                                        // Unable to parse version, try to find the corresponding identifier from the MSBuild loaded MSBuild properties
                                        ProjectProperty property = properties[propertyName].FirstOrDefault();
                                        if (property == null)
                                        {
                                            Log.LogError($"Unable to find '{item.Key}' in properties defined in '{VersionsPropsPath}'");
                                        }
                                        else if (!SemanticVersion.TryParse(property.EvaluatedValue, out version))
                                        {
                                            Log.LogError($"Unable to parse '{item.Key}' from properties defined in '{VersionsPropsPath}'");
                                        }
                                    }

                                    if (version != null)
                                    {
                                        string arguments = $"-runtime \"{runtimeItem.Key}\" -version \"{version.ToNormalizedString()}\"";
                                        if (!string.IsNullOrEmpty(architecture))
                                        {
                                            arguments += $" -architecture {architecture}";
                                        }

                                        if (!string.IsNullOrWhiteSpace(RuntimeSourceFeed))
                                        {
                                            arguments += $" -runtimeSourceFeed {RuntimeSourceFeed}";
                                        }

                                        // The default RuntimeSourceFeed doesn't need a key
                                        if (!string.IsNullOrWhiteSpace(RuntimeSourceFeed) && !string.IsNullOrWhiteSpace(RuntimeSourceFeedKey))
                                        {
                                            arguments += $" -runtimeSourceFeedKey {RuntimeSourceFeedKey}";
                                        }

                                        Log.LogMessage(MessageImportance.Low, $"Executing: {DotNetInstallScript} {arguments}");
                                        var process = Process.Start(new ProcessStartInfo()
                                        {
                                            FileName = DotNetInstallScript,
                                            Arguments = arguments,
                                            UseShellExecute = false,
                                            // Redirect to stdout/err. Addressing https://github.com/dotnet/msbuild/issues/7913
                                            // Without it script execution was failing on Linux when run from
                                            RedirectStandardOutput = true,
                                            RedirectStandardError = true,
                                        });
                                        process.OutputDataReceived += (sender, e) =>
                                        {
                                            if (!String.IsNullOrEmpty(e.Data))
                                            {
                                                Console.WriteLine(e.Data);
                                            }
                                        };
                                        process.ErrorDataReceived += (sender, e) =>
                                        {
                                            if (!String.IsNullOrEmpty(e.Data))
                                            {
                                                Console.Error.WriteLine(e.Data);
                                            }
                                        };
                                        process.BeginOutputReadLine();
                                        process.BeginErrorReadLine();
                                        process.WaitForExit();
                                        if (process.ExitCode != 0)
                                        {
                                            Log.LogError("dotnet-install failed");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return !Log.HasLoggedErrors;
        }

        private string GetArchitecture(string architecture)
        {
            if (!string.IsNullOrWhiteSpace(architecture))
            {
                return architecture;
            }
            else if (!string.IsNullOrWhiteSpace(Platform) && !string.Equals(Platform, "AnyCpu", StringComparison.OrdinalIgnoreCase))
            {
                return Platform;
            }
            else if (RuntimeInformation.OSArchitecture == Architecture.X86 ||
                     RuntimeInformation.OSArchitecture == Architecture.X64)
            {
                return "x64";
            }

            // let dotnet-install.sh/ps1 infer a default arch
            return null;
        }

        /*
         * Parses a json token of this format
         * { (runtime): [(version), ..., (version)] }
         * or this format
         * { (runtime/architecture): [(version), ..., (version)] }
         */
        private IEnumerable<KeyValuePair<string, string>> GetItemsFromJsonElementArray(JsonProperty token, out string runtime)
        {
            var items = new List<KeyValuePair<string, string>>();

            runtime = token.Name;
            string architecture = string.Empty;
            if (runtime.Contains('/'))
            {
                var parts = runtime.Split(new char[] { '/' }, 2);
                runtime = parts[0];
                architecture = parts[1];
            }
            foreach (var version in token.Value.EnumerateArray())
            {
                items.Add(new KeyValuePair<string, string>(version.GetString(), architecture));
            }
            return items.ToArray();
        }
    }
}
