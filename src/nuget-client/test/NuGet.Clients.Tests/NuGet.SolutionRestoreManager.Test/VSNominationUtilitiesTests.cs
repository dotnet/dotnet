// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using NuGet.Common;
using NuGet.ProjectManagement;
using NuGet.Versioning;
using Xunit;

namespace NuGet.SolutionRestoreManager.Test
{
    public class VSNominationUtilitiesTests
    {
        [Fact]
        public void GetRestoreAuditProperties_WithoutSuppressions_ReturnsNull()
        {
            // Arrange
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net8.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.NuGetAudit, "true");
                })
                .Build();

            // Act
            var actual = VSNominationUtilities.GetRestoreAuditProperties(projectRestoreInfo.TargetFrameworks);

            // Assert
            actual.Should().NotBeNull();
            actual.SuppressedAdvisories.Should().BeNull();
        }

        [Fact]
        public void GetRestoreAuditProperties_MultiTargetingHasDifferentModeValues_ReturnsAuditModeAllWhenDefined()
        {
            // Arrange
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net8.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.NuGetAuditMode, "direct");
                })
                .WithTargetFrameworkInfo("net10.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.NuGetAuditMode, "all");
                })
                .Build();

            // Act
            var actual = VSNominationUtilities.GetRestoreAuditProperties(projectRestoreInfo.TargetFrameworks);

            // Assert
            actual.Should().NotBeNull();
            actual.AuditMode.Should().Be("all");
        }

        [Fact]
        public void GetRestoreAuditProperties_MultiTargetingHasDifferentModeValues_ThrowsWhenValuesAreDifferentAndNoneAreAll()
        {
            // Arrange
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net8.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.NuGetAuditMode, "one");
                })
                .WithTargetFrameworkInfo("net10.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.NuGetAuditMode, "two");
                })
                .Build();

            // Act && Assert
            Assert.Throws<InvalidOperationException>(() => VSNominationUtilities.GetRestoreAuditProperties(projectRestoreInfo.TargetFrameworks));
        }

        [Fact]
        public void GetRestoreAuditProperties_WithEmptySuppressionsList_ReturnsNull()
        {
            // Arrange
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net8.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.NuGetAudit, "true");
                })
                .Build();

            // Act
            var actual = VSNominationUtilities.GetRestoreAuditProperties(projectRestoreInfo.TargetFrameworks);

            // Assert
            actual.Should().NotBeNull();
            actual.SuppressedAdvisories.Should().BeNull();
        }

        [Fact]
        public void GetRestoreAuditProperties_OneTfmWithSuppressions_ReturnsSuppressions()
        {
            // Arrange
            var cve1Url = "https://cve.test/1";
            var cve2Url = "https://cve.test/2";

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net8.0", builder =>
                {
                    builder
                    .WithItem(ProjectItems.NuGetAuditSuppress, cve1Url, [])
                    .WithItem(ProjectItems.NuGetAuditSuppress, cve2Url, []);
                })
                .Build();

            // Act
            var actual = VSNominationUtilities.GetRestoreAuditProperties(projectRestoreInfo.TargetFrameworks);

            // Assert
            actual.Should().NotBeNull();
            actual.SuppressedAdvisories.Should().HaveCount(2);
            actual.SuppressedAdvisories.Should().Contain(cve1Url);
            actual.SuppressedAdvisories.Should().Contain(cve2Url);
        }

        [Fact]
        public void GetRestoreAuditProperties_TwoTfmWithSuppressions_ReturnsSuppressions()
        {
            // Arrange
            var cve1Url = "https://cve.test/1";
            var cve2Url = "https://cve.test/2";

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net8.0", builder =>
                {
                    builder
                    .WithItem(ProjectItems.NuGetAuditSuppress, cve1Url, [])
                    .WithItem(ProjectItems.NuGetAuditSuppress, cve2Url, []);
                })
                .WithTargetFrameworkInfo("net10.0", builder =>
                {
                    builder
                    .WithItem(ProjectItems.NuGetAuditSuppress, cve1Url, [])
                    .WithItem(ProjectItems.NuGetAuditSuppress, cve2Url, []);
                })
                .Build();

            // Act
            var actual = VSNominationUtilities.GetRestoreAuditProperties(projectRestoreInfo.TargetFrameworks);

            // Assert
            actual.Should().NotBeNull();
            actual.SuppressedAdvisories.Should().HaveCount(2);
            actual.SuppressedAdvisories.Should().Contain(cve1Url);
            actual.SuppressedAdvisories.Should().Contain(cve2Url);
        }

        [Fact]
        public void GetRestoreAuditProperties_FirstTfmHasNoSuppressions_Throws()
        {
            // Arrange
            var cve1Url = "https://cve.test/1";
            var cve2Url = "https://cve.test/2";

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net8.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.NuGetAudit, "true");
                })
                .WithTargetFrameworkInfo("net10.0", builder =>
                {
                    builder
                    .WithProperty(ProjectBuildProperties.NuGetAudit, "true")
                    .WithItem(ProjectItems.NuGetAuditSuppress, cve1Url, [])
                    .WithItem(ProjectItems.NuGetAuditSuppress, cve2Url, []);
                })
                .Build();

            // Act & Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => VSNominationUtilities.GetRestoreAuditProperties(projectRestoreInfo.TargetFrameworks));
            exception.Message.Should().Contain(ProjectItems.NuGetAuditSuppress);
        }

        [Fact]
        public void GetRestoreAuditProperties_SecondTfmHasNoSuppressions_Throws()
        {
            // Arrange
            var cve1Url = "https://cve.test/1";
            var cve2Url = "https://cve.test/2";

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net8.0", builder =>
                {
                    builder
                    .WithProperty(ProjectBuildProperties.NuGetAudit, "true")
                    .WithItem(ProjectItems.NuGetAuditSuppress, cve1Url, [])
                    .WithItem(ProjectItems.NuGetAuditSuppress, cve2Url, []);
                })
                .WithTargetFrameworkInfo("net10.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.NuGetAudit, "true");
                })
                .Build();

            // Act & Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => VSNominationUtilities.GetRestoreAuditProperties(projectRestoreInfo.TargetFrameworks));
            exception.Message.Should().Contain(ProjectItems.NuGetAuditSuppress);
        }

        [Fact]
        public void GetRestoreAuditProperties_TwoTfmWithDifferentSuppressions_Throws()
        {
            // Arrange
            var cve1Url = "https://cve.test/1";
            var cve2Url = "https://cve.test/2";

            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net8.0", builder =>
                {
                    builder
                    .WithItem(ProjectItems.NuGetAuditSuppress, cve1Url, []);
                })
                .WithTargetFrameworkInfo("net10.0", builder =>
                {
                    builder
                    .WithItem(ProjectItems.NuGetAuditSuppress, cve2Url, []);
                })
                .Build();

            // Act & Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => VSNominationUtilities.GetRestoreAuditProperties(projectRestoreInfo.TargetFrameworks));
            exception.Message.Should().Contain(ProjectItems.NuGetAuditSuppress);
        }

        [Theory]
        [InlineData("9.0.100")]
        [InlineData("7.0.100")]
        [InlineData("9.1.100")]
        [InlineData("9.2.101")]
        public void GetSdkAnalysisLevel_WithValidVersions_ReturnsNuGetVersion(string sdkAnalysisLevel)
        {
            // Arrange
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net9.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.SdkAnalysisLevel, sdkAnalysisLevel);
                })
                .Build();
            NuGetVersion expected = new NuGetVersion(sdkAnalysisLevel);

            //Act
            NuGetVersion? actual = VSNominationUtilities.GetSdkAnalysisLevel(projectRestoreInfo.TargetFrameworks);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("1.3e")]
        public void GetSdkAnalysisLevel_WithInvalidVersions_ThrowsException(string sdkAnalysisLevel)
        {
            // Arrange
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net9.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.SdkAnalysisLevel, sdkAnalysisLevel);
                })
                .Build();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => VSNominationUtilities.GetSdkAnalysisLevel(projectRestoreInfo.TargetFrameworks));
        }

        [Theory]
        [InlineData("true")]
        [InlineData("True")]
        [InlineData("trUe")]
        [InlineData("TrUe")]
        public void GetUsingMicrosoftNETSdk_WithTrueValue_ReturnsTrue(string usingMicrosoftNETSdk)
        {
            // Arrange
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net8.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.UsingMicrosoftNETSdk, usingMicrosoftNETSdk);
                })
                .Build();

            // Act
            bool actual = VSNominationUtilities.GetUsingMicrosoftNETSdk(projectRestoreInfo.TargetFrameworks);

            // Assert
            Assert.True(actual);
        }

        [Theory]
        [InlineData("false")]
        [InlineData("False")]
        [InlineData("falSe")]
        [InlineData("FalsE")]
        public void GetUsingMicrosoftNETSdk_WithFalseValue_ReturnsFalse(string usingMicrosoftNETSdk)
        {
            // Arrange
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net8.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.UsingMicrosoftNETSdk, usingMicrosoftNETSdk);
                })
                .Build();

            // Act
            bool actual = VSNominationUtilities.GetUsingMicrosoftNETSdk(projectRestoreInfo.TargetFrameworks);

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineData("t")]
        [InlineData("1.3e")]
        [InlineData("1")]
        public void GetUsingMicrosoftNETSdk_WithInvalidValue_ThrowsException(string usingMicrosoftNETSdk)
        {
            // Arrange
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net8.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.UsingMicrosoftNETSdk, usingMicrosoftNETSdk);
                })
                .Build();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => VSNominationUtilities.GetUsingMicrosoftNETSdk(projectRestoreInfo.TargetFrameworks));
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("falSe", false)]
        [InlineData(null, false)]
        public void GetPackageSpec_WithUseLegacyDependencyResolver(string? useLegacyDependencyResolver, bool expected)
        {
            // Arrange
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net8.0", builder =>
                {
                    if (useLegacyDependencyResolver != null)
                    {
                        builder.WithProperty(ProjectBuildProperties.RestoreUseLegacyDependencyResolver, useLegacyDependencyResolver);
                    }
                })
                .Build();

            // Act & Assert
            VSNominationUtilities.GetUseLegacyDependencyResolver(projectRestoreInfo.TargetFrameworks).Should().Be(expected);
        }

        [Fact]
        public void GetPackageSpec_WithUseLegacyDependencyResolver_DoesNotSupportPerFrameworkConfiguration()
        {
            // Arrange
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net8.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.RestoreUseLegacyDependencyResolver, "true");
                })
                .WithTargetFrameworkInfo("net10.0", builder =>
                {
                    builder.WithProperty(ProjectBuildProperties.RestoreUseLegacyDependencyResolver, "false");
                })
                .Build();

            // Act & Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => VSNominationUtilities.GetUseLegacyDependencyResolver(projectRestoreInfo.TargetFrameworks));
            exception.Message.Should().Contain(ProjectBuildProperties.RestoreUseLegacyDependencyResolver);
        }

        [Fact]
        public void GetDistinctNuGetLogCodesOrDefault_SameLogCodes()
        {
            // Arrange
            ImmutableArray<NuGetLogCode> logCodes1 = [NuGetLogCode.NU1000, NuGetLogCode.NU1001];
            ImmutableArray<NuGetLogCode> logCodes2 = [NuGetLogCode.NU1001, NuGetLogCode.NU1000];

            ImmutableArray<ImmutableArray<NuGetLogCode>> logCodesList = [logCodes1, logCodes2];

            // Act
            var result = VSNominationUtilities.GetDistinctNuGetLogCodesOrDefault(logCodesList);

            // Assert
            Assert.Equal(2, result.Length);
            Assert.True(result.All(logCodes2.Contains));
        }

        [Fact]
        public void GetDistinctNuGetLogCodesOrDefault_EmptyLogCodes()
        {
            // Arrange
            ImmutableArray<ImmutableArray<NuGetLogCode>> logCodesList = [];

            // Act
            var result = VSNominationUtilities.GetDistinctNuGetLogCodesOrDefault(logCodesList);

            // Assert
            Assert.Equal(0, result.Length);
        }

        [Fact]
        public void GetDistinctNuGetLogCodesOrDefault_DiffLogCodes()
        {
            // Arrange
            ImmutableArray<NuGetLogCode> logCodes1 = [NuGetLogCode.NU1000];
            ImmutableArray<NuGetLogCode> logCodes2 = [NuGetLogCode.NU1001, NuGetLogCode.NU1000];

            ImmutableArray<ImmutableArray<NuGetLogCode>> logCodesList = [logCodes1, logCodes2];

            // Act
            var result = VSNominationUtilities.GetDistinctNuGetLogCodesOrDefault(logCodesList);

            // Assert
            Assert.Equal(0, result.Length);
        }

        [Fact]
        public void GetDistinctNuGetLogCodesOrDefault_OneDefaultCode()
        {
            // Arrange
            ImmutableArray<NuGetLogCode> logCodes1 = [NuGetLogCode.NU1001, NuGetLogCode.NU1000];

            ImmutableArray<ImmutableArray<NuGetLogCode>> logCodesList = [default, logCodes1];

            // Act
            var result = VSNominationUtilities.GetDistinctNuGetLogCodesOrDefault(logCodesList);

            // Assert
            Assert.Equal(0, result.Length);
        }

        [Fact]
        public void GetDistinctNuGetLogCodesOrDefault_AllDefaultCodes()
        {
            // Arrange
            ImmutableArray<ImmutableArray<NuGetLogCode>> logCodesList = [default, default];

            // Act
            var result = VSNominationUtilities.GetDistinctNuGetLogCodesOrDefault(logCodesList);

            // Assert
            Assert.Equal(0, result.Length);
        }

        [Fact]
        public void GetDistinctNuGetLogCodesOrDefault_DefaultCodesAfterFirst()
        {
            // Arrange
            ImmutableArray<NuGetLogCode> logCodes1 = [NuGetLogCode.NU1001, NuGetLogCode.NU1000];
            ImmutableArray<ImmutableArray<NuGetLogCode>> logCodesList = [logCodes1, default];

            // Act
            var result = VSNominationUtilities.GetDistinctNuGetLogCodesOrDefault(logCodesList);

            // Assert
            Assert.Equal(0, result.Length);
        }

        [Theory]
        [InlineData("9.0.100", "9.0.100")]
        [InlineData("Not a version", null)]
        public void GetSdkVersion_WithVariousInputs(string sdkVersion, string? expectedSdkVersion)
        {
            // Arrange
            var projectRestoreInfo = new TestProjectRestoreInfoBuilder()
                .WithTargetFrameworkInfo("net8.0", builder =>
                {
                    builder.WithProperty("NETCoreSdkVersion", sdkVersion);
                })
                .Build();
            NuGetVersion? expected = expectedSdkVersion != null ? new NuGetVersion(expectedSdkVersion) : null;

            //Act
            NuGetVersion? actual = VSNominationUtilities.GetSdkVersion(projectRestoreInfo.TargetFrameworks);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(IsPruningEnabledGlobally))]
        public void IsPruningEnabledGlobally_WithVariousInputs_ReturnsExpectedResult(string[] members, bool expected)
        {
            // Arrange
            var builder = new TestProjectRestoreInfoBuilder();

            for (int i = 0; i < members.Length; i++)
            {
                builder = builder.WithTargetFrameworkInfo($"net{i + 1}.0", builder =>
                {
                    builder
                    .WithProperty(ProjectBuildProperties.RestorePackagePruningDefault, members[i]);
                });
            }
            var projectRestoreInfo = builder.Build();

            // Act & Assert
            VSNominationUtilities.IsPruningEnabledGlobally(projectRestoreInfo.TargetFrameworks).Should().Be(expected);
        }

        public static readonly List<object[]> IsPruningEnabledGlobally
            = new List<object[]>
            {
                    new object[] { new string[] { "true", "false" }, true },
                    new object[] { new string[] { "true", "" }, true },
                    new object[] { new string[] { "", "", "false" }, false },
            };
    }
}
