namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.Mock;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Assert = Xunit.Assert;

    [TestClass]
    public class EnvironmentVariableMonitorTests
    {
        private Dictionary<string, string> testEnvironmentVariables;
        private MockEnvironmentVariableMonitor testEnvironmentMonitor;

        [TestInitialize]
        public void BeforeEachTest()
        {
            this.testEnvironmentVariables = this.GetCurrentAppServiceEnvironmentVariableValues();
            this.testEnvironmentMonitor = new MockEnvironmentVariableMonitor(this.testEnvironmentVariables.Keys);
        }

        [TestCleanup]
        public void AfterEachTest()
        {
            this.RemoveTestEnvironmentVariablesFromEnvironment(this.testEnvironmentVariables);
        }

        [TestMethod]
        public void EnsureInstanceWorksAsIntended()
        {
            Assert.NotNull(AppServiceEnvironmentVariableMonitor.Instance);
        }

        [TestMethod]
        public void EnsureEnvironmentVariablesAreCapturedImmediately()
        {
            foreach (var kvp in this.testEnvironmentVariables)
            {
                string cachedVal = string.Empty;
                this.testEnvironmentMonitor.GetCurrentEnvironmentVariableValue(kvp.Key, ref cachedVal);
                Assert.Equal(kvp.Value, cachedVal, StringComparer.Ordinal);
            }
        }

        [TestMethod]
        public void ConfirmUpdatedEnvironmentIsNotDetectedPriorToUpdate()
        {
            foreach (var kvp in this.testEnvironmentVariables)
            {
                string updatedValue = Guid.NewGuid().ToString();
                Assert.NotEqual(kvp.Value, updatedValue, StringComparer.Ordinal);

                Environment.SetEnvironmentVariable(kvp.Key, updatedValue);

                string cachedValue = string.Empty;
                this.testEnvironmentMonitor.GetCurrentEnvironmentVariableValue(kvp.Key, ref cachedValue);
                Assert.Equal(kvp.Value, cachedValue, StringComparer.Ordinal);
            }
        }

        [TestMethod]
        public void ConfirmUpdatedEnvironmentIsDetectedPostUpdate()
        {
            var updatedVars = new Dictionary<string, string>();

            foreach (var kvp in this.testEnvironmentVariables)
            {
                string updatedValue = Guid.NewGuid().ToString();
                Assert.NotEqual(kvp.Value, updatedValue, StringComparer.Ordinal);

                Environment.SetEnvironmentVariable(kvp.Key, updatedValue);
                updatedVars.Add(kvp.Key, updatedValue);
            }

            this.testEnvironmentMonitor.PerformCheckForUpdatedVariables();
            Assert.True(this.testEnvironmentMonitor.DetectedUpdatedVarValue);

            foreach (var kvp in this.testEnvironmentVariables)
            {
                string cachedValue = string.Empty;
                this.testEnvironmentMonitor.GetCurrentEnvironmentVariableValue(kvp.Key, ref cachedValue);

                Assert.Equal(updatedVars[kvp.Key], cachedValue, StringComparer.Ordinal);
                Assert.NotEqual(kvp.Value, cachedValue, StringComparer.Ordinal);
            }
        }

        /// <summary>
        /// Create a set of environment variables that mimics the default values used by
        /// the AppServiceEnvVarMonitor, and a set of values for them. Each time this method
        /// is called the names and values of the environment variables will be unique as a Guid
        /// is used.
        /// </summary>
        /// <returns>Dictionary containing the environment variable names and their current values.</returns>
        private Dictionary<string, string> GetCurrentAppServiceEnvironmentVariableValues()
        {
            int testValueCount = 0;
            Dictionary<string, string> envVars = new Dictionary<string, string>();

            string testVarSuffix = Guid.NewGuid().ToString();
            foreach (string envVarName in AppServiceEnvironmentVariableMonitor.PreloadedMonitoredEnvironmentVariables)
            {
                string testVarName = string.Concat(envVarName, "_", testVarSuffix);
                string testVarValue = $"{testValueCount}_Stand-inValue_{testVarSuffix}_{testValueCount}";
                testValueCount++;
                Environment.SetEnvironmentVariable(testVarName, testVarValue);

                envVars.Add(testVarName, testVarValue);
            }

            return envVars;
        }

        /// <summary>
        /// Clean up the test environment by removing each set environment variable in the given dictionary.
        /// </summary>
        /// <param name="envVars">Environment variables currently set for a test method run.</param>
        private void RemoveTestEnvironmentVariablesFromEnvironment(Dictionary<string, string> envVars)
        {
            foreach (var kvp in envVars)
            {
                Environment.SetEnvironmentVariable(kvp.Key, string.Empty);
            }
        }
    }
}
