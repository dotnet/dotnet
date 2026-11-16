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
    public class InAzureStorageDomainTests
    {
        [Fact]
        public void Should_ReturnFalse_ForNullAndEmptyInputs()
        {
            Assert.False(URIValidator.InAzureStorageDomain((string)null!));
            Assert.False(URIValidator.InAzureStorageDomain((Uri)null!));
            Assert.False(URIValidator.InAzureStorageDomain(""));
        }

        [Theory]
        [InlineData("https://myapp.blob.core.windows.net")]
        [InlineData("https://frontend3.web.core.windows.net")]
        [InlineData("https://data-lake.dfs.core.windows.net")]
        [InlineData("https://files123.file.core.windows.net")]
        [InlineData("https://queue-svc.queue.core.windows.net")]
        [InlineData("https://tables01.table.core.windows.net")]
        [InlineData("https://secure.blob.storage.azure.net")]
        [InlineData("https://internal9.web.storage.azure.net")]
        [InlineData("https://private-ep.dfs.storage.azure.net")]
        [InlineData("https://corp-files.file.storage.azure.net")]
        [InlineData("https://company2.queue.storage.azure.net")]
        [InlineData("https://enterprise.table.storage.azure.net")]
        [InlineData("https://gov-data.blob.core.usgovcloudapi.net")]
        [InlineData("https://portal456.web.core.usgovcloudapi.net")]
        [InlineData("https://analytics.dfs.core.usgovcloudapi.net")]
        [InlineData("https://docs-gov.file.core.usgovcloudapi.net")]
        [InlineData("https://notify123.queue.core.usgovcloudapi.net")]
        [InlineData("https://records.table.core.usgovcloudapi.net")]
        [InlineData("https://china-app.blob.core.chinacloudapi.cn")]
        [InlineData("https://website7.web.core.chinacloudapi.cn")]
        [InlineData("https://bigdata99.dfs.core.chinacloudapi.cn")]
        [InlineData("https://storage.file.core.chinacloudapi.cn")]
        [InlineData("https://events-cn.queue.core.chinacloudapi.cn")]
        [InlineData("https://metadata2.table.core.chinacloudapi.cn")]
        public void Should_ReturnTrue_ForValidAzureStorageDomains(string url)
        {
            Assert.True(URIValidator.InAzureStorageDomain(url));
            Assert.True(URIValidator.InAzureStorageDomain(new Uri(url)));
        }

        [Theory]
        [InlineData("https://myapp-secondary.blob.storage.azure.net")]
        [InlineData("https://website-secondary.web.core.windows.net")]
        [InlineData("https://files5-secondary.dfs.core.usgovcloudapi.net")]
        [InlineData("https://messages-secondary.queue.core.chinacloudapi.cn")]
        [InlineData("https://corp99-secondary.table.storage.azure.net")]
        [InlineData("https://backup-secondary.file.core.windows.net")]
        public void Should_ReturnTrue_ForSecondaryAzureStorageDomains(string url)
        {
            Assert.True(URIValidator.InAzureStorageDomain(url));
            Assert.True(URIValidator.InAzureStorageDomain(new Uri(url)));
        }

        [Theory]
        [InlineData("https://acct.privatelink.blob.storage.azure.net")]
        [InlineData("https://web12.privatelink.web.core.windows.net")]
        [InlineData("https://data.privatelink.dfs.storage.azure.net")]
        [InlineData("https://files99.privatelink.file.core.usgovcloudapi.net")]
        [InlineData("https://queue.privatelink.queue.core.chinacloudapi.cn")]
        [InlineData("https://tables5-secondary.privatelink.table.storage.azure.net")]
        public void Should_ReturnTrue_ForPrivateAzureStorageDomains(string url)
        {
            Assert.True(URIValidator.InAzureStorageDomain(url));
            Assert.True(URIValidator.InAzureStorageDomain(new Uri(url)));
        }

        [Theory]
        [InlineData("https://contosostaticsite.z22.web.core.windows.net")]
        [InlineData("https://webapp5.z03.web.storage.azure.net")]
        [InlineData("https://frontend.z45.blob.core.usgovcloudapi.net")]
        [InlineData("https://portal99.z01.web.core.chinacloudapi.cn")]
        [InlineData("https://static-site.privatelink.z88.dfs.storage.azure.net")]
        [InlineData("https://demo-secondary.z0.web.core.windows.net")]
        public void Should_ReturnTrue_ForStaticSitesAndDNSZones(string url)
        {
            Assert.True(URIValidator.InAzureStorageDomain(url));
            Assert.True(URIValidator.InAzureStorageDomain(new Uri(url)));
        }

        [Theory]
        [InlineData("https://my--app.blob.core.windows.net")]
        [InlineData("https://data--lake.dfs.storage.azure.net")]
        [InlineData("https://web--site.web.core.usgovcloudapi.net")]
        [InlineData("https://myapp.blob.core.windwos.net")]
        [InlineData("https://storage.table.core.windoes.net")]
        [InlineData("https://files.dfs.core.chinacloudapi.com")]
        [InlineData("https://queue.queue.stoarge.azure.net")]
        [InlineData("https://myapp.database.core.windows.net")]
        [InlineData("https://storage.cache.storage.azure.net")]
        [InlineData("https://files.storage.core.usgovcloudapi.net")]
        [InlineData("https://myapp.core.blob.windows.net")]
        [InlineData("https://storage.azure.storage.net")]
        [InlineData("https://files.windows.core.net")]
        [InlineData("https://myapp.blob.core.blob.windows.net")]
        [InlineData("https://storage.web.core.windows.net.storage.azure.net")]
        [InlineData("https://myapp.blob.core.windows.net.evil.com")]
        [InlineData("https://storage.table.core.windows.netmalicious")]
        [InlineData("https://files.dfs.storage.azure.net.attacker.org")]
        [InlineData("https://queue.queue.core.chinacloudapi.cnbad")]
        [InlineData("https://secure.web.storage.azure.netphishing")]
        [InlineData("https://corp.file.core.usgovcloudapi.net.fake")]
        public void Should_ReturnFalse_ForInvalidAzureStorageDomains(string url)
        {
            Assert.False(URIValidator.InAzureStorageDomain(url));
            Assert.False(URIValidator.InAzureStorageDomain(new Uri(url)));
        }

        [Theory]
        [InlineData("http://accountname.blob.core.windows.net/some/path", true)]
        [InlineData("http://accountname.blob.core.windows.net#fragment", true)]
        [InlineData("http://accountname.blob.core.windows.net/?query=hi", true)]
        [InlineData("http://accountname.blob.core.windows.net:45", true)]
        [InlineData("https://username@accountname.blob.core.windows.net", true)]
        [InlineData("https://username:password@accountname.blob.core.windows.net", true)]
        [InlineData("https:accountname.blob.core.windows.net", false)]
        [InlineData("http:/accountname.blob.core.windows.net", false)]
        [InlineData("http:/\\accountname.blob.core.windows.net", true)]
        [InlineData("http:\\/accountname.blob.core.windows.net", true)]
        [InlineData("http://accountname.blob.core.windows.net:badPort", false)]
        [InlineData("http://:accountname.blob.core.windows.net", false)]
        public void Should_ReturnCorrectResult_ForUrlsWithVariousComponents(string url, bool expectedResult)
        {
            Assert.Equal(expectedResult, URIValidator.InAzureStorageDomain(url));
            
            // Only test Uri overload for valid URI formats
            try {
                Uri parsedUri = new(url);
                Assert.Equal(expectedResult, URIValidator.InAzureStorageDomain(parsedUri));
            }
            catch (UriFormatException)
            {
                // Ignore exceptions for invalid URI formats
            }
        }

        [Theory]
        [InlineData("http://ñame.blob.core.windows.net/")]
        [InlineData("http://name.blob.core.wiñdows.net/")]
        [InlineData("http://evil.c℁.blob.core.windows.net")]
        [InlineData("https://tëst.web.storage.azure.net")]
        [InlineData("https://app.blob.core.windöws.net")]
        [InlineData("https://файлы.file.core.chinacloudapi.cn")]
        [InlineData("https://データ.dfs.core.usgovcloudapi.net")]
        [InlineData("https://myapp.bløb.core.windows.net")]
        public void Should_ReturnFalse_ForUnicodeCharactersInDomains(string url)
        {
            Assert.False(URIValidator.InAzureStorageDomain(url));
            
            // Test Uri overload if the string can be parsed as a Uri
            try
            {
                Uri parsedUri = new(url);
                Assert.False(URIValidator.InAzureStorageDomain(parsedUri));
            }
            catch (UriFormatException)
            {
                // Ignore exceptions for invalid URI formats
            }
        }

        [Theory]
        [InlineData("http://ACCOUNTNAME.blob.core.windows.net")]
        [InlineData("http://accountname.BLOB.core.windows.net")]
        [InlineData("http://ACCOUNTNAME.BLOB.CORE.WINDOWS.NET")]
        [InlineData("hTtP://test.blob.core.windows.net/")]
        [InlineData("HTTPS://myapp.WEB.storage.azure.net")]
        [InlineData("https://DATA.dfs.STORAGE.AZURE.NET")]
        [InlineData("HtTpS://files.FILE.core.usgovcloudapi.net")]
        [InlineData("http://QUEUE.queue.CORE.chinacloudapi.cn")]
        [InlineData("https://TABLES.table.core.WINDOWS.net")]
        public void Should_ReturnTrue_ForMixedCaseDomains(string url)
        {
            Assert.True(URIValidator.InAzureStorageDomain(url));
            Assert.True(URIValidator.InAzureStorageDomain(new Uri(url)));
        }

        [Theory]
        [InlineData("http://accountname.blob.core.windows.net", true)]
        [InlineData("https://accountname.blob.core.windows.net", true)]
        [InlineData("ws://accountname.blob.core.windows.net", false)]
        [InlineData("wss://accountname.blob.core.windows.net", false)]
        [InlineData("ftp://accountname.blob.core.windows.net", false)]
        [InlineData("file://accountname.blob.core.windows.net", false)]
        [InlineData("gopher://accountname.blob.core.windows.net", false)]
        [InlineData("mailto:accountname.blob.core.windows.net", false)]
        [InlineData("data://accountname.blob.core.windows.net", false)]
        [InlineData("javascript:alert('XSS')", false)]
        [InlineData("evil.com://accountname.blob.core.windows.net", false)]
        public void Should_ReturnCorrectResult_BasedOnProtocolScheme(string url, bool expectedResult)
        {
            Assert.Equal(expectedResult, URIValidator.InAzureStorageDomain(url));
            
            // Only test Uri overload for schemes that can create valid Uris
            try {
                Uri parsedUri = new(url);
                Assert.Equal(expectedResult, URIValidator.InAzureStorageDomain(parsedUri));
            }
            catch (UriFormatException)
            {
                // Ignore exceptions from invalid URI formats
            }
        }
    }
}
