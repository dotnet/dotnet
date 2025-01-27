// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Commands.Test.RestoreCommandTests;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.LibraryModel;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Repositories;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Commands.Test
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class BuildAssetsUtilsTests
    {
        [Fact]
        public void BuildAssetsUtils_GenerateProjectAssetsFilePath()
        {
            // Arrange
            var path = "/tmp/test/project.assets.json";

            var doc = BuildAssetsUtils.GenerateEmptyImportsFile();

            // Act
            BuildAssetsUtils.AddNuGetProperties(
                doc,
                Enumerable.Empty<string>(),
                string.Empty,
                ProjectStyle.PackageReference,
                path,
                success: true);

            var props = TargetsUtility.GetMSBuildProperties(doc);

            // Assert
            Assert.Equal(path, props["ProjectAssetsFile"]);
        }

        [Fact]
        public void AddNuGetProperty_WithPackageFolders_AddsSourceRootItem()
        {
            // Arrange
            var separator = Path.DirectorySeparatorChar;
            var packageFolders = $"{separator}tmp{separator}gpf;{separator}tmp{separator}fallbackFolder";

            var doc = BuildAssetsUtils.GenerateEmptyImportsFile();

            // Act
            BuildAssetsUtils.AddNuGetProperties(
                doc,
                packageFolders: packageFolders.Split(';'),
                string.Empty,
                ProjectStyle.PackageReference,
                "/tmp/test/project.assets.json",
                success: true);

            var props = TargetsUtility.GetMSBuildProperties(doc);
            var items = TargetsUtility.GetMSBuildItems(doc);
            var sourceRootItems = items.Where(e => e.Item1.Equals("SourceRoot")).Select(e => e.Item2).ToList();

            // Assert
            Assert.Equal(packageFolders, props["NuGetPackageFolders"]);
            Assert.Equal(2, items.Count);
            Assert.Equal(2, sourceRootItems.Count);
            Assert.Equal($"{separator}tmp{separator}gpf{separator}", sourceRootItems[0]["Include"]);
            Assert.Equal($"{separator}tmp{separator}fallbackFolder{separator}", sourceRootItems[1]["Include"]);
        }

        [Fact]
        public void BuildAssetsUtils_GenerateContentFilesItem_CompileAsset()
        {
            // Arrange
            var path = "contentFiles/cs/net46/test/test.cs";
            var item = new LockFileContentFile(path);
            item.BuildAction = BuildAction.Compile;
            item.CodeLanguage = "cs";

            // Act
            var node = BuildAssetsUtils.GenerateContentFilesItem(path, item, "a", "1.0.0");
            var metadata = node.Elements().ToDictionary(e => e.Name.LocalName, e => e.Value, StringComparer.OrdinalIgnoreCase);

            // Assert
            Assert.Equal("a", metadata["NuGetPackageId"]);
            Assert.Equal("1.0.0", metadata["NuGetPackageVersion"]);
            Assert.Equal("Compile", metadata["NuGetItemType"]);
            Assert.Equal("False", metadata["Private"]);
            Assert.Equal("test/test.cs", metadata["Link"].Replace('\\', '/'));
            Assert.Equal("false", metadata["Pack"]);
        }

        [Fact]
        public void BuildAssetsUtils_GenerateContentFilesItem_CopyToOutputFullPath()
        {
            // Arrange
            var path = "contentFiles/cs/net46/a/b/c.txt";
            var item = new LockFileContentFile(path);
            item.BuildAction = BuildAction.None;
            item.CodeLanguage = "cs";
            item.CopyToOutput = true;
            item.OutputPath = "a/b/c.txt";

            // Act
            var node = BuildAssetsUtils.GenerateContentFilesItem(path, item, "a", "1.0.0");
            var metadata = node.Elements().ToDictionary(e => e.Name.LocalName, e => e.Value, StringComparer.OrdinalIgnoreCase);

            // Assert
            Assert.Equal("a", metadata["NuGetPackageId"]);
            Assert.Equal("1.0.0", metadata["NuGetPackageVersion"]);
            Assert.Equal("None", metadata["NuGetItemType"]);
            Assert.Equal("True", metadata["Private"]);
            Assert.Equal("a/b/c.txt", metadata["Link"].Replace('\\', '/'));
            Assert.Equal("PreserveNewest", metadata["CopyToOutputDirectory"]);
            Assert.Equal("a/b/", metadata["DestinationSubDirectory"].Replace('\\', '/'));
            Assert.Equal("a/b/c.txt", metadata["TargetPath"].Replace('\\', '/'));
            Assert.Equal("false", metadata["Pack"]);
        }

        [Fact]
        public void BuildAssetsUtils_GenerateContentFilesItem_CopyToOutput_Flatten()
        {
            // Arrange
            var path = "contentFiles/cs/net46/a/b/c.txt";
            var item = new LockFileContentFile(path);
            item.BuildAction = BuildAction.None;
            item.CodeLanguage = "cs";
            item.CopyToOutput = true;
            item.OutputPath = "c.txt";

            // Act
            var node = BuildAssetsUtils.GenerateContentFilesItem(path, item, "a", "1.0.0");
            var metadata = node.Elements().ToDictionary(e => e.Name.LocalName, e => e.Value, StringComparer.OrdinalIgnoreCase);

            // Assert
            Assert.Equal("a", metadata["NuGetPackageId"]);
            Assert.Equal("1.0.0", metadata["NuGetPackageVersion"]);
            Assert.Equal("None", metadata["NuGetItemType"]);
            Assert.Equal("True", metadata["Private"]);
            Assert.Equal("c.txt", metadata["TargetPath"]);
            Assert.Equal("a/b/c.txt", metadata["Link"].Replace('\\', '/'));
            Assert.Equal("PreserveNewest", metadata["CopyToOutputDirectory"]);
            Assert.False(metadata.ContainsKey("DestinationSubDirectory"));
            Assert.Equal("false", metadata["Pack"]);
        }

        [Theory]
        [InlineData("cs", "C#")]
        [InlineData("fs", "F#")]
        [InlineData("", "")]
        [InlineData("vb", "VB")]
        [InlineData("py", "PY")]
        [InlineData("clj", "CLJ")]
        [InlineData("test", "TEST")]
        public void BuildAssetsUtils_GetLanguage(string input, string expected)
        {
            // Act
            var output = BuildAssetsUtils.GetLanguage(input);

            // Assert
            Assert.Equal(expected, output);
        }

        [Fact]
        public void BuildAssetsUtils_ReplaceWithUserProfileMacro()
        {
            // Arrange
            using (var randomProjectDirectory = TestDirectory.Create())
            {
                var globalPackagesFolder = SettingsUtility.GetGlobalPackagesFolder(NullSettings.Instance);

                if (!string.IsNullOrEmpty(globalPackagesFolder))
                {
                    // Act
                    var xml = BuildAssetsUtils.GenerateEmptyImportsFile();

                    BuildAssetsUtils.AddNuGetProperties(
                        xml,
                        new[] { globalPackagesFolder },
                        globalPackagesFolder,
                        ProjectStyle.PackageReference,
                        assetsFilePath: string.Empty,
                        success: true);

                    // Assert
                    var ns = XNamespace.Get("http://schemas.microsoft.com/developer/msbuild/2003");
                    var elements = xml.Root.Descendants(ns + "NuGetPackageRoot");
                    Assert.Single(elements);

                    var element = elements.Single();
                    string expected = null;

                    if (RuntimeEnvironmentHelper.IsWindows)
                    {
                        expected = Path.Combine(@"$(UserProfile)", ".nuget", "packages") + Path.DirectorySeparatorChar;
                    }
                    else
                    {
                        expected = Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".nuget", "packages") + Path.DirectorySeparatorChar;
                    }
                    Assert.Equal(expected, element.Value);
                }
            }
        }

        [Fact]
        public void BuildAssetsUtils_MultipleTFMs_CrossTargeting()
        {
            // Arrange
            using (var globalPackagesFolder = TestDirectory.Create())
            using (var randomProjectDirectory = TestDirectory.Create())
            {
                var propGroups = new List<MSBuildRestoreItemGroup>();
                var targetGroups = new List<MSBuildRestoreItemGroup>();

                targetGroups.Add(new MSBuildRestoreItemGroup()
                {
                    Items = new List<XElement>()
                    {
                        BuildAssetsUtils.GenerateImport("a.targets"),
                        BuildAssetsUtils.GenerateImport("b.targets")
                    },
                    Conditions = new List<string>()
                    {
                        "'$(TargetFramework)' == 'net45'"
                    }
                });

                targetGroups.Add(new MSBuildRestoreItemGroup()
                {
                    Items = new List<XElement>()
                    {
                        BuildAssetsUtils.GenerateImport("c.targets")
                    },
                    Conditions = new List<string>()
                    {
                        "'$(TargetFramework)' == 'netstandard16'"
                    }
                });

                targetGroups.Add(new MSBuildRestoreItemGroup()
                {
                    Conditions = new List<string>()
                    {
                        "'$(TargetFramework)' == 'netStandard1.7'"
                    }
                });

                targetGroups.Add(new MSBuildRestoreItemGroup()
                {
                    Items = new List<XElement>()
                    {
                        BuildAssetsUtils.GenerateImport("x.targets"),
                        BuildAssetsUtils.GenerateImport("y.targets")
                    },
                    Conditions = new List<string>()
                    {
                        "'$(TargetFramework)' == ''"
                    },
                    Position = 0,
                });

                propGroups.Add(new MSBuildRestoreItemGroup()
                {
                    Items = new List<XElement>()
                    {
                        BuildAssetsUtils.GenerateImport("a.props"),
                        BuildAssetsUtils.GenerateImport("b.props")
                    },
                    Conditions = new List<string>()
                    {
                        "'$(TargetFramework)' == 'net45'"
                    }
                });

                propGroups.Add(new MSBuildRestoreItemGroup()
                {
                    Items = new List<XElement>()
                    {
                        BuildAssetsUtils.GenerateImport("c.props")
                    },
                    Conditions = new List<string>()
                    {
                        "'$(TargetFramework)' == 'netstandard16'"
                    }
                });

                propGroups.Add(new MSBuildRestoreItemGroup()
                {
                    Conditions = new List<string>()
                    {
                        "'$(TargetFramework)' == 'netStandard1.7'"
                    }
                });

                propGroups.Add(new MSBuildRestoreItemGroup()
                {
                    Conditions = new List<string>()
                    {
                        "'$(TargetFramework)' == 'netStandard1.8'"
                    }
                });

                propGroups.Add(new MSBuildRestoreItemGroup()
                {
                    Items = new List<XElement>()
                    {
                        BuildAssetsUtils.GenerateImport("z.props")
                    },
                    Conditions = new List<string>()
                    {
                        "'$(TargetFramework)' == ''"
                    },
                    Position = 0,
                });

                // Act
                var targetsXML = BuildAssetsUtils.GenerateMSBuildFile(
                    targetGroups,
                    ProjectStyle.PackageReference);

                var propsXML = BuildAssetsUtils.GenerateMSBuildFile(
                    propGroups,
                    ProjectStyle.PackageReference);

                // Assert
                var targetItemGroups = targetsXML.Root.Elements().Where(e => e.Name.LocalName == "ImportGroup").ToList();
                var propsItemGroups = propsXML.Root.Elements().Where(e => e.Name.LocalName == "ImportGroup").ToList();

                Assert.Equal(3, targetItemGroups.Count);
                Assert.Equal("'$(TargetFramework)' == ''", targetItemGroups[0].Attribute(XName.Get("Condition")).Value.Trim());
                Assert.Equal("'$(TargetFramework)' == 'net45'", targetItemGroups[1].Attribute(XName.Get("Condition")).Value.Trim());
                Assert.Equal("'$(TargetFramework)' == 'netstandard16'", targetItemGroups[2].Attribute(XName.Get("Condition")).Value.Trim());

                Assert.Equal(2, targetItemGroups[0].Elements().Count());
                Assert.Equal("x.targets", targetItemGroups[0].Elements().ToList()[0].Attribute(XName.Get("Project")).Value);
                Assert.Equal("y.targets", targetItemGroups[0].Elements().ToList()[1].Attribute(XName.Get("Project")).Value);

                Assert.Equal(2, targetItemGroups[1].Elements().Count());
                Assert.Equal("a.targets", targetItemGroups[1].Elements().ToList()[0].Attribute(XName.Get("Project")).Value);
                Assert.Equal("b.targets", targetItemGroups[1].Elements().ToList()[1].Attribute(XName.Get("Project")).Value);

                Assert.Equal(1, targetItemGroups[2].Elements().Count());
                Assert.Equal("c.targets", targetItemGroups[2].Elements().ToList()[0].Attribute(XName.Get("Project")).Value);

                Assert.Equal(3, propsItemGroups.Count);
                Assert.Equal("'$(TargetFramework)' == ''", propsItemGroups[0].Attribute(XName.Get("Condition")).Value.Trim());

                Assert.Equal(1, propsItemGroups[0].Elements().Count());
                Assert.Equal("z.props", propsItemGroups[0].Elements().ToList()[0].Attribute(XName.Get("Project")).Value);
            }
        }

        [Fact]
        public void BuildAssetsUtils_MultipleTFMs()
        {
            // Arrange
            using (var globalPackagesFolder = TestDirectory.Create())
            using (var randomProjectDirectory = TestDirectory.Create())
            {
                var props = new List<MSBuildRestoreItemGroup>();
                var targets = new List<MSBuildRestoreItemGroup>();

                targets.Add(new MSBuildRestoreItemGroup()
                {
                    Conditions = new List<string>()
                    {
                        "'$(TargetFramework)' == 'net45'"
                    },
                    Items = new List<XElement>()
                    {
                        BuildAssetsUtils.GenerateImport("a.targets"),
                        BuildAssetsUtils.GenerateImport("b.targets")
                    },
                });

                targets.Add(new MSBuildRestoreItemGroup()
                {
                    Conditions = new List<string>()
                    {
                        "'$(TargetFramework)' == 'netstandard16'"
                    },
                    Items = new List<XElement>()
                    {
                        BuildAssetsUtils.GenerateImport("c.targets")
                    },
                });

                targets.Add(new MSBuildRestoreItemGroup()
                {
                    Conditions = new List<string>()
                    {
                        "'$(TargetFramework)' == 'netStandard1.7'"
                    }
                });

                // Act
                var targetsXML = BuildAssetsUtils.GenerateMSBuildFile(
                    targets,
                    ProjectStyle.PackageReference);

                // Assert
                var targetItemGroups = targetsXML.Root.Elements().Where(e => e.Name.LocalName == "ImportGroup").ToList();

                Assert.Equal(2, targetItemGroups.Count);
                Assert.Equal("'$(TargetFramework)' == 'net45'", targetItemGroups[0].Attribute(XName.Get("Condition")).Value.Trim());
                Assert.Equal("'$(TargetFramework)' == 'netstandard16'", targetItemGroups[1].Attribute(XName.Get("Condition")).Value.Trim());

                Assert.Equal(2, targetItemGroups[0].Elements().Count());
                Assert.Equal("a.targets", targetItemGroups[0].Elements().ToList()[0].Attribute(XName.Get("Project")).Value);
                Assert.Equal("b.targets", targetItemGroups[0].Elements().ToList()[1].Attribute(XName.Get("Project")).Value);

                Assert.Equal(1, targetItemGroups[1].Elements().Count());
                Assert.Equal("c.targets", targetItemGroups[1].Elements().ToList()[0].Attribute(XName.Get("Project")).Value);
            }
        }

        [Fact]
        public void BuildAssetsUtils_NETCoreWithNoItemsStillGeneratesFile()
        {
            // Arrange
            using (var globalPackagesFolder = TestDirectory.Create())
            using (var randomProjectDirectory = TestDirectory.Create())
            {
                var props = new List<MSBuildRestoreItemGroup>();
                var targets = new List<MSBuildRestoreItemGroup>();

                // Act
                var xml = BuildAssetsUtils.GenerateMSBuildFile(
                    targets,
                    ProjectStyle.PackageReference);

                // Assert
                Assert.NotNull(xml);
            }
        }

        [Fact]
        public void BuildAssetsUtils_ProjectJsonWithNoItemsDoesNotGenerateFile()
        {
            // Arrange
            using (var globalPackagesFolder = TestDirectory.Create())
            using (var randomProjectDirectory = TestDirectory.Create())
            {
                var props = new List<MSBuildRestoreItemGroup>();
                var targets = new List<MSBuildRestoreItemGroup>();

                // Act
                var xml = BuildAssetsUtils.GenerateMSBuildFile(
                    targets,
                    ProjectStyle.ProjectJson);

                // Assert
                Assert.Null(xml);
            }
        }

        [Fact]
        public void BuildAssetsUtils_SingleTFM()
        {
            // Arrange
            using (var globalPackagesFolder = TestDirectory.Create())
            using (var randomProjectDirectory = TestDirectory.Create())
            {
                var props = new List<MSBuildRestoreItemGroup>();
                var targets = new List<MSBuildRestoreItemGroup>();

                targets.Add(new MSBuildRestoreItemGroup()
                {
                    Conditions = new List<string>()
                    {
                        "'$(TargetFramework)' == 'net45'"
                    },
                    Items = new List<XElement>()
                    {
                        BuildAssetsUtils.GenerateImport("a.targets"),
                        BuildAssetsUtils.GenerateImport("b.targets")
                    },
                });

                props.Add(new MSBuildRestoreItemGroup()
                {
                    Conditions = new List<string>()
                    {
                        "'$(TargetFramework)' == 'net45'"
                    },
                    Items = new List<XElement>()
                    {
                        BuildAssetsUtils.GenerateImport("a.props"),
                        BuildAssetsUtils.GenerateImport("b.props")
                    },
                });

                // Act
                var xml = BuildAssetsUtils.GenerateMSBuildFile(
                    targets,
                    ProjectStyle.PackageReference);

                // Assert
                var targetItemGroups = xml.Root.Elements().Where(e => e.Name.LocalName == "ImportGroup").ToList();

                Assert.Equal(1, targetItemGroups.Count);
                Assert.Equal("'$(TargetFramework)' == 'net45'", targetItemGroups[0].Attribute(XName.Get("Condition")).Value.Trim());
                Assert.Equal(2, targetItemGroups[0].Elements().Count());
                Assert.Equal("a.targets", targetItemGroups[0].Elements().ToList()[0].Attribute(XName.Get("Project")).Value);
                Assert.Equal("b.targets", targetItemGroups[0].Elements().ToList()[1].Attribute(XName.Get("Project")).Value);
            }
        }

        [Fact]
        public void BuildAssetsUtils_SingleTFM_NoConditionals()
        {
            // Arrange
            using (var globalPackagesFolder = TestDirectory.Create())
            using (var randomProjectDirectory = TestDirectory.Create())
            {
                // Only run the test if globalPackagesFolder can be determined
                // Because, globalPackagesFolder would be null if %USERPROFILE% was null

                var props = new List<MSBuildRestoreItemGroup>();
                var targets = new List<MSBuildRestoreItemGroup>();

                targets.Add(new MSBuildRestoreItemGroup()
                {
                    Items = new List<XElement>()
                    {
                        BuildAssetsUtils.GenerateImport("a.targets"),
                        BuildAssetsUtils.GenerateImport("b.targets")
                    },
                });

                props.Add(new MSBuildRestoreItemGroup()
                {
                    Items = new List<XElement>()
                    {
                        BuildAssetsUtils.GenerateImport("a.props"),
                        BuildAssetsUtils.GenerateImport("b.props")
                    },
                });

                // Act
                var xml = BuildAssetsUtils.GenerateMSBuildFile(
                    targets,
                    ProjectStyle.ProjectJson);

                // Assert
                var targetItemGroups = xml.Root.Elements().Where(e => e.Name.LocalName == "ImportGroup").ToList();

                Assert.Equal(1, targetItemGroups.Count);
                Assert.Equal(0, targetItemGroups[0].Attributes().Count());
                Assert.Equal(2, targetItemGroups[0].Elements().Count());
                Assert.Equal("a.targets", targetItemGroups[0].Elements().ToList()[0].Attribute(XName.Get("Project")).Value);
                Assert.Equal("b.targets", targetItemGroups[0].Elements().ToList()[1].Attribute(XName.Get("Project")).Value);
            }
        }

        [Fact]
        public void BuildAssetsUtils_VerifyPositionAndSortOrder()
        {
            // Arrange
            using (var globalPackagesFolder = TestDirectory.Create())
            using (var randomProjectDirectory = TestDirectory.Create())
            {
                var props = new List<MSBuildRestoreItemGroup>();
                var targets = new List<MSBuildRestoreItemGroup>();

                targets.Add(new MSBuildRestoreItemGroup()
                {
                    Conditions = new List<string>()
                    {
                        "b"
                    },
                    Items = new List<XElement>()
                    {
                        BuildAssetsUtils.GenerateImport("a.targets")
                    },
                    Position = 0
                });

                targets.Add(new MSBuildRestoreItemGroup()
                {
                    Conditions = new List<string>()
                    {
                        "a"
                    },
                    Items = new List<XElement>()
                    {
                        BuildAssetsUtils.GenerateImport("a.targets")
                    },
                    Position = 0
                });

                targets.Add(new MSBuildRestoreItemGroup()
                {
                    Conditions = new List<string>()
                    {
                        "z"
                    },
                    Items = new List<XElement>()
                    {
                        BuildAssetsUtils.GenerateImport("a.targets")
                    },
                    Position = -1
                });

                targets.Add(new MSBuildRestoreItemGroup()
                {
                    Conditions = new List<string>()
                    {
                        "x"
                    },
                    Items = new List<XElement>()
                    {
                        BuildAssetsUtils.GenerateImport("a.targets")
                    },
                    Position = 100
                });

                // Act
                var xml = BuildAssetsUtils.GenerateMSBuildFile(
                    targets,
                    ProjectStyle.ProjectJson);

                // Assert
                var targetItemGroups = xml.Root.Elements().Where(e => e.Name.LocalName == "ImportGroup").ToList();

                Assert.Equal(4, targetItemGroups.Count);
                Assert.Equal("z", targetItemGroups[0].Attribute(XName.Get("Condition")).Value.Trim());
                Assert.Equal("a", targetItemGroups[1].Attribute(XName.Get("Condition")).Value.Trim());
                Assert.Equal("b", targetItemGroups[2].Attribute(XName.Get("Condition")).Value.Trim());
                Assert.Equal("x", targetItemGroups[3].Attribute(XName.Get("Condition")).Value.Trim());
            }
        }

        [Fact]
        public async Task BuildAssetsUtils_GeneratePathProperty()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Arrange
                var identity = new PackageIdentity("packagea", NuGetVersion.Parse("1.0.0"));

                var packageDirectory = Directory.CreateDirectory(Path.Combine(pathContext.UserPackagesFolder, identity.Id, identity.Version.ToNormalizedString()));

                File.WriteAllText(Path.Combine(packageDirectory.FullName, $"{identity.Id}.{identity.Version.ToNormalizedString()}.nupkg.sha512"), string.Empty);

                var packagePath = await SimpleTestPackageUtility.CreateFullPackageAsync(
                    packageDirectory.FullName,
                    identity.Id,
                    identity.Version.ToString());

                var logger = new TestLogger();

                const string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""netcoreapp1.0"": {
                            ""dependencies"": {
                            }
                        }
                    }
                }";
                var projectName = "a";
                var rootProjectsPath = pathContext.WorkingDirectory;
                var projectDirectory = Path.Combine(rootProjectsPath, projectName);

                var spec = JsonPackageSpecReader.GetPackageSpec(referenceSpec, projectName, Path.Combine(projectDirectory, projectName)).WithTestRestoreMetadata();

                spec.Dependencies.Add(new LibraryDependency
                {
                    GeneratePathProperty = true,
                    IncludeType = LibraryIncludeFlags.All,
                    LibraryRange = new LibraryRange(identity.Id.ToUpperInvariant(), new VersionRange(identity.Version), LibraryDependencyTarget.Package)
                });

                var targetGraphs = new List<RestoreTargetGraph>
                {
                    OriginalCaseGlobalPackageFolderTests.GetRestoreTargetGraph(pathContext.PackageSource, identity, packagePath, logger)
                };

                targetGraphs[0].Graphs.FirstOrDefault().Item.Data.Dependencies = spec.Dependencies.ToList();

                var lockFile = new LockFile
                {
                    Libraries =
                    {
                        new LockFileLibrary
                        {
                            Name = identity.Id,
                            Version = identity.Version,
                            Path = $"{identity.Id.ToLowerInvariant()}/{identity.Version.ToNormalizedString()}",
                            Type = LibraryType.Package
                        }
                    },
                    Targets =
                    {
                        new LockFileTarget
                        {
                            RuntimeIdentifier = targetGraphs[0].RuntimeIdentifier,
                            TargetFramework = targetGraphs[0].Framework,
                            Libraries =
                            {
                                new LockFileTargetLibrary
                                {
                                    Name = identity.Id,
                                    Version = identity.Version
                                }
                            }
                        }
                    }
                };

                var repositories = new List<NuGetv3LocalRepository>
                {
                    new NuGetv3LocalRepository(pathContext.UserPackagesFolder)
                };

                var restoreRequest = new TestRestoreRequest(spec, new[] { new PackageSource(pathContext.PackageSource) }, pathContext.PackagesV2, logger)
                {
                    ProjectStyle = spec.RestoreMetadata.ProjectStyle
                };

                var assetsFilePath = Path.Combine(projectDirectory, "obj", "project.assets.json");

                // Act
                var outputFiles = BuildAssetsUtils.GetMSBuildOutputFiles(spec, lockFile, targetGraphs, repositories, restoreRequest, assetsFilePath, true, logger);

                // Assert
                var expectedPropertyGroup = outputFiles.FirstOrDefault().Content.Root.Elements().LastOrDefault();

                Assert.NotNull(expectedPropertyGroup);

                Assert.Equal(" '$(ExcludeRestorePackageImports)' != 'true' ", expectedPropertyGroup.Attribute("Condition")?.Value);

                var expectedProperty = expectedPropertyGroup.Elements().FirstOrDefault();

                Assert.Equal($"Pkg{identity.Id.Replace(".", "_")}", expectedProperty.Name.LocalName);

                Assert.Equal($" '$({expectedProperty.Name.LocalName})' == '' ", expectedProperty.Attribute("Condition")?.Value);

                Assert.Equal(packageDirectory.FullName, expectedProperty?.Value, ignoreCase: true);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task BuildAssetsUtils_GeneratePathPropertyForTools(bool hasTools)
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Arrange
                var identity = new PackageIdentity("packagea", NuGetVersion.Parse("1.0.0"));

                var packageDirectory = Directory.CreateDirectory(Path.Combine(pathContext.UserPackagesFolder, identity.Id, identity.Version.ToNormalizedString()));

                File.WriteAllText(Path.Combine(packageDirectory.FullName, $"{identity.Id}.{identity.Version.ToNormalizedString()}.nupkg.sha512"), string.Empty);

                var packagePath = await SimpleTestPackageUtility.CreateFullPackageAsync(
                    packageDirectory.FullName,
                    identity.Id,
                    identity.Version.ToString());

                var logger = new TestLogger();

                const string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""netcoreapp1.0"": {
                            ""dependencies"": {
                            }
                        }
                    }
                }";
                var projectName = "a";
                var rootProjectsPath = pathContext.WorkingDirectory;
                var projectDirectory = Path.Combine(rootProjectsPath, projectName);

                var spec = JsonPackageSpecReader.GetPackageSpec(referenceSpec, projectName, Path.Combine(projectDirectory, projectName)).WithTestRestoreMetadata();

                spec.Dependencies.Add(new LibraryDependency
                {
                    IncludeType = LibraryIncludeFlags.All,
                    LibraryRange = new LibraryRange(identity.Id, new VersionRange(identity.Version), LibraryDependencyTarget.Package)
                });

                var targetGraphs = new List<RestoreTargetGraph>
                {
                    OriginalCaseGlobalPackageFolderTests.GetRestoreTargetGraph(pathContext.PackageSource, identity, packagePath, logger)
                };

                targetGraphs[0].Graphs.FirstOrDefault().Item.Data.Dependencies = spec.Dependencies.ToList();

                var lockFile = new LockFile
                {
                    Libraries =
                    {
                        new LockFileLibrary
                        {
                            Name = identity.Id,
                            Version = identity.Version,
                            Path = $"{identity.Id.ToLowerInvariant()}/{identity.Version.ToNormalizedString()}",
                            Type = LibraryType.Package,
                            HasTools = hasTools,
                        }
                    },
                    Targets =
                    {
                        new LockFileTarget
                        {
                            RuntimeIdentifier = targetGraphs[0].RuntimeIdentifier,
                            TargetFramework = targetGraphs[0].Framework,
                            Libraries =
                            {
                                new LockFileTargetLibrary
                                {
                                    Name = identity.Id,
                                    Version = identity.Version
                                }
                            }
                        }
                    }
                };

                var repositories = new List<NuGetv3LocalRepository>
                {
                    new NuGetv3LocalRepository(pathContext.UserPackagesFolder)
                };

                var restoreRequest = new TestRestoreRequest(spec, new[] { new PackageSource(pathContext.PackageSource) }, pathContext.PackagesV2, logger)
                {
                    ProjectStyle = spec.RestoreMetadata.ProjectStyle
                };

                var assetsFilePath = Path.Combine(projectDirectory, "obj", "project.assets.json");

                // Act
                var outputFiles = BuildAssetsUtils.GetMSBuildOutputFiles(spec, lockFile, targetGraphs, repositories, restoreRequest, assetsFilePath, true, logger);

                var expectedPropertyName = $"Pkg{identity.Id.Replace(".", "_")}";

                var actualPropertyElement = outputFiles.FirstOrDefault().Content.Root.Descendants().FirstOrDefault(i => i.Name.LocalName.Equals(expectedPropertyName));

                if (hasTools)
                {
                    // Assert
                    Assert.NotNull(actualPropertyElement);


                    Assert.Equal($" '$({actualPropertyElement.Name.LocalName})' == '' ", actualPropertyElement.Attribute("Condition")?.Value);

                    Assert.Equal(packageDirectory.FullName, actualPropertyElement?.Value, ignoreCase: true);

                    Assert.Equal(" '$(ExcludeRestorePackageImports)' != 'true' ", actualPropertyElement.Parent.Attribute("Condition")?.Value);
                }
                else
                {
                    Assert.Null(actualPropertyElement);
                }
            }
        }

        [Fact]
        public void BuildAssetsUtils_GenerateProjectRelativeAssetsFilePath()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var filePath = Path.Combine(workingDir, "obj", "test.props");
                var assetsPath = Path.Combine(workingDir, "obj", "project.assets.json");

                var doc = BuildAssetsUtils.GenerateEmptyImportsFile();
                var file = new MSBuildOutputFile(filePath, doc);

                // Act
                BuildAssetsUtils.AddNuGetPropertiesToFirstImport(
                    new[] { file },
                    Enumerable.Empty<string>(),
                    string.Empty,
                    ProjectStyle.PackageReference,
                    assetsPath,
                    success: true);

                var props = TargetsUtility.GetMSBuildProperties(doc);

                // Assert
                Assert.Equal("$(MSBuildThisFileDirectory)project.assets.json", props["ProjectAssetsFile"]);
            }
        }

        [Fact]
        public void BuildAssetsUtils_GenerateProjectRelativeAssetsFilePathInOtherDir()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange
                var filePath = Path.Combine(workingDir, "obj", "test.props");
                var assetsPath = Path.Combine(workingDir, "nuget", "project.assets.json");

                var doc = BuildAssetsUtils.GenerateEmptyImportsFile();
                var file = new MSBuildOutputFile(filePath, doc);

                // Act
                BuildAssetsUtils.AddNuGetPropertiesToFirstImport(
                    new[] { file },
                    Enumerable.Empty<string>(),
                    string.Empty,
                    ProjectStyle.PackageReference,
                    assetsPath,
                    success: true);

                var props = TargetsUtility.GetMSBuildProperties(doc);

                Assert.Equal("$(MSBuildThisFileDirectory)project.assets.json".Replace('/', Path.DirectorySeparatorChar), props["ProjectAssetsFile"]);
            }
        }

        [Fact]
        public void HasChanges_WhenXDocumentDoesNotMatchFileContent_Success()
        {
            // Arrange
            using (var randomProjectDirectory = TestDirectory.Create())
            {
                var filePath = Path.Combine(randomProjectDirectory, "propsXML.xml");
                var internalSubset = @"<!ENTITY greeting ""Hello"">
   <!ENTITY name ""NuGet Client "">
   <!ENTITY sayhello ""&greeting; &name;"">";
                var newValue = "&sayhello;";

                XDocument doc = BuildAssetsUtils.GenerateEmptyImportsFile();
                doc.Save(filePath);

                XDocument newdoc = XDocument.Load(filePath);
                newdoc.AddFirst(new XDocumentType("Project", null, null, internalSubset));
                XAttribute attribute = newdoc.Root.Attributes(XName.Get("ToolsVersion")).FirstOrDefault();
                attribute.Value = newValue;
                newdoc.Save(filePath);

                string newxml = File.ReadAllText(filePath);
                File.WriteAllText(filePath, newxml.Replace("&amp;", "&"));

                //Act & Assert
                Assert.True(BuildAssetsUtils.HasChanges(doc, filePath, NullLogger.Instance));
            }
        }

        [Fact]
        public void HasChanges_WhenXDocumentMatchesFileContent_Fails()
        {
            // Arrange
            using (var randomProjectDirectory = TestDirectory.Create())
            {
                var filePath = Path.Combine(randomProjectDirectory, "propsXML.xml");
                XDocument doc = BuildAssetsUtils.GenerateEmptyImportsFile();
                doc.Save(filePath);

                //Act
                bool result = BuildAssetsUtils.HasChanges(doc, filePath, NullLogger.Instance);

                Assert.False(result);
            }
        }
    }
}
