// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class ClientCertificateProviderTests
    {
        [PlatformFact(Platform.Windows)]
        public void CertificateFromFile_Success_ParsedAndAddedToAssociatedPackageSource()
        {
            using (var testInfo = new TestInfo())
            {
                // Arrange
                testInfo.SetupCertificateFile();

                // Act
                var settings = testInfo.LoadSettingsFromConfigFile();
                var clientCertificateProvider = new ClientCertificateProvider(settings);
                clientCertificateProvider.AddOrUpdate(new FileClientCertItem(testInfo.PackageSourceName,
                                                                             testInfo.CertificateAbsoluteFilePath,
                                                                             testInfo.CertificatePassword,
                                                                             false,
                                                                             testInfo.ConfigFile));

                // Assert
                var packageSourceProvider = new PackageSourceProvider(settings, TestConfigurationDefaults.NullInstance);
                var packageSourceList = packageSourceProvider.LoadPackageSources().ToList();
                Assert.Equal(1, packageSourceList.Count);
                Assert.Equal(1, packageSourceList[0].ClientCertificates!.Count);
                Assert.Equal(testInfo.Certificate, packageSourceList[0].ClientCertificates![0]);
            }
        }

        [PlatformFact(Platform.Windows, Platform.Linux)]
        public void CertificateFromStore_Success_ParsedAndAddedToAssociatedPackageSource()
        {
            using (var testInfo = new TestInfo())
            {
                // Arrange
                testInfo.SetupCertificateInStorage();

                // Act
                var settings = testInfo.LoadSettingsFromConfigFile();
                var clientCertificateProvider = new ClientCertificateProvider(settings);
                clientCertificateProvider.AddOrUpdate(new StoreClientCertItem(testInfo.PackageSourceName,
                                                                              testInfo.CertificateFindValue.ToString()!,
                                                                              testInfo.CertificateStoreLocation,
                                                                              testInfo.CertificateStoreName,
                                                                              testInfo.CertificateFindBy));

                // Assert
                var packageSourceProvider = new PackageSourceProvider(settings, TestConfigurationDefaults.NullInstance);
                var packageSourceList = packageSourceProvider.LoadPackageSources().ToList();
                Assert.Equal(1, packageSourceList.Count);
                Assert.Equal(1, packageSourceList[0].ClientCertificates!.Count);
                Assert.Equal(testInfo.Certificate, packageSourceList[0].ClientCertificates![0]);
            }
        }

        private sealed class TestInfo : IDisposable
        {
            public TestInfo()
            {
                WorkingPath = TestDirectory.Create();
                ConfigFile = Path.Combine(WorkingPath, "NuGet.config");
                PackageSourceName = "Foo";
                CertificateFileName = "contoso.pfx";
                CertificateAbsoluteFilePath = Path.Combine(WorkingPath, CertificateFileName);
                CertificateRelativeFilePath = ".\\" + CertificateFileName;
                CertificatePassword = "password";

                CertificateStoreLocation = StoreLocation.CurrentUser;
                CertificateStoreName = StoreName.My;
                CertificateFindBy = X509FindType.FindByIssuerName;
                CertificateFindValue = "Contoso";
                Certificate = GetCertificate();
                File.WriteAllText(ConfigFile,
                                  $@"
<configuration>
    <packageSources>
        <add key=""{PackageSourceName}"" value=""https://contoso.com/v3/index.json"" />
    </packageSources>
</configuration>
");
            }

            public X509Certificate2 Certificate { get; }

            public string CertificateAbsoluteFilePath { get; }
            public string CertificateFileName { get; }
            public X509FindType CertificateFindBy { get; }
            public object CertificateFindValue { get; }
            public string CertificatePassword { get; }
            public string CertificateRelativeFilePath { get; }
            public StoreLocation CertificateStoreLocation { get; }
            public StoreName CertificateStoreName { get; }
            public string ConfigFile { get; }

            public string PackageSourceName { get; }
            public TestDirectory WorkingPath { get; }

            public void Dispose()
            {
                WorkingPath.Dispose();
                RemoveCertificateFromStorage();
                Certificate.Dispose();
            }

            public ISettings LoadSettingsFromConfigFile()
            {
                var directory = Path.GetDirectoryName(ConfigFile)!;
                var filename = Path.GetFileName(ConfigFile);
                return Settings.LoadSpecificSettings(directory, filename);
            }

            public void SetupCertificateFile()
            {
                File.WriteAllBytes(CertificateAbsoluteFilePath, Certificate.RawData);
            }

            public void SetupCertificateInStorage()
            {
                using (var store = new X509Store(CertificateStoreName, CertificateStoreLocation))
                {
                    store.Open(OpenFlags.ReadWrite);
                    store.Add(Certificate);
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

            private X509Certificate2 GetCertificate()
            {
#if NET9_0_OR_GREATER
                return X509CertificateLoader.LoadPkcs12(CreateCertificate(), CertificatePassword);
#else
                return new X509Certificate2(CreateCertificate(), CertificatePassword);
#endif
            }

            private void RemoveCertificateFromStorage()
            {
                using (var store = new X509Store(CertificateStoreName, CertificateStoreLocation))
                {
                    store.Open(OpenFlags.ReadWrite);

                    X509Certificate2Collection resultCertificates = store.Certificates.Find(
                        X509FindType.FindByIssuerDistinguishedName,
                        Certificate.Issuer,
                        validOnly: false);

                    foreach (X509Certificate2 certificate in resultCertificates)
                    {
                        store.Remove(certificate);
                    }
                }
            }
        }
    }
}
