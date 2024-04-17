namespace Microsoft.ApplicationInsights.AspNetCore.Tests.Helpers
{
    using System;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    public static class CommonMocks
    {
        public const string InstrumentationKey = "REQUIRED";
        public const string TestApplicationId = nameof(TestApplicationId);

        public static TelemetryClient MockTelemetryClient(Action<ITelemetry> onSendCallback, bool isW3C = true)
        {
            if(isW3C)
            {
                Activity.DefaultIdFormat = ActivityIdFormat.W3C;                
            }
            else
            {
                Activity.DefaultIdFormat = ActivityIdFormat.Hierarchical;                
            }
            Activity.ForceDefaultIdFormat = true;

            return new TelemetryClient(new TelemetryConfiguration()
            {
                InstrumentationKey = InstrumentationKey,
                TelemetryChannel = new FakeTelemetryChannel { OnSend = onSendCallback },                
            });
        }

        public static TelemetryClient MockTelemetryClient(Action<ITelemetry> onSendCallback, TelemetryConfiguration configuration)
        {
            configuration.InstrumentationKey = InstrumentationKey;
            configuration.TelemetryChannel = new FakeTelemetryChannel {OnSend = onSendCallback};
            return new TelemetryClient(configuration);
        }

        internal static IApplicationIdProvider GetMockApplicationIdProvider()
        {
            return new MockApplicationIdProvider(InstrumentationKey, TestApplicationId);
        }
    }
}
