// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AwesomeAssertions;
using Microsoft.DotNet.Build.Tasks.Workloads.Msi;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    public abstract class TestBase
    {
        public static readonly string BaseIntermediateOutputPath = Path.Combine(AppContext.BaseDirectory, "obj", Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
        public static readonly string BaseOutputPath = Path.Combine(AppContext.BaseDirectory, "bin", Path.GetFileNameWithoutExtension(Path.GetTempFileName()));

        public static readonly string MsiOutputPath = Path.Combine(BaseOutputPath, "msi");
        public static readonly string TestAssetsPath = Path.Combine(AppContext.BaseDirectory, "testassets");

        public static readonly string WixToolsetPath = Path.Combine(TestAssetsPath, "wix");

        public static readonly string TestOutputRoot = Path.Combine(AppContext.BaseDirectory, "TEST_OUTPUT");

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
            registryKeys.Should().Contain(r =>
                    r.Name == "Version" &&
                    r.Value == "[ProductVersion]");
            registryKeys.Should().Contain(r =>
                r.Name == "DisplayName" &&
                r.Value == "[ProductName]");
        }
    }
}
