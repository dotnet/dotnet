namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers
{
    using System;

    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy;

    internal class StubTransmissionPolicy : TransmissionPolicy
    {
        public Action<Transmitter> OnInitialize;

        public StubTransmissionPolicy()
        {
            this.OnInitialize = base.Initialize;
        }

        public new int? MaxSenderCapacity
        {
            get { return base.MaxSenderCapacity; }
            set { base.MaxSenderCapacity = value; }
        }

        public new int? MaxBufferCapacity
        {
            get { return base.MaxBufferCapacity; }
            set { base.MaxBufferCapacity = value; }
        }

        public new int? MaxStorageCapacity
        {
            get { return base.MaxStorageCapacity; }
            set { base.MaxStorageCapacity = value; }
        }
       
        public override void Initialize(Transmitter transmitter)
        {
            this.OnInitialize(transmitter);
        }
    }
}
