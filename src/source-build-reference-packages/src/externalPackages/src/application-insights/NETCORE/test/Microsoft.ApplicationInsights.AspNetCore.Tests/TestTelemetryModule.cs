namespace Microsoft.ApplicationInsights.AspNetCore.Tests
{
    using System;    
    using Microsoft.ApplicationInsights.Extensibility;

    internal class TestTelemetryModule : ITelemetryModule
    {        
        public TestTelemetryModule()
        {                        
            this.IsInitialized = false;
        }

        public bool IsInitialized { get; private set; }

        public string CustomProperty { get; set; }

        public void Initialize(TelemetryConfiguration configuration)
        {
            this.IsInitialized = true;
        }
    }
}
