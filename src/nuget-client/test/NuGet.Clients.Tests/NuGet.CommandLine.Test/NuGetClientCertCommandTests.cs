// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace NuGet.CommandLine.Test
{
    public class NuGetClientCertCommandTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public NuGetClientCertCommandTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ClientCertAddCommand_Fail_CertificateSourceCombinationSpecified()
        {
            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                var args = new[]
                {
                    "client-certs",
                    "add",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    testInfo.PackageSourceName,
                    "-Path",
                    testInfo.CertificateRelativeFilePath,
                    "-StoreLocation",
                    testInfo.CertificateStoreLocation.ToString()
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args.Select(a => $"\"{a}\"")));

                // Assert
                var expectedError = "Invalid combination of arguments";
                Assert.True(result.Output.Contains(expectedError));
            }
        }

        [Fact]
        public void ClientCertAddCommand_Fail_CertificateSourceNotSpecified()
        {
            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                var args = new[]
                {
                    "client-certs",
                    "add",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    testInfo.PackageSourceName
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args.Select(a => $"\"{a}\"")));

                // Assert
                var expectedError = "Invalid combination of arguments";
                Assert.True(result.Output.Contains(expectedError));
            }
        }

        [Fact]
        public void ClientCertAddCommand_Fail_FileCertificateNotExist()
        {
            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                var args = new[]
                {
                    "client-certs",
                    "add",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    testInfo.PackageSourceName,
                    "-Path",
                    @".\MyCertificate.pfx"
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args.Select(a => $"\"{a}\"")));

                // Assert
                var expectedError = "file that does not exist";
                Util.VerifyResultFailure(result, expectedError);
            }
        }

        [Fact]
        public void ClientCertAddCommand_Fail_NoSourceSpecified()
        {
            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                var args = new[] { "client-certs", "add" };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args.Select(a => $"\"{a}\"")));

                // Assert
                var expectedError = "Property 'PackageSource' should not be null or empty";
                Assert.True(result.Output.Contains(expectedError));
            }
        }

        [Fact]
        public void ClientCertAddCommand_Fail_StoreCertificateNotExist()
        {
            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                var args = new[]
                {
                    "client-certs",
                    "add",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    testInfo.PackageSourceName,
                    "-FindValue",
                    "SOME"
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args.Select(a => $"\"{a}\"")));

                // Assert
                var expectedError = "was not found";
                Util.VerifyResultFailure(result, expectedError);
            }
        }

        [PlatformFact(Platform.Windows, SkipMono = true)]
        public void ClientCertAddCommand_Success_FileCertificateAbsolute()
        {
            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                testInfo.SetupCertificateFile();

                var packageSource = testInfo.PackageSourceName;
                var path = testInfo.CertificateAbsoluteFilePath;
                var password = testInfo.CertificatePassword;

                var args = new[]
                {
                    "client-certs",
                    "add",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    packageSource,
                    "-Path",
                    path,
                    "-Password",
                    password
                };

                // Act
                testInfo.RunNuGetExpectSuccess(args, expectedOutput: "was successfully added");

                // Assert
                testInfo.ValidateSettings(new FileClientCertItem(packageSource, path, password, false, testInfo.ConfigFile));
            }
        }

        [Fact]
        public void ClientCertAddCommand_Success_FileCertificateNotExistForce()
        {
            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                var packageSource = testInfo.PackageSourceName;
                var path = "MyCertificate.pfx";
                var password = "42";

                var args = new[]
                {
                    "client-certs",
                    "add",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    packageSource,
                    "-Path",
                    path,
                    "-Password",
                    password,
                    "-Force"
                };

                // Act
                testInfo.RunNuGetExpectSuccess(args, expectedOutput: "was successfully added");

                // Assert
                testInfo.ValidateSettings(new FileClientCertItem(packageSource, path, password, false, testInfo.ConfigFile));
            }
        }

        [PlatformFact(Platform.Windows, SkipMono = true)]
        public void ClientCertAddCommand_Success_FileCertificateRelative()
        {
            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                testInfo.SetupCertificateFile();

                var packageSource = testInfo.PackageSourceName;
                var path = testInfo.CertificateRelativeFilePath;
                var password = testInfo.CertificatePassword;

                var args = new[]
                {
                    "client-certs",
                    "add",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    packageSource,
                    "-Path",
                    path,
                    "-Password",
                    password
                };

                // Act
                testInfo.RunNuGetExpectSuccess(args, expectedOutput: "was successfully added");

                // Assert
                testInfo.ValidateSettings(new FileClientCertItem(packageSource, path, password, false, testInfo.ConfigFile));
            }
        }

        [PlatformFact(Platform.Windows, Platform.Linux, SkipMono = true)]
        public void ClientCertAddCommand_Success_StoreCertificate()
        {
            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                testInfo.SetupCertificateInStorage();

                var args = new[]
                {
                    "client-certs",
                    "add",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    testInfo.PackageSourceName,
                    "-StoreLocation",
                    testInfo.CertificateStoreLocation.ToString(),
                    "-StoreName",
                    testInfo.CertificateStoreName.ToString(),
                    "-FindBy",
                    testInfo.CertificateFindBy.ToString().Replace("FindBy", string.Empty),
                    "-FindValue",
                    testInfo.CertificateFindValue.ToString()
                };

                // Act
                testInfo.RunNuGetExpectSuccess(args, expectedOutput: "was successfully added");

                // Assert
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
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                var args = new[]
                {
                    "client-certs",
                    "add",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    testInfo.PackageSourceName,
                    "-StoreLocation",
                    testInfo.CertificateStoreLocation.ToString(),
                    "-StoreName",
                    testInfo.CertificateStoreName.ToString(),
                    "-FindBy",
                    testInfo.CertificateFindBy.ToString().Replace("FindBy", string.Empty),
                    "-FindValue",
                    testInfo.CertificateFindValue.ToString(),
                    "-Force"
                };

                // Act
                testInfo.RunNuGetExpectSuccess(args, expectedOutput: "was successfully added");

                // Assert
                testInfo.ValidateSettings(new StoreClientCertItem(testInfo.PackageSourceName,
                                                                  testInfo.CertificateFindValue.ToString(),
                                                                  testInfo.CertificateStoreLocation,
                                                                  testInfo.CertificateStoreName,
                                                                  testInfo.CertificateFindBy));
            }
        }

        [Fact]
        public void ClientCertDefaultCommand_Success_NotEmpty()
        {
            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                var args = new[]
                {
                    "client-certs",
                    "-ConfigFile",
                    testInfo.ConfigFile
                };

                // Act & Assert
                testInfo.RunNuGetExpectSuccess(args, expectedOutput: "There are no client certificates");
            }
        }

        [Fact]
        public void ClientCertListCommand_Success_EmptyList()
        {
            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                var args = new[]
                {
                    "client-certs",
                    "list",
                    "-ConfigFile",
                    testInfo.ConfigFile
                };

                // Act & Assert
                testInfo.RunNuGetExpectSuccess(args, expectedOutput: "There are no client certificates");
            }
        }

        [Fact]
        public void ClientCertListCommand_Success_NotEmptyList()
        {
            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                testInfo.SetupInitialItems(new FileClientCertItem(testInfo.PackageSourceName,
                                                                  testInfo.CertificateRelativeFilePath,
                                                                  testInfo.CertificatePassword,
                                                                  false,
                                                                  testInfo.ConfigFile));

                var args = new[]
                {
                    "client-certs",
                    "list",
                    "-ConfigFile",
                    testInfo.ConfigFile
                };

                // Act & Assert
                testInfo.RunNuGetExpectSuccess(args, expectedOutput: $"{testInfo.PackageSourceName} [fileCert]");
            }
        }

        [Fact]
        public void ClientCertRemoveCommand_Failed_NotExist()
        {
            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                var args = new[]
                {
                    "client-certs",
                    "remove",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    testInfo.PackageSourceName
                };

                // Act
                testInfo.RunNuGetExpectSuccess(args, expectedOutput: "There are no client certificates configured for");
                // Assert
                testInfo.ValidateSettings();
            }
        }

        [Fact]
        public void ClientCertRemoveCommand_Success_ItemCertificate()
        {
            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                testInfo.SetupInitialItems(new FileClientCertItem(testInfo.PackageSourceName,
                                                                  testInfo.CertificateAbsoluteFilePath,
                                                                  testInfo.CertificatePassword,
                                                                  false,
                                                                  testInfo.ConfigFile));
                var args = new[]
                {
                    "client-certs",
                    "remove",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    testInfo.PackageSourceName
                };

                // Act
                testInfo.RunNuGetExpectSuccess(args, expectedOutput: "was successfully removed");

                // Assert
                testInfo.ValidateSettings();
            }
        }

        [Fact]
        public void ClientCertUpdateCommand_Fail_CertificateSourceCombinationSpecified()
        {
            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                var args = new[]
                {
                    "client-certs",
                    "update",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    testInfo.PackageSourceName,
                    "-Path",
                    testInfo.CertificateAbsoluteFilePath,
                    "-StoreLocation",
                    testInfo.CertificateStoreLocation.ToString()
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args.Select(a => $"\"{a}\"")));

                // Assert
                var expectedError = "Invalid combination of arguments";
                Assert.True(result.Output.Contains(expectedError));
            }
        }

        [Fact]
        public void ClientCertUpdateCommand_Fail_CertificateSourceNotFound()
        {
            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                var args = new[]
                {
                    "client-certs",
                    "update",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    testInfo.InvalidPackageSourceName,
                    "-Path",
                    testInfo.CertificateAbsoluteFilePath,
                    "-Password",
                    testInfo.CertificatePassword
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args.Select(a => $"\"{a}\"")));

                // Assert
                var expectedError = "does not exist";
                Assert.True(result.Errors.Contains(expectedError));
            }
        }

        [Fact]
        public void ClientCertUpdateCommand_Fail_CertificateSourceNotSpecified()
        {
            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                var args = new[]
                {
                    "client-certs",
                    "update",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    testInfo.PackageSourceName
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args.Select(a => $"\"{a}\"")));

                // Assert
                var expectedError = "Invalid combination of arguments";
                Assert.True(result.Output.Contains(expectedError));
            }
        }

        [Fact]
        public void ClientCertUpdateCommand_Success_FileCertificateForce()
        {
            var updatedPath = "MyCertificateSecond.pfx";
            var updatedPassword = "42";

            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                testInfo.SetupInitialItems(new FileClientCertItem(testInfo.PackageSourceName,
                                                                  testInfo.CertificateAbsoluteFilePath,
                                                                  testInfo.CertificatePassword,
                                                                  false,
                                                                  testInfo.ConfigFile));
                var args = new[]
                {
                    "client-certs",
                    "update",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    testInfo.PackageSourceName,
                    "-Path",
                    updatedPath,
                    "-Password",
                    updatedPassword,
                    "-Force"
                };

                // Act
                testInfo.RunNuGetExpectSuccess(args, expectedOutput: "was successfully updated");

                // Assert
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
            var updatedStoreName = StoreName.My;
            var updatedFindBy = X509FindType.FindByCertificatePolicy;
            var updatedFindValue = "SOMEUpdated";

            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                testInfo.SetupInitialItems(new StoreClientCertItem(testInfo.PackageSourceName,
                                                                   testInfo.CertificateFindValue.ToString(),
                                                                   testInfo.CertificateStoreLocation,
                                                                   testInfo.CertificateStoreName,
                                                                   testInfo.CertificateFindBy));
                var args = new[]
                {
                    "client-certs",
                    "update",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    testInfo.PackageSourceName,
                    "-StoreLocation",
                    updatedStoreLocation.ToString(),
                    "-StoreName",
                    updatedStoreName.ToString(),
                    "-FindBy",
                    updatedFindBy.ToString().Replace("FindBy", string.Empty),
                    "-FindValue",
                    updatedFindValue,
                    "-Force"
                };

                // Act
                testInfo.RunNuGetExpectSuccess(args, expectedOutput: "was successfully updated");

                // Assert
                testInfo.ValidateSettings(new StoreClientCertItem(testInfo.PackageSourceName,
                                                                  updatedFindValue,
                                                                  updatedStoreLocation,
                                                                  updatedStoreName,
                                                                  updatedFindBy));
            }
        }

        [Fact]
        public void ClientCertUpdatedCommand_Fail_FileCertificateNotExist()
        {
            var updatedPath = "MyCertificateSecond.pfx";
            var updatedPassword = "42";

            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                testInfo.SetupInitialItems(new FileClientCertItem(testInfo.PackageSourceName,
                                                                  testInfo.CertificateAbsoluteFilePath,
                                                                  testInfo.CertificatePassword,
                                                                  false,
                                                                  testInfo.ConfigFile));

                var args = new[]
                {
                    "client-certs",
                    "update",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    testInfo.PackageSourceName,
                    "-Path",
                    updatedPath,
                    "-Password",
                    updatedPassword
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args.Select(a => $"\"{a}\"")));

                // Assert
                var expectedError = "A fileCert path specified a file that does not exist";
                Util.VerifyResultFailure(result, expectedError);
            }
        }

        [Fact]
        public void ClientCertUpdatedCommand_Fail_NotConfigured()
        {
            var updatedPath = "MyCertificateSecond.pfx";
            var updatedPassword = "42";

            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                var args = new[]
                {
                    "client-certs",
                    "update",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    testInfo.PackageSourceName,
                    "-Path",
                    updatedPath,
                    "-Password",
                    updatedPassword
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args.Select(a => $"\"{a}\"")));

                // Assert
                var expectedError = "does not exist";
                Util.VerifyResultFailure(result, expectedError);
            }
        }

        [Fact]
        public void ClientCertUpdatedCommand_Fail_StoreCertificateNotExist()
        {
            var updatedStoreLocation = StoreLocation.CurrentUser;
            var updatedStoreName = StoreName.AuthRoot;
            var updatedFindBy = X509FindType.FindByIssuerName;
            var updatedFindValue = "SOMEUpdated";

            // Arrange
            using (var testInfo = new TestInfo(_testOutputHelper))
            {
                testInfo.SetupInitialItems(new StoreClientCertItem(testInfo.PackageSourceName,
                                                                   testInfo.CertificateFindValue.ToString(),
                                                                   testInfo.CertificateStoreLocation,
                                                                   testInfo.CertificateStoreName,
                                                                   testInfo.CertificateFindBy));

                var args = new[]
                {
                    "client-certs",
                    "update",
                    "-ConfigFile",
                    testInfo.ConfigFile,
                    "-PackageSource",
                    testInfo.PackageSourceName,
                    "-StoreLocation",
                    updatedStoreLocation.ToString(),
                    "-StoreName",
                    updatedStoreName.ToString(),
                    "-FindBy",
                    updatedFindBy.ToString().Replace("FindBy", string.Empty),
                    "-FindValue",
                    updatedFindValue
                };

                // Act
                var result = CommandRunner.Run(
                    testInfo.NuGetExePath,
                    testInfo.WorkingPath,
                    string.Join(" ", args.Select(a => $"\"{a}\"")));

                // Assert
                var expectedError = "was not found";
                Util.VerifyResultFailure(result, expectedError);
            }
        }

        private sealed class TestInfo : IDisposable
        {
            private readonly ITestOutputHelper _testOutputHelper;

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

            public TestInfo(ITestOutputHelper testOutputHelper)
            {
                _testOutputHelper = testOutputHelper;

                NuGetExePath = Util.GetNuGetExePath();
                WorkingPath = TestDirectory.Create();
                ConfigFile = Path.Combine(WorkingPath, "NuGet.config");
                PackageSourceName = $"Contoso-{Guid.NewGuid():N}";
                InvalidPackageSourceName = "Bar";
                CertificateFileName = PackageSourceName + ".pfx";
                CertificateAbsoluteFilePath = Path.Combine(WorkingPath, CertificateFileName);
                CertificateRelativeFilePath = ".\\" + CertificateFileName;
                CertificatePassword = "password";

                CertificateStoreLocation = StoreLocation.CurrentUser;
                CertificateStoreName = StoreName.My;
                CertificateFindBy = X509FindType.FindByIssuerName;
                CertificateFindValue = PackageSourceName;

                Util.CreateFile(Path.GetDirectoryName(ConfigFile),
                                Path.GetFileName(ConfigFile),
                                $@"
<configuration>
    <packageSources>
        <add key=""{PackageSourceName}"" value=""https://contoso.com/v3/index.json"" />
    </packageSources>
</configuration>
");
            }

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
            public string NuGetExePath { get; }

            public string PackageSourceName { get; }
            public TestDirectory WorkingPath { get; }

            public void RunNuGetExpectSuccess(string[] args, string expectedOutput = null)
            {
                string arguments = string.Join(" ", args.Select(i => i.StartsWith("-") ? i : $"\"{i}\""));

                _testOutputHelper.WriteLine("Running command {0} {1}", NuGetExePath, arguments);

                CommandRunnerResult result = CommandRunner.Run(
                    NuGetExePath,
                    WorkingPath,
                    arguments);

                _testOutputHelper.WriteLine("Exit code: {0}", result.ExitCode);

                if (!result.Success)
                {
                    _testOutputHelper.WriteLine("Output:");
                    _testOutputHelper.WriteLine(result.Output);
                    _testOutputHelper.WriteLine("Errors:");
                    _testOutputHelper.WriteLine(result.Errors);
                }

                LogInstalledCertificates();


                Util.VerifyResultSuccess(result, expectedOutput);
            }

            public void Dispose()
            {
                WorkingPath.Dispose();
                RemoveCertificateFromStorage();
            }

            public void SetupCertificateFile()
            {
                var certificateData = CreateCertificate();
                File.WriteAllBytes(CertificateAbsoluteFilePath, certificateData);

                _testOutputHelper.WriteLine("Wrote certificate to file {0}", CertificateAbsoluteFilePath);
            }

            public void SetupCertificateInStorage()
            {
                using (var store = new X509Store(CertificateStoreName, CertificateStoreLocation))
                {
                    store.Open(OpenFlags.ReadWrite);
                    var password = new SecureString();
                    foreach (var symbol in CertificatePassword)
                    {
                        password.AppendChar(symbol);
                    }

                    var cert = new X509Certificate2(CreateCertificate(), password, X509KeyStorageFlags.Exportable);
                    store.Add(cert);

                    _testOutputHelper.WriteLine("Added certificate {0} to store {1}\\{2}", cert.Subject, CertificateStoreLocation, CertificateStoreName);
                }

                LogInstalledCertificates();
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

            private void LogInstalledCertificates()
            {
                _testOutputHelper.WriteLine("Installed Certificates:");
                using (var store = new X509Store(CertificateStoreName, CertificateStoreLocation))
                {
                    store.Open(OpenFlags.ReadOnly);

                    foreach (X509Certificate2 cert in store.Certificates)
                    {
                        _testOutputHelper.WriteLine("  {0}", cert.Subject);
                    }
                }
            }

            private byte[] CreateCertificate()
            {
                var rsa = RSA.Create(2048);
                var request = new CertificateRequest("cn=" + CertificateFindValue, rsa, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
                var start = DateTime.UtcNow.AddDays(-1);
                var end = start.AddYears(1);

                var cert = request.CreateSelfSigned(start, end);

                _testOutputHelper.WriteLine("Created certificate {0}", request.SubjectName.Name);

                return cert.Export(X509ContentType.Pfx, CertificatePassword);
            }

            private Configuration.ISettings LoadSettingsFromConfigFile()
            {
                var directory = Path.GetDirectoryName(ConfigFile);
                var filename = Path.GetFileName(ConfigFile);
                return Configuration.Settings.LoadSpecificSettings(directory, filename);
            }

            private void RemoveCertificateFromStorage()
            {
                bool certificateRemoved = false;

                using (var store = new X509Store(CertificateStoreName, CertificateStoreLocation))
                {
                    store.Open(OpenFlags.ReadWrite);
                    var resultCertificates = store.Certificates.Find(CertificateFindBy, CertificateFindValue, false);
                    foreach (var certificate in resultCertificates)
                    {
                        _testOutputHelper.WriteLine("Removing certificate {0} from store {1}\\{2}", certificate.Subject, CertificateStoreLocation, CertificateStoreName);
                        store.Remove(certificate);

                        certificateRemoved = true;
                    }
                }
                if (certificateRemoved)
                {
                    LogInstalledCertificates();
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
