namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Assert = Xunit.Assert;

    [TestClass]
    public class AzureWebAppRoleEnvironmentTelemetryInitializerTest
    {
        [TestMethod]
        public void AzureWebAppRoleEnvironmentTelemetryInitializerSetsRoleName()
        {
            var telemetryItem = new EventTelemetry();

            var testVarName = "WEBSITE_" + Guid.NewGuid().ToString() + "_HOSTNAME";
            Environment.SetEnvironmentVariable(testVarName, "TestRoleName.azurewebsites.net");

            var initializer = new AzureWebAppRoleEnvironmentTelemetryInitializer()
            {
                WebAppHostNameEnvironmentVariable = testVarName
            };

            initializer.Initialize(telemetryItem);

            Assert.Equal("TestRoleName", telemetryItem.Context.Cloud.RoleName);            
            Assert.Equal("TestRoleName.azurewebsites.net", telemetryItem.Context.GetInternalContext().NodeName);

            Environment.SetEnvironmentVariable(testVarName, null);
        }

        [TestMethod]
        public void AzureWebAppRoleEnvironmentTelemetryInitializerDoesNotOverrideRoleName()
        {
            var testVarName = "WEBSITE_" + Guid.NewGuid().ToString() + "_HOSTNAME";
            Environment.SetEnvironmentVariable(testVarName, "TestRoleName.azurewebsites.net");

            var telemetryItem = new EventTelemetry();
            telemetryItem.Context.Cloud.RoleName = "Test";

            var initializer = new AzureWebAppRoleEnvironmentTelemetryInitializer()
            {
                WebAppHostNameEnvironmentVariable = testVarName
            };
            initializer.Initialize(telemetryItem);

            Assert.Equal("Test", telemetryItem.Context.Cloud.RoleName);            
            Assert.Equal("TestRoleName.azurewebsites.net", telemetryItem.Context.GetInternalContext().NodeName);

            Environment.SetEnvironmentVariable(testVarName, null);
        }

        [TestMethod]
        public void AzureWebAppRoleEnvironmentTelemetryInitializerDoesNotOverrideRoleInstance()
        {
            var testVarName = "WEBSITE_" + Guid.NewGuid().ToString() + "_HOSTNAME";
            Environment.SetEnvironmentVariable(testVarName, "TestRoleName.azurewebsites.net");

            var telemetryItem = new EventTelemetry();
            telemetryItem.Context.Cloud.RoleInstance = "Test";

            var initializer = new AzureWebAppRoleEnvironmentTelemetryInitializer()
            {
                WebAppHostNameEnvironmentVariable = testVarName
            };
            initializer.Initialize(telemetryItem);

            Assert.Equal("TestRoleName", telemetryItem.Context.Cloud.RoleName);
            Assert.Equal("Test", telemetryItem.Context.Cloud.RoleInstance);
            Assert.Equal("TestRoleName.azurewebsites.net", telemetryItem.Context.GetInternalContext().NodeName);

            Environment.SetEnvironmentVariable(testVarName, null);
        }

        [TestMethod]
        public void AzureWebAppRoleEnvironmentTelemetryInitializerDoesNotOverrideNodeName()
        {
            var testVarName = "WEBSITE_" + Guid.NewGuid().ToString() + "_HOSTNAME";
            Environment.SetEnvironmentVariable(testVarName, "TestRoleName.azurewebsites.net");

            var telemetryItem = new EventTelemetry();
            telemetryItem.Context.GetInternalContext().NodeName = "Test";

            var initializer = new AzureWebAppRoleEnvironmentTelemetryInitializer()
            {
                WebAppHostNameEnvironmentVariable = testVarName
            };
            initializer.Initialize(telemetryItem);

            Assert.Equal("TestRoleName", telemetryItem.Context.Cloud.RoleName);            
            Assert.Equal("Test", telemetryItem.Context.GetInternalContext().NodeName);

            Environment.SetEnvironmentVariable(testVarName, null);
        }

        [TestMethod]
        public void AzureWebAppRoleEnvironmentTelemetryInitializerEmptyVariable()
        {
            var testVarName = "WEBSITE_" + Guid.NewGuid().ToString() + "_HOSTNAME";
            Environment.SetEnvironmentVariable(testVarName, null);

            var telemetryItem = new EventTelemetry();

            var initializer = new AzureWebAppRoleEnvironmentTelemetryInitializer()
            {
                WebAppHostNameEnvironmentVariable = testVarName
            };
            initializer.Initialize(telemetryItem);

            Assert.Null(telemetryItem.Context.Cloud.RoleName);
            Assert.Null(telemetryItem.Context.Cloud.RoleInstance);
            Assert.Null(telemetryItem.Context.GetInternalContext().NodeName);
        }
    }
}
