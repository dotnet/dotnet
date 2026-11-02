// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Security.AntiSSRF.FunctionalTests;
using Xunit;

using Microsoft.Security.AntiSSRF;

namespace Microsoft.Security.AntiSSRF.FunctionalTests
{
    public class InAzureKeyVaultDomainTests
    {
        [Fact]
        public void Should_ReturnFalse_ForNullAndEmptyInputs()
        {
            Assert.False(URIValidator.InAzureKeyVaultDomain((string)null!));
            Assert.False(URIValidator.InAzureKeyVaultDomain((Uri)null!));
            Assert.False(URIValidator.InAzureKeyVaultDomain(""));
        }

        [Theory]
        [InlineData("https://contoso.vault.azure.net/")]
        [InlineData("https://fabrikam42.managedhsm.azure.net")]
        [InlineData("https://corp-hsm.vault.azure.cn")]
        [InlineData("https://devkeys99.managedhsm.azure.cn/")]
        [InlineData("https://govvault1.vault.usgovcloudapi.net")]
        [InlineData("https://securehsm.managedhsm.usgovcloudapi.net/")]
        public void Should_ReturnTrue_ForValidAzureKeyVaultDomains(string url)
        {
            Assert.True(URIValidator.InAzureKeyVaultDomain(url));
            Assert.True(URIValidator.InAzureKeyVaultDomain(new Uri(url)));
        }

        [Theory]
        [InlineData("https://contoso.privatelink.vault.azure.net")]
        [InlineData("https://fabrikam42.privatelink.managedhsm.azure.net")]
        [InlineData("https://corp-hsm.privatelink.vault.azure.cn")]
        [InlineData("https://devkeys99.privatelink.managedhsm.azure.cn")]
        [InlineData("https://govvault1.privatelink.vault.usgovcloudapi.net")]
        [InlineData("https://securehsm.privatelink.managedhsm.usgovcloudapi.net")]
        public void Should_ReturnTrue_ForPrivateAzureKeyVaultDomains(string url)
        {
            Assert.True(URIValidator.InAzureKeyVaultDomain(url));
            Assert.True(URIValidator.InAzureKeyVaultDomain(new Uri(url)));
        }

        [Theory]
        [InlineData("https://my--vault.vault.azure.net")]
        [InlineData("https://contoso.managedhsm.azure.net.evil.com")]
        [InlineData("https://fabrikam.vault.azure.net.attacker.org")]
        [InlineData("https://corp.vault.azuree.net")]
        [InlineData("https://devkeys.vault.azure.nett")]
        [InlineData("https://contoso.vault.azure.netmalicious")]
        [InlineData("https://contoso.managedhsm.azure.usgovcloudapi.net")]
        [InlineData("https://contoso.azure.vault.net")]
        [InlineData("https://contoso.vault.azure.cn.fake")]
        [InlineData("https://securehsm.managedhsm.usgovcloudapi.net.phishing")]
        [InlineData("https://corp.managedhsm.azure.cnn")]
        [InlineData("https://contoso.vaultazure.net")]
        [InlineData("https://contoso.vault.azure")]
        public void Should_ReturnFalse_ForInvalidAzureKeyVaultDomains(string url)
        {
            Assert.False(URIValidator.InAzureKeyVaultDomain(url));
            Assert.False(URIValidator.InAzureKeyVaultDomain(new Uri(url)));
        }

        [Theory]
        [InlineData("http://accountname.vault.azure.net/some/path", true)]
        [InlineData("http://accountname.vault.azure.net#fragment", true)]
        [InlineData("http://accountname.vault.azure.net/?query=hi", true)]
        [InlineData("http://accountname.vault.azure.net:45", true)]
        [InlineData("https://username@accountname.vault.azure.net", true)]
        [InlineData("https://username:password@accountname.vault.azure.net", true)]
        [InlineData("https:accountname.vault.azure.net", false)]
        [InlineData("http:/accountname.vault.azure.net", false)]
        [InlineData("http:/\\accountname.vault.azure.net", true)]
        [InlineData("http:\\/accountname.vault.azure.net", true)]
        [InlineData("http://accountname.vault.azure.net:badPort", false)]
        [InlineData("http://:accountname.vault.azure.net", false)]
        public void Should_ReturnCorrectResult_ForUrlsWithVariousComponents(string url, bool expectedResult)
        {
            Assert.Equal(expectedResult, URIValidator.InAzureKeyVaultDomain(url));
            
            // Only test Uri overload for valid URI formats
            try {
                Uri parsedUri = new(url);
                Assert.Equal(expectedResult, URIValidator.InAzureKeyVaultDomain(parsedUri));
            }
            catch (UriFormatException)
            {
                // Ignore exceptions for invalid URI formats
            }
        }

        [Theory]
        [InlineData("http://ñame.vault.azure.net/")]
        [InlineData("https://contøso.managedhsm.azure.net")]
        [InlineData("https://fabrikäm.vault.azure.cn/")]
        [InlineData("https://corp.vàult.azure.net")]
        [InlineData("https://devkeys.vault.àzure.net")]
        [InlineData("https://contoso.vault.azure.cñ")]
        [InlineData("https://сontoso.vault.usgovcloudapi.net")]
        [InlineData("https://myapp.mànagedhsm.azure.cn")]
        [InlineData("http://evil.c℁.vault.azure.net")]
        [InlineData("https://データ.vault.usgovcloudapi.net")]
        [InlineData("https://файлы.managedhsm.usgovcloudapi.net")]
        public void Should_ReturnFalse_ForUnicodeCharactersInDomains(string url)
        {
            Assert.False(URIValidator.InAzureKeyVaultDomain(url));
            
            // Test Uri overload if the string can be parsed as a Uri
            try
            {
                Uri parsedUri = new(url);
                Assert.False(URIValidator.InAzureKeyVaultDomain(parsedUri));
            }
            catch (UriFormatException)
            {
                // Ignore exceptions for invalid URI formats
            }
        }

        [Theory]
        [InlineData("http://CONTOSO.vault.azure.net")]
        [InlineData("https://fabrikam42.VAULT.azure.net")]
        [InlineData("https://corp-hsm.vault.AZURE.net")]
        [InlineData("https://DEVKEYS99.managedhsm.azure.cn/")]
        [InlineData("https://govvault1.MANAGEDHSM.azure.cn")]
        [InlineData("https://SECUREHSM.vault.USGOVCLOUDAPI.net")]
        [InlineData("https://contoso.managedhsm.USGOVCLOUDAPI.NET")]
        [InlineData("HTTPS://fabrikam42.vault.azure.net")]
        [InlineData("hTtPs://corp-hsm.managedhsm.azure.net")]
        [InlineData("https://CONTOSO.VAULT.AZURE.NET")]
        [InlineData("HtTpS://devkeys99.vault.azure.cn")]
        [InlineData("https://GovVault1.Vault.Azure.Net")]
        public void Should_ReturnTrue_ForMixedCaseDomains(string url)
        {
            Assert.True(URIValidator.InAzureKeyVaultDomain(url));
            Assert.True(URIValidator.InAzureKeyVaultDomain(new Uri(url)));
        }

        [Theory]
        [InlineData("http://accountname.vault.azure.net", true)]
        [InlineData("https://accountname.vault.azure.net", true)]
        [InlineData("ws://accountname.vault.azure.net", false)]
        [InlineData("wss://accountname.vault.azure.net", false)]
        [InlineData("ftp://accountname.vault.azure.net", false)]
        [InlineData("file://accountname.vault.azure.net", false)]
        [InlineData("gopher://accountname.vault.azure.net", false)]
        [InlineData("mailto:accountname.vault.azure.net", false)]
        [InlineData("data://accountname.vault.azure.net", false)]
        [InlineData("javascript:alert('XSS')", false)]
        [InlineData("evil.com://accountname.vault.azure.net", false)]
        public void Should_ReturnCorrectResult_BasedOnProtocolScheme(string url, bool expectedResult)
        {
            Assert.Equal(expectedResult, URIValidator.InAzureKeyVaultDomain(url));
            
            // Only test Uri overload for schemes that can create valid Uris
            try {
                Uri parsedUri = new(url);
                Assert.Equal(expectedResult, URIValidator.InAzureKeyVaultDomain(parsedUri));
            }
            catch (UriFormatException)
            {
                // Ignore exceptions from invalid URI formats
            }
        }
    }
}
