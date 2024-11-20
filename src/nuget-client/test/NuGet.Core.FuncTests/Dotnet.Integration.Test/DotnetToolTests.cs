// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;
using Xunit.Abstractions;

namespace Dotnet.Integration.Test
{
    [Collection(DotnetIntegrationCollection.Name)]
    public class DotnetToolTests
    {
        private DotnetIntegrationTestFixture _dotnetFixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public DotnetToolTests(DotnetIntegrationTestFixture dotnetFixture, ITestOutputHelper testOutputHelper)
        {
            _dotnetFixture = dotnetFixture;
            _testOutputHelper = testOutputHelper;
        }

        [PlatformFact(Platform.Windows)]
        public void DotnetToolTests_NoPackageReferenceToolRestore_ThrowsError()
        {
            using (TestDirectory testDirectory = _dotnetFixture.CreateTestDirectory())
            {
                var tfm = "netcoreapp2.0";
                var projectName = "ToolRestoreProject";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var rid = "win-x86";

                _dotnetFixture.CreateDotnetToolProject(testDirectory.Path, projectName, tfm, rid);

                // Act
                var result = _dotnetFixture.RestoreToolProjectExpectFailure(workingDirectory, projectName, testOutputHelper: _testOutputHelper);

                // Assert
                Assert.True(result.ExitCode == 1, result.AllOutput);
                Assert.Contains("NU1211", result.Output);
            }
        }

        [PlatformFact(Platform.Windows)]
        public void DotnetToolTests_RegularDependencyPackageWithDependenciesToolRestore_ThrowsError()
        {
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                string testDirectory = pathContext.WorkingDirectory;
                var tfm = "netcoreapp2.0";
                var projectName = "ToolRestoreProject";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var packageSource = "https://api.nuget.org/v3/index.json";
                var rid = "win-x86";
                var packages = new List<PackageIdentity>() { new PackageIdentity("Newtonsoft.Json", NuGetVersion.Parse("10.0.3")) };

                _dotnetFixture.CreateDotnetToolProject(testDirectory, projectName, tfm, rid, packageSource, packages);

                // Act
                var result = _dotnetFixture.RestoreToolProjectExpectFailure(workingDirectory, projectName, testOutputHelper: _testOutputHelper);

                // Assert
                Assert.True(result.ExitCode == 1, result.AllOutput);
                Assert.Contains("NU1212", result.Output);
                Assert.DoesNotContain("NU1211", result.Output); // It's the correct dependency count!
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetToolTests_BasicDotnetToolRestore_SucceedsAsync()
        {
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                string testDirectory = pathContext.WorkingDirectory;
                var tfm = "netcoreapp2.0";
                var projectName = "ToolRestoreProject";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var packageSource = Path.Combine(testDirectory, "packageSource");
                var rid = "win-x64";
                var packageName = string.Join("ToolPackage-", tfm, rid);
                var packageVersion = NuGetVersion.Parse("1.0.0");
                var packages = new List<PackageIdentity>() { new PackageIdentity(packageName, packageVersion) };

                var package = new SimpleTestPackageContext(packageName, packageVersion.OriginalVersion);
                package.Files.Clear();
                package.AddFile($"tools/{tfm}/{rid}/a.dll");
                package.PackageType = PackageType.DotnetTool;
                package.UseDefaultRuntimeAssemblies = false;
                package.PackageTypes.Add(PackageType.DotnetTool);
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, package);

                _dotnetFixture.CreateDotnetToolProject(testDirectory, projectName, tfm, rid, packageSource, packages);

                // Act
                var result = _dotnetFixture.RestoreToolProjectExpectSuccess(workingDirectory, projectName, testOutputHelper: _testOutputHelper);

                // Assert
                Assert.True(2 == result.AllOutput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Length, result.AllOutput);
                // Verify the assets file
                var lockFile = LockFileUtilities.GetLockFile(Path.Combine(testDirectory, projectName, "project.assets.json"), NullLogger.Instance);
                Assert.NotNull(lockFile);
                Assert.Equal(2, lockFile.Targets.Count);
                var ridTargets = lockFile.Targets.Where(e => e.RuntimeIdentifier != null ? e.RuntimeIdentifier.Equals(rid, StringComparison.CurrentCultureIgnoreCase) : false);
                Assert.Equal(1, ridTargets.Count());
                var toolsAssemblies = ridTargets.First().Libraries.First().ToolsAssemblies;
                Assert.Equal(1, toolsAssemblies.Count);
                Assert.Equal($"tools/{tfm}/{rid}/a.dll", toolsAssemblies.First().Path);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetToolTests_MismatchedRID_FailsAsync()
        {
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                string testDirectory = pathContext.WorkingDirectory;
                var tfm = "netcoreapp2.0";
                var projectName = "ToolRestoreProject";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var packageSource = Path.Combine(testDirectory, "packageSource");
                var projectRID = "win-x64";
                var packageRID = "win-x86";

                var packageName = string.Join("ToolPackage-", tfm, packageRID);
                var packageVersion = NuGetVersion.Parse("1.0.0");
                var packages = new List<PackageIdentity>() { new PackageIdentity(packageName, packageVersion) };
                var package = new SimpleTestPackageContext(packageName, packageVersion.OriginalVersion);
                package.Files.Clear();
                package.AddFile($"tools/{tfm}/{packageRID}/a.dll");
                package.PackageType = PackageType.DotnetTool;
                package.UseDefaultRuntimeAssemblies = false;
                package.PackageTypes.Add(PackageType.DotnetTool);
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, package);

                _dotnetFixture.CreateDotnetToolProject(testDirectory, projectName, tfm, projectRID, packageSource, packages);

                // Act
                var result = _dotnetFixture.RestoreToolProjectExpectFailure(workingDirectory, projectName, testOutputHelper: _testOutputHelper);

                // Assert
                Assert.True(result.ExitCode == 1, result.AllOutput);
                Assert.Contains("NU1202", result.Output);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetToolTests_BasicDotnetToolRestore_WithJsonCompatibleAssets_SucceedsAsync()
        {
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                string testDirectory = pathContext.WorkingDirectory;
                var tfm = "netcoreapp2.0";
                var projectName = "ToolRestoreProject";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var packageSource = Path.Combine(testDirectory, "packageSource");
                var rid = "win-x64";
                var packageName = string.Join("ToolPackage-", tfm, rid);
                var packageVersion = NuGetVersion.Parse("1.0.0");
                var packages = new List<PackageIdentity>() { new PackageIdentity(packageName, packageVersion) };

                var package = new SimpleTestPackageContext(packageName, packageVersion.OriginalVersion);
                package.Files.Clear();
                package.AddFile($"tools/{tfm}/{rid}/a.dll");
                package.AddFile($"tools/{tfm}/{rid}/Settings.json");

                package.PackageType = PackageType.DotnetTool;
                package.UseDefaultRuntimeAssemblies = false;
                package.PackageTypes.Add(PackageType.DotnetTool);
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, package);

                _dotnetFixture.CreateDotnetToolProject(testDirectory, projectName, tfm, rid, packageSource, packages);

                // Act
                var result = _dotnetFixture.RestoreToolProjectExpectSuccess(workingDirectory, projectName, testOutputHelper: _testOutputHelper);

                // Assert
                // Verify the assets file
                var lockFile = LockFileUtilities.GetLockFile(Path.Combine(testDirectory, projectName, "project.assets.json"), NullLogger.Instance);
                Assert.NotNull(lockFile);
                Assert.Equal(2, lockFile.Targets.Count);
                var ridTargets = lockFile.Targets.Where(e => e.RuntimeIdentifier != null ? e.RuntimeIdentifier.Equals(rid, StringComparison.CurrentCultureIgnoreCase) : false);
                Assert.Equal(1, ridTargets.Count());
                var toolsAssemblies = ridTargets.First().Libraries.First().ToolsAssemblies;
                Assert.Equal(2, toolsAssemblies.Count);
                Assert.True(toolsAssemblies.Contains($"tools/{tfm}/{rid}/a.dll"));
                Assert.True(toolsAssemblies.Contains($"tools/{tfm}/{rid}/Settings.json"));
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("netcoreapp2.0", "any", "win-x64")]
        [InlineData("netcoreapp2.0", "any", "win-x86")]
        public async Task DotnetToolTests_PackageWithRuntimeJson_RuntimeIdentifierAny_SucceedsAsync(string tfm, string packageRID, string projectRID)
        {
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                string testDirectory = pathContext.WorkingDirectory;
                var projectName = "ToolRestoreProject";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var packageSource = Path.Combine(testDirectory, "packageSource");
                var packageName = string.Join("ToolPackage-", tfm, packageRID);
                var packageVersion = NuGetVersion.Parse("1.0.0");
                var packages = new List<PackageIdentity>() { new PackageIdentity(packageName, packageVersion) };

                var package = new SimpleTestPackageContext(packageName, packageVersion.OriginalVersion);
                package.Files.Clear();
                package.AddFile($"tools/{tfm}/{packageRID}/a.dll");
                package.AddFile($"tools/{tfm}/{packageRID}/Settings.json");
                package.RuntimeJson = GetResource("Dotnet.Integration.Test.compiler.resources.runtime.json", GetType());
                package.PackageType = PackageType.DotnetTool;
                package.UseDefaultRuntimeAssemblies = false;
                package.PackageTypes.Add(PackageType.DotnetTool);
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, package);

                _dotnetFixture.CreateDotnetToolProject(testDirectory, projectName, tfm, projectRID, packageSource, packages);

                // Act
                var result = _dotnetFixture.RestoreToolProjectExpectSuccess(workingDirectory, projectName, testOutputHelper: _testOutputHelper);

                // Assert
                // Verify the assets file
                var lockFile = LockFileUtilities.GetLockFile(Path.Combine(testDirectory, projectName, "project.assets.json"), NullLogger.Instance);
                Assert.NotNull(lockFile);
                Assert.Equal(2, lockFile.Targets.Count);
                var ridTargets = lockFile.Targets.Where(e => e.RuntimeIdentifier != null ? e.RuntimeIdentifier.Equals(projectRID, StringComparison.CurrentCultureIgnoreCase) : false);
                Assert.Equal(1, ridTargets.Count());
                var toolsAssemblies = ridTargets.First().Libraries.First().ToolsAssemblies;
                Assert.Equal(2, toolsAssemblies.Count);
                Assert.True(toolsAssemblies.Contains($"tools/{tfm}/{packageRID}/a.dll"));
                Assert.True(toolsAssemblies.Contains($"tools/{tfm}/{packageRID}/Settings.json"));
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetToolTests_RegularDependencyAndToolPackageWithDependenciesToolRestore_ThrowsErrorAsync()
        {
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                string testDirectory = pathContext.WorkingDirectory;
                var tfm = "netcoreapp2.0";
                var projectName = "ToolRestoreProject";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var packageSource = Path.Combine(testDirectory, "packageSource");
                var packageSources = "https://api.nuget.org/v3/index.json" + ";" + packageSource;
                var rid = "win-x64";
                var packageName = string.Join("ToolPackage-", tfm, rid);
                var packageVersion = NuGetVersion.Parse("1.0.0");

                var package = new SimpleTestPackageContext(packageName, packageVersion.OriginalVersion);
                package.Files.Clear();
                package.AddFile($"tools/{tfm}/{rid}/a.dll");
                package.AddFile($"tools/{tfm}/{rid}/Settings.json");

                package.PackageType = PackageType.DotnetTool;
                package.UseDefaultRuntimeAssemblies = false;
                package.PackageTypes.Add(PackageType.DotnetTool);
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, package);

                var packages = new List<PackageIdentity>()
                {
                    new PackageIdentity(packageName, packageVersion),
                    new PackageIdentity("Newtonsoft.Json", NuGetVersion.Parse("10.0.3"))
                };

                _dotnetFixture.CreateDotnetToolProject(testDirectory, projectName, tfm, rid, packageSources, packages);

                // Act
                var result = _dotnetFixture.RestoreToolProjectExpectFailure(workingDirectory, projectName, testOutputHelper: _testOutputHelper);

                // Assert
                Assert.True(result.ExitCode == 1, result.AllOutput);
                Assert.Contains("Invalid project-package combination for Newtonsoft.Json 10.0.3", result.Output);
                Assert.DoesNotContain("Invalid project-package combination for ToolPackage", result.Output);
                Assert.Contains("NU1211", result.Output); //count is wrong
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("netcoreapp1.0", "any", "win7-x64")]
        public async Task DotnetToolTests_ToolWithPlatformPackage_SucceedsAsync(string tfm, string packageRid, string projectRid)
        {
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                string testDirectory = pathContext.WorkingDirectory;
                var projectName = "ToolRestoreProject";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var packageSource = Path.Combine(testDirectory, "packageSource");
                var packageName = string.Join("ToolPackage-", tfm, packageRid);
                var platformsPackageName = "PlatformsPackage";
                var packageVersion = NuGetVersion.Parse("1.0.0");

                var platformsPackage = new SimpleTestPackageContext(platformsPackageName, packageVersion.OriginalVersion);
                platformsPackage.Files.Clear();
                platformsPackage.AddFile($"lib/{tfm}/a.dll");
                platformsPackage.PackageType = PackageType.Dependency;
                platformsPackage.UseDefaultRuntimeAssemblies = true;
                platformsPackage.PackageTypes.Add(PackageType.Dependency);
                platformsPackage.RuntimeJson = GetResource("Dotnet.Integration.Test.compiler.resources.runtime.json", GetType());

                var toolPackage = new SimpleTestPackageContext(packageName, packageVersion.OriginalVersion);
                toolPackage.Files.Clear();
                toolPackage.AddFile($"tools/{tfm}/{packageRid}/a.dll");
                toolPackage.AddFile($"tools/{tfm}/{packageRid}/Settings.json");
                toolPackage.PackageType = PackageType.DotnetTool;
                toolPackage.UseDefaultRuntimeAssemblies = false;
                toolPackage.PackageTypes.Add(PackageType.DotnetTool);
                toolPackage.Dependencies.Add(platformsPackage);

                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, toolPackage, platformsPackage);

                var packages = new List<PackageIdentity>()
                {
                    new PackageIdentity(packageName, packageVersion),
                };

                _dotnetFixture.CreateDotnetToolProject(testDirectory, projectName, tfm, projectRid, packageSource, packages);

                // Act
                var result = _dotnetFixture.RestoreToolProjectExpectSuccess(workingDirectory, projectName, testOutputHelper: _testOutputHelper);

                // Assert
                var lockFile = LockFileUtilities.GetLockFile(Path.Combine(testDirectory, projectName, "project.assets.json"), NullLogger.Instance);
                Assert.NotNull(lockFile);
                Assert.Equal(2, lockFile.Targets.Count);
                var ridTargets = lockFile.Targets.Where(e => e.RuntimeIdentifier != null ? e.RuntimeIdentifier.Equals(projectRid, StringComparison.CurrentCultureIgnoreCase) : false);
                Assert.Equal(1, ridTargets.Count());
                var toolsAssemblies = ridTargets.First().Libraries.First().ToolsAssemblies;
                Assert.Equal(2, toolsAssemblies.Count);
                Assert.Contains($"tools/{tfm}/{packageRid}", toolsAssemblies.First().Path);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetToolTests_ToolPackageWithIncompatibleToolsAssets_FailsAsync()
        {
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                string testDirectory = pathContext.WorkingDirectory;
                var tfm = "netcoreapp2.0";
                var projectName = "ToolRestoreProject";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var packageSource = Path.Combine(testDirectory, "packageSource");
                var rid = "win-x64";
                var packageName = string.Join("ToolPackage-", tfm, rid);
                var packageVersion = NuGetVersion.Parse("1.0.0");
                var packages = new List<PackageIdentity>() { new PackageIdentity(packageName, packageVersion) };

                var package = new SimpleTestPackageContext(packageName, packageVersion.OriginalVersion);
                package.Files.Clear();
                package.AddFile($"tools/{tfm}/a.dll");
                package.AddFile($"tools/runtimes/{rid}/ar.dll");
                package.AddFile($"lib/{tfm}/b.dll");
                package.AddFile($"lib/{tfm}/c.dll");
                package.AddFile($"lib/{tfm}/d.dll");
                package.PackageType = PackageType.DotnetTool;
                package.UseDefaultRuntimeAssemblies = false;
                package.PackageTypes.Add(PackageType.DotnetTool);
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, package);

                _dotnetFixture.CreateDotnetToolProject(testDirectory, projectName, tfm, rid, packageSource, packages);

                // Act
                var result = _dotnetFixture.RestoreToolProjectExpectFailure(workingDirectory, projectName, testOutputHelper: _testOutputHelper);

                // Assert
                Assert.True(result.ExitCode == 1, result.AllOutput);
                Assert.Contains("NU1202", result.AllOutput);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetToolTests_ToolsPackageWithExtraPackageTypes_FailsAsync()
        {
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                string testDirectory = pathContext.WorkingDirectory;
                var tfm = "netcoreapp2.0";
                var projectName = "ToolRestoreProject";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var packageSource = Path.Combine(testDirectory, "packageSource");
                var rid = "win-x64";
                var packageName = string.Join("ToolPackage-", tfm, rid);
                var packageVersion = NuGetVersion.Parse("1.0.0");
                var packages = new List<PackageIdentity>() { new PackageIdentity(packageName, packageVersion) };

                var package = new SimpleTestPackageContext(packageName, packageVersion.OriginalVersion);
                package.Files.Clear();
                package.AddFile($"tools/{tfm}/{rid}/a.dll");
                package.UseDefaultRuntimeAssemblies = false;
                package.PackageTypes.Add(PackageType.DotnetTool);
                package.PackageTypes.Add(PackageType.Dependency);
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, package);

                _dotnetFixture.CreateDotnetToolProject(testDirectory, projectName, tfm, rid, packageSource, packages);

                // Act
                var result = _dotnetFixture.RestoreToolProjectExpectFailure(workingDirectory, projectName, testOutputHelper: _testOutputHelper);

                // Assert
                Assert.True(result.ExitCode == 1, result.AllOutput);
                Assert.Contains("NU1204", result.AllOutput);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetToolTests_BasicDotnetToolRestoreWithNestedValues_SucceedsAsync()
        {
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                string testDirectory = pathContext.WorkingDirectory;
                var tfm = "netcoreapp2.0";
                var projectName = "ToolRestoreProject";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var packageSource = Path.Combine(testDirectory, "packageSource");
                var rid = "win-x64";
                var packageName = string.Join("ToolPackage-", tfm, rid);
                var packageVersion = NuGetVersion.Parse("1.0.0");
                var packages = new List<PackageIdentity>() { new PackageIdentity(packageName, packageVersion) };

                var package = new SimpleTestPackageContext(packageName, packageVersion.OriginalVersion);
                package.Files.Clear();
                package.AddFile($"tools/{tfm}/{rid}/a.dll");
                package.AddFile($"tools/{tfm}/{rid}/tool1/b.dll");
                package.PackageType = PackageType.DotnetTool;
                package.UseDefaultRuntimeAssemblies = false;
                package.PackageTypes.Add(PackageType.DotnetTool);
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, package);

                _dotnetFixture.CreateDotnetToolProject(testDirectory, projectName, tfm, rid, packageSource, packages);

                // Act
                var result = _dotnetFixture.RestoreToolProjectExpectSuccess(workingDirectory, projectName, testOutputHelper: _testOutputHelper);

                // Assert
                // Verify the assets file
                var lockFile = LockFileUtilities.GetLockFile(Path.Combine(testDirectory, projectName, "project.assets.json"), NullLogger.Instance);
                Assert.NotNull(lockFile);
                Assert.Equal(2, lockFile.Targets.Count);
                var ridTargets = lockFile.Targets.Where(e => e.RuntimeIdentifier != null ? e.RuntimeIdentifier.Equals(rid, StringComparison.CurrentCultureIgnoreCase) : false);
                Assert.Equal(1, ridTargets.Count());
                var toolsAssemblies = ridTargets.First().Libraries.First().ToolsAssemblies;
                Assert.Equal(2, toolsAssemblies.Count);
                var toolsAssemblyPaths = toolsAssemblies.Select(e => e.Path);
                Assert.Contains($"tools/{tfm}/{rid}/a.dll", toolsAssemblyPaths);
                Assert.Contains($"tools/{tfm}/{rid}/tool1/b.dll", toolsAssemblyPaths);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetToolTests_AutoreferencedDependencyAndToolPackagToolRestore_SucceedsAsync()
        {
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                string testDirectory = pathContext.WorkingDirectory;
                var tfm = "netcoreapp2.0";
                var projectName = "ToolRestoreProject";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var packageSource = Path.Combine(testDirectory, "packageSource");
                var packageSources = "https://api.nuget.org/v3/index.json" + ";" + packageSource;
                var rid = "win-x64";
                var packageName = string.Join("ToolPackage-", tfm, rid);
                var autoReferencePackageName = "Microsoft.NETCore.Platforms";
                var packageVersion = NuGetVersion.Parse("1.0.0");

                var package = new SimpleTestPackageContext(packageName, packageVersion.OriginalVersion);
                package.Files.Clear();
                package.AddFile($"tools/{tfm}/{rid}/a.dll");
                package.AddFile($"tools/{tfm}/{rid}/Settings.json");

                package.PackageType = PackageType.DotnetTool;
                package.UseDefaultRuntimeAssemblies = false;
                package.PackageTypes.Add(PackageType.DotnetTool);
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, package);

                var packages = new List<PackageIdentity>()
                {
                    new PackageIdentity(packageName, packageVersion),
                    new PackageIdentity(autoReferencePackageName, NuGetVersion.Parse("2.0.1"))
                };

                _dotnetFixture.CreateDotnetToolProject(testDirectory, projectName, tfm, rid, packageSources, packages);

                var fullProjectPath = Path.Combine(workingDirectory, $"{projectName}.csproj");
                MakePackageReferenceImplicitlyDefined(fullProjectPath, autoReferencePackageName);

                // Act
                var result = _dotnetFixture.RestoreToolProjectExpectSuccess(workingDirectory, projectName, testOutputHelper: _testOutputHelper);

                // Assert
                var lockFile = LockFileUtilities.GetLockFile(Path.Combine(testDirectory, projectName, "project.assets.json"), NullLogger.Instance);
                Assert.NotNull(lockFile);
                Assert.Equal(2, lockFile.Targets.Count);
                var ridTargets = lockFile.Targets.Where(e => e.RuntimeIdentifier != null ? e.RuntimeIdentifier.Equals(rid, StringComparison.CurrentCultureIgnoreCase) : false);
                Assert.Equal(1, ridTargets.Count());
                Assert.Equal(2, ridTargets.First().Libraries.Count);
                Assert.DoesNotContain("NU12", result.AllOutput); // Output has no errors
                Assert.DoesNotContain("NU11", result.AllOutput);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetToolTests_AutoreferencedDependencyRegularDependencyAndToolPackagToolRestore_ThrowsAsync()
        {
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                string testDirectory = pathContext.WorkingDirectory;
                var tfm = "netcoreapp2.0";
                var projectName = "ToolRestoreProject";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var packageSource = Path.Combine(testDirectory, "packageSource");
                var packageSources = "https://api.nuget.org/v3/index.json" + ";" + packageSource;
                var rid = "win-x64";
                var packageName = string.Join("ToolPackage-", tfm, rid);
                var autoReferencePackageName = "Microsoft.NETCore.Platforms";
                var packageVersion = NuGetVersion.Parse("1.0.0");

                var package = new SimpleTestPackageContext(packageName, packageVersion.OriginalVersion);
                package.Files.Clear();
                package.AddFile($"tools/{tfm}/{rid}/a.dll");
                package.AddFile($"tools/{tfm}/{rid}/Settings.json");

                package.PackageType = PackageType.DotnetTool;
                package.UseDefaultRuntimeAssemblies = false;
                package.PackageTypes.Add(PackageType.DotnetTool);
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, package);

                var packages = new List<PackageIdentity>()
                {
                    new PackageIdentity(packageName, packageVersion),
                    new PackageIdentity(autoReferencePackageName, NuGetVersion.Parse("2.0.1")),
                    new PackageIdentity("NuGet.Versioning", NuGetVersion.Parse("4.3.0"))
                };

                _dotnetFixture.CreateDotnetToolProject(testDirectory, projectName, tfm, rid, packageSources, packages);

                var fullProjectPath = Path.Combine(workingDirectory, $"{projectName}.csproj");
                MakePackageReferenceImplicitlyDefined(fullProjectPath, autoReferencePackageName);

                // Act
                var result = _dotnetFixture.RestoreToolProjectExpectFailure(workingDirectory, projectName, testOutputHelper: _testOutputHelper);

                // Assert
                var lockFile = LockFileUtilities.GetLockFile(Path.Combine(testDirectory, projectName, "project.assets.json"), NullLogger.Instance);
                Assert.NotNull(lockFile);
                Assert.Equal(2, lockFile.Targets.Count);
                var ridTargets = lockFile.Targets.Where(e => e.RuntimeIdentifier != null ? e.RuntimeIdentifier.Equals(rid, StringComparison.CurrentCultureIgnoreCase) : false);
                Assert.Equal(1, ridTargets.Count());
                Assert.Contains("NU12", result.AllOutput); // Output errors because nuget versioning is not autoreferenced
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetToolTests_ToolPackageAndPlatformsPackageAnyRID_SucceedsAsync()
        {
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                string testDirectory = pathContext.WorkingDirectory;
                var tfm = "netcoreapp2.0";
                var projectName = "ToolRestoreProject";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var packageSource = Path.Combine(testDirectory, "packageSource");
                var packageSources = "https://api.nuget.org/v3/index.json" + ";" + packageSource;
                var rid = "win-x64";
                var packageRid = "any";
                var packageName = string.Join("ToolPackage-", tfm, packageRid);
                var autoReferencePackageName = "Microsoft.NETCore.Platforms";
                var packageVersion = NuGetVersion.Parse("1.0.0");

                var package = new SimpleTestPackageContext(packageName, packageVersion.OriginalVersion);
                package.Files.Clear();
                package.AddFile($"tools/{tfm}/{packageRid}/a.dll");
                package.AddFile($"tools/{tfm}/{packageRid}/Settings.json");

                package.PackageType = PackageType.DotnetTool;
                package.UseDefaultRuntimeAssemblies = false;
                package.PackageTypes.Add(PackageType.DotnetTool);
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, package);

                var packages = new List<PackageIdentity>()
                {
                    new PackageIdentity(packageName, packageVersion),
                    new PackageIdentity(autoReferencePackageName, NuGetVersion.Parse("2.0.1")),
                };

                _dotnetFixture.CreateDotnetToolProject(testDirectory, projectName, tfm, rid, packageSources, packages);

                var fullProjectPath = Path.Combine(workingDirectory, $"{projectName}.csproj");
                MakePackageReferenceImplicitlyDefined(fullProjectPath, autoReferencePackageName);

                // Act
                var result = _dotnetFixture.RestoreToolProjectExpectSuccess(workingDirectory, projectName, testOutputHelper: _testOutputHelper);

                // Assert
                var lockFile = LockFileUtilities.GetLockFile(Path.Combine(testDirectory, projectName, "project.assets.json"), NullLogger.Instance);
                Assert.NotNull(lockFile);
                Assert.Equal(2, lockFile.Targets.Count);
                var ridTargets = lockFile.Targets.Where(e => e.RuntimeIdentifier != null ? e.RuntimeIdentifier.Equals(rid, StringComparison.CurrentCultureIgnoreCase) : false);
                Assert.Equal(1, ridTargets.Count());
                Assert.Equal(2, ridTargets.First().Libraries.Count);
                Assert.DoesNotContain("NU12", result.AllOutput);
                Assert.DoesNotContain("NU11", result.AllOutput);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetToolTests_IncompatibleAutorefPackageAndToolsPackageAsync()
        {
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                string testDirectory = pathContext.WorkingDirectory;
                var tfm = "netcoreapp1.0";
                var incompatibletfm = "netcoreapp2.0";
                var rid = "win-x64";
                var projectName = "ToolRestoreProject";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var packageSource = Path.Combine(testDirectory, "packageSource");
                var packageSources = "https://api.nuget.org/v3/index.json" + ";" + packageSource;
                var packageName = string.Join("ToolPackage-", tfm, rid);
                var autoReferencePackageName = "Microsoft.NETCore.Platforms";
                var packageVersion = NuGetVersion.Parse("1.0.0");

                var package = new SimpleTestPackageContext(packageName, packageVersion.OriginalVersion);
                package.Files.Clear();
                package.AddFile($"tools/{tfm}/{rid}/a.dll");
                package.AddFile($"tools/{tfm}/{rid}/Settings.json");
                package.PackageType = PackageType.DotnetTool;
                package.UseDefaultRuntimeAssemblies = false;
                package.PackageTypes.Add(PackageType.DotnetTool);

                var autorefPackage = new SimpleTestPackageContext(packageName, packageVersion.OriginalVersion);
                autorefPackage.Files.Clear();
                autorefPackage.AddFile($"lib/{incompatibletfm}/b.dll");
                autorefPackage.UseDefaultRuntimeAssemblies = false;

                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, package, autorefPackage);

                var packages = new List<PackageIdentity>()
                {
                    new PackageIdentity(packageName, packageVersion),
                    new PackageIdentity(autoReferencePackageName, packageVersion)
                };

                _dotnetFixture.CreateDotnetToolProject(testDirectory, projectName, tfm, rid, packageSources, packages);

                var fullProjectPath = Path.Combine(workingDirectory, $"{projectName}.csproj");
                MakePackageReferenceImplicitlyDefined(fullProjectPath, autoReferencePackageName);

                // Act
                var result = _dotnetFixture.RestoreToolProjectExpectFailure(workingDirectory, projectName, testOutputHelper: _testOutputHelper);

                // Assert
                var lockFilePath = Path.Combine(testDirectory, projectName, "project.assets.json");
                Assert.True(File.Exists(lockFilePath), result.AllOutput);
                var lockFile = LockFileUtilities.GetLockFile(lockFilePath, NullLogger.Instance);
                Assert.NotNull(lockFile);
                Assert.Equal(2, lockFile.Targets.Count);
                var ridTargets = lockFile.Targets.Where(e => e.RuntimeIdentifier != null ? e.RuntimeIdentifier.Equals(rid, StringComparison.CurrentCultureIgnoreCase) : false);
                Assert.Equal(1, ridTargets.Count());
                Assert.Equal(2, ridTargets.First().Libraries.Count);
                Assert.Contains("NU12", result.AllOutput);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetToolTests_ToolRestoreWithFallback_SucceedsAsync()
        {
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                string testDirectory = pathContext.WorkingDirectory;
                var actualTfm = "netcoreapp2.0";
                var fallbackTfm = "net46";
                var projectName = "ToolRestoreProject";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var packageSource = Path.Combine(testDirectory, "packageSource");
                var rid = "win-x64";
                var packageName = string.Join("ToolPackage-", fallbackTfm, rid);
                var packageVersion = NuGetVersion.Parse("1.0.0");
                var packages = new List<PackageIdentity>() { new PackageIdentity(packageName, packageVersion) };

                var package = new SimpleTestPackageContext(packageName, packageVersion.OriginalVersion);
                package.Files.Clear();
                package.AddFile($"tools/{fallbackTfm}/{rid}/a.dll");
                package.AddFile($"tools/{fallbackTfm}/{rid}/Settings.json");

                package.PackageType = PackageType.DotnetTool;
                package.UseDefaultRuntimeAssemblies = false;
                package.PackageTypes.Add(PackageType.DotnetTool);
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, package);

                _dotnetFixture.CreateDotnetToolProject(testDirectory, projectName, actualTfm, rid, packageSource, packages);

                using (var stream = File.Open(Path.Combine(workingDirectory, projectName + ".csproj"), FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(
                        xml,
                        "AssetTargetFallback",
                        fallbackTfm);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                // Act
                var result = _dotnetFixture.RestoreToolProjectExpectSuccess(workingDirectory, projectName, testOutputHelper: _testOutputHelper);

                // Assert
                // Verify the assets file
                var lockFile = LockFileUtilities.GetLockFile(Path.Combine(testDirectory, projectName, "project.assets.json"), NullLogger.Instance);
                Assert.NotNull(lockFile);
                Assert.Equal(2, lockFile.Targets.Count);
                var ridTargets = lockFile.Targets.Where(e => e.RuntimeIdentifier != null ? e.RuntimeIdentifier.Equals(rid, StringComparison.CurrentCultureIgnoreCase) : false);
                Assert.Equal(1, ridTargets.Count());
                var toolsAssemblies = ridTargets.First().Libraries.First().ToolsAssemblies;
                Assert.Equal(2, toolsAssemblies.Count);
                Assert.True(toolsAssemblies.Contains($"tools/{fallbackTfm}/{rid}/a.dll"));
                Assert.True(toolsAssemblies.Contains($"tools/{fallbackTfm}/{rid}/Settings.json"));
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task DotnetToolTests_ToolRestoreWithRuntimeIdentiferGraphPath_SucceedsAsync()
        {
            using (SimpleTestPathContext pathContext = _dotnetFixture.CreateSimpleTestPathContext())
            {
                string testDirectory = pathContext.WorkingDirectory;
                var tfm = "netcoreapp2.0";
                var projectName = "ToolRestoreProject";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var packageSource = Path.Combine(testDirectory, "packageSource");
                var rid = "win-x64";
                var packageRid = "win";
                var packageName = string.Join("ToolPackage-", tfm, rid);
                var packageVersion = NuGetVersion.Parse("1.0.0");
                var packages = new List<PackageIdentity>() { new PackageIdentity(packageName, packageVersion) };

                var package = new SimpleTestPackageContext(packageName, packageVersion.OriginalVersion);
                package.Files.Clear();
                package.AddFile($"tools/{tfm}/{packageRid}/a.dll");
                package.AddFile($"tools/{tfm}/{packageRid}/Settings.json");

                package.PackageType = PackageType.DotnetTool;
                package.UseDefaultRuntimeAssemblies = false;
                package.PackageTypes.Add(PackageType.DotnetTool);
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, package);

                _dotnetFixture.CreateDotnetToolProject(testDirectory, projectName, tfm, rid, packageSource, packages);

                // set up rid graph
                var ridGraphPath = Path.Combine(testDirectory, "runtime.json");
                File.WriteAllBytes(ridGraphPath, GetTestUtilityResource("runtime.json"));

                using (var stream = File.Open(Path.Combine(workingDirectory, projectName + ".csproj"), FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(
                        xml,
                        "RuntimeIdentifierGraphPath",
                        ridGraphPath);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                // Act
                var result = _dotnetFixture.RestoreToolProjectExpectSuccess(workingDirectory, projectName, testOutputHelper: _testOutputHelper);

                // Assert
                // Verify the assets file
                var lockFile = LockFileUtilities.GetLockFile(Path.Combine(testDirectory, projectName, "project.assets.json"), NullLogger.Instance);
                Assert.NotNull(lockFile);
                Assert.Equal(2, lockFile.Targets.Count);
                var ridTargets = lockFile.Targets.Where(e => e.RuntimeIdentifier != null ? e.RuntimeIdentifier.Equals(rid, StringComparison.CurrentCultureIgnoreCase) : false);
                Assert.Equal(1, ridTargets.Count());
                var toolsAssemblies = ridTargets.First().Libraries.First().ToolsAssemblies;
                Assert.Equal(2, toolsAssemblies.Count);
                Assert.True(toolsAssemblies.Contains($"tools/{tfm}/{packageRid}/a.dll"));
                Assert.True(toolsAssemblies.Contains($"tools/{tfm}/{packageRid}/Settings.json"));
            }
        }

        private static void MakePackageReferenceImplicitlyDefined(string fullProjectPath, string packageName)
        {
            var searchString = $"'{packageName}' ";
            var text = File.ReadAllText(fullProjectPath);

            var referenceElement = text.IndexOf(searchString) + searchString.Length;
            text = text.Substring(0, referenceElement) + " IsImplicitlyDefined='true' " + text.Substring(referenceElement);
            File.WriteAllText(fullProjectPath, text);
        }

        private static byte[] GetTestUtilityResource(string name)
        {
            return ResourceTestUtility.GetResourceBytes(
                $"Microsoft.Internal.NuGet.Testing.SignedPackages.compiler.resources.{name}",
                typeof(ResourceTestUtility));
        }

        public static string GetResource(string name, Type type)
        {
            using (var reader = new StreamReader(type.GetTypeInfo().Assembly.GetManifestResourceStream(name)))
            {
                return reader.ReadToEnd();
            }
        }

    }
}
