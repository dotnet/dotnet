// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Commands.Test
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class RestoreBuildTargetsAndPropsTests
    {
        [Fact]
        public async Task RestoreBuildTargetsAndProps_VerifyPropsAndTargetsGeneratedAsync()
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>
                {
                    new PackageSource(pathContext.PackageSource)
                };

                var spec = GetProject("projectA", "net462", "netstandard1.6");

                spec.RestoreMetadata.CrossTargeting = true;
                spec.Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange("x", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                });

                // Create fake projects, the real data is in the specs
                var projects = CreateProjectsFromSpecs(pathContext, spec);

                // Create dg file
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(spec);
                dgFile.AddRestore(spec.RestoreMetadata.ProjectUniqueName);
                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                packageX.AddFile("build/x.targets");
                packageX.AddFile("build/x.props");
                packageX.AddFile("contentFiles/any/any/_._");

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX);

                var project = projects[0];

                // Act
                var summaries = await RunRestoreAsync(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);

                // Assert
                Assert.True(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));

                var targetsXML = XDocument.Parse(File.ReadAllText(project.TargetsOutput));
                var targetItemGroups = targetsXML.Root.Elements().Where(e => e.Name.LocalName == "ImportGroup").ToList();

                var propsXML = XDocument.Parse(File.ReadAllText(project.PropsOutput));
                var propsItemGroups = propsXML.Root.Elements().Where(e => e.Name.LocalName == "ImportGroup").ToList();

                Assert.Equal("'$(TargetFramework)' == 'net462' AND '$(ExcludeRestorePackageImports)' != 'true'", targetItemGroups[0].Attribute(XName.Get("Condition")).Value.Trim());
                Assert.Equal("'$(TargetFramework)' == 'netstandard1.6' AND '$(ExcludeRestorePackageImports)' != 'true'", targetItemGroups[1].Attribute(XName.Get("Condition")).Value.Trim());

                Assert.Equal("'$(TargetFramework)' == 'net462' AND '$(ExcludeRestorePackageImports)' != 'true'", propsItemGroups[0].Attribute(XName.Get("Condition")).Value.Trim());
                Assert.Equal("'$(TargetFramework)' == 'netstandard1.6' AND '$(ExcludeRestorePackageImports)' != 'true'", propsItemGroups[1].Attribute(XName.Get("Condition")).Value.Trim());
            }
        }

        [Fact]
        public async Task RestoreBuildTargetsAndProps_VerifyPropsAndTargetsGeneratedWithNoTFMConditionsAsync()
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>
                {
                    new PackageSource(pathContext.PackageSource)
                };

                var spec = GetProject("projectA", "net462");

                spec.RestoreMetadata.CrossTargeting = false;
                spec.Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange("x", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                });

                // Create fake projects, the real data is in the specs
                var projects = CreateProjectsFromSpecs(pathContext, spec);

                // Create dg file
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(spec);
                dgFile.AddRestore(spec.RestoreMetadata.ProjectUniqueName);
                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                packageX.AddFile("build/x.targets");
                packageX.AddFile("build/x.props");
                packageX.AddFile("contentFiles/any/any/_._");

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX);

                var project = projects[0];

                // Act
                var summaries = await RunRestoreAsync(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);

                // Assert
                Assert.True(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));

                var targetsXML = XDocument.Parse(File.ReadAllText(project.TargetsOutput));
                var targetItemGroups = targetsXML.Root.Elements().Where(e => e.Name.LocalName == "ImportGroup").ToList();

                var propsXML = XDocument.Parse(File.ReadAllText(project.PropsOutput));
                var propsItemGroups = propsXML.Root.Elements().Where(e => e.Name.LocalName == "ImportGroup").ToList();

                Assert.Equal("'$(ExcludeRestorePackageImports)' != 'true'", targetItemGroups[0].Attribute(XName.Get("Condition")).Value.Trim());
                Assert.Equal("'$(ExcludeRestorePackageImports)' != 'true'", propsItemGroups[0].Attribute(XName.Get("Condition")).Value.Trim());
            }
        }

        [Fact]
        public async Task RestoreBuildTargetsAndProps_VerifyPropsAndTargetsGenerated_SingleTFMWithConditionsAsync()
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>
                {
                    new PackageSource(pathContext.PackageSource)
                };

                var spec = GetProject("projectA", "net462");

                spec.RestoreMetadata.CrossTargeting = true;
                spec.Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange("x", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                });

                // Create fake projects, the real data is in the specs
                var projects = CreateProjectsFromSpecs(pathContext, spec);

                // Create dg file
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(spec);
                dgFile.AddRestore(spec.RestoreMetadata.ProjectUniqueName);
                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                packageX.AddFile("build/x.targets");
                packageX.AddFile("build/x.props");
                packageX.AddFile("contentFiles/any/any/_._");

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX);

                var project = projects[0];

                // Act
                var summaries = await RunRestoreAsync(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);

                // Assert
                Assert.True(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));

                var targetsXML = XDocument.Parse(File.ReadAllText(project.TargetsOutput));
                var targetItemGroups = targetsXML.Root.Elements().Where(e => e.Name.LocalName == "ImportGroup").ToList();

                var propsXML = XDocument.Parse(File.ReadAllText(project.PropsOutput));
                var propsItemGroups = propsXML.Root.Elements().Where(e => e.Name.LocalName == "ImportGroup").ToList();

                Assert.Equal("'$(TargetFramework)' == 'net462' AND '$(ExcludeRestorePackageImports)' != 'true'", targetItemGroups[0].Attribute(XName.Get("Condition")).Value.Trim());
                Assert.Equal("'$(TargetFramework)' == 'net462' AND '$(ExcludeRestorePackageImports)' != 'true'", propsItemGroups[0].Attribute(XName.Get("Condition")).Value.Trim());
            }
        }

        [Fact]
        public async Task RestoreBuildTargetsAndProps_VerifyPropsAndTargetsCrossTargetingAsync()
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>
                {
                    new PackageSource(pathContext.PackageSource)
                };

                var spec = GetProject("projectA", "net462", "netstandard1.6");

                spec.RestoreMetadata.CrossTargeting = true;
                spec.Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange("x", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                });

                // Create fake projects, the real data is in the specs
                var projects = CreateProjectsFromSpecs(pathContext, spec);

                // Create dg file
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(spec);
                dgFile.AddRestore(spec.RestoreMetadata.ProjectUniqueName);
                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                packageX.AddFile("buildCrossTargeting/x.targets");
                packageX.AddFile("buildCrossTargeting/x.props");
                packageX.AddFile("contentFiles/any/any/_._");

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX);

                var project = projects[0];

                // Act
                var summaries = await RunRestoreAsync(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);

                // Assert
                Assert.True(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));

                var targetsXML = XDocument.Parse(File.ReadAllText(project.TargetsOutput));
                var targetItemGroups = targetsXML.Root.Elements().Where(e => e.Name.LocalName == "ImportGroup").ToList();

                var propsXML = XDocument.Parse(File.ReadAllText(project.PropsOutput));
                var propsItemGroups = propsXML.Root.Elements().Where(e => e.Name.LocalName == "ImportGroup").ToList();

                Assert.Equal("'$(TargetFramework)' == '' AND '$(ExcludeRestorePackageImports)' != 'true'", targetItemGroups[0].Attribute(XName.Get("Condition")).Value.Trim());
                Assert.Equal("'$(TargetFramework)' == '' AND '$(ExcludeRestorePackageImports)' != 'true'", propsItemGroups[0].Attribute(XName.Get("Condition")).Value.Trim());
            }
        }

        [Fact]
        public async Task RestoreBuildTargetsAndProps_VerifyRestoreNoopAsync()
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>
                {
                    new PackageSource(pathContext.PackageSource)
                };

                var spec = GetProject("projectA", "net462", "netstandard1.6");

                spec.RestoreMetadata.CrossTargeting = true;
                spec.Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange("x", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                });

                // Create fake projects, the real data is in the specs
                var projects = CreateProjectsFromSpecs(pathContext, spec);

                // Create dg file
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(spec);
                dgFile.AddRestore(spec.RestoreMetadata.ProjectUniqueName);
                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                packageX.AddFile("build/x.targets");
                packageX.AddFile("build/x.props");
                packageX.AddFile("contentFiles/any/any/_._");

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX);

                var project = projects[0];

                // First restore
                var summaries = await RunRestoreAsync(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);
                Assert.True(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));

                // Act
                var secondLogger = new TestLogger();
                summaries = await RunRestoreAsync(pathContext, secondLogger, sources, dgFile, cacheContext);
                success = summaries.All(s => s.Success);
                var messages = string.Join(Environment.NewLine, secondLogger.Messages);
                Assert.True(success, "Failed: " + messages);

                // Verify the file was not rewritten
                Assert.DoesNotContain("Generating MSBuild file", messages);
            }
        }

        [Fact]
        public async Task RestoreBuildTargetsAndProps_VerifyRestoreChangeAsync()
        {
            // Arrange
            using (var cacheContext = new SourceCacheContext())
            using (var pathContext = new SimpleTestPathContext())
            {
                var logger = new TestLogger();
                var sources = new List<PackageSource>
                {
                    new PackageSource(pathContext.PackageSource)
                };

                var spec = GetProject("projectA", "net462", "netstandard1.6");

                spec.RestoreMetadata.CrossTargeting = true;
                spec.Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange("x", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                });

                // Create fake projects, the real data is in the specs
                var projects = CreateProjectsFromSpecs(pathContext, spec);

                // Create dg file
                var dgFile = new DependencyGraphSpec();
                dgFile.AddProject(spec);
                dgFile.AddRestore(spec.RestoreMetadata.ProjectUniqueName);
                dgFile.Save(Path.Combine(pathContext.WorkingDirectory, "out.dg"));

                var packageX = new SimpleTestPackageContext()
                {
                    Id = "x",
                    Version = "1.0.0"
                };

                var packageY = new SimpleTestPackageContext()
                {
                    Id = "y",
                    Version = "1.0.0"
                };

                packageX.AddFile("build/x.targets");
                packageX.AddFile("build/x.props");
                packageX.AddFile("contentFiles/any/any/_._");

                packageY.AddFile("build/y.targets");
                packageY.AddFile("build/y.props");
                packageY.AddFile("contentFiles/any/any/_._");

                await SimpleTestPackageUtility.CreatePackagesAsync(pathContext.PackageSource, packageX, packageY);

                var project = projects[0];

                // First restore
                var summaries = await RunRestoreAsync(pathContext, logger, sources, dgFile, cacheContext);
                var success = summaries.All(s => s.Success);
                Assert.True(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));

                // Modify spec
                spec.Dependencies.Add(new LibraryDependency()
                {
                    LibraryRange = new LibraryRange("y", VersionRange.Parse("1.0.0"), LibraryDependencyTarget.Package)
                });

                // Act
                summaries = await RunRestoreAsync(pathContext, logger, sources, dgFile, cacheContext);
                success = summaries.All(s => s.Success);
                Assert.True(success, "Failed: " + string.Join(Environment.NewLine, logger.Messages));

                // Verify the file was rewritten
                Assert.Contains("y.targets", File.ReadAllText(project.TargetsOutput));
                Assert.Contains("y.props", File.ReadAllText(project.PropsOutput));
            }
        }

        private static List<SimpleTestProjectContext> CreateProjectsFromSpecs(SimpleTestPathContext pathContext, params PackageSpec[] specs)
        {
            var projects = new List<SimpleTestProjectContext>();

            foreach (var spec in specs)
            {
                var project = new SimpleTestProjectContext(spec.Name, ProjectStyle.PackageReference, pathContext.SolutionRoot);

                // Set proj properties
                spec.FilePath = project.ProjectPath;
                spec.RestoreMetadata.OutputPath = project.ProjectExtensionsPath;
                spec.RestoreMetadata.ProjectPath = project.ProjectPath;

                projects.Add(project);
            }

            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot, projects.ToArray());
            solution.Create(pathContext.SolutionRoot);

            return projects;
        }

        private static async Task<IReadOnlyList<RestoreSummary>> RunRestoreAsync(
            SimpleTestPathContext pathContext,
            TestLogger logger,
            List<PackageSource> sources,
            DependencyGraphSpec dgFile,
            SourceCacheContext cacheContext)
        {
            var restoreContext = new RestoreArgs()
            {
                CacheContext = cacheContext,
                DisableParallel = true,
                GlobalPackagesFolder = pathContext.UserPackagesFolder,
                AllowNoOp = true,
                Sources = new List<string>() { pathContext.PackageSource },
                Log = logger,
                CachingSourceProvider = new CachingSourceProvider(new TestPackageSourceProvider(sources)),
                PreLoadedRequestProviders = new List<IPreLoadedRestoreRequestProvider>()
                {
                    new DependencyGraphSpecRequestProvider(new RestoreCommandProvidersCache(), dgFile)
                }
            };

            return await RestoreRunner.RunAsync(restoreContext);
        }

        private static PackageSpec GetProject(string projectName, params string[] frameworks)
        {
            var frameworkGroups = frameworks.Select(s =>
                new TargetFrameworkInformation()
                {
                    FrameworkName = NuGetFramework.Parse(s),
                    TargetAlias = s,
                })
                .ToList();

            // Create two net45 projects
            var spec = new PackageSpec(frameworkGroups)
            {
                RestoreMetadata = new ProjectRestoreMetadata
                {
                    ProjectUniqueName = $"{projectName}-UNIQUENAME",
                    ProjectName = projectName,
                    ProjectStyle = ProjectStyle.PackageReference
                },
                Name = projectName
            };

            foreach (var framework in frameworks)
            {
                spec.RestoreMetadata.OriginalTargetFrameworks.Add(framework);
            }

            foreach (var frameworkInfo in frameworkGroups)
            {
                spec.RestoreMetadata.TargetFrameworks.Add(
                    new ProjectRestoreMetadataFrameworkInfo(frameworkInfo.FrameworkName) { TargetAlias = frameworkInfo.TargetAlias });
            }

            return spec;
        }
    }
}
