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
    public class InDomainTests
    {
        [Fact]
        public void Should_ReturnFalse_ForNullAndEmptyInputs()
        {
            Uri nullUri = null!;
            string nullString = null!;
            string[] nullStrings = null!;

            Assert.False(URIValidator.InDomain(nullUri, "bing.com"));
            Assert.False(URIValidator.InDomain(nullUri, ["bing.com"]));
            Assert.False(URIValidator.InDomain(nullString, "bing.com"));
            Assert.False(URIValidator.InDomain(nullString, ["bing.com"]));

            Assert.False(URIValidator.InDomain("http://bing.com", (string)null!));
            Assert.False(URIValidator.InDomain("http://bing.com", string.Empty));
            Assert.False(URIValidator.InDomain(new Uri("http://bing.com"), (string)null!));
            Assert.False(URIValidator.InDomain(new Uri("http://bing.com"), string.Empty));

            Assert.False(URIValidator.InDomain("http://bing.com", nullStrings));
            Assert.False(URIValidator.InDomain("http://bing.com", Array.Empty<string>()));
            Assert.False(URIValidator.InDomain(new Uri("http://bing.com"), nullStrings));
            Assert.False(URIValidator.InDomain(new Uri("http://bing.com"), Array.Empty<string>()));
        }

        [Theory]
        [InlineData("http://office.com", "office.com")]
        [InlineData("https://office.com", "office.com")]
        [InlineData("https://azure.com", ".azure.com")]
        [InlineData("https://subdomain.microsoft.com", "microsoft.com")]
        [InlineData("https://subdomain.microsoft.com", ".microsoft.com")]
        public void Should_ReturnTrue_ForValidSingleDomains(string url, string trustedDomain)
        {
            Assert.True(URIValidator.InDomain(url, trustedDomain));
            Assert.True(URIValidator.InDomain(new Uri(url), trustedDomain));
        }

        [Theory]
        [InlineData("https://subdomain.one.com", new[] { "one.com", ".two.com" })]
        [InlineData("http://subdomain.two.com", new[] { "one.com", ".two.com" })]
        [InlineData("https://one.com", new[] { "one.com", ".two.com" })]
        [InlineData("https://two.net", new[] { "one.com", ".two.net" })]
        public void Should_ReturnTrue_ForValidTrustedDomainArrays(string url, string[] trustedDomains)
        {
            Assert.True(URIValidator.InDomain(url, trustedDomains));
            Assert.True(URIValidator.InDomain(new Uri(url), trustedDomains));
        }

        [Theory]
        [InlineData("http://azure.com", "office.com")]
        [InlineData("https://office.com", "subdomain.office.com")]
        [InlineData("https://azure.com", ".office.com")]
        [InlineData("https://subdomain.microsoft.com", "differentsubdomain.microsoft.com")]
        public void Should_ReturnFalse_ForInvalidSingleDomains(string url, string trustedDomain)
        {
            Assert.False(URIValidator.InDomain(url, trustedDomain));
            Assert.False(URIValidator.InDomain(new Uri(url), trustedDomain));
        }

        [Theory]
        [InlineData("http://azure.com", new[] { "one.com", "office.com" })]
        [InlineData("https://office.com", new[] { "subdomain.office.com" })]
        [InlineData("https://azure.com", new[] { ".office.com", "two.com" })]
        [InlineData("https://subdomain.microsoft.com", new[] { "differentsubdomain.microsoft.com" })]
        public void Should_ReturnFalse_ForInvalidTrustedDomainArrays(string url, string[] trustedDomains)
        {
            Assert.False(URIValidator.InDomain(url, trustedDomains));
            Assert.False(URIValidator.InDomain(new Uri(url), trustedDomains));
        }

        [Theory]
        [InlineData("http://username@bing.com:/")]
        [InlineData("http://username:password@bing.com")]
        [InlineData("http://bing.com:45")]
        [InlineData("http://bing.com/some/path")]
        [InlineData("http://bing.com#fragment")]
        [InlineData("http://bing.com/?query=hi")]
        [InlineData("http:/\\bing.com")]
        [InlineData("http:\\/bing.com")]
        public void Should_ReturnTrue_ForUrlsWithVariousComponents(string url)
        {
            Assert.True(URIValidator.InDomain(url, "bing.com"));
            Assert.True(URIValidator.InDomain(url, ["bing.com"]));
            Assert.True(URIValidator.InDomain(new Uri(url), "bing.com"));
            Assert.True(URIValidator.InDomain(new Uri(url), ["bing.com"]));
        }

        [Theory]
        [InlineData("http://bing.com:badPort")]
        [InlineData("http://:bing.com")]
        public void Should_ReturnFalse_ForStringOnlyInvalidUrlComponents(string url)
        {
            Assert.False(URIValidator.InDomain(url, "bing.com"));
            Assert.False(URIValidator.InDomain(url, ["bing.com"]));
        }

        [Theory]
        [InlineData("http://español.test.net/", "test.net")]
        [InlineData("http://español.test.net/", "xn--espaol-zwa.test.net")]
        [InlineData("http://你好/", "xn--6qq79v")]
        [InlineData("http://test.你好/", "你好")]
        [InlineData("http://bing.hi.com/", "hi.com")]
        [InlineData("http://bing.hı.com/", "hı.com")]
        [InlineData("http://bing.hí.com/", "hí.com")]
        [InlineData("http://😉", "😉")]
        public void Should_ReturnTrue_ForUnicodeSingleDomains(string url, string trustedDomain)
        {
            Assert.True(URIValidator.InDomain(url, trustedDomain));
            Assert.True(URIValidator.InDomain(new Uri(url), trustedDomain));
        }

        [Theory]
        [InlineData("http://español.test.net/", new[] { "notempty", "test.net" })]
        [InlineData("http://español.test.net/", new[] { "hello", "xn--espaol-zwa.test.net" })]
        [InlineData("http://你好/", new[] { "xn--6qq79v", "not_the_domain.com" })]
        [InlineData("http://bing.hı.com/", new[] { "hi.com", "hı.com" })]
        [InlineData("http://bing.hí.com/", new[] { "hí.com", "notempty" })]
        [InlineData("http://test.你好/", new[] { "你好", "bing.com" })]
        public void Should_ReturnTrue_ForUnicodeTrustedDomainArrays(string url, string[] trustedDomains)
        {
            Assert.True(URIValidator.InDomain(url, trustedDomains));
            Assert.True(URIValidator.InDomain(new Uri(url), trustedDomains));
        }

        [Theory]
        [InlineData("http://bing.hı.com/", "hi.com")]
        [InlineData("http://bing.hí.com/", "hi.com")]
        [InlineData("http://😉", "🔨")]
        public void Should_ReturnFalse_ForInvalidUnicodeSingleDomains(string url, string trustedDomain)
        {
            Assert.False(URIValidator.InDomain(url, trustedDomain));
            Assert.False(URIValidator.InDomain(new Uri(url), trustedDomain));
        }

        [Theory]
        [InlineData("http://bing.hı.com/", new[] { "hi.com", "hí.com" })]
        [InlineData("http://bing.hí.com/", new[] { "hi.com", "hı.com" })]
        [InlineData("http://😉", new[] { "🔨", "" })]
        public void Should_ReturnFalse_ForInvalidUnicodeTrustedDomainArrays(string url, string[] trustedDomains)
        {
            Assert.False(URIValidator.InDomain(url, trustedDomains));
            Assert.False(URIValidator.InDomain(new Uri(url), trustedDomains));
        }

        [Theory]
        [InlineData("http://evil.c℁.core.azure.net", "azure.net")]
        public void Should_ReturnFalse_ForStringOnlyInvalidUnicodeDomains(string url, string trustedDomain)
        {
            Assert.False(URIValidator.InDomain(url, trustedDomain));
        }

        [Theory]
        [InlineData("hTtP://test.net", "test.net")]
        [InlineData("wSS://test.net", "test.net")]
        [InlineData("http://HELLO.com", "hello.com")]
        [InlineData("http://Hello.你好/", "xn--6qq79v")]
        [InlineData("http://español.test.net/", "TeSt.net")]
        [InlineData("http://hello.COM", "HELLO.com")]
        public void Should_ReturnTrue_ForMixedCaseSingleDomains(string url, string trustedDomain)
        {
            Assert.True(URIValidator.InDomain(url, trustedDomain));
            Assert.True(URIValidator.InDomain(new Uri(url), trustedDomain));
        }

        [Theory]
        [InlineData("http://HELLO.com", new[] { "notempty", "hello.com" })]
        [InlineData("http://Hello.你好/", new[] { "xn--6qq79v", "not_the_domain.com" })]
        [InlineData("http://español.test.net/", new[] { "TeSt.net", "asdf" })]
        [InlineData("http://hello.COM", new[] { "HELLO.com" })]
        public void Should_ReturnTrue_ForMixedCaseTrustedDomainArrays(string url, string[] trustedDomains)
        {
            Assert.True(URIValidator.InDomain(url, trustedDomains));
            Assert.True(URIValidator.InDomain(new Uri(url), trustedDomains));
        }

        [Theory]
        [InlineData("http://bing.com", "bing.com")]
        [InlineData("https://bing.com", "bing.com")]
        [InlineData("ws://bing.com", "bing.com")]
        [InlineData("wss://bing.com", "bing.com")]
        public void Should_ReturnTrue_ForAllowedProtocols(string url, string trustedDomain)
        {
            Assert.True(URIValidator.InDomain(url, trustedDomain));
            Assert.True(URIValidator.InDomain(new Uri(url), trustedDomain));
        }

        [Theory]
        [InlineData("ftp://bing.com", "bing.com")]
        [InlineData("file://bing.com", "bing.com")]
        [InlineData("gopher://bing.com", "bing.com")]
        [InlineData("mailto:bing.com", "bing.com")]
        [InlineData("data://bing.com", "bing.com")]
        [InlineData("javascript:alert('XSS')", "bing.com")]
        [InlineData("evil.com://bing.com", "bing.com")]
        public void Should_ReturnFalse_ForDisallowedProtocols(string url, string trustedDomain)
        {
            Assert.False(URIValidator.InDomain(url, trustedDomain));
            Assert.False(URIValidator.InDomain(new Uri(url), trustedDomain));
        }

        [Theory]
        [InlineData("c:\\foo\\bar", "somedomain.com")]
        [InlineData("file:///filepath", "somedomain.com")]
        [InlineData("CCCCCCCCCCCCCCCCCCCCCCC:\\\\\\\\\\\\foo\\bar", "somedomain.com")]
        [InlineData("/foo/bar", "somedomain.com")]
        [InlineData("//////////////////////////////////////", "somedomain.com")]
        [InlineData("\\\\.\\a\\a\\a\\", "somedomain.com")]
        [InlineData("\\\\\\.\\a\\a\\a\\", "somedomain.com")]
        public void Should_ReturnFalse_ForFilePathStrings(string url, string trustedDomain)
        {
            Assert.False(URIValidator.InDomain(url, trustedDomain));
        }
    }
}
