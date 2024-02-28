// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class CertificateItemTests
    {
        [Fact]
        public void CertificateItem_WithoutFingerprint_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <certificate hashAlgorithm=""SHA256"" allowUntrustedRoot=""true""  />
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
                ex.Message.Should().Be(string.Format("Unable to parse config file because: Missing required attribute 'fingerprint' in element 'certificate'. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void CertificateItem_WithoutHashAlgorithm_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <certificate fingerprint=""abcdefg"" allowUntrustedRoot=""true""  />
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
                ex.Message.Should().Be(string.Format("Unable to parse config file because: Missing required attribute 'hashAlgorithm' in element 'certificate'. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void CertificateItem_WithUnsupportedHashAlgorithm_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA32"" allowUntrustedRoot=""true""  />
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
                ex.Message.Should().Be(string.Format("Unable to parse config file because: Certificate entry has an unsupported hash algorithm: 'SHA32'. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void CertificateItem_WithoutAllowUntrustedRoot_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" />
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
                ex.Message.Should().Be(string.Format("Unable to parse config file because: Missing required attribute 'allowUntrustedRoot' in element 'certificate'. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void CertificateItem_WithChildren_Throws()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"">
            <add key=""key"" value=""val"" />
        </certificate>
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
                ex.Message.Should().Be(string.Format("Error parsing NuGet.Config. Element 'certificate' cannot have descendant elements. Path: '{0}'.", Path.Combine(mockBaseDirectory, nugetConfigPath)));
            }
        }

        [Fact]
        public void CertificateItem_ParsedCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
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
                var item = (CertificateItem)section.Items.First();

                var expectedItem = new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA256, allowUntrustedRoot: true);
                SettingsTestUtils.DeepEquals(item, expectedItem).Should().BeTrue();
            }
        }

        [Fact]
        public void CertificateItem_Update_UpdatesHashAlgorithmCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
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
                var item = (CertificateItem)section.Items.First();

                var updatedItem = (CertificateItem)item.Clone();
                updatedItem.HashAlgorithm = Common.HashAlgorithmName.SHA512;

                item.Update(updatedItem);
                SettingsTestUtils.DeepEquals(item, updatedItem).Should().BeTrue();

                settingsFile.SaveToDisk();

                // Assert
                var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA512"" allowUntrustedRoot=""true"" />
    </SectionName>
</configuration>";

                Assert.Equal(result.Replace("\r\n", "\n"), File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void CertificateItem_Update_UpdatesAllowUntrustedRootCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
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
                var item = (CertificateItem)section.Items.First();

                var updatedItem = (CertificateItem)item.Clone();
                updatedItem.AllowUntrustedRoot = false;

                item.Update(updatedItem);
                SettingsTestUtils.DeepEquals(item, updatedItem).Should().BeTrue();

                settingsFile.SaveToDisk();

                // Assert
                var result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <SectionName>
        <certificate fingerprint=""abcdefg"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
    </SectionName>
</configuration>";

                Assert.Equal(result.Replace("\r\n", "\n"), File.ReadAllText(Path.Combine(mockBaseDirectory, nugetConfigPath)).Replace("\r\n", "\n"));
            }
        }

        [Fact]
        public void CertificateItem_Equals_WithSameFingerprint_ReturnsTrue()
        {
            var certificate1 = new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA512);
            var certificate2 = new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA256);

            certificate1.Equals(certificate2).Should().BeTrue();
        }

        [Fact]
        public void CertificateItem_Equals_WithDifferentFingerprint_ReturnsFalse()
        {
            var certificate1 = new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA512);
            var certificate2 = new CertificateItem("stuvxyz", Common.HashAlgorithmName.SHA512);

            certificate1.Equals(certificate2).Should().BeFalse();
        }

        [Fact]
        public void CertificateItem_ElementName_IsCorrect()
        {
            var certificateItem = new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA512);

            certificateItem.ElementName.Should().Be("certificate");
        }

        [Fact]
        public void CertificateItem_Clone_CopiesTheSameItem()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <certificate fingerprint=""abcdefg"" hashAlgorithm=""Sha256"" allowUntrustedRoot=""true"" />
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

                var clone = item.Clone();
                clone.IsCopy().Should().BeTrue();
                clone.Origin.Should().NotBeNull();
                SettingsTestUtils.DeepEquals(clone, item).Should().BeTrue();
            }
        }
    }
}
