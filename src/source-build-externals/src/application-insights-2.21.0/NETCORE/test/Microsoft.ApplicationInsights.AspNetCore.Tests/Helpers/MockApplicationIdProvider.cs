using Microsoft.ApplicationInsights.Extensibility;

namespace Microsoft.ApplicationInsights.AspNetCore.Tests.Helpers
{
    internal class MockApplicationIdProvider : IApplicationIdProvider
    {
        private readonly string expectedInstrumentationKey;
        private readonly string applicationId;

        public MockApplicationIdProvider()
        {
        }

        public MockApplicationIdProvider(string expectedInstrumentationKey, string applicationId)
        {
            this.expectedInstrumentationKey = expectedInstrumentationKey;
            this.applicationId = applicationId;
        }

        public bool TryGetApplicationId(string instrumentationKey, out string applicationId)
        {
            if (this.expectedInstrumentationKey == instrumentationKey)
            {
                applicationId = this.applicationId;
                return true;
            }

            applicationId = null;
            return false;
        }
    }
}
