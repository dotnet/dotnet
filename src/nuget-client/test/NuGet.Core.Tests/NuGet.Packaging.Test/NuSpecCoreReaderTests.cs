// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace NuGet.Packaging.Core.Test
{
    public class NuspecCoreReaderTests
    {
        [Fact]
        public void Id_ReturnsNullWithWrongCase()
        {
            // Arrange
            var contents =
@"<?xml version=""1.0""?>
<package>
  <metadata>
    <ID>foo</ID>
  </metadata>
</package>";

            var reader = new TestNuspecCoreReader(contents);

            // Act
            var id = reader.GetId();

            // Assert
            Assert.Null(id);
        }

        [Fact]
        public void GetPackageType_ReturnsEmptyPackageTypeListIfNotSpecifiedInManfiest()
        {
            // Arrange
            var contents =
@"<?xml version=""1.0""?>
<package>
<metadata/>
</package>";

            var reader = new TestNuspecCoreReader(contents);

            // Act
            var packageTypes = reader.GetPackageTypes();

            // Assert
            Assert.Empty(packageTypes);
        }

        [Theory]
        [InlineData(@"<?xml version=""1.0""?>
<package xmlns=""a-random-xsd"">
  <metadata>
   <id>Test</id>
   <somestuff>some-value</somestuff>
   <ver>123</ver>
  </metadata>
</package>")] // Elements with no children should be extracted.
        [InlineData(@"<?xml version=""1.0""?>
<package xmlns=""a-random-xsd"">
  <metadata>
   <packageTypes>
     <packageType name=""Managed"" version=""2.0"" />
   </packageTypes>
   <id>Test</id>
   <somestuff>some-value</somestuff>
   <ver>123</ver>
  </metadata>
</package>")] // Elements with children should be skipped.
        public void GetMetadata(string contents)
        {
            // Arrange
            var reader = new TestNuspecCoreReader(contents);

            // Act
            var metadata = reader.GetMetadata();

            // Assert
            Assert.Collection(metadata,
                item =>
                {
                    Assert.Equal("id", item.Key);
                    Assert.Equal("Test", item.Value);
                },
                item =>
                {
                    Assert.Equal("somestuff", item.Key);
                    Assert.Equal("some-value", item.Value);
                },
                item =>
                {
                    Assert.Equal("ver", item.Key);
                    Assert.Equal("123", item.Value);
                });
        }

        [Theory]
        [InlineData(
@"<?xml version=""1.0""?>
<package>
  <metadata>
    <packageTypes>
      <packageType name=""Managed"" version=""2.0"" />
    </packageTypes>
  </metadata>
</package>", "Managed", "2.0")]
        [InlineData(
@"<?xml version=""1.0""?>
<package>
  <metadata>
    <packageTypes>
      <packageType name=""SomeFormat"" version=""3.5"" />
    </packageTypes>
  </metadata>
</package>", "SomeFormat", "3.5")]
        [InlineData(
@"<?xml version=""1.0""?>
<package>
  <metadata>
    <packageTypes>
      <packageType name=""RandomFormat123"" />
    </packageTypes>
  </metadata>
</package>", "RandomFormat123", "0.0")]
        public void GetPackageType_ReadsPackageTypeFromManifest(string contents, string expectedType, string expectedVersion)
        {
            // Arrange
            var reader = new TestNuspecCoreReader(contents);

            // Act
            var packageTypes = reader.GetPackageTypes();

            // Assert
            Assert.Equal(1, packageTypes.Count());
            var packageType = packageTypes.First();
            Assert.Equal(expectedType, packageType.Name);
            Assert.Equal(expectedVersion, packageType.Version.ToString());
        }

        [Fact]
        public void GetPackageType_ThrowsIfPackageTypeVersionCannotBeRead()
        {
            // Arrange
            var contents =
@"<?xml version=""1.0""?>
<package>
  <metadata>
    <packageTypes>
      <packageType name=""SomeFormat"" version=""3.5-alpha"" />
    </packageTypes>
  </metadata>
</package>";
            var reader = new TestNuspecCoreReader(contents);

            // Act and Assert
            var exception = Assert.Throws<PackagingException>(() => reader.GetPackageTypes());
            Assert.Equal(
                "Nuspec file contains a package type with an invalid package version '3.5-alpha'.",
                exception.Message);
        }

        [Fact]
        public void GetPackageType_ThrowsIfPackageNameIsMissing()
        {
            // Arrange
            var contents =
@"<?xml version=""1.0""?>
<package>
  <metadata>
    <packageTypes>
      <packageType version=""1.0"">SomeFormat</packageType>
    </packageTypes>
  </metadata>
</package>";
            var reader = new TestNuspecCoreReader(contents);

            // Act and Assert
            var exception = Assert.Throws<PackagingException>(() => reader.GetPackageTypes());
            Assert.Equal(
                "Nuspec file contains a package type that is missing the name attribute.",
                exception.Message);
        }

        [Fact]
        public void GetPackageType_GetsMultiplePackageTypes()
        {
            // Arrange
            var contents =
@"<?xml version=""1.0""?>
<package>
  <metadata>
    <packageTypes>
      <packageType name=""Foo"" version=""1.0"" />
      <packageType name=""Bar"" version=""2.0"" />
    </packageTypes>
  </metadata>
</package>";
            var reader = new TestNuspecCoreReader(contents);

            // Act
            var packageTypes = reader.GetPackageTypes();

            // Assert
            Assert.Equal(2, packageTypes.Count());

            var first = packageTypes.ElementAt(0);
            Assert.Equal("Foo", first.Name);
            Assert.Equal(new Version(1, 0), first.Version);

            var second = packageTypes.ElementAt(1);
            Assert.Equal("Bar", second.Name);
            Assert.Equal(new Version(2, 0), second.Version);
        }

        public class TestNuspecCoreReader : NuspecCoreReaderBase
        {
            public TestNuspecCoreReader(string content)
                : base(new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
            }
        }
    }
}
