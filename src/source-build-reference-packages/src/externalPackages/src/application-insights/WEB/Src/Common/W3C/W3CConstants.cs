#if DEPENDENCY_COLLECTOR
    namespace Microsoft.ApplicationInsights.W3C
#else
    namespace Microsoft.ApplicationInsights.W3C.Internal
#endif
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// W3C constants.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
#if DEPENDENCY_COLLECTOR
    public
#else
    internal
#endif
    static class W3CConstants
    {
        /// <summary>
        /// W3C traceparent header name.
        /// </summary>
        public const string TraceParentHeader = "traceparent";

        /// <summary>
        /// W3C tracestate header name.
        /// </summary>
        public const string TraceStateHeader = "tracestate";

        /// <summary>
        /// Name of the field that carry ApplicationInsights application Id in the tracestate header under az key.
        /// </summary>
        [Obsolete("Dot not use.")]
        public const string ApplicationIdTraceStateField = "cid-v1";

        /// <summary>
        /// Name of the field that carry Azure-specific states in the tracestate header.
        /// </summary>
        [Obsolete("Dot not use.")]
        public const string AzureTracestateNamespace = "az";

        /// <summary>
        /// Separator between Azure namespace values.
        /// </summary>
        [Obsolete("Dot not use.")]
        public const char TracestateAzureSeparator = ';';

        internal const string LegacyRootPropertyIdKey = "ai_legacyRootId";
    }
}
