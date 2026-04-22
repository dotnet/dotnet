namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;

    internal class StubTransmissionSender : TransmissionSender
    {
        public Func<Func<Transmission>, bool> OnEnqueue = getTransmissionAsync => false;
        public Func<int> OnGetCapacity;
        public Action<int> OnSetCapacity;

        private int capacity;

        public StubTransmissionSender()
        {
            this.OnGetCapacity = () => this.capacity;
            this.OnSetCapacity = value => this.capacity = value;
        }

        public override int Capacity
        {
            get { return this.OnGetCapacity(); }
            set { this.OnSetCapacity(value); }
        }

        public override bool Enqueue(Func<Transmission> getTransmissionAsync)
        {
            return this.OnEnqueue(getTransmissionAsync);
        }

        public new void OnTransmissionSent(TransmissionProcessedEventArgs args)
        {
            base.OnTransmissionSent(args);
        }
    }
}
