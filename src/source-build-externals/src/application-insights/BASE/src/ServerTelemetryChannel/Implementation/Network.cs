namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System.Net.NetworkInformation;

    /// <summary>
    /// Encapsulates platform-specific behavior of network information APIs.
    /// </summary>
    internal sealed class Network : INetwork
    {
        public void AddAddressChangedEventHandler(NetworkAddressChangedEventHandler handler)
        {
            NetworkChange.NetworkAddressChanged += handler;
        }

        public void RemoveAddressChangeEventHandler(NetworkAddressChangedEventHandler handler)
        {
            NetworkChange.NetworkAddressChanged -= handler;
        }

        public bool IsAvailable()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }
    }
}
