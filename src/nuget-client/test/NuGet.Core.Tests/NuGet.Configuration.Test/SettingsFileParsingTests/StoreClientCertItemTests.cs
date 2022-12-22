// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FluentAssertions;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Configuration.Test
{
    public class StoreClientCertItemTests
    {
        [Fact]
        public void StoreClientCert_FromDocumentation_ParsedCorrectly()
        {
            // Arrange
            var config = @"
<configuration>
   <SectionName>
      <storeCert packageSource=""Contoso""
           storeLocation = ""currentUser""
           storeName = ""my""
           findBy = ""thumbprint""
           findValue = ""4894671ae5aa84840cc1079e89e82d426bc24ec6"" />
   </SectionName>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();
                var items = section.Items.ToList();

                items.Count.Should().Be(1);

                var storeClientCertItem = (StoreClientCertItem)items[0];
                storeClientCertItem.ElementName.Should().Be("storeCert");
                storeClientCertItem.PackageSource.Should().Be("Contoso");
                storeClientCertItem.StoreLocation.Should().Be(StoreLocation.CurrentUser);
                storeClientCertItem.StoreName.Should().Be(StoreName.My);
                storeClientCertItem.FindType.Should().Be(X509FindType.FindByThumbprint);
                storeClientCertItem.FindValue.Should().Be("4894671ae5aa84840cc1079e89e82d426bc24ec6");

                var expectedStoreClientCertItem = new StoreClientCertItem("Contoso",
                                                                          "4894671ae5aa84840cc1079e89e82d426bc24ec6",
                                                                          StoreLocation.CurrentUser,
                                                                          StoreName.My,
                                                                          X509FindType.FindByThumbprint);
                SettingsTestUtils.DeepEquals(storeClientCertItem, expectedStoreClientCertItem).Should().BeTrue();
            }
        }

        [Theory]
        [InlineData("addressBook", StoreName.AddressBook)]
        [InlineData("authRoot", StoreName.AuthRoot)]
        [InlineData("certificateAuthority", StoreName.CertificateAuthority)]
        [InlineData("disallowed", StoreName.Disallowed)]
        [InlineData("my", StoreName.My)]
        [InlineData("root", StoreName.Root)]
        [InlineData("trustedPeople", StoreName.TrustedPeople)]
        [InlineData("trustedPublisher", StoreName.TrustedPublisher)]
        public void StoreClientCert_StoreName_ParsedCorrectly(string stringValue, StoreName value)
        {
            // Arrange
            var config = $@"
<configuration>
   <SectionName>
      <storeCert packageSource=""Contoso1"" findValue = ""42"" storeName = ""{stringValue}"" />
      <storeCert packageSource=""Contoso2"" findValue = ""42"" storeName = ""{stringValue.ToLower()}"" />
      <storeCert packageSource=""Contoso3"" findValue = ""42"" storeName = ""{stringValue.ToUpper()}"" />
   </SectionName>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();
                var items = section.Items.ToList();

                items.Count.Should().Be(3);

                var storeClientCertItem = (StoreClientCertItem)items[0];
                storeClientCertItem.StoreName.Should().Be(value);

                var storeClientCertItemLowerCase = (StoreClientCertItem)items[1];
                storeClientCertItemLowerCase.StoreName.Should().Be(value);

                var storeClientCertItemUpperCase = (StoreClientCertItem)items[2];
                storeClientCertItemUpperCase.StoreName.Should().Be(value);
            }
        }

        [Theory]
        [InlineData("currentUser", StoreLocation.CurrentUser)]
        [InlineData("localMachine", StoreLocation.LocalMachine)]
        public void StoreClientCert_StoreLocation_ParsedCorrectly(string stringValue, StoreLocation value)
        {
            // Arrange
            var config = $@"
<configuration>
   <SectionName>
      <storeCert packageSource=""Contoso1"" findValue = ""42"" storeLocation = ""{stringValue}"" />
      <storeCert packageSource=""Contoso2"" findValue = ""42"" storeLocation = ""{stringValue.ToLower()}"" />
      <storeCert packageSource=""Contoso3"" findValue = ""42"" storeLocation = ""{stringValue.ToUpper()}"" />
   </SectionName>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();
                var items = section.Items.ToList();

                items.Count.Should().Be(3);

                var storeClientCertItem = (StoreClientCertItem)items[0];
                storeClientCertItem.StoreLocation.Should().Be(value);

                var storeClientCertItemLowerCase = (StoreClientCertItem)items[1];
                storeClientCertItemLowerCase.StoreLocation.Should().Be(value);

                var storeClientCertItemUpperCase = (StoreClientCertItem)items[2];
                storeClientCertItemUpperCase.StoreLocation.Should().Be(value);
            }
        }

        [Theory]
        [InlineData("thumbprint", X509FindType.FindByThumbprint)]
        [InlineData("subjectName", X509FindType.FindBySubjectName)]
        [InlineData("subjectDistinguishedName", X509FindType.FindBySubjectDistinguishedName)]
        [InlineData("issuerName", X509FindType.FindByIssuerName)]
        [InlineData("issuerDistinguishedName", X509FindType.FindByIssuerDistinguishedName)]
        [InlineData("serialNumber", X509FindType.FindBySerialNumber)]
        [InlineData("timeValid", X509FindType.FindByTimeValid)]
        [InlineData("timeNotYetValid", X509FindType.FindByTimeNotYetValid)]
        [InlineData("timeExpired", X509FindType.FindByTimeExpired)]
        [InlineData("templateName", X509FindType.FindByTemplateName)]
        [InlineData("applicationPolicy", X509FindType.FindByApplicationPolicy)]
        [InlineData("certificatePolicy", X509FindType.FindByCertificatePolicy)]
        [InlineData("extension", X509FindType.FindByExtension)]
        [InlineData("keyUsage", X509FindType.FindByKeyUsage)]
        [InlineData("subjectKeyIdentifier", X509FindType.FindBySubjectKeyIdentifier)]
        public void StoreClientCert_FindBy_ParsedCorrectly(string stringValue, X509FindType value)
        {
            // Arrange
            var config = $@"
<configuration>
   <SectionName>
      <storeCert packageSource=""Contoso1"" findValue = ""42"" findBy = ""{stringValue}"" />
      <storeCert packageSource=""Contoso2"" findValue = ""42"" findBy = ""{stringValue.ToLower()}"" />
      <storeCert packageSource=""Contoso3"" findValue = ""42"" findBy = ""{stringValue.ToUpper()}"" />
   </SectionName>
</configuration>";

            var nugetConfigPath = "NuGet.Config";
            using (var mockBaseDirectory = TestDirectory.Create())
            {
                SettingsTestUtils.CreateConfigurationFile(nugetConfigPath, mockBaseDirectory, config);

                // Act and Assert
                var settingsFile = new SettingsFile(mockBaseDirectory);
                var section = settingsFile.GetSection("SectionName");
                section.Should().NotBeNull();
                var items = section.Items.ToList();

                items.Count.Should().Be(3);

                var storeClientCertItem = (StoreClientCertItem)items[0];
                storeClientCertItem.FindType.Should().Be(value);

                var storeClientCertItemLowerCase = (StoreClientCertItem)items[1];
                storeClientCertItemLowerCase.FindType.Should().Be(value);

                var storeClientCertItemUpperCase = (StoreClientCertItem)items[2];
                storeClientCertItemUpperCase.FindType.Should().Be(value);
            }
        }
    }
}
