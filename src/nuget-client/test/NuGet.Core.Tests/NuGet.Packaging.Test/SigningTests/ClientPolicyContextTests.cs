// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using FluentAssertions;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Configuration.Test;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class ClientPolicyContextTests
    {
        [Fact]
        public void GetClientPolicy_WhenNoClientPolicy_DefaultsToAccept()
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

                var clientPolicyContext = ClientPolicyContext.GetClientPolicy(settings, NullLogger.Instance);

                clientPolicyContext.Policy.Should().Be(SignatureValidationMode.Accept);
                clientPolicyContext.AllowList.Should().BeEmpty();

                var verifierSettings = clientPolicyContext.VerifierSettings;

                verifierSettings.AllowUnsigned.Should().BeTrue();
                verifierSettings.AllowIllegal.Should().BeTrue();
                verifierSettings.AllowUntrusted.Should().BeTrue();
                verifierSettings.AllowIgnoreTimestamp.Should().BeTrue();
                verifierSettings.AllowMultipleTimestamps.Should().BeTrue();
                verifierSettings.AllowNoTimestamp.Should().BeTrue();
                verifierSettings.AllowUnknownRevocation.Should().BeTrue();
                verifierSettings.ReportUnknownRevocation.Should().BeFalse();
                verifierSettings.VerificationTarget.Should().Be(VerificationTarget.All);
                verifierSettings.SignaturePlacement.Should().Be(SignaturePlacement.Any);
                verifierSettings.RepositoryCountersignatureVerificationBehavior.Should().Be(SignatureVerificationBehavior.IfExistsAndIsNecessary);
                verifierSettings.RevocationMode.Should().Be(RevocationMode.Online);
            }
        }

        [Fact]
        public void GetClientPolicy_AcceptMode_ReadsClientPolicyCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <config>
        <add key=""signatureValidationMode"" value=""accept"" />
    </config>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var clientPolicyContext = ClientPolicyContext.GetClientPolicy(settings, NullLogger.Instance);

                clientPolicyContext.Policy.Should().Be(SignatureValidationMode.Accept);
                clientPolicyContext.AllowList.Should().BeEmpty();

                var verifierSettings = clientPolicyContext.VerifierSettings;

                verifierSettings.AllowUnsigned.Should().BeTrue();
                verifierSettings.AllowIllegal.Should().BeTrue();
                verifierSettings.AllowUntrusted.Should().BeTrue();
                verifierSettings.AllowIgnoreTimestamp.Should().BeTrue();
                verifierSettings.AllowMultipleTimestamps.Should().BeTrue();
                verifierSettings.AllowNoTimestamp.Should().BeTrue();
                verifierSettings.AllowUnknownRevocation.Should().BeTrue();
                verifierSettings.ReportUnknownRevocation.Should().BeFalse();
                verifierSettings.VerificationTarget.Should().Be(VerificationTarget.All);
                verifierSettings.SignaturePlacement.Should().Be(SignaturePlacement.Any);
                verifierSettings.RepositoryCountersignatureVerificationBehavior.Should().Be(SignatureVerificationBehavior.IfExistsAndIsNecessary);
                verifierSettings.RevocationMode.Should().Be(RevocationMode.Online);
            }
        }

        [Fact]
        public void GetClientPolicy_RequireMode_ReadsClientPolicyCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
    <config>
        <add key=""signatureValidationMode"" value=""require"" />
    </config>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settings = new Settings(mockBaseDirectory);
                settings.Should().NotBeNull();

                var clientPolicyContext = ClientPolicyContext.GetClientPolicy(settings, NullLogger.Instance);

                clientPolicyContext.Policy.Should().Be(SignatureValidationMode.Require);
                clientPolicyContext.AllowList.Should().BeEmpty();

                var verifierSettings = clientPolicyContext.VerifierSettings;

                verifierSettings.AllowUnsigned.Should().BeFalse();
                verifierSettings.AllowIllegal.Should().BeFalse();
                verifierSettings.AllowUntrusted.Should().BeFalse();
                verifierSettings.AllowIgnoreTimestamp.Should().BeTrue();
                verifierSettings.AllowMultipleTimestamps.Should().BeTrue();
                verifierSettings.AllowNoTimestamp.Should().BeTrue();
                verifierSettings.AllowUnknownRevocation.Should().BeTrue();
                verifierSettings.ReportUnknownRevocation.Should().BeTrue();
                verifierSettings.VerificationTarget.Should().Be(VerificationTarget.All);
                verifierSettings.SignaturePlacement.Should().Be(SignaturePlacement.Any);
                verifierSettings.RepositoryCountersignatureVerificationBehavior.Should().Be(SignatureVerificationBehavior.IfExistsAndIsNecessary);
                verifierSettings.RevocationMode.Should().Be(RevocationMode.Online);
            }
        }

        [Fact]
        public void GetClientPolicy_ReadsAndParsesTrustedSigners()
        {
            // Arrange
            var config = @"
<configuration>
    <config>
        <add key=""signatureValidationMode"" value=""require"" />
    </config>
    <trustedSigners>
        <author name=""author1"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <repository name=""repository1"" serviceIndex=""https://v3serviceIndex.test/api/json"">
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""true"" />
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

                var expectedAllowList = new List<VerificationAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Author, SignaturePlacement.PrimarySignature, "abc", HashAlgorithmName.SHA256),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any,"def", HashAlgorithmName.SHA256, allowUntrustedRoot: true)
                };

                var clientPolicyContext = ClientPolicyContext.GetClientPolicy(settings, NullLogger.Instance);

                clientPolicyContext.Policy.Should().Be(SignatureValidationMode.Require);
                clientPolicyContext.AllowList.Count.Should().Be(2);
                clientPolicyContext.AllowList.Should().BeEquivalentTo(expectedAllowList);

                var verifierSettings = clientPolicyContext.VerifierSettings;

                verifierSettings.AllowUnsigned.Should().BeFalse();
                verifierSettings.AllowIllegal.Should().BeFalse();
                verifierSettings.AllowUntrusted.Should().BeFalse();
                verifierSettings.AllowIgnoreTimestamp.Should().BeTrue();
                verifierSettings.AllowMultipleTimestamps.Should().BeTrue();
                verifierSettings.AllowNoTimestamp.Should().BeTrue();
                verifierSettings.AllowUnknownRevocation.Should().BeTrue();
                verifierSettings.ReportUnknownRevocation.Should().BeTrue();
                verifierSettings.VerificationTarget.Should().Be(VerificationTarget.All);
                verifierSettings.SignaturePlacement.Should().Be(SignaturePlacement.Any);
                verifierSettings.RepositoryCountersignatureVerificationBehavior.Should().Be(SignatureVerificationBehavior.IfExistsAndIsNecessary);
                verifierSettings.RevocationMode.Should().Be(RevocationMode.Online);
            }
        }

        [Fact]
        public void GetClientPolicy_MutipleConfigs_ReadsParsesAndMergesTrustedSigners()
        {
            // Arrange
            var config1 = @"
<configuration>
    <config>
        <add key=""signatureValidationMode"" value=""require"" />
    </config>
    <trustedSigners>
        <author name=""author1"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <repository name=""repository1"" serviceIndex=""https://v3serviceIndex.test/api/json"">
            <certificate fingerprint=""def"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </repository>
    </trustedSigners>
</configuration>";

            var config2 = @"
<configuration>
    <config>
        <add key=""signatureValidationMode"" value=""require"" />
    </config>
    <trustedSigners>
        <author name=""author1"">
            <certificate fingerprint=""xyz"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <repository name=""repository2"" serviceIndex=""https://v3serviceIndex2.test/api/json"">
            <certificate fingerprint=""rst"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </repository>
    </trustedSigners>
</configuration>";

            var config3 = @"
<configuration>
    <config>
        <add key=""signatureValidationMode"" value=""require"" />
    </config>
    <trustedSigners>
        <author name=""author2"">
            <certificate fingerprint=""xyz"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
        <repository name=""repository1"" serviceIndex=""https://v3serviceIndex.test/api/json"">
            <certificate fingerprint=""opq"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </repository>
    </trustedSigners>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            using (var innerMockDirectory = TestDirectory.Create(mockBaseDirectory))
            using (var closerDirectory = TestDirectory.Create(innerMockDirectory))
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config1);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, innerMockDirectory, config2);
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, closerDirectory, config3);

                // Act and Assert
                var settings = Settings.LoadDefaultSettings(closerDirectory);
                settings.Should().NotBeNull();

                var expectedAllowList = new List<VerificationAllowListEntry>()
                {
                    new TrustedSignerAllowListEntry(VerificationTarget.Author, SignaturePlacement.PrimarySignature, "xyz", HashAlgorithmName.SHA256),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any,"rst", HashAlgorithmName.SHA256),
                    new TrustedSignerAllowListEntry(VerificationTarget.Repository, SignaturePlacement.Any,"opq", HashAlgorithmName.SHA256)
                };

                var clientPolicyContext = ClientPolicyContext.GetClientPolicy(settings, NullLogger.Instance);

                clientPolicyContext.Policy.Should().Be(SignatureValidationMode.Require);
                clientPolicyContext.AllowList.Count.Should().Be(3);
                clientPolicyContext.AllowList.Should().BeEquivalentTo(expectedAllowList);

                var verifierSettings = clientPolicyContext.VerifierSettings;

                verifierSettings.AllowUnsigned.Should().BeFalse();
                verifierSettings.AllowIllegal.Should().BeFalse();
                verifierSettings.AllowUntrusted.Should().BeFalse();
                verifierSettings.AllowIgnoreTimestamp.Should().BeTrue();
                verifierSettings.AllowMultipleTimestamps.Should().BeTrue();
                verifierSettings.AllowNoTimestamp.Should().BeTrue();
                verifierSettings.AllowUnknownRevocation.Should().BeTrue();
                verifierSettings.ReportUnknownRevocation.Should().BeTrue();
                verifierSettings.VerificationTarget.Should().Be(VerificationTarget.All);
                verifierSettings.SignaturePlacement.Should().Be(SignaturePlacement.Any);
                verifierSettings.RepositoryCountersignatureVerificationBehavior.Should().Be(SignatureVerificationBehavior.IfExistsAndIsNecessary);
                verifierSettings.RevocationMode.Should().Be(RevocationMode.Online);
            }
        }
    }
}
