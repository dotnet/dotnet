namespace Microsoft.ApplicationInsights.TestFramework
{
    using Microsoft.ApplicationInsights.Channel;

    /// <summary>
    /// Required by Windows Runtime, which does not allow generics in public APIs.
    /// </summary>
    public delegate void TelemetryAction(ITelemetry telemetry);
}
