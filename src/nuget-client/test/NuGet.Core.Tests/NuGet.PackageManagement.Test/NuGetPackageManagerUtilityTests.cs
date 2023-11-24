// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using FluentAssertions;
using NuGet.Commands.Test;
using NuGet.Frameworks;
using Xunit;

namespace NuGet.PackageManagement.Test
{
    public class NuGetPackageManagerUtilityTests
    {
        [Fact]
        public void CreateInstallationContextForPackageId_WithCompleteOperation_ReturnsCorrectValue()
        {
            // Arrange
            string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""net472"": {
                            ""dependencies"": {
                                ""b"" : ""2.0.0"",
                                ""a"" : ""1.0.0"" 
                            }
                        },
                        ""net5.0"": {
                            ""dependencies"": {
                                ""a"" : ""1.0.0"" 
                            }
                        }
                    }
                }";
            Dictionary<NuGetFramework, string> originalFrameworks = new()
            {
                { FrameworkConstants.CommonFrameworks.Net472, "net472" },
                { FrameworkConstants.CommonFrameworks.Net50, "net50" }
            };

            var originalPackageSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project", @"C:\", referenceSpec);

            // Act
            var buildIntegrationInstallationContext = NuGetPackageManager.CreateInstallationContextForPackageId(packageIdentityId: "a", originalPackageSpec, unsuccessfulFrameworks: new(), originalFrameworks);

            // Assert
            buildIntegrationInstallationContext.OriginalFrameworks.Should().Equal(originalFrameworks);
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().HaveCount(2);
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().HaveCount(0);
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net50);
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net472);
        }

        [Fact]
        public void CreateInstallationContextForPackageId_WithConditionalOperation_ReturnsCorrectValue()
        {
            // Arrange
            string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""net472"": {
                            ""dependencies"": {
                                ""b"" : ""2.0.0"",
                                ""a"" : ""1.0.0"" 
                            }
                        },
                        ""net5.0"": {
                            ""dependencies"": {
                                ""b"" : ""2.0.0"",
                            }
                        }
                    }
                }";
            Dictionary<NuGetFramework, string> originalFrameworks = new()
            {
                { FrameworkConstants.CommonFrameworks.Net472, "net472" },
                { FrameworkConstants.CommonFrameworks.Net50, "net50" }
            };

            var originalPackageSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project", @"C:\", referenceSpec);

            // Act
            var buildIntegrationInstallationContext = NuGetPackageManager.CreateInstallationContextForPackageId(packageIdentityId: "a", originalPackageSpec, unsuccessfulFrameworks: new(), originalFrameworks);

            // Assert
            buildIntegrationInstallationContext.OriginalFrameworks.Should().Equal(originalFrameworks);
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().HaveCount(1);
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().HaveCount(1);
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net50);
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net472);
        }

        [Fact]
        public void CreateInstallationContextForPackageId_WithFailedConditionalOperation_ReturnsCorrectValue()
        {
            // Arrange
            string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""net472"": {
                            ""dependencies"": {
                                ""b"" : ""2.0.0"",
                                ""a"" : ""1.0.0"" 
                            }
                        },
                        ""net5.0"": {
                            ""dependencies"": {
                            }
                        }
                    }
                }";
            Dictionary<NuGetFramework, string> originalFrameworks = new()
            {
                { FrameworkConstants.CommonFrameworks.Net472, "net472" },
                { FrameworkConstants.CommonFrameworks.Net50, "net50" }
            };

            var originalPackageSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project", @"C:\", referenceSpec);

            // Act
            var buildIntegrationInstallationContext = NuGetPackageManager.CreateInstallationContextForPackageId(packageIdentityId: "a", originalPackageSpec, unsuccessfulFrameworks: new() { FrameworkConstants.CommonFrameworks.Net472 }, originalFrameworks);

            // Assert
            buildIntegrationInstallationContext.OriginalFrameworks.Should().Equal(originalFrameworks);
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().HaveCount(0);
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().HaveCount(2);
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net50);
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net472);
        }

        [Fact]
        public void CreateInstallationContextForPackageId_WithFailedCompleteOperation_ReturnsCorrectValue()
        {
            // Arrange
            string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""net472"": {
                            ""dependencies"": {
                                ""b"" : ""2.0.0"",
                                ""a"" : ""1.0.0"" 
                            }
                        },
                        ""net5.0"": {
                            ""dependencies"": {
                                ""a"" : ""1.0.0"" 
                            }
                        }
                    }
                }";
            Dictionary<NuGetFramework, string> originalFrameworks = new()
            {
                { FrameworkConstants.CommonFrameworks.Net472, "net472" },
                { FrameworkConstants.CommonFrameworks.Net50, "net50" }
            };

            var originalPackageSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project", @"C:\", referenceSpec);

            // Act
            var buildIntegrationInstallationContext = NuGetPackageManager.CreateInstallationContextForPackageId(packageIdentityId: "a", originalPackageSpec, unsuccessfulFrameworks: new() { FrameworkConstants.CommonFrameworks.Net50 }, originalFrameworks);

            // Assert
            buildIntegrationInstallationContext.OriginalFrameworks.Should().Equal(originalFrameworks);
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().HaveCount(1);
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().HaveCount(1);
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net50);
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net472);
        }
    }
}
