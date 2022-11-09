// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using NuGet.LibraryModel;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class IncludeFlagTests
    {
        [Fact]
        public void IncludeFlag_ConvertFromTypeOverride()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""type"": ""build"",
                                    ""suppressParent"": ""none""
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
            Assert.Equal(LibraryIncludeFlags.None, dependency.SuppressParent);
        }

        [Fact]
        public void IncludeFlag_ConvertFromTypeDefault()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0""
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
            Assert.Equal(LibraryIncludeFlagUtils.DefaultSuppressParent, dependency.SuppressParent);
        }


        [Fact]
        public void IncludeFlag_UnknownFlagsParse()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""include"": ""futureFlag"",
                                    ""exclude"": ""futureFlag2"",
                                    ""suppressParent"": ""futureFlag""
                                }
                            },
                            ""frameworks"": {
                                ""net46"": {}
                            }
                        }";


            // Act
            var spec = JsonPackageSpecReader.GetPackageSpec(json, "TestProject", "project.json");
            var dependency = spec.Dependencies.Single();

            var futureFlag = LibraryIncludeFlagUtils.GetFlags(new string[] { "futureFlag" });

            // Assert
            Assert.Equal(LibraryIncludeFlags.None, futureFlag);
        }

        [Fact]
        public void IncludeFlag_SuppressParentFlags()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""suppressParent"": ""build,contentFiles,runtime,native""
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
            var expected = LibraryIncludeFlagUtils.GetFlags(
                new string[]
                    { "build", "runtime", "contentFiles", "native" });

            Assert.Equal(expected, dependency.SuppressParent);
        }

        [Fact]
        public void IncludeFlag_SuppressParentAll()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""suppressParent"": ""all""
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
            Assert.True(dependency.SuppressParent == LibraryIncludeFlags.All);
        }

        [Fact]
        public void IncludeFlag_SuppressParentDefault()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0""
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
            Assert.True(dependency.SuppressParent == LibraryIncludeFlagUtils.DefaultSuppressParent);
        }

        [Fact]
        public void IncludeFlag_EmptyExcludeInclude()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""exclude"": """",
                                    ""include"": """"
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
            Assert.True(dependency.IncludeType == LibraryIncludeFlags.None);
        }

        [Fact]
        public void IncludeFlag_EmptyExclude()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""exclude"": """"
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
            Assert.True(dependency.IncludeType == LibraryIncludeFlags.All);
        }

        [Fact]
        public void IncludeFlag_EmptyInclude()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""include"": """"
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
            Assert.True(dependency.IncludeType == LibraryIncludeFlags.None);
        }

        [Fact]
        public void IncludeFlag_ExcludeOnlyContent()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""exclude"": ""contentFiles""
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
            Assert.Equal(dependency.IncludeType, LibraryIncludeFlags.All & ~LibraryIncludeFlags.ContentFiles);
        }

        [Fact]
        public void IncludeFlag_IncludeOnlyContent()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""include"": ""contentFiles""
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
            Assert.True(dependency.IncludeType
                == LibraryIncludeFlagUtils.GetFlags(new string[] { "contentFiles" }));
        }

        [Fact]
        public void IncludeFlag_IncludeBuildExcludeBuild()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""include"": ""build"", 
                                    ""exclude"": ""build""
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
            Assert.True(dependency.IncludeType == LibraryIncludeFlags.None);
        }

        [Fact]
        public void IncludeFlag_IncludeBuildExcludeAll()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""include"": ""build,compile"", 
                                    ""exclude"": ""all""
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
            Assert.True(dependency.IncludeType.Equals(LibraryIncludeFlags.None));
        }

        [Fact]
        public void IncludeFlag_IncludeNoneExcludeAll()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""include"": ""none"", 
                                    ""exclude"": ""all""
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
            Assert.True(dependency.IncludeType.Equals(LibraryIncludeFlags.None));
        }

        [Fact]
        public void IncludeFlag_IncludeAllExcludeAll()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0"",
                                    ""include"": ""all"", 
                                    ""exclude"": ""all""
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
            Assert.True(dependency.IncludeType.Equals(LibraryIncludeFlags.None));
        }

        [Fact]
        public void IncludeFlag_DefaultValues()
        {
            // Arrange
            var json = @"{
                          ""dependencies"": {
                                ""packageA"": {
                                    ""version"": ""1.0.0""
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
            Assert.True(dependency.IncludeType.Equals(LibraryIncludeFlags.All));
        }

        [Theory]
        [InlineData("all", "all")]
        [InlineData("none", "none")]
        [InlineData("none", "unknown")]
        [InlineData("runtime", "runtime")]
        [InlineData("runtime, build", "build|runtime")]
        public void IncludeFlag_RoundTrip(string expected, string flags)
        {
            // Arrange & Act
            var parsed = LibraryIncludeFlagUtils.GetFlags(flags.Split('|'));
            var actual = LibraryIncludeFlagUtils.GetFlagString(parsed);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
