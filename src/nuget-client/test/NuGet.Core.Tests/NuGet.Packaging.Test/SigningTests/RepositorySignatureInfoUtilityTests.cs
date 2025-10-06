// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class RepositorySignatureInfoUtilityTests
    {
        private static SignedPackageVerifierSettings _defaultSettings = SignedPackageVerifierSettings.GetDefault(TestEnvironmentVariableReader.EmptyInstance);
        private static SignedPackageVerifierSettings _verifyCommandDefaultSettings = SignedPackageVerifierSettings.GetVerifyCommandDefaultPolicy(TestEnvironmentVariableReader.EmptyInstance);

        [Fact]
        public void GetSignedPackageVerifierSettings_NullFallbackSettingsThrows()
        {
            // Arrange & Act
            Action action = () => RepositorySignatureInfoUtility.GetSignedPackageVerifierSettings(repoSignatureInfo: null, fallbackSettings: null);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetSignedPackageVerifierSettings_NullRepoSignatureInfoReturnsFallbackSettings()
        {
            // Arrange & Act
            var settings = RepositorySignatureInfoUtility.GetSignedPackageVerifierSettings(repoSignatureInfo: null, fallbackSettings: _defaultSettings);

            // Assert
            settings.Should().Be(_defaultSettings);
        }

        [Fact]
        public void GetSignedPackageVerifierSettings_RepoSignatureInfoAllSignedTrue_ClearsAllowUnsignedIfSet()
        {
            // Arrange
            var allSigned = true;
            var repoSignatureInfo = new RepositorySignatureInfo(allSigned, repositoryCertificateInfos: null);

            // Act
            var settings = RepositorySignatureInfoUtility.GetSignedPackageVerifierSettings(repoSignatureInfo, _defaultSettings);

            // Assert
            settings.AllowUnsigned.Should().BeFalse();
            settings.AllowIllegal.Should().BeTrue();
            settings.AllowUntrusted.Should().BeFalse();
            settings.AllowIgnoreTimestamp.Should().BeTrue();
            settings.AllowNoTimestamp.Should().BeTrue();
            settings.AllowMultipleTimestamps.Should().BeTrue();
            settings.AllowUnknownRevocation.Should().BeTrue();
            settings.ReportUnknownRevocation.Should().BeFalse();
            settings.VerificationTarget.Should().Be(VerificationTarget.All);
            settings.SignaturePlacement.Should().Be(SignaturePlacement.Any);
            settings.RepositoryCountersignatureVerificationBehavior.Should().Be(SignatureVerificationBehavior.IfExistsAndIsNecessary);
        }

        [Fact]
        public void GetSignedPackageVerifierSettings_RepoSignatureInfoTrueAllSignedClearsAllowUnsignedIfNotSet()
        {
            // Arrange
            var allSigned = true;
            var repoSignatureInfo = new RepositorySignatureInfo(allSigned, repositoryCertificateInfos: null);

            // Act
            var settings = RepositorySignatureInfoUtility.GetSignedPackageVerifierSettings(repoSignatureInfo, _verifyCommandDefaultSettings);

            // Assert
            settings.AllowUnsigned.Should().BeFalse();
            settings.AllowIllegal.Should().BeFalse();
            settings.AllowUntrusted.Should().BeFalse();
            settings.AllowIgnoreTimestamp.Should().BeFalse();
            settings.AllowNoTimestamp.Should().BeTrue();
            settings.AllowMultipleTimestamps.Should().BeTrue();
            settings.AllowUnknownRevocation.Should().BeTrue();
            settings.ReportUnknownRevocation.Should().BeTrue();
            settings.VerificationTarget.Should().Be(VerificationTarget.All);
            settings.SignaturePlacement.Should().Be(SignaturePlacement.Any);
            settings.RepositoryCountersignatureVerificationBehavior.Should().Be(SignatureVerificationBehavior.IfExists);
        }

        [Fact]
        public void GetSignedPackageVerifierSettings_RepoSignatureInfoFalseAllSignedDoesNotSetAllowUnsigned()
        {
            // Arrange
            var repoSignatureInfo = new RepositorySignatureInfo(allRepositorySigned: false, repositoryCertificateInfos: null);

            // Act
            var settings = RepositorySignatureInfoUtility.GetSignedPackageVerifierSettings(repoSignatureInfo, _defaultSettings);

            // Assert
            settings.Should().BeEquivalentTo(_defaultSettings);
        }

        [Fact]
        public void GetSignedPackageVerifierSettings_RepoSignatureInfoFalseAllSignedDoesNotClearAllowUnsigned()
        {
            // Arrange
            var repoSignatureInfo = new RepositorySignatureInfo(allRepositorySigned: false, repositoryCertificateInfos: null);


            // Act
            var settings = RepositorySignatureInfoUtility.GetSignedPackageVerifierSettings(repoSignatureInfo, _verifyCommandDefaultSettings);

            // Assert
            settings.Should().BeEquivalentTo(_verifyCommandDefaultSettings);
        }

        [Fact]
        public void GetRepositoryAllowList_RepoSignatureInfoCertificateListWithOneEntryCorrectlyPassedToSetting()
        {
            // Arrange
            var target = VerificationTarget.Repository;
            var placement = SignaturePlacement.PrimarySignature | SignaturePlacement.Countersignature;

            var certFingerprints = new Dictionary<string, string>()
            {
                { HashAlgorithmName.SHA256.ConvertToOidString(), HashAlgorithmName.SHA256.ToString() },
                { HashAlgorithmName.SHA384.ConvertToOidString(), HashAlgorithmName.SHA384.ToString() },
                { HashAlgorithmName.SHA512.ConvertToOidString(), HashAlgorithmName.SHA512.ToString() },
                { "1.3.14.3.2.26", "SHA1" },
            };

            var testCertInfo = new TestRepositoryCertificateInfo()
            {
                ContentUrl = @"https://unit.test",
                Fingerprints = new Fingerprints(certFingerprints),
                Issuer = "CN=Issuer",
                Subject = "CN=Subject",
                NotBefore = DateTimeOffset.UtcNow,
                NotAfter = DateTimeOffset.UtcNow
            };

            var repoCertificateInfo = new List<IRepositoryCertificateInfo>()
            {
                testCertInfo
            };

            var expectedAllowList = new List<CertificateHashAllowListEntry>()
            {
                new CertificateHashAllowListEntry(target, placement, HashAlgorithmName.SHA256.ToString(), HashAlgorithmName.SHA256),
                new CertificateHashAllowListEntry(target, placement, HashAlgorithmName.SHA384.ToString(), HashAlgorithmName.SHA384),
                new CertificateHashAllowListEntry(target, placement, HashAlgorithmName.SHA512.ToString(), HashAlgorithmName.SHA512)
            };

            // Act
            var allowList = RepositorySignatureInfoUtility.GetRepositoryAllowList(repoCertificateInfo);

            // Assert
            allowList.Should().BeEquivalentTo(expectedAllowList);
        }

        [Fact]
        public void GetRepositoryAllowList_RepoSignatureInfoCertificateListWithMultipleEntriesCorrectlyPassedToSetting()
        {
            // Arrange
            var target = VerificationTarget.Repository;
            var placement = SignaturePlacement.PrimarySignature | SignaturePlacement.Countersignature;

            var firstCertFingerprints = new Dictionary<string, string>()
            {
                { HashAlgorithmName.SHA256.ConvertToOidString(), $"{HashAlgorithmName.SHA256.ToString()}_first" },
                { HashAlgorithmName.SHA384.ConvertToOidString(), $"{HashAlgorithmName.SHA384.ToString()}_first" },
                { HashAlgorithmName.SHA512.ConvertToOidString(), $"{HashAlgorithmName.SHA512.ToString()}_first" }
            };

            var secondCertFingerprints = new Dictionary<string, string>()
            {
                { HashAlgorithmName.SHA256.ConvertToOidString(), $"{HashAlgorithmName.SHA256.ToString()}_second"},
            };

            var repoCertificateInfo = new List<IRepositoryCertificateInfo>()
            {
                new TestRepositoryCertificateInfo()
                {
                    ContentUrl = @"https://unit.test/1",
                    Fingerprints = new Fingerprints(firstCertFingerprints),
                    Issuer = "CN=Issuer1",
                    Subject = "CN=Subject1",
                    NotBefore = DateTimeOffset.UtcNow,
                    NotAfter = DateTimeOffset.UtcNow
                },
                new TestRepositoryCertificateInfo()
                {
                    ContentUrl = @"https://unit.test/2",
                    Fingerprints = new Fingerprints(secondCertFingerprints),
                    Issuer = "CN=Issuer2",
                    Subject = "CN=Subject2",
                    NotBefore = DateTimeOffset.UtcNow,
                    NotAfter = DateTimeOffset.UtcNow
                }
            };

            var expectedAllowList = new List<CertificateHashAllowListEntry>()
            {
                new CertificateHashAllowListEntry(target, placement, $"{HashAlgorithmName.SHA256.ToString()}_first", HashAlgorithmName.SHA256),
                new CertificateHashAllowListEntry(target, placement, $"{HashAlgorithmName.SHA384.ToString()}_first", HashAlgorithmName.SHA384),
                new CertificateHashAllowListEntry(target, placement, $"{HashAlgorithmName.SHA512.ToString()}_first", HashAlgorithmName.SHA512),
                new CertificateHashAllowListEntry(target, placement, $"{HashAlgorithmName.SHA256.ToString()}_second", HashAlgorithmName.SHA256)
            };

            // Act
            var allowList = RepositorySignatureInfoUtility.GetRepositoryAllowList(repoCertificateInfo);

            // Assert
            allowList.Should().BeEquivalentTo(expectedAllowList);
        }
    }
}
