namespace Microsoft.ApplicationInsights.Common
{
    using System.Collections;
    using System.Diagnostics;
    using Microsoft.ApplicationInsights.Web.Implementation;

    internal class ActivityHelpers
    {
        /// <summary>
        /// Name of the item under which Activity created by request tracking (if any) will be stored
        /// It's exactly the same as one Microsoft.AspNet.TelemetryCorrelation uses
        /// https://github.com/aspnet/Microsoft.AspNet.TelemetryCorrelation/blob/6ccf0729050be4fac6797fa85af0200883db1c83/src/Microsoft.AspNet.TelemetryCorrelation/ActivityHelper.cs#L33
        /// so that TelemetryCorrelation will restore and treat 'our' Activity as it's own.
        /// </summary>
        internal const string RequestActivityItemName = "Microsoft.ApplicationInsights.Activity";

        internal static string RootOperationIdHeaderName { get; set; }

        internal static string ParentOperationIdHeaderName { get; set; }

        /// <summary> 
        /// Checks if given RequestId is hierarchical.
        /// </summary>
        /// <param name="requestId">Request id.</param>
        /// <returns>True if requestId is hierarchical false otherwise.</returns>
        internal static bool IsHierarchicalRequestId(string requestId)
        {
            return !string.IsNullOrEmpty(requestId) && requestId[0] == '|';
        }

        /// <summary>
        /// It's possible that a request is executed in both native threads and managed threads,
        /// in such case Activity.Current will be lost during native thread and managed thread switch.
        /// This method is intended to restore the current activity in order to correlate the child
        /// activities with the root activity of the request.
        /// </summary>
        /// <param name="contextItems">Dictionary of HttpContext.Items.</param>
        internal static void RestoreActivityIfNeeded(IDictionary contextItems)
        {
            if (contextItems == null)
            {
                WebEventSource.Log.NoHttpContextWarning();
                return;
            }

            if (Activity.Current == null && contextItems.Contains(RequestActivityItemName))
            {
                Activity.Current = (Activity)contextItems[RequestActivityItemName];
            }
        }
    }
}