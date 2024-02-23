// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void DependencyTarget_ExternalProjectValue(IEnvironmentVariableReader environmentVariableReader)
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
            var spec = GetPackageSpec(json, "TestProject", "project.json", environmentVariableReader);
            var dependency = spec.Dependencies.Single();

            // Assert
            Assert.Equal(LibraryDependencyTarget.ExternalProject, dependency.LibraryRange.TypeConstraint);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void DependencyTarget_ProjectValue(IEnvironmentVariableReader environmentVariableReader)
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
            var spec = GetPackageSpec(json, "TestProject", "project.json", environmentVariableReader);
            var dependency = spec.Dependencies.Single();

            // Assert
            Assert.Equal(LibraryDependencyTarget.Project, dependency.LibraryRange.TypeConstraint);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void DependencyTarget_PackageValue(IEnvironmentVariableReader environmentVariableReader)
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
            var spec = GetPackageSpec(json, "TestProject", "project.json", environmentVariableReader);
            var dependency = spec.Dependencies.Single();

            // Assert
            Assert.Equal(LibraryDependencyTarget.Package, dependency.LibraryRange.TypeConstraint);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void DependencyTarget_CaseInsensitive(IEnvironmentVariableReader environmentVariableReader)
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
            var spec = GetPackageSpec(json, "TestProject", "project.json", environmentVariableReader);
            var dependency = spec.Dependencies.Single();

            // Assert
            Assert.Equal(LibraryDependencyTarget.Package, dependency.LibraryRange.TypeConstraint);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void DependencyTarget_DefaultValueDefault(IEnvironmentVariableReader environmentVariableReader)
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
            var spec = GetPackageSpec(json, "TestProject", "project.json", environmentVariableReader);
            var dependency = spec.Dependencies.Single();

            // Assert
            var expected = LibraryDependencyTarget.All & ~LibraryDependencyTarget.Reference;
            Assert.Equal(expected, dependency.LibraryRange.TypeConstraint);
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void DependencyTarget_UnknownValueFails(IEnvironmentVariableReader environmentVariableReader)
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
                var spec = GetPackageSpec(json, "TestProject", "project.json", environmentVariableReader);
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

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal(5, exception.Line);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void DependencyTarget_NonWhiteListValueFails(IEnvironmentVariableReader environmentVariableReader)
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
                var spec = GetPackageSpec(json, "TestProject", "project.json", environmentVariableReader);
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

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal(5, exception.Line);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void DependencyTarget_MultipleValuesFail(IEnvironmentVariableReader environmentVariableReader)
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
                var spec = GetPackageSpec(json, "TestProject", "project.json", environmentVariableReader);
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

            if (string.Equals(bool.TrueString, environmentVariableReader.GetEnvironmentVariable(JsonUtility.NUGET_EXPERIMENTAL_USE_NJ_FOR_FILE_PARSING)))
            {
                Assert.Equal(5, exception.Line);
            }
        }

        [Theory]
        [MemberData(nameof(LockFileParsingEnvironmentVariable.TestEnvironmentVariableReader), MemberType = typeof(LockFileParsingEnvironmentVariable))]
        public void DependencyTarget_AcceptsWhitespace(IEnvironmentVariableReader environmentVariableReader)
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
            var spec = GetPackageSpec(json, "TestProject", "project.json", environmentVariableReader);

            // Assert
            var dependency = spec.Dependencies.Single();
            Assert.Equal(LibraryDependencyTarget.Package, dependency.LibraryRange.TypeConstraint);
        }

        private static PackageSpec GetPackageSpec(string json, string name, string packageSpecPath, IEnvironmentVariableReader environmentVariableReader)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return JsonPackageSpecReader.GetPackageSpec(stream, name, packageSpecPath, null, environmentVariableReader, true);
        }

    }
}
