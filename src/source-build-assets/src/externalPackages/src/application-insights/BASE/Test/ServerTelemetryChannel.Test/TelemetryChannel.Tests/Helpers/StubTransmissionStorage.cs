namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.TestFramework;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    using Channel.Helpers;

    using TaskEx = System.Threading.Tasks.Task;

    internal class StubTransmissionStorage : TransmissionStorage
    {
        public Func<Transmission> OnDequeue;
        public Func<Transmission, bool> OnEnqueue;
        public Func<long> OnGetCapacity;
        public Action<long> OnSetCapacity;
        public Queue<Transmission> Queue;
        public Action<IApplicationFolderProvider> OnInitialize;

        private long capacity;

        public StubTransmissionStorage()
            
        {
            this.Queue = new Queue<Transmission>();
            this.OnDequeue = () => this.Queue.Count == 0 ? null : this.Queue.Dequeue();
            this.OnEnqueue = transmission =>
            {
                if (transmission != null)
                {
                    this.Queue.Enqueue(transmission);
                    return true;
                }

                return false;
            };

            this.OnGetCapacity = () => this.capacity;
            this.OnSetCapacity = value => this.capacity = value;
            this.OnInitialize = _ => base.Initialize(_ ?? new StubApplicationFolderProvider());
        }

        public override void Initialize(IApplicationFolderProvider transmissionFolderProvider = null)
        {
            this.OnInitialize(transmissionFolderProvider);
        }

        public override long Capacity
        {
            get { return this.OnGetCapacity(); }
            set { this.OnSetCapacity(value); }
        }

        public override Transmission Dequeue()
        {
            return this.OnDequeue();
        }

        public override bool Enqueue(Func<Transmission> getTransmissionAsync)
        {
            return this.OnEnqueue(getTransmissionAsync());
        }
    }
}
