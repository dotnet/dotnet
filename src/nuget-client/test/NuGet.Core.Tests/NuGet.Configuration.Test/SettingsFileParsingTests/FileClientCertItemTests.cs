// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class FileClientCertItemTests
    {
        [Fact]
        public void FileClientCertItem_WithoutPassword_ParsedCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
   <SectionName>
      <fileCert packageSource=""Foo"" path = "".\certificate.pfx"" />
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
                var items = section!.Items.ToList();

                items.Count.Should().Be(1);

                var fileClientCertItem = (FileClientCertItem)items[0];
                fileClientCertItem.ElementName.Should().Be("fileCert");
                fileClientCertItem.PackageSource.Should().Be("Foo");
                fileClientCertItem.FilePath.Should().Be(@".\certificate.pfx");
                fileClientCertItem.IsPasswordIsClearText.Should().Be(false);
                fileClientCertItem.Password.Should().Be(null);

                var expectedFileClientCertItem = new FileClientCertItem("Foo",
                                                                        @".\certificate.pfx",
                                                                        null,
                                                                        false,
                                                                        Path.Combine(mockBaseDirectory, nugetConfigPath));

                SettingsTestUtils.DeepEquals(fileClientCertItem, expectedFileClientCertItem).Should().BeTrue();
            }
        }

        [Fact]
        public void FileClientCertItem_WithClearTextPassword_ParsedCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
   <SectionName>
      <fileCert packageSource=""Foo"" path="".\certificate.pfx"" clearTextPassword=""..."" />
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
                var items = section!.Items.ToList();

                items.Count.Should().Be(1);

                var fileClientCertItem = (FileClientCertItem)items[0];
                fileClientCertItem.ElementName.Should().Be("fileCert");
                fileClientCertItem.PackageSource.Should().Be("Foo");
                fileClientCertItem.FilePath.Should().Be(@".\certificate.pfx");
                fileClientCertItem.Password.Should().Be(@"...");
                fileClientCertItem.IsPasswordIsClearText.Should().Be(true);

                var expectedFileClientCertItem = new FileClientCertItem("Foo",
                                                                        @".\certificate.pfx",
                                                                        "...",
                                                                        true,
                                                                        Path.Combine(mockBaseDirectory, nugetConfigPath));

                SettingsTestUtils.DeepEquals(fileClientCertItem, expectedFileClientCertItem).Should().BeTrue();
            }
        }
    }
}
