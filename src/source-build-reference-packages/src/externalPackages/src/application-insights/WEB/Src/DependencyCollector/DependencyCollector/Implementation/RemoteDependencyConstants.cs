namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation
{
    internal static class RemoteDependencyConstants
    {
        public const string SQL = "SQL";
        public const string HTTP = "Http";
        public const string AI = "Http (tracked component)";

        public const string AzureBlob = "Azure blob";
        public const string AzureTable = "Azure table";
        public const string AzureQueue = "Azure queue";
        public const string AzureDocumentDb = "Azure DocumentDB";
        public const string AzureEventHubs = "Azure Event Hubs";
        public const string AzureServiceBus = "Azure Service Bus";
        public const string AzureIotHub = "Azure IoT Hub";
        public const string AzureSearch = "Azure Search";
        public const string InProc = "InProc";
        public const string QueueMessage = "Queue Message";

        public const string WcfService = "WCF Service";
        public const string WebService = "Web Service";

        public const string DependencyErrorPropertyKey = "Error";
    }
}
