namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TransmissionPolicy
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    internal class TransmissionPolicyCollection : IDisposable
    {
        private readonly IEnumerable<TransmissionPolicy> policies;
        private bool isDisposed;

        [SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "All policies are stored in the IEnumerable and are properly disposed.")]
        private AuthenticationTransmissionPolicy authenticationTransmissionPolicy;

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "applicationLifecycle is required only for NetFramework.")]
        public TransmissionPolicyCollection(INetwork network, IApplicationLifecycle applicationLifecycle)
        {
            this.policies = new TransmissionPolicy[]
            { 
#if NETFRAMEWORK
                // We don't have implementation for IApplicationLifecycle for .NET Core
                new ApplicationLifecycleTransmissionPolicy(applicationLifecycle),
#endif
                new ThrottlingTransmissionPolicy(),
                new ErrorHandlingTransmissionPolicy(),
                new PartialSuccessTransmissionPolicy(),
                new NetworkAvailabilityTransmissionPolicy(network),
                this.authenticationTransmissionPolicy = new AuthenticationTransmissionPolicy(),
            };
        }

        /// <summary>
        /// Constructor intended for unit tests only. 
        /// This is also used by the <see cref="TransmissionPolicyCollection.Default"/> to create an empty collection.
        /// </summary>
        /// <param name="policies">A collection of <see cref="TransmissionPolicy"/> specific to a test scenario.</param>
        internal TransmissionPolicyCollection(IEnumerable<TransmissionPolicy> policies)
        {
            this.policies = policies ?? Enumerable.Empty<TransmissionPolicy>();
        }

        public static TransmissionPolicyCollection Default => new TransmissionPolicyCollection(Enumerable.Empty<TransmissionPolicy>());

        public void Initialize(Transmitter transmitter)
        {
            foreach (var policy in this.policies)
            {
                policy.Initialize(transmitter);
            }
        }

        public void EnableAuthenticationPolicy() => this.authenticationTransmissionPolicy.Enabled = true;

        public int? CalculateMinimumMaxSenderCapacity() => this.CalculateMinimumCapacity(p => p.MaxSenderCapacity);

        public int? CalculateMinimumMaxBufferCapacity() => this.CalculateMinimumCapacity(p => p.MaxBufferCapacity);

        public int? CalculateMinimumMaxStorageCapacity() => this.CalculateMinimumCapacity(p => p.MaxStorageCapacity);

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private int? CalculateMinimumCapacity(Func<TransmissionPolicy, int?> getMaxPolicyCapacity)
        {
            int? maxComponentCapacity = null;
            foreach (TransmissionPolicy policy in this.policies)
            {
                int? maxPolicyCapacity = getMaxPolicyCapacity(policy);
                if (maxPolicyCapacity != null)
                {
                    maxComponentCapacity = maxComponentCapacity == null
                        ? maxPolicyCapacity
                        : Math.Min(maxComponentCapacity.Value, maxPolicyCapacity.Value);
                }
            }

            return maxComponentCapacity;
        }

        private void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    foreach (var policy in this.policies.OfType<IDisposable>())
                    {
                        policy.Dispose();
                    }
                }

                this.isDisposed = true;
            }
        }
    }
}
