namespace Microsoft.ApplicationInsights.WindowsServer.Channel.Helpers
{
    using System;
    using System.Net.NetworkInformation;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;

    internal class StubNetwork : INetwork
    {
        public Func<bool> OnIsAvailable = () => true;

        public Action<NetworkAddressChangedEventHandler> OnAddAddressChangedEventHandler;

        public Action<NetworkAddressChangedEventHandler> OnRemoveAddressChangedEventHandler;

        private NetworkAddressChangedEventHandler addressChanged = delegate { };

        public StubNetwork()
        {
            this.OnAddAddressChangedEventHandler = handler => this.addressChanged += handler;
            this.OnRemoveAddressChangedEventHandler = handler => this.addressChanged -= handler;
        }

        public void AddAddressChangedEventHandler(NetworkAddressChangedEventHandler handler)
        {
            this.OnAddAddressChangedEventHandler(handler);
        }

        public void RemoveAddressChangeEventHandler(NetworkAddressChangedEventHandler handler)
        {
            this.OnRemoveAddressChangedEventHandler(handler);
        }

        public bool IsAvailable()
        {
            return this.OnIsAvailable();
        }

        public void OnStatusChanged(EventArgs e)
        {
            this.addressChanged(this, e);
        }
    }
}
