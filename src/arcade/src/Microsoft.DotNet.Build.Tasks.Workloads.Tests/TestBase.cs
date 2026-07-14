// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AwesomeAssertions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Workloads.Msi;
using Microsoft.DotNet.Build.Tasks.Workloads.Wix;
using WixToolset.Dtf.WindowsInstaller;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    public abstract class TestBase
    {
        public static readonly string BaseIntermediateOutputPath = Path.Combine(AppContext.BaseDirectory, "obj", Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
        public static readonly string BaseOutputPath = Path.Combine(AppContext.BaseDirectory, "bin", Path.GetFileNameWithoutExtension(Path.GetTempFileName()));

        public static readonly string TestAssetsPath = Path.Combine(AppContext.BaseDirectory, "testassets");

        public static readonly string TestOutputRoot = Path.Combine(AppContext.BaseDirectory, "TEST_OUTPUT");

        /// <summary>
        /// Wix Toolset to use for tests.. 
        /// </summary>
        public static WixToolsetConfiguration WixToolsetConfig = WixToolsetConfiguration.Create(
            ToolsetInfo.WixExePath, ToolsetInfo.HeatExePath,
            ToolsetInfo.DependencyExt, ToolsetInfo.UtilExt, ToolsetInfo.UIExt);

        /// <summary>
        /// Item group containing WiX extensions. This is required by the public tasks and is similar
        /// to how users would pass information about the extensions.
        /// </summary>
        public static ITaskItem[] WixExtensions = [
            new TaskItem(ToolsetInfo.DependencyExt),
            new TaskItem(ToolsetInfo.UtilExt),
            new TaskItem(ToolsetInfo.UIExt)
        ];

        public static string MSBuildExePath;

        /// <summary>
        /// Returns a new, random directory for a test case.
        /// </summary>
        public string GetTestCaseDirectory() =>
            Path.Combine(TestOutputRoot, Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));

        /// <summary>
        /// Verifies that the Upgrade table contains a row matching the provided expected values.
        /// </summary>
        protected static void ValidatedRelatedProduct(IEnumerable<RelatedProduct> relatedProducts,
            string expectedUpgradeCode, string expectedVersionMin,
            string expectedVersionMax, int expectedAttributes, string expectedActionProperty)
        {
            relatedProducts.Should().Contain(r => string.Equals(r.UpgradeCode, expectedUpgradeCode, StringComparison.OrdinalIgnoreCase) &&
                r.VersionMin == expectedVersionMin &&
                r.VersionMax == expectedVersionMax &&
                r.ActionProperty == expectedActionProperty &&
                r.Attributes == expectedAttributes);
        }

        /// <summary>
        /// Verify that the CustomAction table contains the expected entries for setting DOTNETHOME when executing
        /// under emulation on arm64.
        /// </summary>
        /// <param name="customActions">List of custom actions to validate.</param>
        /// <param name="platform">The platform of the MSI to validate.</param>
        protected static void ValidateDotNetHomeCustomActions(IEnumerable<CustomActionRow> customActions, string platform)
        {
            if (platform == "x64")
            {
                customActions.Should().Contain(c => c.Action == "Set_NON_NATIVE_ARCHITECTURE" &&
                    c.Source == "NON_NATIVE_ARCHITECTURE");
                customActions.Should().Contain(c => c.Action == "Set_DOTNETHOME_NON_NATIVE_ARCHITECTURE" &&
                    c.Source == "DOTNETHOME");
            }
            else
            {
                customActions.Should().NotContain(c => c.Action == "Set_NON_NATIVE_ARCHITECTURE" &&
                    c.Source == "NON_NATIVE_ARCHITECTURE");
                customActions.Should().NotContain(c => c.Action == "Set_DOTNETHOME_NON_NATIVE_ARCHITECTURE" &&
                    c.Source == "DOTNETHOME");
            }
        }

        /// <summary>
        /// Verify that the registry keys for the workload installation record exists. The records
        /// are used by the .NET CLI to manage installs.
        /// </summary>
        /// <param name="registryKeys">The set of keys to verify.</param>
        /// <param name="installationRecordKey">The installation record key, for example, <b>SOFTWARE\Microsoft\dotnet\InstalledPacks\x64\Microsoft.NET.Runtime.Emscripten.2.0.23.Python.win-x64\6.0.4</b>.</param>
        /// <param name="expectedProviderKey">The dependency provider key of the MSI.</param>
        /// <param name="expectedProductCode">The ProductCode of the MSI.</param>
        /// <param name="expectedUpgradeCode">The UpgradeCode of the MSI.</param>
        /// <param name="expectedProductVersion">The ProductVersion of the MSI.</param>
        /// <param name="expectedProductLanguage">The ProductLanguage of the MSI.</param>
        protected static void ValidateInstallationRecord(IEnumerable<RegistryRow> registryKeys,
            string installationRecordKey, string expectedProviderKey, string expectedProductCode, string expectedUpgradeCode,
            string expectedProductVersion,
            string expectedProductLanguage = "#1033")
        {
            // Filter out the installation record keys. They should all be under HKLM (Root == 2).
            var keys = registryKeys.Where(r => r.Key == installationRecordKey && r.Root == 2);

            keys.Should().Contain(r =>
                r.Name == "DependencyProviderKey" &&
                r.Value == expectedProviderKey);
            keys.Should().Contain(r =>
                r.Name == "ProductCode" &&
                string.Equals(r.Value, expectedProductCode, StringComparison.OrdinalIgnoreCase));
            keys.Should().Contain(r =>
                r.Name == "UpgradeCode" &&
                string.Equals(r.Value, expectedUpgradeCode, StringComparison.OrdinalIgnoreCase));
            keys.Should().Contain(r =>
                r.Name == "ProductVersion" &&
                r.Value == expectedProductVersion);
            keys.Should().Contain(r =>
                r.Name == "ProductLanguage" &&
                r.Value == expectedProductLanguage);
        }

        /// <summary>
        /// Validates that the specified registry keys collection contains an entry for each provided pack key with the
        /// expected properties.
        /// </summary>
        /// <param name="registryKeys">The collection of registry key records to validate. Each record is checked for the presence of the specified
        /// pack keys.</param>
        /// <param name="packKeys">The names of the pack keys to verify within the registry keys collection. Each key must correspond to a
        /// registry entry with a root value of 2 and an empty value.</param>
        protected static void ValidatePackGroupInstallRecordKeys(IEnumerable<RegistryRow> registryKeys,
            params string[] packKeys)
        {
            foreach (var key in packKeys)
            {
                registryKeys.Should().Contain(r => r.Key == key &&
                    r.Root == 2 &&
                    r.Name == "" &&
                    r.Value == "");
            }
        }

        /// <summary>
        /// Verify that the registry table contains entries for the dependency provider.
        /// </summary>
        /// <param name="registryKeys">Rows from the Registry table to validate.</param>
        /// <param name="dependencyProviderKey">The dependency provider key.</param>
        protected static void ValidateDependencyProviderKey(IEnumerable<RegistryRow> registryKeys, string dependencyProviderKey)
        {
            // Filter out the provider keys. The Root is expected to be -1 because the dependency
            // provider extension can be used to author per-machine or per-user packages. If ALLUSERS is
            // set to 1, the key will be written to HKLM, otherwise it's written to HKCU.
            var keys = registryKeys.Where(r => r.Key == dependencyProviderKey && r.Root == -1);

            // Dependency provider entries reference the ProductVersion and ProductName properties. These
            // properties are set at install time.
            keys.Should().Contain(r =>
                    r.Name == "Version" &&
                    r.Value == "[ProductVersion]");
            keys.Should().Contain(r =>
                r.Name == "DisplayName" &&
                r.Value == "[ProductName]");
        }

        /// <summary>
        /// Verify that the summary information stream matches the expected values for the given platform.
        /// </summary>
        /// <param name="path">The path to the MSI.</param>
        /// <param name="platform">The target platform from the test.</param>
        protected static void ValidateSummaryInformation(string path, string platform)
        {
            using SummaryInfo si = new(path, enableWrite: false);

            switch (platform)
            {
                case "x86":
                    Assert.Equal("Intel;1033", si.Template);
                    break;
                case "x64":
                    Assert.Equal($"x64;1033", si.Template);
                    break;
                case "arm64":
                    Assert.Equal($"Arm64;1033", si.Template);
                    break;
                default: throw new Exception($"Invalid platform: {platform}");
            }
        }

        /// <summary>
        /// Compiles a SWIX project.
        /// </summary>
        /// <param name="projectPath">The path of the SWIX project to build.</param>
        /// <param name="manifestOutputPath">The file system path of the directory where the SWIX manifest will be generated</param>
        /// <exception cref="Exception"></exception>
        protected static void BuildSwixProject(string projectPath, string manifestOutputPath)
        {
            string[] args = [$@"""{projectPath}""",
                $@"/p:ManifestOutputPath={manifestOutputPath}",
                $@"/p:SwixBuildTargets={ToolsetInfo.SwixTargetsPath}"
            ];

            var msbuildStartInfo = new ProcessStartInfo(MSBuildExePath, string.Join(" ", args))
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            var proc = Process.Start(msbuildStartInfo)!;
            proc.WaitForExit();
        }

        static TestBase()
        {
            var vswhere = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                @"Microsoft Visual Studio\Installer\vswhere.exe");

            if (!File.Exists(vswhere))
            {
                vswhere = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                @"Microsoft Visual Studio\Installer\vswhere.exe");
            }

            if (!File.Exists(vswhere))
            {
                Console.WriteLine("Skipping because vswhere.exe is unavailable on this machine.");
                return;
            }

            var vsPath = Process.Start(new ProcessStartInfo(vswhere,
                "-latest -requires Microsoft.Component.MSBuild -property installationPath")
            { RedirectStandardOutput = true })!.StandardOutput.ReadToEnd().Trim();

            MSBuildExePath = Path.Combine(vsPath, @"MSBuild\Current\Bin\MSBuild.exe");
        }
    }
}
