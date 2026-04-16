namespace Microsoft.ManagementServices.RealTimeDataProcessing.QuickPulseService
{
    internal enum TelemetryDocumentType
    {
        Unknown = 0,

        Request,

        RemoteDependency,

        Exception,

        Event,

        Trace,
    }
}
