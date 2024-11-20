// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#if IS_SIGNING_SUPPORTED

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Internal.NuGet.Testing.SignedPackages;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Configuration.Test;
using NuGet.Packaging.Signing;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.FuncTest
{
    [Collection(SigningTestCollection.Name)]
    public class ClientPolicyTests : IDisposable
    {
        private readonly SigningTestFixture _testFixture;
        private readonly TrustedTestCert<TestCertificate> _trustedRepoTestCert;

        public ClientPolicyTests(SigningTestFixture fixture)
        {
            _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _trustedRepoTestCert = SigningTestUtility.GenerateTrustedTestCertificate();
        }

        public void Dispose()
        {
            _trustedRepoTestCert.Dispose();
        }

        [CIOnlyTheory]
        [InlineData(SigningTestType.Author, "accept", true, 0)]
        [InlineData(SigningTestType.Author, "require", false, 1)]
        [InlineData(SigningTestType.RepositoryPrimary, "accept", true, 0)]
        [InlineData(SigningTestType.RepositoryPrimary, "require", false, 1)]
        [InlineData(SigningTestType.RepositoryCountersigned, "accept", true, 0)]
        [InlineData(SigningTestType.RepositoryCountersigned, "require", false, 1)]
        public async Task ClientPolicies_WithNoTrustedSignersListAsync(SigningTestType signature, string validationMode, bool expectedResult, int expectedErrors)
        {
            // Arrange
            using (var dir = TestDirectory.Create())
            using (var authorCertificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
            using (var repoCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var authorCertificateFingerprintString = SignatureTestUtility.GetFingerprint(authorCertificate, HashAlgorithmName.SHA256);
                var repoCertificateFingerprintString = SignatureTestUtility.GetFingerprint(repoCertificate, HashAlgorithmName.SHA256);
                var signedPackagePath = await CreateSignedPackageAsync(dir, signature, authorCertificate, repoCertificate);

                var config = $@"
<configuration>
    <config>
        <add key=""signatureValidationMode"" value=""{validationMode}"" />
    </config>
</configuration>";

                var nugetConfigPath = "NuGet.Config";
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, dir, config);

                // Act and Assert
                var settings = new Settings(dir);

                var clientPolicyContext = ClientPolicyContext.GetClientPolicy(settings, NullLogger.Instance);
                var trustProviders = new[]
                {
                    new AllowListVerificationProvider(clientPolicyContext.AllowList, requireNonEmptyAllowList: clientPolicyContext.Policy == SignatureValidationMode.Require)
                };
                var verifier = new PackageSignatureVerifier(trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, clientPolicyContext.VerifierSettings, CancellationToken.None);
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                    // Assert
                    result.IsValid.Should().Be(expectedResult);
                    totalWarningIssues.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(expectedErrors);
                }
            }
        }

        [CIOnlyTheory]
        [InlineData(SigningTestType.Author, "accept", true, 0, 1)]
        [InlineData(SigningTestType.Author, "require", false, 1, 0)]
        [InlineData(SigningTestType.RepositoryPrimary, "accept", true, 0, 1)]
        [InlineData(SigningTestType.RepositoryPrimary, "require", false, 1, 0)]
        [InlineData(SigningTestType.RepositoryCountersigned, "accept", true, 0, 1)]
        [InlineData(SigningTestType.RepositoryCountersigned, "require", false, 1, 0)]
        public async Task ClientPolicies_WithSignerNotInTrustedSignersListAsync(SigningTestType signature, string validationMode, bool expectedResult, int expectedErrors, int expectedWarnings)
        {
            // Arrange
            using (var dir = TestDirectory.Create())
            using (var authorCertificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
            using (var repoCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var authorCertificateFingerprintString = SignatureTestUtility.GetFingerprint(authorCertificate, HashAlgorithmName.SHA256);
                var repoCertificateFingerprintString = SignatureTestUtility.GetFingerprint(repoCertificate, HashAlgorithmName.SHA256);

                var signedPackagePath = await CreateSignedPackageAsync(dir, signature, authorCertificate, repoCertificate);

                var config = $@"
<configuration>
    <config>
        <add key=""signatureValidationMode"" value=""{validationMode}"" />
    </config>
    <trustedSigners>
        <author name=""randomAuthor"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
        </author>
    </trustedSigners>
</configuration>";

                var nugetConfigPath = "NuGet.Config";
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, dir, config);

                // Act and Assert
                var settings = new Settings(dir);

                var clientPolicyContext = ClientPolicyContext.GetClientPolicy(settings, NullLogger.Instance);
                var trustProviders = new[]
                {
                    new AllowListVerificationProvider(clientPolicyContext.AllowList, requireNonEmptyAllowList: clientPolicyContext.Policy == SignatureValidationMode.Require)
                };
                var verifier = new PackageSignatureVerifier(trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, clientPolicyContext.VerifierSettings, CancellationToken.None);
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                    // Assert
                    result.IsValid.Should().Be(expectedResult);
                    totalWarningIssues.Count().Should().Be(expectedWarnings);
                    totalErrorIssues.Count().Should().Be(expectedErrors);
                }
            }
        }

        [CIOnlyTheory]
        [InlineData(SigningTestType.Author, SignaturePlacement.PrimarySignature, "accept")]
        [InlineData(SigningTestType.Author, SignaturePlacement.PrimarySignature, "require")]
        [InlineData(SigningTestType.RepositoryPrimary, SignaturePlacement.PrimarySignature, "accept")]
        [InlineData(SigningTestType.RepositoryPrimary, SignaturePlacement.PrimarySignature, "require")]
        [InlineData(SigningTestType.RepositoryCountersigned, SignaturePlacement.PrimarySignature, "accept")]
        [InlineData(SigningTestType.RepositoryCountersigned, SignaturePlacement.PrimarySignature, "require")]
        [InlineData(SigningTestType.RepositoryCountersigned, SignaturePlacement.Countersignature, "accept")]
        [InlineData(SigningTestType.RepositoryCountersigned, SignaturePlacement.Countersignature, "require")]
        public async Task ClientPolicies_WithSignerInTrustedSignersListAsync(SigningTestType signature, SignaturePlacement trustedSigner, string validationMode)
        {
            // Arrange
            using (var dir = TestDirectory.Create())
            using (var authorCertificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
            using (var repoCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var authorCertificateFingerprintString = SignatureTestUtility.GetFingerprint(authorCertificate, HashAlgorithmName.SHA256);
                var repoCertificateFingerprintString = SignatureTestUtility.GetFingerprint(repoCertificate, HashAlgorithmName.SHA256);

                var signedPackagePath = await CreateSignedPackageAsync(dir, signature, authorCertificate, repoCertificate);
                var trustedSignerString = "";

                if (signature == SigningTestType.Author || (signature == SigningTestType.RepositoryCountersigned && trustedSigner == SignaturePlacement.PrimarySignature))
                {
                    trustedSignerString = $@"<author name=""author1""><certificate fingerprint=""{authorCertificateFingerprintString}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" /></author>";
                }
                else
                {
                    trustedSignerString = $@"<repository name=""repo1"" serviceIndex=""https://api.v3serviceIndex.test/json""><certificate fingerprint=""{repoCertificateFingerprintString}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" /></repository>";
                }

                var config = $@"
<configuration>
    <config>
        <add key=""signatureValidationMode"" value=""{validationMode}"" />
    </config>
    <trustedSigners>
        {trustedSignerString}
    </trustedSigners>
</configuration>";

                var nugetConfigPath = "NuGet.Config";
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, dir, config);

                // Act and Assert
                var settings = new Settings(dir);

                var clientPolicyContext = ClientPolicyContext.GetClientPolicy(settings, NullLogger.Instance);
                var trustProviders = new[]
                {
                    new AllowListVerificationProvider(clientPolicyContext.AllowList, requireNonEmptyAllowList: clientPolicyContext.Policy == SignatureValidationMode.Require)
                };
                var verifier = new PackageSignatureVerifier(trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, clientPolicyContext.VerifierSettings, CancellationToken.None);
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                    // Assert
                    result.IsValid.Should().BeTrue();
                    totalWarningIssues.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(0);
                }
            }
        }

        [CIOnlyTheory]
        [InlineData(SigningTestType.RepositoryPrimary, "accept")]
        [InlineData(SigningTestType.RepositoryPrimary, "require")]
        [InlineData(SigningTestType.RepositoryCountersigned, "accept")]
        [InlineData(SigningTestType.RepositoryCountersigned, "require")]
        public async Task ClientPolicies_WithSignerInTrustedSignersList_WithMatchingOwnersAsync(SigningTestType signature, string validationMode)
        {
            // Arrange
            using (var dir = TestDirectory.Create())
            using (var authorCertificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
            using (var repoCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var authorCertificateFingerprintString = SignatureTestUtility.GetFingerprint(authorCertificate, HashAlgorithmName.SHA256);
                var repoCertificateFingerprintString = SignatureTestUtility.GetFingerprint(repoCertificate, HashAlgorithmName.SHA256);

                var signedPackagePath = await CreateSignedPackageAsync(dir, signature, authorCertificate, repoCertificate);

                var config = $@"
<configuration>
    <config>
        <add key=""signatureValidationMode"" value=""{validationMode}"" />
    </config>
    <trustedSigners>
        <repository name=""repo1"" serviceIndex=""https://api.v3serviceIndex.test/json"">
            <certificate fingerprint=""{repoCertificateFingerprintString}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <owners>owner1</owners>
        </repository>
    </trustedSigners>
</configuration>";

                var nugetConfigPath = "NuGet.Config";
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, dir, config);

                // Act and Assert
                var settings = new Settings(dir);

                var clientPolicyContext = ClientPolicyContext.GetClientPolicy(settings, NullLogger.Instance);
                var trustProviders = new[]
                {
                    new AllowListVerificationProvider(clientPolicyContext.AllowList, requireNonEmptyAllowList: clientPolicyContext.Policy == SignatureValidationMode.Require)
                };
                var verifier = new PackageSignatureVerifier(trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, clientPolicyContext.VerifierSettings, CancellationToken.None);
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                    // Assert
                    result.IsValid.Should().BeTrue();
                    totalWarningIssues.Count().Should().Be(0);
                    totalErrorIssues.Count().Should().Be(0);
                }
            }
        }

        [CIOnlyTheory]
        [InlineData(SigningTestType.RepositoryPrimary, "accept", true, 0, 1)]
        [InlineData(SigningTestType.RepositoryPrimary, "require", false, 1, 0)]
        [InlineData(SigningTestType.RepositoryCountersigned, "accept", true, 0, 1)]
        [InlineData(SigningTestType.RepositoryCountersigned, "require", false, 1, 0)]
        public async Task ClientPolicies_WithSignerInTrustedSignersList_WithoutMatchingOwnersAsync(SigningTestType signature, string validationMode, bool expectedResult, int expectedErrors, int expectedWarnings)
        {
            // Arrange
            using (var dir = TestDirectory.Create())
            using (var authorCertificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
            using (var repoCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var authorCertificateFingerprintString = SignatureTestUtility.GetFingerprint(authorCertificate, HashAlgorithmName.SHA256);
                var repoCertificateFingerprintString = SignatureTestUtility.GetFingerprint(repoCertificate, HashAlgorithmName.SHA256);

                var signedPackagePath = await CreateSignedPackageAsync(dir, signature, authorCertificate, repoCertificate);

                var config = $@"
<configuration>
    <config>
        <add key=""signatureValidationMode"" value=""{validationMode}"" />
    </config>
    <trustedSigners>
        <repository name=""repo1"" serviceIndex=""https://api.v3serviceIndex.test/json"">
            <certificate fingerprint=""{repoCertificateFingerprintString}"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <owners>owner7</owners>
        </repository>
    </trustedSigners>
</configuration>";

                var nugetConfigPath = "NuGet.Config";
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, dir, config);

                // Act and Assert
                var settings = new Settings(dir);

                var clientPolicyContext = ClientPolicyContext.GetClientPolicy(settings, NullLogger.Instance);
                var trustProviders = new[]
                {
                    new AllowListVerificationProvider(clientPolicyContext.AllowList, requireNonEmptyAllowList: clientPolicyContext.Policy == SignatureValidationMode.Require)
                };
                var verifier = new PackageSignatureVerifier(trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, clientPolicyContext.VerifierSettings, CancellationToken.None);
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                    // Assert
                    result.IsValid.Should().Be(expectedResult);
                    totalWarningIssues.Count().Should().Be(expectedWarnings);
                    totalErrorIssues.Count().Should().Be(expectedErrors);
                }
            }
        }

        [CIOnlyTheory]
        [InlineData(SigningTestType.RepositoryPrimary, "accept", true, 0, 1)]
        [InlineData(SigningTestType.RepositoryPrimary, "require", false, 1, 0)]
        [InlineData(SigningTestType.RepositoryCountersigned, "accept", true, 0, 1)]
        [InlineData(SigningTestType.RepositoryCountersigned, "require", false, 1, 0)]
        public async Task ClientPolicies_WithoutSignerInTrustedSignersList_WithMatchingOwnersAsync(SigningTestType signature, string validationMode, bool expectedResult, int expectedErrors, int expectedWarnings)
        {
            // Arrange
            using (var dir = TestDirectory.Create())
            using (var authorCertificate = new X509Certificate2(_testFixture.TrustedTestCertificate.Source.Cert))
            using (var repoCertificate = new X509Certificate2(_trustedRepoTestCert.Source.Cert))
            {
                var authorCertificateFingerprintString = SignatureTestUtility.GetFingerprint(authorCertificate, HashAlgorithmName.SHA256);
                var repoCertificateFingerprintString = SignatureTestUtility.GetFingerprint(repoCertificate, HashAlgorithmName.SHA256);

                var signedPackagePath = await CreateSignedPackageAsync(dir, signature, authorCertificate, repoCertificate);

                var config = $@"
<configuration>
    <config>
        <add key=""signatureValidationMode"" value=""{validationMode}"" />
    </config>
    <trustedSigners>
        <repository name=""repo1"" serviceIndex=""https://api.v3serviceIndex.test/json"">
            <certificate fingerprint=""abc"" hashAlgorithm=""SHA256"" allowUntrustedRoot=""false"" />
            <owners>owner1</owners>
        </repository>
    </trustedSigners>
</configuration>";

                var nugetConfigPath = "NuGet.Config";
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, dir, config);

                // Act and Assert
                var settings = new Settings(dir);

                var clientPolicyContext = ClientPolicyContext.GetClientPolicy(settings, NullLogger.Instance);
                var trustProviders = new[]
                {
                    new AllowListVerificationProvider(clientPolicyContext.AllowList, requireNonEmptyAllowList: clientPolicyContext.Policy == SignatureValidationMode.Require)
                };
                var verifier = new PackageSignatureVerifier(trustProviders);

                using (var packageReader = new PackageArchiveReader(signedPackagePath))
                {
                    // Act
                    var result = await verifier.VerifySignaturesAsync(packageReader, clientPolicyContext.VerifierSettings, CancellationToken.None);
                    var resultsWithWarnings = result.Results.Where(r => r.GetWarningIssues().Any());
                    var resultsWithErrors = result.Results.Where(r => r.GetErrorIssues().Any());
                    var totalWarningIssues = resultsWithWarnings.SelectMany(r => r.GetWarningIssues());
                    var totalErrorIssues = resultsWithErrors.SelectMany(r => r.GetErrorIssues());

                    // Assert
                    result.IsValid.Should().Be(expectedResult);
                    totalWarningIssues.Count().Should().Be(expectedWarnings);
                    totalErrorIssues.Count().Should().Be(expectedErrors);
                }
            }
        }

        private async Task<string> CreateSignedPackageAsync(TestDirectory dir, SigningTestType signature, X509Certificate2 authorcert, X509Certificate2 repocert)
        {
            var nupkg = new SimpleTestPackageContext();

            var signedPackagePath = "";
            if (signature == SigningTestType.Author || signature == SigningTestType.RepositoryCountersigned)
            {
                signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(authorcert, nupkg, dir);
            }

            if (signature == SigningTestType.RepositoryPrimary)
            {
                signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(repocert, nupkg, dir, new Uri(@"https://api.v3serviceIndex.test/json"),
                    packageOwners: new List<string>() { "owner1", "owner2", "owner3" });
            }

            if (signature == SigningTestType.RepositoryCountersigned)
            {
                signedPackagePath = await SignedArchiveTestUtility.RepositorySignPackageAsync(repocert, signedPackagePath, dir, new Uri(@"https://api.v3serviceIndex.test/json"),
                    packageOwners: new List<string>() { "owner1", "owner2", "owner3" });
            }

            return signedPackagePath;
        }

        public enum SigningTestType
        {
            Author,
            RepositoryPrimary,
            RepositoryCountersigned
        }
    }
}

#endif
