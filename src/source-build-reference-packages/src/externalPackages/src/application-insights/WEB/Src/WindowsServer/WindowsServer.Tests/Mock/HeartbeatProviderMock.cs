namespace Microsoft.ApplicationInsights.WindowsServer.Mock
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    internal class HeartbeatProviderMock : IHeartbeatPropertyManager
    {
        public bool Enabled = true;
        public TimeSpan Interval = TimeSpan.FromMinutes(15);
        public List<string> ExcludedProps = new List<string>();
        public List<string> ExcludedPropProviders = new List<string>();
        public Dictionary<string, string> HbeatProps = new Dictionary<string, string>();
        public Dictionary<string, bool> HbeatHealth = new Dictionary<string, bool>();

        public bool IsHeartbeatEnabled { get => this.Enabled; set => this.Enabled = value; }

        public TimeSpan HeartbeatInterval { get => this.Interval; set => this.Interval = value; }

        public IList<string> ExcludedHeartbeatProperties => this.ExcludedProps;

        public IList<string> ExcludedHeartbeatPropertyProviders => this.ExcludedPropProviders;

        public bool AddHeartbeatProperty(string propertyName, string propertyValue, bool isHealthy)
        {
            if (!this.HbeatProps.ContainsKey(propertyName))
            {
                this.HbeatProps.Add(propertyName, propertyValue);
                this.HbeatHealth.Add(propertyName, isHealthy);
                return true;
            }

            return false;
        }

        public bool SetHeartbeatProperty(string propertyName, string propertyValue = null, bool? isHealthy = null)
        {
            if (!string.IsNullOrEmpty(propertyName) && this.HbeatProps.ContainsKey(propertyName))
            {
                if (!string.IsNullOrEmpty(propertyValue))
                {
                    this.HbeatProps[propertyName] = propertyValue;
                }

                if (isHealthy.HasValue)
                {
                    this.HbeatHealth[propertyName] = isHealthy.GetValueOrDefault(false);
                }

                return true;
            }

            return false;
        }
    }
}
