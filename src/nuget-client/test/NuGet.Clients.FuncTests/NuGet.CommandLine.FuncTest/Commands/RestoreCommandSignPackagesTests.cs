// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging.Signing;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Test.Utility.Signing;
using Xunit;

namespace NuGet.CommandLine.FuncTest.Commands
{
    /// <summary>
    /// Tests restore command
    /// These tests require admin privilege as the certs need to be added to the root store location
    /// </summary>
    [Collection(SignCommandTestCollection.Name)]
    public class RestoreCommandSignPackagesTests
    {
        private static readonly string NU3008Message = "The package integrity check failed. The package has changed since it was signed. Try clearing the local http-cache and run nuget operation again.";
        private static readonly string NU3008 = "NU3008: {0}";
        private static readonly string NU3027Message = "The signature should be timestamped to enable long-term signature validity after the certificate has expired.";
        private static readonly string NU3027 = "NU3027: {0}";
        private static readonly string NU3005CompressedMessage = "The package signature file entry is invalid. The central directory header field 'compression method' has an invalid value (8).";
        private static readonly string NU3005 = "NU3005: {0}";

        private SignCommandTestFixture _testFixture;
        private TrustedTestCert<TestCertificate> _trustedTestCert;
        private readonly string _nugetExePath;

        public RestoreCommandSignPackagesTests(SignCommandTestFixture fixture)
        {
            _testFixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _trustedTestCert = SigningTestUtility.GenerateTrustedTestCertificate();
            _nugetExePath = _testFixture.NuGetExePath;
        }

        [CIOnlyFact]
        public async Task Restore_TamperedPackageInPackagesConfig_FailsWithErrorAsync()
        {
            // Arrange
            var packagesConfigContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<packages>" +
                "  <package id=\"X\" version=\"9.0.0\" targetFramework=\"net461\" />" +
                "</packages>";

            using (var pathContext = new SimpleTestPathContext())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = new SimpleTestProjectContext(
                    "a",
                    ProjectStyle.PackagesConfig,
                    pathContext.SolutionRoot);

                var packageX = new SimpleTestPackageContext("X", "9.0.0");
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, packageX, pathContext.PackageSource);
                SignedArchiveTestUtility.TamperWithPackage(signedPackagePath);

                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                var packagesConfigPath = Path.Combine(Directory.GetParent(projectA.ProjectPath).FullName, "packages.config");

                File.WriteAllBytes(packagesConfigPath, Encoding.ASCII.GetBytes(packagesConfigContent));

                var args = new string[]
                {
                    projectA.ProjectPath,
                    "-Source",
                    pathContext.PackageSource,
                    "-PackagesDirectory",
                    "./packages"
                };

                // Act
                var result = RunRestore(_nugetExePath, pathContext, expectedExitCode: 1, additionalArgs: args);

                // Assert
                result.ExitCode.Should().Be(1);
                result.Errors.Should().Contain(string.Format(NU3008, SigningTestUtility.AddSignatureLogPrefix(NU3008Message, packageX.Identity, pathContext.PackageSource)));
                result.AllOutput.Should().Contain(string.Format(NU3027, SigningTestUtility.AddSignatureLogPrefix(NU3027Message, packageX.Identity, pathContext.PackageSource)));
            }
        }

        [CIOnlyFact]
        public async Task Restore_TamperedPackage_FailsAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard2.0"));

                var packageX = new SimpleTestPackageContext("X", "9.0.0");
                var signedPackagePath = await SignedArchiveTestUtility.AuthorSignPackageAsync(testCertificate, packageX, pathContext.PackageSource);
                SignedArchiveTestUtility.TamperWithPackage(signedPackagePath);

                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                var args = new string[]
                {
                    projectA.ProjectPath,
                    "-Source",
                    pathContext.PackageSource
                };

                // Act
                var result = RunRestore(_nugetExePath, pathContext, expectedExitCode: 1, additionalArgs: args);
                var reader = new LockFileFormat();
                var lockFile = reader.Read(projectA.AssetsFileOutputPath);
                var errors = lockFile.LogMessages.Where(m => m.Level == LogLevel.Error);
                var warnings = lockFile.LogMessages.Where(m => m.Level == LogLevel.Warning);

                // Assert
                result.ExitCode.Should().Be(1);
                result.Errors.Should().Contain(string.Format(NU3008, SigningTestUtility.AddSignatureLogPrefix(NU3008Message, packageX.Identity, pathContext.PackageSource)));
                result.AllOutput.Should().Contain($"WARNING: {string.Format(NU3027, SigningTestUtility.AddSignatureLogPrefix(NU3027Message, packageX.Identity, pathContext.PackageSource))}");

                errors.Count().Should().Be(1);
                errors.First().Code.Should().Be(NuGetLogCode.NU3008);
                errors.First().Message.Should().Be(SigningTestUtility.AddSignatureLogPrefix(NU3008Message, packageX.Identity, pathContext.PackageSource));
                errors.First().LibraryId.Should().Be(packageX.Id);

                warnings.Count().Should().Be(1);
                warnings.First().Code.Should().Be(NuGetLogCode.NU3027);
                warnings.First().Message.Should().Be(SigningTestUtility.AddSignatureLogPrefix(NU3027Message, packageX.Identity, pathContext.PackageSource));
                warnings.First().LibraryId.Should().Be(packageX.Id);
            }
        }

        [CIOnlyFact]
        public async Task Restore_PackageWithCompressedSignature_WarnsAsync()
        {
            // Arrange
            var packageX = new SimpleTestPackageContext();

            using (var pathContext = new SimpleTestPathContext())
            using (var packageStream = await packageX.CreateAsStreamAsync())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                AuthorPrimarySignature signature = await SignedArchiveTestUtility.CreateAuthorSignatureForPackageAsync(testCertificate, packageStream);

                using (var package = new ZipArchive(packageStream, ZipArchiveMode.Update, leaveOpen: true))
                {
                    var signatureEntry = package.CreateEntry(SigningSpecifications.V1.SignaturePath);
                    using (var signatureStream = new MemoryStream(signature.GetBytes()))
                    using (var signatureEntryStream = signatureEntry.Open())
                    {
                        signatureStream.CopyTo(signatureEntryStream);
                    }
                }

                var packagePath = Path.Combine(pathContext.PackageSource, $"{packageX.ToString()}.nupkg");
                packageStream.Seek(offset: 0, loc: SeekOrigin.Begin);

                using (var fileStream = File.OpenWrite(packagePath))
                {
                    packageStream.CopyTo(fileStream);
                }

                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard2.0"));

                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                var args = new string[]
                {
                    projectA.ProjectPath
                };

                // Act
                var result = RunRestore(_nugetExePath, pathContext, expectedExitCode: 0, additionalArgs: args);
                var assetFileReader = new LockFileFormat();
                var assetsFile = assetFileReader.Read(projectA.AssetsFileOutputPath);
                var errors = assetsFile.LogMessages.Where(m => m.Level == LogLevel.Error);
                var warnings = assetsFile.LogMessages.Where(m => m.Level == LogLevel.Warning);

                // Assert
                result.ExitCode.Should().Be(0);
                result.AllOutput.Should().Contain($"WARNING: {string.Format(NU3005, SigningTestUtility.AddSignatureLogPrefix(NU3005CompressedMessage, packageX.Identity, pathContext.PackageSource))}");

                errors.Count().Should().Be(0);

                warnings.Count().Should().Be(1);
                warnings.First().Code.Should().Be(NuGetLogCode.NU3005);
                warnings.First().Message.Should().Be(SigningTestUtility.AddSignatureLogPrefix(NU3005CompressedMessage, packageX.Identity, pathContext.PackageSource));
                warnings.First().LibraryId.Should().Be(packageX.Id);
            }
        }

        [CIOnlyFact]
        public async Task Restore_PackageWithCompressedSignature_WarnAsError_FailsAndDoesNotExpandAsync()
        {
            // Arrange
            var packageX = new SimpleTestPackageContext();

            using (var pathContext = new SimpleTestPathContext())
            using (var packageStream = await packageX.CreateAsStreamAsync())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                AuthorPrimarySignature signature = await SignedArchiveTestUtility.CreateAuthorSignatureForPackageAsync(testCertificate, packageStream);

                using (var package = new ZipArchive(packageStream, ZipArchiveMode.Update, leaveOpen: true))
                {
                    var signatureEntry = package.CreateEntry(SigningSpecifications.V1.SignaturePath);
                    using (var signatureStream = new MemoryStream(signature.GetBytes()))
                    using (var signatureEntryStream = signatureEntry.Open())
                    {
                        signatureStream.CopyTo(signatureEntryStream);
                    }
                }

                var packagePath = Path.Combine(pathContext.PackageSource, $"{packageX.ToString()}.nupkg");
                packageStream.Seek(offset: 0, loc: SeekOrigin.Begin);

                using (var fileStream = File.OpenWrite(packagePath))
                {
                    packageStream.CopyTo(fileStream);
                }

                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var propsFile = Path.Combine(pathContext.SolutionRoot, "Directory.Build.props");

                using (var stream = File.OpenWrite(propsFile))
                using (var textWritter = new StreamWriter(stream))
                {
                    textWritter.Write(@"<Project><PropertyGroup><TreatWarningsAsErrors>true</TreatWarningsAsErrors></PropertyGroup></Project>");
                }

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard2.0"));

                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                var args = new string[]
                {
                    projectA.ProjectPath
                };

                // Act
                var result = RunRestore(_nugetExePath, pathContext, expectedExitCode: 1, additionalArgs: args);
                var assetFileReader = new LockFileFormat();
                var assetsFile = assetFileReader.Read(projectA.AssetsFileOutputPath);
                var errors = assetsFile.LogMessages.Where(m => m.Level == LogLevel.Error);
                var warnings = assetsFile.LogMessages.Where(m => m.Level == LogLevel.Warning);

                // Assert
                result.ExitCode.Should().Be(1);
                result.Errors.Should().Contain(string.Format(NU3005, "Warning As Error: " + SigningTestUtility.AddSignatureLogPrefix(NU3005CompressedMessage, packageX.Identity, pathContext.PackageSource)));

                errors.Count().Should().Be(1);
                errors.First().Code.Should().Be(NuGetLogCode.NU3005);
                errors.First().Message.Should().Be("Warning As Error: " + SigningTestUtility.AddSignatureLogPrefix(NU3005CompressedMessage, packageX.Identity, pathContext.PackageSource));
                errors.First().LibraryId.Should().Be(packageX.Id);

                warnings.Count().Should().Be(0);

                var installedPackageDir = Path.Combine(pathContext.UserPackagesFolder, packageX.Identity.Id);
                Directory.Exists(installedPackageDir).Should().BeFalse();
            }
        }


        [CIOnlyFact]
        public async Task Restore_PackageWithCompressedSignature_RequireMode_FailsAndDoesNotExpandAsync()
        {
            // Arrange
            var packageX = new SimpleTestPackageContext();

            using (var pathContext = new SimpleTestPathContext())
            using (var packageStream = await packageX.CreateAsStreamAsync())
            using (var testCertificate = new X509Certificate2(_trustedTestCert.Source.Cert))
            {
                AuthorPrimarySignature signature = await SignedArchiveTestUtility.CreateAuthorSignatureForPackageAsync(testCertificate, packageStream);

                using (var package = new ZipArchive(packageStream, ZipArchiveMode.Update, leaveOpen: true))
                {
                    var signatureEntry = package.CreateEntry(SigningSpecifications.V1.SignaturePath);
                    using (var signatureStream = new MemoryStream(signature.GetBytes()))
                    using (var signatureEntryStream = signatureEntry.Open())
                    {
                        signatureStream.CopyTo(signatureEntryStream);
                    }
                }

                var packagePath = Path.Combine(pathContext.PackageSource, $"{packageX.ToString()}.nupkg");
                packageStream.Seek(offset: 0, loc: SeekOrigin.Begin);

                using (var fileStream = File.OpenWrite(packagePath))
                {
                    packageStream.CopyTo(fileStream);
                }

                // Set up solution, project, and packages
                var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot);

                var propsFile = Path.Combine(pathContext.SolutionRoot, "NuGet.Config");

                using (var stream = File.OpenWrite(propsFile))
                using (var textWritter = new StreamWriter(stream))
                {
                    textWritter.Write(@"<configuration><config><add key=""signatureValidationMode"" value=""require"" /></config></configuration>");
                }

                var projectA = SimpleTestProjectContext.CreateNETCore(
                    "a",
                    pathContext.SolutionRoot,
                    NuGetFramework.Parse("NETStandard2.0"));

                projectA.AddPackageToAllFrameworks(packageX);
                solution.Projects.Add(projectA);
                solution.Create(pathContext.SolutionRoot);

                var args = new string[]
                {
                    projectA.ProjectPath
                };

                // Act
                var result = RunRestore(_nugetExePath, pathContext, expectedExitCode: 1, additionalArgs: args);
                var assetFileReader = new LockFileFormat();
                var assetsFile = assetFileReader.Read(projectA.AssetsFileOutputPath);
                var errors = assetsFile.LogMessages.Where(m => m.Level == LogLevel.Error);
                var warnings = assetsFile.LogMessages.Where(m => m.Level == LogLevel.Warning);

                // Assert
                result.ExitCode.Should().Be(1);
                result.Errors.Should().Contain(string.Format(NU3005, SigningTestUtility.AddSignatureLogPrefix(NU3005CompressedMessage, packageX.Identity, pathContext.PackageSource)));

                errors.Count().Should().Be(1);
                errors.First().Code.Should().Be(NuGetLogCode.NU3005);
                errors.First().Message.Should().Be(SigningTestUtility.AddSignatureLogPrefix(NU3005CompressedMessage, packageX.Identity, pathContext.PackageSource));
                errors.First().LibraryId.Should().Be(packageX.Identity.Id.ToString());

                warnings.Count().Should().Be(0);

                var installedPackageDir = Path.Combine(pathContext.UserPackagesFolder, packageX.Identity.Id);
                Directory.Exists(installedPackageDir).Should().BeFalse();
            }
        }

        [Fact]
        public void GetCertificateChain_WithUntrustedRoot_Throws()
        {
            using (X509ChainHolder chainHolder = X509ChainHolder.CreateForCodeSigning())
            using (var rootCertificate = SigningTestUtility.GetCertificate("root.crt"))
            using (var intermediateCertificate = SigningTestUtility.GetCertificate("intermediate.crt"))
            using (var leafCertificate = SigningTestUtility.GetCertificate("leaf.crt"))
            {
                var extraStore = new X509Certificate2Collection() { rootCertificate, intermediateCertificate };
                var logger = new TestLogger();

                var exception = Assert.Throws<SignatureException>(
                    () => CertificateChainUtility.GetCertificateChain(
                        leafCertificate,
                        extraStore,
                        logger,
                        CertificateType.Signature));

                Assert.Equal(NuGetLogCode.NU3018, exception.Code);
                Assert.Equal("Certificate chain validation failed.", exception.Message);
                Assert.Equal(1, logger.Errors);

                SigningTestUtility.AssertUntrustedRoot(logger.LogMessages, LogLevel.Error);
                SigningTestUtility.AssertOfflineRevocation(logger.LogMessages, LogLevel.Warning);

                if (RuntimeEnvironmentHelper.IsWindows)
                {
                    SigningTestUtility.AssertRevocationStatusUnknown(logger.LogMessages, LogLevel.Warning);
                }
            }
        }

        public static CommandRunnerResult RunRestore(string nugetExe, SimpleTestPathContext pathContext, int expectedExitCode = 0, params string[] additionalArgs)
        {
            // Store the dg file for debugging
            var envVars = new Dictionary<string, string>()
            {
                { "NUGET_HTTP_CACHE_PATH", pathContext.HttpCacheFolder }
            };

            var args = new string[]
            {
                "restore",
                "-Verbosity",
                "detailed"
            };

            args = args.Concat(additionalArgs).ToArray();

            // Act
            var r = CommandRunner.Run(
                nugetExe,
                pathContext.WorkingDirectory,
                string.Join(" ", args),
                waitForExit: true,
                environmentVariables: envVars);

            // Assert
            Assert.True(expectedExitCode == r.ExitCode, r.AllOutput);

            return r;
        }
    }
}
