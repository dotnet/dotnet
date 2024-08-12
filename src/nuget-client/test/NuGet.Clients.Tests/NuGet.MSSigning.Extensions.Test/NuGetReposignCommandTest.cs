// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Moq;
using NuGet.MSSigning.Extensions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.MSSigning.Extensions.Test
{
    public class NuGetReposignCommandTest
    {
        private const string NoPackageException = "No package was provided. For a list of accepted ways to provide a package, please visit https://docs.nuget.org/docs/reference/command-line-reference";
        private const string InvalidArgException = "Invalid value provided for '{0}'. For a list of accepted values, please visit https://learn.microsoft.com/nuget/reference/cli-reference/cli-ref-sign";
        private const string NoCertificateException = "No {0} provided or provided file is not a p7b file.";
        private const string InvalidCertificateFingerprint = "Invalid value for 'CertificateFingerprint' option. The value must be a SHA-256, SHA-384, or SHA-512 certificate fingerprint (in hexadecimal).";
        private const string Sha256Hash = "a591a6d40bf420404a011733cfb7b190d62c65bf0bcda32b55b046cbb7f506fb";

        [Fact]
        public void ReposignCommandArgParsing_NoPackagePath()
        {
            using (var dir = TestDirectory.Create())
            {
                // Arrange
                var timestamper = "https://timestamper.test";
                var certFile = Path.Combine(dir, "cert.p7b");
                var certificateFingerprint = new Guid().ToString();
                var keyContainer = new Guid().ToString();
                var cspName = "cert provider";
                var v3serviceIndexUrl = "https://v3serviceIndex.test/api/index.json";

                var mockConsole = new Mock<IConsole>();
                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestamper,
                    CertificateFile = certFile,
                    CSPName = cspName,
                    KeyContainer = keyContainer,
                    CertificateFingerprint = certificateFingerprint,
                    V3ServiceIndexUrl = v3serviceIndexUrl,
                };

                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() => reposignCommand.GetRepositorySignRequest());
                Assert.Equal(NoPackageException, ex.Message);
            }
        }

        [Fact]
        public void ReposignCommandArgParsing_NoCertificateFile()
        {
            using (var dir = TestDirectory.Create())
            {
                // Arrange
                var packagePath = Path.Combine(dir, "package.nupkg");
                var timestamper = "https://timestamper.test";
                var certificateFingerprint = new Guid().ToString();
                var keyContainer = new Guid().ToString();
                var cspName = "cert provider";
                var v3serviceIndexUrl = "https://v3serviceIndex.test/api/index.json";

                var mockConsole = new Mock<IConsole>();
                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestamper,
                    CSPName = cspName,
                    KeyContainer = keyContainer,
                    CertificateFingerprint = certificateFingerprint,
                    V3ServiceIndexUrl = v3serviceIndexUrl,
                };
                reposignCommand.Arguments.Add(packagePath);

                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() => reposignCommand.GetRepositorySignRequest());
                Assert.Equal(string.Format(NoCertificateException, nameof(reposignCommand.CertificateFile)), ex.Message);
            }
        }

        [Fact]
        public void ReposignCommandArgParsing_NoCSPName()
        {
            using (var dir = TestDirectory.Create())
            {
                // Arrange
                var packagePath = Path.Combine(dir, "package.nupkg");
                var timestamper = "https://timestamper.test";
                var certFile = Path.Combine(dir, "cert.p7b");
                var certificateFingerprint = Sha256Hash;
                var keyContainer = new Guid().ToString();
                var v3serviceIndexUrl = "https://v3serviceIndex.test/api/index.json";

                var mockConsole = new Mock<IConsole>();
                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestamper,
                    CertificateFile = certFile,
                    KeyContainer = keyContainer,
                    CertificateFingerprint = certificateFingerprint,
                    V3ServiceIndexUrl = v3serviceIndexUrl,
                };

                reposignCommand.Arguments.Add(packagePath);

                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() => reposignCommand.GetRepositorySignRequest());
                Assert.Equal(string.Format(InvalidArgException, nameof(reposignCommand.CSPName)), ex.Message);
            }
        }

        [Fact]
        public void ReposignCommandArgParsing_NoKeyContainer()
        {
            using (var dir = TestDirectory.Create())
            {
                // Arrange
                var packagePath = Path.Combine(dir, "package.nupkg");
                var timestamper = "https://timestamper.test";
                var certFile = Path.Combine(dir, "cert.p7b");
                var certificateFingerprint = Sha256Hash;
                var cspName = "cert provider";
                var v3serviceIndexUrl = "https://v3serviceIndex.test/api/index.json";

                var mockConsole = new Mock<IConsole>();
                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestamper,
                    CertificateFile = certFile,
                    CSPName = cspName,
                    CertificateFingerprint = certificateFingerprint,
                    V3ServiceIndexUrl = v3serviceIndexUrl,
                };

                reposignCommand.Arguments.Add(packagePath);

                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() => reposignCommand.GetRepositorySignRequest());
                Assert.Equal(string.Format(InvalidArgException, nameof(reposignCommand.KeyContainer)), ex.Message);
            }
        }

        [Fact]
        public void ReposignCommandArgParsing_NoCertificateFingerprint()
        {
            using (var dir = TestDirectory.Create())
            {
                // Arrange
                var packagePath = Path.Combine(dir, "package.nupkg");
                var timestamper = "https://timestamper.test";
                var certFile = Path.Combine(dir, "cert.p7b");
                var keyContainer = new Guid().ToString();
                var cspName = "cert provider";
                var v3serviceIndexUrl = "https://v3serviceIndex.test/api/index.json";

                var mockConsole = new Mock<IConsole>();
                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestamper,
                    CertificateFile = certFile,
                    CSPName = cspName,
                    KeyContainer = keyContainer,
                    V3ServiceIndexUrl = v3serviceIndexUrl,
                };

                reposignCommand.Arguments.Add(packagePath);

                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() => reposignCommand.GetRepositorySignRequest());
                Assert.Equal(InvalidCertificateFingerprint, ex.Message);
            }
        }

        [Theory]
        [InlineData("89967D1DD995010B6C66AE24FF8E66885E6E03")] // 39 characters long not SHA-1
        [InlineData("invalid-certificate-fingerprint")]
        public void ReposignCommandArgParsing_InvalidCertificateFingerprint_Throws_Exception(string certificateFingerprint)
        {
            using (var dir = TestDirectory.Create())
            {
                // Arrange
                var packagePath = Path.Combine(dir, "package.nupkg");
                var timestamper = "https://timestamper.test";
                var certFile = Path.Combine(dir, "cert.p7b");
                var keyContainer = new Guid().ToString();
                var cspName = "cert provider";
                var v3serviceIndexUrl = "https://v3serviceIndex.test/api/index.json";

                var mockConsole = new Mock<IConsole>();
                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestamper,
                    CertificateFile = certFile,
                    CertificateFingerprint = certificateFingerprint,
                    CSPName = cspName,
                    KeyContainer = keyContainer,
                    V3ServiceIndexUrl = v3serviceIndexUrl,
                };

                reposignCommand.Arguments.Add(packagePath);

                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() => reposignCommand.GetRepositorySignRequest());
                Assert.Equal(InvalidCertificateFingerprint, ex.Message);
            }
        }

        [Fact]
        public void ReposignCommandArgParsing_NoV3ServiceIndex()
        {
            using (var dir = TestDirectory.Create())
            {
                // Arrange
                var packagePath = Path.Combine(dir, "package.nupkg");
                var timestamper = "https://timestamper.test";
                var certFile = Path.Combine(dir, "cert.p7b");
                var certificateFingerprint = Sha256Hash;
                var keyContainer = new Guid().ToString();
                var cspName = "cert provider";

                var mockConsole = new Mock<IConsole>();
                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestamper,
                    CertificateFile = certFile,
                    CSPName = cspName,
                    KeyContainer = keyContainer,
                    CertificateFingerprint = certificateFingerprint,
                };

                reposignCommand.Arguments.Add(packagePath);

                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() => reposignCommand.GetRepositorySignRequest());
                Assert.Equal(string.Format(InvalidArgException, nameof(reposignCommand.V3ServiceIndexUrl)), ex.Message);
            }
        }

        [Fact]
        public void ReposignCommandArgParsing_InvalidV3ServiceIndex_NotValidURI()
        {
            using (var dir = TestDirectory.Create())
            {
                // Arrange
                var packagePath = Path.Combine(dir, "package.nupkg");
                var timestamper = "https://timestamper.test";
                var certFile = Path.Combine(dir, "cert.p7b");
                var certificateFingerprint = Sha256Hash;
                var keyContainer = new Guid().ToString();
                var cspName = "cert provider";
                var serviceIndex = "not-valid-uri";

                var mockConsole = new Mock<IConsole>();
                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestamper,
                    CertificateFile = certFile,
                    CSPName = cspName,
                    KeyContainer = keyContainer,
                    CertificateFingerprint = certificateFingerprint,
                    V3ServiceIndexUrl = serviceIndex,
                };

                reposignCommand.Arguments.Add(packagePath);

                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() => reposignCommand.GetRepositorySignRequest());
                Assert.Equal(string.Format(InvalidArgException, nameof(reposignCommand.V3ServiceIndexUrl)), ex.Message);
            }
        }

        [Fact]
        public void ReposignCommandArgParsing_InvalidV3ServiceIndex_NotHTTPS()
        {
            using (var dir = TestDirectory.Create())
            {
                // Arrange
                var packagePath = Path.Combine(dir, "package.nupkg");
                var timestamper = "https://timestamper.test";
                var certFile = Path.Combine(dir, "cert.p7b");
                var certificateFingerprint = Sha256Hash;
                var keyContainer = new Guid().ToString();
                var cspName = "cert provider";
                var serviceIndex = "http://validv3NonhttpsUri.test/api/index.json";

                var mockConsole = new Mock<IConsole>();
                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestamper,
                    CertificateFile = certFile,
                    CSPName = cspName,
                    KeyContainer = keyContainer,
                    CertificateFingerprint = certificateFingerprint,
                    V3ServiceIndexUrl = serviceIndex,
                };

                reposignCommand.Arguments.Add(packagePath);

                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() => reposignCommand.GetRepositorySignRequest());
                Assert.Equal(string.Format(InvalidArgException, nameof(reposignCommand.V3ServiceIndexUrl)), ex.Message);
            }
        }

        [Fact]
        public void ReposignCommandArgParsing_InvalidPackageOwners()
        {
            using (var dir = TestDirectory.Create())
            {
                // Arrange
                var packagePath = Path.Combine(dir, "package.nupkg");
                var timestamper = "https://timestamper.test";
                var certFile = Path.Combine(dir, "cert.p7b");
                var certificateFingerprint = Sha256Hash;
                var keyContainer = new Guid().ToString();
                var cspName = "cert provider";
                var serviceIndex = "https://v3serviceindex.test/api/index.json";
                var packageOwners = new List<string>() { "owner", "", "anotherOwner", null };

                var mockConsole = new Mock<IConsole>();
                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestamper,
                    CertificateFile = certFile,
                    CSPName = cspName,
                    KeyContainer = keyContainer,
                    CertificateFingerprint = certificateFingerprint,
                    V3ServiceIndexUrl = serviceIndex,
                    PackageOwners = packageOwners,
                };

                reposignCommand.Arguments.Add(packagePath);

                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() => reposignCommand.GetRepositorySignRequest());
                Assert.Equal(string.Format(InvalidArgException, nameof(reposignCommand.PackageOwners)), ex.Message);
            }
        }

        [Theory]
        [InlineData("sha256")]
        [InlineData("sha384")]
        [InlineData("sha512")]
        [InlineData("ShA256")]
        [InlineData("SHA256")]
        [InlineData("SHA384")]
        [InlineData("SHA512")]
        public void ReposignCommandArgParsing_ValidHashAlgorithm(string hashAlgorithm)
        {
            using (var dir = TestDirectory.Create())
            {
                // Arrange
                var packagePath = Path.Combine(dir, "package.nupkg");
                var timestamper = "https://timestamper.test";
                var certFile = Path.Combine(dir, "cert.p7b");
                var certificateFingerprint = Sha256Hash;
                var keyContainer = new Guid().ToString();
                var cspName = "cert provider";
                var serviceIndex = "https://v3serviceindex.test/api/index.json";
                var parsable = Enum.TryParse(hashAlgorithm, ignoreCase: true, result: out Common.HashAlgorithmName parsedHashAlgorithm);
                var mockConsole = new Mock<IConsole>();
                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestamper,
                    CertificateFile = certFile,
                    CertificateFingerprint = certificateFingerprint,
                    KeyContainer = keyContainer,
                    CSPName = cspName,
                    V3ServiceIndexUrl = serviceIndex,
                    HashAlgorithm = hashAlgorithm
                };

                reposignCommand.Arguments.Add(packagePath);

                // Act & Assert
                Assert.True(parsable);
                var ex = Assert.Throws<CryptographicException>(() => reposignCommand.GetRepositorySignRequest());
                Assert.NotEqual(string.Format(InvalidArgException, nameof(reposignCommand.HashAlgorithm)), ex.Message);
            }
        }

        [Fact]
        public void ReposignCommandArgParsing_InvalidHashAlgorithm()
        {
            using (var dir = TestDirectory.Create())
            {
                // Arrange
                var packagePath = Path.Combine(dir, "package.nupkg");
                var timestamper = "https://timestamper.test";
                var certFile = Path.Combine(dir, "cert.p7b");
                var certificateFingerprint = Sha256Hash;
                var keyContainer = new Guid().ToString();
                var cspName = "cert provider";
                var serviceIndex = "https://v3serviceindex.test/api/index.json";
                var hashAlgorithm = "MD5";
                var mockConsole = new Mock<IConsole>();

                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestamper,
                    CertificateFile = certFile,
                    CertificateFingerprint = certificateFingerprint,
                    KeyContainer = keyContainer,
                    CSPName = cspName,
                    V3ServiceIndexUrl = serviceIndex,
                    HashAlgorithm = hashAlgorithm
                };

                reposignCommand.Arguments.Add(packagePath);

                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() => reposignCommand.GetRepositorySignRequest());
                Assert.Equal(string.Format(InvalidArgException, nameof(reposignCommand.HashAlgorithm)), ex.Message);
            }
        }

        [Theory]
        [InlineData("sha256")]
        [InlineData("sha384")]
        [InlineData("sha512")]
        [InlineData("ShA256")]
        [InlineData("SHA256")]
        [InlineData("SHA384")]
        [InlineData("SHA512")]
        public void ReposignCommandArgParsing_ValidTimestampHashAlgorithm(string timestampHashAlgorithm)
        {
            using (var dir = TestDirectory.Create())
            {
                // Arrange
                var packagePath = Path.Combine(dir, "package.nupkg");
                var timestamper = "https://timestamper.test";
                var certFile = Path.Combine(dir, "cert.p7b");
                var certificateFingerprint = Sha256Hash;
                var keyContainer = new Guid().ToString();
                var cspName = "cert provider";
                var serviceIndex = "https://v3serviceindex.test/api/index.json";
                var parsable = Enum.TryParse(timestampHashAlgorithm, ignoreCase: true, result: out Common.HashAlgorithmName parsedHashAlgorithm);
                var mockConsole = new Mock<IConsole>();
                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestamper,
                    CertificateFile = certFile,
                    CertificateFingerprint = certificateFingerprint,
                    KeyContainer = keyContainer,
                    CSPName = cspName,
                    V3ServiceIndexUrl = serviceIndex,
                    TimestampHashAlgorithm = timestampHashAlgorithm
                };

                reposignCommand.Arguments.Add(packagePath);

                // Act & Assert
                Assert.True(parsable);
                var ex = Assert.Throws<CryptographicException>(() => reposignCommand.GetRepositorySignRequest());
                Assert.NotEqual(string.Format(InvalidArgException, nameof(reposignCommand.TimestampHashAlgorithm)), ex.Message);
            }
        }

        [Fact]
        public void ReposignCommandArgParsing_InvalidTimestampHashAlgorithm()
        {
            using (var dir = TestDirectory.Create())
            {
                // Arrange
                var packagePath = Path.Combine(dir, "package.nupkg");
                var timestamper = "https://timestamper.test";
                var certFile = Path.Combine(dir, "cert.p7b");
                var certificateFingerprint = Sha256Hash;
                var keyContainer = new Guid().ToString();
                var cspName = "cert provider";
                var serviceIndex = "https://v3serviceindex.test/api/index.json";
                var timestampHashAlgorithm = "MD5";
                var mockConsole = new Mock<IConsole>();

                var reposignCommand = new RepoSignCommand
                {
                    Console = mockConsole.Object,
                    Timestamper = timestamper,
                    CertificateFile = certFile,
                    CertificateFingerprint = certificateFingerprint,
                    KeyContainer = keyContainer,
                    CSPName = cspName,
                    V3ServiceIndexUrl = serviceIndex,
                    TimestampHashAlgorithm = timestampHashAlgorithm
                };

                reposignCommand.Arguments.Add(packagePath);

                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() => reposignCommand.GetRepositorySignRequest());
                Assert.Equal(string.Format(InvalidArgException, nameof(reposignCommand.TimestampHashAlgorithm)), ex.Message);
            }
        }
    }
}
