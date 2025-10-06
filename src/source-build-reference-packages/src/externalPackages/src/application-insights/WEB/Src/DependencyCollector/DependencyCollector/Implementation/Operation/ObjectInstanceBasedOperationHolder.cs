namespace Microsoft.ApplicationInsights.DependencyCollector.Implementation.Operation
{
    using System;
    using System.Runtime.CompilerServices;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    internal class ObjectInstanceBasedOperationHolder<TTelemetry> where TTelemetry : OperationTelemetry
    {
        private ConditionalWeakTable<object, Tuple<TTelemetry, bool>> weakTableForCorrelation = new ConditionalWeakTable<object, Tuple<TTelemetry, bool>>();

        public Tuple<TTelemetry, bool> Get(object holderInstance)
        {
            if (holderInstance == null)
            {
                throw new ArgumentNullException(nameof(holderInstance));
            }

            Tuple<TTelemetry, bool> result = null;
            if (!this.weakTableForCorrelation.TryGetValue(holderInstance, out result))
            {
                result = null;
            }

            return result;
        }

        public bool Remove(object holderInstance)
        {
            if (holderInstance == null)
            {
                throw new ArgumentNullException(nameof(holderInstance));
            }

            return this.weakTableForCorrelation.Remove(holderInstance);
        }

        public void Store(object holderInstance, Tuple<TTelemetry, bool> telemetryTuple)
        {
            if (holderInstance == null)
            {
                throw new ArgumentNullException(nameof(holderInstance));
            }

            if (telemetryTuple == null)
            {
                throw new ArgumentNullException(nameof(telemetryTuple));
            }

            this.weakTableForCorrelation.Add(holderInstance, telemetryTuple);
        }
    }
}
