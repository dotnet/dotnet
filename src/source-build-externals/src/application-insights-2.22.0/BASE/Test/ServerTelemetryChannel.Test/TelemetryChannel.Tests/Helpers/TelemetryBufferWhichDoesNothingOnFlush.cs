namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Helpers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation;
    using System.Threading.Tasks;

    internal class TelemetryBufferWhichDoesNothingOnFlush : Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.TelemetryBuffer
    {
        public Action<IEnumerable<ITelemetry>> OnSerialize = _ => { };

        public TelemetryBufferWhichDoesNothingOnFlush(TelemetrySerializer serializer, IApplicationLifecycle applicationLifecycle) : base(serializer, applicationLifecycle)
        {            

        }
        public override Task FlushAsync()
        {
            // Intentionally blank to simulate situation where buffer is not emptied.
            return null;
        }
    }
}
