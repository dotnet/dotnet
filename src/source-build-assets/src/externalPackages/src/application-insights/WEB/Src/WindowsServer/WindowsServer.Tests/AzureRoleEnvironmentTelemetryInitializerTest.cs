#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.Azure;
    using Microsoft.ApplicationInsights.WindowsServer.Azure.Emulation;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Web.TestFramework;
    using Assert = Xunit.Assert;

    [TestClass]
    public class AzureRoleEnvironmentTelemetryInitializerTest
    {
        [TestMethod]
        public void AzureRoleEnvironmentTelemetryInitializerDoesNotOverrideRoleName()
        {
            var telemetryItem = new EventTelemetry();
            AzureRoleEnvironmentContextReader.Instance = null;
            AzureRoleEnvironmentTelemetryInitializer initializer = new AzureRoleEnvironmentTelemetryInitializer();
            telemetryItem.Context.Cloud.RoleName = "Test";
            initializer.Initialize(telemetryItem);

            Assert.Equal("Test", telemetryItem.Context.Cloud.RoleName);
        }

        [TestMethod]
        public void AzureRoleEnvironmentTelemetryInitializerDoesNotOverrideRoleInstance()
        {
            var telemetryItem = new EventTelemetry();
            AzureRoleEnvironmentTelemetryInitializer initializer = new AzureRoleEnvironmentTelemetryInitializer();
            telemetryItem.Context.Cloud.RoleInstance = "Test";
            initializer.Initialize(telemetryItem);

            Assert.Equal("Test", telemetryItem.Context.Cloud.RoleInstance);
        }

        [TestMethod]
        public void AzureRoleEnvironmentTelemetryInitializerDoesNotOverrideNodeName()
        {
            var telemetryItem = new EventTelemetry();
            AzureRoleEnvironmentContextReader.Instance = null;
            AzureRoleEnvironmentTelemetryInitializer initializer = new AzureRoleEnvironmentTelemetryInitializer();
            telemetryItem.Context.GetInternalContext().NodeName = "Test";
            initializer.Initialize(telemetryItem);

            Assert.Equal("Test", telemetryItem.Context.GetInternalContext().NodeName);
        }

        [TestMethod]
        public void AzureRoleEnvironmentTelemetryInitializerSetsTelemetryContextPropertiesToNullWhenNotRunningInsideAzureCloudService()
        {
            // This test asssumes that it is not running inside a cloud service.
            // Its Ok even if Azure ServiceRunTime dlls are in the GAC, as IsAvailable() will return false, and hence 
            // no context will be further attempted to be read.
            var telemetryItem = new EventTelemetry();
            AzureRoleEnvironmentContextReader.Instance = null;
            AzureRoleEnvironmentContextReader.AssemblyLoaderType = null;
            AzureRoleEnvironmentTelemetryInitializer initializer = new AzureRoleEnvironmentTelemetryInitializer();
            initializer.Initialize(telemetryItem);

            Assert.Null(telemetryItem.Context.Cloud.RoleName);
            Assert.Null(telemetryItem.Context.Cloud.RoleInstance);
            Assert.Null(telemetryItem.Context.GetInternalContext().NodeName);
        }

        [TestMethod]
        public void AzureRoleEnvironmentTelemetryInitializerDoNotPopulateContextIfRunningAzureWebApp()
        {
            try
            {
                // Set the ENV variable so as to trick app is running as Azure WebApp
                Environment.SetEnvironmentVariable("WEBSITE_SITE_NAME", "TestRoleName.AzureWebSites.net");

                // Initialize telemetry using AzureRoleEnvironmentTelemetryInitializer
                var telemetryItem = new EventTelemetry();
                AzureRoleEnvironmentTelemetryInitializer initializer = new AzureRoleEnvironmentTelemetryInitializer();
                initializer.Initialize(telemetryItem);

                // As app is running as Azure WebApp, AzureRoleEnvironmentTelemetryInitializer will not populate any context.
                Assert.Null(telemetryItem.Context.Cloud.RoleName);
                Assert.Null(telemetryItem.Context.Cloud.RoleInstance);
                Assert.Null(telemetryItem.Context.GetInternalContext().NodeName);
            }
            finally
            {
                Environment.SetEnvironmentVariable("WEBSITE_SITE_NAME", null);
            }
        }

        [TestMethod]
        [Description("Validates that requested DLL was loaded into separate AppDomain and not to the current domain. This test will fail if not run with admin privileges.")]
        public void AzureRoleEnvironmentTelemetryInitializerLoadDllToSeparateAppDomain()
        {
            // A random dll which is not already loaded to the current AppDomain but dropped into bin folder.
            string dllPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Microsoft.ApplicationInsights.Log4NetAppender.dll");

            Assert.True(File.Exists(dllPath));

            try
            {
                // Publish the dll to GAC to give a chance for  AzureRoleEnvironmentTelemetryInitializer to load it to a new AppDomain
                new System.EnterpriseServices.Internal.Publish().GacInstall(dllPath);

                // Validate that the dll is not loaded to test AppDomaion to begin with.
                var retrievedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(item => string.Equals(item.GetName().Name, "Microsoft.ApplicationInsights.Log4NetAppender", StringComparison.OrdinalIgnoreCase));
                Assert.Null(retrievedAssembly);

                // TestAssemblyLoader will load a random assembly (Microsoft.ApplicationInsights.Log4NetAppender.dll) and populate TestRoleName, TestRoleInstanceId into the fields.
                AzureRoleEnvironmentContextReader.AssemblyLoaderType = typeof(TestAzureServiceRuntimeAssemblyLoader);
                AzureRoleEnvironmentContextReader.Instance = null;

                // Create initializer - this will internally create separate appdomain and load assembly into it.
                AzureRoleEnvironmentTelemetryInitializer initializer = new AzureRoleEnvironmentTelemetryInitializer();

                // Validate that the dll is still not loaded to current appdomain.
                retrievedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(item => string.Equals(item.GetName().Name, "Microsoft.ApplicationInsights.Log4NetAppender", StringComparison.OrdinalIgnoreCase));
                Assert.Null(retrievedAssembly);

                // Validate that initializer has populated expected context properties. (set by TestAssemblyLoader)
                var telemetryItem = new EventTelemetry();
                initializer.Initialize(telemetryItem);
                Assert.Equal("TestRoleName", telemetryItem.Context.Cloud.RoleName);
                Assert.Equal("TestRoleInstanceId", telemetryItem.Context.Cloud.RoleInstance);
            }
            finally
            {
                new System.EnterpriseServices.Internal.Publish().GacRemove(dllPath);
            }
        }
    }
}
#endif