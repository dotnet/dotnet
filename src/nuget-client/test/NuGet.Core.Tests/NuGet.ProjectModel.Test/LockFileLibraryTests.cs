// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using NuGet.LibraryModel;
using NuGet.Versioning;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class LockFileLibraryTests
    {
        [Fact]
        public void LockFileLibrary_ComparesEqualPaths()
        {
            // Arrange
            var libraryA = new LockFileLibrary
            {
                Name = "SomeLibrary",
                Version = new NuGetVersion("1.0.0"),
                Path = "SomeLibrary/1.0.0"
            };

            // same thing
            var libraryB = new LockFileLibrary
            {
                Name = "SomeLibrary",
                Version = new NuGetVersion("1.0.0"),
                Path = "SomeLibrary/1.0.0"
            };

            // Act & Assert
            Assert.True(libraryA.Equals(libraryB), "The two libraries should be equal.");
        }

        [Fact]
        public void LockFileLibrary_ComparesDifferentCasePaths()
        {
            // Arrange
            var libraryA = new LockFileLibrary
            {
                Name = "SomeLibrary",
                Version = new NuGetVersion("1.0.0"),
                Path = "SomeLibrary/1.0.0"
            };

            // different case
            var libraryB = new LockFileLibrary
            {
                Name = "SomeLibrary",
                Version = new NuGetVersion("1.0.0"),
                Path = "somelibrary/1.0.0"
            };

            // Act & Assert
            Assert.False(libraryA.Equals(libraryB), "The two libraries should not be equal.");
        }

        [Fact]
        public void LockFileLibrary_ComparesDifferentPaths()
        {
            // Arrange
            var libraryA = new LockFileLibrary
            {
                Name = "SomeLibrary",
                Version = new NuGetVersion("1.0.0"),
                Path = "SomeLibrary/1.0.0"
            };

            // different thing
            var libraryB = new LockFileLibrary
            {
                Name = "SomeLibrary",
                Version = new NuGetVersion("1.0.0"),
                Path = null
            };

            // Act & Assert
            Assert.False(libraryA.Equals(libraryB), "The two libraries should not be equal.");
        }

        [Fact]
        public void LockFileLibrary_ComparesDifferentCaseFiles()
        {
            // Arrange
            var libraryA = new LockFileLibrary
            {
                Files = new List<string>
                {
                    "path/a.txt",
                    "path/b.txt"
                }
            };

            // different case
            var libraryB = new LockFileLibrary
            {
                Files = new List<string>
                {
                    "path/a.txt",
                    "PATH/b.txt"
                }
            };

            // Act & Assert
            Assert.False(libraryA.Equals(libraryB), "The two libraries should not be equal.");
        }

        [Fact]
        public void LockFileLibrary_CloneIncludesAllProperties()
        {
            // Arrange
            var original = new LockFileLibrary
            {
                Name = "SomeLibrary",
                Version = new NuGetVersion("1.0.0"),
                Path = "SomeLibrary/1.0.0",
                IsServiceable = true,
                MSBuildProject = "MSBuildProject",
                Sha512 = "FAKE-HASH",
                Type = LibraryType.Package,
                Files = new List<string>
                {
                    "file/a.txt",
                    "file/b.txt"
                }
            };

            // Use Newtonsoft.Json to enumerate all properties.
            var originalSerialized = JsonConvert.SerializeObject(original, Formatting.Indented);

            // Act
            var clone = original.Clone();

            // Assert
            var cloneSerialized = JsonConvert.SerializeObject(original, Formatting.Indented);
            Assert.Equal(originalSerialized, cloneSerialized);
        }

        [Fact]
        public void LockFileLibrary_CloneIsEqual()
        {
            // Arrange
            var original = new LockFileLibrary
            {
                Name = "SomeLibrary",
                Version = new NuGetVersion("1.0.0"),
                Path = "SomeLibrary/1.0.0",
                IsServiceable = true,
                MSBuildProject = "MSBuildProject",
                Sha512 = "FAKE-HASH",
                Type = LibraryType.Package,
                Files = new List<string>
                {
                    "file/a.txt",
                    "file/b.txt"
                }
            };

            // Act
            var clone = original.Clone();

            // Assert
            Assert.Equal(original, clone);
        }

        [Fact]
        public void LockFileLibrary_CloneReturnsDifferentInstance()
        {
            // Arrange
            var original = new LockFileLibrary
            {
                Name = "SomeLibrary",
                Version = new NuGetVersion("1.0.0"),
                Path = "SomeLibrary/1.0.0",
                IsServiceable = true,
                MSBuildProject = "MSBuildProject",
                Sha512 = "FAKE-HASH",
                Type = LibraryType.Package,
                Files = new List<string>
                {
                    "file/a.txt",
                    "file/b.txt"
                }
            };

            // Use Newtonsoft.Json to take a snapshot of all properties.
            var originalSerializedBefore = JsonConvert.SerializeObject(original, Formatting.Indented);

            // Act
            var clone = original.Clone();

            // Assert
            Assert.NotSame(original, clone);

            // Ensure that the clone is deep. Technically the properties that are read-only or value
            // types do not need to be mutated, but this protects against future refactorings.
            clone.Name += "Different";
            clone.Version = new NuGetVersion("2.0.0");
            clone.Path += "Different";
            clone.IsServiceable = !clone.IsServiceable;
            clone.MSBuildProject += "Different";
            clone.Sha512 += "Different";
            clone.Type += "Different";
            clone.Files.Add("Different");

            var originalSerializedAfter = JsonConvert.SerializeObject(original, Formatting.Indented);
            Assert.Equal(originalSerializedBefore, originalSerializedAfter);
        }

        [Fact]
        public void LockFileLibrary_EqualityEmpty()
        {
            // Arrange
            var library1 = new LockFileLibrary();
            var library2 = new LockFileLibrary();

            // Act & Assert
            Assert.True(library1.Equals(library2));
        }

        [Fact]
        public void LockFileLibrary_EqualityDiffersOnMSBuildPath()
        {
            // Arrange
            var library1 = new LockFileLibrary()
            {
                MSBuildProject = "a"
            };

            var library2 = new LockFileLibrary()
            {
                MSBuildProject = "b"
            };

            // Act & Assert
            Assert.False(library1.Equals(library2));
        }

        [Fact]
        public void LockFileLibrary_EqualitySameMSBuildPath()
        {
            // Arrange
            var library1 = new LockFileLibrary()
            {
                MSBuildProject = "b"
            };

            var library2 = new LockFileLibrary()
            {
                MSBuildProject = "b"
            };

            // Act & Assert
            Assert.True(library1.Equals(library2));
        }

        [Theory]
        [InlineData("name", "name", true)]
        [InlineData("NAME", "name", true)]
        [InlineData("name", "name2", false)]
        public void Equals_WithName(string left, string right, bool expected)
        {
            var leftSide = new LockFileLibrary()
            {
                Name = left
            };

            var rightSide = new LockFileLibrary()
            {
                Name = right
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("project", "project", true)]
        [InlineData("PROJECT", "project", true)]
        [InlineData("project", "package", false)]
        public void Equals_WithType(string left, string right, bool expected)
        {
            var leftSide = new LockFileLibrary()
            {
                Type = left
            };

            var rightSide = new LockFileLibrary()
            {
                Type = right
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("1.0.0", "1.0.0", true)]
        [InlineData("1.0.0-preview.1", "1.0.0-preview.1", true)]
        [InlineData("1.0.0", "2.1.0", false)]
        public void Equals_WithVersion(string left, string right, bool expected)
        {
            var leftSide = new LockFileLibrary()
            {
                Version = NuGetVersion.Parse(left)
            };

            var rightSide = new LockFileLibrary()
            {
                Version = NuGetVersion.Parse(right)
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(false, false, true)]
        [InlineData(true, false, false)]
        public void Equals_WithIsServiceable(bool left, bool right, bool expected)
        {
            var leftSide = new LockFileLibrary()
            {
                IsServiceable = left
            };

            var rightSide = new LockFileLibrary()
            {
                IsServiceable = right
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(false, false, true)]
        [InlineData(true, false, false)]
        public void Equals_WithHasTools(bool left, bool right, bool expected)
        {
            var leftSide = new LockFileLibrary()
            {
                HasTools = left
            };

            var rightSide = new LockFileLibrary()
            {
                HasTools = right
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("project", "project", true)]
        [InlineData("project", "PROJECT", false)]
        [InlineData("project", "package", false)]
        public void Equals_WithPath(string left, string right, bool expected)
        {
            var leftSide = new LockFileLibrary()
            {
                Path = left
            };

            var rightSide = new LockFileLibrary()
            {
                Path = right
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("project", "project", true)]
        [InlineData("project", "PROJECT", false)]
        [InlineData("project", "package", false)]
        public void Equals_WithMSBuildProject(string left, string right, bool expected)
        {
            var leftSide = new LockFileLibrary()
            {
                MSBuildProject = left
            };

            var rightSide = new LockFileLibrary()
            {
                MSBuildProject = right
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("project", "project", true)]
        [InlineData("project", "PROJECT", false)]
        [InlineData("project", "package", false)]
        public void Equals_WithSha512(string left, string right, bool expected)
        {
            var leftSide = new LockFileLibrary()
            {
                Sha512 = left
            };

            var rightSide = new LockFileLibrary()
            {
                Sha512 = right
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("project", "project", true)]
        [InlineData("PROJECT", "project", false)]
        [InlineData("project", "package", false)]
        [InlineData("project;project2", "project2;project", true)]
        [InlineData("project;project2", "project;project2;project3", false)]
        public void Equals_WithFiles(string left, string right, bool expected)
        {
            var leftSide = new LockFileLibrary()
            {
                Files = left.Split(';')
            };

            var rightSide = new LockFileLibrary()
            {
                Files = right.Split(';')
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }
    }
}
