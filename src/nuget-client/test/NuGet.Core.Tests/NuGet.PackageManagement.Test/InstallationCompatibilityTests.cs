// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Moq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.DependencyResolver;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Test;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Test.Utility;
using Xunit;

namespace NuGet.PackageManagement.Test
{
    public class InstallationCompatibilityTests
    {
        [Fact]
        public async Task EnsurePackageCompatibilityAsync_ThrowsForNullNuGetProject()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => InstallationCompatibility.Instance.EnsurePackageCompatibilityAsync(
                    nuGetProject: null,
                    packageIdentity: new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0")),
                    resourceResult: new DownloadResourceResult(DownloadResourceResultStatus.NotFound),
                    cancellationToken: CancellationToken.None));

            Assert.Equal("nuGetProject", exception.ParamName);
        }

        [Fact]
        public async Task EnsurePackageCompatibilityAsync_ThrowsForNullPackageIdentity()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => InstallationCompatibility.Instance.EnsurePackageCompatibilityAsync(
                    Mock.Of<NuGetProject>(),
                    packageIdentity: null,
                    resourceResult: new DownloadResourceResult(DownloadResourceResultStatus.NotFound),
                    cancellationToken: CancellationToken.None));

            Assert.Equal("packageIdentity", exception.ParamName);
        }

        [Fact]
        public async Task EnsurePackageCompatibilityAsync_ThrowsForNullResourceResult()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => InstallationCompatibility.Instance.EnsurePackageCompatibilityAsync(
                    Mock.Of<NuGetProject>(),
                    new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0")),
                    resourceResult: null,
                    cancellationToken: CancellationToken.None));

            Assert.Equal("resourceResult", exception.ParamName);
        }

        [Fact]
        public async Task EnsurePackageCompatibilityAsync_ThrowsIfCancelled()
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => InstallationCompatibility.Instance.EnsurePackageCompatibilityAsync(
                    Mock.Of<NuGetProject>(),
                    new PackageIdentity(id: "a", version: NuGetVersion.Parse("1.0.0")),
                    new DownloadResourceResult(DownloadResourceResultStatus.NotFound),
                    new CancellationToken(canceled: true)));
        }

        [Fact]
        public async Task EnsurePackageCompatibilityAsync_WithLowerMinClientVersion_Fails()
        {
            // Arrange
            var tc = new TestContext();
            tc.MinClientVersion = new NuGetVersion("10.0.0");
            var result = new DownloadResourceResult(Stream.Null, tc.PackageReader.Object, string.Empty);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<MinClientVersionException>(() =>
                 tc.Target.EnsurePackageCompatibilityAsync(
                    tc.NuGetProject,
                    tc.PackageIdentityA,
                    result,
                    CancellationToken.None));

            Assert.Equal(
                "The 'PackageA 1.0.0' package requires NuGet client version '10.0.0' or above, " +
                $"but the current NuGet version is '{MinClientVersionUtility.GetNuGetClientVersion()}'. " +
                "To upgrade NuGet, please go to https://docs.nuget.org/consume/installing-nuget",
                ex.Message);

            tc.PackageReader.Verify(x => x.GetMinClientVersion(), Times.Never);
            tc.PackageReader.Verify(x => x.GetPackageTypes(), Times.Never);
            tc.NuspecReader.Verify(x => x.GetMinClientVersion(), Times.AtLeastOnce);
            tc.NuspecReader.Verify(x => x.GetPackageTypes(), Times.Never);
        }

        [Fact]
        public async Task EnsurePackageCompatibilityAsync_WithNuGetProject_WithInvalidType_Fails()
        {
            // Arrange
            var tc = new TestContext();
            tc.PackageTypes.Add(tc.InvalidPackageType);

            // Act & Assert
            await tc.VerifyFailureAsync(
                tc.NuGetProject,
                "Package 'PackageA 1.0.0' has a package type 'Invalid 1.2' that is not supported by project 'TestNuGetProject'.");
        }

        [Fact]
        public async Task EnsurePackageCompatibilityAsync_WithNuGetProject_WithInvalidTypeAndEmptyVersion_Fails()
        {
            // Arrange
            var tc = new TestContext();
            tc.PackageTypes.Add(new PackageType("Invalid", PackageType.EmptyVersion));

            // Act & Assert
            await tc.VerifyFailureAsync(
                tc.NuGetProject,
                "Package 'PackageA 1.0.0' has a package type 'Invalid' that is not supported by project 'TestNuGetProject'.");
        }

        [Fact]
        public async Task EnsurePackageCompatibilityAsync_WithNuGetProject_WitMultipleTypes_Fails()
        {
            // Arrange
            var tc = new TestContext();
            tc.PackageTypes.Add(PackageType.Legacy);
            tc.PackageTypes.Add(PackageType.Dependency);

            // Act & Assert
            await tc.VerifyFailureAsync(
                tc.NuGetProject,
                "Package 'PackageA 1.0.0' has multiple package types, which is not supported.");
        }

        [Fact]
        public async Task EnsurePackageCompatibilityAsync_WithNuGetProject_WithDotnetCliToolType_Fails()
        {
            // Arrange
            var tc = new TestContext();
            tc.PackageTypes.Add(PackageType.DotnetCliTool);

            // Act & Assert
            await tc.VerifyFailureAsync(
                tc.NuGetProject,
                "Package 'PackageA 1.0.0' has a package type 'DotnetCliTool' that is not supported by project 'TestNuGetProject'.");
        }

        [Fact]
        public async Task EnsurePackageCompatibilityAsync_WithNuGetProject_WithLegacyType_Succeeds()
        {
            // Arrange
            var tc = new TestContext();
            tc.PackageTypes.Add(PackageType.Legacy);

            // Act & Assert
            await tc.VerifySuccessAsync(tc.NuGetProject);
        }

        [Fact]
        public async Task EnsurePackageCompatibilityAsync_WithNuGetProject_WithDependency_Succeeds()
        {
            // Arrange
            var tc = new TestContext();
            tc.PackageTypes.Add(PackageType.Dependency);

            // Act & Assert
            await tc.VerifySuccessAsync(tc.NuGetProject);
        }

        private class TestContext
        {
            public TestContext(string userPackageFolder = null)
            {
                // Dependencies
                SourceRepository = new SourceRepository(new PackageSource("http://example/index.json"), Enumerable.Empty<INuGetResourceProvider>());

                PackageIdentityA = new PackageIdentity("PackageA", NuGetVersion.Parse("1.0.0"));
                PackageIdentityB = new PackageIdentity("PackageB", NuGetVersion.Parse("1.0.0"));
                PackageIdentityC = new PackageIdentity("PackageC", NuGetVersion.Parse("1.0.0"));
                PackageTypes = new List<PackageType>();
                MinClientVersion = new NuGetVersion(2, 0, 0);

                InvalidPackageType = new PackageType("Invalid", new Version(1, 2));

                NuGetProject = new TestNuGetProject(new List<PackageReference>());

                NuGetPathContext = new Mock<INuGetPathContext>();

                NuspecReader = new Mock<NuspecReader>(new XDocument());

                PackageReader = new Mock<PackageReaderBase>(
                    new FrameworkNameProvider(new[] { DefaultFrameworkMappings.Instance },
                    new[] { DefaultPortableFrameworkMappings.Instance }))
                {
                    CallBase = true
                };

                // Setup
                NuGetPathContext
                    .Setup(x => x.UserPackageFolder)
                    .Returns(userPackageFolder);
                NuGetPathContext
                    .Setup(x => x.FallbackPackageFolders)
                    .Returns(Array.Empty<string>());

                NuspecReader
                    .Setup(p => p.GetIdentity())
                    .Returns(() => PackageIdentityA);
                NuspecReader
                    .Setup(p => p.GetMinClientVersion())
                    .Returns(() => MinClientVersion);
                NuspecReader
                    .Setup(p => p.GetPackageTypes())
                    .Returns(() => PackageTypes);

                PackageReader
                    .Setup(p => p.GetIdentity())
                    .Returns(() => PackageIdentityA);
                PackageReader
                    .Setup(p => p.GetMinClientVersion())
                    .Returns(() => MinClientVersion);
                PackageReader
                    .Setup(p => p.GetPackageTypes())
                    .Returns(() => PackageTypes);

                PackageReader
                    .Setup(p => p.NuspecReader)
                    .Returns(() => NuspecReader.Object);

                Target = new InstallationCompatibility();
            }

            public SourceRepository SourceRepository { get; }
            public PackageIdentity PackageIdentityA { get; }
            public PackageIdentity PackageIdentityB { get; }
            public PackageIdentity PackageIdentityC { get; }
            public List<PackageType> PackageTypes { get; }
            public NuGetVersion MinClientVersion { get; set; }
            public TestNuGetProject NuGetProject { get; }
            public Mock<INuGetPathContext> NuGetPathContext { get; }
            public Mock<PackageReaderBase> PackageReader { get; }
            public Mock<NuspecReader> NuspecReader { get; }
            public InstallationCompatibility Target { get; }
            public PackageType InvalidPackageType { get; }

            public async Task VerifyFailureAsync(
                NuGetProject nugetProject,
                string expected)
            {
                // Arrange
                var result = new DownloadResourceResult(Stream.Null, PackageReader.Object, string.Empty);

                // Act & Assert
                var ex = await Assert.ThrowsAsync<PackagingException>(() =>
                     Target.EnsurePackageCompatibilityAsync(
                        nugetProject,
                        PackageIdentityA,
                        result,
                        CancellationToken.None));

                Assert.Equal(expected, ex.Message);
                PackageReader.Verify(x => x.GetMinClientVersion(), Times.Never);
                PackageReader.Verify(x => x.GetPackageTypes(), Times.Never);
                NuspecReader.Verify(x => x.GetMinClientVersion(), Times.Once);
                NuspecReader.Verify(x => x.GetPackageTypes(), Times.Once);
            }

            public async Task VerifySuccessAsync(NuGetProject nugetProject)
            {
                // Arrange
                var result = new DownloadResourceResult(Stream.Null, PackageReader.Object, string.Empty);

                // Act & Assert
                await Target.EnsurePackageCompatibilityAsync(
                    nugetProject,
                    PackageIdentityA,
                    result,
                    CancellationToken.None);

                PackageReader.Verify(x => x.GetMinClientVersion(), Times.Never);
                PackageReader.Verify(x => x.GetPackageTypes(), Times.Never);
                NuspecReader.Verify(x => x.GetMinClientVersion(), Times.Once);
                NuspecReader.Verify(x => x.GetPackageTypes(), Times.Once);
            }

            public RestoreResult GetRestoreResult(IEnumerable<PackageIdentity> identities)
            {
                var node = new GraphNode<RemoteResolveResult>(new LibraryRange("project", LibraryDependencyTarget.All))
                {
                    Item = new GraphItem<RemoteResolveResult>(
                        new LibraryIdentity(
                            "project",
                            new NuGetVersion("1.0.0"),
                            LibraryType.Project))
                    {
                        Data = new RemoteResolveResult
                        {
                            Match = new RemoteMatch
                            {
                                Provider = null
                            }
                        }
                    }
                };

                foreach (var identity in identities)
                {
                    var dependencyNode = new GraphNode<RemoteResolveResult>(
                        new LibraryRange(
                            identity.Id,
                            new VersionRange(identity.Version),
                            LibraryDependencyTarget.All))
                    {
                        Item = new GraphItem<RemoteResolveResult>(
                            new LibraryIdentity(
                                identity.Id,
                                identity.Version,
                                LibraryType.Package))
                        {
                            Data = new RemoteResolveResult
                            {
                                Match = new RemoteMatch
                                {
                                    Provider = null
                                }
                            }
                        }
                    };

                    dependencyNode.OuterNode = node;
                    node.InnerNodes.Add(dependencyNode);
                }

                var graph = RestoreTargetGraph.Create(
                    new[] { node },
                    new TestRemoteWalkContext(),
                    NullLogger.Instance,
                    FrameworkConstants.CommonFrameworks.NetStandard10);

                return new RestoreResult(
                    true,
                    new[] { graph },
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    ProjectStyle.Unknown,
                    TimeSpan.MinValue);
            }
        }
    }
}
