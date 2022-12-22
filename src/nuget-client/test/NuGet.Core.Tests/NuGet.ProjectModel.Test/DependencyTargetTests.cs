// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using NuGet.LibraryModel;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class DependencyTargetTests
    {
        [Fact]
        public void DependencyTarget_ExternalProjectValue()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""target"": ""externalProject""
                                }
                            },
                            ""frameworks"": {
                                ""net46"": {}
                            }
                        }";

            // Act
            var spec = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");
            var dependency = spec.Dependencies.Single();

            // Assert
            Assert.Equal(LibraryDependencyTarget.ExternalProject, dependency.LibraryRange.TypeConstraint);
        }

        [Fact]
        public void DependencyTarget_ProjectValue()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""target"": ""project""
                                }
                            },
                            ""frameworks"": {
                                ""net46"": {}
                            }
                        }";

            // Act
            var spec = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");
            var dependency = spec.Dependencies.Single();

            // Assert
            Assert.Equal(LibraryDependencyTarget.Project, dependency.LibraryRange.TypeConstraint);
        }

        [Fact]
        public void DependencyTarget_PackageValue()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""target"": ""package""
                                }
                            },
                            ""frameworks"": {
                                ""net46"": {}
                            }
                        }";

            // Act
            var spec = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");
            var dependency = spec.Dependencies.Single();

            // Assert
            Assert.Equal(LibraryDependencyTarget.Package, dependency.LibraryRange.TypeConstraint);
        }

        [Fact]
        public void DependencyTarget_CaseInsensitive()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""target"": ""PACKage""
                                }
                            },
                            ""frameworks"": {
                                ""net46"": {}
                            }
                        }";

            // Act
            var spec = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");
            var dependency = spec.Dependencies.Single();

            // Assert
            Assert.Equal(LibraryDependencyTarget.Package, dependency.LibraryRange.TypeConstraint);
        }

        [Fact]
        public void DependencyTarget_DefaultValueDefault()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": ""1.0.0""
                            },
                            ""frameworks"": {
                                ""net46"": {}
                            }
                        }";

            // Act
            var spec = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");
            var dependency = spec.Dependencies.Single();

            // Assert
            var expected = LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference;
            Assert.Equal(expected, dependency.LibraryRange.TypeConstraint);
        }

        [Fact]
        public void DependencyTarget_UnknownValueFails()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""target"": ""blah""
                                }
                            },
                            ""frameworks"": {
                                ""net46"": {}
                            }
                        }";


            // Act
            FileFormatException exception = null;

            try
            {
                var spec = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");
                var dependency = spec.Dependencies.Single();
            }
            catch (FileFormatException ex)
            {
                exception = ex;
            }

            // Assert
            Assert.NotNull(exception);
            Assert.Equal("Invalid dependency target value 'blah'.", exception.Message);
            Assert.EndsWith("project.json", exception.Path);
            Assert.Equal(5, exception.Line);
        }

        [Fact]
        public void DependencyTarget_NonWhiteListValueFails()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""target"": ""winmd""
                                }
                            },
                            ""frameworks"": {
                                ""net46"": {}
                            }
                        }";


            // Act
            FileFormatException exception = null;

            try
            {
                var spec = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");
                var dependency = spec.Dependencies.Single();
            }
            catch (FileFormatException ex)
            {
                exception = ex;
            }

            // Assert
            Assert.NotNull(exception);
            Assert.Equal("Invalid dependency target value 'winmd'.", exception.Message);
            Assert.EndsWith("project.json", exception.Path);
            Assert.Equal(5, exception.Line);
        }

        [Fact]
        public void DependencyTarget_MultipleValuesFail()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""target"": ""package,project""
                                }
                            },
                            ""frameworks"": {
                                ""net46"": {}
                            }
                        }";


            // Act
            FileFormatException exception = null;

            try
            {
                var spec = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");
                var dependency = spec.Dependencies.Single();
            }
            catch (FileFormatException ex)
            {
                exception = ex;
            }

            // Assert
            Assert.NotNull(exception);
            Assert.Equal("Invalid dependency target value 'package,project'.", exception.Message);
            Assert.EndsWith("project.json", exception.Path);
            Assert.Equal(5, exception.Line);
        }

        [Fact]
        public void DependencyTarget_AcceptsWhitespace()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""target"": "" package ""
                                }
                            },
                            ""frameworks"": {
                                ""net46"": {}
                            }
                        }";


            // Act
            var spec = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");

            // Assert
            var dependency = spec.Dependencies.Single();
            Assert.Equal(LibraryDependencyTarget.Package, dependency.LibraryRange.TypeConstraint);
        }
    }
}
