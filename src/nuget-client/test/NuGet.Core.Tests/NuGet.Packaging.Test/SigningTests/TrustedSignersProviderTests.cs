// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Configuration.Test;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class TrustedSignersProviderTests
    {
        [Fact]
        public void TrustedSignersProvider_Constructor_WithNullSettings_Throws()
        {
            // Act and Assert
            var ex = Record.Exception(() => new TrustedSignersProvider(settings: null));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void GetTrustedSigners_WithoutTrustedSignersSection_ReturnsEmptyList()
        {
            // Arrange
            var config = @"
<configuration>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var trustedSignerProvider = new TrustedSignersProvider(settings);

                var trustedSigners = trustedSignerProvider.GetTrustedSigners();
                trustedSigners.Should().NotBeNull();
                trustedSigners.Should().BeEmpty();
            }
        }

        [Fact]
        public void GetTrustedSigner_WithEmptyTrustedSignersSection_ReturnsEmptyList()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var trustedSignerProvider = new TrustedSignersProvider(settings);

                var trustedSigners = trustedSignerProvider.GetTrustedSigners();
                trustedSigners.Should().NotBeNull();
                trustedSigners.Should().BeEmpty();
            }
        }

        [Fact]
        public void GetTrustedSigner_WithNonEmptyTrustedSignersSection_ReturnsAllTrustedSignersInSection()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
        <author name=""author1"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <author name=""author2"">
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <repository name=""repo1"" serviceIndex=""https://serviceIndex.test/v3/api.json"">
            <certificate fingerprint=""ghi"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </repository>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                var expectedTrustedSigners = new List<TrustedSignerItem>()
                {
                    new AuthorItem("author1", new CertificateItem("abc", HashAlgorithmName.SHA256)),
                    new AuthorItem("author2", new CertificateItem("def", HashAlgorithmName.SHA256)),
                    new RepositoryItem("repo1", "https://serviceIndex.test/v3/api.json", new CertificateItem("ghi", HashAlgorithmName.SHA256))
                };

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var trustedSignerProvider = new TrustedSignersProvider(settings);

                var trustedSigners = trustedSignerProvider.GetTrustedSigners();
                trustedSigners.Should().NotBeNull();
                trustedSigners.Count.Should().Be(3);
                trustedSigners.Should().BeEquivalentTo(expectedTrustedSigners,
                    options => options
                        .Excluding(o => o.Path == "[0].Origin")
                        .Excluding(o => o.Path == "[1].Origin")
                        .Excluding(o => o.Path == "[2].Origin"));
            }
        }

        [Fact]
        public void GetTrustedSigner_WithItemsDifferentThanTrustedSigners_IgnoresThemAndOnlyReturnsTrustedSigners()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
        <add key=""oneKey"" value=""val"" />
        <author name=""author1"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <author name=""author2"">
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <certificate fingerprint=""ghi"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        <repository name=""repo1"" serviceIndex=""https://serviceIndex.test/v3/api.json"">
            <certificate fingerprint=""ghi"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </repository>
        <add key=""otherKey"" value=""val"" />
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                var expectedTrustedSigners = new List<TrustedSignerItem>()
                {
                    new AuthorItem("author1", new CertificateItem("abc", HashAlgorithmName.SHA256)),
                    new AuthorItem("author2", new CertificateItem("def", HashAlgorithmName.SHA256)),
                    new RepositoryItem("repo1", "https://serviceIndex.test/v3/api.json", new CertificateItem("ghi", HashAlgorithmName.SHA256))
                };

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var trustedSignerProvider = new TrustedSignersProvider(settings);

                var trustedSigners = trustedSignerProvider.GetTrustedSigners();
                trustedSigners.Should().NotBeNull();
                trustedSigners.Count.Should().Be(3);
                trustedSigners.Should().BeEquivalentTo(
                    expectedTrustedSigners,
                    options => options
                        .Excluding(o => o.Path == "[0].Origin")
                        .Excluding(o => o.Path == "[1].Origin")
                        .Excluding(o => o.Path == "[2].Origin"));
            }
        }

        [Fact]
        public void Remove_WithNullListOFSignersToRemove_Throws()
        {
            var trustedSignersProvider = new TrustedSignersProvider(settings: NullSettings.Instance);

            // Act and Assert
            var ex = Record.Exception(() => trustedSignersProvider.Remove(trustedSigners: null));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentException>();
        }


        [Fact]
        public void Remove_WithEmptyListOFSignersToRemove_Throws()
        {
            var trustedSignersProvider = new TrustedSignersProvider(settings: NullSettings.Instance);

            // Act and Assert
            var ex = Record.Exception(() => trustedSignersProvider.Remove(trustedSigners: new List<TrustedSignerItem>()));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentException>();
        }

        [Fact]
        public void Remove_SuccessfullyRemovesTrustedSigners()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
        <author name=""author1"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <author name=""author2"">
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <repository name=""repo1"" serviceIndex=""https://serviceIndex.test/v3/api.json"">
            <certificate fingerprint=""ghi"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </repository>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                var expectedTrustedSigners = new List<TrustedSignerItem>()
                {
                    new AuthorItem("author2", new CertificateItem("def", HashAlgorithmName.SHA256)),
                    new RepositoryItem("repo1", "https://serviceIndex.test/v3/api.json", new CertificateItem("ghi", HashAlgorithmName.SHA256))
                };

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var trustedSignerProvider = new TrustedSignersProvider(settings);

                var trustedSigners = trustedSignerProvider.GetTrustedSigners();
                trustedSignerProvider.Remove(new[] { trustedSigners.First() });

                trustedSigners = trustedSignerProvider.GetTrustedSigners();
                trustedSigners.Should().NotBeNull();
                trustedSigners.Count.Should().Be(2);
                trustedSigners.Should().BeEquivalentTo(
                    expectedTrustedSigners,
                    options => options
                        .Excluding(o => o.Path == "[0].Origin")
                        .Excluding(o => o.Path == "[1].Origin"));
            }
        }

        [Fact]
        public void Remove_IgnoresAnyUnexistantTrustedSigner()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
        <author name=""author1"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <author name=""author2"">
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <repository name=""repo1"" serviceIndex=""https://serviceIndex.test/v3/api.json"">
            <certificate fingerprint=""ghi"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </repository>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                var expectedTrustedSigners = new List<TrustedSignerItem>()
                {
                    new AuthorItem("author2", new CertificateItem("def", HashAlgorithmName.SHA256)),
                    new RepositoryItem("repo1", "https://serviceIndex.test/v3/api.json", new CertificateItem("ghi", HashAlgorithmName.SHA256))
                };

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var trustedSignerProvider = new TrustedSignersProvider(settings);

                var trustedSigners = trustedSignerProvider.GetTrustedSigners();
                trustedSignerProvider.Remove(new[] { trustedSigners.First(), new AuthorItem("DontExist", new CertificateItem("abc", HashAlgorithmName.SHA256)) });

                trustedSigners = trustedSignerProvider.GetTrustedSigners();
                trustedSigners.Should().NotBeNull();
                trustedSigners.Count.Should().Be(2);
                trustedSigners.Should().BeEquivalentTo(
                    expectedTrustedSigners,
                    options => options
                        .Excluding(o => o.Path == "[0].Origin")
                        .Excluding(o => o.Path == "[1].Origin"));
            }
        }

        [Fact]
        public void AddOrUpdateTrustedSigner_WithNullItem_Throws()
        {
            var trustedSignersProvider = new TrustedSignersProvider(settings: NullSettings.Instance);

            // Act and Assert
            var ex = Record.Exception(() => trustedSignersProvider.AddOrUpdateTrustedSigner(trustedSigner: null));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void AddOrUpdateTrustedSigner_WithNewTrustedSigner_AddsItSuccesfully()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
        <author name=""author1"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <author name=""author2"">
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <repository name=""repo1"" serviceIndex=""https://serviceIndex.test/v3/api.json"">
            <certificate fingerprint=""ghi"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </repository>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                var expectedTrustedSigners = new List<TrustedSignerItem>()
                {
                    new AuthorItem("author1", new CertificateItem("abc", HashAlgorithmName.SHA256)),
                    new AuthorItem("author2", new CertificateItem("def", HashAlgorithmName.SHA256)),
                    new RepositoryItem("repo1", "https://serviceIndex.test/v3/api.json", new CertificateItem("ghi", HashAlgorithmName.SHA256)),
                    new AuthorItem("author3", new CertificateItem("jkl", HashAlgorithmName.SHA256)),
                };

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var trustedSignerProvider = new TrustedSignersProvider(settings);

                trustedSignerProvider.AddOrUpdateTrustedSigner(new AuthorItem("author3", new CertificateItem("jkl", HashAlgorithmName.SHA256)));

                var trustedSigners = trustedSignerProvider.GetTrustedSigners();
                trustedSigners.Should().NotBeNull();
                trustedSigners.Count.Should().Be(4);
                trustedSigners.Should().BeEquivalentTo(
                    expectedTrustedSigners,
                    options => options
                        .Excluding(o => o.Path == "[0].Origin")
                        .Excluding(o => o.Path == "[1].Origin")
                        .Excluding(o => o.Path == "[2].Origin")
                        .Excluding(o => o.Path == "[3].Origin"));
            }
        }

        [Fact]
        public void AddOrUpdateTrustedSigner_WithExistingTrustedSigner_UpdatesItSuccesfully()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
        <author name=""author1"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <author name=""author2"">
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <repository name=""repo1"" serviceIndex=""https://serviceIndex.test/v3/api.json"">
            <certificate fingerprint=""ghi"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </repository>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                var expectedTrustedSigners = new List<TrustedSignerItem>()
                {
                    new AuthorItem("author1", new CertificateItem("jkl", HashAlgorithmName.SHA256)),
                    new AuthorItem("author2", new CertificateItem("def", HashAlgorithmName.SHA256)),
                    new RepositoryItem("repo1", "https://serviceIndex.test/v3/api.json", new CertificateItem("ghi", HashAlgorithmName.SHA256))
                };

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var trustedSignerProvider = new TrustedSignersProvider(settings);

                trustedSignerProvider.AddOrUpdateTrustedSigner(new AuthorItem("author1", new CertificateItem("jkl", HashAlgorithmName.SHA256)));

                var trustedSigners = trustedSignerProvider.GetTrustedSigners();
                trustedSigners.Should().NotBeNull();
                trustedSigners.Count.Should().Be(3);
                trustedSigners.Should().BeEquivalentTo(expectedTrustedSigners,
                    options => options
                        .Excluding(o => o.Path == "[0].Origin")
                        .Excluding(o => o.Path == "[1].Origin")
                        .Excluding(o => o.Path == "[2].Origin"));
            }
        }

        [Fact]
        public void GetAllowListEntries_WithNullSettings_Throws()
        {
            // Act and Assert
            var ex = Record.Exception(() => TrustedSignersProvider.GetAllowListEntries(settings: null, logger: NullLogger.Instance));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void GetAllowListEntries_WithNullLogger_Throws()
        {
            // Act and Assert
            var ex = Record.Exception(() => TrustedSignersProvider.GetAllowListEntries(settings: NullSettings.Instance, logger: null));

            ex.Should().NotBeNull();
            ex.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void GetAllowListEntries_WithoutTrustedSignersSection_ReturnsEmptyList()
        {
            // Arrange
            var config = @"
<configuration>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var entries = TrustedSignersProvider.GetAllowListEntries(settings, NullLogger.Instance);
                entries.Should().NotBeNull();
                entries.Should().BeEmpty();
            }
        }

        [Fact]
        public void GetAllowListEntries_WithEmptyTrustedSignersSection_ReturnsEmptyList()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var entries = TrustedSignersProvider.GetAllowListEntries(settings, NullLogger.Instance);
                entries.Should().NotBeNull();
                entries.Should().BeEmpty();
            }
        }

        [Fact]
        public void GetAllowListEntries_TrustedAuthor_WithOneCertificate_ParsedCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
        <author name=""author1"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <author name=""author2"">
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var entries = TrustedSignersProvider.GetAllowListEntries(settings, NullLogger.Instance);
                entries.Should().NotBeNull();
                entries.Count.Should().Be(2);

                var expectedEntries = new List<VerificationAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Author, SignaturePlacement.PrimarySignature, "abc", HashAlgorithmName.SHA256),
                    new TrustedSignerAllowListEntry(VerificationTarget.Author, SignaturePlacement.PrimarySignature, "def", HashAlgorithmName.SHA256)
                };

                entries.Should().BeEquivalentTo(expectedEntries);
            }
        }

        [Fact]
        public void GetAllowListEntries_TrustedAuthor_WithMultipleCertificates_ParsedCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
        <author name=""author1"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""ghi"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <author name=""author2"">
            <certificate fingerprint=""jkl"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""mno"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var entries = TrustedSignersProvider.GetAllowListEntries(settings, NullLogger.Instance);
                entries.Should().NotBeNull();
                entries.Count.Should().Be(5);

                var expectedEntries = new List<VerificationAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Author, SignaturePlacement.PrimarySignature, "abc", HashAlgorithmName.SHA256),
                    new TrustedSignerAllowListEntry(VerificationTarget.Author, SignaturePlacement.PrimarySignature, "def", HashAlgorithmName.SHA256),
                    new TrustedSignerAllowListEntry(VerificationTarget.Author, SignaturePlacement.PrimarySignature, "ghi", HashAlgorithmName.SHA256),
                    new TrustedSignerAllowListEntry(VerificationTarget.Author, SignaturePlacement.PrimarySignature, "jkl", HashAlgorithmName.SHA256),
                    new TrustedSignerAllowListEntry(VerificationTarget.Author, SignaturePlacement.PrimarySignature, "mno", HashAlgorithmName.SHA256)
                };

                entries.Should().BeEquivalentTo(expectedEntries);
            }
        }

        [Fact]
        public void GetAllowListEntries_TrustedRepository_WithOneCertificate_ParsedCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
        <repository name=""repository1"" serviceIndex=""api.v3ServiceIndex.test/json"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </repository>
        <repository name=""repository2"" serviceIndex=""api.v3ServiceIndex.test/json"">
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </repository>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var entries = TrustedSignersProvider.GetAllowListEntries(settings, NullLogger.Instance);
                entries.Should().NotBeNull();
                entries.Count.Should().Be(2);

                var expectedEntries = new List<VerificationAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "abc", HashAlgorithmName.SHA256),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "def", HashAlgorithmName.SHA256)
                };

                entries.Should().BeEquivalentTo(expectedEntries);
            }
        }

        [Fact]
        public void GetAllowListEntries_TrustedRepository_WithMultipleCertificates_ParsedCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
        <repository name=""repository1"" serviceIndex=""api.v3ServiceIndex.test/json"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""ghi"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </repository>
        <repository name=""repository2"" serviceIndex=""api.v3ServiceIndex.test/json"">
            <certificate fingerprint=""jkl"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""mno"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </repository>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var entries = TrustedSignersProvider.GetAllowListEntries(settings, NullLogger.Instance);
                entries.Should().NotBeNull();
                entries.Count.Should().Be(5);

                var expectedEntries = new List<VerificationAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "abc", HashAlgorithmName.SHA256),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "def", HashAlgorithmName.SHA256),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "ghi", HashAlgorithmName.SHA256),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "jkl", HashAlgorithmName.SHA256),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "mno", HashAlgorithmName.SHA256)
                };

                entries.Should().BeEquivalentTo(expectedEntries);
            }
        }

        [Fact]
        public void GetAllowListEntries_TrustedRepository_WithMultipleCertificates_WithOwners_ParsedCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
        <repository name=""repository1"" serviceIndex=""api.v3ServiceIndex.test/json"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""ghi"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <owners>nuget;randomOwner</owners>
        </repository>
        <repository name=""repository2"" serviceIndex=""api.v3ServiceIndex.test/json"">
            <certificate fingerprint=""jkl"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""mno"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <owners>anotherOwner;owner3</owners>
        </repository>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var entries = TrustedSignersProvider.GetAllowListEntries(settings, NullLogger.Instance);
                entries.Should().NotBeNull();
                entries.Count.Should().Be(5);

                var expectedEntries = new List<VerificationAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "abc", HashAlgorithmName.SHA256, owners: new List<string>() { "nuget", "randomOwner" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "def", HashAlgorithmName.SHA256, owners: new List<string>() { "nuget", "randomOwner" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "ghi", HashAlgorithmName.SHA256, owners: new List<string>() { "nuget", "randomOwner" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "jkl", HashAlgorithmName.SHA256, owners: new List<string>() { "anotherOwner", "owner3" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "mno", HashAlgorithmName.SHA256, owners: new List<string>() { "anotherOwner", "owner3" })
                };

                entries.Should().BeEquivalentTo(expectedEntries);
            }
        }

        [Fact]
        public void GetAllowListEntries_TrustedAuthorsAndRepositories_ParsedCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
        <repository name=""repository1"" serviceIndex=""api.v3ServiceIndex.test/json"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""ghi"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <owners>nuget;randomOwner</owners>
        </repository>
        <author name=""author1"">
            <certificate fingerprint=""xyz"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <author name=""author2"">
            <certificate fingerprint=""pqr"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""stu"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <repository name=""repository2"" serviceIndex=""api.v3ServiceIndex.test/json"">
            <certificate fingerprint=""jkl"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""mno"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <owners>anotherOwner;owner3</owners>
        </repository>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var entries = TrustedSignersProvider.GetAllowListEntries(settings, NullLogger.Instance);
                entries.Should().NotBeNull();
                entries.Count.Should().Be(8);

                var expectedEntries = new List<VerificationAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "abc", HashAlgorithmName.SHA256, owners: new List<string>() { "nuget", "randomOwner" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "def", HashAlgorithmName.SHA256, owners: new List<string>() { "nuget", "randomOwner" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "ghi", HashAlgorithmName.SHA256, owners: new List<string>() { "nuget", "randomOwner" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Author, SignaturePlacement.PrimarySignature, "xyz", HashAlgorithmName.SHA256),
                    new TrustedSignerAllowListEntry(VerificationTarget.Author, SignaturePlacement.PrimarySignature, "pqr", HashAlgorithmName.SHA256),
                    new TrustedSignerAllowListEntry(VerificationTarget.Author, SignaturePlacement.PrimarySignature, "stu", HashAlgorithmName.SHA256),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "jkl", HashAlgorithmName.SHA256, owners: new List<string>() { "anotherOwner", "owner3" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "mno", HashAlgorithmName.SHA256, owners: new List<string>() { "anotherOwner", "owner3" })
                };

                entries.Should().BeEquivalentTo(expectedEntries);
            }
        }

        [Fact]
        public void GetAllowListEntries_WithDuplicateEntries_IgnoresDuplicates()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
        <repository name=""repository1"" serviceIndex=""api.v3ServiceIndex.test/json"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <owners>nuget;randomOwner</owners>
        </repository>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var entries = TrustedSignersProvider.GetAllowListEntries(settings, NullLogger.Instance);
                entries.Should().NotBeNull();
                entries.Count.Should().Be(2);

                var expectedEntries = new List<VerificationAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "abc", HashAlgorithmName.SHA256, owners: new List<string>() { "nuget", "randomOwner" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "def", HashAlgorithmName.SHA256, owners: new List<string>() { "nuget", "randomOwner" }),
                };

                entries.Should().BeEquivalentTo(expectedEntries);
            }
        }

        [Fact]
        public void GetAllowListEntries_WithDuplicateEntries_UpdatesVerificationTarget()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
        <repository name=""repository1"" serviceIndex=""api.v3ServiceIndex.test/json"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <owners>nuget;randomOwner</owners>
        </repository>
        <author name=""author1"">
            <certificate fingerprint=""jkl"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var entries = TrustedSignersProvider.GetAllowListEntries(settings, NullLogger.Instance);
                entries.Should().NotBeNull();
                entries.Count.Should().Be(3);

                var expectedEntries = new List<VerificationAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Author|VerificationTarget.Repository, SignaturePlacement.Any, "abc", HashAlgorithmName.SHA256, owners: new List<string>() { "nuget", "randomOwner" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "def", HashAlgorithmName.SHA256, owners: new List<string>() { "nuget", "randomOwner" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Author, SignaturePlacement.PrimarySignature, "jkl", HashAlgorithmName.SHA256),
                };

                entries.Should().BeEquivalentTo(expectedEntries);
            }
        }

        [Fact]
        public void GetAllowListEntries_WithDuplicateEntries_AppendsOwners()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
        <repository name=""repository1"" serviceIndex=""api.v3ServiceIndex.test/json"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <owners>nuget;randomOwner</owners>
        </repository>
        <repository name=""repository2"" serviceIndex=""api.v3ServiceIndex2.test/json"">
            <certificate fingerprint=""jkl"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <owners>otherOwner</owners>
        </repository>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var entries = TrustedSignersProvider.GetAllowListEntries(settings, NullLogger.Instance);
                entries.Should().NotBeNull();
                entries.Count.Should().Be(3);

                var expectedEntries = new List<VerificationAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "abc", HashAlgorithmName.SHA256, owners: new List<string>() { "nuget", "randomOwner", "otherOwner" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "def", HashAlgorithmName.SHA256, owners: new List<string>() { "nuget", "randomOwner" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "jkl", HashAlgorithmName.SHA256, owners: new List<string>() { "otherOwner" }),
                };

                entries.Should().BeEquivalentTo(expectedEntries);
            }
        }


        [Fact]
        public void GetAllowListEntries_WithDuplicateFingerprints_DifferentHashAlgorithm_TakesThemAsDifferentEntries()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
        <repository name=""repository1"" serviceIndex=""api.v3ServiceIndex.test/json"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA512"" allowUntrustedRoot=""false"" />
            <owners>nuget;randomOwner</owners>
        </repository>
        <author name=""author1"">
            <certificate fingerprint=""jkl"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var entries = TrustedSignersProvider.GetAllowListEntries(settings, NullLogger.Instance);
                entries.Should().NotBeNull();
                entries.Count.Should().Be(4);

                var expectedEntries = new List<VerificationAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "abc", HashAlgorithmName.SHA512, owners: new List<string>() { "nuget", "randomOwner" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Author|VerificationTarget.Repository, SignaturePlacement.Any, "abc", HashAlgorithmName.SHA256, owners: new List<string>() { "nuget", "randomOwner" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "def", HashAlgorithmName.SHA256, owners: new List<string>() { "nuget", "randomOwner" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Author, SignaturePlacement.PrimarySignature, "jkl", HashAlgorithmName.SHA256),
                };

                entries.Should().BeEquivalentTo(expectedEntries);
            }
        }

        [Fact]
        public void GetAllowListEntries_WithDuplicateEntries_ConflictingAllowUntrustedRoot_WarnsAndSetsToFalse()
        {
            // Arrange
            var config = @"
<configuration>
    <trustedSigners>
        <repository name=""repository1"" serviceIndex=""api.v3ServiceIndex.test/json"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA512"" allowUntrustedRoot=""false"" />
            <owners>nuget;randomOwner</owners>
        </repository>
        <author name=""author1"">
            <certificate fingerprint=""jkl"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
        </author>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var logger = new Mock<ILogger>();

                var entries = TrustedSignersProvider.GetAllowListEntries(settings, logger.Object);
                entries.Should().NotBeNull();
                entries.Count.Should().Be(4);

                logger.Verify(l =>
                    l.Log(It.Is<ILogMessage>(m =>
                        m.Level == LogLevel.Warning &&
                        m.Code == NuGetLogCode.NU3040)));

                var expectedEntries = new List<VerificationAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "abc", HashAlgorithmName.SHA512, owners: new List<string>() { "nuget", "randomOwner" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Author|VerificationTarget.Repository, SignaturePlacement.Any, "abc", HashAlgorithmName.SHA256, owners: new List<string>() { "nuget", "randomOwner" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any, "def", HashAlgorithmName.SHA256, owners: new List<string>() { "nuget", "randomOwner" }),
                    new TrustedSignerAllowListEntry(VerificationTarget.Author, SignaturePlacement.PrimarySignature, "jkl", HashAlgorithmName.SHA256),
                };

                entries.Should().BeEquivalentTo(expectedEntries);
            }
        }
    }
}
