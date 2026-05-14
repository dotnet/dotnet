// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using NuGet.Packaging;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class RepositoryItemTests
    {
        [Fact]
        public void RepositoryItem_WithoutName_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <repository serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""Sha256"" allowUntrustedRoot=""true""  />
        </repository>
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
                ex.Message.Should().Be(string.Format("Unable to parse config file because: Missing required attribute 'name' in element 'repository'. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void RepositoryItem_WithoutServiceIndex_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <repository name=""repositoryName"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""Sha256"" allowUntrustedRoot=""true""  />
        </repository>
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
                ex.Message.Should().Be(string.Format("Unable to parse config file because: Missing required attribute 'serviceIndex' in element 'repository'. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void RepositoryItem_WithoutCertificates_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
        </repository>
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
        public void RepositoryItem_WithEmptyOwners_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""Sha256"" allowUntrustedRoot=""true""  />
            <owners></owners>
        </repository>
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
                ex.Message.Should().Be(string.Format("Unable to parse config file because: Owners item must only have text content and cannot be empty. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void RepositoryItem_WithMultipleOwners_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""Sha256"" allowUntrustedRoot=""true""  />
            <owners>one,two,three</owners>
            <owners>four,five,six</owners>
        </repository>
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
                ex.Message.Should().Be(string.Format("Unable to parse config file because: The repository item with name 'repositoryName' and service index 'https://api.test/index/' has more than one owners item in it. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void RepositoryItem_WithCertificatesAndNoOwners_ParsedCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""Sha256"" allowUntrustedRoot=""true""  />
        </repository>
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
                var item = (RepositoryItem)section.Items.First();

                var expectedItem = new RepositoryItem("repositoryName", "https://api.test/index/",
                    new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA256, allowUntrustedRoot: true));
                SettingsTestUtils.DeepEquals(item, expectedItem).Should().BeTrue();
            }
        }

        [Fact]
        public void RepositoryItem_WithCertificatesAndOwners_ParsedCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""Sha256"" allowUntrustedRoot=""true""  />
            <owners>test;text</owners>
        </repository>
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
                var item = (RepositoryItem)section.Items.First();

                var expectedItem = new RepositoryItem("repositoryName", "https://api.test/index/", "test;text",
                    new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA256, allowUntrustedRoot: true));
                SettingsTestUtils.DeepEquals(item, expectedItem).Should().BeTrue();
            }
        }

        [Fact]
        public void RepositoryItem_Update_AddsCertificatesCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
        </repository>
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
                var item = (RepositoryItem)section.Items.First();

                var updatedItem = (RepositoryItem)item.Clone();
                updatedItem.Certificates.Add(new CertificateItem("xyz", Common.HashAlgorithmName.SHA256));

                item.Update(updatedItem);
                SettingsTestUtils.DeepEquals(item, updatedItem).Should().BeTrue();

                settingsFile.SaveToDisk();

                // Assert
                var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
            <certificate fingerprint=""xyz"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </repository>
    </SectionName>
</configuration>";

                Assert.Equal(result.Replace("\r\n", "\n"), File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void RepositoryItem_Update_RemovesCertificatesCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
            <certificate fingerprint=""xyz"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </repository>
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
                var item = (RepositoryItem)section.Items.Last();
                item.Certificates.Count.Should().Be(2);

                var updatedItem = (RepositoryItem)item.Clone();
                updatedItem.Certificates.RemoveAt(1);

                item.Update(updatedItem);
                SettingsTestUtils.DeepEquals(item, updatedItem).Should().BeTrue();

                settingsFile.SaveToDisk();

                // Assert
                var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
        </repository>
    </SectionName>
</configuration>";

                Assert.Equal(result.Replace("\r\n", "\n"), File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
            }
        }


        [Fact]
        public void RepositoryItem_Update_RemovingAllCertificates_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
        </repository>
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
                var item = (RepositoryItem)section.Items.First();
                item.Certificates.Count.Should().Be(1);

                var updatedItem = (RepositoryItem)item.Clone();
                updatedItem.Certificates.Clear();

                var ex = Record.Exception(() => item.Update(updatedItem));

                ex.Should().NotBeNull();
                ex.Should().BeOfType<InvalidOperationException>();
                ex.Message.Should().Be("A trusted signer entry must have at least one certificate entry.");
            }
        }


        [Fact]
        public void RepositoryItem_Update_UpdatesCertificatesCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
        </repository>
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
                var item = (RepositoryItem)section.Items.First();

                var updatedItem = (RepositoryItem)item.Clone();
                var cert = updatedItem.Certificates.First();
                cert.HashAlgorithm = Common.HashAlgorithmName.SHA384;

                item.Update(updatedItem);
                SettingsTestUtils.DeepEquals(item, updatedItem).Should().BeTrue();

                settingsFile.SaveToDisk();

                // Assert
                var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA384"" allowUntrustedRoot=""true"" />
        </repository>
    </SectionName>
</configuration>";

                Assert.Equal(result.Replace("\r\n", "\n"), File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void RepositoryItem_Update_AddOwnersForTheFirstTime_AddsOwnersEntry()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
        </repository>
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
                var item = (RepositoryItem)section.Items.First();
                item.Owners.Count.Should().Be(0);

                var updatedItem = (RepositoryItem)item.Clone();
                updatedItem.Owners.Add("owner1");

                item.Update(updatedItem);
                SettingsTestUtils.DeepEquals(item, updatedItem).Should().BeTrue();

                settingsFile.SaveToDisk();

                // Assert
                var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
            <owners>owner1</owners>
        </repository>
    </SectionName>
</configuration>";

                Assert.Equal(result.Replace("\r\n", "\n"), File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void RepositoryItem_Update_AddsOwnersCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
            <owners>owner1</owners>
        </repository>
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
                var item = (RepositoryItem)section.Items.First();
                item.Owners.Count.Should().Be(1);

                var updatedItem = (RepositoryItem)item.Clone();
                updatedItem.Owners.AddRange(new[] { "owner2", "owner3" });

                item.Update(updatedItem);
                SettingsTestUtils.DeepEquals(item, updatedItem).Should().BeTrue();

                settingsFile.SaveToDisk();

                // Assert
                var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
            <owners>owner1;owner2;owner3</owners>
        </repository>
    </SectionName>
</configuration>";

                Assert.Equal(result.Replace("\r\n", "\n"), File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void RepositoryItem_Update_RemovesOwnersCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
            <owners>owner1;owner2</owners>
        </repository>
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
                var item = (RepositoryItem)section.Items.First();
                item.Owners.Count.Should().Be(2);

                var updatedItem = (RepositoryItem)item.Clone();
                updatedItem.Owners.RemoveAt(1);

                item.Update(updatedItem);
                SettingsTestUtils.DeepEquals(item, updatedItem).Should().BeTrue();

                settingsFile.SaveToDisk();

                // Assert
                var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
            <owners>owner1</owners>
        </repository>
    </SectionName>
</configuration>";

                Assert.Equal(result.Replace("\r\n", "\n"), File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
            }
        }


        [Fact]
        public void RepositoryItem_Update_RemovingAllOwners_RemovesOwenrsEntry()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
            <owners>owner1;owner2</owners>
        </repository>
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
                var item = (RepositoryItem)section.Items.First();
                item.Owners.Count.Should().Be(2);

                var updatedItem = (RepositoryItem)item.Clone();
                updatedItem.Owners.Clear();

                item.Update(updatedItem);
                SettingsTestUtils.DeepEquals(item, updatedItem).Should().BeTrue();

                settingsFile.SaveToDisk();

                // Assert
                var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
        </repository>
    </SectionName>
</configuration>";

                Assert.Equal(result.Replace("\r\n", "\n"), File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void RepositoryItem_Update_UpdatesNameCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
            <owners>owner1;owner2</owners>
        </repository>
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
                var item = (RepositoryItem)section.Items.First();

                var updatedItem = (RepositoryItem)item.Clone();
                updatedItem.Name = "newName";

                item.Update(updatedItem);
                SettingsTestUtils.DeepEquals(item, updatedItem).Should().BeTrue();

                settingsFile.SaveToDisk();

                // Assert
                var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <repository name=""newName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
            <owners>owner1;owner2</owners>
        </repository>
    </SectionName>
</configuration>";

                Assert.Equal(result.Replace("\r\n", "\n"), File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
            }
        }


        [Fact]
        public void RepositoryItem_Equals_WithSameServiceIndex_ReturnsTrue()
        {
            var repository1 = new RepositoryItem("repositoryName", "https://api.test/index/",
             new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA512));

            var repository2 = new RepositoryItem("otherRepositoryName", "https://api.test/index/",
                new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA512));

            repository1.Equals(repository2).Should().BeTrue();
        }

        [Fact]
        public void RepositoryItem_Equals_WithDifferentServiceIndex_ReturnsFalse()
        {
            var repository1 = new RepositoryItem("repositoryName", "https://api1.test/index/",
                new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA512));

            var repository2 = new RepositoryItem("repositoryName", "https://api2.test/index/",
                new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA512));

            repository1.Equals(repository2).Should().BeFalse();
        }

        [Fact]
        public void RepositoryItem_ElementName_IsCorrect()
        {
            var repositoryItem = new RepositoryItem("repositoryName", "https://api.test/index/",
                new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA512));

            repositoryItem.ElementName.Should().Be("repository");
        }

        [Fact]
        public void RepositoryItem_Clone_CopiesTheSameItem()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""Sha256"" allowUntrustedRoot=""true""  />
            <owners>test;text</owners>
        </repository>
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

                var clone = (RepositoryItem)item.Clone();
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
