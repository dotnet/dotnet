// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace System.PrivateUri.Tests
{
    public class IdnHostNameValidationTest
    {
        [Fact]
        public void IdnHost_Call()
        {
            Uri u = new Uri("http://someHost\u1234.com");
            Assert.Equal("xn--somehost-vk7a.com", u.IdnHost);
        }

        [Fact]
        public void IdnHost_Call_ThrowsException()
        {
            Uri u = new Uri("/test/test2/file.tst", UriKind.Relative);
            Assert.Throws<InvalidOperationException>(() => u.IdnHost);
        }

        [Fact]
        public void IdnHost_UnknownFqdn_Success()
        {
            CommonSchemesTest("unknown");
        }

        [Fact]
        public void IdnHost_HttpFqdn_Success()
        {
            CommonSchemesTest("http");
        }

        [Fact]
        public void IdnHost_FtpUnicodeFqdn_Success()
        {
            CommonSchemesTest("ftp");
        }

        [Fact]
        public void IdnHost_FileFqdn_Success()
        {
            CommonSchemesTest("file");
        }

        [Fact]
        public void IdnHost_NetPipeFqdn_Success()
        {
            CommonSchemesTest("net.pipe");
        }

        [Fact]
        public void IdnHost_NetTcpFqdn_Success()
        {
            CommonSchemesTest("net.tcp");
        }

        [Fact]
        public void IdnHost_VSMacrosFqdn_Success()
        {
            CommonSchemesTest("vsmacros");
        }

        [Fact]
        public void IdnHost_GopherFqdn_Success()
        {
            CommonSchemesTest("gopher");
        }

        [Fact]
        public void IdnHost_NntpFqdn_Success()
        {
            CommonSchemesTest("nntp");
        }

        [Fact]
        public void IdnHost_TelnetFqdn_Success()
        {
            CommonSchemesTest("telnet");
        }

        [Fact]
        public void IdnHost_LdapFqdn_Success()
        {
            CommonSchemesTest("ldap");
        }

        [Fact]
        public void IdnHost_Internal_Call_ThrowsException()
        {
            Uri u = new Uri("/test/test2/file.tst", UriKind.Relative);
            Assert.Throws<InvalidOperationException>(() => u.DnsSafeHost);
        }

        #region Helpers

        private static void CommonSchemesTest(string scheme)
        {
            string unicodeHost = "a\u00FChost.dom\u00FCin.n\u00FCet";
            string punycodeHost = "xn--ahost-kva.xn--domin-mva.xn--net-hoa";
            string fqdnHost = "foo.bar.";

            // initial unicode host
            ValidateUri(scheme, unicodeHost, UriHostNameType.Dns, unicodeHost, unicodeHost, punycodeHost);

            // initial punycode host
            ValidateUri(scheme, punycodeHost, UriHostNameType.Dns, punycodeHost, punycodeHost, punycodeHost);

            // Host ending with a dot
            ValidateUri(scheme, fqdnHost, UriHostNameType.Dns, fqdnHost, fqdnHost, fqdnHost);
        }

        private static void ValidateUri(
            string scheme,
            string host,
            UriHostNameType expectedHostType,
            string expectedHost,
            string expectedDnsSafeHost,
            string expectedIdnHost)
        {
            Assert.True(Uri.TryCreate(scheme + "://" + host, UriKind.Absolute, out Uri uri));
            Assert.Equal(expectedHost, uri.Host);
            Assert.Equal(expectedDnsSafeHost, uri.DnsSafeHost);
            Assert.Equal(expectedHostType, uri.HostNameType);
            Assert.Equal(expectedHostType, Uri.CheckHostName(host));
            Assert.Equal(expectedIdnHost, uri.IdnHost);
        }

        #endregion Helpers
    }
}
