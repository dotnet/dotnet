namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy
{
    using System;
    
    internal abstract class TransmissionPolicy
    {
        private readonly string policyName;
        
        protected TransmissionPolicy()
        {
            this.policyName = this.GetType().ToString();
        }

        public int? MaxSenderCapacity { get; protected set; }

        public int? MaxBufferCapacity { get; protected set; }

        public int? MaxStorageCapacity { get; protected set; }

        protected Transmitter Transmitter { get; private set; }

        public void Apply()
        {
            if (this.Transmitter == null)
            {
                throw new InvalidOperationException("Transmission policy has not been initialized.");
            }

            try
            {
                this.Transmitter.ApplyPolicies();
            }
            catch (Exception exp)
            {
                TelemetryChannelEventSource.Log.ApplyPoliciesError(exp.ToString());
            }
        }

        public virtual void Initialize(Transmitter transmitter)
        {
            this.Transmitter = transmitter ?? throw new ArgumentNullException(nameof(transmitter));
        }

        protected void LogCapacityChanged()
        {
            if (this.MaxSenderCapacity.HasValue)
            {
                TelemetryChannelEventSource.Log.SenderCapacityChanged(this.policyName, this.MaxSenderCapacity.Value);
            }
            else
            {
                TelemetryChannelEventSource.Log.SenderCapacityReset(this.policyName);
            }

            if (this.MaxBufferCapacity.HasValue)
            {
                TelemetryChannelEventSource.Log.BufferCapacityChanged(this.policyName, this.MaxBufferCapacity.Value);
            }
            else
            {
                TelemetryChannelEventSource.Log.BufferCapacityReset(this.policyName);
            }

            if (this.MaxStorageCapacity.HasValue)
            {
                TelemetryChannelEventSource.Log.StorageCapacityChanged(this.policyName, this.MaxStorageCapacity.Value);
            }
            else
            {
                TelemetryChannelEventSource.Log.StorageCapacityReset(this.policyName);
            }
        }
    }
}
