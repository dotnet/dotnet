namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;

    internal class StubTelemetrySerializer : TelemetrySerializer
    {
        public Action<IEnumerable<ITelemetry>> OnSerialize = _ => { };
        public Func<IEnumerable<ITelemetry>, CancellationToken, Task<bool>> OnSerializeAsync;

        public override void Serialize(ICollection<ITelemetry> items)
        {
            this.OnSerialize(items);
        }

        public override Task<bool> SerializeAsync(ICollection<ITelemetry> items, CancellationToken cancellationToken)
        {
            return this.OnSerializeAsync(items, cancellationToken);
        }
    }
}
