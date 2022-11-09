namespace Microsoft.ApplicationInsights.AspNetCore.Tests.TelemetryInitializers
{
    using System;
    using System.Net;

    using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
    using Microsoft.ApplicationInsights.AspNetCore.Tests.Helpers;
    using Microsoft.ApplicationInsights.DataContracts;
    using Xunit;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;

    public class ClientIpHeaderTelemetryInitializerTests
    {
        [Fact]
        public void InitializeThrowIfHttpContextAccessorIsNull()
        {
            Assert.ThrowsAny<ArgumentNullException>(() => { var initializer = new ClientIpHeaderTelemetryInitializer(null);  });
        }

        [Fact]
        public void InitializeDoesNotThrowIfHttpContextIsUnavailable()
        {
            var ac = new HttpContextAccessor { HttpContext = null };
            
            var initializer = new ClientIpHeaderTelemetryInitializer(ac);

            initializer.Initialize(new RequestTelemetry());
        }

        [Fact]
        public void InitializeDoesNotThrowIfRequestServicesAreUnavailable()
        {
            var ac = new HttpContextAccessor { HttpContext = new DefaultHttpContext() };
            
            var initializer = new ClientIpHeaderTelemetryInitializer(ac);

            initializer.Initialize(new RequestTelemetry());
        }

        [Fact]
        public void InitializeDoesNotThrowIfRequestIsUnavailable()
        {
            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessorWithoutRequest(new DefaultHttpContext(), new RequestTelemetry());
            
            var initializer = new ClientIpHeaderTelemetryInitializer(contextAccessor);

            initializer.Initialize(new EventTelemetry());
        }

        [Fact]
        public void InitializeDoesNotThrowIfHeaderCollectionIsUnavailable()
        {
            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessorWithoutRequest(new DefaultHttpContext(), new RequestTelemetry());

            var initializer = new ClientIpHeaderTelemetryInitializer(contextAccessor);

            initializer.Initialize(new EventTelemetry());
        }

        [Fact]
        public void InitializeDoesNotThrowIfRemoteIpAddressIsNull()
        {
            var requestTelemetry = new RequestTelemetry();
            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(requestTelemetry);

            contextAccessor.HttpContext.Features.Set<IHttpConnectionFeature>(new HttpConnectionFeature());
           
            var initializer = new ClientIpHeaderTelemetryInitializer(contextAccessor);

            initializer.Initialize(requestTelemetry);
        }

        [Fact]
        public void InitializeSetsIpFromRemoteIpAddress()
        {
            var requestTelemetry = new RequestTelemetry();
            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(requestTelemetry);

            var httpConnectionFeature = new HttpConnectionFeature
            {
                RemoteIpAddress = new IPAddress(new byte[] {1, 2, 3, 4})
            };
            contextAccessor.HttpContext.Features.Set<IHttpConnectionFeature>(httpConnectionFeature);

            var initializer = new ClientIpHeaderTelemetryInitializer(contextAccessor);

            initializer.Initialize(requestTelemetry);

            Assert.Equal("1.2.3.4", requestTelemetry.Context.Location.Ip);
        }

        [Fact]
        public void InitializeSetsIPFromStandardHeader()
        {
            var requestTelemetry = new RequestTelemetry();
            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(requestTelemetry);

            contextAccessor.HttpContext.Request.Headers.Add("X-Forwarded-For", new string[] { "127.0.0.3" });

            var initializer = new ClientIpHeaderTelemetryInitializer(contextAccessor);

            initializer.Initialize(requestTelemetry);

            Assert.Equal("127.0.0.3", requestTelemetry.Context.Location.Ip);
        }

        [Fact]
        public void InitializeSetsIPFromCustomHeader()
        {
            var requestTelemetry = new RequestTelemetry();
            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(requestTelemetry);
            contextAccessor.HttpContext.Request.Headers.Add("HEADER", new string[] { "127.0.0.3;127.0.0.4" });

            var initializer = new ClientIpHeaderTelemetryInitializer(contextAccessor);
            initializer.HeaderNames.Add("HEADER");
            initializer.HeaderValueSeparators = ",;";

            initializer.Initialize(requestTelemetry);

            Assert.Equal("127.0.0.3", requestTelemetry.Context.Location.Ip);
        }

        [Fact]
        public void InitializeDoesNotOverrideIPProvidedInline()
        {
            var requestTelemetry = new RequestTelemetry();
            requestTelemetry.Context.Location.Ip = "127.0.0.4";
            
            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(requestTelemetry);
            contextAccessor.HttpContext.Request.Headers.Add("X-Forwarded-For", new string[] { "127.0.0.3" });

            var initializer = new ClientIpHeaderTelemetryInitializer(contextAccessor);

            initializer.Initialize(requestTelemetry);

            Assert.Equal("127.0.0.4", requestTelemetry.Context.Location.Ip);
        }
    }
}