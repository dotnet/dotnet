// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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

            var originalPackageSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project", @"C:\", referenceSpec);

            // Act
            var buildIntegrationInstallationContext = NuGetPackageManager.CreateInstallationContextForPackageId(packageIdentityId: "a", originalPackageSpec, originalPackageSpec, unsuccessfulFrameworks: new());

            // Assert
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().HaveCount(2);
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().HaveCount(0);
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net50.GetShortFolderName());
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net472.GetShortFolderName());
            buildIntegrationInstallationContext.AreAllPackagesConditional.Should().BeFalse();
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

            var originalPackageSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project", @"C:\", referenceSpec);

            // Act
            var buildIntegrationInstallationContext = NuGetPackageManager.CreateInstallationContextForPackageId(packageIdentityId: "a", originalPackageSpec, originalPackageSpec, unsuccessfulFrameworks: new());

            // Assert
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().HaveCount(1);
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().HaveCount(1);
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net50.GetShortFolderName());
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net472.GetShortFolderName());
            buildIntegrationInstallationContext.AreAllPackagesConditional.Should().BeFalse();
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

            var originalPackageSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project", @"C:\", referenceSpec);

            // Act
            var buildIntegrationInstallationContext = NuGetPackageManager.CreateInstallationContextForPackageId(packageIdentityId: "a", originalPackageSpec, originalPackageSpec, unsuccessfulFrameworks: new() { FrameworkConstants.CommonFrameworks.Net472.GetShortFolderName() });

            // Assert
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().HaveCount(0);
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().HaveCount(2);
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net50.GetShortFolderName());
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net472.GetShortFolderName());
            buildIntegrationInstallationContext.AreAllPackagesConditional.Should().BeFalse();
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

            var originalPackageSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project", @"C:\", referenceSpec);

            // Act
            var buildIntegrationInstallationContext = NuGetPackageManager.CreateInstallationContextForPackageId(packageIdentityId: "a", originalPackageSpec, originalPackageSpec, unsuccessfulFrameworks: new() { FrameworkConstants.CommonFrameworks.Net50.GetShortFolderName() });

            // Assert
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().HaveCount(1);
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().HaveCount(1);
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net50.GetShortFolderName());
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net472.GetShortFolderName());
            buildIntegrationInstallationContext.AreAllPackagesConditional.Should().BeFalse();
        }

        [Fact]
        public void CreateInstallationContextForPackageId_WithConsolidatingVersions_ReturnsCorrectValue()
        {
            // Arrange
            string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""net472"": {
                            ""dependencies"": {
                                ""a"" : ""2.0.0"",
                                ""b"" : ""1.0.0"" 
                            }
                        },
                        ""net5.0"": {
                            ""dependencies"": {
                                ""a"" : ""1.0.0"",
                            }
                        }
                    }
                }";

            var originalPackageSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project", @"C:\", referenceSpec);

            // Act
            var buildIntegrationInstallationContext = NuGetPackageManager.CreateInstallationContextForPackageId(packageIdentityId: "a", originalPackageSpec, originalPackageSpec, unsuccessfulFrameworks: new());

            // Assert
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().HaveCount(2);
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().HaveCount(0);
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net50.GetShortFolderName());
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net472.GetShortFolderName());
            buildIntegrationInstallationContext.AreAllPackagesConditional.Should().BeTrue();
        }

        [Fact]
        public void CreateInstallationContextForPackageId_WithConsolidatingVersionsWithOriginalPackageSpecForDetectingConditionalPackages_ReturnsCorrectValue()
        {
            // Arrange

            var originalPackageSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project", @"C:\",
                @"
                {
                    ""frameworks"": {
                        ""net472"": {
                            ""dependencies"": {
                                ""a"" : ""2.0.0"",
                                ""b"" : ""1.0.0"" 
                            }
                        },
                        ""net5.0"": {
                            ""dependencies"": {
                                ""a"" : ""1.0.0"",
                            }
                        }
                    }
                }");

            string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""net472"": {
                            ""dependencies"": {
                                ""a"" : ""2.0.0"",
                                ""b"" : ""1.0.0"" 
                            }
                        },
                        ""net5.0"": {
                            ""dependencies"": {
                                ""a"" : ""2.0.0"",
                            }
                        }
                    }
                }";

            var resultingPackageSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project", @"C:\", referenceSpec);

            // Act
            var buildIntegrationInstallationContext = NuGetPackageManager.CreateInstallationContextForPackageId(packageIdentityId: "a", resultingPackageSpec, originalPackageSpec, unsuccessfulFrameworks: new());

            // Assert
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().HaveCount(2);
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().HaveCount(0);
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net50.GetShortFolderName());
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net472.GetShortFolderName());
            buildIntegrationInstallationContext.AreAllPackagesConditional.Should().BeTrue();
        }

        [Fact]
        public void CreateInstallationContextForPackageId_WithDifferencePackageIdCase_ReturnsCorrectValue()
        {
            // Arrange

            var originalPackageSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project", @"C:\",
                @"
                {
                    ""frameworks"": {
                        ""net472"": {
                            ""dependencies"": {
                                ""a"" : ""1.0.0""
                            }
                        }
                    }
                }");

            string referenceSpec = @"
                {
                    ""frameworks"": {
                        ""net472"": {
                            ""dependencies"": {
                                ""a"" : ""2.0.0""
                            }
                        }
                    }
                }";

            var resultingPackageSpec = ProjectTestHelpers.GetPackageSpecWithProjectNameAndSpec("project", @"C:\", referenceSpec);

            // Act
            var buildIntegrationInstallationContext = NuGetPackageManager.CreateInstallationContextForPackageId(packageIdentityId: "A", resultingPackageSpec, originalPackageSpec, unsuccessfulFrameworks: new());

            // Assert
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().HaveCount(1);
            buildIntegrationInstallationContext.UnsuccessfulFrameworks.Should().HaveCount(0);
            buildIntegrationInstallationContext.SuccessfulFrameworks.Should().Contain(FrameworkConstants.CommonFrameworks.Net472.GetShortFolderName());
            buildIntegrationInstallationContext.AreAllPackagesConditional.Should().BeFalse();
        }
    }
}
