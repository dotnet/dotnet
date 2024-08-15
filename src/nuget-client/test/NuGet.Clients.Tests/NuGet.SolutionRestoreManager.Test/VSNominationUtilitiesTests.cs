// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.ProjectManagement;
using System.Collections.Generic;
using System;
using Xunit;
using FluentAssertions;
using System.Collections.Immutable;
using NuGet.Versioning;

namespace NuGet.SolutionRestoreManager.Test
{
    public class VSNominationUtilitiesTests
    {
        private static readonly IReadOnlyDictionary<string, string> EmptyMetadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        [Fact]
        public void GetRestoreAuditProperties_WithoutSuppressions_ReturnsNull()
        {
            // Arrange
            var targetFrameworks = new VsTargetFrameworkInfo4[]
            {
                new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase),
                    properties: new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase)
                    {
                        [ProjectBuildProperties.NuGetAudit] = "true"
                    }),
            };

            // Act
            var actual = VSNominationUtilities.GetRestoreAuditProperties(targetFrameworks);

            // Assert
            actual.SuppressedAdvisories.Should().BeNull();
        }

        [Fact]
        public void GetRestoreAuditProperties_WithEmptySuppressionsList_ReturnsNull()
        {
            // Arrange
            var targetFrameworks = new VsTargetFrameworkInfo4[]
            {
                new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase)
                    {
                        [ProjectItems.NuGetAuditSuppress] = ImmutableArray<IVsReferenceItem2>.Empty,
                    },
                    properties: new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase)
                    {
                        [ProjectBuildProperties.NuGetAudit] = "true"
                    }),
            };

            // Act
            var actual = VSNominationUtilities.GetRestoreAuditProperties(targetFrameworks);

            // Assert
            actual.SuppressedAdvisories.Should().BeNull();
        }

        [Fact]
        public void GetRestoreAuditProperties_NullAndEmptySuppressions_ReturnsNull()
        {
            // Arrange
            var targetFrameworks = new VsTargetFrameworkInfo4[]
            {
                new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase),
                    properties: new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase)
                    {
                        [ProjectBuildProperties.NuGetAudit] = "true"
                    }),
                new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase)
                    {
                        [ProjectItems.NuGetAuditSuppress] = ImmutableArray<IVsReferenceItem2>.Empty,
                    },
                    properties: new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase)
                    {
                        [ProjectBuildProperties.NuGetAudit] = "true"
                    }),
            };

            // Act
            var actual = VSNominationUtilities.GetRestoreAuditProperties(targetFrameworks);

            // Assert
            actual.SuppressedAdvisories.Should().BeNull();
        }

        [Fact]
        public void GetRestoreAuditProperties_OneTfmWithSuppressions_ReturnsSuppressions()
        {
            // Arrange
            var cve1Url = "https://cve.test/1";
            var cve2Url = "https://cve.test/2";
            var targetFrameworks = new VsTargetFrameworkInfo4[]
            {
                new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase)
                    {
                        [ProjectItems.NuGetAuditSuppress] =
                            [
                                new VsReferenceItem2(cve1Url, metadata: EmptyMetadata),
                                new VsReferenceItem2(cve2Url, metadata: EmptyMetadata),
                            ]
                    },
                    properties: new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase)),
            };

            // Act
            var actual = VSNominationUtilities.GetRestoreAuditProperties(targetFrameworks);

            // Assert
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
            var targetFrameworks = new VsTargetFrameworkInfo4[]
            {
                new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase)
                    {
                        [ProjectItems.NuGetAuditSuppress] =
                            [
                                new VsReferenceItem2(cve1Url, metadata: EmptyMetadata),
                                new VsReferenceItem2(cve2Url, metadata: EmptyMetadata),
                            ]
                    },
                    properties: new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase)),
                new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase)
                    {
                        [ProjectItems.NuGetAuditSuppress] =
                            [
                                new VsReferenceItem2(cve1Url, metadata: EmptyMetadata),
                                new VsReferenceItem2(cve2Url, metadata: EmptyMetadata),
                            ]
                    },
                    properties: new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase)),
            };

            // Act
            var actual = VSNominationUtilities.GetRestoreAuditProperties(targetFrameworks);

            // Assert
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
            var targetFrameworks = new VsTargetFrameworkInfo4[]
            {
                new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase),
                    properties: new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase)
                    {
                        [ProjectBuildProperties.NuGetAudit] = "true"
                    }),
                new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase)
                    {
                        [ProjectItems.NuGetAuditSuppress] =
                            [
                                new VsReferenceItem2(cve1Url, metadata: EmptyMetadata),
                                new VsReferenceItem2(cve2Url, metadata: EmptyMetadata),
                            ]
                    },
                    properties: new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase)
                    {
                        [ProjectBuildProperties.NuGetAudit] = "true"
                    }),
            };

            // Act & Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => VSNominationUtilities.GetRestoreAuditProperties(targetFrameworks));
            exception.Message.Should().Contain(ProjectItems.NuGetAuditSuppress);
        }

        [Fact]
        public void GetRestoreAuditProperties_SecondTfmHasNoSuppressions_Throws()
        {
            // Arrange
            var cve1Url = "https://cve.test/1";
            var cve2Url = "https://cve.test/2";
            var targetFrameworks = new VsTargetFrameworkInfo4[]
            {
                new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase)
                    {
                        [ProjectItems.NuGetAuditSuppress] =
                            [
                                new VsReferenceItem2(cve1Url, metadata: EmptyMetadata),
                                new VsReferenceItem2(cve2Url, metadata: EmptyMetadata),
                            ]
                    },
                    properties: new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase)),
                new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase),
                    properties: new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase)),
            };

            // Act & Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => VSNominationUtilities.GetRestoreAuditProperties(targetFrameworks));
            exception.Message.Should().Contain(ProjectItems.NuGetAuditSuppress);
        }

        [Fact]
        public void GetRestoreAuditProperties_TwoTfmWithDifferentSuppressions_Throws()
        {
            // Arrange
            var cve1Url = "https://cve.test/1";
            var cve2Url = "https://cve.test/2";
            var targetFrameworks = new VsTargetFrameworkInfo4[]
            {
                new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase)
                    {
                        [ProjectItems.NuGetAuditSuppress] =
                            [
                                new VsReferenceItem2(cve1Url, metadata: EmptyMetadata),
                            ]
                    },
                    properties: new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase)),
                new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase)
                    {
                        [ProjectItems.NuGetAuditSuppress] =
                            [
                                new VsReferenceItem2(cve2Url, metadata: EmptyMetadata),
                            ]
                    },
                    properties: new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase)),
            };

            // Act & Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => VSNominationUtilities.GetRestoreAuditProperties(targetFrameworks));
            exception.Message.Should().Contain(ProjectItems.NuGetAuditSuppress);
        }

        private VsTargetFrameworkInfo4[] TargetFrameworkWithSdkAnalysisLevel(string sdkAnalysisLevel)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs["SdkAnalysisLevel"] = sdkAnalysisLevel;
            var targetFrameworks = new VsTargetFrameworkInfo4[]
            {
                new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase),
                    properties: new Dictionary<string, string>
                    {
                        { ProjectBuildProperties.SdkAnalysisLevel, sdkAnalysisLevel }
                    })
            };

            return targetFrameworks;
        }

        private VsTargetFrameworkInfo4[] TargetFrameworkWithUsingMicrosoftNetSdk(string UsingMicrosoftNetSdk)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs["UsingMicrosoftNETSdk"] = UsingMicrosoftNetSdk;
            var targetFrameworks = new VsTargetFrameworkInfo4[]
            {
                new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase),
                    properties: new Dictionary<string, string>
                    {
                        { ProjectBuildProperties.UsingMicrosoftNETSdk, UsingMicrosoftNetSdk }
                    })
            };

            return targetFrameworks;
        }

        [Theory]
        [InlineData("9.0.100")]
        [InlineData("7.0.100")]
        [InlineData("9.1.100")]
        [InlineData("9.2.101")]
        public void GetSdkAnalysisLevel_WithValidVersions_ReturnsNuGetVersion(string sdkAnalysisLevel)
        {
            // Arrange
            var targetFrameworks = TargetFrameworkWithSdkAnalysisLevel(sdkAnalysisLevel);
            NuGetVersion expected = new NuGetVersion(sdkAnalysisLevel);

            //Act
            NuGetVersion actual = VSNominationUtilities.GetSdkAnalysisLevel(targetFrameworks);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("1.3e")]
        public void GetSdkAnalysisLevel_WithInvalidVersions_ThrowsException(string sdkAnalysisLevel)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => VSNominationUtilities.GetSdkAnalysisLevel(TargetFrameworkWithSdkAnalysisLevel(sdkAnalysisLevel)));
        }

        [Theory]
        [InlineData("true")]
        [InlineData("True")]
        [InlineData("trUe")]
        [InlineData("TrUe")]
        public void GetUsingMicrosoftNETSdk_WithTrueValue_ReturnsTrue(string usingMicrosoftNETSdk)
        {
            // Act
            bool actual = VSNominationUtilities.GetUsingMicrosoftNETSdk(TargetFrameworkWithUsingMicrosoftNetSdk(usingMicrosoftNETSdk));

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
            // Act
            bool actual = VSNominationUtilities.GetUsingMicrosoftNETSdk(TargetFrameworkWithUsingMicrosoftNetSdk(usingMicrosoftNETSdk));

            // Assert
            Assert.False(actual);
        }

        [Theory]
        [InlineData("t")]
        [InlineData("1.3e")]
        [InlineData("1")]
        public void GetUsingMicrosoftNETSdk_WithInvalidValue_ThrowsException(string usingMicrosoftNETSdk)
        {
            Assert.Throws<ArgumentException>(() => VSNominationUtilities.GetUsingMicrosoftNETSdk(TargetFrameworkWithUsingMicrosoftNetSdk(usingMicrosoftNETSdk)));
        }

    }
}
