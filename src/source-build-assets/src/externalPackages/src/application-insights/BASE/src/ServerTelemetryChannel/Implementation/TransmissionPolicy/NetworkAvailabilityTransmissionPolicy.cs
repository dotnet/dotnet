namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy
{
    using System;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    
    internal class NetworkAvailabilityTransmissionPolicy : TransmissionPolicy, IDisposable
    {
        private INetwork network;

        public NetworkAvailabilityTransmissionPolicy(INetwork network)
        {
            this.network = network;
        }

        public override void Initialize(Transmitter transmitter)
        {
            base.Initialize(transmitter);
            this.SubscribeToNetworkAddressChangedEvents();
            this.SetBufferAndSenderCapacity();
        }

        /// <summary>
        /// Releases resources used by this <see cref="NetworkAvailabilityTransmissionPolicy"/> instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void SubscribeToNetworkAddressChangedEvents()
        {
            try
            {
                this.network.AddAddressChangedEventHandler(this.HandleNetworkStatusChangedEvent);
            }
            catch (SocketException se) 
            {
                TelemetryChannelEventSource.Log.SubscribeToNetworkFailureWarning(se.ToString());
            }
            catch (NetworkInformationException nie)
            {
                TelemetryChannelEventSource.Log.SubscribeToNetworkFailureWarning(nie.ToString());
            }
        }

        private void UnsubscribeFromNetworkAddressChangedEvents()
        {
            try
            {
                this.network.RemoveAddressChangeEventHandler(this.HandleNetworkStatusChangedEvent);
            }
            catch (Exception)
            {
                // Eat up any exceptions, since this is only called in the dispose path.
            }
        }

        private void HandleNetworkStatusChangedEvent(object sender, EventArgs e)
        {
            this.SetBufferAndSenderCapacity();
            this.Apply();
        }

        private void SetBufferAndSenderCapacity()
        {
            if (!this.IsNetworkAvailable())
            {
                this.MaxSenderCapacity = 0;
                this.MaxBufferCapacity = 0;
            }
            else
            {
                this.MaxSenderCapacity = null;
                this.MaxBufferCapacity = null;
            }

            this.LogCapacityChanged();
        }

        private bool IsNetworkAvailable()
        {
            bool result = true;
            try
            {
                result = this.network.IsAvailable();
                if (!result)
                {
                    TelemetryChannelEventSource.Log.NetworkIsNotAvailableWarning();
                }
            }
            catch (SocketException se)
            {
                TelemetryChannelEventSource.Log.SubscribeToNetworkFailureWarning(se.ToString());
            }
            catch (NetworkInformationException nie)
            {
                TelemetryChannelEventSource.Log.SubscribeToNetworkFailureWarning(nie.ToString());
            }
            catch (Exception ex)
            {
                TelemetryChannelEventSource.Log.SubscribeToNetworkFailureWarning(ex.ToString());
            }

            return result;
        }

        private void Dispose(bool disposing)
        {
            if (disposing && this.network != null)
            {
                this.UnsubscribeFromNetworkAddressChangedEvents();
                this.network = null;
            }
        }
    }
}
