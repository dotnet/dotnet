namespace Microsoft.ApplicationInsights.AspNetCore.Tests.ContextInitializers
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Net.NetworkInformation;
    using Helpers;
    using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.AspNetCore.Http;
    using Xunit;

    public class DomainNameRoleInstanceTelemetryInitializerTests
    {
        private const string TestListenerName = "TestListener";              
        
        [Fact]
        public void RoleInstanceNameIsSetToDomainAndHost()
        {            
            var source = new DomainNameRoleInstanceTelemetryInitializer();
            var requestTelemetry = new RequestTelemetry();
            source.Initialize(requestTelemetry);

            string hostName = Dns.GetHostName();

#if NETFRAMEWORK
            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            if (hostName.EndsWith(domainName, StringComparison.OrdinalIgnoreCase) == false)
            {
                hostName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", hostName, domainName);
            }
#endif

            Assert.Equal(hostName, requestTelemetry.Context.Cloud.RoleInstance);            
        }

        [Fact]
        public void ContextInitializerDoesNotOverrideMachineName()
        {            
            var source = new DomainNameRoleInstanceTelemetryInitializer();
            var requestTelemetry = new RequestTelemetry();
            requestTelemetry.Context.Cloud.RoleInstance = "Test";            
            source.Initialize(requestTelemetry);
            Assert.Equal("Test", requestTelemetry.Context.Cloud.RoleInstance);            
        }
    }
}