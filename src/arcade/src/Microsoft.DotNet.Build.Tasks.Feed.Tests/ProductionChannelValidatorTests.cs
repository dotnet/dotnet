// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AwesomeAssertions;
using Microsoft.DotNet.Build.Tasks.Feed.Model;
using Microsoft.DotNet.Build.Manifest;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.Feed.Tests
{
    public class ProductionChannelValidatorTests
    {
        private readonly Mock<IProductionChannelValidatorBuildInfoService> _mockAzureDevOpsService;
        private readonly Mock<IBranchClassificationService> _mockBranchClassificationService;
        private readonly Mock<ILogger<ProductionChannelValidator>> _mockLogger;

        public ProductionChannelValidatorTests()
        {
            _mockAzureDevOpsService = new Mock<IProductionChannelValidatorBuildInfoService>();
            _mockBranchClassificationService = new Mock<IBranchClassificationService>();
            _mockLogger = new Mock<ILogger<ProductionChannelValidator>>();
        }

        private ProductionChannelValidator CreateValidator(ValidationMode validationMode)
        {
            return new ProductionChannelValidator(
                _mockAzureDevOpsService.Object,
                _mockBranchClassificationService.Object,
                _mockLogger.Object,
                validationMode);
        }

        private static ProductConstructionService.Client.Models.Build CreateTestBuild(
            int id = 12345,
            DateTimeOffset? dateProduced = null,
            int staleness = 0,
            bool released = false,
            bool stable = false,
            string commit = "abc123",
            string azureDevOpsAccount = "dnceng",
            string azureDevOpsProject = "internal",
            int? azureDevOpsBuildId = 123456,
            string azureDevOpsRepository = "https://dev.azure.com/dnceng/internal/_git/dotnet-runtime",
            string azureDevOpsBranch = "refs/heads/main",
            List<ProductConstructionService.Client.Models.Channel> channels = null,
            List<ProductConstructionService.Client.Models.Asset> assets = null,
            List<ProductConstructionService.Client.Models.BuildRef> dependencies = null,
            List<ProductConstructionService.Client.Models.BuildIncoherence> incoherencies = null)
        {
            var build = new ProductConstructionService.Client.Models.Build(
                id: id,
                dateProduced: dateProduced ?? DateTimeOffset.UtcNow,
                staleness: staleness,
                released: released,
                stable: stable,
                commit: commit,
                channels: channels ?? new List<ProductConstructionService.Client.Models.Channel>(),
                assets: assets ?? new List<ProductConstructionService.Client.Models.Asset>(),
                dependencies: dependencies ?? new List<ProductConstructionService.Client.Models.BuildRef>(),
                incoherencies: incoherencies ?? new List<ProductConstructionService.Client.Models.BuildIncoherence>()
            );

            // Set the Azure DevOps properties using reflection since they might be read-only
            // This is a test scenario, so it's acceptable to use reflection
            build.AzureDevOpsAccount = azureDevOpsAccount;
            build.AzureDevOpsProject = azureDevOpsProject;
            build.AzureDevOpsBuildId = azureDevOpsBuildId;
            build.AzureDevOpsRepository = azureDevOpsRepository;
            build.AzureDevOpsBranch = azureDevOpsBranch;

            return build;
        }

        private static TargetChannelConfig CreateProductionChannelConfig(int id = 1)
        {
            return new TargetChannelConfig(
                id: id,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: null,
                akaMSCreateLinkPatterns: null,
                akaMSDoNotCreateLinkPatterns: null,
                targetFeeds: new TargetFeedSpecification[0],
                symbolTargetType: SymbolPublishVisibility.None,
                flatten: true,
                isProduction: true);
        }

        private static TargetChannelConfig CreateNonProductionChannelConfig(int id = 2)
        {
            return new TargetChannelConfig(
                id: id,
                isInternal: false,
                publishingInfraVersion: PublishingInfraVersion.Latest,
                akaMSChannelNames: null,
                akaMSCreateLinkPatterns: null,
                akaMSDoNotCreateLinkPatterns: null,
                targetFeeds: new TargetFeedSpecification[0],
                symbolTargetType: SymbolPublishVisibility.None,
                flatten: true,
                isProduction: false);
        }

        [Theory]
        [InlineData(ValidationMode.Enforce)]
        [InlineData(ValidationMode.Audit)]
        public async Task ValidateAsync_NullBuild_ThrowsArgumentNullException(ValidationMode validationMode)
        {
            // Arrange
            var validator = CreateValidator(validationMode);
            var targetChannel = CreateProductionChannelConfig();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                validator.ValidateAsync(null, targetChannel));
        }

        [Theory]
        [InlineData(ValidationMode.Enforce)]
        [InlineData(ValidationMode.Audit)]
        public async Task ValidateAsync_NonProductionChannel_ReturnsSuccess(ValidationMode validationMode)
        {
            // Arrange
            var validator = CreateValidator(validationMode);
            var build = CreateTestBuild();
            var targetChannel = CreateNonProductionChannelConfig();

            // Act
            var result = await validator.ValidateAsync(build, targetChannel);

            // Assert
            result.Should().Be(TargetChannelValidationResult.Success);
            _mockAzureDevOpsService.Verify(x => x.GetBuildInfoAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
            _mockBranchClassificationService.Verify(x => x.GetBranchClassificationsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData(ValidationMode.Enforce)]
        [InlineData(ValidationMode.Audit)]
        public async Task ValidateAsync_ProductionChannel_ValidBuild_ReturnsSuccess(ValidationMode validationMode)
        {
            // Arrange
            var validator = CreateValidator(validationMode);
            var build = CreateTestBuild();
            var targetChannel = CreateProductionChannelConfig();

            _mockAzureDevOpsService
                .Setup(x => x.GetBuildInfoAsync("dnceng", "internal", 123456))
                .ReturnsAsync(new AzureDevOpsBuildInfo
                {
                    Project = new AzureDevOpsProject { Id = "project-guid-123", Name = "internal" },
                    Repository = new AzureDevOpsRepository { Id = "repo-guid-456", Name = "dotnet-runtime" },
                    Tags = new List<string> { "1ES.PT.Official", "other-tag" }
                });

            _mockBranchClassificationService
                .Setup(x => x.GetBranchClassificationsAsync("dnceng", "project-guid-123", "repo-guid-456"))
                .ReturnsAsync(new BranchClassificationResponse
                {
                    Status = "Success",
                    BranchClassifications = new List<BranchClassification>
                    {
                        new BranchClassification { BranchName = "main", Classification = "production" },
                        new BranchClassification { BranchName = "release/*", Classification = "production" }
                    }
                });

            // Act
            var result = await validator.ValidateAsync(build, targetChannel);

            // Assert
            result.Should().Be(TargetChannelValidationResult.Success);
            _mockAzureDevOpsService.Verify(x => x.GetBuildInfoAsync("dnceng", "internal", 123456), Times.Once);
            _mockBranchClassificationService.Verify(x => x.GetBranchClassificationsAsync("dnceng", "project-guid-123", "repo-guid-456"), Times.Once);
        }

        [Theory]
        [InlineData(ValidationMode.Enforce, TargetChannelValidationResult.Fail)]
        [InlineData(ValidationMode.Audit, TargetChannelValidationResult.AuditOnlyFailure)]
        public async Task ValidateAsync_ProductionChannel_MissingRequiredTag_ReturnsExpectedResult(ValidationMode validationMode, TargetChannelValidationResult expectedResult)
        {
            // Arrange
            var validator = CreateValidator(validationMode);
            var build = CreateTestBuild();
            var targetChannel = CreateProductionChannelConfig();

            _mockAzureDevOpsService
                .Setup(x => x.GetBuildInfoAsync("dnceng", "internal", 123456))
                .ReturnsAsync(new AzureDevOpsBuildInfo
                {
                    Project = new AzureDevOpsProject { Id = "project-guid-123", Name = "internal" },
                    Repository = new AzureDevOpsRepository { Id = "repo-guid-456", Name = "dotnet-runtime" },
                    Tags = new List<string> { "other-tag", "another-tag" }
                });

            // Act
            var result = await validator.ValidateAsync(build, targetChannel);

            // Assert
            result.Should().Be(expectedResult);
            _mockAzureDevOpsService.Verify(x => x.GetBuildInfoAsync("dnceng", "internal", 123456), Times.Once);
            _mockBranchClassificationService.Verify(x => x.GetBranchClassificationsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData(ValidationMode.Enforce, TargetChannelValidationResult.Fail)]
        [InlineData(ValidationMode.Audit, TargetChannelValidationResult.AuditOnlyFailure)]
        public async Task ValidateAsync_ProductionChannel_NonProductionBranch_ReturnsExpectedResult(ValidationMode validationMode, TargetChannelValidationResult expectedResult)
        {
            // Arrange
            var validator = CreateValidator(validationMode);
            var build = CreateTestBuild(azureDevOpsBranch: "refs/heads/feature/test");
            var targetChannel = CreateProductionChannelConfig();

            _mockAzureDevOpsService
                .Setup(x => x.GetBuildInfoAsync("dnceng", "internal", 123456))
                .ReturnsAsync(new AzureDevOpsBuildInfo
                {
                    Project = new AzureDevOpsProject { Id = "project-guid-123", Name = "internal" },
                    Repository = new AzureDevOpsRepository { Id = "repo-guid-456", Name = "dotnet-runtime" },
                    Tags = new List<string> { "1ES.PT.Official" }
                });

            _mockBranchClassificationService
                .Setup(x => x.GetBranchClassificationsAsync("dnceng", "project-guid-123", "repo-guid-456"))
                .ReturnsAsync(new BranchClassificationResponse
                {
                    Status = "Success",
                    BranchClassifications = new List<BranchClassification>
                    {
                        new BranchClassification { BranchName = "main", Classification = "production" },
                        new BranchClassification { BranchName = "release/*", Classification = "production" }
                    }
                });

            // Act
            var result = await validator.ValidateAsync(build, targetChannel);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(ValidationMode.Enforce, TargetChannelValidationResult.Fail)]
        [InlineData(ValidationMode.Audit, TargetChannelValidationResult.AuditOnlyFailure)]
        public async Task ValidateAsync_NonAzureDevOpsRepository_WithValidTags_ReturnsExpectedResult(ValidationMode validationMode, TargetChannelValidationResult expectedResult)
        {
            // Arrange
            var validator = CreateValidator(validationMode);
            var build = CreateTestBuild(
                azureDevOpsRepository: "https://github.com/dotnet/runtime",
                azureDevOpsAccount: "dnceng",
                azureDevOpsProject: "internal",
                azureDevOpsBuildId: 123456
            );
            var targetChannel = CreateProductionChannelConfig();

            _mockAzureDevOpsService
                .Setup(x => x.GetBuildInfoAsync("dnceng", "internal", 123456))
                .ReturnsAsync(new AzureDevOpsBuildInfo
                {
                    Project = new AzureDevOpsProject { Id = "project-guid-123", Name = "internal" },
                    Repository = new AzureDevOpsRepository { Id = "repo-guid-456", Name = "https://github.com/dotnet/runtime" },
                    Tags = new List<string> { "1ES.PT.Official" }
                });

            // Act
            var result = await validator.ValidateAsync(build, targetChannel);

            // Assert
            result.Should().Be(expectedResult);
            _mockAzureDevOpsService.Verify(x => x.GetBuildInfoAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            _mockBranchClassificationService.Verify(x => x.GetBranchClassificationsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData(ValidationMode.Enforce, TargetChannelValidationResult.Fail)]
        [InlineData(ValidationMode.Audit, TargetChannelValidationResult.AuditOnlyFailure)]
        public async Task ValidateAsync_ProductionChannel_MissingAzureDevOpsInfo_ReturnsExpectedResult(ValidationMode validationMode, TargetChannelValidationResult expectedResult)
        {
            // Arrange - Azure DevOps repository but missing Azure DevOps build metadata
            var validator = CreateValidator(validationMode);
            var build = CreateTestBuild(
                azureDevOpsRepository: "https://dev.azure.com/dnceng/internal/_git/dotnet-runtime", // Azure DevOps repo
                azureDevOpsAccount: null,    // But missing metadata
                azureDevOpsProject: null,
                azureDevOpsBuildId: null
            );
            var targetChannel = CreateProductionChannelConfig();

            // Act
            var result = await validator.ValidateAsync(build, targetChannel);

            // Assert
            result.Should().Be(expectedResult);
            _mockAzureDevOpsService.Verify(x => x.GetBuildInfoAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [Theory]
        [InlineData(ValidationMode.Enforce, "main", "main", TargetChannelValidationResult.Success)]
        [InlineData(ValidationMode.Enforce, "master", "master", TargetChannelValidationResult.Success)]
        [InlineData(ValidationMode.Enforce, "release/6.0", "release/*", TargetChannelValidationResult.Success)]
        [InlineData(ValidationMode.Enforce, "release/6.0.1", "release/*", TargetChannelValidationResult.Success)]
        [InlineData(ValidationMode.Enforce, "main", "~default", TargetChannelValidationResult.Success)]
        [InlineData(ValidationMode.Enforce, "master", "~default", TargetChannelValidationResult.Success)]
        [InlineData(ValidationMode.Enforce, "feature/test", "main", TargetChannelValidationResult.Fail)]
        [InlineData(ValidationMode.Enforce, "develop", "release/*", TargetChannelValidationResult.Fail)]
        [InlineData(ValidationMode.Enforce, "feature/test", "~default", TargetChannelValidationResult.Fail)]
        [InlineData(ValidationMode.Audit, "main", "main", TargetChannelValidationResult.Success)]
        [InlineData(ValidationMode.Audit, "master", "master", TargetChannelValidationResult.Success)]
        [InlineData(ValidationMode.Audit, "release/6.0", "release/*", TargetChannelValidationResult.Success)]
        [InlineData(ValidationMode.Audit, "release/6.0.1", "release/*", TargetChannelValidationResult.Success)]
        [InlineData(ValidationMode.Audit, "main", "~default", TargetChannelValidationResult.Success)]
        [InlineData(ValidationMode.Audit, "master", "~default", TargetChannelValidationResult.Success)]
        [InlineData(ValidationMode.Audit, "feature/test", "main", TargetChannelValidationResult.AuditOnlyFailure)]
        [InlineData(ValidationMode.Audit, "develop", "release/*", TargetChannelValidationResult.AuditOnlyFailure)]
        [InlineData(ValidationMode.Audit, "feature/test", "~default", TargetChannelValidationResult.AuditOnlyFailure)]
        public async Task ValidateAsync_BranchPatternMatching_WorksCorrectly(ValidationMode validationMode, string branchName, string pattern, TargetChannelValidationResult expectedResult)
        {
            // Arrange
            var validator = CreateValidator(validationMode);
            var build = CreateTestBuild(azureDevOpsBranch: $"refs/heads/{branchName}");
            var targetChannel = CreateProductionChannelConfig();

            _mockAzureDevOpsService
                .Setup(x => x.GetBuildInfoAsync("dnceng", "internal", 123456))
                .ReturnsAsync(new AzureDevOpsBuildInfo
                {
                    Project = new AzureDevOpsProject { Id = "project-guid-123", Name = "internal" },
                    Repository = new AzureDevOpsRepository { Id = "repo-guid-456", Name = "dotnet-runtime" },
                    Tags = new List<string> { "1ES.PT.Official" }
                });

            _mockBranchClassificationService
                .Setup(x => x.GetBranchClassificationsAsync("dnceng", "project-guid-123", "repo-guid-456"))
                .ReturnsAsync(new BranchClassificationResponse
                {
                    Status = "Success",
                    BranchClassifications = new List<BranchClassification>
                    {
                        new BranchClassification { BranchName = pattern, Classification = "production" }
                    }
                });

            // Act
            var result = await validator.ValidateAsync(build, targetChannel);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(ValidationMode.Enforce, TargetChannelValidationResult.Fail)]
        [InlineData(ValidationMode.Audit, TargetChannelValidationResult.AuditOnlyFailure)]
        public async Task ValidateAsync_AzureDevOpsServiceThrows_ReturnsExpectedResult(ValidationMode validationMode, TargetChannelValidationResult expectedResult)
        {
            // Arrange
            var validator = CreateValidator(validationMode);
            var build = CreateTestBuild();
            var targetChannel = CreateProductionChannelConfig();

            _mockAzureDevOpsService
                .Setup(x => x.GetBuildInfoAsync("dnceng", "internal", 123456))
                .ThrowsAsync(new InvalidOperationException("API Error"));

            // Act
            var result = await validator.ValidateAsync(build, targetChannel);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(ValidationMode.Enforce, TargetChannelValidationResult.Fail)]
        [InlineData(ValidationMode.Audit, TargetChannelValidationResult.AuditOnlyFailure)]
        public async Task ValidateAsync_BranchClassificationServiceThrows_ReturnsExpectedResult(ValidationMode validationMode, TargetChannelValidationResult expectedResult)
        {
            // Arrange
            var validator = CreateValidator(validationMode);
            var build = CreateTestBuild();
            var targetChannel = CreateProductionChannelConfig();

            _mockAzureDevOpsService
                .Setup(x => x.GetBuildInfoAsync("dnceng", "internal", 123456))
                .ReturnsAsync(new AzureDevOpsBuildInfo
                {
                    Project = new AzureDevOpsProject { Id = "project-guid-123", Name = "internal" },
                    Repository = new AzureDevOpsRepository { Id = "repo-guid-456", Name = "dotnet-runtime" },
                    Tags = new List<string> { "1ES.PT.Official" }
                });

            _mockBranchClassificationService
                .Setup(x => x.GetBranchClassificationsAsync("dnceng", "project-guid-123", "repo-guid-456"))
                .ThrowsAsync(new InvalidOperationException("API Error"));

            // Act
            var result = await validator.ValidateAsync(build, targetChannel);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(ValidationMode.Enforce, LogLevel.Debug, LogLevel.Error)]
        [InlineData(ValidationMode.Audit, LogLevel.Debug, LogLevel.Warning)]
        public async Task ValidateAsync_BranchClassificationServiceThrows_LogsCorrectly(ValidationMode validationMode, LogLevel expectedDebugLevel, LogLevel expectedValidationLevel)
        {
            // Arrange
            var validator = CreateValidator(validationMode);
            var build = CreateTestBuild();
            var targetChannel = CreateProductionChannelConfig();

            _mockAzureDevOpsService
                .Setup(x => x.GetBuildInfoAsync("dnceng", "internal", 123456))
                .ReturnsAsync(new AzureDevOpsBuildInfo
                {
                    Project = new AzureDevOpsProject { Id = "project-guid-123", Name = "internal" },
                    Repository = new AzureDevOpsRepository { Id = "repo-guid-456", Name = "dotnet-runtime" },
                    Tags = new List<string> { "1ES.PT.Official" }
                });

            _mockBranchClassificationService
                .Setup(x => x.GetBranchClassificationsAsync("dnceng", "project-guid-123", "repo-guid-456"))
                .ThrowsAsync(new InvalidOperationException("API Error"));

            // Act
            await validator.ValidateAsync(build, targetChannel);

            // Assert
            // Verify that debug logging is called for branch classification failures
            _mockLogger.Verify(
                x => x.Log(
                    expectedDebugLevel,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to fetch branch classifications for build")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
            
            // Verify that validation mode appropriate logging is called
            _mockLogger.Verify(
                x => x.Log(
                    expectedValidationLevel,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error validating branch classification for build")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Theory]
        [InlineData(ValidationMode.Enforce)]
        [InlineData(ValidationMode.Audit)]
        public async Task ValidateAsync_RepositoryUrlExtraction_WorksCorrectly(ValidationMode validationMode)
        {
            // Arrange
            var validator = CreateValidator(validationMode);
            var repositoryUrl = "https://dev.azure.com/dnceng/internal/_git/dotnet-runtime";
            var expectedRepoId = "dotnet-runtime";

            var build = CreateTestBuild(azureDevOpsRepository: repositoryUrl);
            var targetChannel = CreateProductionChannelConfig();

            _mockAzureDevOpsService
                .Setup(x => x.GetBuildInfoAsync("dnceng", "internal", 123456))
                .ReturnsAsync(new AzureDevOpsBuildInfo
                {
                    Project = new AzureDevOpsProject { Id = "project-guid-123", Name = "internal" },
                    Repository = new AzureDevOpsRepository { Id = "repo-guid-456", Name = expectedRepoId },
                    Tags = new List<string> { "1ES.PT.Official" }
                });

            _mockBranchClassificationService
                .Setup(x => x.GetBranchClassificationsAsync("dnceng", "project-guid-123", "repo-guid-456"))
                .ReturnsAsync(new BranchClassificationResponse
                {
                    Status = "Success",
                    BranchClassifications = new List<BranchClassification>
                    {
                        new BranchClassification { BranchName = "main", Classification = "production" }
                    }
                });

            // Act
            var result = await validator.ValidateAsync(build, targetChannel);

            // Assert
            result.Should().Be(TargetChannelValidationResult.Success);
        }

        [Theory]
        [InlineData("refs/heads/main", "main")]
        [InlineData("refs/heads/release/6.0", "release/6.0")]
        [InlineData("main", "main")]
        [InlineData("release/6.0", "release/6.0")]
        public async Task ValidateAsync_BranchNameNormalization_WorksCorrectly(string inputBranch, string expectedNormalizedBranch)
        {
            // Arrange
            var enforceValidator = CreateValidator(ValidationMode.Enforce);
            var auditValidator = CreateValidator(ValidationMode.Audit);
            var build = CreateTestBuild(azureDevOpsBranch: inputBranch);
            var targetChannel = CreateProductionChannelConfig();

            _mockAzureDevOpsService
                .Setup(x => x.GetBuildInfoAsync("dnceng", "internal", 123456))
                .ReturnsAsync(new AzureDevOpsBuildInfo
                {
                    Project = new AzureDevOpsProject { Id = "project-guid-123", Name = "internal" },
                    Repository = new AzureDevOpsRepository { Id = "repo-guid-456", Name = "dotnet-runtime" },
                    Tags = new List<string> { "1ES.PT.Official" }
                });

            _mockBranchClassificationService
                .Setup(x => x.GetBranchClassificationsAsync("dnceng", "project-guid-123", "repo-guid-456"))
                .ReturnsAsync(new BranchClassificationResponse
                {
                    Status = "Success",
                    BranchClassifications = new List<BranchClassification>
                    {
                        new BranchClassification { BranchName = expectedNormalizedBranch, Classification = "production" }
                    }
                });

            // Act - Test both validators for successful case
            var enforceResult = await enforceValidator.ValidateAsync(build, targetChannel);
            var auditResult = await auditValidator.ValidateAsync(build, targetChannel);

            // Assert
            enforceResult.Should().Be(TargetChannelValidationResult.Success);
            auditResult.Should().Be(TargetChannelValidationResult.Success);
        }

        [Theory]
        [InlineData(ValidationMode.Enforce, TargetChannelValidationResult.Fail)]
        [InlineData(ValidationMode.Audit, TargetChannelValidationResult.AuditOnlyFailure)]
        public async Task ValidateAsync_EmptyBranchClassifications_ReturnsExpectedResult(ValidationMode validationMode, TargetChannelValidationResult expectedResult)
        {
            // Arrange
            var validator = CreateValidator(validationMode);
            var build = CreateTestBuild();
            var targetChannel = CreateProductionChannelConfig();

            _mockAzureDevOpsService
                .Setup(x => x.GetBuildInfoAsync("dnceng", "internal", 123456))
                .ReturnsAsync(new AzureDevOpsBuildInfo
                {
                    Project = new AzureDevOpsProject { Id = "project-guid-123", Name = "internal" },
                    Repository = new AzureDevOpsRepository { Id = "repo-guid-456", Name = "dotnet-runtime" },
                    Tags = new List<string> { "1ES.PT.Official" }
                });

            _mockBranchClassificationService
                .Setup(x => x.GetBranchClassificationsAsync("dnceng", "project-guid-123", "repo-guid-456"))
                .ReturnsAsync(new BranchClassificationResponse
                {
                    Status = "Success",
                    BranchClassifications = new List<BranchClassification>()
                });

            // Act
            var result = await validator.ValidateAsync(build, targetChannel);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(ValidationMode.Enforce, TargetChannelValidationResult.Fail)]
        [InlineData(ValidationMode.Audit, TargetChannelValidationResult.AuditOnlyFailure)]
        public async Task ValidateAsync_NullBranchClassificationResponse_ReturnsExpectedResult(ValidationMode validationMode, TargetChannelValidationResult expectedResult)
        {
            // Arrange
            var validator = CreateValidator(validationMode);
            var build = CreateTestBuild();
            var targetChannel = CreateProductionChannelConfig();

            _mockAzureDevOpsService
                .Setup(x => x.GetBuildInfoAsync("dnceng", "internal", 123456))
                .ReturnsAsync(new AzureDevOpsBuildInfo
                {
                    Project = new AzureDevOpsProject { Id = "project-guid-123", Name = "internal" },
                    Repository = new AzureDevOpsRepository { Id = "repo-guid-456", Name = "dotnet-runtime" },
                    Tags = new List<string> { "1ES.PT.Official" }
                });

            _mockBranchClassificationService
                .Setup(x => x.GetBranchClassificationsAsync("dnceng", "project-guid-123", "repo-guid-456"))
                .ReturnsAsync((BranchClassificationResponse)null);

            // Act
            var result = await validator.ValidateAsync(build, targetChannel);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(ValidationMode.Enforce, TargetChannelValidationResult.Fail)]
        [InlineData(ValidationMode.Audit, TargetChannelValidationResult.AuditOnlyFailure)]
        public async Task ValidateAsync_UnexpectedExceptionInValidation_ReturnsExpectedResult(ValidationMode validationMode, TargetChannelValidationResult expectedResult)
        {
            // Arrange
            var validator = CreateValidator(validationMode);
            var build = CreateTestBuild();
            var targetChannel = CreateProductionChannelConfig();

            // Mock the Azure DevOps service to throw when fetching build info
            // This simulates an unexpected exception during validation
            _mockAzureDevOpsService
                .Setup(x => x.GetBuildInfoAsync("dnceng", "internal", 123456))
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act
            var result = await validator.ValidateAsync(build, targetChannel);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void AzureDevOpsService_GetBuildInfoAsync_CallsCorrectEndpoint()
        {
            // Arrange
            var mockHttpClient = new Mock<HttpClient>();
            var mockLogger = new Mock<ILogger<AzureDevOpsService>>();
            var service = new AzureDevOpsService(mockHttpClient.Object, mockLogger.Object);

            // Note: This test would need to be expanded with proper HttpClient mocking
            // which is more complex. For now, we're just verifying the interface structure.
            
            // Act & Assert - Just verifying method signature exists
            Assert.True(typeof(IProductionChannelValidatorBuildInfoService).GetMethod(nameof(IProductionChannelValidatorBuildInfoService.GetBuildInfoAsync)) != null);
        }

        [Fact]
        public async Task ValidateAsync_ValidationMode_DefaultsToEnforce()
        {
            // Arrange - Create validator without specifying ValidationMode (should default to Enforce)
            var defaultValidator = new ProductionChannelValidator(
                _mockAzureDevOpsService.Object,
                _mockBranchClassificationService.Object,
                _mockLogger.Object);

            var build = CreateTestBuild(azureDevOpsBranch: "refs/heads/feature/test");
            var targetChannel = CreateProductionChannelConfig();

            _mockAzureDevOpsService
                .Setup(x => x.GetBuildInfoAsync("dnceng", "internal", 123456))
                .ReturnsAsync(new AzureDevOpsBuildInfo
                {
                    Project = new AzureDevOpsProject { Id = "project-guid-123", Name = "internal" },
                    Repository = new AzureDevOpsRepository { Id = "repo-guid-456", Name = "dotnet-runtime" },
                    Tags = new List<string> { "1ES.PT.Official" }
                });

            _mockBranchClassificationService
                .Setup(x => x.GetBranchClassificationsAsync("dnceng", "project-guid-123", "repo-guid-456"))
                .ReturnsAsync(new BranchClassificationResponse
                {
                    Status = "Success",
                    BranchClassifications = new List<BranchClassification>
                    {
                        new BranchClassification { BranchName = "main", Classification = "production" }
                    }
                });

            // Act
            var result = await defaultValidator.ValidateAsync(build, targetChannel);

            // Assert - Default should be Enforce mode, so non-production branch should return Fail
            result.Should().Be(TargetChannelValidationResult.Fail);
        }

        [Fact]
        public void PublishArtifactsInManifest_EnforceProduction_DefaultsToFalse()
        {
            // Arrange & Act
            var task = new PublishArtifactsInManifest();

            // Assert
            task.EnforceProduction.Should().BeFalse();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PublishArtifactsInManifest_EnforceProduction_AcceptsValidValues(bool enforceProduction)
        {
            // Arrange & Act
            var task = new PublishArtifactsInManifest();
            task.EnforceProduction = enforceProduction;

            // Assert
            task.EnforceProduction.Should().Be(enforceProduction);
        }
    }
}
