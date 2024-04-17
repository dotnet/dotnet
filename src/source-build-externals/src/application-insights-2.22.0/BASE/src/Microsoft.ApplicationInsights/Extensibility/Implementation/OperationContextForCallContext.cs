namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Operation class that holds operation id and operation name for the current call context.
    /// </summary>
    internal class OperationContextForCallContext
    {
        /// <summary>
        /// Operation id that will be assigned to all the child telemetry items.
        /// Parent Operation id that will be assigned to all the child telemetry items.
        /// </summary>
        public string ParentOperationId;

        /// <summary>
        /// Root Operation id that will be assigned to all the child telemetry items.
        /// </summary>
        public string RootOperationId;

        /// <summary>
        /// Operation name that will be assigned to all the child telemetry items.
        /// </summary>
        public string RootOperationName;

        /// <summary>
        /// Context that is propagated with HTTP outbound calls, check for null.
        /// <see href="https://github.com/lmolkova/correlation/blob/master/http_protocol_proposal_v1.md"/>. 
        /// </summary>
        public IDictionary<string, string> CorrelationContext;
    }
}
