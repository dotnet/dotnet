// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Test.Utility;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.CommandLine.FuncTest.Commands
{
    /// <summary>
    /// Tests install command
    /// These tests require admin privilege as the certs need to be added to the root store location
    /// </summary>
    [Collection(SignCommandTestCollection.Name)]
    public class InstallCommandTests
    {
        private static readonly string _NU3008Message = "The package integrity check failed. The package has changed since it was signed. Try clearing the local http-cache and run nuget operation again.";
        private static readonly string _NU3008 = "NU3008: {0}";
        private static readonly string _NU3027Message = "The signature should be timestamped to enable long-term signature validity after the certificate has expired.";
        private static readonly string _NU3027 = "NU3027: {0}";
        private static readonly string _NU3012Message = "The author primary signature found a chain building issue: Revoked: The certificate is revoked.";
        private static readonly string _NU3012 = "NU3012: {0}";
        private static readonly string _NU3018Message = "The author primary signature's signing certificate is not trusted by the trust provider.";
        private static readonly string _NU3018 = "NU3018: {0}";

        private SignCommandTestFixture _testFixture;
        private TrustedTestCert<TestCertificate> _trustedTestCert;
        private string _nugetExePath;

        public InstallCommandTests(SignCommandTestFixture fixture)
        {
            _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _trustedTestCert = SigningTestUtility.GenerateTrustedTestCertificate();
            _nugetExePath = _testFixture.NuGetExePath;
        }

        [CIOnlyFact]
        public async Task Install_AuthorSignedPackage_SucceedsAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext("A", "1.0.0");

            using (var context = new SimpleTestPathContext())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, context.WorkingDirectory);

                var args = new string[]
                {
                    nupkg.Id,
                    "-Version",
                    nupkg.Version,
                    "-DirectDownload",
                    "-NoCache",
                    "-Source",
                    context.WorkingDirectory,
                    "-OutputDirectory",
                    Path.Combine(context.WorkingDirectory, "packages")
                };

                // Act
                var result = RunInstall(_nugetExePath, context, expectedExitCode: 0, additionalArgs: args);

                // Assert
                result.ExitCode.Should().Be(0);
                result.AllOutput.Should().Contain($"WARNING: {string.Format(_NU3027, SigningTestUtility.AddSignatureLogPrefix(_NU3027Message, nupkg.Identity, context.WorkingDirectory))}");
            }
        }

        [CIOnlyFact]
        public async Task Install_RepoSignedPackage_SucceedsAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext("A", "1.0.0");

            using (var context = new SimpleTestPathContext())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                await SignedArchiveTestUtility.RepositorySignPackageAsync(testCertificate, nupkg, context.WorkingDirectory, new Uri("https://v3serviceIndex.test/api/index.json"));

                var args = new string[]
                {
                    nupkg.Id,
                    "-Version",
                    nupkg.Version,
                    "-DirectDownload",
                    "-NoCache",
                    "-Source",
                    context.WorkingDirectory,
                    "-OutputDirectory",
                    Path.Combine(context.WorkingDirectory, "packages")
                };

                // Act
                var result = RunInstall(_nugetExePath, context, expectedExitCode: 0, additionalArgs: args);

                // Assert
                result.ExitCode.Should().Be(0);
                result.AllOutput.Should().Contain($"WARNING: {string.Format(_NU3027, SigningTestUtility.AddSignatureLogPrefix(_NU3027Message, nupkg.Identity, context.WorkingDirectory))}");
            }
        }

        [CIOnlyFact]
        public async Task Install_UntrustedCertSignedPackage_WarnsAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext("A", "1.0.0");

            using (var context = new SimpleTestPathContext())
            using (var testCertificate = new X509Certificate2(_testFixture.UntrustedSelfIssuedCertificateInCertificateStore))
            {
                await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, context.WorkingDirectory);

                var args = new string[]
                {
                    nupkg.Id,
                    "-Version",
                    nupkg.Version,
                    "-DirectDownload",
                    "-NoCache",
                    "-Source",
                    context.WorkingDirectory,
                    "-OutputDirectory",
                    Path.Combine(context.WorkingDirectory, "packages")
                };

                // Act
                var result = RunInstall(_nugetExePath, context, expectedExitCode: 0, additionalArgs: args);

                // Assert
                result.ExitCode.Should().Be(0);
                result.AllOutput.Should().Contain($"WARNING: {string.Format(_NU3018, SigningTestUtility.AddSignatureLogPrefix(_NU3018Message, nupkg.Identity, context.WorkingDirectory))}");
                result.AllOutput.Should().Contain($"WARNING: {string.Format(_NU3027, SigningTestUtility.AddSignatureLogPrefix(_NU3027Message, nupkg.Identity, context.WorkingDirectory))}");
            }
        }

        [CIOnlyFact]
        public async Task Install_TamperedPackage_FailsAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext("A", "1.0.0");

            using (var context = new SimpleTestPathContext())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, context.WorkingDirectory);
                SignedArchiveTestUtility.TamperWithPackage(signedPackagePath);

                var args = new string[]
                {
                    nupkg.Id,
                    "-Version",
                    nupkg.Version,
                    "-DirectDownload",
                    "-NoCache",
                    "-Source",
                    context.WorkingDirectory,
                    "-OutputDirectory",
                    Path.Combine(context.WorkingDirectory, "packages")
                };

                // Act
                var result = RunInstall(_nugetExePath, context, expectedExitCode: 1, additionalArgs: args);

                // Assert
                result.ExitCode.Should().Be(1);
                result.Errors.Should().Contain(string.Format(_NU3008, SigningTestUtility.AddSignatureLogPrefix(_NU3008Message, nupkg.Identity, context.WorkingDirectory)));
                result.AllOutput.Should().Contain($"WARNING: {string.Format(_NU3027, SigningTestUtility.AddSignatureLogPrefix(_NU3027Message, nupkg.Identity, context.WorkingDirectory))}");
            }
        }

        [CIOnlyFact]
        public async Task Install_TamperedAndRevokedCertificateSignaturePackage_FailsAsync()
        {
            // Arrange
            var nupkg = new SimpleTestPackageContext("A", "1.0.0");
            var testServer = await _testFixture.GetSigningTestServerAsync();
            var certificateAuthority = await _testFixture.GetDefaultTrustedCertificateAuthorityAsync();
            var issueOptions = IssueCertificateOptions.CreateDefaultForEndCertificate();
            var bcCertificate = certificateAuthority.IssueCertificate(issueOptions);

            using (var context = new SimpleTestPathContext())
            using (var testCertificate = new X509Certificate2(bcCertificate.GetEncoded()))
            {
                testCertificate.PrivateKey = DotNetUtilities.ToRSA(issueOptions.KeyPair.Private as RsaPrivateCrtKeyParameters);

                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, nupkg, context.WorkingDirectory);
                SignedArchiveTestUtility.TamperWithPackage(signedPackagePath);

                await certificateAuthority.OcspResponder.WaitForResponseExpirationAsync(bcCertificate);
                certificateAuthority.Revoke(
                    bcCertificate,
                    RevocationReason.KeyCompromise,
                    DateTimeOffset.UtcNow.AddSeconds(-1));

                var args = new string[]
                {
                    nupkg.Id,
                    "-Version",
                    nupkg.Version,
                    "-DirectDownload",
                    "-NoCache",
                    "-Source",
                    context.WorkingDirectory,
                    "-OutputDirectory",
                    Path.Combine(context.WorkingDirectory, "packages")
                };

                // Act
                var result = RunInstall(_nugetExePath, context, expectedExitCode: 1, additionalArgs: args);

                // Assert
                result.ExitCode.Should().Be(1);
                result.Errors.Should().Contain(string.Format(_NU3008, SigningTestUtility.AddSignatureLogPrefix(_NU3008Message, nupkg.Identity, context.WorkingDirectory)));
                result.Errors.Should().Contain(string.Format(_NU3012, SigningTestUtility.AddSignatureLogPrefix(_NU3012Message, nupkg.Identity, context.WorkingDirectory)));
                result.AllOutput.Should().Contain($"WARNING: {string.Format(_NU3027, SigningTestUtility.AddSignatureLogPrefix(_NU3027Message, nupkg.Identity, context.WorkingDirectory))}");
            }
        }

        public static CommandRunnerResult RunInstall(string nugetExe, SimpleTestPathContext pathContext, int expectedExitCode = 0, params string[] additionalArgs)
        {
            // Store the dg file for debugging
            var envVars = new Dictionary<string, string>()
            {
                { "NUGET_HTTP_CACHE_PATH", pathContext.HttpCacheFolder }
            };

            var args = new string[] {
                    "install",
                    "-Verbosity",
                    "detailed"
                };

            args = args.Concat(additionalArgs).ToArray();

            // Act
            var r = CommandRunner.Run(
                nugetExe,
                pathContext.WorkingDirectory,
                string.Join(" ", args),
                environmentVariables: envVars);

            // Assert
            Assert.True(expectedExitCode == r.ExitCode, r.AllOutput);

            return r;
        }
    }
}
