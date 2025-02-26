// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using FluentAssertions;
using Microsoft.Arcade.Common;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Feed.Tests.TestDoubles;
using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.DotNet.VersionTools.BuildManifest.Model;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests
{
    public class BuildModelFactoryTests
    {
        #region Standard test values

        private const string _testAzdoRepoUri = "https://dnceng@dev.azure.com/dnceng/internal/_git/dotnet-buildtest";
        private const string _normalizedTestAzdoRepoUri = "https://dev.azure.com/dnceng/internal/_git/dotnet-buildtest";
        private const string _testRepoOrigin = "emsdk";
        private const string _testBuildBranch = "foobranch";
        private const string _testBuildCommit = "664996a16fa9228cfd7a55d767deb31f62a65f51";
        private const string _testAzdoBuildId = "89999999";
        private const string _testInitialLocation = "https://dnceng.visualstudio.com/project/_apis/build/builds/id/artifacts";
        private const string _normalizedTestInitialLocation = "https://dev.azure.com/dnceng/project/_apis/build/builds/id/artifacts";
        private static readonly string[] _defaultManifestBuildData = new string[]
        {
            $"InitialAssetsLocation={_testInitialLocation}",
            $"AzureDevOpsRepository={_testAzdoRepoUri}"
        };

        #endregion

        readonly TaskLoggingHelper _taskLoggingHelper;
        readonly MockBuildEngine _buildEngine;
        readonly StubTask _stubTask;
        readonly BuildModelFactory _buildModelFactory;
        readonly FileSystem _fileSystem;

        public BuildModelFactoryTests()
        {
            _buildEngine = new MockBuildEngine();
            _stubTask = new StubTask(_buildEngine);
            _taskLoggingHelper = new TaskLoggingHelper(_stubTask);

            ServiceProvider provider = new ServiceCollection()
                .AddSingleton<IBlobArtifactModelFactory, BlobArtifactModelFactory>()
                .AddSingleton<IPackageArtifactModelFactory, PackageArtifactModelFactory>()
                .AddSingleton<IPdbArtifactModelFactory, PdbArtifactModelFactory>()
                .AddSingleton<IPackageArchiveReaderFactory, PackageArchiveReaderFactory>()
                .AddSingleton<INupkgInfoFactory, NupkgInfoFactory>()
                .AddSingleton<IFileSystem, FileSystem>()
                .AddSingleton(typeof(BuildModelFactory))
                .AddSingleton(_taskLoggingHelper)
                .BuildServiceProvider();
            
            _buildModelFactory = ActivatorUtilities.CreateInstance<BuildModelFactory>(provider);
            _fileSystem = ActivatorUtilities.CreateInstance<FileSystem>(provider);
        }

        /// <summary>
        /// A model with no input artifacts is invalid
        /// </summary>
        [Fact]
        public void AttemptToCreateModelWithNoArtifactsFails()
        {
            Action act = () =>
                _buildModelFactory.CreateModel(artifacts: null,
                                               artifactVisibilitiesToInclude: ArtifactVisibility.All,
                                               buildId: _testAzdoBuildId,
                                               manifestBuildData: null,
                                               repoUri: _testAzdoRepoUri,
                                               repoBranch: _testBuildBranch,
                                               repoCommit: _testBuildCommit,
                                               repoOrigin: _testRepoOrigin,
                                               isStableBuild: false,
                                               publishingVersion: PublishingInfraVersion.Latest,
                                               isReleaseOnlyPackageVersion: true);
            act.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Relatively unified test of manifest artifact parsing. Focuses on 3 things:
        /// - That symbol packages are correctly identified as blobs in the correct locations
        /// - Blobs and packages are split into appropriate categories
        /// - Artifact metadata is preserved, which includes the attributes
        /// 
        /// Because there is a ton of overlap between the individual tests of this functionality
        /// (they essentially all need to verify the same permutations), these tests are combined into
        /// one.
        /// </summary>
        [Fact]
        public void ManifestArtifactParsingTest()
        {
            var localPackagePath = TestInputs.GetFullPath(Path.Combine("Nupkgs", "test-package-a.1.0.0.nupkg"));

            const string bopSymbolsNupkg = "foo/bar/baz/bop.symbols.nupkg";
            string bobSymbolsExpectedId = $"assets/symbols/{Path.GetFileName(bopSymbolsNupkg)}";
            const string bopSnupkg = "foo/bar/baz/bop.symbols.nupkg";
            string bopSnupkgExpectedId = $"assets/symbols/{Path.GetFileName(bopSnupkg)}";
            const string zipArtifact = "foo/bar/baz/bing.zip";
            // New PDB artifact
            const string pdbArtifact = "foo/bar/baz/bing.pdb";

            var artifacts = new ITaskItem[]
            {
                new TaskItem(bopSymbolsNupkg, new Dictionary<string, string>
                {
                    { "ManifestArtifactData", "NonShipping=true;Category=SMORKELER" },
                    { "RelativeBlobPath", bobSymbolsExpectedId},
                    { "ThisIsntArtifactMetadata", "YouGoofed!" },
                    { "Kind", "Blob" }
                }),
                new TaskItem(bopSnupkg, new Dictionary<string, string>
                {
                    { "ManifestArtifactData", "NonShipping=false;Category=SNORPKEG;" },
                    { "RelativeBlobPath", bopSnupkgExpectedId},
                    { "Kind", "Blob" },
                }),
                // Include a package and a fake zip too
                // Note that the relative blob path is a "first class" attribute,
                // not parsed from ManifestArtifactData
                new TaskItem(zipArtifact, new Dictionary<string, string>
                {
                    { "RelativeBlobPath", zipArtifact },
                    { "ManifestArtifactData", "ARandomBitOfMAD=" },
                    { "Kind", "Blob" }
                }),
                new TaskItem(localPackagePath, new Dictionary<string, string>()
                {
                    // This isn't recognized or used for a nupkg
                    { "RelativeBlobPath", zipArtifact },
                    { "ManifestArtifactData", "ShouldWePushDaNorpKeg=YES" },
                    { "Kind", "Package" }
                }),
                // New PDB artifact with RelativePdbPath
                new TaskItem(pdbArtifact, new Dictionary<string, string>
                {
                    { "RelativePdbPath", pdbArtifact },
                    { "ManifestArtifactData", "NonShipping=false;Category=PDB" },
                    { "Kind", "PDB" }
                })
            };

            var model = _buildModelFactory.CreateModel(artifacts: artifacts,
                                                       artifactVisibilitiesToInclude: ArtifactVisibility.All,
                                                       buildId: _testAzdoBuildId,
                                                       manifestBuildData: _defaultManifestBuildData,
                                                       repoUri: _testAzdoRepoUri,
                                                       repoBranch: _testBuildBranch,
                                                       repoCommit: _testBuildCommit,
                                                       repoOrigin: _testRepoOrigin,
                                                       isStableBuild: false,
                                                       publishingVersion: PublishingInfraVersion.Latest,
                                                       isReleaseOnlyPackageVersion: true);

            model.Should().NotBeNull();
            _taskLoggingHelper.HasLoggedErrors.Should().BeFalse();

            // When Maestro sees a symbol package, it is supposed to re-do the symbol package path to
            // be assets/symbols/<file-name>
            model.Artifacts.Blobs.Should().SatisfyRespectively(
                blob =>
                {
                    blob.Id.Should().Be(bobSymbolsExpectedId);
                    blob.NonShipping.Should().BeTrue();
                    blob.Attributes.Should().Contain("NonShipping", "true");
                    blob.Attributes.Should().Contain("Category", "SMORKELER");
                    blob.Attributes.Should().Contain("Id", bobSymbolsExpectedId);
                    blob.Attributes.Should().Contain("RepoOrigin", _testRepoOrigin);
                },
                blob =>
                {
                    blob.Id.Should().Be(bopSnupkgExpectedId);
                    blob.NonShipping.Should().BeFalse();
                    blob.Attributes.Should().Contain("NonShipping", "false");
                    blob.Attributes.Should().Contain("Category", "SNORPKEG");
                    blob.Attributes.Should().Contain("Id", bopSnupkgExpectedId);
                    blob.Attributes.Should().Contain("RepoOrigin", _testRepoOrigin);
                },
                blob =>
                {
                    blob.Id.Should().Be(zipArtifact);
                    blob.NonShipping.Should().BeFalse();
                    blob.Attributes.Should().Contain("ARandomBitOfMAD", string.Empty);
                    blob.Attributes.Should().Contain("Id", zipArtifact);
                    blob.Attributes.Should().Contain("RepoOrigin", _testRepoOrigin);
                });

            model.Artifacts.Packages.Should().SatisfyRespectively(
                package =>
                {
                    package.Id.Should().Be("test-package-a");
                    package.Version.Should().Be("1.0.0");
                    package.NonShipping.Should().BeFalse();
                    package.Attributes.Should().Contain("ShouldWePushDaNorpKeg", "YES");
                    package.Attributes.Should().Contain("Id", "test-package-a");
                    package.Attributes.Should().Contain("Version", "1.0.0");
                    package.Attributes.Should().Contain("RepoOrigin", _testRepoOrigin);
                });

            // New verification for the PDB artifact
            model.Artifacts.Pdbs.Should().SatisfyRespectively(
                pdb =>
                {
                    pdb.Id.Should().Be(pdbArtifact);
                    pdb.NonShipping.Should().BeFalse();
                    pdb.Attributes.Should().Contain("NonShipping", "false");
                    pdb.Attributes.Should().Contain("Category", "PDB");
                    pdb.Attributes.Should().Contain("Id", pdbArtifact);
                    pdb.Attributes.Should().Contain("RepoOrigin", _testRepoOrigin);
                }
            );

            model.Identity.Attributes.Should().Contain("AzureDevOpsRepository", _normalizedTestAzdoRepoUri);
        }

        /// <summary>
        /// The artifact metadata is parsed as case-insensitive
        /// </summary>
        [Fact]
        public void ArtifactMetadataIsCaseInsensitive()
        {
            var localPackagePath = TestInputs.GetFullPath(Path.Combine("Nupkgs", "test-package-a.1.0.0.nupkg"));

            var artifacts = new ITaskItem[]
            {
                new TaskItem(localPackagePath, new Dictionary<string, string>()
                {
                    { "ManifestArtifactData", "nonshipping=true;Category=CASE" },
                    { "Kind", "Package" }
                })
            };

            var model = _buildModelFactory.CreateModel(artifacts: artifacts,
                                                       artifactVisibilitiesToInclude: ArtifactVisibility.All,
                                                       buildId: _testAzdoBuildId,
                                                       manifestBuildData: _defaultManifestBuildData,
                                                       repoUri: _testAzdoRepoUri,
                                                       repoBranch: _testBuildBranch,
                                                       repoCommit: _testBuildCommit,
                                                       repoOrigin: _testRepoOrigin,
                                                       isStableBuild: false,
                                                       publishingVersion: PublishingInfraVersion.Latest,
                                                       isReleaseOnlyPackageVersion: true);

            model.Should().NotBeNull();
            _taskLoggingHelper.HasLoggedErrors.Should().BeFalse();

            model.Artifacts.Blobs.Should().BeEmpty();
            model.Artifacts.Packages.Should().SatisfyRespectively(
                package =>
                {
                    package.Id.Should().Be("test-package-a");
                    package.Version.Should().Be("1.0.0");
                    // We used "nonshipping=true" in our artifact metadata
                    package.NonShipping.Should().BeTrue();
                    package.Attributes.Should().Contain("nonshipping", "true");
                    package.Attributes.Should().Contain("Category", "CASE");
                    package.Attributes.Should().Contain("Id", "test-package-a");
                    package.Attributes.Should().Contain("Version", "1.0.0");
                    package.Attributes.Should().Contain("RepoOrigin", _testRepoOrigin);
                });
        }

        

        /// <summary>
        /// We can't create a blob artifact model without a RelativeBlobPath
        /// </summary>
        [Fact]
        public void BlobsWithoutARelativeBlobPathIsInvalid()
        {
            const string zipArtifact = "foo/bar/baz/bing.zip";

            var artifacts = new ITaskItem[]
            {
                // Include a package and a fake zip too
                // Note that the relative blob path is a "first class" attribute,
                // not parsed from ManifestArtifactData
                new TaskItem(zipArtifact, new Dictionary<string, string>
                {
                    { "ManifestArtifactData", "ARandomBitOfMAD=" },
                    { "Kind", "Blob" },
                }),
            };

            _buildModelFactory.CreateModel(artifacts: artifacts,
                                           artifactVisibilitiesToInclude: ArtifactVisibility.All,
                                           buildId: _testAzdoBuildId,
                                           manifestBuildData: _defaultManifestBuildData,
                                           repoUri: _testAzdoRepoUri,
                                           repoBranch: _testBuildBranch,
                                           repoCommit: _testBuildCommit,
                                           repoOrigin: _testRepoOrigin,
                                           isStableBuild: false,
                                           publishingVersion: PublishingInfraVersion.Latest,
                                           isReleaseOnlyPackageVersion: true);

            _taskLoggingHelper.HasLoggedErrors.Should().BeTrue();
            _buildEngine.BuildErrorEvents.Should().Contain(e => e.Message.Equals($"Missing 'RelativeBlobPath' property on blob {zipArtifact}"));
        }

        /// <summary>
        /// We can't create a PDB artifact model without a RelativePdbPath
        /// </summary>
        [Fact]
        public void PdbsWithoutARelativePdbPathIsInvalid()
        {
            const string pdbArtifact = "foo/bar/baz/bing.pdb";

            var artifacts = new ITaskItem[]
            {
                // Include a package and a fake pdb too
                // Note that the relative pdb path is a "first class" attribute,
                // not parsed from ManifestArtifactData
                new TaskItem(pdbArtifact, new Dictionary<string, string>
                {
                    { "ManifestArtifactData", "ARandomBitOfMAD=" },
                    { "Kind", "PDB" },
                }),
            };

            _buildModelFactory.CreateModel(artifacts: artifacts,
                                           artifactVisibilitiesToInclude: ArtifactVisibility.All,
                                           buildId: _testAzdoBuildId,
                                           manifestBuildData: _defaultManifestBuildData,
                                           repoUri: _testAzdoRepoUri,
                                           repoBranch: _testBuildBranch,
                                           repoCommit: _testBuildCommit,
                                           repoOrigin: _testRepoOrigin,
                                           isStableBuild: false,
                                           publishingVersion: PublishingInfraVersion.Latest,
                                           isReleaseOnlyPackageVersion: true);

            _taskLoggingHelper.HasLoggedErrors.Should().BeTrue();
            _buildEngine.BuildErrorEvents.Should().Contain(e => e.Message.Equals($"Missing 'RelativePdbPath' property on pdb {pdbArtifact}"));
        }

        /// <summary>
        /// Test that a build without initial location information is rejected
        /// </summary>
        [Fact]
        public void MissingLocationInformationThrowsError()
        {
            var localPackagePath = TestInputs.GetFullPath(Path.Combine("Nupkgs", "test-package-a.zip"));

            var artifacts = new ITaskItem[]
            {
                new TaskItem(localPackagePath, new Dictionary<string, string>
                {
                    { "ManifestArtifactData", "NonShipping=true" },
                    { "Kind", "Package" }
                })
            };

            _buildModelFactory.CreateModel(artifacts: artifacts,
                                           artifactVisibilitiesToInclude: ArtifactVisibility.All,
                                           buildId: _testAzdoBuildId,
                                           manifestBuildData: null,
                                           repoUri: _testAzdoRepoUri,
                                           repoBranch: _testBuildBranch,
                                           repoCommit: _testBuildCommit,
                                           repoOrigin: _testRepoOrigin,
                                           isStableBuild: false,
                                           publishingVersion: PublishingInfraVersion.Latest,
                                           isReleaseOnlyPackageVersion: true);

            // Should have logged an error that an initial location was not present.
            _taskLoggingHelper.HasLoggedErrors.Should().BeTrue();
            _buildEngine.BuildErrorEvents.Should().Contain(e => e.Message.Equals("Missing 'location' property from ManifestBuildData"));
        }

        /// <summary>
        /// Test that a build with initial location attributes in the manifest build data
        /// are accepted.
        /// </summary>
        [Theory]
        [InlineData("Location")]
        [InlineData("InitialAssetsLocation")]
        public void InitialLocationInformationAttributesAreAccepted(string attributeName)
        {
            var localPackagePath = TestInputs.GetFullPath(Path.Combine("Nupkgs", "test-package-a.1.0.0.nupkg"));

            var artifacts = new ITaskItem[]
            {
                new TaskItem(localPackagePath, new Dictionary<string, string>
                {
                    { "ManifestArtifactData", "NonShipping=true" },
                    { "Kind", "Package" }
                })
            };

            var manifestBuildData = new string[]
            {
                $"{attributeName}={_testInitialLocation}"
            };

            var model = _buildModelFactory.CreateModel(artifacts: artifacts,
                                                       artifactVisibilitiesToInclude: ArtifactVisibility.All,
                                                       buildId: _testAzdoBuildId,
                                                       manifestBuildData: manifestBuildData,
                                                       repoUri: _testAzdoRepoUri,
                                                       repoBranch: _testBuildBranch,
                                                       repoCommit: _testBuildCommit,
                                                       repoOrigin: _testRepoOrigin,
                                                       isStableBuild: false,
                                                       publishingVersion: PublishingInfraVersion.Latest,
                                                       isReleaseOnlyPackageVersion: true);

            // Should have logged an error that an initial location was not present.
            _taskLoggingHelper.HasLoggedErrors.Should().BeFalse();

            // Check that the build model has the initial assets location
            model.Identity.Attributes.Should().Contain(attributeName, _normalizedTestInitialLocation);
        }

        /// <summary>
        /// Basic round trip from model -> xml -> model has the desired results.
        /// There is already tests for the xml bits of this in the model itself (BuildManifestModel just wraps it
        /// with some file writing).
        /// 
        /// This also tests a few extra cases, like that the additional metadata (e.g. repo uri)
        /// are correctly modeled and preserved.
        /// </summary>
        [Fact]
        public void RoundTripFromTaskItemsToFileToXml()
        {
            var localPackagePath = TestInputs.GetFullPath(Path.Combine("Nupkgs", "test-package-a.1.0.0.nupkg"));

            const string bopSymbolsNupkg = "foo/bar/baz/bop.symbols.nupkg";
            string bobSymbolsExpectedId = $"assets/symbols/{Path.GetFileName(bopSymbolsNupkg)}";
            const string bopSnupkg = "foo/bar/baz/bop.symbols.nupkg";
            string bopSnupkgExpectedId = $"assets/symbols/{Path.GetFileName(bopSnupkg)}";
            const string zipArtifact = "foo/bar/baz/bing.zip";
            // New PDB artifact
            const string pdbArtifact = "foo/bar/baz/bing.pdb";

            var artifacts = new ITaskItem[]
            {
                new TaskItem(bopSymbolsNupkg, new Dictionary<string, string>
                {
                    { "ManifestArtifactData", "NonShipping=true;Category=SMORKELER" },
                    { "RelativeBlobPath", bobSymbolsExpectedId},
                    { "ThisIsntArtifactMetadata", "YouGoofed!" },
                    { "Kind", "Blob" }
                }),
                new TaskItem(bopSnupkg, new Dictionary<string, string>
                {
                    { "ManifestArtifactData", "NonShipping=false;Category=SNORPKEG;" },
                    { "RelativeBlobPath", bopSnupkgExpectedId},
                    { "Kind", "Blob" },
                }),
                // Include a package and a fake zip too
                // Note that the relative blob path is a "first class" attribute,
                // not parsed from ManifestArtifactData
                new TaskItem(zipArtifact, new Dictionary<string, string>
                {
                    { "RelativeBlobPath", zipArtifact },
                    { "ManifestArtifactData", "ARandomBitOfMAD=" },
                    { "Kind", "Blob" }
                }),
                new TaskItem(localPackagePath, new Dictionary<string, string>()
                {
                    // This isn't recognized or used for a nupkg
                    { "RelativeBlobPath", zipArtifact },
                    { "ManifestArtifactData", "ShouldWePushDaNorpKeg=YES" },
                    { "Kind", "Package" }
                }),
                // New PDB artifact with RelativePdbPath
                new TaskItem(pdbArtifact, new Dictionary<string, string>
                {
                    { "RelativePdbPath", pdbArtifact },
                    { "ManifestArtifactData", "NonShipping=false;Category=PDB" },
                    { "Kind", "PDB" }
                })
            };

            string tempXmlFile = Path.GetTempFileName();
            try
            {
                var modelFromItems = _buildModelFactory.CreateModel(artifacts: artifacts,
                                                                    artifactVisibilitiesToInclude: ArtifactVisibility.All,
                                                                    buildId: _testAzdoBuildId,
                                                                    manifestBuildData: _defaultManifestBuildData,
                                                                    repoUri: _testAzdoRepoUri,
                                                                    repoBranch: _testBuildBranch,
                                                                    repoCommit: _testBuildCommit,
                                                                    repoOrigin: _testRepoOrigin,
                                                                    isStableBuild: true,
                                                                    publishingVersion: PublishingInfraVersion.Latest,
                                                                    isReleaseOnlyPackageVersion: false);

                modelFromItems.Should().NotBeNull();
                _taskLoggingHelper.HasLoggedErrors.Should().BeFalse();

                // Write to file
                _fileSystem.WriteToFile(tempXmlFile, modelFromItems.ToXml().ToString(SaveOptions.DisableFormatting));

                // Read the xml file back in and create a model from it.
                var modelFromFile = _buildModelFactory.ManifestFileToModel(tempXmlFile);

                // There will be some reordering of the attributes here (they are written to the xml file in
                // a defined order for some properties, then ordered by case).
                // As a result, this comparison isn't exactly the same as some other tests.
                _taskLoggingHelper.HasLoggedErrors.Should().BeFalse();
                modelFromItems.Identity.Name.Should().Be(_testAzdoRepoUri);
                modelFromItems.Identity.BuildId.Should().Be(_testAzdoBuildId);
                modelFromItems.Identity.Commit.Should().Be(_testBuildCommit);
                modelFromItems.Identity.PublishingVersion.Should().Be(VersionTools.BuildManifest.Model.PublishingInfraVersion.Latest);
                modelFromItems.Identity.IsReleaseOnlyPackageVersion.Should().BeFalse();
                modelFromItems.Identity.IsStable.Should().BeTrue();
                modelFromFile.Artifacts.Blobs.Should().SatisfyRespectively(
                    blob =>
                    {
                        blob.Id.Should().Be(bobSymbolsExpectedId);
                        blob.NonShipping.Should().BeTrue();
                        blob.Attributes.Should().Contain("Id", bobSymbolsExpectedId);
                        blob.Attributes.Should().Contain("Category", "SMORKELER");
                        blob.Attributes.Should().Contain("NonShipping", "true");
                        blob.Attributes.Should().Contain("RepoOrigin", _testRepoOrigin);
                    },
                    blob =>
                    {
                        blob.Id.Should().Be(bopSnupkgExpectedId);
                        blob.NonShipping.Should().BeFalse();
                        blob.Attributes.Should().Contain("Id", bopSnupkgExpectedId);
                        blob.Attributes.Should().Contain("Category", "SNORPKEG");
                        blob.Attributes.Should().Contain("NonShipping", "false");
                        blob.Attributes.Should().Contain("RepoOrigin", _testRepoOrigin);
                    },
                    blob =>
                    {
                        blob.Id.Should().Be(zipArtifact);
                        blob.NonShipping.Should().BeFalse();
                        blob.Attributes.Should().Contain("Id", zipArtifact);
                        blob.Attributes.Should().Contain("ARandomBitOfMAD", string.Empty);
                        blob.Attributes.Should().Contain("RepoOrigin", _testRepoOrigin);
                    });

                modelFromFile.Artifacts.Packages.Should().SatisfyRespectively(
                    package =>
                    {
                        package.Id.Should().Be("test-package-a");
                        package.Version.Should().Be("1.0.0");
                        package.NonShipping.Should().BeFalse();
                        package.Attributes.Should().Contain("Id", "test-package-a");
                        package.Attributes.Should().Contain("Version", "1.0.0");
                        package.Attributes.Should().Contain("ShouldWePushDaNorpKeg", "YES");
                        package.Attributes.Should().Contain("RepoOrigin", _testRepoOrigin);
                    });

                // New verification for the PDB artifact round trip
                modelFromFile.Artifacts.Pdbs.Should().SatisfyRespectively(
                    pdb =>
                    {
                        pdb.Id.Should().Be(pdbArtifact);
                        pdb.NonShipping.Should().BeFalse();
                        pdb.Attributes.Should().Contain("NonShipping", "false");
                        pdb.Attributes.Should().Contain("Category", "PDB");
                        pdb.Attributes.Should().Contain("Id", pdbArtifact);
                        pdb.Attributes.Should().Contain("RepoOrigin", _testRepoOrigin);
                    }
                );
            }
            finally
            {
                if (File.Exists(tempXmlFile))
                {
                    File.Delete(tempXmlFile);
                }
            }
        }

        /// <summary>
        /// Validates that errors are logged and null is returned when Kind metadata is missing from an artifact.
        /// </summary>
        [Fact]
        public void CreateModel_MissingKindMetadata_ReturnsNullAndLogsError()
        {
            // Arrange
            var artifacts = new ITaskItem[]
            {
                new TaskItem("missingKindArtifact.nupkg", new Dictionary<string, string>
                {
                    { "ManifestArtifactData", "NonShipping=true;Category=TEST" }
                    // "Kind" metadata is intentionally missing
                })
            };

            // Act
            var model = _buildModelFactory.CreateModel(
                artifacts: artifacts,
                artifactVisibilitiesToInclude: ArtifactVisibility.All,
                buildId: _testAzdoBuildId,
                manifestBuildData: _defaultManifestBuildData,
                repoUri: _testAzdoRepoUri,
                repoBranch: _testBuildBranch,
                repoCommit: _testBuildCommit,
                repoOrigin: _testRepoOrigin,
                isStableBuild: false,
                publishingVersion: PublishingInfraVersion.Latest,
                isReleaseOnlyPackageVersion: true);

            // Assert
            model.Should().BeNull();
            _taskLoggingHelper.HasLoggedErrors.Should().BeTrue();
            _buildEngine.BuildErrorEvents.Should().Contain(e => e.Message.Contains("Missing 'Kind' property on artifact missingKindArtifact.nupkg"));
        }
    }
}
