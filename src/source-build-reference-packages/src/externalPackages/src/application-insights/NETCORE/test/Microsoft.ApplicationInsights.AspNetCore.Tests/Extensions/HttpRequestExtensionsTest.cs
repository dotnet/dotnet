namespace Microsoft.ApplicationInsights.AspNetCore.Tests.Extensions
{
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Globalization;
    using Xunit;

    public class HttpRequestExtensionsTest
    {
        const string ExpectedSchema = "http";
        const string ExpectedHostName = "randomhost";
        const string ExpectedDefaultHostName = "unknown-host";
        const string ExpectedMulltipleHostName = "multiple-host";
        const string ExpectedPath = "/path/path/";
        const string ExpectedPathBase = "/pathbase";
        const string ExpectedQueryString = "?queryType=1";

        [Fact]
        public void TestGetUriThrowsExceptionOnNullRequestObject()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    HttpRequestExtensions.GetUri(null);
                });
        }

        [Fact]
        public void TestGetUriThrowsExceptionOnRequestObjectSchemeIsEmpty()
        {
            var request = new DefaultHttpContext().Request;

            var exception = Assert.Throws<ArgumentException>(
                () =>
                {
                    HttpRequestExtensions.GetUri(request);
                });

            Assert.True(exception.Message.Contains("Scheme"), "Scheme is not mentioned in the exception");
        }

        [Fact]
        public void TestGetUriUsesDefaultHostNameOnRequestObjectHostIsNotSpecified()
        {
            var request = new DefaultHttpContext().Request;
            request.Scheme = ExpectedSchema;

            var uri = HttpRequestExtensions.GetUri(request);
            Assert.Equal(
                new Uri(string.Format(CultureInfo.InvariantCulture, "{0}://{1}", ExpectedSchema, ExpectedDefaultHostName)),
                uri);
        }

        [Fact]
        public void TestGetUriUsesMultipleHostNameOnRequestWithManyHostsSpecified()
        {
            var request = new DefaultHttpContext().Request;
            request.Scheme = ExpectedSchema;
            request.Host = new HostString("host1,host2");

            var uri = HttpRequestExtensions.GetUri(request);

            Assert.Equal(
                new Uri(string.Format(CultureInfo.InvariantCulture, "{0}://{1}", ExpectedSchema, ExpectedMulltipleHostName)),
                uri);
        }

        [Fact]
        public void TestGetUriReturnsCorrectUriIfRequestObjectSchemeAndHostAreSpecified()
        {
            var request = new DefaultHttpContext().Request;

            request.Scheme = ExpectedSchema;
            request.Host = new HostString(ExpectedHostName);

            var uri = HttpRequestExtensions.GetUri(request);

            Assert.Equal(
                new Uri(string.Format(CultureInfo.InvariantCulture, "{0}://{1}", ExpectedSchema, ExpectedHostName)),
                uri);
        }

        [Fact]
        public void TestGetUriReturnsCorrectUriIfRequestObjectSchemeAndHostAndPathAreSpecified()
        {
            var request = new DefaultHttpContext().Request;

            request.Scheme = ExpectedSchema;
            request.Host = new HostString(ExpectedHostName);
            request.Path = new PathString(ExpectedPath);

            var uri = HttpRequestExtensions.GetUri(request);

            Assert.Equal(
                new Uri(string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", ExpectedSchema, ExpectedHostName, ExpectedPath)),
                uri);
        }

        [Fact]
        public void TestGetUriReturnsCorrectUriIfRequestObjectSchemeAndHostAndPathAndQueryStringAreSpecified()
        {
            var request = new DefaultHttpContext().Request;

            request.Scheme = ExpectedSchema;
            request.Host = new HostString(ExpectedHostName);
            request.Path = new PathString(ExpectedPath);
            request.QueryString = new QueryString(ExpectedQueryString);

            var uri = HttpRequestExtensions.GetUri(request);

            Assert.Equal(
                new Uri(string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}{3}", ExpectedSchema, ExpectedHostName, ExpectedPath, ExpectedQueryString)),
                uri);
        }

        [Fact]
        public void TestGetUriReturnsCorrectUriIfRequestObjectSchemeAndHostAndPathBaseAndPathAndQueryStringAreSpecified()
        {
            var request = new DefaultHttpContext().Request;

            request.Scheme = ExpectedSchema;
            request.Host = new HostString(ExpectedHostName);
            request.PathBase = new PathString(ExpectedPathBase);
            request.Path = new PathString(ExpectedPath);
            request.QueryString = new QueryString(ExpectedQueryString);

            var uri = HttpRequestExtensions.GetUri(request);

            Assert.Equal(
                new Uri(string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}{3}{4}", ExpectedSchema, ExpectedHostName, ExpectedPathBase, ExpectedPath, ExpectedQueryString)),
                uri);
        }
    }
}