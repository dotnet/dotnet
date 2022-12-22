// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using Moq;
using NuGet.CommandLine.XPlat;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Packaging.Signing;
using Xunit;

namespace NuGet.XPlat.FuncTest
{
    [Collection("NuGet XPlat Test Collection")]
    public class XplatSignTests
    {
        private const string _invalidArgException = "Invalid value provided for '{0}'. The accepted values are {1}.";

        [Fact]
        public void SignCommandArgsParsing_MissingPackagePath_Throws()
        {
            SignCommandArgs(
                (mockCommandRunner, testApp, getLogLevel, getParsedArg) =>
                {
                    // Arrange
                    var argList = new List<string>() { "sign" };

                    // Act
                    var ex = Assert.Throws<AggregateException>(() => testApp.Execute(argList.ToArray()));

                    // Assert
                    Assert.IsType<ArgumentException>(ex.InnerException);
                    Assert.Equal("Unable to sign package. Argument '<package-paths>' not provided.", ex.InnerException.Message);
                });
        }

        [Theory]
        [InlineData(@"\\path\file.pfx", "test_cert_subject", "")]
        [InlineData("\\path\file.cert", "", "test_cert_fingerprint")]
        [InlineData("\\path\file.cert", "test_cert_subject", "test_cert_fingerprint")]
        [InlineData("", "test_cert_subject", "test_cert_fingerprint")]
        public void SignCommandArgParsing_MultipleCertificateOptions_Throws(
            string certificatePath,
            string certificateSubjectName,
            string certificateFingerprint)
        {
            var packagePath = @"\\path\package.nupkg";
            var timestamper = "https://timestamper.test";

            SignCommandArgs(
                (mockCommandRunner, testApp, getLogLevel, getParsedArg) =>
                {
                    //Arrange
                    var argList = new List<string>() { "sign", packagePath, "--certificate-path", certificatePath, "--certificate-subject-name", certificateSubjectName, "--certificate-fingerprint", certificateFingerprint, timestamper };

                    // Act
                    var ex = Assert.Throws<AggregateException>(() => testApp.Execute(argList.ToArray()));

                    // Assert
                    Assert.IsType<ArgumentException>(ex.InnerException);
                    Assert.Equal(Strings.SignCommandMultipleCertificateException, ex.InnerException.Message);
                });
        }

        [Theory]
        [InlineData("AddressBook")]
        [InlineData("AuthRoot")]
        [InlineData("CertificateAuthority")]
        [InlineData("Disallowed")]
        [InlineData("My")]
        [InlineData("Root")]
        [InlineData("TrustedPeople")]
        [InlineData("TrustedPublisher")]
        [InlineData("AddreSSBook")]
        [InlineData("addressbook")]
        public void SignCommandArgParsing_ValidCertificateStoreName_Succeeds(string storeName)
        {
            // Arrange
            var packagePath = @"\\path\package.nupkg";
            var timestamper = "https://timestamper.test";
            var certificateFingerprint = new Guid().ToString();
            var parsable = Enum.TryParse(storeName, ignoreCase: true, result: out StoreName parsedStoreName);

            SignCommandArgs(
                (mockCommandRunner, testApp, getLogLevel, getParsedArg) =>
                {
                    //Arrange
                    var argList = new List<string>() { "sign", packagePath, "--certificate-store-name", storeName, "--certificate-fingerprint", certificateFingerprint, "--timestamper", timestamper };

                    // Act
                    testApp.Execute(argList.ToArray());

                    // Assert
                    Assert.True(parsable);
                    Assert.Equal(StoreLocation.CurrentUser, getParsedArg().CertificateStoreLocation);
                    Assert.Equal(parsedStoreName, getParsedArg().CertificateStoreName);
                });
        }

        [Fact]
        public void SignCommandArgParsing_InvalidCertificateStoreName_Throws()
        {
            // Arrange
            var packagePath = @"\\path\package.nupkg";
            var timestamper = "https://timestamper.test";
            var certificateFingerprint = new Guid().ToString();
            var storeName = "random_store";

            SignCommandArgs(
                (mockCommandRunner, testApp, getLogLevel, getParsedArg) =>
                {
                    //Arrange
                    var argList = new List<string>() { "sign", packagePath, "--certificate-store-name", storeName, "--certificate-fingerprint", certificateFingerprint, "--timestamper", timestamper };

                    // Act
                    var ex = Assert.Throws<AggregateException>(() => testApp.Execute(argList.ToArray()));

                    // Assert
                    Assert.IsType<ArgumentException>(ex.InnerException);
                    string acceptedStoreNameList = string.Join(",", Enum.GetValues(typeof(StoreName)).Cast<StoreName>().ToList());
                    Assert.Equal(string.Format(_invalidArgException, "certificate-store-name", acceptedStoreNameList), ex.InnerException.Message);
                });
        }


        [Theory]
        [InlineData("CurrentUser")]
        [InlineData("LocalMachine")]
        [InlineData("currentuser")]
        [InlineData("cURRentuser")]
        public void SignCommandArgParsing_ValidCertificateStoreLocation_Succeeds(string storeLocation)
        {
            // Arrange
            var packagePath = @"\\path\package.nupkg";
            var timestamper = "https://timestamper.test";
            var certificateFingerprint = new Guid().ToString();
            var parsable = Enum.TryParse(storeLocation, ignoreCase: true, result: out StoreLocation parsedStoreLocation);

            SignCommandArgs(
                (mockCommandRunner, testApp, getLogLevel, getParsedArg) =>
                {

                    //Arrange
                    var argList = new List<string>() { "sign", packagePath, "--certificate-fingerprint", certificateFingerprint, "--certificate-store-location", storeLocation, "--timestamper", timestamper };

                    //Act
                    testApp.Execute(argList.ToArray());

                    //Assert
                    Assert.True(parsable);
                    Assert.Equal(StoreName.My, getParsedArg().CertificateStoreName);
                    Assert.Equal(parsedStoreLocation, getParsedArg().CertificateStoreLocation);
                });
        }

        [Fact]
        public void SignCommandArgParsing_InvalidCertificateStoreLocation_Throws()
        {
            // Arrange
            var packagePath = @"\\path\package.nupkg";
            var timestamper = "https://timestamper.test";
            var certificateFingerprint = new Guid().ToString();
            var storeLocation = "random_location";

            SignCommandArgs(
                (mockCommandRunner, testApp, getLogLevel, getParsedArg) =>
                {

                    //Arrange
                    var argList = new List<string>() { "sign", packagePath, "--certificate-fingerprint", certificateFingerprint, "--certificate-store-location", storeLocation, "--timestamper", timestamper };

                    // Act
                    var ex = Assert.Throws<AggregateException>(() => testApp.Execute(argList.ToArray()));

                    // Assert
                    Assert.IsType<ArgumentException>(ex.InnerException);
                    string acceptedStoreLocationList = string.Join(",", Enum.GetValues(typeof(StoreLocation)).Cast<StoreLocation>().ToList());
                    Assert.Equal(string.Format(_invalidArgException, "certificate-store-location", acceptedStoreLocationList), ex.InnerException.Message);
                });
        }

        [Theory]
        [InlineData("sha256")]
        [InlineData("sha384")]
        [InlineData("sha512")]
        [InlineData("ShA256")]
        [InlineData("SHA256")]
        [InlineData("SHA384")]
        [InlineData("SHA512")]
        public void SignCommandArgParsing_ValidHashAlgorithm_Succeeds(string hashAlgorithm)
        {
            // Arrange
            var packagePath = @"\\path\package.nupkg";
            var timestamper = "https://timestamper.test";
            var certificatePath = @"\\path\file.pfx";
            var parsable = Enum.TryParse(hashAlgorithm, ignoreCase: true, result: out HashAlgorithmName parsedHashAlgorithm);

            SignCommandArgs(
                (mockCommandRunner, testApp, getLogLevel, getParsedArg) =>
                {

                    // Arrange
                    var argList = new List<string>() { "sign", packagePath, "--certificate-path", certificatePath, "--hash-algorithm", hashAlgorithm, "--timestamper", timestamper };

                    // Act
                    testApp.Execute(argList.ToArray());

                    // Assert
                    Assert.True(parsable);
                    Assert.Equal(parsedHashAlgorithm, getParsedArg().SignatureHashAlgorithm);
                    Assert.Equal(HashAlgorithmName.SHA256, getParsedArg().TimestampHashAlgorithm);
                });
        }

        [Fact]
        public void SignCommandArgParsing_InvalidHashAlgorithm_Throws()
        {
            // Arrange
            var packagePath = @"\\path\package.nupkg";
            var timestamper = "https://timestamper.test";
            var certificatePath = @"\\path\file.pfx";
            var hashAlgorithm = "MD5";

            SignCommandArgs(
                (mockCommandRunner, testApp, getLogLevel, getParsedArg) =>
                {

                    //Arrange
                    var argList = new List<string>() { "sign", packagePath, "--certificate-path", certificatePath, "--hash-algorithm", hashAlgorithm, "--timestamper", timestamper };

                    //Act & Assert
                    var ex = Assert.Throws<AggregateException>(() => testApp.Execute(argList.ToArray()));
                    Assert.IsType<ArgumentException>(ex.InnerException);
                    var allowedHashAlgorithms = string.Join(",", SigningSpecifications.V1.AllowedHashAlgorithms);
                    Assert.Equal(string.Format(_invalidArgException, "hash-algorithm", allowedHashAlgorithms), ex.InnerException.Message);
                });
        }

        [Theory]
        [InlineData("sha256")]
        [InlineData("sha384")]
        [InlineData("sha512")]
        [InlineData("ShA256")]
        [InlineData("SHA256")]
        [InlineData("SHA384")]
        [InlineData("SHA512")]
        public void SignCommandArgParsing_ValidTimestampHashAlgorithm_Succeeds(string timestampHashAlgorithm)
        {
            // Arrange
            var packagePath = @"\\path\package.nupkg";
            var timestamper = "https://timestamper.test";
            var certificatePath = @"\\path\file.pfx";
            var parsable = Enum.TryParse(timestampHashAlgorithm, ignoreCase: true, result: out HashAlgorithmName parsedTimestampHashAlgorithm);

            SignCommandArgs(
                (mockCommandRunner, testApp, getLogLevel, getParsedArg) =>
                {

                    //Arrange
                    var argList = new List<string>() { "sign", packagePath, "--certificate-path", certificatePath, "--timestamper", timestamper, "--timestamp-hash-algorithm", timestampHashAlgorithm };

                    // Act
                    testApp.Execute(argList.ToArray());

                    // Assert
                    Assert.True(parsable);
                    Assert.Equal(parsedTimestampHashAlgorithm, getParsedArg().TimestampHashAlgorithm);
                    Assert.Equal(HashAlgorithmName.SHA256, getParsedArg().SignatureHashAlgorithm);

                });
        }

        [Fact]
        public void SignCommandArgParsing_InvalidTimestampHashAlgorithm_Throws()
        {
            // Arrange
            var packagePath = @"\\path\package.nupkg";
            var timestamper = "https://timestamper.test";
            var certificatePath = @"\\path\file.pfx";
            var timestampHashAlgorithm = "MD5";

            SignCommandArgs(
                (mockCommandRunner, testApp, getLogLevel, getParsedArg) =>
                {
                    //Arrange
                    var argList = new List<string>() { "sign", packagePath, "--certificate-path", certificatePath, "--timestamper", timestamper, "--timestamp-hash-algorithm", timestampHashAlgorithm };

                    //Act & Assert
                    var ex = Assert.Throws<AggregateException>(() => testApp.Execute(argList.ToArray()));
                    Assert.IsType<ArgumentException>(ex.InnerException);
                    var allowedHashAlgorithms = string.Join(",", SigningSpecifications.V1.AllowedHashAlgorithms);
                    Assert.Equal(string.Format(_invalidArgException, "timestamp-hash-algorithm", allowedHashAlgorithms), ex.InnerException.Message);
                });
        }

        [Fact]
        public void SignCommandArgParsing_ValidArgsContainsCertFingerprintAsync_Succeeds()
        {
            // Arrange
            var packagePath = @"\\path\package.nupkg";
            var timestamper = "https://timestamper.test";
            var certificateFingerprint = new Guid().ToString();
            var hashAlgorithm = "sha256";
            Enum.TryParse(hashAlgorithm, ignoreCase: true, result: out HashAlgorithmName parsedHashAlgorithm);
            var timestampHashAlgorithm = "sha512";
            Enum.TryParse(timestampHashAlgorithm, ignoreCase: true, result: out HashAlgorithmName parsedTimestampHashAlgorithm);
            var storeName = "CertificateAuthority";
            Enum.TryParse(storeName, ignoreCase: true, result: out StoreName parsedStoreName);
            var storeLocation = "LocalMachine";
            Enum.TryParse(storeLocation, ignoreCase: true, result: out StoreLocation parsedStoreLocation);
            var nonInteractive = true;
            var overwrite = true;
            var outputDir = @".\test\output\path";

            SignCommandArgs(
                (mockCommandRunner, testApp, getLogLevel, getParsedArg) =>
                {
                    //Arrange
                    var argList = new List<string>() { "sign", packagePath, "--certificate-fingerprint", certificateFingerprint, "--certificate-store-name", storeName, "--certificate-store-location", storeLocation, "--hash-algorithm", hashAlgorithm,
                        "--timestamper", timestamper, "--timestamp-hash-algorithm", timestampHashAlgorithm, "--output", outputDir, "--overwrite" };

                    //Act
                    testApp.Execute(argList.ToArray());

                    //Assert
                    Assert.Null(getParsedArg().CertificatePath);
                    Assert.Equal(certificateFingerprint, getParsedArg().CertificateFingerprint, StringComparer.Ordinal);
                    Assert.Null(getParsedArg().CertificateSubjectName);
                    Assert.Equal(parsedStoreLocation, getParsedArg().CertificateStoreLocation);
                    Assert.Equal(parsedStoreName, getParsedArg().CertificateStoreName);
                    Assert.Equal(nonInteractive, getParsedArg().NonInteractive);
                    Assert.Equal(overwrite, getParsedArg().Overwrite);
                    Assert.Equal(packagePath, getParsedArg().PackagePaths[0], StringComparer.Ordinal);
                    Assert.Equal(timestamper, getParsedArg().Timestamper, StringComparer.Ordinal);
                    Assert.Equal(outputDir, getParsedArg().OutputDirectory, StringComparer.Ordinal);
                });
        }

        [Fact]
        public void SignCommandArgParsing_ValidArgsContainsCertSubjectNameAsync_Succeeds()
        {
            // Arrange
            var packagePath = @"\\path\package.nupkg";
            var timestamper = "https://timestamper.test";
            var certificateSubjectName = new Guid().ToString();
            var hashAlgorithm = "sha256";
            Enum.TryParse(hashAlgorithm, ignoreCase: true, result: out HashAlgorithmName parsedHashAlgorithm);
            var timestampHashAlgorithm = "sha512";
            Enum.TryParse(timestampHashAlgorithm, ignoreCase: true, result: out HashAlgorithmName parsedTimestampHashAlgorithm);
            var storeName = "CertificateAuthority";
            Enum.TryParse(storeName, ignoreCase: true, result: out StoreName parsedStoreName);
            var storeLocation = "LocalMachine";
            Enum.TryParse(storeLocation, ignoreCase: true, result: out StoreLocation parsedStoreLocation);
            var nonInteractive = true;
            var overwrite = true;
            var outputDir = @".\test\output\path";

            SignCommandArgs(
                (mockCommandRunner, testApp, getLogLevel, getParsedArg) =>
                {
                    //Arrange
                    var argList = new List<string>() { "sign", packagePath, "--certificate-subject-name", certificateSubjectName, "--certificate-store-name", storeName, "--certificate-store-location", storeLocation, "--hash-algorithm", hashAlgorithm,
                        "--timestamper", timestamper, "--timestamp-hash-algorithm", timestampHashAlgorithm, "--output", outputDir, "--overwrite" };

                    //Act
                    testApp.Execute(argList.ToArray());

                    //Assert
                    Assert.Null(getParsedArg().CertificatePath);
                    Assert.Null(getParsedArg().CertificateFingerprint);
                    Assert.Equal(certificateSubjectName, getParsedArg().CertificateSubjectName, StringComparer.Ordinal);
                    Assert.Equal(parsedStoreLocation, getParsedArg().CertificateStoreLocation);
                    Assert.Equal(parsedStoreName, getParsedArg().CertificateStoreName);
                    Assert.Equal(nonInteractive, getParsedArg().NonInteractive);
                    Assert.Equal(overwrite, getParsedArg().Overwrite);
                    Assert.Equal(packagePath, getParsedArg().PackagePaths[0], StringComparer.Ordinal);
                    Assert.Equal(timestamper, getParsedArg().Timestamper, StringComparer.Ordinal);
                    Assert.Equal(outputDir, getParsedArg().OutputDirectory, StringComparer.Ordinal);
                });
        }

        [Fact]
        public void SignCommandArgParsing_ValidArgsContainsCertPathAsync_Succeeds()
        {
            // Arrange
            var packagePath = @"\\path\package.nupkg";
            var timestamper = "https://timestamper.test";
            var certificatePath = @"\\path\file.pfx";
            var hashAlgorithm = "sha256";
            Enum.TryParse(hashAlgorithm, ignoreCase: true, result: out HashAlgorithmName parsedHashAlgorithm);
            var timestampHashAlgorithm = "sha512";
            Enum.TryParse(timestampHashAlgorithm, ignoreCase: true, result: out HashAlgorithmName parsedTimestampHashAlgorithm);
            var storeName = "My";
            Enum.TryParse(storeName, ignoreCase: true, result: out StoreName parsedStoreName);
            var storeLocation = "CurrentUser";
            Enum.TryParse(storeLocation, ignoreCase: true, result: out StoreLocation parsedStoreLocation);
            var nonInteractive = true;
            var overwrite = true;
            var outputDir = @".\test\output\path";

            SignCommandArgs(
                (mockCommandRunner, testApp, getLogLevel, getParsedArg) =>
                {
                    //Arrange
                    var argList = new List<string>() { "sign", packagePath, "--certificate-path", certificatePath, "--hash-algorithm", hashAlgorithm, "--timestamper", timestamper, "--timestamp-hash-algorithm", timestampHashAlgorithm, "--output", outputDir, "--overwrite" };

                    //Act
                    testApp.Execute(argList.ToArray());

                    //Assert
                    Assert.Equal(certificatePath, getParsedArg().CertificatePath, StringComparer.Ordinal);
                    Assert.Null(getParsedArg().CertificateFingerprint);
                    Assert.Null(getParsedArg().CertificateSubjectName);
                    Assert.Equal(parsedStoreLocation, getParsedArg().CertificateStoreLocation);
                    Assert.Equal(parsedStoreName, getParsedArg().CertificateStoreName);
                    Assert.Equal(nonInteractive, getParsedArg().NonInteractive);
                    Assert.Equal(overwrite, getParsedArg().Overwrite);
                    Assert.Equal(packagePath, getParsedArg().PackagePaths[0], StringComparer.Ordinal);
                    Assert.Equal(timestamper, getParsedArg().Timestamper, StringComparer.Ordinal);
                    Assert.Equal(outputDir, getParsedArg().OutputDirectory, StringComparer.Ordinal);
                });
        }

        private void SignCommandArgs(Action<Mock<ISignCommandRunner>, CommandLineApplication, Func<LogLevel>, Func<SignArgs>> verify)
        {
            // Arrange
            var logLevel = LogLevel.Information;
            var logger = new TestCommandOutputLogger();
            var testApp = new CommandLineApplication();
            var mockCommandRunner = new Mock<ISignCommandRunner>();

            SignArgs parsedArgs = null;
            mockCommandRunner
                .Setup(m => m.ExecuteCommandAsync(It.IsAny<SignArgs>()))
                .Callback<SignArgs>(x => parsedArgs = x)
                .Returns(Task.FromResult(0));

            testApp.Name = "dotnet nuget_test";
            SignCommand.Register(testApp,
                () => logger,
                ll => logLevel = ll,
                () => mockCommandRunner.Object);

            // Act & Assert
            verify(mockCommandRunner, testApp, () => logLevel, () => parsedArgs);
        }
    }
}
