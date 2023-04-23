// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Arcade.Common;
using Microsoft.Arcade.Test.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks.Feed.Tests.TestDoubles;
using Microsoft.DotNet.VersionTools.Automation;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using FluentAssertions;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests
{
    public class BuildModelFactoryTests
    {
        #region Standard test values

        private const string _testAzdoRepoUri = "https://dnceng@dev.azure.com/dnceng/internal/_git/dotnet-buildtest";
        private const string _normalizedTestAzdoRepoUri = "https://dev.azure.com/dnceng/internal/_git/dotnet-buildtest";
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

        public BuildModelFactoryTests()
        {
            _buildEngine = new MockBuildEngine();
            _stubTask = new StubTask(_buildEngine);
            _taskLoggingHelper = new TaskLoggingHelper(_stubTask);

            ServiceProvider provider = new ServiceCollection()
                .AddSingleton<ISigningInformationModelFactory, SigningInformationModelFactory>()
                .AddSingleton<IBlobArtifactModelFactory, BlobArtifactModelFactory>()
                .AddSingleton<IPackageArtifactModelFactory, PackageArtifactModelFactory>()
                .AddSingleton<IPackageArchiveReaderFactory, PackageArchiveReaderFactory>()
                .AddSingleton<INupkgInfoFactory, NupkgInfoFactory>()
                .AddSingleton<IFileSystem, FileSystem>()
                .AddSingleton(typeof(BuildModelFactory))
                .AddSingleton(_taskLoggingHelper)
                .BuildServiceProvider();
            
            _buildModelFactory = ActivatorUtilities.CreateInstance<BuildModelFactory>(provider);
        }

        #region Artifact related tests
        /// <summary>
        /// A model with no input artifacts is invalid
        /// </summary>
        [Fact]
        public void AttemptToCreateModelWithNoArtifactsFails()
        {
            Action act = () =>
                _buildModelFactory.CreateModelFromItems(null, null,
                null, null, null, null, _testAzdoBuildId, null, _testAzdoRepoUri, _testBuildBranch, _testBuildCommit, false,
                VersionTools.BuildManifest.Model.PublishingInfraVersion.Latest,
                true);
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

            var artifacts = new ITaskItem[]
            {
                new TaskItem(bopSymbolsNupkg, new Dictionary<string, string>
                {
                    { "ManifestArtifactData", "NonShipping=true;Category=SMORKELER" },
                    { "ThisIsntArtifactMetadata", "YouGoofed!" }
                }),
                new TaskItem(bopSnupkg, new Dictionary<string, string>
                {
                    { "ManifestArtifactData", "NonShipping=false;Category=SNORPKEG;" }
                }),
                // Include a package and a fake zip too
                // Note that the relative blob path is a "first class" attribute,
                // not parsed from ManifestArtifactData
                new TaskItem(zipArtifact, new Dictionary<string, string>
                {
                    { "RelativeBlobPath", zipArtifact },
                    { "ManifestArtifactData", "ARandomBitOfMAD=" },
                }),
                new TaskItem(localPackagePath, new Dictionary<string, string>()
                {
                    // This isn't recognized or used for a nupkg
                    { "RelativeBlobPath", zipArtifact },
                    { "ManifestArtifactData", "ShouldWePushDaNorpKeg=YES" },
                })
            };

            var model = _buildModelFactory.CreateModelFromItems(artifacts, null,
                null, null, null, null, _testAzdoBuildId, _defaultManifestBuildData, _testAzdoRepoUri, _testBuildBranch, _testBuildCommit, false,
                VersionTools.BuildManifest.Model.PublishingInfraVersion.Latest,
                true);

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
                },
                blob =>
                {
                    blob.Id.Should().Be(bopSnupkgExpectedId);
                    blob.NonShipping.Should().BeFalse();
                    blob.Attributes.Should().Contain("NonShipping", "false");
                    blob.Attributes.Should().Contain("Category", "SNORPKEG");
                    blob.Attributes.Should().Contain("Id", bopSnupkgExpectedId);
                },
                blob =>
                {
                    blob.Id.Should().Be(zipArtifact);
                    blob.NonShipping.Should().BeFalse();
                    blob.Attributes.Should().Contain("ARandomBitOfMAD", string.Empty);
                    blob.Attributes.Should().Contain("Id", zipArtifact);
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
                });

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
                })
            };

            var model = _buildModelFactory.CreateModelFromItems(artifacts, null,
                null, null, null, null, _testAzdoBuildId, _defaultManifestBuildData, _testAzdoRepoUri, _testBuildBranch, _testBuildCommit, false,
                VersionTools.BuildManifest.Model.PublishingInfraVersion.Latest,
                true);

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
                }),
            };

            _buildModelFactory.CreateModelFromItems(artifacts, null,
                null, null, null, null, _testAzdoBuildId, _defaultManifestBuildData, _testAzdoRepoUri, _testBuildBranch, _testBuildCommit, false,
                VersionTools.BuildManifest.Model.PublishingInfraVersion.Latest,
                true);

            _taskLoggingHelper.HasLoggedErrors.Should().BeTrue();
            _buildEngine.BuildErrorEvents.Should().Contain(e => e.Message.Equals($"Missing 'RelativeBlobPath' property on blob {zipArtifact}"));
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
                    { "ManifestArtifactData", "NonShipping=true" }
                })
            };

            _buildModelFactory.CreateModelFromItems(artifacts, null,
                null, null, null, null, _testAzdoBuildId, null, _testAzdoRepoUri, _testBuildBranch, _testBuildCommit, false,
                VersionTools.BuildManifest.Model.PublishingInfraVersion.Latest,
                true);

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
                    { "ManifestArtifactData", "NonShipping=true" }
                })
            };

            var manifestBuildData = new string[]
            {
                $"{attributeName}={_testInitialLocation}"
            };

            var model = _buildModelFactory.CreateModelFromItems(artifacts, null,
                null, null, null, null, _testAzdoBuildId, manifestBuildData, _testAzdoRepoUri, _testBuildBranch, _testBuildCommit, false,
                VersionTools.BuildManifest.Model.PublishingInfraVersion.Latest,
                true);

            // Should have logged an error that an initial location was not present.
            _taskLoggingHelper.HasLoggedErrors.Should().BeFalse();

            // Check that the build model has the initial assets location
            model.Identity.Attributes.Should().Contain(attributeName, _normalizedTestInitialLocation);
        }

        #endregion

        #region Round trip tests

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

            var artifacts = new ITaskItem[]
            {
                new TaskItem(bopSymbolsNupkg, new Dictionary<string, string>
                {
                    { "ManifestArtifactData", "NonShipping=true;Category=SMORKELER" },
                    { "ThisIsntArtifactMetadata", "YouGoofed!" }
                }),
                new TaskItem(bopSnupkg, new Dictionary<string, string>
                {
                    { "ManifestArtifactData", "NonShipping=false;Category=SNORPKEG;" }
                }),
                // Include a package and a fake zip too
                // Note that the relative blob path is a "first class" attribute,
                // not parsed from ManifestArtifactData
                new TaskItem(zipArtifact, new Dictionary<string, string>
                {
                    { "RelativeBlobPath", zipArtifact },
                    { "ManifestArtifactData", "ARandomBitOfMAD=" },
                }),
                new TaskItem(localPackagePath, new Dictionary<string, string>()
                {
                    // This isn't recognized or used for a nupkg
                    { "RelativeBlobPath", zipArtifact },
                    { "ManifestArtifactData", "ShouldWePushDaNorpKeg=YES" },
                })
            };

            var itemsToSign = new ITaskItem[]
            {
                new TaskItem(localPackagePath),
                new TaskItem(zipArtifact)
            };

            var strongNameSignInfo = new ITaskItem[]
            {
                new TaskItem(localPackagePath, new Dictionary<string, string>()
                {
                    { "CertificateName", "IHasACert" },
                    { "PublicKeyToken", "abcdabcdabcdabcd" }
                })
            };

            var fileSignInfo = new ITaskItem[]
            {
                new TaskItem(localPackagePath, new Dictionary<string, string>()
                {
                    { "CertificateName", "IHasACert2" }
                }),
                // Added per issue: dotnet/arcade#7064
                new TaskItem("Microsoft.DiaSymReader.dll", new Dictionary<string, string>()
                {
                    { "CertificateName", "MicrosoftWin8WinBlue" },
                    { "TargetFramework", ".NETFramework,Version=v2.0" },
                    { "PublicKeyToken", "31bf3856ad364e35" }
                }),
                new TaskItem("Microsoft.DiaSymReader.dll", new Dictionary<string, string>()
                {
                    { "CertificateName", "Microsoft101240624" }, // lgtm [cs/common-default-passwords] Safe, these are certificate names
                    { "TargetFramework", ".NETStandard,Version=v1.1" },
                    { "PublicKeyToken", "31bf3856ad364e35" }
                })
            };

            var certificatesSignInfo = new ITaskItem[]
            {
                new TaskItem("MyCert", new Dictionary<string, string>()
                {
                    { "DualSigningAllowed", "false" }
                }),
                new TaskItem("MyOtherCert", new Dictionary<string, string>()
                {
                    { "DualSigningAllowed", "true" }
                })
            };

            var fileExtensionSignInfo = new ITaskItem[]
            {
                new TaskItem(".dll", new Dictionary<string, string>()
                {
                    { "CertificateName", "MyCert" }
                })
            };

            string tempXmlFile = Path.GetTempFileName();
            try
            {
                var modelFromItems = _buildModelFactory.CreateModelFromItems(artifacts, itemsToSign,
                    strongNameSignInfo, fileSignInfo, fileExtensionSignInfo, certificatesSignInfo, _testAzdoBuildId,
                    _defaultManifestBuildData, _testAzdoRepoUri, _testBuildBranch, _testBuildCommit, true,
                    VersionTools.BuildManifest.Model.PublishingInfraVersion.Latest,
                    false);

                _buildModelFactory.CreateBuildManifest(
                        modelFromItems.Artifacts.Blobs,
                        modelFromItems.Artifacts.Packages,
                        tempXmlFile,
                        modelFromItems.Identity.Name,
                        modelFromItems.Identity.BuildId,
                        modelFromItems.Identity.Branch,
                        modelFromItems.Identity.Commit,
                        modelFromItems.Identity.Attributes.Select(kv => $"{kv.Key}={kv.Value}").ToArray(),
                        modelFromItems.Identity.IsStable,
                        modelFromItems.Identity.PublishingVersion,
                        modelFromItems.Identity.IsReleaseOnlyPackageVersion,
                        modelFromItems.SigningInformation);

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
                    },
                    blob =>
                    {
                        blob.Id.Should().Be(bopSnupkgExpectedId);
                        blob.NonShipping.Should().BeFalse();
                        blob.Attributes.Should().Contain("Id", bopSnupkgExpectedId);
                        blob.Attributes.Should().Contain("Category", "SNORPKEG");
                        blob.Attributes.Should().Contain("NonShipping", "false");
                    },
                    blob =>
                    {
                        blob.Id.Should().Be(zipArtifact);
                        blob.NonShipping.Should().BeFalse();
                        blob.Attributes.Should().Contain("Id", zipArtifact);
                        blob.Attributes.Should().Contain("ARandomBitOfMAD", string.Empty);
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
                    });

                modelFromFile.SigningInformation.Should().NotBeNull();
                modelFromFile.SigningInformation.ItemsToSign.Should().SatisfyRespectively(
                    item =>
                    {
                        item.Include.Should().Be("bing.zip");
                    },
                    item =>
                    {
                        item.Include.Should().Be("test-package-a.1.0.0.nupkg");
                    });
                modelFromFile.SigningInformation.StrongNameSignInfo.Should().SatisfyRespectively(
                    item =>
                    {
                        item.Include.Should().Be("test-package-a.1.0.0.nupkg");
                        item.CertificateName.Should().Be("IHasACert");
                        item.PublicKeyToken.Should().Be("abcdabcdabcdabcd");
                    });
                modelFromFile.SigningInformation.FileSignInfo.Should().SatisfyRespectively(
                    item =>
                    {
                        item.Include.Should().Be("Microsoft.DiaSymReader.dll");
                        item.CertificateName.Should().Be("Microsoft101240624"); // lgtm [cs/common-default-passwords] Safe, these certificate names
                        item.TargetFramework.Should().Be(".NETStandard,Version=v1.1");
                        item.PublicKeyToken.Should().Be("31bf3856ad364e35");
                    },
                    item =>
                    {
                        item.Include.Should().Be("Microsoft.DiaSymReader.dll");
                        item.CertificateName.Should().Be("MicrosoftWin8WinBlue");
                        item.TargetFramework.Should().Be(".NETFramework,Version=v2.0");
                        item.PublicKeyToken.Should().Be("31bf3856ad364e35");
                    },
                    item =>
                    {
                        item.Include.Should().Be("test-package-a.1.0.0.nupkg");
                        item.CertificateName.Should().Be("IHasACert2");
                    });
                modelFromFile.SigningInformation.CertificatesSignInfo.Should().SatisfyRespectively(
                    item =>
                    {
                        item.Include.Should().Be("MyCert");
                        item.DualSigningAllowed.Should().Be(false);
                    },
                    item =>
                    {
                        item.Include.Should().Be("MyOtherCert");
                        item.DualSigningAllowed.Should().Be(true);
                    });
                modelFromFile.SigningInformation.FileExtensionSignInfo.Should().SatisfyRespectively(
                    item =>
                    {
                        item.Include.Should().Be(".dll");
                        item.CertificateName.Should().Be("MyCert");
                    });
            }
            finally
            {
                if (File.Exists(tempXmlFile))
                {
                    File.Delete(tempXmlFile);
                }
            }
        }

        #endregion

        #region Signing Model Information

        /// <summary>
        /// If no signing information is present, we should still get a signing model,
        /// with nothing in it
        /// </summary>
        [Fact]
        public void NoSigningInformationDoesNotThrowAnError()
        {
            var localPackagePath = TestInputs.GetFullPath(Path.Combine("Nupkgs", "test-package-a.1.0.0.nupkg"));

            var artifacts = new ITaskItem[]
            {
                new TaskItem(localPackagePath, new Dictionary<string, string>()
                {
                    { "ManifestArtifactData", "nonshipping=true;Category=CASE" },
                })
            };

            var model = _buildModelFactory.CreateModelFromItems(artifacts, null,
                null, null, null, null, _testAzdoBuildId, _defaultManifestBuildData, _testAzdoRepoUri, _testBuildBranch, _testBuildCommit, false,
                VersionTools.BuildManifest.Model.PublishingInfraVersion.Latest,
                true);

            _taskLoggingHelper.HasLoggedErrors.Should().BeFalse();
            model.SigningInformation.Should().NotBeNull();
            model.SigningInformation.ItemsToSign.Should().BeEmpty();
            model.SigningInformation.CertificatesSignInfo.Should().BeEmpty();
            model.SigningInformation.FileExtensionSignInfo.Should().BeEmpty();
            model.SigningInformation.FileSignInfo.Should().BeEmpty();
            model.SigningInformation.StrongNameSignInfo.Should().BeEmpty();
        }

        /// <summary>
        /// Validate the strong name signing information is correctly propagated to the model
        /// </summary>
        [Fact]
        public void SignInfoIsCorrectlyPopulatedFromItems()
        {
            var localPackagePath = TestInputs.GetFullPath(Path.Combine("Nupkgs", "test-package-a.1.0.0.nupkg"));
            var zipPath = @"this/is/a/zip.zip";

            var artifacts = new ITaskItem[]
            {
                new TaskItem(localPackagePath, new Dictionary<string, string>()),
                new TaskItem(zipPath, new Dictionary<string, string>()
                {
                    { "RelativeBlobPath", zipPath },
                })
            };

            var itemsToSign = new ITaskItem[]
            {
                new TaskItem(localPackagePath),
                new TaskItem(zipPath)
            };

            var strongNameSignInfo = new ITaskItem[]
            {
                new TaskItem(localPackagePath, new Dictionary<string, string>()
                {
                    { "CertificateName", "IHasACert" },
                    { "PublicKeyToken", "abcdabcdabcdabcd" }
                })
            };

            var fileSignInfo = new ITaskItem[]
            {
                new TaskItem(localPackagePath, new Dictionary<string, string>()
                {
                    { "CertificateName", "IHasACert2" }
                })
            };

            var certificatesSignInfo = new ITaskItem[]
            {
                new TaskItem("MyCert", new Dictionary<string, string>()
                {
                    { "DualSigningAllowed", "false" }
                }),
                new TaskItem("MyOtherCert", new Dictionary<string, string>()
                {
                    { "DualSigningAllowed", "true" }
                })
            };

            var fileExtensionSignInfo = new ITaskItem[]
            {
                new TaskItem(".dll", new Dictionary<string, string>()
                {
                    { "CertificateName", "MyCert" }
                })
            };

            var model = _buildModelFactory.CreateModelFromItems(artifacts, itemsToSign,
                strongNameSignInfo, fileSignInfo, fileExtensionSignInfo, certificatesSignInfo,
                _testAzdoBuildId, _defaultManifestBuildData, _testAzdoRepoUri, _testBuildBranch, _testBuildCommit, false,
                VersionTools.BuildManifest.Model.PublishingInfraVersion.Latest,
                true);

            _taskLoggingHelper.HasLoggedErrors.Should().BeFalse();
            model.SigningInformation.Should().NotBeNull();
            model.SigningInformation.ItemsToSign.Should().SatisfyRespectively(
                item =>
                {
                    item.Include.Should().Be("test-package-a.1.0.0.nupkg");
                },
                item =>
                {
                    item.Include.Should().Be("zip.zip");
                });
            model.SigningInformation.StrongNameSignInfo.Should().SatisfyRespectively(
                item =>
                {
                    item.Include.Should().Be("test-package-a.1.0.0.nupkg");
                    item.CertificateName.Should().Be("IHasACert");
                    item.PublicKeyToken.Should().Be("abcdabcdabcdabcd");
                });
            model.SigningInformation.FileSignInfo.Should().SatisfyRespectively(
                item =>
                {
                    item.Include.Should().Be("test-package-a.1.0.0.nupkg");
                    item.CertificateName.Should().Be("IHasACert2");
                });
            model.SigningInformation.CertificatesSignInfo.Should().SatisfyRespectively(
                item =>
                {
                    item.Include.Should().Be("MyCert");
                    item.DualSigningAllowed.Should().Be(false);
                },
                item =>
                {
                    item.Include.Should().Be("MyOtherCert");
                    item.DualSigningAllowed.Should().Be(true);
                });
            model.SigningInformation.FileExtensionSignInfo.Should().SatisfyRespectively(
                item =>
                {
                    item.Include.Should().Be(".dll");
                    item.CertificateName.Should().Be("MyCert");
                });
        }

        /// <summary>
        /// If a file is in ItemsToSign, it should also be in the artifacts.
        /// </summary>
        [Fact]
        public void ArtifactToSignMustExistInArtifacts()
        {
            var localPackagePath = TestInputs.GetFullPath(Path.Combine("Nupkgs", "test-package-a.1.0.0.nupkg"));
            const string zipPath = @"this/is/a/zip.zip";
            const string bogusNupkgToSign = "totallyboguspackage.nupkg";

            var artifacts = new ITaskItem[]
            {
                new TaskItem(localPackagePath, new Dictionary<string, string>()),
                new TaskItem(zipPath, new Dictionary<string, string>()
                {
                    { "RelativeBlobPath", zipPath },
                })
            };

            var itemsToSign = new ITaskItem[]
            {
                new TaskItem(bogusNupkgToSign),
                new TaskItem(Path.GetFileName(zipPath)),
                new TaskItem(Path.GetFileName(localPackagePath)),
            };

            var model = _buildModelFactory.CreateModelFromItems(artifacts, itemsToSign,
                null, null, null, null,
                _testAzdoBuildId, _defaultManifestBuildData, _testAzdoRepoUri, _testBuildBranch, _testBuildCommit, false,
                VersionTools.BuildManifest.Model.PublishingInfraVersion.Latest,
                true);

            _taskLoggingHelper.HasLoggedErrors.Should().BeTrue();
            _buildEngine.BuildErrorEvents.Should().HaveCount(1);
            _buildEngine.BuildErrorEvents.Should().Contain(e => e.Message.Equals($"Item to sign '{bogusNupkgToSign}' was not found in the artifacts"));
        }

        #endregion
    }
}
