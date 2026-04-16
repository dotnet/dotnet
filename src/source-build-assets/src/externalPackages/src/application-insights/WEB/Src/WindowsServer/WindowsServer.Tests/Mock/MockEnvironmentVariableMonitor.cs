namespace Microsoft.ApplicationInsights.WindowsServer.Mock
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;

    internal class MockEnvironmentVariableMonitor : EnvironmentVariableMonitor
    {
        public Dictionary<string, string> ExpectedEnvVars;
        private List<string> updatedVars;
        private bool hasBeenUpdated = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockEnvironmentVariableMonitor"/> class.
        /// Create a mocked-up instance of the abstract class EnvironmentVariableMonitor, disabling any
        /// timed updates to ensure unit tests are determinant. Testers must use 'PerformCheckForUpdatedVariables'
        /// to simulate update timer calls.
        /// </summary>
        /// <param name="envVars">List of environment variables to check for updates during the test. Hint: Keep these random/testable as a unit.</param>
        public MockEnvironmentVariableMonitor(IEnumerable<string> envVars) : base(envVars, TimeSpan.FromMilliseconds(-1))
        {
            this.isEnabled = false;
            this.updatedVars = new List<string>();
            this.ExpectedEnvVars = new Dictionary<string, string>();
            foreach (string key in envVars)
            {
                this.ExpectedEnvVars.Add(key, Environment.GetEnvironmentVariable(key));
            }
        }

        public bool DetectedUpdatedVarValue
        {
            get
            {
                var updatedStatus = this.hasBeenUpdated;
                this.hasBeenUpdated = false;
                return updatedStatus;
            }

            set => this.hasBeenUpdated = true;
        }

        public List<string> GetAndClearAllUpdatedEnvVars()
        {
            var updatedVars = new List<string>(this.updatedVars);
            this.updatedVars.Clear();
            return updatedVars;
        }

        public void PerformCheckForUpdatedVariables()
        {
            this.CheckVariablesIntermittent(null);
        }

        protected override void OnEnvironmentVariableUpdated()
        {
            this.DetectedUpdatedVarValue = true;
            this.updatedVars.Clear();

            var keys = this.ExpectedEnvVars.Keys.ToList();
            foreach (string key in keys)
            {
                var had = this.ExpectedEnvVars[key];
                var got = Environment.GetEnvironmentVariable(key);
                if (!had.Equals(got, StringComparison.Ordinal))
                {
                    this.updatedVars.Add(key);
                    this.ExpectedEnvVars[key] = got;
                }
            }
        }
    }
}
