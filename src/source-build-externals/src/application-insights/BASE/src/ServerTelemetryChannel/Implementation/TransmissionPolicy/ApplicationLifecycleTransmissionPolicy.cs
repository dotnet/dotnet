namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy
{
    internal class ApplicationLifecycleTransmissionPolicy : TransmissionPolicy
    {
        private readonly IApplicationLifecycle applicationLifecycle;

        public ApplicationLifecycleTransmissionPolicy(IApplicationLifecycle applicationLifecycle)
        {
            this.applicationLifecycle = applicationLifecycle;
        }

        public override void Initialize(Transmitter transmitter)
        {
            base.Initialize(transmitter);
            this.applicationLifecycle.Stopping += this.HandleApplicationStoppingEvent;
        }

        private void HandleApplicationStoppingEvent(object sender, ApplicationStoppingEventArgs e)
        {
            this.SetMaxTransmissionCapacity(0);
        }

        private void SetMaxTransmissionCapacity(int value)
        {
            this.MaxSenderCapacity = value;
            this.MaxBufferCapacity = value;
            this.LogCapacityChanged();

            this.Apply();
        }
    }
}
