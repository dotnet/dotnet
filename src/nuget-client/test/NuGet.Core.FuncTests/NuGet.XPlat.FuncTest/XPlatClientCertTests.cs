// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NuGet.CommandLine.XPlat;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.XPlat.FuncTest
{
    [Collection("NuGet XPlat Test Collection")]
    public class XPlatClientCertTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public XPlatClientCertTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ClientCertAddCommand_Fail_CertificateSourceCombinationSpecified()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                var args = new[]
                {
                    "add",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    testInfo.PackageSourceName,
                    "--path",
                    testInfo.CertificateRelativeFilePath,
                    "--store-location",
                    testInfo.CertificateStoreLocation.ToString()
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                Assert.Contains("Invalid combination of arguments", log.ShowErrors());
                Assert.Equal(1, exitCode);
            }
        }

        [Fact]
        public void ClientCertAddCommand_Fail_CertificateSourceNotSpecified()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                var args = new[]
                {
                    "add",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    testInfo.PackageSourceName
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                var expectedError = "Invalid combination of arguments";
                Assert.Contains(expectedError, log.ShowErrors());
                Assert.Equal(1, exitCode);
            }
        }

        [Fact]
        public void ClientCertAddCommand_Fail_FileCertificateNotExist()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                var args = new[]
                {
                    "add",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    testInfo.PackageSourceName,
                    "--path",
                    @".\MyCertificate.pfx"
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                var expectedError = "A fileCert path specified a file that does not exist";
                Assert.Contains(expectedError, log.ShowErrors());
                Assert.Equal(1, exitCode);
            }
        }

        [Fact]
        public void ClientCertAddCommand_Fail_NoSourceSpecified()
        {
            // Arrange
            var args = new[] { "add", "client-cert" };

            var log = new TestCommandOutputLogger(_testOutputHelper);

            // Act
            var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

            // Assert
            var expectedError = "Property 'PackageSource' should not be null or empty";
            Assert.Contains(expectedError, log.ShowErrors());
            Assert.Equal(1, exitCode);
        }

        // Skip: https://github.com/NuGet/Home/issues/9684
        [PlatformFact(Platform.Windows, Platform.Linux, SkipMono = true)]
        public void ClientCertAddCommand_Fail_StoreCertificateNotExist()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                var args = new[]
                {
                    "add",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    testInfo.PackageSourceName,
                    "--find-value",
                    "SOME"
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                var expectedError = "was not found";
                Assert.Contains(expectedError, log.ShowErrors());
                Assert.Equal(1, exitCode);
            }
        }

        // Skip: https://github.com/NuGet/Home/issues/9684
        [PlatformFact(Platform.Windows, SkipMono = true)]
        public void ClientCertAddCommand_Success_FileCertificateAbsolute()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                testInfo.SetupCertificateFile();

                var args = new[]
                {
                    "add",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    testInfo.PackageSourceName,
                    "--path",
                    testInfo.CertificateAbsoluteFilePath,
                    "--password",
                    testInfo.CertificatePassword
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                //Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                Assert.Equal(string.Empty, log.ShowErrors());
                Assert.Equal(0, exitCode);
                Assert.Contains("was successfully added", log.ShowMessages());

                testInfo.ValidateSettings(new FileClientCertItem(testInfo.PackageSourceName, testInfo.CertificateAbsoluteFilePath, testInfo.CertificatePassword, false, testInfo.ConfigFile));
            }
        }

        // Skip: https://github.com/NuGet/Home/issues/9684
        [PlatformFact(Platform.Windows, SkipMono = true)]
        public void ClientCertAddCommand_Success_FileCertificateNotExistForce()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                var packageSource = testInfo.PackageSourceName;
                var path = "MyCertificate.pfx";
                var password = "42";

                var args = new[]
                {
                    "add",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    packageSource,
                    "--path",
                    path,
                    "--password",
                    password,
                    "--force"
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                Assert.Equal(string.Empty, log.ShowErrors());
                Assert.Equal(0, exitCode);
                Assert.Contains("was successfully added", log.ShowMessages());

                testInfo.ValidateSettings(new FileClientCertItem(packageSource, path, password, false, testInfo.ConfigFile));
            }
        }

        // Skip: https://github.com/NuGet/Home/issues/9684
        [PlatformFact(Platform.Windows, SkipMono = true)]
        public void ClientCertAddCommand_Success_FileCertificateRelative()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                testInfo.SetupCertificateFile();

                var packageSource = testInfo.PackageSourceName;
                var path = testInfo.CertificateRelativeFilePath;
                var password = testInfo.CertificatePassword;

                var args = new[]
                {
                    "add",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    packageSource,
                    "--path",
                    path,
                    "--password",
                    password
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                Assert.Equal(string.Empty, log.ShowErrors());
                Assert.Equal(0, exitCode);
                Assert.Contains("was successfully added", log.ShowMessages());

                testInfo.ValidateSettings(new FileClientCertItem(packageSource, path, password, false, testInfo.ConfigFile));
            }
        }

        // Skip: https://github.com/NuGet/Home/issues/9684
        [PlatformFact(Platform.Windows, Platform.Linux, SkipMono = true)]
        public void ClientCertAddCommand_Success_StoreCertificate()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                testInfo.SetupCertificateInStorage();

                var args = new[]
                {
                    "add",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    testInfo.PackageSourceName,
                    "--store-location",
                    testInfo.CertificateStoreLocation.ToString(),
                    "--store-name",
                    testInfo.CertificateStoreName.ToString(),
                    "--find-by",
                    testInfo.CertificateFindBy.ToString().Replace("FindBy", string.Empty),
                    "--find-value",
                    testInfo.CertificateFindValue.ToString()
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                Assert.Equal(string.Empty, log.ShowErrors());
                Assert.Equal(0, exitCode);
                Assert.Contains("was successfully added", log.ShowMessages());

                testInfo.ValidateSettings(new StoreClientCertItem(testInfo.PackageSourceName,
                                                                  testInfo.CertificateFindValue.ToString(),
                                                                  testInfo.CertificateStoreLocation,
                                                                  testInfo.CertificateStoreName,
                                                                  testInfo.CertificateFindBy));
            }
        }

        [Fact]
        public void ClientCertAddCommand_Success_StoreCertificateNotExistForce()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                var args = new[]
                {
                    "add",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    testInfo.PackageSourceName,
                    "--store-location",
                    testInfo.CertificateStoreLocation.ToString(),
                    "--store-name",
                    testInfo.CertificateStoreName.ToString(),
                    "--find-by",
                    testInfo.CertificateFindBy.ToString().Replace("FindBy", string.Empty),
                    "--find-value",
                    testInfo.CertificateFindValue.ToString(),
                    "--force"
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                Assert.Equal(string.Empty, log.ShowErrors());
                Assert.Equal(0, exitCode);
                Assert.Contains("was successfully added", log.ShowMessages());

                testInfo.ValidateSettings(new StoreClientCertItem(testInfo.PackageSourceName,
                                                                  testInfo.CertificateFindValue.ToString(),
                                                                  testInfo.CertificateStoreLocation,
                                                                  testInfo.CertificateStoreName,
                                                                  testInfo.CertificateFindBy));
            }
        }

        [Fact]
        public void ClientCertListCommand_Success_EmptyList()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                var args = new[]
                {
                    "list",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                Assert.Equal(string.Empty, log.ShowErrors());
                Assert.Equal(0, exitCode);
                Assert.Contains("There are no client certificates", log.ShowMessages());
            }
        }

        // Skip: https://github.com/NuGet/Home/issues/9684
        [PlatformFact(Platform.Windows, SkipMono = true)]
        public void ClientCertListCommand_Success_NotEmptyList()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                testInfo.SetupInitialItems(new FileClientCertItem(testInfo.PackageSourceName,
                                                                  testInfo.CertificateRelativeFilePath,
                                                                  testInfo.CertificatePassword,
                                                                  false,
                                                                  testInfo.ConfigFile),
                                           new StoreClientCertItem(testInfo.InvalidPackageSourceName,
                                                                   testInfo.CertificateFindValue.ToString(),
                                                                   testInfo.CertificateStoreLocation,
                                                                   testInfo.CertificateStoreName,
                                                                   testInfo.CertificateFindBy));

                var args = new[]
                {
                    "list",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                Assert.Equal(string.Empty, log.ShowErrors());
                Assert.Equal(0, exitCode);
                var showMessages = log.ShowMessages();
                Assert.Contains($"{testInfo.PackageSourceName} [{ConfigurationConstants.FileCertificate}]", showMessages);
                Assert.Contains($"{testInfo.InvalidPackageSourceName} [{ConfigurationConstants.StoreCertificate}]", showMessages);
            }
        }

        [Fact]
        public void ClientCertRemoveCommand_Failed_NotExist()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                var args = new[]
                {
                    "remove",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    testInfo.PackageSourceName
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                Assert.Equal(string.Empty, log.ShowErrors());
                Assert.Equal(0, exitCode);
                Assert.Contains("There are no client certificates configured for", log.ShowMessages());

                testInfo.ValidateSettings();
            }
        }

        // Skip: https://github.com/NuGet/Home/issues/9684
        [PlatformFact(Platform.Windows, SkipMono = true)]
        public void ClientCertRemoveCommand_Success_ItemCertificate()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                testInfo.SetupInitialItems(new FileClientCertItem(testInfo.PackageSourceName,
                                                                  testInfo.CertificateAbsoluteFilePath,
                                                                  testInfo.CertificatePassword,
                                                                  false,
                                                                  testInfo.ConfigFile));
                var args = new[]
                {
                    "remove",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    testInfo.PackageSourceName
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                Assert.Equal(string.Empty, log.ShowErrors());
                Assert.Equal(0, exitCode);
                Assert.Contains("was successfully removed", log.ShowMessages());

                testInfo.ValidateSettings();
            }
        }

        [Fact]
        public void ClientCertUpdateCommand_Fail_CertificateSourceCombinationSpecified()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                var args = new[]
                {
                    "update",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    testInfo.PackageSourceName,
                    "--path",
                    testInfo.CertificateAbsoluteFilePath,
                    "--store-location",
                    testInfo.CertificateStoreLocation.ToString()
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                var expectedError = "Invalid combination of arguments";
                Assert.Contains(expectedError, log.ShowErrors());
                Assert.Equal(1, exitCode);
            }
        }

        [Fact]
        public void ClientCertUpdateCommand_Fail_CertificateSourceNotFound()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                var args = new[]
                {
                    "update",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    testInfo.InvalidPackageSourceName,
                    "--path",
                    testInfo.CertificateAbsoluteFilePath,
                    "--password",
                    testInfo.CertificatePassword
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                var expectedError = "does not exist";
                Assert.Contains(expectedError, log.ShowErrors());
                Assert.Equal(1, exitCode);
            }
        }

        [Fact]
        public void ClientCertUpdateCommand_Fail_CertificateSourceNotSpecified()
        {
            // Arrange
            using (var testInfo = new TestInfo())
            {
                var args = new[]
                {
                    "update",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    testInfo.PackageSourceName
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                var expectedError = "Invalid combination of arguments";
                Assert.Contains(expectedError, log.ShowErrors());
                Assert.Equal(1, exitCode);
            }
        }

        // Skip: https://github.com/NuGet/Home/issues/9684
        [PlatformFact(Platform.Windows, SkipMono = true)]
        public void ClientCertUpdateCommand_Success_FileCertificateForce()
        {
            var updatedPath = "MyCertificateSecond.pfx";
            var updatedPassword = "42";

            // Arrange
            using (var testInfo = new TestInfo())
            {
                testInfo.SetupInitialItems(new FileClientCertItem(testInfo.PackageSourceName,
                                                                  testInfo.CertificateAbsoluteFilePath,
                                                                  testInfo.CertificatePassword,
                                                                  false,
                                                                  testInfo.ConfigFile));
                var args = new[]
                {
                    "update",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    testInfo.PackageSourceName,
                    "--path",
                    updatedPath,
                    "--password",
                    updatedPassword,
                    "--force"
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                Assert.Equal(string.Empty, log.ShowErrors());
                Assert.Equal(0, exitCode);
                Assert.Contains("was successfully updated", log.ShowMessages());

                testInfo.ValidateSettings(new FileClientCertItem(testInfo.PackageSourceName,
                                                                 updatedPath,
                                                                 updatedPassword,
                                                                 false,
                                                                 testInfo.ConfigFile));
            }
        }

        [Fact]
        public void ClientCertUpdateCommand_Success_StoreCertificateForce()
        {
            var updatedStoreLocation = StoreLocation.CurrentUser;
            var updatedStoreName = StoreName.AuthRoot;
            var updatedFindBy = X509FindType.FindByCertificatePolicy;
            var updatedFindValue = "SOMEUpdated";

            // Arrange
            using (var testInfo = new TestInfo())
            {
                testInfo.SetupInitialItems(new StoreClientCertItem(testInfo.PackageSourceName,
                                                                   testInfo.CertificateFindValue.ToString(),
                                                                   testInfo.CertificateStoreLocation,
                                                                   testInfo.CertificateStoreName,
                                                                   testInfo.CertificateFindBy));
                var args = new[]
                {
                    "update",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    testInfo.PackageSourceName,
                    "--store-location",
                    updatedStoreLocation.ToString(),
                    "--store-name",
                    updatedStoreName.ToString(),
                    "--find-by",
                    updatedFindBy.ToString().Replace("FindBy", string.Empty),
                    "--find-value",
                    updatedFindValue,
                    "--force"
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                Assert.Equal(string.Empty, log.ShowErrors());
                Assert.Equal(0, exitCode);
                Assert.Contains("was successfully updated", log.ShowMessages());

                testInfo.ValidateSettings(new StoreClientCertItem(testInfo.PackageSourceName,
                                                                  updatedFindValue,
                                                                  updatedStoreLocation,
                                                                  updatedStoreName,
                                                                  updatedFindBy));
            }
        }

        // Skip: https://github.com/NuGet/Home/issues/9684
        [PlatformFact(Platform.Windows, SkipMono = true)]
        public void ClientCertUpdatedCommand_Fail_FileCertificateNotExist()
        {
            var updatedPath = "MyCertificateSecond.pfx";
            var updatedPassword = "42";

            // Arrange
            using (var testInfo = new TestInfo())
            {
                testInfo.SetupInitialItems(new FileClientCertItem(testInfo.PackageSourceName,
                                                                  testInfo.CertificateAbsoluteFilePath,
                                                                  testInfo.CertificatePassword,
                                                                  false,
                                                                  testInfo.ConfigFile));

                var args = new[]
                {
                    "update",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    testInfo.PackageSourceName,
                    "--path",
                    updatedPath,
                    "--password",
                    updatedPassword
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                var expectedError = "A fileCert path specified a file that does not exist";
                Assert.Contains(expectedError, log.ShowErrors());
                Assert.Equal(1, exitCode);
            }
        }

        [Fact]
        public void ClientCertUpdatedCommand_Fail_NotConfigured()
        {
            var updatedPath = "MyCertificateSecond.pfx";
            var updatedPassword = "42";

            // Arrange
            using (var testInfo = new TestInfo())
            {
                var args = new[]
                {
                    "update",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    testInfo.PackageSourceName,
                    "--path",
                    updatedPath,
                    "--password",
                    updatedPassword
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                var expectedError = "does not exist";
                Assert.Contains(expectedError, log.ShowErrors());
                Assert.Equal(1, exitCode);
            }
        }

        // Skip: https://github.com/NuGet/Home/issues/9684
        [PlatformFact(Platform.Windows, Platform.Linux, SkipMono = true)]
        public void ClientCertUpdatedCommand_Fail_StoreCertificateNotExist()
        {
            var updatedStoreLocation = StoreLocation.CurrentUser;
            var updatedStoreName = StoreName.My;
            var updatedFindBy = X509FindType.FindByIssuerName;
            var updatedFindValue = "SOMEUpdated";

            // Arrange
            using (var testInfo = new TestInfo())
            {
                testInfo.SetupInitialItems(new StoreClientCertItem(testInfo.PackageSourceName,
                                                                   testInfo.CertificateFindValue.ToString(),
                                                                   testInfo.CertificateStoreLocation,
                                                                   testInfo.CertificateStoreName,
                                                                   testInfo.CertificateFindBy));

                var args = new[]
                {
                    "update",
                    "client-cert",
                    "--configfile",
                    testInfo.ConfigFile,
                    "--package-source",
                    testInfo.PackageSourceName,
                    "--store-location",
                    updatedStoreLocation.ToString(),
                    "--store-name",
                    updatedStoreName.ToString(),
                    "--find-by",
                    updatedFindBy.ToString().Replace("FindBy", string.Empty),
                    "--find-value",
                    updatedFindValue
                };

                var log = new TestCommandOutputLogger(_testOutputHelper);

                // Act
                var exitCode = Program.MainInternal(args.ToArray(), log, TestEnvironmentVariableReader.EmptyInstance);

                // Assert
                var expectedError = "was not found";
                Assert.Contains(expectedError, log.ShowErrors());
                Assert.Equal(1, exitCode);
            }
        }

        internal class TestInfo : IDisposable
        {
            public static void CreateFile(string directory, string fileName, string fileContent)
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var fileFullName = Path.Combine(directory, fileName);
                CreateFile(fileFullName, fileContent);
            }

            public static void CreateFile(string fileFullName, string fileContent)
            {
                using (var writer = new StreamWriter(fileFullName))
                {
                    writer.Write(fileContent);
                }
            }

            private static CollectionCompareResult<TFirst, TSecond> Compare<TFirst, TSecond>(IEnumerable<TFirst> first, IEnumerable<TSecond> second)
            {
                return Compare(first, second, null, null);
            }

            private static CollectionCompareResult<TFirst, TSecond> Compare<TFirst, TSecond>(IEnumerable<TFirst> first,
                                                                                             IEnumerable<TSecond> second,
                                                                                             Func<TFirst, int> firstHasher,
                                                                                             Func<TSecond, int> secondHasher)
            {
                firstHasher = firstHasher ?? (i => i.GetHashCode());
                secondHasher = secondHasher ?? (i => i.GetHashCode());

                var firstSet = first.Distinct().ToLookup(v => firstHasher(v), v => v).ToDictionary(g => g.Key, g => g.First());
                var secondSet = second.Distinct().ToLookup(v => secondHasher(v), v => v).ToDictionary(g => g.Key, g => g.First());
                return new CollectionCompareResult<TFirst, TSecond>
                {
                    PresentInFirstOnly = firstSet.Keys.Except(secondSet.Keys).Select(k => firstSet[k]).ToList(),
                    PresentInSecondOnly = secondSet.Keys.Except(firstSet.Keys).Select(k => secondSet[k]).ToList(),
                    PresentInBoth = firstSet.Keys.Intersect(secondSet.Keys).Select(k => new Tuple<TFirst, TSecond>(firstSet[k], secondSet[k])).ToList()
                };
            }

            public TestInfo()
            {
                WorkingPath = TestDirectory.Create();
                ConfigFile = Path.Combine(WorkingPath, "NuGet.config");
                PackageSourceName = "Foo";
                InvalidPackageSourceName = "Bar";
                CertificateFileName = "contoso.pfx";
                CertificateAbsoluteFilePath = Path.Combine(WorkingPath, CertificateFileName);
                CertificateRelativeFilePath = ".\\" + CertificateFileName;
                CertificatePassword = "password";

                CertificateStoreLocation = StoreLocation.CurrentUser;
                CertificateStoreName = StoreName.My;
                CertificateFindBy = X509FindType.FindByIssuerName;
                CertificateFindValue = "Contoso";

                CreateFile(Path.GetDirectoryName(ConfigFile),
                           Path.GetFileName(ConfigFile),
                           $@"
<configuration>
    <packageSources>
        <add key=""{PackageSourceName}"" value=""https://contoso.com/v3/index.json"" />
    </packageSources>
</configuration>
");
            }

            public X509Certificate2 Certificate { get; private set; }

            public string CertificateAbsoluteFilePath { get; }
            public string CertificateFileName { get; }
            public X509FindType CertificateFindBy { get; }
            public object CertificateFindValue { get; }
            public string CertificatePassword { get; }
            public string CertificateRelativeFilePath { get; }
            public StoreLocation CertificateStoreLocation { get; }
            public StoreName CertificateStoreName { get; }

            public string ConfigFile { get; }
            public string InvalidPackageSourceName { get; }

            public string PackageSourceName { get; }
            public TestDirectory WorkingPath { get; }

            public void Dispose()
            {
                WorkingPath.Dispose();
                RemoveCertificateFromStorage();
                Certificate?.Dispose();
            }

            public void SetupCertificateFile()
            {
                var certificateData = CreateCertificate();
                File.WriteAllBytes(CertificateAbsoluteFilePath, certificateData);
            }

            public void SetupCertificateInStorage()
            {
                if (Certificate is not null)
                {
                    return;
                }

                using (var store = new X509Store(CertificateStoreName, CertificateStoreLocation))
                {
                    store.Open(OpenFlags.ReadWrite);
                    Certificate = X509CertificateLoader.LoadPkcs12(CreateCertificate(), CertificatePassword, X509KeyStorageFlags.Exportable);
                    store.Add(Certificate);
                }
            }

            public void SetupInitialItems(params ClientCertItem[] initialItems)
            {
                var settings = LoadSettingsFromConfigFile();
                var clientCertificateProvider = new ClientCertificateProvider(settings);
                foreach (ClientCertItem item in initialItems)
                {
                    clientCertificateProvider.AddOrUpdate(item);
                }

                settings.SaveToDisk();
            }

            public void ValidateSettings(params ClientCertItem[] expectedItems)
            {
                var settings = LoadSettingsFromConfigFile();
                var clientCertificateProvider = new ClientCertificateProvider(settings);
                var existingItems = clientCertificateProvider.GetClientCertificates();

                var comparison = Compare(expectedItems, existingItems);
                Assert.Empty(comparison.PresentInFirstOnly);
                Assert.Empty(comparison.PresentInSecondOnly);

                foreach (Tuple<ClientCertItem, ClientCertItem> tuple in comparison.PresentInBoth)
                {
                    var expectedItem = tuple.Item1;
                    var existItem = tuple.Item2;
                    Assert.Equal(expectedItem.GetType(), existItem.GetType());
                    if (expectedItem is FileClientCertItem expectedFileClientCertItem && existItem is FileClientCertItem existFileClientCertItem)
                    {
                        Assert.Equal(expectedFileClientCertItem.FilePath, existFileClientCertItem.FilePath);
                        Assert.Equal(expectedFileClientCertItem.Password, existFileClientCertItem.Password);
                        Assert.Equal(expectedFileClientCertItem.IsPasswordIsClearText, existFileClientCertItem.IsPasswordIsClearText);
                    }
                    else if (expectedItem is StoreClientCertItem expectedStoreClientCertItem && existItem is StoreClientCertItem existStoreClientCertItem)
                    {
                        Assert.Equal(expectedStoreClientCertItem.StoreLocation, existStoreClientCertItem.StoreLocation);
                        Assert.Equal(expectedStoreClientCertItem.StoreName, existStoreClientCertItem.StoreName);
                        Assert.Equal(expectedStoreClientCertItem.FindType, existStoreClientCertItem.FindType);
                        Assert.Equal(expectedStoreClientCertItem.FindValue, existStoreClientCertItem.FindValue);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }
            }

            private byte[] CreateCertificate()
            {
                using (RSA rsa = RSA.Create(2048))
                {
                    var request = new CertificateRequest("cn=" + CertificateFindValue, rsa, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
                    var start = DateTime.UtcNow.AddMinutes(-1);
                    var end = start.AddMinutes(10);

                    using (X509Certificate2 cert = request.CreateSelfSigned(start, end))
                    {
                        return cert.Export(X509ContentType.Pfx, CertificatePassword);
                    }
                }
            }

            private ISettings LoadSettingsFromConfigFile()
            {
                var directory = Path.GetDirectoryName(ConfigFile);
                var filename = Path.GetFileName(ConfigFile);
                return Settings.LoadSpecificSettings(directory, filename);
            }

            private void RemoveCertificateFromStorage()
            {
                if (Certificate is null)
                {
                    return;
                }

                using (var store = new X509Store(CertificateStoreName, CertificateStoreLocation))
                {
                    store.Open(OpenFlags.ReadWrite);

                    X509Certificate2Collection resultCertificates = store.Certificates.Find(
                        X509FindType.FindByIssuerDistinguishedName,
                        Certificate.Issuer,
                        validOnly: false);

                    foreach (X509Certificate2 resultCertificate in resultCertificates)
                    {
                        store.Remove(resultCertificate);
                    }
                }
            }

            public class CollectionCompareResult<TFirst, TSecond>
            {
                public IReadOnlyCollection<Tuple<TFirst, TSecond>> PresentInBoth { get; internal set; }
                public IReadOnlyCollection<TFirst> PresentInFirstOnly { get; internal set; }
                public IReadOnlyCollection<TSecond> PresentInSecondOnly { get; internal set; }
            }
        }
    }
}
