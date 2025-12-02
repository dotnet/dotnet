// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System.IO;
using System.Linq;
using System.Text;
using NuGet.Common;
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
                            ""frameworks"": {
                                ""net46"": {
                                    ""dependencies"": {
                                        ""packageA"": {
                                            ""version"": ""1.0.0"",
                                            ""target"": ""externalProject""
                                        }
                                    }
                                }
                            }
                        }";

            // Act
            var spec = GetPackageSpec(json, "TestProject", "project.json", EnvironmentVariableWrapper.Instance);
            var dependency = spec.TargetFrameworks[0].Dependencies.Single();

            // Assert
            Assert.Equal(LibraryDependencyTarget.ExternalProject, dependency.LibraryRange.TypeConstraint);
        }

        [Fact]
        public void DependencyTarget_ProjectValue()
        {
            // Arrange
            var json = @"{
                            ""frameworks"": {
                                ""net46"": {
                                    ""dependencies"": {
                                        ""packageA"": {
                                            ""version"": ""1.0.0"",
                                            ""target"": ""project""
                                        }
                                    }
                                }
                            }
                        }";

            // Act
            var spec = GetPackageSpec(json, "TestProject", "project.json", EnvironmentVariableWrapper.Instance);
            var dependency = spec.TargetFrameworks[0].Dependencies.Single();

            // Assert
            Assert.Equal(LibraryDependencyTarget.Project, dependency.LibraryRange.TypeConstraint);
        }

        [Fact]
        public void DependencyTarget_PackageValue()
        {
            // Arrange
            var json = @"{
                            ""frameworks"": {
                                ""net46"": {
                                    ""dependencies"": {
                                        ""packageA"": {
                                            ""version"": ""1.0.0"",
                                            ""target"": ""package""
                                        }
                                    }
                                }
                            }
                        }";

            // Act
            var spec = GetPackageSpec(json, "TestProject", "project.json", EnvironmentVariableWrapper.Instance);
            var dependency = spec.TargetFrameworks[0].Dependencies.Single();

            // Assert
            Assert.Equal(LibraryDependencyTarget.Package, dependency.LibraryRange.TypeConstraint);
        }

        [Fact]
        public void DependencyTarget_CaseInsensitive()
        {
            // Arrange
            var json = @"{
                            ""frameworks"": {
                                ""net46"": {
                                    ""dependencies"": {
                                        ""packageA"": {
                                            ""version"": ""1.0.0"",
                                            ""target"": ""PACKage""
                                        }
                                    }
                                }
                            }
                        }";

            // Act
            var spec = GetPackageSpec(json, "TestProject", "project.json", EnvironmentVariableWrapper.Instance);
            var dependency = spec.TargetFrameworks[0].Dependencies.Single();

            // Assert
            Assert.Equal(LibraryDependencyTarget.Package, dependency.LibraryRange.TypeConstraint);
        }

        [Fact]
        public void DependencyTarget_DefaultValueDefault()
        {
            // Arrange
            var json = @"{
                            ""frameworks"": {
                                ""net46"": {
                                    ""dependencies"": {
                                        ""packageA"": ""1.0.0""
                                    }
                                }
                            }
                        }";

            // Act
            var spec = GetPackageSpec(json, "TestProject", "project.json", EnvironmentVariableWrapper.Instance);
            var dependency = spec.TargetFrameworks[0].Dependencies.Single();

            // Assert
            var expected = LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference;
            Assert.Equal(expected, dependency.LibraryRange.TypeConstraint);
        }

        [Fact]
        public void DependencyTarget_UnknownValueFails()
        {
            // Arrange
            var json = @"{
                            ""frameworks"": {
                                ""net46"": {
                                    ""dependencies"": {
                                        ""packageA"": {
                                            ""version"": ""1.0.0"",
                                            ""target"": ""blah""
                                        }
                                    }
                                }
                            }
                        }";


            // Act
            FileFormatException exception = null;

            try
            {
                var spec = GetPackageSpec(json, "TestProject", "project.json", EnvironmentVariableWrapper.Instance);
                var dependency = spec.TargetFrameworks[0].Dependencies.Single();
            }
            catch (FileFormatException ex)
            {
                exception = ex;
            }

            // Assert
            Assert.NotNull(exception);
            Assert.Equal("Error reading '' : Invalid dependency target value 'blah'.", exception.Message);
        }

        [Fact]
        public void DependencyTarget_NonWhiteListValueFails()
        {
            // Arrange
            var json = @"{
                            ""frameworks"": {
                                ""net46"": {
                                    ""dependencies"": {
                                        ""packageA"": {
                                            ""version"": ""1.0.0"",
                                            ""target"": ""winmd""
                                        }
                                    }
                                }
                            }
                        }";


            // Act
            FileFormatException exception = null;

            try
            {
                var spec = GetPackageSpec(json, "TestProject", "project.json", EnvironmentVariableWrapper.Instance);
                var dependency = spec.TargetFrameworks[0].Dependencies.Single();
            }
            catch (FileFormatException ex)
            {
                exception = ex;
            }

            // Assert
            Assert.NotNull(exception);
            Assert.Equal("Error reading '' : Invalid dependency target value 'winmd'.", exception.Message);
        }

        [Fact]
        public void DependencyTarget_MultipleValuesFail()
        {
            // Arrange
            var json = @"{
                            ""frameworks"": {
                                ""net46"": {
                                    ""dependencies"": {
                                        ""packageA"": {
                                            ""version"": ""1.0.0"",
                                            ""target"": ""package,project""
                                        }
                                    }
                                }
                            }
                        }";


            // Act
            FileFormatException exception = null;

            try
            {
                var spec = GetPackageSpec(json, "TestProject", "project.json", EnvironmentVariableWrapper.Instance);
                var dependency = spec.TargetFrameworks[0].Dependencies.Single();
            }
            catch (FileFormatException ex)
            {
                exception = ex;
            }

            // Assert
            Assert.NotNull(exception);
            Assert.Equal("Error reading '' : Invalid dependency target value 'package,project'.", exception.Message);
        }

        [Fact]
        public void DependencyTarget_AcceptsWhitespace()
        {
            // Arrange
            var json = @"{
                            ""frameworks"": {
                                ""net46"": {
                                    ""dependencies"": {
                                        ""packageA"": {
                                            ""version"": ""1.0.0"",
                                            ""target"": "" package ""
                                        }
                                    }
                                }
                            }
                        }";


            // Act
            var spec = GetPackageSpec(json, "TestProject", "project.json", EnvironmentVariableWrapper.Instance);

            // Assert
            var dependency = spec.TargetFrameworks[0].Dependencies.Single();
            Assert.Equal(LibraryDependencyTarget.Package, dependency.LibraryRange.TypeConstraint);
        }

        private static PackageSpec GetPackageSpec(string json, string name, string packageSpecPath, IEnvironmentVariableReader environmentVariableReader)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return JsonPackageSpecReader.GetPackageSpec(stream, name, packageSpecPath, null, environmentVariableReader);
        }
    }
}
