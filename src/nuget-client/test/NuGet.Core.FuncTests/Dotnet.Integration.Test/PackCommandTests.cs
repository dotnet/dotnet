// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Licenses;
using NuGet.ProjectManagement;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace Dotnet.Integration.Test
{
    [Collection(DotnetIntegrationCollection.Name)]
    public class PackCommandTests
    {
        private MsbuildIntegrationTestFixture msbuildFixture;

        public PackCommandTests(MsbuildIntegrationTestFixture fixture)
        {
            msbuildFixture = fixture;
        }

        [PlatformFact(Platform.Windows)]
        public void CreatePackageWithFiles()
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                File.WriteAllText(Path.Combine(workingDirectory, "abc.txt"), "hello world");
                File.WriteAllText(Path.Combine(workingDirectory, "abc.props"), "<project />");
                File.WriteAllText(Path.Combine(workingDirectory, "abc.dll"), "");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    var itemGroup = @"<ItemGroup>
                    <None Include=""abc.props"" Pack=""True""  PackagePath=""build"" />
                    <None Include=""abc.dll"" Pack=""True""  PackagePath=""lib"" />
                    <Content Include=""abc.txt"" Pack=""True"" />
</ItemGroup>";
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFramework", "net5.0");
                    ProjectFileUtils.AddCustomXmlToProjectRoot(xml, itemGroup);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var files = nupkgReader.GetFiles();
                    Assert.Contains($"content/abc.txt", files);
                    Assert.Contains($"build/abc.props", files);
                    Assert.Contains($"lib/abc.dll", files);
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_NewProject_OutputsInDefaultPaths()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var nupkgPath = Path.Combine(workingDirectory, @"bin\Debug", $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, @"obj\Debug", $"{projectName}.1.0.0.nuspec");

                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, "classlib");
                msbuildFixture.PackProject(workingDirectory, projectName, string.Empty, null);

                // Assert
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the default place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the default place");
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_NewProject_ContinuousOutputInBothDefaultAndCustomPaths()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, "classlib");
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.BuildProject(workingDirectory, projectName, string.Empty);

                // With default output path
                var nupkgPath = Path.Combine(workingDirectory, @"bin\Debug", $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, @"obj\Debug", $"{projectName}.1.0.0.nuspec");

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, "--no-build", null);

                // Assert
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the default place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the default place");

                // With custom output path
                var publishDir = Path.Combine(workingDirectory, "publish");
                nupkgPath = Path.Combine(publishDir, $"{projectName}.1.0.0.nupkg");
                nuspecPath = Path.Combine(publishDir, $"{projectName}.1.0.0.nuspec");

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"--no-build -o {publishDir}", publishDir);

                // Assert
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_NewSolution_OutputInDefaultPaths()
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var solutionName = "Solution1";
                var projectName = "ClassLibrary1";
                var referencedProject1 = "ClassLibrary2";
                var referencedProject2 = "ClassLibrary3";

                var projectAndReference1Folder = "Src";
                var reference2Folder = "src";

                var projectFolder = Path.Combine(testDirectory.Path, projectAndReference1Folder, projectName);

                var projectFileRelativ = Path.Combine(projectAndReference1Folder, projectName, $"{projectName}.csproj");
                var referencedProject1RelativDir = Path.Combine(projectAndReference1Folder, referencedProject1, $"{referencedProject1}.csproj");
                var referencedProject2RelativDir = Path.Combine(reference2Folder, referencedProject2, $"{referencedProject2}.csproj");

                msbuildFixture.CreateDotnetNewProject(Path.Combine(testDirectory.Path, projectAndReference1Folder), projectName, "classlib");
                msbuildFixture.CreateDotnetNewProject(Path.Combine(testDirectory.Path, projectAndReference1Folder), referencedProject1, "classlib");
                msbuildFixture.CreateDotnetNewProject(Path.Combine(testDirectory.Path, reference2Folder), referencedProject2, "classlib");

                msbuildFixture.RunDotnet(testDirectory.Path, $"new sln -n {solutionName}");
                msbuildFixture.RunDotnet(testDirectory.Path, $"sln {solutionName}.sln add {projectFileRelativ}");
                msbuildFixture.RunDotnet(testDirectory.Path, $"sln {solutionName}.sln add {referencedProject1RelativDir}");
                msbuildFixture.RunDotnet(testDirectory.Path, $"sln {solutionName}.sln add {referencedProject2RelativDir}");

                var projectFile = Path.Combine(testDirectory.Path, projectFileRelativ);
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    var attributes = new Dictionary<string, string>();
                    var properties = new Dictionary<string, string>();

                    ProjectFileUtils.AddItem(xml, "ProjectReference", @"..\ClassLibrary2\ClassLibrary2.csproj", string.Empty, properties, attributes);
                    ProjectFileUtils.AddItem(xml, "ProjectReference", @"..\ClassLibrary3\ClassLibrary3.csproj", string.Empty, properties, attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreSolution(testDirectory, solutionName, string.Empty);

                var nupkgPath = Path.Combine(projectFolder, @"bin\Debug", $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(projectFolder, @"obj\Debug", $"{projectName}.1.0.0.nuspec");

                // Act
                msbuildFixture.PackSolution(testDirectory, solutionName, string.Empty, null);

                // Assert
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the default place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the default place");
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_NewSolution_ContinuousOutputInDefaultPaths()
        {
            // Arrange
            using (var testDirectory = TestDirectory.Create())
            {
                var solutionName = "Solution1";
                var projectName = "ClassLibrary1";
                var referencedProject1 = "ClassLibrary2";
                var referencedProject2 = "ClassLibrary3";

                var projectAndReference1Folder = "Src";
                var reference2Folder = "src";

                var projectFolder = Path.Combine(testDirectory.Path, projectAndReference1Folder, projectName);

                var projectFileRelativ = Path.Combine(projectAndReference1Folder, projectName, $"{projectName}.csproj");
                var referencedProject1RelativDir = Path.Combine(projectAndReference1Folder, referencedProject1, $"{referencedProject1}.csproj");
                var referencedProject2RelativDir = Path.Combine(reference2Folder, referencedProject2, $"{referencedProject2}.csproj");

                msbuildFixture.CreateDotnetNewProject(Path.Combine(testDirectory.Path, projectAndReference1Folder), projectName, "classlib");
                msbuildFixture.CreateDotnetNewProject(Path.Combine(testDirectory.Path, projectAndReference1Folder), referencedProject1, "classlib");
                msbuildFixture.CreateDotnetNewProject(Path.Combine(testDirectory.Path, reference2Folder), referencedProject2, "classlib");

                msbuildFixture.RunDotnet(testDirectory.Path, $"new sln -n {solutionName}");
                msbuildFixture.RunDotnet(testDirectory.Path, $"sln {solutionName}.sln add {projectFileRelativ}");
                msbuildFixture.RunDotnet(testDirectory.Path, $"sln {solutionName}.sln add {referencedProject1RelativDir}");
                msbuildFixture.RunDotnet(testDirectory.Path, $"sln {solutionName}.sln add {referencedProject2RelativDir}");

                var projectFile = Path.Combine(testDirectory.Path, projectFileRelativ);
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    var attributes = new Dictionary<string, string>();
                    var properties = new Dictionary<string, string>();

                    ProjectFileUtils.AddItem(xml, "ProjectReference", @"..\ClassLibrary2\ClassLibrary2.csproj", string.Empty, properties, attributes);
                    ProjectFileUtils.AddItem(xml, "ProjectReference", @"..\ClassLibrary3\ClassLibrary3.csproj", string.Empty, properties, attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreSolution(testDirectory, solutionName, string.Empty);
                msbuildFixture.BuildSolution(testDirectory, solutionName, string.Empty);

                // With default output path within project folder

                // Arrange
                var nupkgPath = Path.Combine(projectFolder, @"bin\Debug", $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(projectFolder, @"obj\Debug", $"{projectName}.1.0.0.nuspec");

                // Act
                msbuildFixture.PackSolution(testDirectory, solutionName, "--no-build", null);

                // Assert
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the default place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the default place");
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackNewDefaultProject_NupkgExists()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib -f netstandard2.0");
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the output .nuspec.
                    Assert.Equal("ClassLibrary1", nuspecReader.GetId());
                    Assert.Equal("1.0.0", nuspecReader.GetVersion().ToFullString());
                    Assert.Equal("ClassLibrary1", nuspecReader.GetAuthors());
                    Assert.Equal("", nuspecReader.GetOwners());
                    Assert.Equal("Package Description", nuspecReader.GetDescription());
                    Assert.False(nuspecReader.GetRequireLicenseAcceptance());

                    var dependencyGroups = nuspecReader.GetDependencyGroups().ToList();
                    Assert.Equal(1, dependencyGroups.Count);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard20, dependencyGroups[0].TargetFramework);
                    var packages = dependencyGroups[0].Packages.ToList();
                    Assert.Equal(0, packages.Count);

                    // Validate the assets.
                    var libItems = nupkgReader.GetLibItems().ToList();
                    Assert.Equal(1, libItems.Count);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard20, libItems[0].TargetFramework);
                    Assert.Equal(new[] { "lib/netstandard2.0/ClassLibrary1.dll" }, libItems[0].Items);
                }

            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackToolUsingAlias_DoesNotWarnAboutNoExactMatchInDependencyGroupAndLibRefDirectories()
        {
            // Ref: https://github.com/NuGet/Home/issues/10097
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                // Arrange
                var projectName = "ConsoleApp1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " console");
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);

                    ProjectFileUtils.AddProperty(xml, "PackAsTool", "true");
                    ProjectFileUtils.ChangeProperty(xml, "TargetFramework", "myalias");

                    var tfmProps = new Dictionary<string, string>();
                    tfmProps["TargetFrameworkIdentifier"] = ".NETCoreApp";
                    tfmProps["TargetFrameworkVersion"] = "v3.1";
                    tfmProps["TargetFrameworkMoniker"] = ".NETCoreApp,Version=v3.1";
                    ProjectFileUtils.AddProperties(xml, tfmProps, " '$(TargetFramework)' == 'myalias' ");

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                // Act
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                // Assert
                result.AllOutput.Should().NotContain("NU5128");
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackNewDefaultProject_IncludeSymbolsWithSnupkg()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib -f netstandard2.0");
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName, $"--include-symbols /p:SymbolPackageFormat=snupkg -o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var symbolPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.snupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(symbolPath), "The output .snupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                using (var symbolReader = new PackageArchiveReader(symbolPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the assets.
                    var libItems = nupkgReader.GetLibItems().ToList();
                    var libSymItems = symbolReader.GetLibItems().ToList();
                    Assert.Equal(1, libItems.Count);
                    Assert.Equal(1, libSymItems.Count);
                    Assert.Equal(symbolReader.GetPackageTypes().Count, 1);
                    Assert.Equal(symbolReader.GetPackageTypes()[0], NuGet.Packaging.Core.PackageType.SymbolsPackage);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard20, libItems[0].TargetFramework);
                    Assert.Equal(new[] { "lib/netstandard2.0/ClassLibrary1.dll" }, libItems[0].Items);
                    Assert.Equal(new[] { "lib/netstandard2.0/ClassLibrary1.pdb" }, libSymItems[0].Items);
                }

            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackProjectWithPackageType_SnupkgContainsOnlyOnePackageType()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib -f netstandard2.0");
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "PackageType", NuGet.Packaging.Core.PackageType.Dependency.Name);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName, $"--include-symbols /p:SymbolPackageFormat=snupkg -o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var symbolPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.snupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(symbolPath), "The output .snupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                using (var symbolReader = new PackageArchiveReader(symbolPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the assets.
                    var libItems = nupkgReader.GetLibItems().ToList();
                    var libSymItems = symbolReader.GetLibItems().ToList();
                    Assert.Equal(1, libItems.Count);
                    Assert.Equal(1, libSymItems.Count);
                    Assert.Equal(nupkgReader.GetPackageTypes().Count, 1);
                    Assert.Equal(nupkgReader.GetPackageTypes()[0], NuGet.Packaging.Core.PackageType.Dependency);
                    Assert.Equal(symbolReader.GetPackageTypes().Count, 1);
                    Assert.Equal(symbolReader.GetPackageTypes()[0], NuGet.Packaging.Core.PackageType.SymbolsPackage);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard20, libItems[0].TargetFramework);
                    Assert.Equal(new[] { "lib/netstandard2.0/ClassLibrary1.dll" }, libItems[0].Items);
                    Assert.Equal(new[] { "lib/netstandard2.0/ClassLibrary1.pdb" }, libSymItems[0].Items);
                }

            }
        }

        [PlatformTheory(Platform.Windows, Skip = "https://github.com/NuGet/Home/issues/12194")]
        [InlineData(true)]
        [InlineData(false)]
        public void PackCommand_PackConsoleAppWithRID_NupkgValid(bool includeSymbols)
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ConsoleApp1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " console");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.ChangeProperty(xml, "TargetFramework", "netcoreapp2.1");
                    ProjectFileUtils.AddProperty(xml, "RuntimeIdentifier", "win7-x64");
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var args = includeSymbols ? $"-o {workingDirectory} --include-symbols" : $"-o {workingDirectory}";
                msbuildFixture.PackProject(workingDirectory, projectName, args);

                var nupkgExtension = includeSymbols ? ".symbols.nupkg" : ".nupkg";
                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0{nupkgExtension}");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), $"The output {nupkgExtension} is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    // Validate the assets.
                    var libItems = nupkgReader.GetLibItems().ToList();
                    Assert.Equal(1, libItems.Count);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetCoreApp21, libItems[0].TargetFramework);
                    if (includeSymbols)
                    {
                        Assert.Equal(new[]
                        {
                            "lib/netcoreapp2.1/ConsoleApp1.dll",
                            "lib/netcoreapp2.1/ConsoleApp1.pdb",
                            "lib/netcoreapp2.1/ConsoleApp1.runtimeconfig.json"
                        }, libItems[0].Items);
                    }
                    else
                    {
                        Assert.Equal(new[]
                        {
                            "lib/netcoreapp2.1/ConsoleApp1.dll",
                            "lib/netcoreapp2.1/ConsoleApp1.runtimeconfig.json"
                        }, libItems[0].Items);
                    }
                }

            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackProject_PackageReferenceFloatingVersionRange()
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName);

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFramework", "net45");

                    var attributes = new Dictionary<string, string>();

                    attributes["Version"] = "(10.0.*,11.0.1]";
                    ProjectFileUtils.AddItem(
                        xml,
                        "PackageReference",
                        "Newtonsoft.Json",
                        string.Empty,
                        new Dictionary<string, string>(),
                        attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                // Assert
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    var dependencyGroups = nuspecReader
                        .GetDependencyGroups()
                        .OrderBy(x => x.TargetFramework,
                            new NuGetFrameworkSorter())
                        .ToList();

                    Assert.Equal(1,
                        dependencyGroups.Count);

                    Assert.Equal(FrameworkConstants.CommonFrameworks.Net45, dependencyGroups[0].TargetFramework);
                    var packagesB = dependencyGroups[0].Packages.ToList();
                    Assert.Equal(1, packagesB.Count);
                    Assert.Equal("Newtonsoft.Json", packagesB[0].Id);
                    Assert.Equal(new VersionRange(new NuGetVersion("10.0.3"), false, new NuGetVersion("11.0.1"), true), packagesB[0].VersionRange);
                    Assert.Equal(new List<string> { "Analyzers", "Build" }, packagesB[0].Exclude);
                    Assert.Empty(packagesB[0].Include);
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task PackCommand_PackProject_PackageReferenceAllStableFloatingVersionRange_UsesRestoredVersionInNuspecAsync()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = msbuildFixture.CreateSimpleTestPathContext())
            {
                var projectName = "ClassLibrary1";
                var availableVersions = "1.0.0;2.0.0";
                var workingDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(pathContext.SolutionRoot, projectName);

                foreach (string version in availableVersions.Split(';'))
                {
                    // Set up the package and source
                    var package = new SimpleTestPackageContext()
                    {
                        Id = "x",
                        Version = version
                    };

                    package.Files.Clear();
                    package.AddFile($"lib/net472/a.dll");

                    await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                         pathContext.PackageSource,
                         PackageSaveMode.Defaultv3,
                         package);
                }

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFramework", "net472");

                    var attributes = new Dictionary<string, string>();

                    attributes["Version"] = "*";
                    ProjectFileUtils.AddItem(
                        xml,
                        "PackageReference",
                        "x",
                        string.Empty,
                        new Dictionary<string, string>(),
                        attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                // Assert
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    var dependencyGroups = nuspecReader
                        .GetDependencyGroups()
                        .OrderBy(x => x.TargetFramework,
                            new NuGetFrameworkSorter())
                        .ToList();

                    Assert.Equal(1,
                        dependencyGroups.Count);

                    Assert.Equal(FrameworkConstants.CommonFrameworks.Net472, dependencyGroups[0].TargetFramework);
                    var packagesB = dependencyGroups[0].Packages.ToList();
                    Assert.Equal(1, packagesB.Count);
                    Assert.Equal("x", packagesB[0].Id);
                    Assert.Equal(new VersionRange(new NuGetVersion("2.0.0"), true, null, false), packagesB[0].VersionRange);
                    Assert.Equal(new List<string> { "Analyzers", "Build" }, packagesB[0].Exclude);
                    Assert.Empty(packagesB[0].Include);
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackProject_SupportMultipleFrameworks()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = msbuildFixture.CreateSimpleTestPathContext())
            {
                SimpleTestSettingsContext settings = pathContext.Settings;
                settings.AddNetStandardFeeds();

                string testDirectory = pathContext.WorkingDirectory;
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory, projectName);

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFrameworks", "netcoreapp1.0;net45");

                    var attributes = new Dictionary<string, string>();

                    attributes["Version"] = "9.0.1";
                    ProjectFileUtils.AddItem(
                        xml,
                        "PackageReference",
                        "Newtonsoft.Json",
                        "net45",
                        new Dictionary<string, string>(),
                        attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                // Assert
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    var dependencyGroups = nuspecReader
                        .GetDependencyGroups()
                        .OrderBy(x => x.TargetFramework,
                            new NuGetFrameworkSorter())
                        .ToList();

                    Assert.Equal(2,
                        dependencyGroups.Count);

                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetCoreApp10,
                        dependencyGroups[0].TargetFramework);
                    var packagesA = dependencyGroups[0].Packages.ToList();
                    Assert.Equal(1,
                        packagesA.Count);
                    Assert.Equal("Microsoft.NETCore.App", packagesA[0].Id);
                    Assert.Equal(new VersionRange(new NuGetVersion("1.0.5")), packagesA[0].VersionRange);
                    Assert.Equal(new List<string> { "Analyzers", "Build" }, packagesA[0].Exclude);
                    Assert.Empty(packagesA[0].Include);

                    Assert.Equal(FrameworkConstants.CommonFrameworks.Net45, dependencyGroups[1].TargetFramework);
                    var packagesB = dependencyGroups[1].Packages.ToList();
                    Assert.Equal(1, packagesB.Count);
                    Assert.Equal("Newtonsoft.Json", packagesB[0].Id);
                    Assert.Equal(new VersionRange(new NuGetVersion("9.0.1")), packagesB[0].VersionRange);
                    Assert.Equal(new List<string> { "Analyzers", "Build" }, packagesB[0].Exclude);
                    Assert.Empty(packagesB[0].Include);

                    // Validate the assets.
                    var libItems = nupkgReader.GetLibItems().ToList();
                    Assert.Equal(2, libItems.Count);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetCoreApp10, libItems[0].TargetFramework);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.Net45, libItems[1].TargetFramework);
                    Assert.Equal(
                        new[]
                        {"lib/netcoreapp1.0/ClassLibrary1.dll", "lib/netcoreapp1.0/ClassLibrary1.runtimeconfig.json"},
                        libItems[0].Items);
                    Assert.Equal(new[] { "lib/net45/ClassLibrary1.exe" },
                        libItems[1].Items);
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackProject_DependenciesWithContentFiles_NupkgExcludesContentFilesFromDependencies()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = msbuildFixture.CreateSimpleTestPathContext())
            {
                string testDirectory = pathContext.WorkingDirectory;
                // layout
                var topName = "top";
                var basePackageName = "BasePackage";
                var topPath = Path.Combine(testDirectory, topName);
                var basePackagePath = Path.Combine(testDirectory, basePackageName);
                var pkgsPath = Path.Combine(testDirectory, "pkgs");
                Directory.CreateDirectory(topPath);
                Directory.CreateDirectory(pkgsPath);
                Directory.CreateDirectory(basePackagePath);

                string tfm = Constants.DefaultTargetFramework.GetShortFolderName();

                // Base Package
                var basePackageProjectContent = @$"<Project Sdk='Microsoft.NET.Sdk'>
  <PropertyGroup>
    <TargetFramework>{tfm}</TargetFramework>
    <PackageOutputPath>$(MSBuildThisFileDirectory)..\pkgs</PackageOutputPath>
  </PropertyGroup>
  <ItemGroup>
    <Content Include='data.json'>
      <PackagePath>contentFiles/any/any/data.json</PackagePath>
    </Content>
  </ItemGroup>
</Project>";
                File.WriteAllText(Path.Combine(basePackagePath, $"{basePackageName}.csproj"), basePackageProjectContent);

                var dataJsonContent = @"{""data"":""file""}";

                File.WriteAllText(Path.Combine(basePackagePath, "data.json"), dataJsonContent);

                // Top package
                var customNuGetConfigContent = @"<configuration>
  <packageSources>
    <clear />
    <add key='nuget' value ='https://api.nuget.org/v3/index.json' />
    <add key ='local' value ='../pkgs' />
  </packageSources>
</configuration>";

                File.WriteAllText(Path.Combine(topPath, "NuGet.Config"), customNuGetConfigContent);

                var topProjectContent = @$"<Project Sdk='Microsoft.NET.Sdk'>
  <PropertyGroup>
    <TargetFramework>{tfm}</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include='BasePackage' Version='1.0.0' />
  </ItemGroup>
</Project>";

                File.WriteAllText(Path.Combine(topPath, $"{topName}.csproj"), topProjectContent);

                // create the base package
                msbuildFixture.PackProject(basePackagePath, basePackageName, "");

                // create the top package
                msbuildFixture.PackProject(topPath, topName, $"-o {topPath}");

                var basePkgPath = Path.Combine(pkgsPath, "BasePackage.1.0.0.nupkg");
                Assert.True(File.Exists(basePkgPath));
                var topPkgPath = Path.Combine(topPath, "top.1.0.0.nupkg");
                Assert.True(File.Exists(topPkgPath));

                // Asset package content
                using (var par = new PackageArchiveReader(topPkgPath))
                {
                    foreach (var pkgFile in par.GetFiles())
                    {
                        Assert.DoesNotContain("data.json", pkgFile);
                    }
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData(null, null, null, true, "", "Analyzers,Build")]
        [InlineData(null, "Native", null, true, "", "Analyzers,Build,Native")]
        [InlineData("Compile", null, null, true, "", "Analyzers,Build,BuildTransitive,Native,Runtime")]
        [InlineData("Compile;Runtime", null, null, true, "", "Analyzers,Build,BuildTransitive,Native")]
        [InlineData("All", null, "None", true, "All", "")]
        [InlineData("All", null, "Compile", true, "Analyzers,Build,BuildTransitive,ContentFiles,Native,Runtime", "")]
        [InlineData("All", null, "Compile;Build", true, "Analyzers,BuildTransitive,ContentFiles,Native,Runtime", "")]
        [InlineData("All", "Native", "Compile;Build", true, "Analyzers,BuildTransitive,ContentFiles,Runtime", "")]
        [InlineData("All", "Native", "Native;Build", true, "Analyzers,BuildTransitive,Compile,ContentFiles,Runtime", "")]
        [InlineData("Compile", "Native", "Native;Build", true, "", "Analyzers,Build,BuildTransitive,Native,Runtime")]
        [InlineData("All", "All", null, false, null, null)]
        [InlineData("Compile;Runtime", "All", null, false, null, null)]
        [InlineData(null, null, "All", false, null, null)]
        public void PackCommand_SupportsIncludeExcludePrivateAssets_OnPackages(
            string includeAssets,
            string excludeAssets,
            string privateAssets,
            bool hasPackage,
            string expectedInclude,
            string expectedExclude)
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFramework", "net45");

                    var attributes = new Dictionary<string, string>();
                    attributes["Version"] = "9.0.1";

                    var properties = new Dictionary<string, string>();
                    if (!string.IsNullOrEmpty(includeAssets))
                    {
                        properties["IncludeAssets"] = includeAssets;
                    }
                    if (!string.IsNullOrEmpty(excludeAssets))
                    {
                        properties["ExcludeAssets"] = excludeAssets;
                    }
                    if (!string.IsNullOrEmpty(privateAssets))
                    {
                        properties["PrivateAssets"] = privateAssets;
                    }

                    ProjectFileUtils.AddItem(
                        xml,
                        "PackageReference",
                        "Newtonsoft.Json",
                        string.Empty,
                        properties,
                        attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;
                    var package = nuspecReader
                        .GetDependencyGroups()
                        .SingleOrDefault()?
                        .Packages
                        .SingleOrDefault();

                    if (!hasPackage)
                    {
                        Assert.Null(package);
                    }
                    else
                    {
                        Assert.NotNull(package);
                        Assert.Equal(expectedInclude, string.Join(",", package.Include));
                        Assert.Equal(expectedExclude, string.Join(",", package.Exclude));
                    }
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackProject_AddsProjectRefsAsPackageRefs()
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var referencedProject = "ClassLibrary2";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, "console");
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, referencedProject, "classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);

                    var attributes = new Dictionary<string, string>();

                    var properties = new Dictionary<string, string>();
                    ProjectFileUtils.AddItem(
                        xml,
                        "ProjectReference",
                        @"..\ClassLibrary2\ClassLibrary2.csproj",
                        string.Empty,
                        properties,
                        attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                // Assert
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    var dependencyGroups = nuspecReader
                        .GetDependencyGroups()
                        .OrderBy(x => x.TargetFramework,
                            new NuGetFrameworkSorter())
                        .ToList();

                    Assert.Equal(1,
                        dependencyGroups.Count);

                    var packagesA = dependencyGroups[0].Packages.ToList();
                    Assert.Equal(1,
                        packagesA.Count);

                    Assert.Equal("ClassLibrary2", packagesA[0].Id);
                    Assert.Equal(new VersionRange(new NuGetVersion("1.0.0")), packagesA[0].VersionRange);
                    Assert.Equal(new List<string> { "Analyzers", "Build" }, packagesA[0].Exclude);
                    Assert.Empty(packagesA[0].Include);

                    // Validate the assets.
                    var libItems = nupkgReader.GetLibItems().ToList();
                    libItems.Should().HaveCount(1);
                    var files = libItems[0].Items;
                    files.Should().HaveCount(2);
                    files.Should().ContainSingle(filePath => filePath.Contains("ClassLibrary1.runtimeconfig.json"));
                    files.Should().ContainSingle(filePath => filePath.Contains("ClassLibrary1.dll"));
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("TargetFramework", "netstandard1.4")]
        [InlineData("TargetFrameworks", "netstandard1.4;net46")]
#if NETCOREAPP5_0
        [InlineData("TargetFramework", "net5.0")]
        [InlineData("TargetFrameworks", "netstandard1.4;net5.0")]
#endif
        public void PackCommand_PackProject_ExactVersionOverrideProjectRefVersionInMsbuild(string tfmProperty, string tfmValue)
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var referencedProject = "ClassLibrary2";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                var refProjectFile = Path.Combine(testDirectory, referencedProject, $"{referencedProject}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, referencedProject, "classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                using (var refStream = new FileStream(refProjectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);

                    var attributes = new Dictionary<string, string>();

                    var properties = new Dictionary<string, string>();
                    var target = @"<Target Name=""_ExactProjectReferencesVersion"" AfterTargets=""_GetProjectReferenceVersions"">
    <ItemGroup>
      <_ProjectReferencesWithExactVersions Include=""@(_ProjectReferencesWithVersions)"">
        <ProjectVersion>[%(_ProjectReferencesWithVersions.ProjectVersion)]</ProjectVersion>
      </_ProjectReferencesWithExactVersions>
    </ItemGroup>

    <ItemGroup>
      <_ProjectReferencesWithVersions Remove=""@(_ProjectReferencesWithVersions)"" />
      <_ProjectReferencesWithVersions Include=""@(_ProjectReferencesWithExactVersions)"" />
    </ItemGroup>
  </Target>";
                    ProjectFileUtils.AddItem(
                        xml,
                        "ProjectReference",
                        @"..\ClassLibrary2\ClassLibrary2.csproj",
                        string.Empty,
                        properties,
                        attributes);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, tfmProperty, tfmValue);
                    ProjectFileUtils.AddCustomXmlToProjectRoot(xml, target);

                    var refXml = XDocument.Load(refStream);
                    ProjectFileUtils.AddProperty(refXml, "PackageVersion", "1.2.3-alpha", "'$(ExcludeRestorePackageImports)' != 'true'");
                    ProjectFileUtils.SetTargetFrameworkForProject(refXml, tfmProperty, tfmValue);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                    ProjectFileUtils.WriteXmlToFile(refXml, refStream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                // Assert
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    var dependencyGroups = nuspecReader
                        .GetDependencyGroups()
                        .OrderBy(x => x.TargetFramework,
                            new NuGetFrameworkSorter())
                        .ToList();

                    Assert.Equal(tfmValue.Split(';').Count(),
                        dependencyGroups.Count);
                    foreach (var depGroup in dependencyGroups)
                    {
                        var packages = depGroup.Packages.ToList();
                        var package = packages.Where(t => t.Id.Equals("ClassLibrary2")).First();
                        Assert.Equal(new VersionRange(new NuGetVersion("1.2.3-alpha"), true, new NuGetVersion("1.2.3-alpha"), true), package.VersionRange);
                    }
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("TargetFramework", "netstandard1.4")]
        [InlineData("TargetFrameworks", "netstandard1.4;net46")]
#if NETCOREAPP5_0
        [InlineData("TargetFramework", "net5.0")]
        [InlineData("TargetFrameworks", "netstandard1.4;net5.0")]
#endif
        public void PackCommand_PackProject_GetsProjectRefVersionFromMsbuild(string tfmProperty, string tfmValue)
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var referencedProject = "ClassLibrary2";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                var refProjectFile = Path.Combine(testDirectory, referencedProject, $"{referencedProject}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, referencedProject, "classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                using (var refStream = new FileStream(refProjectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);

                    var attributes = new Dictionary<string, string>();

                    var properties = new Dictionary<string, string>();
                    ProjectFileUtils.AddItem(
                        xml,
                        "ProjectReference",
                        @"..\ClassLibrary2\ClassLibrary2.csproj",
                        string.Empty,
                        properties,
                        attributes);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, tfmProperty, tfmValue);

                    var refXml = XDocument.Load(refStream);
                    ProjectFileUtils.AddProperty(refXml, "PackageVersion", "1.2.3-alpha", "'$(ExcludeRestorePackageImports)' != 'true'");
                    ProjectFileUtils.SetTargetFrameworkForProject(refXml, tfmProperty, tfmValue);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                    ProjectFileUtils.WriteXmlToFile(refXml, refStream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                // Assert
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    var dependencyGroups = nuspecReader
                        .GetDependencyGroups()
                        .OrderBy(x => x.TargetFramework,
                            new NuGetFrameworkSorter())
                        .ToList();

                    Assert.Equal(tfmValue.Split(';').Count(),
                        dependencyGroups.Count);
                    foreach (var depGroup in dependencyGroups)
                    {
                        var packages = depGroup.Packages.ToList();
                        var package = packages.Where(t => t.Id.Equals("ClassLibrary2")).First();
                        Assert.Equal(new VersionRange(new NuGetVersion("1.2.3-alpha")), package.VersionRange);
                    }
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("TargetFramework", "netstandard1.4")]
        [InlineData("TargetFrameworks", "netstandard1.4;net46")]
#if NETCOREAPP5_0
        [InlineData("TargetFramework", "net5.0")]
        [InlineData("TargetFrameworks", "netstandard1.4;net5.0")]
#endif
        public void PackCommand_PackProject_GetPackageVersionDependsOnWorks(string tfmProperty, string tfmValue)
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var referencedProject = "ClassLibrary2";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                var refProjectFile = Path.Combine(testDirectory, referencedProject, $"{referencedProject}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, referencedProject, "classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                using (var refStream = new FileStream(refProjectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);

                    var attributes = new Dictionary<string, string>();

                    var properties = new Dictionary<string, string>();
                    var target = @"<Target Name=""CalculatePackageVersion"">
    <PropertyGroup>
      <PackageVersion>1.2.3-alpha</PackageVersion>
    </PropertyGroup>
  </Target>";
                    ProjectFileUtils.AddItem(
                        xml,
                        "ProjectReference",
                        @"..\ClassLibrary2\ClassLibrary2.csproj",
                        string.Empty,
                        properties,
                        attributes);
                    ProjectFileUtils.AddProperty(xml, "GetPackageVersionDependsOn", "CalculatePackageVersion");
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, tfmProperty, tfmValue);
                    ProjectFileUtils.AddCustomXmlToProjectRoot(xml, target);

                    var refXml = XDocument.Load(refStream);
                    ProjectFileUtils.AddProperty(refXml, "GetPackageVersionDependsOn", "CalculatePackageVersion");
                    ProjectFileUtils.SetTargetFrameworkForProject(refXml, tfmProperty, tfmValue);
                    ProjectFileUtils.AddCustomXmlToProjectRoot(refXml, target);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                    ProjectFileUtils.WriteXmlToFile(refXml, refStream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.2.3-alpha.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.2.3-alpha.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                // Assert
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    var dependencyGroups = nuspecReader
                        .GetDependencyGroups()
                        .OrderBy(x => x.TargetFramework,
                            new NuGetFrameworkSorter())
                        .ToList();

                    Assert.Equal(tfmValue.Split(';').Count(),
                        dependencyGroups.Count);
                    foreach (var depGroup in dependencyGroups)
                    {
                        var packages = depGroup.Packages.ToList();
                        var package = packages.Where(t => t.Id.Equals("ClassLibrary2")).First();
                        Assert.Equal(new VersionRange(new NuGetVersion("1.2.3-alpha")), package.VersionRange);
                    }
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackProject_PacksFromNuspec()
        {
            var nuspecFileContent = @"<?xml version=""1.0""?>
<package>
  <metadata>
    <id>PackedFromNuspec</id>
    <version>1.2.1</version>
    <authors>Microsoft</authors>
    <owners>NuGet</owners>
    <description>This was packed from nuspec</description>
  </metadata>
  <files>
    <file src=""abc.txt"" target=""CoreCLR/"" />
  </files>
</package>";
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                File.WriteAllText(Path.Combine(workingDirectory, "input.nuspec"), nuspecFileContent);
                File.WriteAllText(Path.Combine(workingDirectory, "abc.txt"), "sample text");

                msbuildFixture.PackProject(workingDirectory, projectName,
                    $"-o {workingDirectory} /p:NuspecFile=input.nuspec");

                var nupkgPath = Path.Combine(workingDirectory, $"PackedFromNuspec.1.2.1.nupkg");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the .nuspec.
                    Assert.Equal("PackedFromNuspec", nuspecReader.GetId());
                    Assert.Equal("1.2.1", nuspecReader.GetVersion().ToFullString());
                    Assert.Equal("Microsoft", nuspecReader.GetAuthors());
                    Assert.Equal("NuGet", nuspecReader.GetOwners());
                    Assert.Equal("This was packed from nuspec", nuspecReader.GetDescription());
                    Assert.False(nuspecReader.GetRequireLicenseAcceptance());

                    // Validate the assets.
                    var libItems = nupkgReader.GetFiles("CoreCLR").ToArray();
                    Assert.Equal("CoreCLR/abc.txt", libItems[0]);
                }

            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackProject_EmptyNuspecFilePropertyWithNuspecProperties()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "NuspecProperties", "token1=value1");

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                msbuildFixture.PackProject(workingDirectory, projectName,
                    $"-o {workingDirectory} /p:NuspecFile=");

                var nupkgPath = Path.Combine(workingDirectory, $"ClassLibrary1.1.0.0.nupkg");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");

            }
        }

        [PlatformFact(Platform.Windows)]
        // This test is asserting that nuspec can be packed via dotnet.exe
        // without having to specify IncludeBuildOutput=false when using the
        // --no-build switch.
        public void PackCommand_PackProject_PackNuspecWithoutBuild()
        {
            var nuspecFileContent = @"<?xml version=""1.0""?>
<package>
  <metadata>
    <id>PackedFromNuspec</id>
    <version>1.2.1</version>
    <authors>Microsoft</authors>
    <owners>NuGet</owners>
    <description>This was packed from nuspec</description>
  </metadata>
  <files>
    <file src=""abc.txt"" target=""CoreCLR/"" />
  </files>
</package>";
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");
                msbuildFixture.BuildProject(workingDirectory, projectName, "/restore");
                File.WriteAllText(Path.Combine(workingDirectory, "abc.nuspec"), nuspecFileContent);
                File.WriteAllText(Path.Combine(workingDirectory, "abc.txt"), "sample text");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "NuspecFile", "abc.nuspec");
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.PackProject(workingDirectory, projectName,
                    $"-o {workingDirectory} --no-build");

                var nupkgPath = Path.Combine(workingDirectory, $"PackedFromNuspec.1.2.1.nupkg");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the .nuspec.
                    Assert.Equal("PackedFromNuspec", nuspecReader.GetId());
                    Assert.Equal("1.2.1", nuspecReader.GetVersion().ToFullString());
                    Assert.Equal("Microsoft", nuspecReader.GetAuthors());
                    Assert.Equal("NuGet", nuspecReader.GetOwners());
                    Assert.Equal("This was packed from nuspec", nuspecReader.GetDescription());
                    Assert.False(nuspecReader.GetRequireLicenseAcceptance());

                    // Validate the assets.
                    var libItems = nupkgReader.GetFiles("CoreCLR").ToArray();
                    Assert.Equal("CoreCLR/abc.txt", libItems[0]);
                }

            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackNewDefaultProject_InstallPackageToOutputPath()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib -f netstandard2.0");
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory} /p:OutputFileNamesWithoutVersion=true /p:InstallPackageToOutputPath=true");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.nupkg");
                var nupkgSha512Path = Path.Combine(workingDirectory, $"{projectName}.nupkg.sha512");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nupkgSha512Path), "The output .sha512 is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the output .nuspec.
                    Assert.Equal("ClassLibrary1", nuspecReader.GetId());
                    Assert.Equal("1.0.0", nuspecReader.GetVersion().ToFullString());
                    Assert.Equal("ClassLibrary1", nuspecReader.GetAuthors());
                    Assert.Equal("", nuspecReader.GetOwners());
                    Assert.Equal("Package Description", nuspecReader.GetDescription());
                    Assert.False(nuspecReader.GetRequireLicenseAcceptance());

                    var dependencyGroups = nuspecReader.GetDependencyGroups().ToList();
                    Assert.Equal(1, dependencyGroups.Count);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard20, dependencyGroups[0].TargetFramework);
                    var packages = dependencyGroups[0].Packages.ToList();
                    Assert.Equal(0, packages.Count);

                    // Validate the assets.
                    var libItems = nupkgReader.GetLibItems().ToList();
                    Assert.Equal(1, libItems.Count);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard20, libItems[0].TargetFramework);
                    Assert.Equal(new[] { "lib/netstandard2.0/ClassLibrary1.dll" }, libItems[0].Items);
                }

            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackProject_PacksFromNuspec_InstallPackageToOutputPath()
        {
            var nuspecFileContent = @"<?xml version=""1.0""?>
<package>
  <metadata>
    <id>PackedFromNuspec</id>
    <version>1.2.1</version>
    <authors>Microsoft</authors>
    <owners>NuGet</owners>
    <description>This was packed from nuspec</description>
  </metadata>
</package>";
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                File.WriteAllText(Path.Combine(workingDirectory, "input.nuspec"), nuspecFileContent);

                msbuildFixture.PackProject(
                    workingDirectory,
                    projectName,
                    $"-o {workingDirectory} /p:NuspecFile=input.nuspec /p:OutputFileNamesWithoutVersion=true /p:InstallPackageToOutputPath=true");

                var nuspecFilePath = Path.Combine(workingDirectory, "PackedFromNuspec.nuspec");
                var nupackageFilePath = Path.Combine(workingDirectory, "PackedFromNuspec.nupkg");
                var nupackageSha512FilePath = Path.Combine(workingDirectory, "PackedFromNuspec.nupkg.sha512");
                Assert.True(File.Exists(nuspecFilePath), "The output .nuspec is not in the expected place: " + nuspecFilePath);
                Assert.True(File.Exists(nupackageFilePath), "The output .nupkg is not in the expected place: " + nupackageFilePath);
                Assert.True(File.Exists(nupackageSha512FilePath), "The output .sha512 is not in the expected place: " + nupackageSha512FilePath);

                using (var nupkgReader = new PackageArchiveReader(nupackageFilePath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the .nuspec.
                    Assert.Equal("PackedFromNuspec", nuspecReader.GetId());
                    Assert.Equal("1.2.1", nuspecReader.GetVersion().ToFullString());
                    Assert.Equal("Microsoft", nuspecReader.GetAuthors());
                    Assert.Equal("NuGet", nuspecReader.GetOwners());
                    Assert.Equal("This was packed from nuspec", nuspecReader.GetDescription());
                    Assert.False(nuspecReader.GetRequireLicenseAcceptance());
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        // Command line : /p:NuspecProperties=\"id=MyPackage;version=1.2.3;tags=tag1;description="hello world"\"
        [InlineData("/p:NuspecProperties=\\\"id=MyPackage;version=1.2.3;tags=tag1;description=\"hello world\"\\\"", "MyPackage",
            "1.2.3", "hello world", "tag1")]
        // Command line : /p:NuspecProperties=\"id=MyPackage;version=1.2.3;tasg=tag1,tag2;description=""\"
        [InlineData("/p:NuspecProperties=\\\"id=MyPackage;version=1.2.3;tags=tag1,tag2;description=\"hello world\"\\\"", "MyPackage",
            "1.2.3", "hello world", "tag1,tag2")]
        // Command line : /p:NuspecProperties=\"id=MyPackage;version=1.2.3;tags=;description="hello = world"\"
        [InlineData("/p:NuspecProperties=\\\"id=MyPackage;version=1.2.3;tags=;description=\"hello = world\"\\\"", "MyPackage",
            "1.2.3", "hello = world", "")]
        // Command line : /p:NuspecProperties=\"id=MyPackage;version=1.2.3;tags="";description="hello = world with a %3B"\"
        [InlineData("/p:NuspecProperties=\\\"id=MyPackage;version=1.2.3;tags=\"\";description=\"hello = world with a %3B\"\\\"",
            "MyPackage", "1.2.3", "hello = world with a ;", "")]
        public void PackCommand_PackProject_PacksFromNuspecWithTokenSubstitution(
            string nuspecProperties,
            string expectedId,
            string expectedVersion,
            string expectedDescription,
            string expectedTags
            )
        {
            var nuspecFileContent = @"<?xml version=""1.0""?>
<package>
  <metadata>
    <id>$id$</id>
    <version>$version$</version>
    <authors>Microsoft</authors>
    <owners>NuGet</owners>
    <tags>$tags$</tags>
    <description>$description$</description>
  </metadata>
  <files>
    <file src=""abc.txt"" target=""CoreCLR/"" />
  </files>
</package>";
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                File.WriteAllText(Path.Combine(workingDirectory, "input.nuspec"), nuspecFileContent);
                File.WriteAllText(Path.Combine(workingDirectory, "abc.txt"), "sample text");

                msbuildFixture.PackProject(workingDirectory, projectName,
                    $"-o {workingDirectory} /p:NuspecFile=input.nuspec " + nuspecProperties);

                var nupkgPath = Path.Combine(workingDirectory, $"{expectedId}.{expectedVersion}.nupkg");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the .nuspec.
                    Assert.Equal(expectedId, nuspecReader.GetId());
                    Assert.Equal(expectedVersion, nuspecReader.GetVersion().ToFullString());
                    Assert.Equal("Microsoft", nuspecReader.GetAuthors());
                    Assert.Equal("NuGet", nuspecReader.GetOwners());
                    Assert.Equal(expectedDescription, nuspecReader.GetDescription());
                    Assert.Equal(expectedTags, nuspecReader.GetTags());
                    Assert.False(nuspecReader.GetRequireLicenseAcceptance());

                    // Validate the assets.
                    var libItems = nupkgReader.GetFiles("CoreCLR").ToArray();
                    Assert.Equal("CoreCLR/abc.txt", libItems[0]);
                }

            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackProject_PacksFromNuspecWithBasePath()
        {
            var nuspecFileContent = @"<?xml version=""1.0""?>
<package>
  <metadata>
    <id>PackedFromNuspec</id>
    <version>1.2.1</version>
    <authors>Microsoft</authors>
    <owners>NuGet</owners>
    <description>This was packed from nuspec</description>
  </metadata>
  <files>
    <file src=""abc.txt"" target=""CoreCLR/"" />
  </files>
</package>";
            using (var basePathDirectory = msbuildFixture.CreateTestDirectory())
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                File.WriteAllText(Path.Combine(workingDirectory, "input.nuspec"), nuspecFileContent);
                File.WriteAllText(Path.Combine(basePathDirectory, "abc.txt"), "sample text");

                msbuildFixture.PackProject(workingDirectory, projectName,
                    $"-o {workingDirectory} /p:NuspecFile=input.nuspec /p:NuspecBasePath={basePathDirectory.Path}");

                var nupkgPath = Path.Combine(workingDirectory, $"PackedFromNuspec.1.2.1.nupkg");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    // Validate the assets.
                    var libItems = nupkgReader.GetFiles("CoreCLR").ToArray();
                    Assert.Equal("CoreCLR/abc.txt", libItems[0]);
                }

            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("abc.txt", null, "content/abc.txt;contentFiles/any/netstandard1.4/abc.txt")]
        [InlineData("folderA/abc.txt", null, "content/folderA/abc.txt;contentFiles/any/netstandard1.4/folderA/abc.txt")]
        [InlineData("folderA/folderB/abc.txt", null,
            "content/folderA/folderB/abc.txt;contentFiles/any/netstandard1.4/folderA/folderB/abc.txt")]
        [InlineData("../abc.txt", null, "content/abc.txt;contentFiles/any/netstandard1.4/abc.txt")]
        [InlineData("{AbsolutePath}/abc.txt", null, "content/abc.txt;contentFiles/any/netstandard1.4/abc.txt")]
        [InlineData("abc.txt", "folderA/", "folderA/abc.txt")]
        [InlineData("abc.txt", "folderA/xyz.txt", "folderA/xyz.txt")]
        [InlineData("abc.txt", "", "abc.txt")]
        [InlineData("abc.txt", "/", "abc.txt")]
        [InlineData("abc.txt", "folderA;folderB", "folderA/abc.txt;folderB/abc.txt")]
        [InlineData("abc.txt", "folderA;contentFiles", "folderA/abc.txt;contentFiles/abc.txt")]
        [InlineData("abc.txt", "folderA;contentFiles/", "folderA/abc.txt;contentFiles/abc.txt")]
        [InlineData("abc.txt", "folderA;contentFiles\\", "folderA/abc.txt;contentFiles/abc.txt")]
        [InlineData("abc.txt", "folderA;contentFiles/folderA", "folderA/abc.txt;contentFiles/folderA/abc.txt")]
        [InlineData("folderA/abc.txt", "folderA/", "folderA/abc.txt")]
        [InlineData("folderA/abc.txt", "", "abc.txt")]
        [InlineData("folderA/abc.txt", "/", "abc.txt")]
        [InlineData("folderA/abc.txt", "folderA;folderB", "folderA/abc.txt;folderB/abc.txt")]
        [InlineData("folderA/abc.txt", "folderA;contentFiles", "folderA/abc.txt;contentFiles/abc.txt")]
        [InlineData("folderA/abc.txt", "folderA;contentFiles\\", "folderA/abc.txt;contentFiles/abc.txt")]
        [InlineData("folderA/abc.txt", "folderA;contentFiles/", "folderA/abc.txt;contentFiles/abc.txt")]
        [InlineData("folderA/abc.txt", "folderA;contentFiles/folderA", "folderA/abc.txt;contentFiles/folderA/abc.txt")]
        [InlineData("folderA/abc.txt", "folderA/xyz.txt", "folderA/xyz.txt")]
        [InlineData("{AbsolutePath}/abc.txt", "folderA/", "folderA/abc.txt")]
        [InlineData("{AbsolutePath}/abc.txt", "folderA/xyz.txt", "folderA/xyz.txt")]
        [InlineData("{AbsolutePath}/abc.txt", "", "abc.txt")]
        [InlineData("{AbsolutePath}/abc.txt", "/", "abc.txt")]
        [InlineData("{AbsolutePath}/abc.txt", "folderA;folderB", "folderA/abc.txt;folderB/abc.txt")]
        [InlineData("{AbsolutePath}/abc.txt", "folderA;contentFiles", "folderA/abc.txt;contentFiles/abc.txt")]
        [InlineData("{AbsolutePath}/abc.txt", "folderA;contentFiles\\", "folderA/abc.txt;contentFiles/abc.txt")]
        [InlineData("{AbsolutePath}/abc.txt", "folderA;contentFiles/", "folderA/abc.txt;contentFiles/abc.txt")]
        [InlineData("{AbsolutePath}/abc.txt", "folderA;contentFiles/folderA", "folderA/abc.txt;contentFiles/folderA/abc.txt")]
        [InlineData("../abc.txt", "folderA/", "folderA/abc.txt")]
        [InlineData("../abc.txt", "folderA/xyz.txt", "folderA/xyz.txt")]
        [InlineData("../abc.txt", "", "abc.txt")]
        [InlineData("../abc.txt", "/", "abc.txt")]
        [InlineData("../abc.txt", "folderA;folderB", "folderA/abc.txt;folderB/abc.txt")]
        [InlineData("../abc.txt", "folderA;contentFiles", "folderA/abc.txt;contentFiles/abc.txt")]
        [InlineData("../abc.txt", "folderA;contentFiles/", "folderA/abc.txt;contentFiles/abc.txt")]
        [InlineData("../abc.txt", "folderA;contentFiles\\", "folderA/abc.txt;contentFiles/abc.txt")]
        [InlineData("../abc.txt", "folderA;contentFiles/folderA", "folderA/abc.txt;contentFiles/folderA/abc.txt")]
        // ## is a special syntax specifically for this test which means that ## should be replaced by the absolute path to the project directory.
        [InlineData("##/abc.txt", null, "content/abc.txt;contentFiles/any/netstandard1.4/abc.txt")]
        [InlineData("##/folderA/abc.txt", null,
            "content/folderA/abc.txt;contentFiles/any/netstandard1.4/folderA/abc.txt")]
        [InlineData("##/../abc.txt", null, "content/abc.txt")]
        [InlineData("##/abc.txt", "", "abc.txt")]
        [InlineData("##/abc.txt", "folderX;folderY", "folderX/abc.txt;folderY/abc.txt")]
        [InlineData("##/folderA/abc.txt", "folderX;folderY", "folderX/abc.txt;folderY/abc.txt")]
        [InlineData("##/../abc.txt", "folderX;folderY", "folderX/abc.txt;folderY/abc.txt")]

        public void PackCommand_PackProject_PackagePathPacksContentCorrectly(string sourcePath, string packagePath,
            string expectedTargetPaths)
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {

                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);

                if (sourcePath.StartsWith("##"))
                {
                    sourcePath = sourcePath.Replace("##", workingDirectory);
                }
                else if (sourcePath.StartsWith("{AbsolutePath}"))
                {
                    sourcePath = sourcePath.Replace("{AbsolutePath}", Path.GetTempPath().Replace('\\', '/'));
                }

                // Create the subdirectory structure for testing possible source paths for the content file
                Directory.CreateDirectory(Path.Combine(workingDirectory, "folderA"));
                Directory.CreateDirectory(Path.Combine(workingDirectory, "folderA", "folderB"));
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFramework", "netstandard1.4");

                    var attributes = new Dictionary<string, string>();

                    var properties = new Dictionary<string, string>();
                    if (packagePath != null)
                    {
                        properties["PackagePath"] = packagePath;
                    }
                    ProjectFileUtils.AddItem(
                        xml,
                        "Content",
                        sourcePath,
                        string.Empty,
                        properties,
                        attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                var pathToContent = string.Empty;
                if (Path.IsPathRooted(sourcePath))
                {
                    pathToContent = sourcePath;
                }
                else
                {
                    pathToContent = Path.Combine(workingDirectory, sourcePath);
                }

                File.WriteAllText(pathToContent, "this is sample text in the content file");

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var items = new HashSet<string>(nupkgReader.GetFiles());
                    var expectedPaths = expectedTargetPaths.Split(';');
                    foreach (var path in expectedPaths)
                    {
                        Assert.Contains(path, items);
                    }
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData(null, null, null, "1.0.0")]
        [InlineData("1.2.3", null, null, "1.2.3")]
        [InlineData(null, "rtm-1234", null, "1.0.0-rtm-1234")]
        [InlineData("1.2.3", "rtm-1234", null, "1.2.3-rtm-1234")]
        [InlineData(null, null, "2.3.1", "2.3.1")]
        [InlineData("1.2.3", null, "2.3.1", "2.3.1")]
        [InlineData(null, "rtm-1234", "2.3.1", "2.3.1")]
        [InlineData("1.2.3", "rtm-1234", "2.3.1", "2.3.1")]
        public void PackCommand_PackProject_OutputsCorrectVersion(string versionPrefix, string versionSuffix,
            string packageVersion, string expectedVersion)
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFramework", "netstandard1.4");

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                var args = "" +
                           (versionPrefix != null ? $" /p:VersionPrefix={versionPrefix} " : string.Empty) +
                           (versionSuffix != null ? $" /p:VersionSuffix={versionSuffix} " : string.Empty) +
                           (packageVersion != null ? $" /p:PackageVersion={packageVersion} " : string.Empty);
                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory} {args}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.{expectedVersion}.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.{expectedVersion}.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                // Assert
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var intermediateNuspec = new NuspecReader(nuspecPath);
                    var nuspecReader = nupkgReader.NuspecReader;
                    Assert.Equal(expectedVersion, nuspecReader.GetVersion().ToFullString());
                    Assert.Equal(expectedVersion, intermediateNuspec.GetVersion().ToFullString());
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("abc.txt", null, "any/netstandard1.4/abc.txt")]
        [InlineData("folderA/abc.txt", null, "any/netstandard1.4/folderA/abc.txt")]
        [InlineData("folderA/folderB/abc.txt", null, "any/netstandard1.4/folderA/folderB/abc.txt")]
        [InlineData("../abc.txt", null, "any/netstandard1.4/abc.txt")]
        [InlineData("##/abc.txt", null, "any/netstandard1.4/abc.txt")]
        [InlineData("##/folderA/abc.txt", null, "any/netstandard1.4/folderA/abc.txt")]
        [InlineData("##/../abc.txt", null, "any/netstandard1.4/abc.txt")]
        [InlineData("abc.txt", "contentFiles", "abc.txt")]
        [InlineData("folderA/abc.txt", "contentFiles", "abc.txt")]
        [InlineData("folderA/folderB/abc.txt", "contentFiles", "abc.txt")]
        [InlineData("../abc.txt", "contentFiles", "abc.txt")]
        [InlineData("##/abc.txt", "contentFiles", "abc.txt")]
        [InlineData("##/folderA/abc.txt", "contentFiles", "abc.txt")]
        [InlineData("##/../abc.txt", "contentFiles", "abc.txt")]
        [InlineData("abc.txt", "contentFiles\\", "abc.txt")]
        [InlineData("folderA/abc.txt", "contentFiles\\", "abc.txt")]
        [InlineData("folderA/folderB/abc.txt", "contentFiles\\", "abc.txt")]
        [InlineData("../abc.txt", "contentFiles\\", "abc.txt")]
        [InlineData("##/abc.txt", "contentFiles\\", "abc.txt")]
        [InlineData("##/folderA/abc.txt", "contentFiles\\", "abc.txt")]
        [InlineData("##/../abc.txt", "contentFiles\\", "abc.txt")]
        [InlineData("folderA/abc.txt", "contentFiles/", "abc.txt")]
        [InlineData("folderA/folderB/abc.txt", "contentFiles/", "abc.txt")]
        [InlineData("../abc.txt", "contentFiles/", "abc.txt")]
        [InlineData("##/abc.txt", "contentFiles/", "abc.txt")]
        [InlineData("##/folderA/abc.txt", "contentFiles/", "abc.txt")]
        [InlineData("##/../abc.txt", "contentFiles/", "abc.txt")]
        [InlineData("folderA/abc.txt", "contentFiles/xyz.txt", "xyz.txt")]
        [InlineData("folderA/folderB/abc.txt", "contentFiles/xyz.txt", "xyz.txt")]
        [InlineData("../abc.txt", "contentFiles/xyz.txt", "xyz.txt")]
        [InlineData("##/abc.txt", "contentFiles/xyz.txt", "xyz.txt")]
        [InlineData("##/folderA/abc.txt", "contentFiles/xyz.txt", "xyz.txt")]
        [InlineData("##/../abc.txt", "contentFiles/xyz.txt", "xyz.txt")]
        [InlineData("abc.txt", "folderA", null)]
        [InlineData("folderA/abc.txt", "folderA", null)]
        [InlineData("folderA/folderB/abc.txt", "folderA", null)]
        [InlineData("../abc.txt", "folderA", null)]
        [InlineData("##/abc.txt", "folderA", null)]
        [InlineData("##/folderA/abc.txt", "folderA", null)]
        [InlineData("##/../abc.txt", "folderA", null)]
        [InlineData("abc.txt", "contentFiles/folderA", "folderA/abc.txt")]
        [InlineData("folderA/abc.txt", "contentFiles/folderA", "folderA/abc.txt")]
        [InlineData("folderA/folderB/abc.txt", "contentFiles/folderA", "folderA/abc.txt")]
        [InlineData("../abc.txt", "contentFiles/folderA", "folderA/abc.txt")]
        [InlineData("##/abc.txt", "contentFiles/folderA", "folderA/abc.txt")]
        [InlineData("##/folderA/abc.txt", "contentFiles/folderA", "folderA/abc.txt")]
        [InlineData("##/../abc.txt", "contentFiles/folderA", "folderA/abc.txt")]
        public void PackCommand_PackProject_OutputsContentFilesInNuspecForSingleFramework(string sourcePath,
            string packagePath, string expectedIncludeString)
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {

                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);

                if (sourcePath.StartsWith("##"))
                {
                    sourcePath = sourcePath.Replace("##", workingDirectory);
                }

                // Create the subdirectory structure for testing possible source paths for the content file
                Directory.CreateDirectory(Path.Combine(workingDirectory, "folderA"));
                Directory.CreateDirectory(Path.Combine(workingDirectory, "folderA", "folderB"));

                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                var pathToContent = Path.Combine(workingDirectory, sourcePath);
                if (Path.IsPathRooted(sourcePath))
                {
                    pathToContent = sourcePath;
                }
                File.WriteAllText(pathToContent, "this is sample text in the content file");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFramework", "netstandard1.4");

                    var attributes = new Dictionary<string, string>();

                    var properties = new Dictionary<string, string>();
                    if (packagePath != null)
                    {
                        properties["PackagePath"] = packagePath;
                    }
                    ProjectFileUtils.AddItem(
                        xml,
                        "Content",
                        sourcePath,
                        string.Empty,
                        properties,
                        attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;
                    var contentFiles = nuspecReader.GetContentFiles().ToArray();

                    if (expectedIncludeString == null)
                    {
                        Assert.True(contentFiles.Count() == 0);
                    }
                    else
                    {
                        Assert.True(contentFiles.Count() == 1);
                        var contentFile = contentFiles[0];
                        Assert.Equal(expectedIncludeString, contentFile.Include);
                        Assert.Equal("Content", contentFile.BuildAction);

                        var files = nupkgReader.GetFiles("contentFiles");
                        Assert.Contains("contentFiles/" + expectedIncludeString, files);
                    }
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("abc.txt", "any/net45/abc.txt;any/netstandard1.3/abc.txt")]
        [InlineData("folderA/abc.txt", "any/net45/folderA/abc.txt;any/netstandard1.3/folderA/abc.txt")]
        [InlineData("folderA/folderB/abc.txt",
            "any/net45/folderA/folderB/abc.txt;any/netstandard1.3/folderA/folderB/abc.txt")]
        [InlineData("../abc.txt", "any/net45/abc.txt;any/netstandard1.3/abc.txt")]
        [InlineData("##/abc.txt", "any/net45/abc.txt;any/netstandard1.3/abc.txt")]
        [InlineData("##/folderA/abc.txt", "any/net45/folderA/abc.txt;any/netstandard1.3/folderA/abc.txt")]
        [InlineData("##/../abc.txt", "any/net45/abc.txt;any/netstandard1.3/abc.txt")]
        public void PackCommand_PackProject_OutputsContentFilesInNuspecForMultipleFrameworks(string sourcePath,
            string expectedIncludeString)
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {

                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);

                if (sourcePath.StartsWith("##"))
                {
                    sourcePath = sourcePath.Replace("##", workingDirectory);
                }

                // Create the subdirectory structure for testing possible source paths for the content file
                Directory.CreateDirectory(Path.Combine(workingDirectory, "folderA"));
                Directory.CreateDirectory(Path.Combine(workingDirectory, "folderA", "folderB"));

                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                var pathToContent = Path.Combine(workingDirectory, sourcePath);
                if (Path.IsPathRooted(sourcePath))
                {
                    pathToContent = sourcePath;
                }
                File.WriteAllText(pathToContent, "this is sample text in the content file");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFrameworks", "net45;netstandard1.3");

                    var attributes = new Dictionary<string, string>();
                    var properties = new Dictionary<string, string>();

                    ProjectFileUtils.AddItem(
                        xml,
                        "Content",
                        sourcePath,
                        string.Empty,
                        properties,
                        attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;
                    var contentFiles = nuspecReader.GetContentFiles().ToArray();

                    if (expectedIncludeString == null)
                    {
                        Assert.True(contentFiles.Count() == 0);
                    }
                    else
                    {
                        var expectedStrings = expectedIncludeString.Split(';');
                        Assert.True(contentFiles.Count() == 2);
                        var contentFileSet = contentFiles.Select(p => p.Include);
                        var files = nupkgReader.GetFiles("contentFiles");
                        foreach (var expected in expectedStrings)
                        {
                            Assert.Contains(expected, contentFileSet);
                            Assert.Contains("contentFiles/" + expected, files);
                        }
                    }
                }
            }
        }

#if NETCOREAPP5_0
        [PlatformFact(Platform.Windows)]
        public void PackCommand_SingleFramework_GeneratesPackageOnBuildUsingNet5()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFramework", "net5.0");
                    ProjectFileUtils.AddProperty(xml, "GeneratePackageOnBuild", "true");
                    ProjectFileUtils.AddProperty(xml, "NuspecOutputPath", "obj\\Debug");

                    var attributes = new Dictionary<string, string>();

                    attributes["Version"] = "9.0.1";
                    ProjectFileUtils.AddItem(
                        xml,
                        "PackageReference",
                        "Newtonsoft.json",
                        "",
                        new Dictionary<string, string>(),
                        attributes);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.BuildProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", "Debug", $"{projectName}.1.0.0.nuspec");

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the output .nuspec.
                    Assert.Equal("ClassLibrary1", nuspecReader.GetId());
                    Assert.Equal("1.0.0", nuspecReader.GetVersion().ToFullString());
                    Assert.Equal("ClassLibrary1", nuspecReader.GetAuthors());
                    Assert.Equal("", nuspecReader.GetOwners());
                    Assert.Equal("Package Description", nuspecReader.GetDescription());
                    Assert.False(nuspecReader.GetRequireLicenseAcceptance());

                    var dependencyGroups = nuspecReader.GetDependencyGroups().ToList();
                    Assert.Equal(1, dependencyGroups.Count);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.Net50, dependencyGroups[0].TargetFramework);
                    var packages = dependencyGroups[0].Packages.ToList();
                    Assert.Equal(1, packages.Count);
                    Assert.Equal("Newtonsoft.json", packages[0].Id);
                    Assert.Equal(new VersionRange(new NuGetVersion("9.0.1")), packages[0].VersionRange);
                    Assert.Equal(new List<string> { "Analyzers", "Build" }, packages[0].Exclude);
                    Assert.Empty(packages[0].Include);

                    // Validate the assets.
                    var libItems = nupkgReader.GetLibItems().ToList();
                    Assert.Equal(1, libItems.Count);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.Net50, libItems[0].TargetFramework);
                    Assert.Equal(new[] { "lib/net5.0/ClassLibrary1.dll" }, libItems[0].Items);
                }

            }
        }
#endif

        [PlatformFact(Platform.Windows)]
        public void PackCommand_SingleFramework_GeneratesPackageOnBuild()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFramework", "netstandard1.4");
                    ProjectFileUtils.AddProperty(xml, "GeneratePackageOnBuild", "true");
                    ProjectFileUtils.AddProperty(xml, "NuspecOutputPath", "obj\\Debug");

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.BuildProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", "Debug", $"{projectName}.1.0.0.nuspec");

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the output .nuspec.
                    Assert.Equal("ClassLibrary1", nuspecReader.GetId());
                    Assert.Equal("1.0.0", nuspecReader.GetVersion().ToFullString());
                    Assert.Equal("ClassLibrary1", nuspecReader.GetAuthors());
                    Assert.Equal("", nuspecReader.GetOwners());
                    Assert.Equal("Package Description", nuspecReader.GetDescription());
                    Assert.False(nuspecReader.GetRequireLicenseAcceptance());

                    var dependencyGroups = nuspecReader.GetDependencyGroups().ToList();
                    Assert.Equal(1, dependencyGroups.Count);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard14, dependencyGroups[0].TargetFramework);
                    var packages = dependencyGroups[0].Packages.ToList();
                    Assert.Equal(1, packages.Count);
                    Assert.Equal("NETStandard.Library", packages[0].Id);
                    Assert.Equal(new VersionRange(new NuGetVersion("1.6.1")), packages[0].VersionRange);
                    Assert.Equal(new List<string> { "Analyzers", "Build" }, packages[0].Exclude);
                    Assert.Empty(packages[0].Include);

                    // Validate the assets.
                    var libItems = nupkgReader.GetLibItems().ToList();
                    Assert.Equal(1, libItems.Count);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard14, libItems[0].TargetFramework);
                    Assert.Equal(new[] { "lib/netstandard1.4/ClassLibrary1.dll" }, libItems[0].Items);
                }

            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("netstandard1.4")]
        [InlineData("netstandard1.4;net451")]
        [InlineData("netstandard1.4;net451;netcoreapp1.0")]
        public void PackCommand_MultipleFrameworks_GeneratesPackageOnBuild(string frameworks)
        {
            using (SimpleTestPathContext pathContext = msbuildFixture.CreateSimpleTestPathContext())
            {
                SimpleTestSettingsContext settings = pathContext.Settings;
                settings.AddNetStandardFeeds();

                string testDirectory = pathContext.WorkingDirectory;
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFrameworks", frameworks);
                    ProjectFileUtils.AddProperty(xml, "GeneratePackageOnBuild", "true");
                    ProjectFileUtils.AddProperty(xml, "NuspecOutputPath", "obj\\Debug");
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.BuildProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", "Debug", $"{projectName}.1.0.0.nuspec");

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");


                var frameworksArray = frameworks.Split(';');
                var count = frameworksArray.Length;

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the output .nuspec.
                    Assert.Equal("ClassLibrary1", nuspecReader.GetId());
                    Assert.Equal("1.0.0", nuspecReader.GetVersion().ToFullString());
                    Assert.Equal("ClassLibrary1", nuspecReader.GetAuthors());
                    Assert.Equal("", nuspecReader.GetOwners());
                    Assert.Equal("Package Description", nuspecReader.GetDescription());
                    Assert.False(nuspecReader.GetRequireLicenseAcceptance());

                    var dependencyGroups = nuspecReader.GetDependencyGroups().ToList();
                    Assert.Equal(count, dependencyGroups.Count);

                    // Validate the assets.
                    var libItems = nupkgReader.GetFiles("lib").ToList();
                    Assert.Equal(count, libItems.Count());

                    foreach (var framework in frameworksArray)
                    {
                        Assert.Contains($"lib/{framework}/ClassLibrary1.dll", libItems);
                    }
                }
            }
        }

        // This test checks to see that when IncludeBuildOutput=false, the generated nupkg does not contain lib folder
        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackNewDefaultProject_IncludeBuildOutputDoesNotCreateLibFolder()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib -f netstandard2.0");
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                using (var stream = new FileStream(Path.Combine(workingDirectory, $"{projectName}.csproj"), FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);

                    var attributes = new Dictionary<string, string>();

                    attributes["Version"] = "9.0.1";
                    ProjectFileUtils.AddItem(
                        xml,
                        "PackageReference",
                        "Newtonsoft.Json",
                        string.Empty,
                        new Dictionary<string, string>(),
                        attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.PackProject(workingDirectory, projectName,
                    $"-o {workingDirectory} /p:IncludeBuildOutput=false");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the output .nuspec.
                    Assert.Equal("ClassLibrary1", nuspecReader.GetId());
                    Assert.Equal("1.0.0", nuspecReader.GetVersion().ToFullString());
                    Assert.Equal("ClassLibrary1", nuspecReader.GetAuthors());
                    Assert.Equal("", nuspecReader.GetOwners());
                    Assert.Equal("Package Description", nuspecReader.GetDescription());
                    Assert.False(nuspecReader.GetRequireLicenseAcceptance());

                    var dependencyGroups = nuspecReader.GetDependencyGroups().ToList();
                    Assert.Equal(1, dependencyGroups.Count);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard20, dependencyGroups[0].TargetFramework);
                    var packages = dependencyGroups[0].Packages.ToList();
                    Assert.Equal(1, packages.Count);
                    Assert.Equal("Newtonsoft.Json", packages[0].Id);

                    // Validate the assets.
                    var libItems = nupkgReader.GetLibItems().ToList();
                    Assert.Equal(0, libItems.Count);
                }
            }
        }

        // This test checks to see that when BuildOutputTargetFolder is specified, the generated nupkg has the DLLs in the specified output folder
        // instead of the default lib folder.
        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackNewDefaultProject_BuildOutputTargetFolderOutputsLibsToRightFolder()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var buildOutputTargetFolder = "build";
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib -f netstandard2.0");
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName,
                    $"-o {workingDirectory} /p:BuildOutputTargetFolder={buildOutputTargetFolder}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    // Validate the assets.
                    var libItems = nupkgReader.GetLibItems().ToList();
                    Assert.Equal(0, libItems.Count);
                    libItems = nupkgReader.GetItems(buildOutputTargetFolder).ToList();
                    Assert.Equal(1, libItems.Count);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard20, libItems[0].TargetFramework);
                    Assert.Equal(new[] { $"{buildOutputTargetFolder}/netstandard2.0/ClassLibrary1.dll" },
                        libItems[0].Items);
                }
            }
        }

        // This test checks to see that when GeneratePackageOnBuild is set to true, the generated nupkg and the intermediate
        // nuspec is deleted when the clean target is invoked.
        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackNewProject_CleanDeletesNupkgAndNuspec()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "GeneratePackageOnBuild", "true");
                    ProjectFileUtils.AddProperty(xml, "NuspecOutputPath", "obj");

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.BuildProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory} ");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                // Run the clean target
                msbuildFixture.BuildProject(workingDirectory, projectName,
                    $"/t:Clean /p:PackageOutputPath={workingDirectory}\\");

                Assert.True(!File.Exists(nupkgPath), "The output .nupkg was not deleted by the Clean target");
                Assert.True(!File.Exists(nuspecPath), "The intermediate nuspec file was not deleted by the Clean target");
            }
        }

        // This test checks to see that when GeneratePackageOnBuild is set to true, the generated nupkg and the intermediate
        // nuspec is deleted when the clean target is invoked and no other nupkg or nuspec file in the PackageOutputPath.
        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackNewProject_CleanDeletesOnlyGeneratedNupkgAndNuspec()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "GeneratePackageOnBuild", "true");
                    ProjectFileUtils.AddProperty(xml, "NuspecOutputPath", "obj");

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.BuildProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory} ");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                var extraNupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.2.0.nupkg");
                var extraNuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.2.0.nuspec");
                File.WriteAllBytes(extraNupkgPath, new byte[1024]);
                File.WriteAllText(extraNuspecPath, "hello world");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                // Run the clean target
                msbuildFixture.BuildProject(workingDirectory, projectName,
                    $"/t:Clean /p:PackageOutputPath={workingDirectory}\\");

                Assert.True(!File.Exists(nupkgPath), "The output .nupkg was not deleted by the Clean target");
                Assert.True(!File.Exists(nuspecPath), "The intermediate nuspec file was not deleted by the Clean target");
                Assert.True(File.Exists(extraNuspecPath), "All nuspec files were deleted by the clean target");
                Assert.True(File.Exists(extraNupkgPath), "All nupkg files were deleted by the clean target");
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("abc.txt", null, "content/abc.txt;contentFiles/any/netstandard1.4/abc.txt")]
        [InlineData("folderA/abc.txt", null, "content/folderA/abc.txt;contentFiles/any/netstandard1.4/folderA/abc.txt")]
        [InlineData("folderA/folderB/abc.txt", null, "content/folderA/folderB/abc.txt;contentFiles/any/netstandard1.4/folderA/folderB/abc.txt")]
        [InlineData("../abc.txt", null, "content/abc.txt;contentFiles/any/netstandard1.4/abc.txt")]
        [InlineData("{AbsolutePath}/abc.txt", null, "content/abc.txt;contentFiles/any/netstandard1.4/abc.txt")]
        [InlineData("abc.txt", "folderA/", "folderA/abc.txt")]
        [InlineData("abc.txt", "folderA/xyz.txt", "folderA/xyz.txt/abc.txt")]
        [InlineData("abc.txt", "folderA;folderB", "folderA/abc.txt;folderB/abc.txt")]
        [InlineData("abc.txt", "folderA;contentFiles", "folderA/abc.txt;contentFiles/any/netstandard1.4/abc.txt")]
        [InlineData("abc.txt", "folderA;contentFiles/", "folderA/abc.txt;contentFiles/any/netstandard1.4/abc.txt")]
        [InlineData("abc.txt", "folderA;contentFiles\\", "folderA/abc.txt;contentFiles/any/netstandard1.4/abc.txt")]
        [InlineData("abc.txt", "folderA;contentFiles/folderA", "folderA/abc.txt;contentFiles/folderA/abc.txt")]
        [InlineData("folderA/abc.txt", "folderA/", "folderA/folderA/abc.txt")]
        [InlineData("folderA/abc.txt", "folderA;folderB", "folderA/folderA/abc.txt;folderB/folderA/abc.txt")]
        [InlineData("folderA/abc.txt", "folderA;contentFiles", "folderA/folderA/abc.txt;contentFiles/any/netstandard1.4/folderA/abc.txt")]
        [InlineData("folderA/abc.txt", "folderA;contentFiles\\", "folderA/folderA/abc.txt;contentFiles/any/netstandard1.4/folderA/abc.txt")]
        [InlineData("folderA/abc.txt", "folderA;contentFiles/", "folderA/folderA/abc.txt;contentFiles/any/netstandard1.4/folderA/abc.txt")]
        [InlineData("folderA/abc.txt", "folderA;contentFiles/folderA", "folderA/folderA/abc.txt;contentFiles/folderA/folderA/abc.txt")]
        [InlineData("folderA/abc.txt", "folderA/xyz.txt", "folderA/xyz.txt/folderA/abc.txt")]
        [InlineData("{AbsolutePath}/abc.txt", "folderA/", "folderA/abc.txt")]
        [InlineData("{AbsolutePath}/abc.txt", "folderA/xyz.txt", "folderA/xyz.txt/abc.txt")]
        [InlineData("{AbsolutePath}/abc.txt", "folderA;folderB", "folderA/abc.txt;folderB/abc.txt")]
        [InlineData("{AbsolutePath}/abc.txt", "folderA;contentFiles", "folderA/abc.txt;contentFiles/any/netstandard1.4/abc.txt")]
        [InlineData("{AbsolutePath}/abc.txt", "folderA;contentFiles\\", "folderA/abc.txt;contentFiles/any/netstandard1.4/abc.txt")]
        [InlineData("{AbsolutePath}/abc.txt", "folderA;contentFiles/", "folderA/abc.txt;contentFiles/any/netstandard1.4/abc.txt")]
        [InlineData("{AbsolutePath}/abc.txt", "folderA;contentFiles/folderA", "folderA/abc.txt;contentFiles/folderA/abc.txt")]
        [InlineData("../abc.txt", "folderA/", "folderA/abc.txt")]
        [InlineData("../abc.txt", "folderA/xyz.txt", "folderA/xyz.txt/abc.txt")]
        [InlineData("../abc.txt", "folderA;folderB", "folderA/abc.txt;folderB/abc.txt")]
        [InlineData("../abc.txt", "folderA;contentFiles", "folderA/abc.txt;contentFiles/any/netstandard1.4/abc.txt")]
        [InlineData("../abc.txt", "folderA;contentFiles/", "folderA/abc.txt;contentFiles/any/netstandard1.4/abc.txt")]
        [InlineData("../abc.txt", "folderA;contentFiles\\", "folderA/abc.txt;contentFiles/any/netstandard1.4/abc.txt")]
        [InlineData("../abc.txt", "folderA;contentFiles/folderA", "folderA/abc.txt;contentFiles/folderA/abc.txt")]
        // ## is a special syntax specifically for this test which means that ## should be replaced by the absolute path to the project directory.
        [InlineData("##/abc.txt", null, "content/abc.txt;contentFiles/any/netstandard1.4/abc.txt")]
        [InlineData("##/folderA/abc.txt", null, "content/folderA/abc.txt;contentFiles/any/netstandard1.4/folderA/abc.txt")]
        [InlineData("##/../abc.txt", null, "content/abc.txt;contentFiles/any/netstandard1.4/abc.txt")]
        [InlineData("##/abc.txt", "folderX;folderY", "folderX/abc.txt;folderY/abc.txt")]
        [InlineData("##/folderA/abc.txt", "folderX;folderY", "folderX/folderA/abc.txt;folderY/folderA/abc.txt")]
        [InlineData("##/../abc.txt", "folderX;folderY", "folderX/abc.txt;folderY/abc.txt")]

        public void PackCommand_PackProject_ContentTargetFoldersPacksContentCorrectly(string sourcePath,
            string contentTargetFolders, string expectedTargetPaths)
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {

                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);

                if (sourcePath.StartsWith("##"))
                {
                    sourcePath = sourcePath.Replace("##", workingDirectory);
                }
                else if (sourcePath.StartsWith("{AbsolutePath}"))
                {
                    sourcePath = sourcePath.Replace("{AbsolutePath}", Path.GetTempPath().Replace('\\', '/'));
                }

                // Create the subdirectory structure for testing possible source paths for the content file
                Directory.CreateDirectory(Path.Combine(workingDirectory, "folderA"));
                Directory.CreateDirectory(Path.Combine(workingDirectory, "folderA", "folderB"));
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFramework", "netstandard1.4");
                    ProjectFileUtils.AddProperty(xml, "ContentTargetFolders", contentTargetFolders);

                    var attributes = new Dictionary<string, string>();

                    var properties = new Dictionary<string, string>();
                    ProjectFileUtils.AddItem(
                        xml,
                        "Content",
                        sourcePath,
                        string.Empty,
                        properties,
                        attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                var pathToContent = string.Empty;
                if (Path.IsPathRooted(sourcePath))
                {
                    pathToContent = sourcePath;
                }
                else
                {
                    pathToContent = Path.Combine(workingDirectory, sourcePath);
                }
                File.WriteAllText(pathToContent, "this is sample text in the content file");

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var items = new HashSet<string>(nupkgReader.GetFiles());
                    var expectedPaths = expectedTargetPaths.Split(';');
                    foreach (var path in expectedPaths)
                    {
                        Assert.Contains(path, items);
                    }
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_NewProject_AddsTitleToNuspec()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFramework", "netstandard1.4");
                    ProjectFileUtils.AddProperty(xml, "Title", "MyPackageTitle");

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;
                    var intermediateNuspec = new NuspecReader(nuspecPath);

                    // Validate the output .nuspec.
                    Assert.Equal("ClassLibrary1", nuspecReader.GetId());
                    Assert.Equal("1.0.0", nuspecReader.GetVersion().ToFullString());
                    Assert.Equal("MyPackageTitle", nuspecReader.GetTitle());
                    Assert.Equal("ClassLibrary1", nuspecReader.GetAuthors());
                    Assert.Equal("", nuspecReader.GetOwners());
                    Assert.Equal("Package Description", nuspecReader.GetDescription());
                    Assert.False(nuspecReader.GetRequireLicenseAcceptance());

                    var dependencyGroups = nuspecReader.GetDependencyGroups().ToList();
                    Assert.Equal(1, dependencyGroups.Count);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard14, dependencyGroups[0].TargetFramework);
                    var packages = dependencyGroups[0].Packages.ToList();
                    Assert.Equal(1, packages.Count);
                    Assert.Equal("NETStandard.Library", packages[0].Id);
                    Assert.Equal(new VersionRange(new NuGetVersion("1.6.1")), packages[0].VersionRange);
                    Assert.Equal(new List<string> { "Analyzers", "Build" }, packages[0].Exclude);
                    Assert.Empty(packages[0].Include);

                    // Validate title property in intermediate nuspec
                    Assert.Equal("MyPackageTitle", intermediateNuspec.GetTitle());
                }

            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("TargetFramework", "netstandard1.4")]
        [InlineData("TargetFrameworks", "netstandard1.4;net46")]
        public void PackCommand_IncludeSource_AddsSourceFiles(string tfmProperty, string tfmValue)
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var utilitySrcFileContent = @"using System;
namespace ClassLibrary
{
    public class UtilityMethods
    {
    }
}";
                var extensionSrcFileContent = @"using System;
namespace ClassLibrary
{
    public class ExtensionMethods
    {
    }
}";
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                Directory.CreateDirectory(Path.Combine(workingDirectory, "Utils"));
                Directory.CreateDirectory(Path.Combine(workingDirectory, "Extensions"));
                File.WriteAllText(Path.Combine(workingDirectory, "Utils", "Utility.cs"), utilitySrcFileContent);
                File.WriteAllText(Path.Combine(workingDirectory, "Extensions", "ExtensionMethods.cs"),
                    extensionSrcFileContent);

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, tfmProperty, tfmValue);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName,
                    $"--include-source /p:PackageOutputPath={workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var symbolsNupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.symbols.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(symbolsNupkgPath), "The output symbols nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(symbolsNupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;
                    // Validate the assets.
                    var srcItems = nupkgReader.GetFiles("src").ToArray();
                    Assert.True(srcItems.Length == 4);
                    Assert.Contains("src/ClassLibrary1/ClassLibrary1.csproj", srcItems);
                    Assert.Contains("src/ClassLibrary1/Class1.cs", srcItems);
                    Assert.Contains("src/ClassLibrary1/Extensions/ExtensionMethods.cs", srcItems);
                    Assert.Contains("src/ClassLibrary1/Utils/Utility.cs", srcItems);
                }

            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("TargetFramework", "netstandard1.4")]
        [InlineData("TargetFrameworks", "netstandard1.4;net46")]
        public void PackCommand_ContentInnerTargetExtension_AddsTfmSpecificContent(string tfmProperty, string tfmValue)
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                Directory.CreateDirectory(Path.Combine(workingDirectory, "Extensions", "cs"));
                File.WriteAllText(Path.Combine(workingDirectory, "abc.txt"), "hello world");
                File.WriteAllText(Path.Combine(workingDirectory, "Extensions", "ext.txt"), "hello world again");
                File.WriteAllText(Path.Combine(workingDirectory, "Extensions", "cs", "ext.cs.txt"), "hello world again");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    var target = @"<Target Name=""CustomContentTarget"">
    <ItemGroup>
      <TfmSpecificPackageFile Include=""abc.txt"">
        <PackagePath>mycontent/$(TargetFramework)</PackagePath>
      </TfmSpecificPackageFile>
      <TfmSpecificPackageFile Include=""Extensions/ext.txt"" Condition=""'$(TargetFramework)' == 'net46'"">
        <PackagePath>net46content</PackagePath>
      </TfmSpecificPackageFile>
      <TfmSpecificPackageFile Include=""Extensions/**/ext.cs.txt"" Condition=""'$(TargetFramework)' == 'net46'"">
        <PackagePath>net46content</PackagePath>
      </TfmSpecificPackageFile>
    </ItemGroup>
  </Target>";
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, tfmProperty, tfmValue);
                    ProjectFileUtils.AddProperty(xml, "TargetsForTfmSpecificContentInPackage", "CustomContentTarget");
                    ProjectFileUtils.AddCustomXmlToProjectRoot(xml, target);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var files = nupkgReader.GetFiles("mycontent");
                    var tfms = tfmValue.Split(';');
                    Assert.Equal(tfms.Length, files.Count());

                    foreach (var tfm in tfms)
                    {
                        Assert.Contains($"mycontent/{tfm}/abc.txt", files);
                        var net46files = nupkgReader.GetFiles("net46content");
                        if (tfms.Length > 1)
                        {
                            Assert.Equal(2, net46files.Count());
                            Assert.Contains("net46content/ext.txt", net46files);
                            Assert.Contains("net46content/cs/ext.cs.txt", net46files);
                        }
                        else
                        {
                            Assert.Equal(0, net46files.Count());
                        }
                    }
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("TargetFramework", "netstandard1.4")]
        [InlineData("TargetFrameworks", "netstandard1.4;net46")]
        public void PackCommand_ContentInnerTargetExtension_AddsExtraSymbolFiles(string tfmProperty, string tfmValue)
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                Directory.CreateDirectory(workingDirectory);
                File.WriteAllText(Path.Combine(workingDirectory, "abc.pdb"), "hello world");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    var target = @"<Target Name=""CustomContentTarget"">
    <ItemGroup>
      <TfmSpecificDebugSymbolsFile Include=""abc.pdb"">
        <TargetPath>/runtimes/win/lib/$(TargetFramework)/abc.pdb</TargetPath>
        <TargetFramework>$(TargetFramework)</TargetFramework>
      </TfmSpecificDebugSymbolsFile>
    </ItemGroup>
  </Target>";
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, tfmProperty, tfmValue);
                    ProjectFileUtils.AddProperty(xml, "TargetsForTfmSpecificDebugSymbolsInPackage", "CustomContentTarget");
                    ProjectFileUtils.AddProperty(xml, "IncludeSymbols", "true");
                    ProjectFileUtils.AddCustomXmlToProjectRoot(xml, target);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.symbols.nupkg");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var files = nupkgReader.GetFiles();
                    Assert.Contains(@"runtimes/win/lib/netstandard1.4/abc.pdb", files);

                    if (tfmProperty == "TargetFrameworks")
                    {
                        Assert.Contains(@"runtimes/win/lib/net46/abc.pdb", files);
                    }
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_ContentInnerTargetExtension_SymbolFilesWithoutDll()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                Directory.CreateDirectory(workingDirectory);
                File.WriteAllText(Path.Combine(workingDirectory, "abc.pdb"), "hello world");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    var target = @"<Target Name=""CustomContentTarget"">
    <ItemGroup>
      <TfmSpecificDebugSymbolsFile Include=""abc.pdb"">
        <TargetPath>/runtimes/win/lib/$(TargetFramework)/abc.pdb</TargetPath>
        <TargetFramework>$(TargetFramework)</TargetFramework>
      </TfmSpecificDebugSymbolsFile>
    </ItemGroup>
  </Target>";
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFrameworks", "netstandard1.4;net46");
                    ProjectFileUtils.AddProperty(xml, "TargetsForTfmSpecificDebugSymbolsInPackage", "CustomContentTarget");
                    ProjectFileUtils.AddProperty(xml, "IncludeBuildOutput", "false", $"'$(TargetFramework)'=='netstandard1.4'");
                    ProjectFileUtils.AddProperty(xml, "IncludeSymbols", "true");
                    ProjectFileUtils.AddCustomXmlToProjectRoot(xml, target);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.symbols.nupkg");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var files = nupkgReader.GetFiles();
                    Assert.Contains(@"runtimes/win/lib/netstandard1.4/abc.pdb", files);
                    Assert.Contains(@"runtimes/win/lib/net46/abc.pdb", files);
                    Assert.Contains(@"lib/net46/ClassLibrary1.pdb", files);
                    Assert.Contains(@"lib/net46/ClassLibrary1.dll", files);
                    Assert.DoesNotContain(@"lib/netstandard1.4/ClassLibrary1.pdb", files);
                    Assert.DoesNotContain(@"lib/netstandard1.4/ClassLibrary1.dll", files);
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_ContentInnerTargetExtension_Snupkg()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                Directory.CreateDirectory(workingDirectory);
                File.WriteAllText(Path.Combine(workingDirectory, "abc.pdb"), "hello world");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    var target = @"<Target Name=""CustomContentTarget"">
    <ItemGroup>
      <TfmSpecificDebugSymbolsFile Include=""abc.pdb"">
        <TargetPath>/runtimes/win/lib/$(TargetFramework)/abc.pdb</TargetPath>
        <TargetFramework>$(TargetFramework)</TargetFramework>
      </TfmSpecificDebugSymbolsFile>
    </ItemGroup>
  </Target>";
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFrameworks", "netstandard1.4;net46");
                    ProjectFileUtils.AddProperty(xml, "TargetsForTfmSpecificDebugSymbolsInPackage", "CustomContentTarget");
                    ProjectFileUtils.AddProperty(xml, "IncludeSymbols", "true");
                    ProjectFileUtils.AddProperty(xml, "SymbolPackageFormat", "snupkg");
                    ProjectFileUtils.AddCustomXmlToProjectRoot(xml, target);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.snupkg");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var files = nupkgReader.GetFiles();
                    Assert.Contains(@"runtimes/win/lib/netstandard1.4/abc.pdb", files);
                    Assert.Contains(@"runtimes/win/lib/net46/abc.pdb", files);
                    Assert.Contains(@"lib/net46/ClassLibrary1.pdb", files);
                    Assert.Contains(@"lib/netstandard1.4/ClassLibrary1.pdb", files);
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_ContentInnerTargetExtension_SymbolFilesWithoutBuildOuput()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                Directory.CreateDirectory(workingDirectory);
                File.WriteAllText(Path.Combine(workingDirectory, "abc.pdb"), "hello world");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    var target = @"<Target Name=""CustomContentTarget"">
    <ItemGroup>
      <TfmSpecificDebugSymbolsFile Include=""abc.pdb"">
        <TargetPath>/runtimes/win/lib/$(TargetFramework)/abc.pdb</TargetPath>
        <TargetFramework>$(TargetFramework)</TargetFramework>
      </TfmSpecificDebugSymbolsFile>
    </ItemGroup>
  </Target>";
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFrameworks", "netstandard1.4;net46");
                    ProjectFileUtils.AddProperty(xml, "TargetsForTfmSpecificDebugSymbolsInPackage", "CustomContentTarget");
                    ProjectFileUtils.AddProperty(xml, "IncludeBuildOutput", "false", $"'$(TargetFramework)'=='netstandard1.4'");
                    ProjectFileUtils.AddProperty(xml, "IncludeSymbols", "true");
                    ProjectFileUtils.AddProperty(xml, "SymbolPackageFormat", "snupkg");
                    ProjectFileUtils.AddCustomXmlToProjectRoot(xml, target);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.snupkg");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var files = nupkgReader.GetFiles();
                    Assert.Contains(@"runtimes/win/lib/netstandard1.4/abc.pdb", files);
                    Assert.Contains(@"runtimes/win/lib/net46/abc.pdb", files);
                    Assert.Contains(@"lib/net46/ClassLibrary1.pdb", files);
                    Assert.DoesNotContain(@"lib/netstandard1.4/ClassLibrary1.pdb", files);
                }
            }
        }
        [PlatformFact(Platform.Windows)]
        public void PackCommand_ContentInnerTargetExtension_SymbolFilesDllWithRecursive()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                string symbolPath = Path.Combine(workingDirectory, "Random", "AnotherRandom");
                Directory.CreateDirectory(symbolPath);
                File.WriteAllText(Path.Combine(symbolPath, "abc.pdb"), "hello world");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    var target = @"<Target Name=""CustomContentTarget"">
    <ItemGroup>
      <TfmSpecificDebugSymbolsFile Include=""Random/**/abc.pdb"">
        <TargetPath>/runtimes/win/lib/$(TargetFramework)/random/abc.pdb</TargetPath>
        <TargetFramework>$(TargetFramework)</TargetFramework>
      </TfmSpecificDebugSymbolsFile>
    </ItemGroup>
  </Target>";
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFrameworks", "netstandard1.4;net46");
                    ProjectFileUtils.AddProperty(xml, "TargetsForTfmSpecificDebugSymbolsInPackage", "CustomContentTarget");
                    ProjectFileUtils.AddProperty(xml, "IncludeBuildOutput", "false", $"'$(TargetFramework)'=='netstandard1.4'");
                    ProjectFileUtils.AddProperty(xml, "IncludeSymbols", "true");
                    ProjectFileUtils.AddCustomXmlToProjectRoot(xml, target);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.symbols.nupkg");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var files = nupkgReader.GetFiles();
                    Assert.Contains(@"runtimes/win/lib/netstandard1.4/random/abc.pdb", files);
                    Assert.Contains(@"runtimes/win/lib/net46/random/abc.pdb", files);
                    Assert.Contains(@"lib/net46/ClassLibrary1.pdb", files);
                    Assert.Contains(@"lib/net46/ClassLibrary1.dll", files);
                    Assert.DoesNotContain(@"lib/netstandard1.4/ClassLibrary1.pdb", files);
                    Assert.DoesNotContain(@"lib/netstandard1.4/ClassLibrary1.dll", files);
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("TargetFramework", "netstandard1.4")]
        [InlineData("TargetFrameworks", "netstandard1.4;net46")]
        public void PackCommand_BuildOutputInnerTargetExtension_AddsTfmSpecificBuildOuput(string tfmProperty,
    string tfmValue)
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                Directory.CreateDirectory(workingDirectory);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                File.WriteAllText(Path.Combine(workingDirectory, "abc.dll"), "hello world");
                File.WriteAllText(Path.Combine(workingDirectory, "abc.pdb"), "hello world");
                var pathToDll = Path.Combine(workingDirectory, "abc.dll");
                var pathToPdb = Path.Combine(workingDirectory, "abc.pdb");
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    var target =
                        $@"<Target Name=""CustomBuildOutputTarget"">
    <ItemGroup>
      <BuildOutputInPackage Include=""abc.dll"">
        <FinalOutputPath>{pathToDll}</FinalOutputPath>
      </BuildOutputInPackage>
      <BuildOutputInPackage Include=""abc.pdb"">
        <FinalOutputPath>{pathToPdb}</FinalOutputPath>
      </BuildOutputInPackage>
    </ItemGroup>
  </Target>";
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, tfmProperty, tfmValue);
                    ProjectFileUtils.AddProperty(xml, "TargetsForTfmSpecificBuildOutput", "CustomBuildOutputTarget");
                    ProjectFileUtils.AddCustomXmlToProjectRoot(xml, target);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory} /p:IncludeSymbols=true /p:SymbolPackageFormat=symbols.nupkg");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                var symbolNupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.symbols.nupkg");

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                using (var symbolNupkgReader = new PackageArchiveReader(symbolNupkgPath))
                {
                    var tfms = tfmValue.Split(';');
                    // Validate the assets.
                    var libItems = nupkgReader.GetLibItems().ToList();
                    var symbolLibItems = symbolNupkgReader.GetLibItems().ToList();
                    Assert.Equal(tfms.Length, libItems.Count);

                    if (tfms.Length == 2)
                    {
                        Assert.Equal(FrameworkConstants.CommonFrameworks.Net46, libItems[0].TargetFramework);
                        Assert.Equal(new[] { "lib/net46/abc.dll", "lib/net46/ClassLibrary1.dll" },
                            libItems[0].Items);
                        Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard14, libItems[1].TargetFramework);
                        Assert.Equal(new[] { "lib/netstandard1.4/abc.dll", "lib/netstandard1.4/ClassLibrary1.dll" },
                            libItems[1].Items);
                        Assert.Equal(FrameworkConstants.CommonFrameworks.Net46, symbolLibItems[0].TargetFramework);
                        Assert.Equal(new[] { "lib/net46/abc.dll", "lib/net46/abc.pdb", "lib/net46/ClassLibrary1.dll", "lib/net46/ClassLibrary1.pdb" },
                            symbolLibItems[0].Items);
                        Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard14, symbolLibItems[1].TargetFramework);
                        Assert.Equal(new[] { "lib/netstandard1.4/abc.dll", "lib/netstandard1.4/abc.pdb", "lib/netstandard1.4/ClassLibrary1.dll", "lib/netstandard1.4/ClassLibrary1.pdb" },
                            symbolLibItems[1].Items);
                    }
                    else
                    {
                        Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard14, libItems[0].TargetFramework);
                        Assert.Equal(new[] { "lib/netstandard1.4/abc.dll", "lib/netstandard1.4/ClassLibrary1.dll" },
                            libItems[0].Items);
                        Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard14, symbolLibItems[0].TargetFramework);
                        Assert.Equal(new[] { "lib/netstandard1.4/abc.dll", "lib/netstandard1.4/abc.pdb", "lib/netstandard1.4/ClassLibrary1.dll", "lib/netstandard1.4/ClassLibrary1.pdb" },
                            symbolLibItems[0].Items);
                    }
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("folderA\\**\\*", null, "content/folderA/folderA.txt;content/folderA/folderB/folderB.txt;" +
                                                                                                "contentFiles/any/netstandard1.4/folderA/folderA.txt;" +
                                                                                                "contentFiles/any/netstandard1.4/folderA/folderB/folderB.txt")]
        [InlineData("folderA\\**\\*", "pkgA", "pkgA/folderA.txt;pkgA/folderB/folderB.txt")]
        [InlineData("folderA\\**\\*", "pkgA/", "pkgA/folderA.txt;pkgA/folderB/folderB.txt")]
        [InlineData("folderA\\**\\*", "pkgA\\", "pkgA/folderA.txt;pkgA/folderB/folderB.txt")]
        [InlineData("folderA\\**", "pkgA", "pkgA/folderA.txt;pkgA/folderB/folderB.txt")]
        public void PackCommand_PackProject_GlobbingPathsPacksContentCorrectly(string sourcePath, string packagePath,
            string expectedTargetPaths)
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);

                // Create the subdirectory structure for testing possible source paths for the content file
                Directory.CreateDirectory(Path.Combine(workingDirectory, "folderA", "folderB"));
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFramework", "netstandard1.4");

                    var attributes = new Dictionary<string, string>();

                    var properties = new Dictionary<string, string>();
                    if (packagePath != null)
                    {
                        properties["PackagePath"] = packagePath;
                    }
                    ProjectFileUtils.AddItem(
                        xml,
                        "Content",
                        sourcePath,
                        string.Empty,
                        properties,
                        attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                File.WriteAllText(Path.Combine(workingDirectory, "folderA", "folderA.txt"), "hello world from subfolder A directory");
                File.WriteAllText(Path.Combine(workingDirectory, "folderA", "folderB", "folderB.txt"), "hello world from subfolder B directory");
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                // Assert
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var items = new HashSet<string>(nupkgReader.GetFiles());
                    var expectedPaths = expectedTargetPaths.Split(';');
                    // we add 5 because of the 5 standard files present in the nupkg that won't change.
                    Assert.Equal(items.Count(), expectedPaths.Length + 5);
                    foreach (var path in expectedPaths)
                    {
                        Assert.Contains(path, items);
                    }
                }
            }
        }

        [PlatformTheory(Platform.Windows)]

        [InlineData("PresentationFramework", true, "netstandard1.4;net461", "", "net461")]
        [InlineData("PresentationFramework", false, "netstandard1.4;net461", "", "net461")]
        [InlineData("System.IO", true, "netstandard1.4;net46", "", "net46")]
        [InlineData("System.IO", true, "net46;net461", "net461", "net461")]
        [InlineData("System.IO", true, "net461", "", "net461")]
        public void PackCommand_PackProject_AddsReferenceAsFrameworkAssemblyReference(string referenceAssembly, bool pack,
            string targetFrameworks, string conditionalFramework, string expectedTargetFramework)
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);

                // Create the subdirectory structure for testing possible source paths for the content file
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    var frameworkProperty = "TargetFrameworks";
                    if (targetFrameworks.Split(';').Count() == 1)
                    {
                        frameworkProperty = "TargetFramework";
                    }
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, frameworkProperty, targetFrameworks);

                    var attributes = new Dictionary<string, string>();

                    var properties = new Dictionary<string, string>();
                    if (!pack)
                    {
                        attributes["Pack"] = "false";
                    }
                    ProjectFileUtils.AddItem(
                        xml,
                        "Reference",
                        referenceAssembly,
                        conditionalFramework,
                        properties,
                        attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                // Assert
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");
                //XPlatTestUtils.WaitForDebugger();
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var expectedFrameworks = expectedTargetFramework.Split(';');
                    var frameworkItems = nupkgReader.GetFrameworkItems();
                    foreach (var framework in expectedFrameworks)
                    {
                        var nugetFramework = NuGetFramework.Parse(framework);
                        var frameworkSpecificGroup = frameworkItems.Where(t => t.TargetFramework.Equals(nugetFramework)).FirstOrDefault();
                        if (pack)
                        {
                            Assert.True(frameworkSpecificGroup?.Items.Contains(referenceAssembly));
                        }
                        else
                        {
                            Assert.Null(frameworkSpecificGroup);
                        }

                    }
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("Content", "", "Content")]
        [InlineData("Content", "Page", "Page")]
        [InlineData("EmbeddedResource", "", "EmbeddedResource")]
        [InlineData("EmbeddedResource", "ApplicationDefinition", "ApplicationDefinition")]
        [InlineData("Content", "LinkDescription", "LinkDescription")]
        [InlineData("Content", "RandomBuildAction", "RandomBuildAction")]
        public void PackCommand_PackProject_OutputsBuildActionForContentFiles(string itemType, string buildAction, string expectedBuildAction)
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);

                // Create the subdirectory structure for testing possible source paths for the content file
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                File.WriteAllBytes(Path.Combine(workingDirectory, "abc.png"), Array.Empty<byte>());

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);

                    var attributes = new Dictionary<string, string>();
                    attributes["Pack"] = "true";
                    var properties = new Dictionary<string, string>();
                    properties["BuildAction"] = buildAction;

                    ProjectFileUtils.AddItem(
                        xml,
                        itemType,
                        "abc.png",
                        NuGetFramework.AnyFramework,
                        properties,
                        attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                // Assert
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;
                    var contentFiles = nuspecReader.GetContentFiles().ToArray();

                    Assert.True(contentFiles.Count() == 1);
                    var contentFile = contentFiles[0];
                    Assert.Equal(expectedBuildAction, contentFile.BuildAction);
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackSolution_AddsProjectRefsAsPackageRefs()
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var solutionName = "Solution1";
                var projectName = "ClassLibrary1";
                var referencedProject1 = "ClassLibrary2";
                var referencedProject2 = "ClassLibrary3";

                var projectAndReference1Folder = "Src";
                var rederence2Folder = "src";

                var projectFolder = Path.Combine(testDirectory.Path, projectAndReference1Folder, projectName);

                var projectFileRelativ = Path.Combine(projectAndReference1Folder, projectName, $"{projectName}.csproj");
                var referencedProject1RelativDir = Path.Combine(projectAndReference1Folder, referencedProject1, $"{referencedProject1}.csproj");
                var referencedProject2RelativDir = Path.Combine(rederence2Folder, referencedProject2, $"{referencedProject2}.csproj");

                msbuildFixture.CreateDotnetNewProject(Path.Combine(testDirectory.Path, projectAndReference1Folder), projectName, "classlib -f netstandard2.0");
                msbuildFixture.CreateDotnetNewProject(Path.Combine(testDirectory.Path, projectAndReference1Folder), referencedProject1, "classlib -f netstandard2.0");
                msbuildFixture.CreateDotnetNewProject(Path.Combine(testDirectory.Path, rederence2Folder), referencedProject2, "classlib -f netstandard2.0");

                msbuildFixture.RunDotnet(testDirectory.Path, $"new sln -n {solutionName}");
                msbuildFixture.RunDotnet(testDirectory.Path, $"sln {solutionName}.sln add {projectFileRelativ}");
                msbuildFixture.RunDotnet(testDirectory.Path, $"sln {solutionName}.sln add {referencedProject1RelativDir}");
                msbuildFixture.RunDotnet(testDirectory.Path, $"sln {solutionName}.sln add {referencedProject2RelativDir}");

                var projectFile = Path.Combine(testDirectory.Path, projectFileRelativ);
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);

                    var attributes = new Dictionary<string, string>();

                    var properties = new Dictionary<string, string>();
                    ProjectFileUtils.AddItem(
                        xml,
                        "ProjectReference",
                        @"..\ClassLibrary2\ClassLibrary2.csproj",
                        string.Empty,
                        properties,
                        attributes);

                    ProjectFileUtils.AddItem(
                        xml,
                        "ProjectReference",
                        @"..\ClassLibrary3\ClassLibrary3.csproj",
                        string.Empty,
                        properties,
                        attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreSolution(testDirectory, solutionName, args: string.Empty);
                // Act
                msbuildFixture.PackSolution(testDirectory, solutionName, args: string.Empty);

                var nupkgPath = Path.Combine(projectFolder, "bin", "Debug", $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(projectFolder, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                // Assert
                Assert.Equal("Src", projectAndReference1Folder);
                Assert.Equal("src", rederence2Folder);

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    var dependencyGroups = nuspecReader
                        .GetDependencyGroups()
                        .OrderBy(x => x.TargetFramework,
                            new NuGetFrameworkSorter())
                        .ToList();

                    Assert.Equal(1,
                        dependencyGroups.Count);

                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard20, dependencyGroups[0].TargetFramework);
                    var packagesA = dependencyGroups[0].Packages.ToList();
                    Assert.Equal(2, packagesA.Count);
                    Assert.Equal(referencedProject1, packagesA[0].Id);
                    Assert.Equal(new VersionRange(new NuGetVersion("1.0.0")), packagesA[0].VersionRange);
                    Assert.Equal(new List<string> { "Analyzers", "Build" }, packagesA[0].Exclude);
                    Assert.Empty(packagesA[0].Include);

                    Assert.Equal(referencedProject2, packagesA[1].Id);
                    Assert.Equal(new VersionRange(new NuGetVersion("1.0.0")), packagesA[1].VersionRange);
                    Assert.Equal(new List<string> { "Analyzers", "Build" }, packagesA[1].Exclude);
                    Assert.Empty(packagesA[1].Include);

                    // Validate the assets.
                    var libItems = nupkgReader.GetLibItems().ToList();
                    Assert.Equal(1, libItems.Count);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard20, libItems[0].TargetFramework);
                    Assert.Equal(
                        new[]
                        {$"lib/netstandard2.0/{projectName}.dll"},
                        libItems[0].Items);
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("TargetFramework", "netstandard1.4")]
        [InlineData("TargetFrameworks", "netstandard1.4;net46")]
#if NETCOREAPP5_0
        [InlineData("TargetFramework", "net5.0")]
        [InlineData("TargetFrameworks", "netstandard1.4;net5.0")]
#endif
        public void PackCommand_PackTargetHook_ExecutesBeforePack(string tfmProperty,
    string tfmValue)
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                Directory.CreateDirectory(workingDirectory);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    var target =
                        $@"<Target Name=""RunBeforePack"">
    <Message Text= ""Hello World"" Importance=""High""/>
    </Target>";
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, tfmProperty, tfmValue);
                    ProjectFileUtils.AddProperty(xml, "BeforePack", "RunBeforePack");
                    ProjectFileUtils.AddCustomXmlToProjectRoot(xml, target);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");
                var indexOfHelloWorld = result.AllOutput.IndexOf("Hello World");
                var indexOfPackSuccessful = result.AllOutput.IndexOf("Successfully created package");
                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");
                Assert.True(indexOfHelloWorld < indexOfPackSuccessful, "The custom target RunBeforePack did not run before pack target.");

            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("TargetFramework", "netstandard1.4")]
        [InlineData("TargetFrameworks", "netstandard1.4;net46")]
#if NETCOREAPP5_0
        [InlineData("TargetFramework", "net5.0")]
        [InlineData("TargetFrameworks", "netstandard1.4;net5.0")]
#endif
        public void PackCommand_PackTarget_IsIncremental(string tfmProperty, string tfmValue)
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                Directory.CreateDirectory(workingDirectory);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);

                    ProjectFileUtils.SetTargetFrameworkForProject(xml, tfmProperty, tfmValue);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");

                msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory} -bl:firstPack.binlog");

                var nupkgLastWriteTime = File.GetLastWriteTimeUtc(nupkgPath);
                var nuspecLastWriteTime = File.GetLastWriteTimeUtc(nuspecPath);

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory} -bl:secondPack.binlog");

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                Assert.Equal(nupkgLastWriteTime, File.GetLastWriteTimeUtc(nupkgPath));
                Assert.Equal(nuspecLastWriteTime, File.GetLastWriteTimeUtc(nuspecPath));
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("NoWarn", "NU5125", false)]
        [InlineData("NoWarn", "NU5106", true)]
        public void PackCommand_NoWarn_SuppressesWarnings(string property, string value, bool expectToWarn)
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                Directory.CreateDirectory(workingDirectory);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);

                    ProjectFileUtils.AddProperty(xml, property, value);
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseUrl", "http://contoso.com/license.html");
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");

                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");
                var expectedWarning = string.Format("warning " + NuGetLogCode.NU5125 + ": " + NuGet.Packaging.Rules.AnalysisResources.LicenseUrlDeprecationWarning);

                if (expectToWarn)
                {
                    result.AllOutput.Should().Contain(expectedWarning);
                }
                else
                {
                    result.AllOutput.Should().NotContain(expectedWarning);
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("WarningsAsErrors", "NU5102", true)]
        [InlineData("WarningsAsErrors", "NU5106", false)]
        [InlineData("TreatWarningsAsErrors", "true", true)]
        public void PackCommand_WarnAsError_PrintsWarningsAsErrors(string property, string value, bool expectToError)
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var semver2Version = "1.0.0-rtm+asdassd";
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                Directory.CreateDirectory(workingDirectory);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);

                    ProjectFileUtils.AddProperty(xml, "PackageProjectUrl", "http://project_url_here_or_delete_this_line/");
                    ProjectFileUtils.AddProperty(xml, property, value);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0-rtm.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0-rtm.nuspec");

                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory} /p:Version={semver2Version}", validateSuccess: false);

                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");
                if (expectToError)
                {
                    result.AllOutput.Should().Contain(NuGetLogCode.NU5102.ToString());
                    result.ExitCode.Should().NotBe(0);
                    result.AllOutput.Should().NotContain("success");
                    Assert.False(File.Exists(nupkgPath), "The output .nupkg should not exist when pack fails.");
                }
                else
                {
                    Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_IncrementalPack_FailsWhenInvokedTwiceInARow()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var semver2Version = "1.0.0-rtm+asdassd";
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                Directory.CreateDirectory(workingDirectory);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);

                    ProjectFileUtils.AddProperty(xml, "PackageProjectUrl", "http://project_url_here_or_delete_this_line/");
                    ProjectFileUtils.AddProperty(xml, "TreatWarningsAsErrors", "true");
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0-rtm.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0-rtm.nuspec");

                // Call once.
                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory} /p:Version={semver2Version}", validateSuccess: false);
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");
                Assert.False(File.Exists(nupkgPath), "The output .nupkg should not exist when pack fails.");
                result.AllOutput.Should().Contain(NuGetLogCode.NU5102.ToString());
                result.ExitCode.Should().NotBe(0);
                result.AllOutput.Should().NotContain("success");

                // Call twice.
                result = msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory} /p:Version={semver2Version}", validateSuccess: false);
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");
                Assert.False(File.Exists(nupkgPath), "The output .nupkg should not exist when pack fails.");
                result.AllOutput.Should().Contain(NuGetLogCode.NU5102.ToString());
                result.ExitCode.Should().NotBe(0);
                result.AllOutput.Should().NotContain("success");
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackWithRepositoryVerifyNuspec()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "RepositoryType", "git");
                    ProjectFileUtils.AddProperty(xml, "RepositoryUrl", "https://github.com/NuGet/NuGet.Client.git");
                    ProjectFileUtils.AddProperty(xml, "RepositoryBranch", "dev");
                    ProjectFileUtils.AddProperty(xml, "RepositoryCommit", "e1c65e4524cd70ee6e22abe33e6cb6ec73938cb3");
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the output .nuspec.
                    nuspecReader.GetRepositoryMetadata().Type.Should().Be("git");
                    nuspecReader.GetRepositoryMetadata().Url.Should().Be("https://github.com/NuGet/NuGet.Client.git");
                    nuspecReader.GetRepositoryMetadata().Branch.Should().Be("dev");
                    nuspecReader.GetRepositoryMetadata().Commit.Should().Be("e1c65e4524cd70ee6e22abe33e6cb6ec73938cb3");
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackWithSourceControlInformation_Unsupported_VerifyNuspec()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    var ns = xml.Root.Name.Namespace;

                    ProjectFileUtils.AddProperty(xml, "RepositoryType", "git");

                    // mock implementation of InitializeSourceControlInformation common targets:
                    xml.Root.Add(
                        new XElement(ns + "Target",
                            new XAttribute("Name", "InitializeSourceControlInformation"),
                            new XElement(ns + "PropertyGroup",
                                new XElement("SourceRevisionId", "e1c65e4524cd70ee6e22abe33e6cb6ec73938cb1"),
                                new XElement("PrivateRepositoryUrl", "https://github.com/NuGet/NuGet.Client.git"))));

                    xml.Root.Add(
                        new XElement(ns + "PropertyGroup",
                            new XElement("SourceControlInformationFeatureSupported", "false")));

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the output .nuspec.
                    var repositoryMetadata = nuspecReader.GetRepositoryMetadata();
                    repositoryMetadata.Type.Should().Be("git");
                    repositoryMetadata.Url.Should().Be("");
                    repositoryMetadata.Branch.Should().Be("");
                    repositoryMetadata.Commit.Should().Be("");
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackWithSourceControlInformation_PrivateUrl_VerifyNuspec()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    var ns = xml.Root.Name.Namespace;

                    ProjectFileUtils.AddProperty(xml, "RepositoryType", "git");

                    xml.Root.Add(
                        new XElement(ns + "PropertyGroup",
                            new XElement("SourceControlInformationFeatureSupported", "true")));

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                // mock implementation of InitializeSourceControlInformation common targets:
                var mockXml = @"<Project>
<Target Name=""InitializeSourceControlInformation"">
    <PropertyGroup>
      <SourceRevisionId>e1c65e4524cd70ee6e22abe33e6cb6ec73938cb1</SourceRevisionId>
      <PrivateRepositoryUrl>https://github.com/NuGet/NuGet.Client.git</PrivateRepositoryUrl>
    </PropertyGroup>
</Target>
</Project>";

                File.WriteAllText(Path.Combine(workingDirectory, "Directory.build.targets"), mockXml);


                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the output .nuspec.
                    var repositoryMetadata = nuspecReader.GetRepositoryMetadata();
                    repositoryMetadata.Type.Should().Be("git");
                    repositoryMetadata.Url.Should().Be("");
                    repositoryMetadata.Branch.Should().Be("");
                    repositoryMetadata.Commit.Should().Be("e1c65e4524cd70ee6e22abe33e6cb6ec73938cb1");
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackWithSourceControlInformation_PublishedUrl_VerifyNuspec()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    var ns = xml.Root.Name.Namespace;

                    ProjectFileUtils.AddProperty(xml, "RepositoryType", "git");
                    ProjectFileUtils.AddProperty(xml, "PublishRepositoryUrl", "true");

                    xml.Root.Add(
                        new XElement(ns + "PropertyGroup",
                            new XElement("SourceControlInformationFeatureSupported", "true")));

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                // mock implementation of InitializeSourceControlInformation common targets:
                var mockXml = @"<Project>
<Target Name=""InitializeSourceControlInformation"">
    <PropertyGroup>
      <SourceRevisionId>e1c65e4524cd70ee6e22abe33e6cb6ec73938cb1</SourceRevisionId>
      <PrivateRepositoryUrl>https://github.com/NuGet/NuGet.Client.git</PrivateRepositoryUrl>
    </PropertyGroup>
</Target>
</Project>";

                File.WriteAllText(Path.Combine(workingDirectory, "Directory.build.targets"), mockXml);

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the output .nuspec.
                    var repositoryMetadata = nuspecReader.GetRepositoryMetadata();
                    repositoryMetadata.Type.Should().Be("git");
                    repositoryMetadata.Url.Should().Be("https://github.com/NuGet/NuGet.Client.git");
                    repositoryMetadata.Branch.Should().Be("");
                    repositoryMetadata.Commit.Should().Be("e1c65e4524cd70ee6e22abe33e6cb6ec73938cb1");
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackWithSourceControlInformation_ProjectOverride_VerifyNuspec()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    var ns = xml.Root.Name.Namespace;

                    ProjectFileUtils.AddProperty(xml, "RepositoryType", "git");
                    ProjectFileUtils.AddProperty(xml, "PublishRepositoryUrl", "true");
                    ProjectFileUtils.AddProperty(xml, "RepositoryCommit", "1111111111111111111111111111111111111111");
                    ProjectFileUtils.AddProperty(xml, "RepositoryUrl", "https://github.com/Overridden");

                    // mock implementation of InitializeSourceControlInformation common targets:
                    xml.Root.Add(
                        new XElement(ns + "Target",
                            new XAttribute("Name", "InitializeSourceControlInformation"),
                            new XElement(ns + "PropertyGroup",
                                new XElement("SourceRevisionId", "e1c65e4524cd70ee6e22abe33e6cb6ec73938cb1"),
                                new XElement("PrivateRepositoryUrl", "https://github.com/NuGet/NuGet.Client"))));

                    xml.Root.Add(
                        new XElement(ns + "PropertyGroup",
                            new XElement("SourceControlInformationFeatureSupported", "true")));

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the output .nuspec.
                    var repositoryMetadata = nuspecReader.GetRepositoryMetadata();
                    repositoryMetadata.Type.Should().Be("git");
                    repositoryMetadata.Url.Should().Be("https://github.com/Overridden");
                    repositoryMetadata.Branch.Should().Be("");
                    repositoryMetadata.Commit.Should().Be("1111111111111111111111111111111111111111");
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_ManualAddPackage_DevelopmentDependency()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = msbuildFixture.CreateSimpleTestPathContext())
            {
                SimpleTestSettingsContext settings = pathContext.Settings;
                settings.AddNetStandardFeeds();

                string testDirectory = pathContext.WorkingDirectory;
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory, projectName);

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFramework", "net45");

                    var attributes = new Dictionary<string, string>();

                    attributes["Version"] = "1.0.2";
                    ProjectFileUtils.AddItem(
                        xml,
                        "PackageReference",
                        "StyleCop.Analyzers",
                        "net45",
                        new Dictionary<string, string>(),
                        attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                // Assert
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    var dependencyGroups = nuspecReader
                        .GetDependencyGroups()
                        .OrderBy(x => x.TargetFramework,
                            new NuGetFrameworkSorter())
                        .ToList();

                    Assert.Equal(1,
                        dependencyGroups.Count);

                    Assert.Equal(FrameworkConstants.CommonFrameworks.Net45, dependencyGroups[0].TargetFramework);
                    Assert.Equal(1, dependencyGroups[0].Packages.Count());
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("net45", "netstandard1.3")]
        [InlineData("netstandard1.3", "net45")]
        [InlineData("", "")]
        public void PackCommand_SuppressDependencies_DoesNotContainAnyDependency(string frameworkToSuppress, string expectedInFramework)
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName);

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFrameworks", "net45;netstandard1.3");
                    if (!string.IsNullOrEmpty(frameworkToSuppress))
                    {
                        ProjectFileUtils.AddProperty(xml, "SuppressDependenciesWhenPacking", "true", $"'$(TargetFramework)'=='{frameworkToSuppress}'");
                    }
                    else
                    {
                        ProjectFileUtils.AddProperty(xml, "SuppressDependenciesWhenPacking", "true");
                    }

                    var attributes = new Dictionary<string, string>();

                    attributes["Version"] = "9.0.1";
                    ProjectFileUtils.AddItem(
                        xml,
                        "PackageReference",
                        "Newtonsoft.json",
                        "",
                        new Dictionary<string, string>(),
                        attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                // Assert
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;
                    var expectedFrameworks = expectedInFramework.Split(';');
                    var dependencyGroups = nuspecReader
                        .GetDependencyGroups()
                        .OrderBy(x => x.TargetFramework,
                            new NuGetFrameworkSorter())
                        .ToList();

                    Assert.Equal(expectedFrameworks.Where(t => !string.IsNullOrEmpty(t)).Count(),
                        dependencyGroups.Count);

                    if (dependencyGroups.Count > 0)
                    {
                        Assert.Equal(dependencyGroups[0].TargetFramework, NuGetFramework.Parse(expectedFrameworks[0]));
                    }
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("net45", ".NETStandard,Version=v1.3")]
        [InlineData("netstandard1.3", ".NETFramework,Version=v4.5")]
        public void PackCommand_BuildOutput_DoesNotContainForSpecificFramework(string frameworkToExclude, string frameworkInPackage)
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName);

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFrameworks", "net45;netstandard1.3");
                    ProjectFileUtils.AddProperty(xml, "IncludeBuildOutput", "false", $"'$(TargetFramework)'=='{frameworkToExclude}'");
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                // Assert
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;
                    var libItems = nupkgReader.GetLibItems().ToList();
                    Assert.Equal(1, libItems.Count);
                    Assert.Contains(frameworkInPackage, libItems[0].TargetFramework.ToString());
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_BuildOutput_DoesNotContainDefaultExtensions()
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName);

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFramework", "net5.0");
                    ProjectFileUtils.AddProperty(xml, "DefaultAllowedOutputExtensionsInPackageBuildOutputFolder", ".dll");
                    ProjectFileUtils.AddProperty(xml, "GenerateDocumentationFile", "true");
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                // Assert
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var allFiles = nupkgReader.GetFiles().ToList();
                    Assert.Contains($"lib/net5.0/{projectName}.dll", allFiles);
                    Assert.DoesNotContain($"lib/net5.0/{projectName}.xml", allFiles);
                    Assert.False(allFiles.Any(f => f.EndsWith(".exe")));
                    Assert.False(allFiles.Any(f => f.EndsWith(".winmd")));
                    Assert.False(allFiles.Any(f => f.EndsWith(".json")));
                    Assert.False(allFiles.Any(f => f.EndsWith(".pri")));
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("MIT")]
        [InlineData("MIT OR Apache-2.0 WITH 389-exception")]
        public void PackCommand_PackLicense_SimpleExpression_StandardLicense(string licenseExpr)
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                // Set up
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                // Setup LicenseExpression
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseExpression", licenseExpr);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");
                Assert.True(!result.AllOutput.Contains("NU5034"));

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the output .nuspec.
                    Assert.Equal("ClassLibrary1", nuspecReader.GetId());
                    Assert.Equal("1.0.0", nuspecReader.GetVersion().ToFullString());
                    Assert.False(nuspecReader.GetRequireLicenseAcceptance());
                    Assert.Equal(new Uri(string.Format(LicenseMetadata.LicenseServiceLinkTemplate, licenseExpr)), new Uri(nuspecReader.GetLicenseUrl()));
                    var licenseMetadata = nuspecReader.GetLicenseMetadata();
                    Assert.NotNull(licenseMetadata);
                    Assert.Equal(licenseMetadata.LicenseUrl.OriginalString, nuspecReader.GetLicenseUrl());
                    Assert.Equal(licenseMetadata.LicenseUrl, new Uri(nuspecReader.GetLicenseUrl()));
                    Assert.Equal(licenseMetadata.Type, LicenseType.Expression);
                    Assert.Equal(licenseMetadata.Version, LicenseMetadata.EmptyVersion);
                    Assert.Equal(licenseMetadata.License, licenseExpr);
                    Assert.Equal(licenseExpr, licenseMetadata.LicenseExpression.ToString());
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackLicense_ComplexExpression_WithNonStandardLicense()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var customLicense = "LicenseRef-Nikolche";
                var licenseExpr = $"MIT OR {customLicense} WITH 389-exception";
                // Set up
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                // Setup LicenseExpression
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseExpression", licenseExpr);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                Assert.True(result.Success);
                Assert.True(result.AllOutput.Contains("NU5124"));

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the output .nuspec.
                    Assert.Equal("ClassLibrary1", nuspecReader.GetId());
                    Assert.Equal("1.0.0", nuspecReader.GetVersion().ToFullString());
                    Assert.False(nuspecReader.GetRequireLicenseAcceptance());
                    Assert.Equal(new Uri(string.Format(LicenseMetadata.LicenseServiceLinkTemplate, licenseExpr)), new Uri(nuspecReader.GetLicenseUrl()));
                    var licenseMetadata = nuspecReader.GetLicenseMetadata();
                    Assert.NotNull(licenseMetadata);
                    Assert.Equal(licenseMetadata.LicenseUrl.OriginalString, nuspecReader.GetLicenseUrl());
                    Assert.Equal(licenseMetadata.LicenseUrl, new Uri(nuspecReader.GetLicenseUrl()));
                    Assert.Equal(licenseMetadata.Type, LicenseType.Expression);
                    Assert.Equal(licenseMetadata.Version, LicenseMetadata.EmptyVersion);
                    Assert.Equal(licenseMetadata.License, licenseExpr);
                    Assert.False(licenseMetadata.LicenseExpression.HasOnlyStandardIdentifiers());
                    Assert.Equal(licenseExpr, licenseMetadata.LicenseExpression.ToString());
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("Cant Parse This")]
        [InlineData("Tanana AND nana nana")]
        public void PackCommand_PackLicense_NonParsableExpressionFailsErrorWithCode(string licenseExpr)
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                // Set up
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                // Setup LicenseExpression
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseExpression", licenseExpr);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}", validateSuccess: false);

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.False(File.Exists(nupkgPath));
                Assert.False(File.Exists(nuspecPath));

                Assert.False(result.Success);
                Assert.True(result.AllOutput.Contains("NU5032"));
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackLicense_NonParsableVersionFailsErrorWithCode()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var licenseExpr = "MIT OR Apache-2.0";
                var version = "1.0.0-babanana";
                // Set up
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                // Setup LicenseExpression
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseExpression", licenseExpr);
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseExpressionVersion", version);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}", validateSuccess: false);

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.False(File.Exists(nupkgPath));
                Assert.False(File.Exists(nuspecPath));

                Assert.False(result.Success);
                Assert.True(result.AllOutput.Contains("NU5034"));
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackLicense_ExpressionVersionHigherFailsWithErrorCode()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var licenseExpr = "MIT OR Apache-2.0";
                var version = "2.0.0";
                // Set up
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                // Setup LicenseExpression
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseExpression", licenseExpr);
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseExpressionVersion", version);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}", validateSuccess: false);

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.False(File.Exists(nupkgPath));
                Assert.False(File.Exists(nuspecPath));

                Assert.False(result.Success);
                Assert.True(result.AllOutput.Contains("NU5034"));
                Assert.True(result.AllOutput.Contains($"'{LicenseMetadata.CurrentVersion.ToString()}'"));
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData(@"LICENSE", ".")]
        [InlineData("LICENSE.md", ".")]
        [InlineData("LICENSE.txt", "LICENSE.txt")]
        public void PackCommand_PackLicense_PackBasicLicenseFile(string licenseFileName, string packagesPath)
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                // Set up
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                var licenseFile = Path.Combine(workingDirectory, licenseFileName);

                var licenseText = "Random licenseFile";
                File.WriteAllText(licenseFile, licenseText);
                Assert.True(File.Exists(licenseFile));

                File.WriteAllText(Path.Combine(Path.GetDirectoryName(projectFile), licenseFileName), "The best license ever.");

                // Setup LicenseFile
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseFile", licenseFileName);

                    var attributes = new Dictionary<string, string>();
                    attributes["Pack"] = "true";
                    attributes["PackagePath"] = packagesPath;
                    var properties = new Dictionary<string, string>();
                    ProjectFileUtils.AddItem(
                        xml,
                        "None",
                        licenseFileName,
                        NuGetFramework.AnyFramework,
                        properties,
                        attributes);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                Assert.True(result.Success);

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the output .nuspec.
                    Assert.Equal("ClassLibrary1", nuspecReader.GetId());
                    Assert.Equal("1.0.0", nuspecReader.GetVersion().ToFullString());
                    Assert.False(nuspecReader.GetRequireLicenseAcceptance());
                    Assert.Equal(LicenseMetadata.LicenseFileDeprecationUrl, new Uri(nuspecReader.GetLicenseUrl()));
                    var licenseMetadata = nuspecReader.GetLicenseMetadata();
                    Assert.NotNull(licenseMetadata);
                    Assert.Equal(LicenseMetadata.LicenseFileDeprecationUrl, licenseMetadata.LicenseUrl);
                    Assert.Equal(licenseMetadata.Type, LicenseType.File);
                    Assert.Equal(licenseMetadata.Version, LicenseMetadata.EmptyVersion);
                    Assert.Equal(licenseMetadata.License, licenseFileName);
                    Assert.Null(licenseMetadata.LicenseExpression);
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackLicense_PackBasicLicenseFile_FileNotInPackage()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                // Set up
                var licenseFileName = "LICENSE.txt";
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                var licenseFile = Path.Combine(workingDirectory, licenseFileName);

                var licenseText = "Random licenseFile";
                File.WriteAllText(licenseFile, licenseText);
                Assert.True(File.Exists(licenseFile));

                // Setup LicenseFile
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseFile", licenseFileName);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}", validateSuccess: false);

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.False(File.Exists(nupkgPath));
                Assert.True(File.Exists(nuspecPath)); // See https://github.com/NuGet/Home/issues/7348. This needs to be fixed.
                Assert.False(result.Success);
                Assert.True(result.Output.Contains("NU5030"));
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackLicense_PackBasicLicenseFile_FileIncorrectCasing()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                // Set up
                var realLicenseFileName = "LICENSE.txt";
                var nuspecLicenseFileName = "License.txt";
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                var licenseFile = Path.Combine(workingDirectory, realLicenseFileName);

                var licenseText = "Random licenseFile";
                File.WriteAllText(licenseFile, licenseText);
                Assert.True(File.Exists(licenseFile));

                // Setup LicenseFile
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseFile", nuspecLicenseFileName);

                    var attributes = new Dictionary<string, string>();
                    attributes["Pack"] = "true";
                    attributes["PackagePath"] = realLicenseFileName;
                    var properties = new Dictionary<string, string>();
                    ProjectFileUtils.AddItem(
                        xml,
                        "None",
                        realLicenseFileName,
                        NuGetFramework.AnyFramework,
                        properties,
                        attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}", validateSuccess: false);

                Assert.False(result.Success);
                Assert.Contains(NuGetLogCode.NU5030.ToString(), result.Output);
                Assert.Contains($"'{realLicenseFileName}'", result.Output, StringComparison.Ordinal); // Check for "Did you mean 'LICENSE.txt'?"
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackLicense_PackBasicLicenseFile_FileExtensionNotValid()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var licenseFileName = "LICENSE.badextension";
                // Set up
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                var licenseFile = Path.Combine(workingDirectory, licenseFileName);

                var licenseText = "Random licenseFile";
                File.WriteAllText(licenseFile, licenseText);
                Assert.True(File.Exists(licenseFile));

                // Setup LicenseFile
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseFile", licenseFileName);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}", validateSuccess: false);

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.False(File.Exists(nupkgPath));
                Assert.True(File.Exists(nuspecPath)); // See https://github.com/NuGet/Home/issues/7348. This needs to be fixed.
                Assert.False(result.Success);
                Assert.True(result.Output.Contains("NU5031"));
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackLicense_BothLicenseExpressionAndFile_FailsWithErrorCode()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                // Set up
                var projectName = "ClassLibrary1";
                var licenseExpr = "MIT";
                var licenseFile = "LICENSE.txt";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                // Setup LicenseExpression
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseExpression", licenseExpr);
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseFile", licenseFile);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}", validateSuccess: false);

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.False(File.Exists(nupkgPath));
                Assert.False(File.Exists(nuspecPath));

                Assert.False(result.Success);
                Assert.True(result.AllOutput.Contains("NU5033"));
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackLicense_LicenseUrlIsBeingDeprecated()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                // Set up
                var projectName = "ClassLibrary1";
                var projectUrl = new Uri("https://www.coolproject.com/license.txt");
                var workingDirectory = Path.Combine(testDirectory, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                // Setup LicenseExpression
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseUrl", projectUrl.ToString());
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}", validateSuccess: false);

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath));
                Assert.True(File.Exists(nuspecPath));

                Assert.True(result.AllOutput.Contains("NU5125"));

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the output .nuspec.
                    Assert.Equal("ClassLibrary1", nuspecReader.GetId());
                    Assert.Equal("1.0.0", nuspecReader.GetVersion().ToFullString());
                    Assert.False(nuspecReader.GetRequireLicenseAcceptance());
                    Assert.Equal(projectUrl, new Uri(nuspecReader.GetLicenseUrl()));
                    var licenseMetadata = nuspecReader.GetLicenseMetadata();
                    Assert.Null(licenseMetadata);
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackLicense_IncludeLicenseFileWithSnupkg()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                // Set up
                var projectName = "ClassLibrary1";
                var licenseFileName = "LICENSE.txt";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib -f netstandard2.0");
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                var licenseFile = Path.Combine(workingDirectory, licenseFileName);
                File.WriteAllText(licenseFile, "Random licenseFile");
                Assert.True(File.Exists(licenseFile));

                // Setup LicenseFile
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseFile", licenseFileName);

                    var attributes = new Dictionary<string, string>();
                    attributes["Pack"] = "true";
                    attributes["PackagePath"] = licenseFileName;
                    var properties = new Dictionary<string, string>();
                    ProjectFileUtils.AddItem(
                        xml,
                        "None",
                        licenseFileName,
                        NuGetFramework.AnyFramework,
                        properties,
                        attributes);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");
                msbuildFixture.PackProject(workingDirectory, projectName, $"--include-symbols /p:SymbolPackageFormat=snupkg -o {workingDirectory}");


                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                var symbolPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.snupkg");

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");
                Assert.True(File.Exists(symbolPath), "The output .snupkg is not in the expected place");

                Assert.True(result.Success);

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the output .nuspec.
                    Assert.Equal("ClassLibrary1", nuspecReader.GetId());
                    Assert.Equal("1.0.0", nuspecReader.GetVersion().ToFullString());
                    Assert.False(nuspecReader.GetRequireLicenseAcceptance());
                    Assert.Equal(LicenseMetadata.LicenseFileDeprecationUrl, new Uri(nuspecReader.GetLicenseUrl()));
                    var licenseMetadata = nuspecReader.GetLicenseMetadata();
                    Assert.NotNull(licenseMetadata);
                    Assert.Equal(LicenseMetadata.LicenseFileDeprecationUrl, licenseMetadata.LicenseUrl);
                    Assert.Equal(licenseMetadata.Type, LicenseType.File);
                    Assert.Equal(licenseMetadata.Version, LicenseMetadata.EmptyVersion);
                    Assert.Equal(licenseMetadata.License, licenseFileName);
                    Assert.Null(licenseMetadata.LicenseExpression);
                }

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                using (var symbolReader = new PackageArchiveReader(symbolPath))
                {
                    // Validate the assets.
                    Assert.False(symbolReader.NuspecReader.GetRequireLicenseAcceptance());
                    Assert.Null(symbolReader.NuspecReader.GetLicenseMetadata());
                    Assert.Null(symbolReader.NuspecReader.GetLicenseUrl());
                    var libItems = nupkgReader.GetLibItems().ToList();
                    var libSymItems = symbolReader.GetLibItems().ToList();
                    Assert.Equal(1, libItems.Count);
                    Assert.Equal(1, libSymItems.Count);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard20, libItems[0].TargetFramework);
                    Assert.Equal(new[] { "lib/netstandard2.0/ClassLibrary1.dll" }, libItems[0].Items);
                    Assert.Equal(new[] { "lib/netstandard2.0/ClassLibrary1.pdb" }, libSymItems[0].Items);
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackLicense_IncludeLicenseFileWithSymbolsNupkg()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                // Set up
                var projectName = "ClassLibrary1";
                var licenseFileName = "LICENSE.txt";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib -f netstandard2.0");
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                var licenseFile = Path.Combine(workingDirectory, licenseFileName);
                File.WriteAllText(licenseFile, "Random licenseFile");
                Assert.True(File.Exists(licenseFile));

                // Setup LicenseFile
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseFile", licenseFileName);

                    var attributes = new Dictionary<string, string>();
                    attributes["Pack"] = "true";
                    attributes["PackagePath"] = licenseFileName;
                    var properties = new Dictionary<string, string>();
                    ProjectFileUtils.AddItem(
                        xml,
                        "None",
                        licenseFileName,
                        NuGetFramework.AnyFramework,
                        properties,
                        attributes);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");
                msbuildFixture.PackProject(workingDirectory, projectName, $"--include-symbols /p:SymbolPackageFormat=symbols.nupkg -o {workingDirectory}");


                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                var symbolPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.symbols.nupkg");

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");
                Assert.True(File.Exists(symbolPath), "The output .snupkg is not in the expected place");

                Assert.True(result.Success);

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the output .nuspec.
                    Assert.Equal("ClassLibrary1", nuspecReader.GetId());
                    Assert.Equal("1.0.0", nuspecReader.GetVersion().ToFullString());
                    Assert.False(nuspecReader.GetRequireLicenseAcceptance());
                    Assert.Equal(LicenseMetadata.LicenseFileDeprecationUrl, new Uri(nuspecReader.GetLicenseUrl()));
                    var licenseMetadata = nuspecReader.GetLicenseMetadata();
                    Assert.NotNull(licenseMetadata);
                    Assert.Equal(LicenseMetadata.LicenseFileDeprecationUrl, licenseMetadata.LicenseUrl);
                    Assert.Equal(licenseMetadata.Type, LicenseType.File);
                    Assert.Equal(licenseMetadata.Version, LicenseMetadata.EmptyVersion);
                    Assert.Equal(licenseMetadata.License, licenseFileName);
                    Assert.Null(licenseMetadata.LicenseExpression);
                }

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                using (var symbolReader = new PackageArchiveReader(symbolPath))
                {
                    // Validate the assets.
                    var libItems = nupkgReader.GetLibItems().ToList();
                    var libSymItems = symbolReader.GetLibItems().ToList();
                    Assert.Equal(1, libItems.Count);
                    Assert.Equal(1, libSymItems.Count);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard20, libItems[0].TargetFramework);
                    Assert.Equal(new[] { "lib/netstandard2.0/ClassLibrary1.dll" }, libItems[0].Items);
                    Assert.Equal(new[] { "lib/netstandard2.0/ClassLibrary1.dll", "lib/netstandard2.0/ClassLibrary1.pdb" }, libSymItems[0].Items);
                    Assert.True(symbolReader.GetEntry(symbolReader.NuspecReader.GetLicenseMetadata().License) != null);
                }
            }
        }
        [PlatformTheory(Platform.Windows)]
        [InlineData("PackageLicenseExpression")]
        [InlineData("PackageLicenseFile")]
        public void PackCommand_PackLicense_LicenseExpressionAndLicenseUrlInConjunction(string licenseType)
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                // Set up
                var projectName = "ClassLibrary1";
                var licenseExpr = "MIT";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                // Setup LicenseExpression
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, licenseType, licenseExpr);
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseUrl", "https://www.mycoolproject.org/license.txt");
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}", validateSuccess: false);

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.False(File.Exists(nupkgPath));
                Assert.False(File.Exists(nuspecPath));

                Assert.False(result.Success);
                Assert.True(result.AllOutput.Contains("NU5035"));
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackEmbedInteropPackage()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib -f netstandard2.0");
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                // Setup BuildOutputTargetFolder
                var buildTargetFolders = "lib;embed";
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "BuildOutputTargetFolder", buildTargetFolders);
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                // Act
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    // Validate Compile assets
                    foreach (var buildTargetFolder in buildTargetFolders.Split(';'))
                    {
                        var compileItems = nupkgReader.GetFiles(buildTargetFolder).ToList();
                        Assert.Equal(1, compileItems.Count);
                        Assert.Equal(buildTargetFolder + "/netstandard2.0/ClassLibrary1.dll", compileItems[0]);
                    }
                }
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("Microsoft.NETCore.App", "true", "netcoreapp3.0", "", "netcoreapp3.0")]
        [InlineData("Microsoft.NETCore.App", "false", "netcoreapp3.0", "", "")]
        [InlineData("Microsoft.WindowsDesktop.App", "true", "netstandard2.1;netcoreapp3.0", "netcoreapp3.0", "netcoreapp3.0")]
        [InlineData("Microsoft.WindowsDesktop.App;Microsoft.AspNetCore.App", "true;true", "netcoreapp3.0", "netcoreapp3.0", "netcoreapp3.0")]
        [InlineData("Microsoft.WindowsDesktop.App.WPF;Microsoft.WindowsDesktop.App.WindowsForms", "true;false", "netcoreapp3.0", "", "netcoreapp3.0")]
        public void PackCommand_PackProject_PacksFrameworkReferences(string frameworkReferences, string packForFrameworkRefs, string targetFrameworks, string conditionalFramework, string expectedTargetFramework)
        {
            // Arrange
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);

                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                var frameworkReftoPack = new Dictionary<string, bool>();
                var frameworkRefs = frameworkReferences.Split(";");
                var pack = packForFrameworkRefs.Split(";").Select(e => bool.Parse(e)).ToArray();
                Assert.Equal(frameworkRefs.Length, pack.Length);
                for (var i = 0; i < frameworkRefs.Length; i++)
                {
                    frameworkReftoPack.Add(frameworkRefs[i], pack[i]);
                }

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    var frameworkProperty = "TargetFrameworks";
                    if (targetFrameworks.Split(';').Count() == 1)
                    {
                        frameworkProperty = "TargetFramework";
                    }
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, frameworkProperty, targetFrameworks);

                    foreach (var frameworkRef in frameworkReftoPack)
                    {
                        var attributes = new Dictionary<string, string>();

                        var properties = new Dictionary<string, string>();
                        if (!frameworkRef.Value)
                        {
                            attributes["PrivateAssets"] = "all";
                        }
                        ProjectFileUtils.AddItem(
                            xml,
                            "FrameworkReference",
                            frameworkRef.Key,
                            conditionalFramework,
                            properties,
                            attributes);
                    }


                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                // Act
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}");

                // Assert
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var expectedFrameworks = expectedTargetFramework.Split(';').Where(fw => !string.IsNullOrEmpty(fw));
                    var allFrameworks = targetFrameworks.Split(';').Where(fw => !string.IsNullOrEmpty(fw));
                    var nupkgFrameworkGroups = nupkgReader.NuspecReader.GetFrameworkRefGroups();

                    if (expectedFrameworks.Any())
                    {
                        Assert.Equal(
                            allFrameworks.Select(fw => NuGetFramework.Parse(fw)).ToHashSet(),
                            nupkgFrameworkGroups.Select(t => t.TargetFramework).ToHashSet()
                        );
                    }
                    else
                    {
                        Assert.Equal(
                            new HashSet<NuGetFramework>(),
                            nupkgFrameworkGroups.Select(t => t.TargetFramework).ToHashSet()
                        );
                    }

                    foreach (var framework in expectedFrameworks)
                    {
                        var nugetFramework = NuGetFramework.Parse(framework);
                        var frameworkSpecificGroup = nupkgFrameworkGroups.Where(t => t.TargetFramework.Equals(nugetFramework)).FirstOrDefault();

                        foreach (var frameworkRef in frameworkReftoPack)
                        {
                            if (frameworkRef.Value)
                            {
                                Assert.True(frameworkSpecificGroup?.FrameworkReferences.Contains(new FrameworkReference(frameworkRef.Key)));
                            }
                            else
                            {
                                Assert.False(frameworkSpecificGroup == null ? false : frameworkSpecificGroup.FrameworkReferences.Select(e => e.Name).Contains(frameworkRef.Key));
                            }
                        }
                    }
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_WithGeneratePackageOnBuildSet_CanPublish()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " console");

                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "GeneratePackageOnBuild", "true");
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }
                // Act
                var result = msbuildFixture.RunDotnet(workingDirectory, $"publish {projectFile}");

                // Assert
                Assert.True(result.Success);
            }
        }

        [PlatformFact(Platform.Windows, Skip = "https://github.com/NuGet/Home/issues/8601")]
        public void PackCommand_Deterministic_MultiplePackInvocations_CreateIdenticalPackages()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.AddProperty(xml, "Deterministic", "true");
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                // Act
                byte[][] packageBytes = new byte[2][];

                for (var i = 0; i < 2; i++)
                {
                    var packageOutputPath = Path.Combine(workingDirectory, i.ToString());
                    var nupkgPath = Path.Combine(packageOutputPath, $"{projectName}.1.0.0.nupkg");
                    var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");

                    // Act
                    msbuildFixture.PackProject(workingDirectory, projectName, $"-o {packageOutputPath}");

                    Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                    Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                    using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                    {
                        var nuspecReader = nupkgReader.NuspecReader;

                        // Validate the output .nuspec.
                        Assert.Equal("ClassLibrary1", nuspecReader.GetId());
                        Assert.Equal("1.0.0", nuspecReader.GetVersion().ToFullString());
                    }

                    using (var reader = new FileStream(nupkgPath, FileMode.Open))
                    using (var ms = new MemoryStream())
                    {
                        reader.CopyTo(ms);
                        packageBytes[i] = ms.ToArray();
                    }
                }
                // Assert
                Assert.Equal(packageBytes[0], packageBytes[1]);
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackageIcon_HappyPath_Warns_Succeeds()
        {
            var testDirBuilder = TestDirectoryBuilder.Create();
            var projectBuilder = ProjectFileBuilder.Create();

            testDirBuilder
                .WithFile("test\\folder\\notes.txt", 10)
                .WithFile("test\\folder\\nested\\content.txt", 10)
                .WithFile("test\\folder\\nested\\sample.txt", 10)
                .WithFile("test\\folder\\nested\\media\\readme.txt", 10)
                .WithFile("test\\icon.jpg", 10)
                .WithFile("test\\other\\files.txt", 10)
                .WithFile("test\\utils\\sources.txt", 10);

            projectBuilder
                .WithProjectName("test")
                .WithPackageIcon("icon.jpg")
                .WithPackageIconUrl("http://test.icon")
                .WithItem(itemType: "None", itemPath: "icon.jpg", packagePath: string.Empty, pack: "true")
                .WithItem(itemType: "None", itemPath: "other\\files.txt", packagePath: null, pack: "true")
                .WithItem(itemType: "None", itemPath: "folder\\**", packagePath: "media", pack: "true")
                .WithItem(itemType: "None", itemPath: "utils\\*", packagePath: "utils", pack: "true");

            using (var srcDir = msbuildFixture.Build(testDirBuilder))
            {
                projectBuilder.Build(msbuildFixture, srcDir.Path);
                var result = msbuildFixture.PackProject(projectBuilder.ProjectFolder, projectBuilder.ProjectName, string.Empty);

                // Validate embedded icon in package
                ValidatePackIcon(projectBuilder);

                // Validate that other content is also included
                var nupkgPath = Path.Combine(projectBuilder.ProjectFolder, "bin", "Debug", $"{projectBuilder.ProjectName}.1.0.0.nupkg");
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    Assert.NotNull(nupkgReader.GetEntry("content/other/files.txt"));
                    Assert.NotNull(nupkgReader.GetEntry("utils/sources.txt"));
                    Assert.NotNull(nupkgReader.GetEntry("media/nested/sample.txt"));
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackageIcon_MissingFile_Fails()
        {
            var testDirBuilder = TestDirectoryBuilder.Create();
            var projectBuilder = ProjectFileBuilder.Create();

            projectBuilder
                .WithProjectName("test")
                .WithPackageIcon("icon.jpg");

            using (var srcDir = msbuildFixture.Build(testDirBuilder))
            {
                projectBuilder.Build(msbuildFixture, srcDir.Path);
                var result = msbuildFixture.PackProject(projectBuilder.ProjectFolder, projectBuilder.ProjectName, string.Empty, validateSuccess: false);

                Assert.NotEqual(0, result.ExitCode);
                Assert.Contains(NuGetLogCode.NU5046.ToString(), result.Output);
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackageIcon_IncorrectCasing_Fails()
        {
            var testDirBuilder = TestDirectoryBuilder.Create();
            var projectBuilder = ProjectFileBuilder.Create();

            testDirBuilder
                .WithFile("test\\icon.jpg", 10);

            projectBuilder
                .WithProjectName("test")
                .WithPackageIcon("ICON.JPG")
                .WithItem(itemType: "None", itemPath: "icon.jpg", packagePath: "icon.jpg", pack: "true");

            using (var srcDir = msbuildFixture.Build(testDirBuilder))
            {
                projectBuilder.Build(msbuildFixture, srcDir.Path);
                var result = msbuildFixture.PackProject(projectBuilder.ProjectFolder, projectBuilder.ProjectName, string.Empty, validateSuccess: false);

                Assert.NotEqual(0, result.ExitCode);
                Assert.Contains(NuGetLogCode.NU5046.ToString(), result.Output);
                Assert.Contains("'icon.jpg'", result.Output, StringComparison.Ordinal); // Check for "Did you mean 'icon.jpg'?"
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackageIcon_IncorrectFolderCasing_Fails()
        {
            var testDirBuilder = TestDirectoryBuilder.Create();
            var projectBuilder = ProjectFileBuilder.Create();

            testDirBuilder
                .WithFile("test\\icon.jpg", 10);

            projectBuilder
                .WithProjectName("test")
                .WithPackageIcon("FOLDER\\icon.jpg")
                .WithItem(itemType: "None", itemPath: "icon.jpg", packagePath: "folder\\icon.jpg", pack: "true");

            using (var srcDir = msbuildFixture.Build(testDirBuilder))
            {
                projectBuilder.Build(msbuildFixture, srcDir.Path);
                var result = msbuildFixture.PackProject(projectBuilder.ProjectFolder, projectBuilder.ProjectName, string.Empty, validateSuccess: false);

                Assert.NotEqual(0, result.ExitCode);
                Assert.Contains(NuGetLogCode.NU5046.ToString(), result.Output);
                Assert.Contains("'folder/icon.jpg'", result.Output, StringComparison.Ordinal); // Check for "Did you mean 'folder/icon.jpg'?"
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("snupkg")]
        [InlineData("symbols.nupkg")]
        public void PackCommand_PackageIcon_PackWithSymbols_Succeeds(string symbolPackageFormat)
        {
            var testDirBuilder = TestDirectoryBuilder.Create();
            var projectBuilder = ProjectFileBuilder.Create();

            testDirBuilder
                .WithFile("test\\icon.jpg", 10);

            projectBuilder
                .WithProjectName("test")
                .WithPackageIcon("icon.jpg")
                .WithItem(itemType: "None", itemPath: "icon.jpg", packagePath: "icon.jpg", pack: "true");

            using (var srcDir = msbuildFixture.Build(testDirBuilder))
            {
                projectBuilder.Build(msbuildFixture, srcDir.Path);
                var result = msbuildFixture.PackProject(
                    projectBuilder.ProjectFolder,
                    projectBuilder.ProjectName,
                    $"--include-symbols /p:SymbolPackageFormat={symbolPackageFormat}");
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackIcon_WithNuspec_IconUrl_Warns_Succeeds()
        {
            var testDirBuilder = TestDirectoryBuilder.Create();
            var projectBuilder = ProjectFileBuilder.Create();
            var nuspecBuilder = NuspecBuilder.Create();

            nuspecBuilder
                .WithIconUrl("https://test/icon2.jpg")
                .WithFile("dummy.txt");

            projectBuilder
                .WithProjectName("test")
                .WithProperty("Authors", "Alice")
                .WithProperty("NuspecFile", "test.nuspec")
                .WithPackageIconUrl("https://test/icon.jpg");

            testDirBuilder
                .WithNuspec(nuspecBuilder, "test\\test.nuspec")
                .WithFile("test\\dummy.txt", 10);

            using (var srcDir = msbuildFixture.Build(testDirBuilder))
            {
                projectBuilder.Build(msbuildFixture, srcDir.Path);
                var result = msbuildFixture.PackProject(projectBuilder.ProjectFolder, projectBuilder.ProjectName, string.Empty);

                Assert.Contains(NuGetLogCode.NU5048.ToString(), result.Output);
                Assert.Contains("iconUrl", result.Output);
                Assert.Contains("PackageIconUrl", result.Output);
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_WhenUsingSemver2Version_NU5105_IsNotRaised()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                Directory.CreateDirectory(workingDirectory);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);

                    ProjectFileUtils.AddProperty(xml, "PackageVersion", "1.0.0+mySpecialSemver2Metadata");
                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");

                var result = msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");

                Assert.True(File.Exists(nupkgPath), $"The output .nupkg is not in the expected place. {result.AllOutput}");
                Assert.True(File.Exists(nuspecPath), $"The intermediate nuspec file is not in the expected place. {result.AllOutput}");
                result.AllOutput.Should().NotContain(NuGetLogCode.NU5105.ToString());
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("false")]
        [InlineData("true")]
        [InlineData(null)]
        public void PackCommand_PackProjectWithCentralTransitiveDependencies(string CentralPackageTransitivePinningEnabled)
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                // Act
                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib -f netstandard2.0", 60000);

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);

                    ProjectFileUtils.AddItem(
                        xml,
                        "PackageReference",
                        "Moq",
                        string.Empty,
                        new Dictionary<string, string>(),
                        new Dictionary<string, string>());

                    ProjectFileUtils.AddProperty(
                        xml,
                        ProjectBuildProperties.ManagePackageVersionsCentrally,
                        "true");

                    if (CentralPackageTransitivePinningEnabled != null)
                    {
                        ProjectFileUtils.AddProperty(
                            xml,
                            ProjectBuildProperties.CentralPackageTransitivePinningEnabled,
                            CentralPackageTransitivePinningEnabled);
                    }

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                // The test depends on the presence of these packages and their versions.
                // Change to Directory.Packages.props when new cli that supports NuGet.props will be downloaded
                var directoryPackagesPropsName = Path.Combine(workingDirectory, $"Directory.Build.props");
                var directoryPackagesPropsContent = @"<Project>
                        <ItemGroup>
                            <PackageVersion Include = ""Moq"" Version = ""4.10.0""/>
                            <PackageVersion Include = ""Castle.Core"" Version = ""4.4.0""/>
                        </ItemGroup>
                        <PropertyGroup>
	                        <CentralPackageVersionsFileImported>true</CentralPackageVersionsFileImported>
                        </PropertyGroup>
                    </Project>";
                File.WriteAllText(directoryPackagesPropsName, directoryPackagesPropsContent);

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName, $"-o {workingDirectory}", "obj", false);

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    // Validate the output .nuspec.
                    var dependencyGroups = nuspecReader.GetDependencyGroups().ToList();
                    Assert.Equal(1, dependencyGroups.Count);
                    Assert.Equal(FrameworkConstants.CommonFrameworks.NetStandard20, dependencyGroups[0].TargetFramework);
                    var packages = dependencyGroups[0].Packages.ToList();
                    if (CentralPackageTransitivePinningEnabled == "true")
                    {
                        Assert.Equal(2, packages.Count);
                        var moqPackage = packages.Where(p => p.Id.Equals("Moq", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        var castleCorePackage = packages.Where(p => p.Id.Equals("Castle.Core", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        Assert.NotNull(moqPackage);
                        Assert.NotNull(castleCorePackage);
                        Assert.Equal("4.10.0", moqPackage.VersionRange.ToShortString());
                        Assert.Equal("4.4.0", castleCorePackage.VersionRange.ToShortString());
                    }
                    else
                    {
                        Assert.Equal(1, packages.Count);
                        var moqPackage = packages.Single();
                        Assert.Equal(moqPackage.Id, "Moq");
                        Assert.Equal("4.10.0", moqPackage.VersionRange.ToShortString());
                    }
                }
            }
        }

        private void ValidatePackIcon(ProjectFileBuilder projectBuilder)
        {
            Assert.True(File.Exists(projectBuilder.ProjectFilePath), "No project was produced");
            var nupkgPath = Path.Combine(projectBuilder.ProjectFolder, "bin", "Debug", $"{projectBuilder.ProjectName}.1.0.0.nupkg");

            Assert.True(File.Exists(nupkgPath), "No package was produced");

            using (var nupkgReader = new PackageArchiveReader(nupkgPath))
            {
                var nuspecReader = nupkgReader.NuspecReader;

                Assert.Equal(projectBuilder.PackageIcon, nuspecReader.GetIcon());
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackageReadmeFile_BasicFunc_Succeeds()
        {
            var testDirBuilder = TestDirectoryBuilder.Create();
            var projectBuilder = ProjectFileBuilder.Create();

            testDirBuilder
                .WithFile("test\\readme.md", 10)
                .WithFile("test\\folder\\notes.txt", 10)
                .WithFile("test\\folder\\nested\\content.txt", 10)
                .WithFile("test\\folder\\nested\\sample.txt", 10)
                .WithFile("test\\icon.jpg", 10)
                .WithFile("test\\other\\files.txt", 10)
                .WithFile("test\\utils\\sources.txt", 10);

            projectBuilder
                .WithProjectName("test")
                .WithPackageReadmeFile("readme.md")
                .WithItem(itemType: "None", itemPath: "readme.md", packagePath: string.Empty, pack: "true")
                .WithItem(itemType: "None", itemPath: "other\\files.txt", packagePath: null, pack: "true")
                .WithItem(itemType: "None", itemPath: "folder\\**", packagePath: "media", pack: "true")
                .WithItem(itemType: "None", itemPath: "utils\\*", packagePath: "utils", pack: "true");

            using (var srcDir = msbuildFixture.Build(testDirBuilder))
            {
                projectBuilder.Build(msbuildFixture, srcDir.Path);
                var result = msbuildFixture.PackProject(projectBuilder.ProjectFolder, projectBuilder.ProjectName, string.Empty);

                // Validate embedded readme in package
                ValidatePackReadme(projectBuilder);

                // Validate that other content is also included
                var nupkgPath = Path.Combine(projectBuilder.ProjectFolder, "bin", "Debug", $"{projectBuilder.ProjectName}.1.0.0.nupkg");
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    Assert.NotNull(nupkgReader.GetEntry("content/other/files.txt"));
                    Assert.NotNull(nupkgReader.GetEntry("utils/sources.txt"));
                    Assert.NotNull(nupkgReader.GetEntry("media/nested/sample.txt"));
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackageReadmeFile_MissingReadmeFileInPackage_Fails()
        {
            var testDirBuilder = TestDirectoryBuilder.Create();
            var projectBuilder = ProjectFileBuilder.Create();

            testDirBuilder
                .WithFile("test\\readme.md", 10);

            projectBuilder
                .WithProjectName("test")
                .WithPackageReadmeFile("readme.md");

            using (var srcDir = msbuildFixture.Build(testDirBuilder))
            {
                projectBuilder.Build(msbuildFixture, srcDir.Path);
                var result = msbuildFixture.PackProject(projectBuilder.ProjectFolder, projectBuilder.ProjectName, string.Empty, validateSuccess: false);

                Assert.NotEqual(0, result.ExitCode);
                Assert.Contains(NuGetLogCode.NU5039.ToString(), result.Output);
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackageReadmeFile_MissingReadmeFileInFolder_Fails()
        {
            var testDirBuilder = TestDirectoryBuilder.Create();
            var projectBuilder = ProjectFileBuilder.Create();

            projectBuilder
                .WithProjectName("test")
                .WithPackageReadmeFile("readme.md")
                .WithItem(itemType: "None", itemPath: "readme.md", packagePath: string.Empty, pack: "true");

            using (var srcDir = msbuildFixture.Build(testDirBuilder))
            {
                projectBuilder.Build(msbuildFixture, srcDir.Path);
                var result = msbuildFixture.PackProject(projectBuilder.ProjectFolder, projectBuilder.ProjectName, string.Empty, validateSuccess: false);

                Assert.NotEqual(0, result.ExitCode);
                Assert.Contains(NuGetLogCode.NU5019.ToString(), result.Output);
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackageReadmeFile_IncorrectReadmeExtension_Fails()
        {
            var testDirBuilder = TestDirectoryBuilder.Create();
            var projectBuilder = ProjectFileBuilder.Create();

            testDirBuilder
                .WithFile("test\\readme.txt", 10);

            projectBuilder
                .WithProjectName("test")
                .WithPackageReadmeFile("readme.txt")
                .WithItem(itemType: "None", itemPath: "readme.txt", packagePath: string.Empty, pack: "true");

            using (var srcDir = msbuildFixture.Build(testDirBuilder))
            {
                projectBuilder.Build(msbuildFixture, srcDir.Path);
                var result = msbuildFixture.PackProject(projectBuilder.ProjectFolder, projectBuilder.ProjectName, string.Empty, validateSuccess: false);

                Assert.NotEqual(0, result.ExitCode);
                Assert.Contains(NuGetLogCode.NU5038.ToString(), result.Output);
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackageReadmeFile_ReadmeFileIsEmpty_Fails()
        {
            var testDirBuilder = TestDirectoryBuilder.Create();
            var projectBuilder = ProjectFileBuilder.Create();

            testDirBuilder
                .WithFile("test\\readme.md", 0);

            projectBuilder
                .WithProjectName("test")
                .WithPackageReadmeFile("readme.md")
                .WithItem(itemType: "None", itemPath: "readme.md", packagePath: string.Empty, pack: "true");

            using (var srcDir = msbuildFixture.Build(testDirBuilder))
            {
                projectBuilder.Build(msbuildFixture, srcDir.Path);
                var result = msbuildFixture.PackProject(projectBuilder.ProjectFolder, projectBuilder.ProjectName, string.Empty, validateSuccess: false);

                Assert.NotEqual(0, result.ExitCode);
                Assert.Contains(NuGetLogCode.NU5040.ToString(), result.Output);
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_PackageReadmeFile_BasicFunc_WithSymbol_Succeeds()
        {
            // Arrange
            TestDirectoryBuilder testDirBuilder = TestDirectoryBuilder.Create();
            ProjectFileBuilder projectBuilder = ProjectFileBuilder.Create();

            testDirBuilder
                .WithFile("test\\readme.md", 10)
                .WithFile("test\\folder\\notes.txt", 10)
                .WithFile("test\\folder\\nested\\content.txt", 10)
                .WithFile("test\\folder\\nested\\sample.txt", 10)
                .WithFile("test\\icon.jpg", 10)
                .WithFile("test\\other\\files.txt", 10)
                .WithFile("test\\utils\\sources.txt", 10);

            projectBuilder
                .WithProjectName("test")
                .WithProperty("IncludeSymbols", "true")
                .WithProperty("SymbolPackageFormat", "snupkg")
                .WithPackageReadmeFile("readme.md")
                .WithItem(itemType: "None", itemPath: "readme.md", packagePath: string.Empty, pack: "true")
                .WithItem(itemType: "None", itemPath: "other\\files.txt", packagePath: null, pack: "true")
                .WithItem(itemType: "None", itemPath: "folder\\**", packagePath: "media", pack: "true")
                .WithItem(itemType: "None", itemPath: "utils\\*", packagePath: "utils", pack: "true");

            // Act
            using (TestDirectory srcDir = msbuildFixture.Build(testDirBuilder))
            {
                projectBuilder.Build(msbuildFixture, srcDir.Path);
                msbuildFixture.PackProject(projectBuilder.ProjectFolder, projectBuilder.ProjectName, string.Empty);

                // Assert
                // Validate embedded readme in package
                ValidatePackReadme(projectBuilder);

                // Validate that other content is also included
                string nupkgPath = Path.Combine(projectBuilder.ProjectFolder, "bin", "Debug", $"{projectBuilder.ProjectName}.1.0.0.nupkg");
                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    Assert.NotNull(nupkgReader.GetEntry("content/other/files.txt"));
                    Assert.NotNull(nupkgReader.GetEntry("utils/sources.txt"));
                    Assert.NotNull(nupkgReader.GetEntry("media/nested/sample.txt"));
                }

                string snupkgPath = Path.Combine(projectBuilder.ProjectFolder, "bin", "Debug", $"{projectBuilder.ProjectName}.1.0.0.snupkg");
                Assert.True(File.Exists(snupkgPath), "No snupkg was produced");
            }
        }

        private void ValidatePackReadme(ProjectFileBuilder projectBuilder)
        {
            Assert.True(File.Exists(projectBuilder.ProjectFilePath), "No project was produced");
            var nupkgPath = Path.Combine(projectBuilder.ProjectFolder, "bin", "Debug", $"{projectBuilder.ProjectName}.1.0.0.nupkg");

            Assert.True(File.Exists(nupkgPath), "No package was produced");

            using (var nupkgReader = new PackageArchiveReader(nupkgPath))
            {
                var nuspecReader = nupkgReader.NuspecReader;

                Assert.Equal(projectBuilder.PackageReadmeFile, nuspecReader.GetReadme());
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_DoesNotGenerateOwnersElement()
        {
            using (var testDirectory = msbuildFixture.CreateTestDirectory())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFramework", "netstandard1.4");

                    ProjectFileUtils.AddProperty(xml, "Authors", "Some authors");

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                var document = XDocument.Load(nuspecPath);
                var ns = document.Root.GetDefaultNamespace();

                Assert.Null(document.Root.Element(ns + "metadata").Element(ns + "owners"));
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_RequireLicenseAcceptanceNotEmittedWhenUnspecified()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                XDocument xml;
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFramework", "netstandard1.4");

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                // Test without a license
                msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                var document = XDocument.Load(nuspecPath);
                var ns = document.Root.GetDefaultNamespace();

                Assert.Null(document.Root.Element(ns + "metadata").Element(ns + "requireLicenseAcceptance"));

                // Test with a license
                ProjectFileUtils.AddProperty(xml, "PackageLicenseExpression", "MIT");

                using (var stream = File.Create(projectFile))
                    ProjectFileUtils.WriteXmlToFile(xml, stream);

                msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");
                document = XDocument.Load(nuspecPath);

                Assert.Null(document.Root.Element(ns + "metadata").Element(ns + "requireLicenseAcceptance"));
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_RequireLicenseAcceptanceNotEmittedWhenSpecifiedAsDefault()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                XDocument xml;
                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFramework", "netstandard1.4");

                    ProjectFileUtils.AddProperty(xml, "PackageRequireLicenseAcceptance", "false");

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);

                // Test without a license
                msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                var document = XDocument.Load(nuspecPath);
                var ns = document.Root.GetDefaultNamespace();

                Assert.Null(document.Root.Element(ns + "metadata").Element(ns + "requireLicenseAcceptance"));

                // Test with a license
                ProjectFileUtils.AddProperty(xml, "PackageLicenseExpression", "MIT");

                using (var stream = File.Create(projectFile))
                    ProjectFileUtils.WriteXmlToFile(xml, stream);

                msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");
                document = XDocument.Load(nuspecPath);

                Assert.Null(document.Root.Element(ns + "metadata").Element(ns + "requireLicenseAcceptance"));
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_RequireLicenseAcceptanceEmittedWhenSpecifiedAsTrue()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");

                msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

                using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);
                    ProjectFileUtils.SetTargetFrameworkForProject(xml, "TargetFramework", "netstandard1.4");

                    ProjectFileUtils.AddProperty(xml, "PackageRequireLicenseAcceptance", "true");
                    ProjectFileUtils.AddProperty(xml, "PackageLicenseExpression", "MIT");

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }

                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");

                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec");

                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                Assert.True(File.Exists(nuspecPath), "The intermediate nuspec file is not in the expected place");

                var document = XDocument.Load(nuspecPath);
                var ns = document.Root.GetDefaultNamespace();

                Assert.Equal(document.Root.Element(ns + "metadata").Element(ns + "requireLicenseAcceptance").Value, "true");
            }
        }

        [PlatformTheory(Platform.Windows)]
        [InlineData("wpf")]
        [InlineData("wpflib")]
        [InlineData("wpfcustomcontrollib")]
        [InlineData("wpfusercontrollib")]
        [InlineData("winforms")]
        [InlineData("winformscontrollib")]
        [InlineData("winformslib")]
        [InlineData("razorclasslib")]
        public void Dotnet_New_Template_Restore_Pack_Success(string template)
        {
            // Arrange
            using (SimpleTestPathContext pathContext = msbuildFixture.CreateSimpleTestPathContext())
            {
                var projectName = "ClassLibrary1";
                string workDirectory = pathContext.SolutionRoot;
                string projectFile = Path.Combine(workDirectory, $"{projectName}.csproj");
                string solutionDirectory = Path.Combine(pathContext.SolutionRoot, projectName);
                string nupkgPath = Path.Combine(solutionDirectory, "bin", "Debug", $"{projectName}.1.0.0.nupkg");

                // Act
                msbuildFixture.CreateDotnetNewProject(workDirectory, projectName, template);
                msbuildFixture.PackProject(solutionDirectory, projectName, string.Empty, null);

                // Assert
                // Make sure restore action was success.
                Assert.True(File.Exists(Path.Combine(solutionDirectory, "obj", "project.assets.json")));
                // Make sure pack action was success.
                Assert.True(File.Exists(nupkgPath));
            }
        }

        [Fact]
        public async Task PackCommand_PrereleaseDependency_SucceedAndLogsWarning()
        {
            // Arrange
            using (SimpleTestPathContext pathContext = msbuildFixture.CreateSimpleTestPathContext())
            {
                var prereleaseDependencyName = "PreReleasePackageA";
                var prereleaseDependencyVersion = "6.0.0-preview.3";
                var prereleaseDependencyPackage = new SimpleTestPackageContext(prereleaseDependencyName, prereleaseDependencyVersion);
                prereleaseDependencyPackage.Files.Clear();
                prereleaseDependencyPackage.AddFile("_._");
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, prereleaseDependencyPackage);

                var projectName = "ClassLibrary1";
                string testDirectory = pathContext.WorkingDirectory;
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory, projectName);
                string projectXml = $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>{Constants.DefaultTargetFramework.GetShortFolderName()}</TargetFramework>
    <Version>1.2.3</Version>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""{prereleaseDependencyName}"" Version=""{prereleaseDependencyVersion}""/>
  </ItemGroup>
</Project>";
                File.WriteAllText(projectFile, projectXml);

                // Act
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                CommandRunnerResult result = msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}", validateSuccess: false);

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.2.3.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.2.3.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                result.AllOutput.Should().Contain(NuGetLogCode.NU5104.ToString());
                result.AllOutput.Should().Contain($"A stable release of a package should not have a prerelease dependency. Either modify the version spec of dependency \"{prereleaseDependencyName} [{prereleaseDependencyVersion}, )\" or update the version field in the nuspec.");
            }
        }

        [Fact]
        public async Task PackCommand_PrereleaseDependency_WarningSuppressed_Succeed()
        {
            using (SimpleTestPathContext pathContext = msbuildFixture.CreateSimpleTestPathContext())
            {
                var prereleaseDependencyName = "PreReleasePackageA";
                var prereleaseDependencyVersion = "6.0.0-preview.3";
                var prereleaseDependencyPackage = new SimpleTestPackageContext(prereleaseDependencyName, prereleaseDependencyVersion);
                prereleaseDependencyPackage.Files.Clear();
                prereleaseDependencyPackage.AddFile("_._");
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, prereleaseDependencyPackage);

                var projectName = "ClassLibrary1";
                string testDirectory = pathContext.WorkingDirectory;
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory, projectName);
                string projectXml = $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>{Constants.DefaultTargetFramework.GetShortFolderName()}</TargetFramework>
    <Version>1.2.3</Version>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include=""{prereleaseDependencyName}"" Version=""{prereleaseDependencyVersion}"" NoWarn = ""NU5104""/>
  </ItemGroup>
</Project>";
                File.WriteAllText(projectFile, projectXml);

                // Act
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                CommandRunnerResult result = msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}", validateSuccess: false);

                // Assert
                result.Success.Should().BeTrue();
                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.2.3.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.2.3.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                result.AllOutput.Should().NotContain(prereleaseDependencyName);
                result.AllOutput.Should().NotContain(NuGetLogCode.NU5104.ToString());

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    var dependencyGroups = nuspecReader
                        .GetDependencyGroups()
                        .OrderBy(x => x.TargetFramework,
                            new NuGetFrameworkSorter())
                        .ToList();

                    Assert.Equal(1,
                        dependencyGroups.Count);

                    var dependencyPackage = dependencyGroups[0].Packages.ToList();
                    Assert.Equal(1, dependencyPackage.Count);
                    Assert.Equal(prereleaseDependencyName, dependencyPackage[0].Id);
                    Assert.Equal(new VersionRange(new NuGetVersion(prereleaseDependencyVersion), true, null, true), dependencyPackage[0].VersionRange);
                    Assert.Equal(new List<string> { "Analyzers", "Build" }, dependencyPackage[0].Exclude);
                    Assert.Empty(dependencyPackage[0].Include);
                }
            }
        }

        [Fact]
        public async Task PackCommand_PrereleaseDependencies_PartialWarningSuppressed_SucceedAndLogsWarning()
        {
            using (SimpleTestPathContext pathContext = msbuildFixture.CreateSimpleTestPathContext())
            {
                var prereleaseDependencyAName = "PreReleasePackageA";
                var prereleaseDependencyAVersion = "4.8.0-beta00011";
                var prereleaseDependencyAPackage = new SimpleTestPackageContext(prereleaseDependencyAName, prereleaseDependencyAVersion);
                prereleaseDependencyAPackage.Files.Clear();
                prereleaseDependencyAPackage.AddFile("_._");
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, prereleaseDependencyAPackage);

                var prereleaseDependencyBName = "PreReleasePackageB";
                var prereleaseDependencyBVersion = "4.4.0-preview1-25305-02";
                var prereleaseDependencyBPackage = new SimpleTestPackageContext(prereleaseDependencyBName, prereleaseDependencyBVersion);
                prereleaseDependencyBPackage.Files.Clear();
                prereleaseDependencyBPackage.AddFile("_._");
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, prereleaseDependencyBPackage);

                var projectName = "ClassLibrary1";
                string testDirectory = pathContext.WorkingDirectory;
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory, projectName);
                string projectXml = $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>{Constants.DefaultTargetFramework.GetShortFolderName()}</TargetFramework>
    <Version>1.2.3</Version>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""{prereleaseDependencyAName}"" Version=""{prereleaseDependencyAVersion}"" NoWarn = ""NU5104""/>
    <!-- Below pre-release doesn't have no warn -->
    <PackageReference Include=""{prereleaseDependencyBName}"" Version=""{prereleaseDependencyBVersion}""/>
  </ItemGroup>
</Project>";
                File.WriteAllText(projectFile, projectXml);

                // Act
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                CommandRunnerResult result = msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}", validateSuccess: false);

                // Assert
                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.2.3.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.2.3.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                result.AllOutput.Should().NotContain(prereleaseDependencyAName);
                result.AllOutput.Should().Contain(prereleaseDependencyBName);
                result.AllOutput.Should().Contain(NuGetLogCode.NU5104.ToString());
            }
        }

        [Fact]
        public async Task PackCommand_PrereleaseDependency_ProjectLevelWarningSuppressed_Succeed()
        {
            using (SimpleTestPathContext pathContext = msbuildFixture.CreateSimpleTestPathContext())
            {
                var prereleaseDependencyAName = "PreReleasePackageA";
                var prereleaseDependencyAVersion = "4.8.0-beta00011";
                var prereleaseDependencyAPackage = new SimpleTestPackageContext(prereleaseDependencyAName, prereleaseDependencyAVersion);
                prereleaseDependencyAPackage.Files.Clear();
                prereleaseDependencyAPackage.AddFile("_._");
                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, prereleaseDependencyAPackage);

                var projectName = "ClassLibrary1";
                string testDirectory = pathContext.WorkingDirectory;
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory, projectName);
                string projectXml = $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFrameworks>{Constants.DefaultTargetFramework.GetShortFolderName()}</TargetFrameworks>
    <Version>1.2.3</Version>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <NoWarn>NU5104</NoWarn>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""{prereleaseDependencyAName}"" Version=""{prereleaseDependencyAVersion}"" />
  </ItemGroup>
</Project>";
                File.WriteAllText(projectFile, projectXml);

                // Act
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                CommandRunnerResult result = msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}", validateSuccess: false);

                // Assert
                result.Success.Should().BeTrue();
                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.2.3.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.2.3.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                result.AllOutput.Should().NotContain(prereleaseDependencyAName);
                result.AllOutput.Should().NotContain(NuGetLogCode.NU5104.ToString());

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    var dependencyGroups = nuspecReader
                        .GetDependencyGroups()
                        .OrderBy(x => x.TargetFramework,
                            new NuGetFrameworkSorter())
                        .ToList();

                    Assert.Equal(1,
                        dependencyGroups.Count);

                    var dependencyPackage = dependencyGroups[0].Packages.ToList();
                    Assert.Equal(1, dependencyPackage.Count);
                    Assert.Equal(prereleaseDependencyAName, dependencyPackage[0].Id);
                    Assert.Equal(new VersionRange(new NuGetVersion(prereleaseDependencyAVersion), true, null, true), dependencyPackage[0].VersionRange);
                    Assert.Equal(new List<string> { "Analyzers", "Build" }, dependencyPackage[0].Exclude);
                    Assert.Empty(dependencyPackage[0].Include);
                }
            }
        }

        [Fact]
        public async Task PackCommand_MultiTfm_PrereleaseDependency_TreatWarningsAsErrors_FailsAndLogsWarning()
        {
            using (SimpleTestPathContext pathContext = msbuildFixture.CreateSimpleTestPathContext())
            {
                SimpleTestSettingsContext settings = pathContext.Settings;
                settings.AddNetStandardFeeds();

                string testDirectory = pathContext.WorkingDirectory;
                string packageSource = pathContext.PackageSource;

                var prereleaseDependencyAName = "PreReleasePackageA";
                var prereleaseDependencyAVersion = "4.8.0-beta00011";
                var prereleaseDependencyAPackage = new SimpleTestPackageContext(prereleaseDependencyAName, prereleaseDependencyAVersion);
                prereleaseDependencyAPackage.Files.Clear();
                prereleaseDependencyAPackage.AddFile("_._");
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, prereleaseDependencyAPackage);

                var prereleaseDependencyBName = "PreReleasePackageB";
                var prereleaseDependencyBVersion = "4.4.0-preview1-25305-02";
                var prereleaseDependencyBPackage = new SimpleTestPackageContext(prereleaseDependencyBName, prereleaseDependencyBVersion);
                prereleaseDependencyBPackage.Files.Clear();
                prereleaseDependencyBPackage.AddFile("_._");
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, prereleaseDependencyBPackage);

                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory, projectName);
                string projectXml = $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFrameworks>net5.0;net45</TargetFrameworks>
    <Version>1.2.3</Version>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""{prereleaseDependencyAName}"" Version=""{prereleaseDependencyAVersion}"" NoWarn = ""NU5104""/>
  </ItemGroup>

  <ItemGroup Condition="" '$(TargetFramework)' == 'net45'"">
    <PackageReference Include=""{prereleaseDependencyBName}"" Version=""{prereleaseDependencyBVersion}""/>
  </ItemGroup>
</Project>";
                File.WriteAllText(projectFile, projectXml);

                // Act
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                CommandRunnerResult result = msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}", validateSuccess: false);

                // Assert
                result.Success.Should().BeFalse();
                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.2.3.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.2.3.nuspec");
                Assert.False(File.Exists(nupkgPath), "The output .nupkg is shouldn't created.");
                result.AllOutput.Should().Contain(NuGetLogCode.NU5104.ToString());
                result.AllOutput.Should().Contain($"A stable release of a package should not have a prerelease dependency. Either modify the version spec of dependency \"{prereleaseDependencyBName} [{prereleaseDependencyBVersion}, )\" or update the version field in the nuspec.");
            }
        }

        [Fact]
        public async Task PackCommand_MultiTfm_PrereleaseDependency_WarningIsSuppressed_Succeeds()
        {
            using (SimpleTestPathContext pathContext = msbuildFixture.CreateSimpleTestPathContext())
            {
                SimpleTestSettingsContext settings = pathContext.Settings;
                settings.AddNetStandardFeeds();

                string testDirectory = pathContext.WorkingDirectory;
                string packageSource = pathContext.PackageSource;

                var prereleaseDependencyAName = "PreReleasePackageA";
                var prereleaseDependencyAVersion = "4.8.0-beta00011";
                var prereleaseDependencyAPackage = new SimpleTestPackageContext(prereleaseDependencyAName, prereleaseDependencyAVersion);
                prereleaseDependencyAPackage.Files.Clear();
                prereleaseDependencyAPackage.AddFile("_._");
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, prereleaseDependencyAPackage);

                var prereleaseDependencyBName = "PreReleasePackageB";
                var prereleaseDependencyBVersion = "4.4.0-preview1-25305-02";
                var prereleaseDependencyBPackage = new SimpleTestPackageContext(prereleaseDependencyBName, prereleaseDependencyBVersion);
                prereleaseDependencyBPackage.Files.Clear();
                prereleaseDependencyBPackage.AddFile("_._");
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, prereleaseDependencyBPackage);

                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory, projectName);
                string projectXml = $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFrameworks>net5.0;net45</TargetFrameworks>
    <Version>1.2.3</Version>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""{prereleaseDependencyAName}"" Version=""{prereleaseDependencyAVersion}"" NoWarn = ""NU5104""/>
  </ItemGroup>

  <ItemGroup Condition="" '$(TargetFramework)' == 'net45'"">
    <PackageReference Include=""{prereleaseDependencyBName}"" Version=""{prereleaseDependencyBVersion}"" NoWarn = ""NU5104""/>
  </ItemGroup>
</Project>";
                File.WriteAllText(projectFile, projectXml);

                // Act
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                CommandRunnerResult result = msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}", validateSuccess: false);

                // Assert
                result.Success.Should().BeTrue();
                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.2.3.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.2.3.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                result.AllOutput.Should().NotContain(prereleaseDependencyAName);
                result.AllOutput.Should().NotContain(NuGetLogCode.NU5104.ToString());

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    var dependencyGroups = nuspecReader
                        .GetDependencyGroups()
                        .OrderBy(x => x.TargetFramework,
                            new NuGetFrameworkSorter())
                        .ToList();

                    Assert.Equal(2,
                        dependencyGroups.Count);

                    var dependencyPackage = dependencyGroups[0].Packages.ToList();
                    Assert.Equal(1, dependencyPackage.Count);
                    Assert.Equal(prereleaseDependencyAName, dependencyPackage[0].Id);
                    Assert.Equal(new VersionRange(new NuGetVersion(prereleaseDependencyAVersion), true, null, true), dependencyPackage[0].VersionRange);
                    Assert.Equal(new List<string> { "Analyzers", "Build" }, dependencyPackage[0].Exclude);
                    Assert.Empty(dependencyPackage[0].Include);
                    dependencyPackage = dependencyGroups[1].Packages.ToList();
                    Assert.Equal(2, dependencyPackage.Count);
                }
            }
        }

        [Fact]
        public async Task PackCommand_MultiTfm_PrereleaseDependency_ProjectLevelWarningSuppressed_Succeed()
        {
            using (SimpleTestPathContext pathContext = msbuildFixture.CreateSimpleTestPathContext())
            {
                SimpleTestSettingsContext settings = pathContext.Settings;
                settings.AddNetStandardFeeds();

                string testDirectory = pathContext.WorkingDirectory;
                string packageSource = pathContext.PackageSource;

                var prereleaseDependencyAName = "PreReleasePackageA";
                var prereleaseDependencyAVersion = "4.8.0-beta00011";
                var prereleaseDependencyAPackage = new SimpleTestPackageContext(prereleaseDependencyAName, prereleaseDependencyAVersion);
                prereleaseDependencyAPackage.Files.Clear();
                prereleaseDependencyAPackage.AddFile("_._");
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, prereleaseDependencyAPackage);

                var prereleaseDependencyBName = "PreReleasePackageB";
                var prereleaseDependencyBVersion = "4.4.0-preview1-25305-02";
                var prereleaseDependencyBPackage = new SimpleTestPackageContext(prereleaseDependencyBName, prereleaseDependencyBVersion);
                prereleaseDependencyBPackage.Files.Clear();
                prereleaseDependencyBPackage.AddFile("_._");
                await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, prereleaseDependencyBPackage);

                var projectName = "ClassLibrary1";
                var workingDirectory = Path.Combine(testDirectory, projectName);
                var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
                msbuildFixture.CreateDotnetNewProject(testDirectory, projectName);
                string projectXml = $@"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFrameworks>{Constants.DefaultTargetFramework.GetShortFolderName()};net48</TargetFrameworks>
    <Version>1.2.3</Version>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <NoWarn>NU5104</NoWarn>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""{prereleaseDependencyAName}"" Version=""{prereleaseDependencyAVersion}"" />
  </ItemGroup>

  <ItemGroup Condition="" '$(TargetFramework)' == 'net48'"">
    <PackageReference Include=""{prereleaseDependencyBName}"" Version=""{prereleaseDependencyBVersion}"" />
  </ItemGroup>
</Project>";
                File.WriteAllText(projectFile, projectXml);

                // Act
                msbuildFixture.RestoreProject(workingDirectory, projectName, string.Empty);
                CommandRunnerResult result = msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}", validateSuccess: false);

                // Assert
                result.Success.Should().BeTrue();
                var nupkgPath = Path.Combine(workingDirectory, $"{projectName}.1.2.3.nupkg");
                var nuspecPath = Path.Combine(workingDirectory, "obj", $"{projectName}.1.2.3.nuspec");
                Assert.True(File.Exists(nupkgPath), "The output .nupkg is not in the expected place");
                result.AllOutput.Should().NotContain(prereleaseDependencyAName);
                result.AllOutput.Should().NotContain(NuGetLogCode.NU5104.ToString());

                using (var nupkgReader = new PackageArchiveReader(nupkgPath))
                {
                    var nuspecReader = nupkgReader.NuspecReader;

                    var dependencyGroups = nuspecReader
                        .GetDependencyGroups()
                        .OrderBy(x => x.TargetFramework,
                            new NuGetFrameworkSorter())
                        .ToList();

                    Assert.Equal(2,
                        dependencyGroups.Count);

                    var dependencyPackage = dependencyGroups[0].Packages.ToList();
                    Assert.Equal(1, dependencyPackage.Count);
                    Assert.Equal(prereleaseDependencyAName, dependencyPackage[0].Id);
                    Assert.Equal(new VersionRange(new NuGetVersion(prereleaseDependencyAVersion), true, null, true), dependencyPackage[0].VersionRange);
                    Assert.Equal(new List<string> { "Analyzers", "Build" }, dependencyPackage[0].Exclude);
                    Assert.Empty(dependencyPackage[0].Include);
                }
            }
        }

        [PlatformFact(Platform.Windows)]
        public void PackCommand_WithAllWarningsAsErrorsAndWarningsNotAsErrors_SucceedsAndWarns()
        {
            using var testDirectory = msbuildFixture.CreateTestDirectory();
            var projectName = "ClassLibrary1";
            var workingDirectory = Path.Combine(testDirectory, projectName);
            Directory.CreateDirectory(workingDirectory);
            var projectFile = Path.Combine(workingDirectory, $"{projectName}.csproj");
            msbuildFixture.CreateDotnetNewProject(testDirectory.Path, projectName, " classlib");

            using (var stream = new FileStream(projectFile, FileMode.Open, FileAccess.ReadWrite))
            {
                var xml = XDocument.Load(stream);
                ProjectFileUtils.AddProperty(xml, "TreatWarningsAsErrors", "true");
                ProjectFileUtils.AddProperty(xml, "WarningsNotAsErrors", "NU5125");
                ProjectFileUtils.AddProperty(xml, "PackageLicenseUrl", "http://contoso.com/license.html");
                ProjectFileUtils.WriteXmlToFile(xml, stream);
            }

            CommandRunnerResult result = msbuildFixture.PackProject(workingDirectory, projectName, $"/p:PackageOutputPath={workingDirectory}");
            Assert.True(File.Exists(Path.Combine(workingDirectory, $"{projectName}.1.0.0.nupkg")), "The output .nupkg is not in the expected place");
            Assert.True(File.Exists(Path.Combine(workingDirectory, "obj", $"{projectName}.1.0.0.nuspec")), "The intermediate nuspec file is not in the expected place");
            var expectedWarning = string.Format("warning " + NuGetLogCode.NU5125 + ": " + NuGet.Packaging.Rules.AnalysisResources.LicenseUrlDeprecationWarning);
            result.AllOutput.Should().Contain(expectedWarning);
        }
    }
}
