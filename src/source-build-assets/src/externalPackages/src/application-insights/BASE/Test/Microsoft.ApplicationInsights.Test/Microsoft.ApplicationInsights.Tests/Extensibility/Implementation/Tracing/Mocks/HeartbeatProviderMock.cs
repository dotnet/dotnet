namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.Mocks
{
    using System;
    using System.Collections.Generic;

    class HeartbeatProviderMock : IHeartbeatProvider
    {
        public Dictionary<string, HeartbeatPropertyPayload> HeartbeatProperties = new Dictionary<string, HeartbeatPropertyPayload>();

        public List<string> ExcludedProviders { get; set; }

        public List<string> ExcludedPropertyFields = new List<string>();

        public string InstrumentationKey { get; set; }

        public bool IsHeartbeatEnabled { get; set; }

        public bool EnableInstanceMetadata { get; set; }

        public TimeSpan HeartbeatInterval { get; set; }

        public IList<string> ExcludedHeartbeatProperties { get => this.ExcludedPropertyFields; }

        public IList<string> ExcludedHeartbeatPropertyProviders { get => this.ExcludedHeartbeatPropertyProviders; }

        public HeartbeatProviderMock()
        {
            this.InstrumentationKey = Guid.NewGuid().ToString();
            this.IsHeartbeatEnabled = true;
            this.EnableInstanceMetadata = true;
            this.HeartbeatInterval = TimeSpan.FromSeconds(31);
        }

        public bool AddHeartbeatProperty(string propertyName, string propertyValue, bool isHealthy)
        {
            return this.AddHeartbeatProperty(propertyName, false, propertyValue, isHealthy);
        }

        public bool SetHeartbeatProperty(string propertyName, string propertyValue, bool? isHealthy)
        {
            return this.SetHeartbeatProperty(propertyName, false, propertyValue, isHealthy);
        }

        public bool AddHeartbeatProperty(string propertyName, bool overrideDefaultField, string propertyValue, bool isHealthy)
        {
            this.HeartbeatProperties.Add(
                propertyName,
                new HeartbeatPropertyPayload()
                {
                    IsHealthy = isHealthy,
                    IsUpdated = true,
                    PayloadValue = propertyValue
                });

            return true;
        }

        public void Dispose()
        {
        }

        public void Initialize(TelemetryConfiguration configuration)
        {
        }

        public bool SetHeartbeatProperty(string propertyName, bool overrideDefaultField, string propertyValue = null, bool? isHealthy = null)
        {
            if (this.HeartbeatProperties.ContainsKey(propertyName))
            {
                HeartbeatPropertyPayload pl = this.HeartbeatProperties[propertyName];
                pl.IsHealthy = isHealthy.GetValueOrDefault(pl.IsHealthy);
                pl.PayloadValue = propertyValue ?? pl.PayloadValue;
                pl.IsUpdated = true;

                return true;
            }
            return false;
        }
    }
}
