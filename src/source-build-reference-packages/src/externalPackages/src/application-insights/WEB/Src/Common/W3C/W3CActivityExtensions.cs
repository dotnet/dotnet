#if DEPENDENCY_COLLECTOR
namespace Microsoft.ApplicationInsights.W3C
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;

    /// <summary>
    /// Extends Activity to support W3C distributed tracing standard.
    /// </summary>
    [Obsolete("Use Microsoft.ApplicationInsights.Extensibility.W3C.W3CActivityExtensions in Microsoft.ApplicationInsights package instead")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class W3CActivityExtensions
    {
        /// <summary>
        /// Generate new W3C context.
        /// </summary>
        /// <param name="activity">Activity to generate W3C context on.</param>
        /// <returns>The same Activity for chaining.</returns>
        [Obsolete("Use Microsoft.ApplicationInsights.Extensibility.W3C.W3CActivityExtensions.GenerateW3CContext in Microsoft.ApplicationInsights package instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Activity GenerateW3CContext(this Activity activity) => Extensibility.W3C.W3CActivityExtensions.GenerateW3CContext(activity);

        /// <summary>
        /// Checks if current Activity has W3C properties on it.
        /// </summary>
        /// <param name="activity">Activity to check.</param>
        /// <returns>True if Activity has W3C properties, false otherwise.</returns>
        [Obsolete("Use Microsoft.ApplicationInsights.Extensibility.W3C.W3CActivityExtensions.IsW3CActivity in Microsoft.ApplicationInsights package instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool IsW3CActivity(this Activity activity) => Extensibility.W3C.W3CActivityExtensions.IsW3CActivity(activity);

        /// <summary>
        /// Updates context on the Activity based on the W3C Context in the parent Activity tree.
        /// </summary>
        /// <param name="activity">Activity to update W3C context on.</param>
        /// <returns>The same Activity for chaining.</returns>
        [Obsolete(
            "Use Microsoft.ApplicationInsights.Extensibility.W3C.W3CActivityExtensions.UpdateContextOnActivity in Microsoft.ApplicationInsights package instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Activity UpdateContextOnActivity(this Activity activity) =>
            Extensibility.W3C.W3CActivityExtensions.UpdateContextOnActivity(activity);

        /// <summary>
        /// Gets traceparent header value for the Activity or null if there is no W3C context on it.
        /// </summary>
        /// <param name="activity">Activity to read W3C context from.</param>
        /// <returns>traceparent header value.</returns>
        [Obsolete("Use Microsoft.ApplicationInsights.Extensibility.W3C.W3CActivityExtensions.GetTraceparent in Microsoft.ApplicationInsights package instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string GetTraceparent(this Activity activity) => Extensibility.W3C.W3CActivityExtensions.GetTraceparent(activity);

        /// <summary>
        /// Initializes W3C context on the Activity from traceparent header value.
        /// </summary>
        /// <param name="activity">Activity to set W3C context on.</param>
        /// <param name="value">Valid traceparent header like 00-0af7651916cd43dd8448eb211c80319c-b9c7c989f97918e1-01.</param>
        [Obsolete("Use Microsoft.ApplicationInsights.Extensibility.W3C.W3CActivityExtensions.SetTraceparent in Microsoft.ApplicationInsights package instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetTraceparent(this Activity activity, string value) => Extensibility.W3C.W3CActivityExtensions.SetTraceparent(activity, value);

        /// <summary>
        /// Gets tracestate header value from the Activity.
        /// </summary>
        /// <param name="activity">Activity to get tracestate from.</param>
        /// <returns>tracestate header value.</returns>
        [Obsolete("Use Microsoft.ApplicationInsights.Extensibility.W3C.W3CActivityExtensions.GetTracestate in Microsoft.ApplicationInsights package instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string GetTracestate(this Activity activity) => Extensibility.W3C.W3CActivityExtensions.GetTracestate(activity);

        /// <summary>
        /// Sets tracestate header value on the Activity.
        /// </summary>
        /// <param name="activity">Activity to set tracestate on.</param>
        /// <param name="value">tracestate header value.</param>
        [Obsolete("Use Microsoft.ApplicationInsights.Extensibility.W3C.W3CActivityExtensions.SetTracestate in Microsoft.ApplicationInsights package instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetTracestate(this Activity activity, string value) => Extensibility.W3C.W3CActivityExtensions.SetTracestate(activity, value);

        /// <summary>
        /// Gets TraceId from the Activity.
        /// Use carefully: if may cause iteration over all tags!.
        /// </summary>
        /// <param name="activity">Activity to get traceId from.</param>
        /// <returns>TraceId value or null if it does not exist.</returns>
        [Obsolete("Use Microsoft.ApplicationInsights.Extensibility.W3C.W3CActivityExtensions.GetTraceId in Microsoft.ApplicationInsights package instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string GetTraceId(this Activity activity) => Extensibility.W3C.W3CActivityExtensions.GetTraceId(activity);

        /// <summary>
        /// Gets SpanId from the Activity.
        /// Use carefully: if may cause iteration over all tags!.
        /// </summary>
        /// <param name="activity">Activity to get spanId from.</param>
        /// <returns>SpanId value or null if it does not exist.</returns>
        [Obsolete("Use Microsoft.ApplicationInsights.Extensibility.W3C.W3CActivityExtensions.GetSpanId in Microsoft.ApplicationInsights package instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string GetSpanId(this Activity activity) => Extensibility.W3C.W3CActivityExtensions.GetSpanId(activity);

        /// <summary>
        /// Gets ParentSpanId from the Activity.
        /// Use carefully: if may cause iteration over all tags!.
        /// </summary>
        /// <param name="activity">Activity to get ParentSpanId from.</param>
        /// <returns>ParentSpanId value or null if it does not exist.</returns>
        [Obsolete("Use Microsoft.ApplicationInsights.Extensibility.W3C.W3CActivityExtensions.GetParentSpanId in Microsoft.ApplicationInsights package instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string GetParentSpanId(this Activity activity) => Extensibility.W3C.W3CActivityExtensions.GetParentSpanId(activity);
    }
}
#endif