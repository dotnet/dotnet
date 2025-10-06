namespace Microsoft.ApplicationInsights.AspNetCore.Tests.TelemetryInitializers
{
    using System;
    using Microsoft.ApplicationInsights.AspNetCore.Implementation;
    using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
    using Microsoft.ApplicationInsights.AspNetCore.Tests.Helpers;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.AspNetCore.Http;

    using Xunit;

    [Trait("Trait", "RoleName")]
    public class AzureAppServiceRoleNameFromHostNameHeaderInitializerTests : IDisposable
    {
        public AzureAppServiceRoleNameFromHostNameHeaderInitializerTests()
        {
            Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", "SomeName");
        }

        [Fact]
        public void VerifyInitializerWorksAsExpected()
        {
            try
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", "a.b.c.azurewebsites.net");

                var initializer = new AzureAppServiceRoleNameFromHostNameHeaderInitializer(webAppSuffix: ".azurewebsites.net");

                var requestTelemetry1 = new RequestTelemetry();
                initializer.Initialize(requestTelemetry1);
                Assert.Equal("a.b.c", requestTelemetry1.Context.Cloud.RoleName);
            }
            finally
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", null);
            }
        }

        [Fact]
        public void VerifyInitializerCanBeChangedAfterConstructor()
        {
            try
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", "a.b.c.azurewebsites.net");

                var initializer = new AzureAppServiceRoleNameFromHostNameHeaderInitializer(webAppSuffix: ".azurewebsites.net");
                
                var requestTelemetry1 = new RequestTelemetry();
                initializer.Initialize(requestTelemetry1);
                Assert.Equal("a.b.c", requestTelemetry1.Context.Cloud.RoleName);

                initializer.WebAppSuffix = ".c.azurewebsites.net";

                var requestTelemetry2 = new RequestTelemetry();
                initializer.Initialize(requestTelemetry2);
                Assert.Equal("a.b", requestTelemetry2.Context.Cloud.RoleName);
            }
            finally
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", null);
            }
        }

        [Fact]
        public void VerifyInitializerRespectsChangesToRoleNameContainerRoleName()
        {
            try
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", "a.b.c.azurewebsites.net");
                var ac = new HttpContextAccessor { HttpContext = null };

                var initializer = new AzureAppServiceRoleNameFromHostNameHeaderInitializer(webAppSuffix: ".azurewebsites.net");

                var requestTelemetry1 = new RequestTelemetry();
                initializer.Initialize(requestTelemetry1);
                Assert.Equal("a.b.c", requestTelemetry1.Context.Cloud.RoleName);

                RoleNameContainer.Instance.RoleName = "test";

                var requestTelemetry2 = new RequestTelemetry();
                initializer.Initialize(requestTelemetry2);
                Assert.Equal("test", requestTelemetry2.Context.Cloud.RoleName);
            }
            finally
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", null);
            }
        }

        [Fact]
        public void InitializeFallsbackToEnvIfHttpContextIsUnavailableWithAzureWebsitesHostnameending()
        {
            try
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", "RoleNameEnv.azurewebsites.net");
                var ac = new HttpContextAccessor { HttpContext = null };

                var initializer = new AzureAppServiceRoleNameFromHostNameHeaderInitializer();
                var req = new RequestTelemetry();
                initializer.Initialize(req);

                Assert.Equal("RoleNameEnv", req.Context.Cloud.RoleName);
            }
            finally
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", null);
            }
        }

        [Fact]
        public void InitializeFallsbackToEnvIfRequestServicesAreUnavailable()
        {
            try
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", "RoleNameEnv");
                var ac = new HttpContextAccessor { HttpContext = new DefaultHttpContext() };

                var initializer = new AzureAppServiceRoleNameFromHostNameHeaderInitializer();
                var req = new RequestTelemetry();
                initializer.Initialize(req);

                Assert.Equal("RoleNameEnv", req.Context.Cloud.RoleName);
            }
            finally
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", null);
            }
        }

        [Fact]
        public void InitializeDoesNotThrowIfRequestIsUnavailable()
        {
            var initializer = new AzureAppServiceRoleNameFromHostNameHeaderInitializer();

            initializer.Initialize(new EventTelemetry());
        }

        [Fact]
        public void InitializeFallsbackToEnvIfRequestIsUnavailable()
        {
            try
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", "RoleNameEnv");
                var initializer = new AzureAppServiceRoleNameFromHostNameHeaderInitializer();
                var req = new RequestTelemetry();
                initializer.Initialize(req);

                Assert.Equal("RoleNameEnv", req.Context.Cloud.RoleName);
            }
            finally
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", null);
            }
        }
        
        [Fact]
        public void InitializeFallsbackToEnvIfHostNameIsEmptyInHeader()
        {
            try
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", "RoleNameEnv");
                var requestTelemetry = new RequestTelemetry();
                var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(requestTelemetry);

                contextAccessor.HttpContext.Request.Headers.Add("WAS-DEFAULT-HOSTNAME", new string[] { string.Empty });

                var initializer = new AzureAppServiceRoleNameFromHostNameHeaderInitializer();

                initializer.Initialize(requestTelemetry);

                Assert.Equal("RoleNameEnv", requestTelemetry.Context.Cloud.RoleName);
            }
            finally
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", null);
            }
        }
        
        [Fact]
        public void InitializeDoesNotOverrideRoleName()
        {
            var requestTelemetry = new RequestTelemetry();
            requestTelemetry.Context.Cloud.RoleName = "ExistingRoleName";
            
            var contextAccessor = HttpContextAccessorHelper.CreateHttpContextAccessor(requestTelemetry);
            contextAccessor.HttpContext.Request.Headers.Add("WAS-DEFAULT-HOSTNAME", new string[] { "MyAppServiceProd" });

            var initializer = new AzureAppServiceRoleNameFromHostNameHeaderInitializer();

            initializer.Initialize(requestTelemetry);

            Assert.Equal("ExistingRoleName", requestTelemetry.Context.Cloud.RoleName);
        }

        [Fact]
        public void InitializewithNoEnvToHostNameHeader()
        {
            Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", null);
            var ac = new HttpContextAccessor { HttpContext = null };

            var initializer = new AzureAppServiceRoleNameFromHostNameHeaderInitializer();

            var requestTelemetry = new RequestTelemetry();
            initializer.Initialize(requestTelemetry);
            Assert.Null(requestTelemetry.Context.Cloud.RoleName);
        }

        public void Dispose()
        {
            Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", null);
        }
    }
}