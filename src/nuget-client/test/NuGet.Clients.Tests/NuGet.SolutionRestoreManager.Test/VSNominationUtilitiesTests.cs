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

        private VsTargetFrameworkInfo4[] TargetFrameworkWithSdkVersion(string sdkAnalysisLevel)
        {
            var targetFrameworks = new VsTargetFrameworkInfo4[]
            {
                new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase),
                    properties: new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "NETCoreSdkVersion", sdkAnalysisLevel }
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

        [Theory]
        [InlineData("true", true)]
        [InlineData("falSe", false)]
        [InlineData(null, false)]
        public void GetPackageSpec_WithUseLegacyDependencyResolver(string useLegacyDependencyResolver, bool expected)
        {
            // Arrange
            var targetFrameworks = new VsTargetFrameworkInfo4[]
            {
                new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase),
                    properties: new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase)
                    {
                        [ProjectBuildProperties.RestoreUseLegacyDependencyResolver] = useLegacyDependencyResolver
                    })
            };

            // Act & Assert
            VSNominationUtilities.GetUseLegacyDependencyResolver(targetFrameworks).Should().Be(expected);
        }

        [Fact]
        public void GetPackageSpec_WithUseLegacyDependencyResolver_DoesNotSupportPerFrameworkConfiguration()
        {
            // Arrange
            var targetFrameworks = new VsTargetFrameworkInfo4[]
            {
                new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase),
                    properties: new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase)
                    {
                        [ProjectBuildProperties.RestoreUseLegacyDependencyResolver] = "true"
                    }),
                 new VsTargetFrameworkInfo4(
                    items: new Dictionary<string, IReadOnlyList<IVsReferenceItem2>>(StringComparer.OrdinalIgnoreCase),
                    properties: new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase)
                    {
                        [ProjectBuildProperties.RestoreUseLegacyDependencyResolver] = "false"
                    })
            };

            // Act & Assert
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => VSNominationUtilities.GetUseLegacyDependencyResolver(targetFrameworks));
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
        public void GetSdkVersion_WithVariousInputs(string sdkVersion, string expectedSdkVersion)
        {
            // Arrange
            var targetFrameworks = TargetFrameworkWithSdkVersion(sdkVersion);
            NuGetVersion expected = expectedSdkVersion != null ? new NuGetVersion(expectedSdkVersion) : null;

            //Act
            NuGetVersion actual = VSNominationUtilities.GetSdkVersion(targetFrameworks);

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
