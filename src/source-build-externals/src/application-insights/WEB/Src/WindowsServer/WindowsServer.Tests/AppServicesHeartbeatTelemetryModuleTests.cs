namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation.DataContracts;
    using Microsoft.ApplicationInsights.WindowsServer.Mock;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Assert = Xunit.Assert;

    [TestClass]
    public class AppServicesHeartbeatTelemetryModuleTests
    {
        private HeartbeatProviderMock testHeartbeatPropertyManager;
        private AppServicesHeartbeatTelemetryModule testAppServiceHbeatModule;
        private Dictionary<string, string> testEnvironmentVariables;

        [TestInitialize]
        public void BeforeEachTestMethod()
        {
            this.testHeartbeatPropertyManager = new HeartbeatProviderMock();
            this.testAppServiceHbeatModule = this.GetAppServiceHeartbeatModuleWithUniqueTestEnvVars(this.testHeartbeatPropertyManager);
            this.testEnvironmentVariables = this.GetEnvVarsAssociatedToModule(this.testAppServiceHbeatModule);
        }

        [TestCleanup]
        public void AfterEachTestMethod()
        {
            this.RemoveTestEnvVarsAssociatedToModule(this.testAppServiceHbeatModule);
        }

        [TestMethod]
        public void InitializeIsWorking()
        {
            this.testAppServiceHbeatModule.Initialize(null);

            foreach (var kvp in this.testAppServiceHbeatModule.WebHeartbeatPropertyNameEnvVarMap)
            {
                Assert.True(this.testHeartbeatPropertyManager.HbeatProps.ContainsKey(kvp.Key));
                Assert.Equal(this.testHeartbeatPropertyManager.HbeatProps[kvp.Key], this.testEnvironmentVariables[kvp.Value]);
            }
        }

        [TestMethod]
        [Description("This test causes a delay and must be updated to be more deterministic.")]
        [Owner("dekeeler")]
        public void UpdateEnvVarsWorksWhenEnvironmentValuesChange()
        {            
            this.testAppServiceHbeatModule.Initialize(null);

            // update each environment variable to have a different value
            foreach (var envVarKvp in this.testEnvironmentVariables)
            {
                string newVal = string.Concat(envVarKvp.Value, "_1");
                Environment.SetEnvironmentVariable(envVarKvp.Key, newVal);
            }

            // wait for the delay set into the monitor, plus one second to ensure we got updated
            Task.Delay(
                AppServiceEnvironmentVariableMonitor.MonitorInterval + TimeSpan.FromSeconds(1))
                .ConfigureAwait(false).GetAwaiter().GetResult();

            var updatedEnvVars = this.GetEnvVarsAssociatedToModule(this.testAppServiceHbeatModule);

            foreach (var kvp in this.testAppServiceHbeatModule.WebHeartbeatPropertyNameEnvVarMap)
            {
                Assert.True(this.testHeartbeatPropertyManager.HbeatProps.ContainsKey(kvp.Key));
                Assert.Equal(this.testHeartbeatPropertyManager.HbeatProps[kvp.Key], updatedEnvVars[kvp.Value]);
            }
        }

        [TestMethod]
        public void NoHeartbeatManagerAvailableDoesntThrow()
        {
            var appSrvHbeatModule = new AppServicesHeartbeatTelemetryModule();
            var envVars = this.GetEnvVarsAssociatedToModule(appSrvHbeatModule);

            try
            {
                appSrvHbeatModule.Initialize(null);
            }
            catch (Exception any)
            {
                Assert.False(any == null);
            }
        }

        [TestMethod]
        public void NoAppServicesEnvVarsWorksWithoutFailure()
        {
            // ensure all environment variables are set to nothing (remove them from the environment)
            this.RemoveTestEnvVarsAssociatedToModule(this.testAppServiceHbeatModule);

            this.testAppServiceHbeatModule.UpdateHeartbeatWithAppServiceEnvVarValues();
            foreach (var kvp in this.testAppServiceHbeatModule.WebHeartbeatPropertyNameEnvVarMap)
            {
                Assert.Null(this.testHeartbeatPropertyManager.HbeatProps[kvp.Key]);
            }
        }

        /// <summary>
        /// Return a dictionary containing the expected environment variables for the AppServicesHeartbeat module. If
        /// the environment does not contain a value for them, set the environment to have them.
        /// </summary>
        /// <returns>Dictionary with expected environment variable names as the key, current environment variable content as the value.</returns>
        private Dictionary<string, string> GetEnvVarsAssociatedToModule(AppServicesHeartbeatTelemetryModule testAppServicesHeartbeatModule)
        {
            Dictionary<string, string> uniqueTestEnvironmentVariables = new Dictionary<string, string>();
            foreach (var kvp in testAppServicesHeartbeatModule.WebHeartbeatPropertyNameEnvVarMap)
            {
                uniqueTestEnvironmentVariables.Add(kvp.Value, Environment.GetEnvironmentVariable(kvp.Value));
                if (string.IsNullOrEmpty(uniqueTestEnvironmentVariables[kvp.Value]))
                {
                    Environment.SetEnvironmentVariable(kvp.Value, kvp.Key);
                    uniqueTestEnvironmentVariables[kvp.Value] = kvp.Key;
                }
            }

            return uniqueTestEnvironmentVariables;
        }

        private AppServicesHeartbeatTelemetryModule GetAppServiceHeartbeatModuleWithUniqueTestEnvVars(HeartbeatProviderMock heartbeatProvider)
        {
            var appServicesHbeatModule = new AppServicesHeartbeatTelemetryModule(heartbeatProvider);
            string testSuffix = Guid.NewGuid().ToString();
            for (int i = 0; i < appServicesHbeatModule.WebHeartbeatPropertyNameEnvVarMap.Length; ++i)
            {
                var kvp = appServicesHbeatModule.WebHeartbeatPropertyNameEnvVarMap[i];
                appServicesHbeatModule.WebHeartbeatPropertyNameEnvVarMap[i] = new KeyValuePair<string, string>(kvp.Key, string.Concat(kvp.Value, "_", testSuffix));
            }

            return appServicesHbeatModule;
        }

        private void RemoveTestEnvVarsAssociatedToModule(AppServicesHeartbeatTelemetryModule appServicesHbeatModule)
        {
            foreach (var kvp in appServicesHbeatModule.WebHeartbeatPropertyNameEnvVarMap)
            {
                Environment.SetEnvironmentVariable(kvp.Value, string.Empty);
            }
        }
    }
}
