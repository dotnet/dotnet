namespace Microsoft.ApplicationInsights
{
    internal class Constants
    {
        internal const string ProfileQueryEndpoint = "https://dc.services.visualstudio.com/api/profiles/{0}/appId";

        internal const string EventNameForUnknownTelemetry = "ConvertedTelemetry";

        internal const int MaxExceptionCountToSave = 10;
    }
}
