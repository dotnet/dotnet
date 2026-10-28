// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class PackagedConsoleTests : IntegrationTestBase
{
    [TestMethod]
    public void PackagedObjectModelShouldDeclareRegistryDependencyForNetCore()
    {
        var packagePath = Path.Combine(
            IntegrationTestEnvironment.LocalPackageSource,
            $"Microsoft.TestPlatform.ObjectModel.{IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}.nupkg");
        using var package = ZipFile.OpenRead(packagePath);
        var manifestEntry = package.Entries.Single(entry => entry.FullName.EndsWith(".nuspec", StringComparison.Ordinal));
        using var manifestStream = manifestEntry.Open();
        var manifest = XDocument.Load(manifestStream);
        var ns = manifest.Root!.Name.Namespace;
        var dependencyGroups = manifest.Descendants(ns + "group").ToArray();
        var coreGroup = dependencyGroups.Single(group => (string?)group.Attribute("targetFramework") == ".NETCoreApp3.1");
        var registry = coreGroup.Elements(ns + "dependency").Single(dependency => (string?)dependency.Attribute("id") == "Microsoft.Win32.Registry");

        Assert.AreEqual("5.0.0", (string?)registry.Attribute("version"));
        Assert.IsFalse(dependencyGroups.Except(new[] { coreGroup })
            .SelectMany(group => group.Elements(ns + "dependency"))
            .Any(dependency => (string?)dependency.Attribute("id") == "Microsoft.Win32.Registry"));
    }

    [DataTestMethod]
    [DataRow("Microsoft.TestPlatform.CLI", @"contentFiles\any\netcoreapp3.1")]
    [DataRow("Microsoft.TestPlatform.Portable", @"tools\netcoreapp3.1")]
    public void PackagedCoreConsoleShouldIncludeRuntimeAssetsAndStart(string packageId, string relativePath)
    {
        var directory = Path.Combine(
            IntegrationTestEnvironment.PublishDirectory,
            $"{packageId}.{IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}.nupkg",
            relativePath.Replace('\\', Path.DirectorySeparatorChar));
        var runtimeAssets = new[]
        {
            @"runtimes\win\lib\netstandard2.0\Microsoft.Win32.Registry.dll",
            @"runtimes\win\lib\netcoreapp2.0\System.Security.AccessControl.dll",
            @"runtimes\win\lib\netcoreapp2.1\System.Security.Principal.Windows.dll",
            @"runtimes\unix\lib\netcoreapp2.1\System.Security.Principal.Windows.dll",
        };
        foreach (var asset in runtimeAssets)
        {
            Assert.IsTrue(
                File.Exists(Path.Combine(directory, asset.Replace('\\', Path.DirectorySeparatorChar))),
                $"{packageId} is missing runtime asset '{asset}'.");
        }

        var console = Path.Combine(directory, "vstest.console.dll");
        ExecuteApplication(
            GetDownloadedDotnetMuxerFromTools("X64"),
            $"exec \"{console}\" --help",
            out string stdOut,
            out string stdError,
            out int exitCode);

        Assert.AreEqual(0, exitCode, $"{packageId} did not start. STDOUT: {stdOut} STDERR: {stdError}");
        Assert.IsFalse(string.IsNullOrWhiteSpace(stdOut), $"{packageId} produced no help output.");
    }
}
