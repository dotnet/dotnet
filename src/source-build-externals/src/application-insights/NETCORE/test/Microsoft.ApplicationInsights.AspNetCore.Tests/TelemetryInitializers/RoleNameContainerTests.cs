using System;

using Microsoft.ApplicationInsights.AspNetCore.Implementation;
using Microsoft.AspNetCore.Http;

using Xunit;

namespace Microsoft.ApplicationInsights.AspNetCore.Tests.TelemetryInitializers
{
    [Trait("Trait", "RoleName")]
    public class RoleNameContainerTests : IDisposable
    {
        public RoleNameContainerTests()
        {
            Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", "initialize");
        }

        [Fact]
        public void VerifyCanSetRoleNameFromEnvironmentVariableAndSetsIsWebAppProperty()
        {
            try
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", "a.b.c.azurewebsites.net");

                var roleNameContainer = new RoleNameContainer(hostNameSuffix: ".azurewebsites.net");

                Assert.Equal("a.b.c", roleNameContainer.RoleName);
                Assert.True(roleNameContainer.IsAzureWebApp);
            }
            finally
            {
                this.ClearEnvironmentVariable();
            }
        }

        [Fact (Skip = "this test fails when run in parallel because other tests modify the environment variable.")]
        public void VerifyWhenEnvironmentVariableIsNull()
        {
            try
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", null);

                var roleNameContainer = new RoleNameContainer(hostNameSuffix: ".azurewebsites.net");

                Assert.Equal(string.Empty, roleNameContainer.RoleName);
                Assert.False(roleNameContainer.IsAzureWebApp);
            }
            finally
            {
                this.ClearEnvironmentVariable();
            }
        }

        [Fact]
        public void VerifyCanSetRoleNameFromHeaders()
        {
            var roleNameContainer = new RoleNameContainer(hostNameSuffix: ".azurewebsites.net");

            var headers = new HeaderDictionary();
            headers.Add("WAS-DEFAULT-HOSTNAME", "d.e.f.azurewebsites.net");
            roleNameContainer.Set(headers);

            Assert.Equal("d.e.f", roleNameContainer.RoleName);
        }

        [Fact]
        public void VerifyEmptyHeadersDoNotOverwriteEnvironmentVaribaleValue()
        {
            try
            {
                Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", "a.b.c.azurewebsites.net");
                var roleNameContainer = new RoleNameContainer(hostNameSuffix: ".azurewebsites.net");

                Assert.Equal("a.b.c", roleNameContainer.RoleName);

                roleNameContainer.Set(new HeaderDictionary()); // empty headers

                Assert.Equal("a.b.c", roleNameContainer.RoleName);
            }
            finally
            {
                this.ClearEnvironmentVariable();
            }
        }

        public void Dispose() => this.ClearEnvironmentVariable();

        private void ClearEnvironmentVariable() => Environment.SetEnvironmentVariable("WEBSITE_HOSTNAME", null);
    }
}
