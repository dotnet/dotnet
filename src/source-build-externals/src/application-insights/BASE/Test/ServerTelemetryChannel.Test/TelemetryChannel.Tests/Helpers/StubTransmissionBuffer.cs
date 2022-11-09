namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers
{
    using System;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    using TaskEx = System.Threading.Tasks.Task;

    internal class StubTransmissionBuffer : TransmissionBuffer
    {
        public Func<Transmission> OnDequeue = () => null;
        public Func<Func<Transmission>, bool> OnEnqueue = getTransmissionAsync => false;
        public Func<int> OnGetCapacity;
        public Action<int> OnSetCapacity;
        public Func<long> OnGetSize = () => 0;

        private int maxNumberOfTransmissions;

        public StubTransmissionBuffer()
        {
            this.OnGetCapacity = () => this.maxNumberOfTransmissions;
            this.OnSetCapacity = value => this.maxNumberOfTransmissions = value;
        }

        public override int Capacity
        {
            get { return this.OnGetCapacity(); }
            set { this.OnSetCapacity(value); }
        }

        public override long Size
        {
            get { return this.OnGetSize(); }
        }

        public override Transmission Dequeue()
        {
            return this.OnDequeue();
        }

        public override bool Enqueue(Func<Transmission> getTransmissionAsync)
        {
            return this.OnEnqueue(getTransmissionAsync);
        }

        public new void OnTransmissionDequeued(TransmissionProcessedEventArgs e)
        {
            base.OnTransmissionDequeued(e);
        }
    }
}
