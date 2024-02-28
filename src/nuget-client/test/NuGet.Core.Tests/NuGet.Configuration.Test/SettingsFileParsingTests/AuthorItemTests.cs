// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class AuthorItemTests
    {
        [Fact]
        public void AuthorItem_WithoutName_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <author>
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true""  />
        </author>
    </SectionName>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));

                ex.Should().NotBeNull();
                ex.Should().BeOfType<NuGetConfigurationException>();
                ex.Message.Should().Be(string.Format("Unable to parse config file because: Missing required attribute 'name' in element 'author'. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void AuthorItem_WithoutCertificates_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <author name=""authorname"" />
    </SectionName>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var ex = Record.Exception(() => new SettingsFile(mockBaseDirectory));

                ex.Should().NotBeNull();
                ex.Should().BeOfType<NuGetConfigurationException>();
                ex.Message.Should().Be(string.Format("Unable to parse config file because: A trusted signer entry must have at least one certificate entry. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void AuthorItem_WithCertificates_ParsedCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <author name=""authorName"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true""  />
        </author>
    </SectionName>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();

                section!.Items.Count.Should().Be(1);
                var item = (AuthorItem)section.Items.First();

                var expectedItem = new AuthorItem("authorName",
                    new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA256, allowUntrustedRoot: true));
                SettingsTestUtils.DeepEquals(item, expectedItem).Should().BeTrue();
            }
        }

        [Fact]
        public void AuthorItem_Update_UpdatesAddsCertificatesCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <author name=""authorName"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
        </author>
    </SectionName>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                settingsFile.TryGetSection("SectionName", out var section).Should().BeTrue();
                section.Should().NotBeNull();

                section!.Items.Count.Should().Be(1);
                var item = (AuthorItem)section.Items.First();

                var updatedItem = (AuthorItem)item.Clone();
                updatedItem.Certificates.Add(new CertificateItem("xyz", Common.HashAlgorithmName.SHA256));

                item.Update(updatedItem);
                SettingsTestUtils.DeepEquals(item, updatedItem).Should().BeTrue();

                settingsFile.SaveToDisk();

                // Assert
                var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <author name=""authorName"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
            <certificate fingerprint=""xyz"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
    </SectionName>
</configuration>";

                Assert.Equal(result.Replace("\r\n", "\n"), File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void AuthorItem_Update_UpdatesRemovesCertificatesCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <author name=""authorName"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
            <certificate fingerprint=""xyz"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
    </SectionName>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                settingsFile.TryGetSection("SectionName", out var section).Should().BeTrue();
                section.Should().NotBeNull();

                section!.Items.Count.Should().Be(1);
                var item = (AuthorItem)section.Items.First();

                var updatedItem = (AuthorItem)item.Clone();
                updatedItem.Certificates.RemoveAt(1);

                item.Update(updatedItem);
                SettingsTestUtils.DeepEquals(item, updatedItem).Should().BeTrue();

                settingsFile.SaveToDisk();

                // Assert
                var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <author name=""authorName"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
        </author>
    </SectionName>
</configuration>";

                Assert.Equal(result.Replace("\r\n", "\n"), File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void AuthorItem_Update_UpdatesRemovingTheLastCertificate_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <author name=""authorName"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
        </author>
    </SectionName>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                settingsFile.TryGetSection("SectionName", out var section).Should().BeTrue();
                section.Should().NotBeNull();

                section!.Items.Count.Should().Be(1);
                var item = (AuthorItem)section.Items.First();

                var updatedItem = (AuthorItem)item.Clone();
                updatedItem.Certificates.Clear();

                // Act and Assert
                var ex = Record.Exception(() => item.Update(updatedItem));

                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
                ex.Message.Should().Be("A trusted signer entry must have at least one certificate entry.");
            }
        }

        [Fact]
        public void AuthorItem_Update_UpdatesUpdatesCertificatesCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <author name=""authorName"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
        </author>
    </SectionName>
</configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                settingsFile.TryGetSection("SectionName", out var section).Should().BeTrue();
                section.Should().NotBeNull();

                section!.Items.Count.Should().Be(1);
                var item = (AuthorItem)section.Items.First();

                var updatedItem = (AuthorItem)item.Clone();
                var cert = updatedItem.Certificates.First();
                cert.HashAlgorithm = Common.HashAlgorithmName.SHA384;

                item.Update(updatedItem);
                SettingsTestUtils.DeepEquals(item, updatedItem).Should().BeTrue();

                settingsFile.SaveToDisk();

                // Assert
                var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <author name=""authorName"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA384"" allowUntrustedRoot=""true"" />
        </author>
    </SectionName>
</configuration>";

                Assert.Equal(result.Replace("\r\n", "\n"), File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void AuthorItem_Equals_WithSameName_ReturnsTrue()
        {
            var author1 = new AuthorItem("authorName",
                new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA512));
            var author2 = new AuthorItem("authorName",
                new CertificateItem("xyz", Common.HashAlgorithmName.SHA512));

            author1.Equals(author2).Should().BeTrue();
        }

        [Fact]
        public void AuthorItem_Equals_WithDifferentName_ReturnsFalse()
        {
            var author1 = new AuthorItem("authorName",
                new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA512));
            var author2 = new AuthorItem("otherAuthorName",
                new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA512));

            author1.Equals(author2).Should().BeFalse();
        }

        [Fact]
        public void AuthorItem_ElementName_IsCorrect()
        {
            var authorItem = new AuthorItem("authorName",
                new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA512));

            authorItem.ElementName.Should().Be("author");
        }

        [Fact]
        public void AuthorItem_Clone_CopiesTheSameItem()
        {
            // Arrange
            var config = @"
    <configuration>
        <SectionName>
            <author name=""authorName"">
                <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true""  />
            </author>
        </SectionName>
    </configuration>";
            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                settingsFile.TryGetSection("SectionName", out var section).Should().BeTrue();
                section.Should().NotBeNull();

                section!.Items.Count.Should().Be(1);
                var item = section.Items.First();
                item.IsCopy().Should().BeFalse();
                item.Origin.Should().NotBeNull();

                var clone = (AuthorItem)item.Clone();
                clone.IsCopy().Should().BeTrue();
                clone.Origin.Should().NotBeNull();
                SettingsTestUtils.DeepEquals(clone, item).Should().BeTrue();

                foreach (var cert in clone.Certificates)
                {
                    cert.IsCopy().Should().BeTrue();
                    cert.Origin.Should().NotBeNull();
                }
            }
        }
    }
}
