// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using FluentAssertions;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Test.Utility;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class SignedPackageVerifierSettingsTests
    {
        const string RevocationModeEnvVar = "NUGET_CERT_REVOCATION_MODE";

        [Fact]
        public void Constructor_WhenVerificationTargetIsUnrecognized_Throws()
        {
            var verificationTarget = (VerificationTarget)int.MaxValue;
            var exception = Assert.Throws<ArgumentException>(() => new SignedPackageVerifierSettings(
                allowUnsigned: false,
                allowIllegal: false,
                allowUntrusted: false,
                allowIgnoreTimestamp: false,
                allowMultipleTimestamps: false,
                allowNoTimestamp: false,
                allowUnknownRevocation: false,
                reportUnknownRevocation: true,
                verificationTarget: verificationTarget,
                signaturePlacement: SignaturePlacement.Any,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExists,
                revocationMode: RevocationMode.Online));

            Assert.Equal("verificationTarget", exception.ParamName);
            Assert.StartsWith($"The enum value '{verificationTarget}' is unrecognized.", exception.Message);
        }

        [Fact]
        public void Constructor_WhenSignaturePlacementIsUnrecognized_Throws()
        {
            var signaturePlacement = (SignaturePlacement)int.MaxValue;
            var exception = Assert.Throws<ArgumentException>(() => new SignedPackageVerifierSettings(
                allowUnsigned: false,
                allowIllegal: false,
                allowUntrusted: false,
                allowIgnoreTimestamp: false,
                allowMultipleTimestamps: false,
                allowNoTimestamp: false,
                allowUnknownRevocation: false,
                reportUnknownRevocation: true,
                verificationTarget: VerificationTarget.All,
                signaturePlacement: signaturePlacement,
                repositoryCountersignatureVerificationBehavior: SignatureVerificationBehavior.IfExists,
                revocationMode: RevocationMode.Online));

            Assert.Equal("signaturePlacement", exception.ParamName);
            Assert.StartsWith($"The enum value '{signaturePlacement}' is unrecognized.", exception.Message);
        }

        [Fact]
        public void Constructor_WhenRepositoryCountersignatureVerificationBehaviorIsUnrecognized_Throws()
        {
            var repositoryCountersignatureVerificationBehavior = (SignatureVerificationBehavior)int.MaxValue;
            var exception = Assert.Throws<ArgumentException>(() => new SignedPackageVerifierSettings(
                allowUnsigned: false,
                allowIllegal: false,
                allowUntrusted: false,
                allowIgnoreTimestamp: false,
                allowMultipleTimestamps: false,
                allowNoTimestamp: false,
                allowUnknownRevocation: false,
                reportUnknownRevocation: true,
                verificationTarget: VerificationTarget.All,
                signaturePlacement: SignaturePlacement.Any,
                repositoryCountersignatureVerificationBehavior: repositoryCountersignatureVerificationBehavior,
                revocationMode: RevocationMode.Online));

            Assert.Equal("repositoryCountersignatureVerificationBehavior", exception.ParamName);
            Assert.StartsWith($"The enum value '{repositoryCountersignatureVerificationBehavior}' is unrecognized.", exception.Message);
        }

        [Theory]
        [InlineData(
            VerificationTarget.Author,
            SignaturePlacement.Countersignature,
            SignatureVerificationBehavior.IfExists,
            "verificationTarget",
            "signaturePlacement")]
        [InlineData(
            VerificationTarget.All,
            SignaturePlacement.Countersignature,
            SignatureVerificationBehavior.IfExists,
            "verificationTarget",
            "signaturePlacement")]
        [InlineData(
            VerificationTarget.All,
            SignaturePlacement.Any,
            SignatureVerificationBehavior.Never,
            "signaturePlacement",
            "repositoryCountersignatureVerificationBehavior")]
        [InlineData(
            VerificationTarget.All,
            SignaturePlacement.PrimarySignature,
            SignatureVerificationBehavior.IfExists,
            "signaturePlacement",
            "repositoryCountersignatureVerificationBehavior")]
        public void Constructor_WhenArgumentCombinationIsInvalid_Throws(
            VerificationTarget verificationTarget,
            SignaturePlacement signaturePlacement,
            SignatureVerificationBehavior repositoryCountersignatureVerificationBehavior,
            string parameterName1,
            string parameterName2)
        {
            var exception = Assert.Throws<ArgumentException>(() => new SignedPackageVerifierSettings(
                allowUnsigned: false,
                allowIllegal: false,
                allowUntrusted: false,
                allowIgnoreTimestamp: false,
                allowMultipleTimestamps: false,
                allowNoTimestamp: false,
                allowUnknownRevocation: false,
                reportUnknownRevocation: true,
                verificationTarget: verificationTarget,
                signaturePlacement: signaturePlacement,
                repositoryCountersignatureVerificationBehavior: repositoryCountersignatureVerificationBehavior,
                revocationMode: RevocationMode.Online));

            Assert.Equal(parameterName2, exception.ParamName);
            Assert.StartsWith($"Invalid combination of arguments {parameterName1} and {parameterName2}.", exception.Message);
        }

        [Theory]
        [InlineData(true, VerificationTarget.Author, SignaturePlacement.PrimarySignature, SignatureVerificationBehavior.Never, RevocationMode.Online)]
        [InlineData(false, VerificationTarget.All, SignaturePlacement.Any, SignatureVerificationBehavior.IfExistsAndIsNecessary, RevocationMode.Offline)]
        public void Constructor_InitializesProperties(
            bool boolValue,
            VerificationTarget verificationTarget,
            SignaturePlacement signaturePlacement,
            SignatureVerificationBehavior signatureVerificationBehavior,
            RevocationMode revocationMode)
        {
            // Arrange & Act
            var settings = new SignedPackageVerifierSettings(
                allowUnsigned: boolValue,
                allowIllegal: boolValue,
                allowUntrusted: boolValue,
                allowIgnoreTimestamp: boolValue,
                allowMultipleTimestamps: boolValue,
                allowNoTimestamp: boolValue,
                allowUnknownRevocation: boolValue,
                reportUnknownRevocation: boolValue,
                verificationTarget: verificationTarget,
                signaturePlacement: signaturePlacement,
                repositoryCountersignatureVerificationBehavior: signatureVerificationBehavior,
                revocationMode: revocationMode);

            // Assert
            settings.AllowUnsigned.Should().Be(boolValue);
            settings.AllowIllegal.Should().Be(boolValue);
            settings.AllowUntrusted.Should().Be(boolValue);
            settings.AllowIgnoreTimestamp.Should().Be(boolValue);
            settings.AllowMultipleTimestamps.Should().Be(boolValue);
            settings.AllowNoTimestamp.Should().Be(boolValue);
            settings.AllowUnknownRevocation.Should().Be(boolValue);
            settings.ReportUnknownRevocation.Should().Be(boolValue);
            settings.RepositoryCountersignatureVerificationBehavior.Should().Be(signatureVerificationBehavior);
            settings.RevocationMode.Should().Be(revocationMode);
        }

        [Theory]
        [InlineData(null, RevocationMode.Online)]
        [InlineData("", RevocationMode.Online)]
        [InlineData("online", RevocationMode.Online)]
        [InlineData("offline", RevocationMode.Offline)]
        [InlineData("Offline", RevocationMode.Offline)]
        public void GetDefault_InitializesProperties(string revocationModeEnvVar, RevocationMode expectedRevocationMode)
        {
            // Arrange
            var defaultValue = true;
            IEnvironmentVariableReader environmentVariableReader = GetEnvironmentVariableReader(revocationModeEnvVar);

            // Act
            var settings = SignedPackageVerifierSettings.GetDefault(environmentVariableReader);

            // Assert
            settings.AllowUnsigned.Should().Be(defaultValue);
            settings.AllowIllegal.Should().Be(defaultValue);
            settings.AllowUntrusted.Should().Be(defaultValue);
            settings.AllowIgnoreTimestamp.Should().Be(defaultValue);
            settings.AllowMultipleTimestamps.Should().Be(defaultValue);
            settings.AllowNoTimestamp.Should().Be(defaultValue);
            settings.AllowUnknownRevocation.Should().Be(defaultValue);
            settings.ReportUnknownRevocation.Should().Be(false);
            settings.VerificationTarget.Should().Be(VerificationTarget.All);
            settings.SignaturePlacement.Should().Be(SignaturePlacement.Any);
            settings.RepositoryCountersignatureVerificationBehavior.Should().Be(SignatureVerificationBehavior.IfExistsAndIsNecessary);
            settings.RevocationMode.Should().Be(expectedRevocationMode);
        }

        [Theory]
        [InlineData(null, RevocationMode.Online)]
        [InlineData("", RevocationMode.Online)]
        [InlineData("online", RevocationMode.Online)]
        [InlineData("offline", RevocationMode.Offline)]
        [InlineData("Offline", RevocationMode.Offline)]
        public void GetAcceptModeDefaultPolicy_InitializesProperties(string revocationModeEnvVar, RevocationMode expectedRevocationMode)
        {
            // Arrange
            var defaultValue = true;
            IEnvironmentVariableReader environmentVariableReader = GetEnvironmentVariableReader(revocationModeEnvVar);

            // Act
            var settings = SignedPackageVerifierSettings.GetAcceptModeDefaultPolicy(environmentVariableReader);

            // Assert
            settings.AllowUnsigned.Should().Be(defaultValue);
            settings.AllowIllegal.Should().Be(defaultValue);
            settings.AllowUntrusted.Should().Be(defaultValue);
            settings.AllowIgnoreTimestamp.Should().Be(defaultValue);
            settings.AllowMultipleTimestamps.Should().Be(defaultValue);
            settings.AllowNoTimestamp.Should().Be(defaultValue);
            settings.AllowUnknownRevocation.Should().Be(defaultValue);
            settings.ReportUnknownRevocation.Should().Be(false);
            settings.VerificationTarget.Should().Be(VerificationTarget.All);
            settings.SignaturePlacement.Should().Be(SignaturePlacement.Any);
            settings.RepositoryCountersignatureVerificationBehavior.Should().Be(SignatureVerificationBehavior.IfExistsAndIsNecessary);
            settings.RevocationMode.Should().Be(expectedRevocationMode);
        }

        [Theory]
        [InlineData(null, RevocationMode.Online)]
        [InlineData("", RevocationMode.Online)]
        [InlineData("online", RevocationMode.Online)]
        [InlineData("offline", RevocationMode.Offline)]
        [InlineData("Offline", RevocationMode.Offline)]
        public void GetRequireModeDefaultPolicy_InitializesProperties(string revocationModeEnvVar, RevocationMode expectedRevocationMode)
        {
            // Arrange
            IEnvironmentVariableReader environmentVariableReader = GetEnvironmentVariableReader(revocationModeEnvVar);

            // Act
            var settings = SignedPackageVerifierSettings.GetRequireModeDefaultPolicy(environmentVariableReader);

            // Assert
            settings.AllowUnsigned.Should().Be(false);
            settings.AllowIllegal.Should().Be(false);
            settings.AllowUntrusted.Should().Be(false);
            settings.AllowIgnoreTimestamp.Should().Be(true);
            settings.AllowMultipleTimestamps.Should().Be(true);
            settings.AllowNoTimestamp.Should().Be(true);
            settings.AllowUnknownRevocation.Should().Be(true);
            settings.ReportUnknownRevocation.Should().Be(true);
            settings.VerificationTarget.Should().Be(VerificationTarget.All);
            settings.SignaturePlacement.Should().Be(SignaturePlacement.Any);
            settings.RepositoryCountersignatureVerificationBehavior.Should().Be(SignatureVerificationBehavior.IfExistsAndIsNecessary);
            settings.RevocationMode.Should().Be(expectedRevocationMode);
        }

        [Theory]
        [InlineData(null, RevocationMode.Online)]
        [InlineData("", RevocationMode.Online)]
        [InlineData("online", RevocationMode.Online)]
        [InlineData("offline", RevocationMode.Offline)]
        [InlineData("Offline", RevocationMode.Offline)]
        public void GetVerifyCommandDefaultPolicy_InitializesProperties(string revocationModeEnvVar, RevocationMode expectedRevocationMode)
        {
            // Arrange
            IEnvironmentVariableReader environmentVariableReader = GetEnvironmentVariableReader(revocationModeEnvVar);

            // Act
            var settings = SignedPackageVerifierSettings.GetVerifyCommandDefaultPolicy(environmentVariableReader);

            // Assert
            settings.AllowUnsigned.Should().Be(false);
            settings.AllowIllegal.Should().Be(false);
            settings.AllowUntrusted.Should().Be(false);
            settings.AllowIgnoreTimestamp.Should().Be(false);
            settings.AllowMultipleTimestamps.Should().Be(true);
            settings.AllowNoTimestamp.Should().Be(true);
            settings.AllowUnknownRevocation.Should().Be(true);
            settings.ReportUnknownRevocation.Should().Be(true);
            settings.VerificationTarget.Should().Be(VerificationTarget.All);
            settings.SignaturePlacement.Should().Be(SignaturePlacement.Any);
            settings.RepositoryCountersignatureVerificationBehavior.Should().Be(SignatureVerificationBehavior.IfExists);
            settings.RevocationMode.Should().Be(expectedRevocationMode);
        }

        private static IEnvironmentVariableReader GetEnvironmentVariableReader(string revocationModeEnvVar)
        {
            if (revocationModeEnvVar == null)
            {
                return TestEnvironmentVariableReader.EmptyInstance;
            }

            return new TestEnvironmentVariableReader(new Dictionary<string, string>() { { RevocationModeEnvVar, revocationModeEnvVar } });
        }
    }
}
