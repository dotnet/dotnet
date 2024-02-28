// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class TrustedSignerItemTests
    {
        [Fact]
        public void TrustedSigner_WithAuthorsAndRepositories_ParsedCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <SectionName>
        <repository name=""repositoryName"" serviceIndex=""https://api.test/index/"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""Sha256"" allowUntrustedRoot=""true""  />
            <owners>test;text</owners>
        </repository>
        <author name=""authorName"">
            <certificate fingerprint=""abcdefg"" hashAlgorithm=""Sha256"" allowUntrustedRoot=""true""  />
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
                var items = section!.Items.ToList();

                items.Count.Should().Be(2);

                var trustedSignerItem = (TrustedSignerItem)items[0];
                trustedSignerItem.Name.Should().Be("repositoryName");

                var repositoryitem = (RepositoryItem)items[0];
                var expectedRepositoryItem = new RepositoryItem("repositoryName", "https://api.test/index/", "test;text",
                    new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA256, allowUntrustedRoot: true));
                SettingsTestUtils.DeepEquals(repositoryitem, expectedRepositoryItem).Should().BeTrue();

                trustedSignerItem = (TrustedSignerItem)items[1];
                trustedSignerItem.Name.Should().Be("authorName");

                var authorItem = (AuthorItem)items[1];
                var expectedAuthorItem = new AuthorItem("authorName",
                    new CertificateItem("abcdefg", Common.HashAlgorithmName.SHA256, allowUntrustedRoot: true));
                SettingsTestUtils.DeepEquals(authorItem, expectedAuthorItem).Should().BeTrue();
            }
        }
    }
}
