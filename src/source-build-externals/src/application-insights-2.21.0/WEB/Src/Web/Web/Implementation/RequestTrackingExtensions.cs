namespace Microsoft.ApplicationInsights.Web.Implementation
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.W3C.Internal;
    using Microsoft.AspNet.TelemetryCorrelation;

    internal static class RequestTrackingExtensions
    {
        internal static RequestTelemetry CreateRequestTelemetryPrivate(
            this HttpContext platformContext)
        {
            if (platformContext == null)
            {
                throw new ArgumentNullException(nameof(platformContext));
            }

            var currentActivity = Activity.Current;

            var result = new RequestTelemetry();
            var requestContext = result.Context.Operation;
            string legacyParentId = null;
            string legacyRootId = null;

            var headers = platformContext.Request.Unvalidated.Headers;
            if (currentActivity == null)
            {
                // if there was no BeginRequest, ASP.NET HttpModule did not have a chance to set current activity yet
                // this could happen if ASP.NET TelemetryCorrelation module is not the first in the pipeline
                // and some module before it tracks telemetry.
                // The ASP.NET module will be invoked later with proper correlation ids.
                // But we only get one chance to create request telemetry and we have to create it when method is called to avoid breaking changes 
                // The correlation will be BROKEN anyway as telemetry reported before ASP.NET TelemetryCorrelation HttpModule is called
                // will not be correlated  properly to telemetry reported within the request 
                // Here we simply maintaining backward compatibility with this behavior...

#pragma warning disable CA2000 // Dispose objects before losing scope
                // Since we don't know when it will finish, we will not dispose
                currentActivity = new Activity(ActivityHelpers.RequestActivityItemName);
#pragma warning restore CA2000 // Dispose objects before losing scope
                if (!currentActivity.Extract(headers))
                {
                    if (ActivityHelpers.ParentOperationIdHeaderName != null &&
                        ActivityHelpers.RootOperationIdHeaderName != null)
                    {
                        legacyRootId = StringUtilities.EnforceMaxLength(platformContext.Request.UnvalidatedGetHeader(ActivityHelpers.RootOperationIdHeaderName),
                            InjectionGuardConstants.RequestHeaderMaxLength);
                        legacyParentId = StringUtilities.EnforceMaxLength(
                            platformContext.Request.UnvalidatedGetHeader(ActivityHelpers.ParentOperationIdHeaderName),
                            InjectionGuardConstants.RequestHeaderMaxLength);
                        currentActivity.SetParentId(legacyRootId);
                    }

                    headers.ReadActivityBaggage(currentActivity);
                }

                currentActivity.Start();
            }

            if (currentActivity.IdFormat == ActivityIdFormat.W3C && 
                currentActivity.ParentId != null 
                && !currentActivity.ParentId.StartsWith("00-", StringComparison.Ordinal))
            {
                if (W3CUtilities.TryGetTraceId(currentActivity.ParentId, out var traceId))
                {
                    legacyParentId = currentActivity.ParentId;
#pragma warning disable CA2000 // Dispose objects before losing scope
                    // Since we don't know when it will finish, we will not dispose
                    currentActivity = CreateSubstituteActivityFromCompatibleRootId(currentActivity, traceId);
#pragma warning restore CA2000 // Dispose objects before losing scope
                }
                else
                {
                    legacyRootId = W3CUtilities.GetRootId(currentActivity.ParentId);
                    legacyParentId = legacyParentId ?? GetLegacyParentId(currentActivity.ParentId, platformContext.Request);
                }
            }
            else if (currentActivity.IdFormat == ActivityIdFormat.Hierarchical &&
                     currentActivity.ParentId != null)
            {
                legacyParentId = GetLegacyParentId(currentActivity.ParentId, platformContext.Request);
            }

            if (currentActivity.IdFormat == ActivityIdFormat.W3C)
            {
                // we have Activity.Current, we need to properly initialize request telemetry and store it in HttpContext
                requestContext.Id = currentActivity.TraceId.ToHexString();

                if (currentActivity.ParentSpanId != default && legacyParentId == null)
                {
                    requestContext.ParentId = currentActivity.ParentSpanId.ToHexString();
                }
                else
                {
                    requestContext.ParentId = legacyParentId;
                    if (legacyRootId != null)
                    {
                        result.Properties[W3CConstants.LegacyRootPropertyIdKey] = legacyRootId;
                    }
                }

                result.Id = currentActivity.SpanId.ToHexString();
            }
            else
            {
                // we have Activity.Current, we need to properly initialize request telemetry and store it in HttpContext
                requestContext.Id = currentActivity.RootId;
                requestContext.ParentId = legacyParentId ?? currentActivity.ParentId;

                result.Id = currentActivity.Id;
            }

            foreach (var item in currentActivity.Baggage)
            {
                if (!result.Properties.ContainsKey(item.Key))
                {
                    result.Properties.Add(item);
                }
            }

            // save current activity in case it will be lost (under the same name TelemetryCorrelation stores it)
            // TelemetryCorrelation will restore it when possible.
            platformContext.Items[ActivityHelpers.RequestActivityItemName] = currentActivity;
            platformContext.Items[RequestTrackingConstants.RequestTelemetryItemName] = result;
            WebEventSource.Log.WebTelemetryModuleRequestTelemetryCreated();

            return result;
        }

        internal static RequestTelemetry ReadOrCreateRequestTelemetryPrivate(
            this HttpContext platformContext)
        {
            if (platformContext == null)
            {
                throw new ArgumentNullException(nameof(platformContext));
            }

            return platformContext.GetRequestTelemetry() ?? 
                   CreateRequestTelemetryPrivate(platformContext);
        }

        /// <summary>
        /// Creates request name on the base of HttpContext.
        /// </summary>
        /// <returns>Controller/Action for MVC or path for other cases.</returns>
        internal static string CreateRequestNamePrivate(this HttpContext platformContext)
        {
            var request = platformContext.Request;
            string name = request.UnvalidatedGetPath();

            var routeValues = request.RequestContext?.RouteData?.Values;

            if (routeValues != null && routeValues.Count > 0)
            {
                routeValues.TryGetValue("controller", out var controller);
                string controllerString = (controller == null) ? string.Empty : controller.ToString();

                if (!string.IsNullOrEmpty(controllerString))
                {
                    routeValues.TryGetValue("action", out var action);
                    string actionString = (action == null) ? string.Empty : action.ToString();

                    name = controllerString;
                    if (!string.IsNullOrEmpty(actionString))
                    {
                        name += "/" + actionString;
                    }
                    else
                    {
                        if (routeValues.Keys.Count > 1)
                        {
                            // We want to include arguments because in WebApi action is usually null 
                            // and action is resolved by controller, http method and number of arguments
                            var sortedKeys = routeValues.Keys
                                .Where(key => !string.Equals(key, "controller", StringComparison.OrdinalIgnoreCase))
                                .OrderBy(key => key, StringComparer.OrdinalIgnoreCase)
                                .ToArray();

                            string arguments = string.Join(@"/", sortedKeys);
                            name += " [" + arguments + "]";
                        }
                    }
                }
            }

            if (name.StartsWith("/__browserLink/requestData/", StringComparison.OrdinalIgnoreCase))
            {
                name = "/__browserLink";
            }

            name = request.HttpMethod + " " + name;

            return name;
        }

        private static Activity CreateSubstituteActivityFromCompatibleRootId(Activity currentActivity, ReadOnlySpan<char> traceId)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            // Since we don't know when it will finish, we will not dispose
            var activity = new Activity(currentActivity.OperationName);
#pragma warning restore CA2000 // Dispose objects before losing scope
            activity.SetParentId(ActivityTraceId.CreateFromString(traceId), default, ActivityTraceFlags.None);

            foreach (var baggage in currentActivity.Baggage)
            {
                activity.AddBaggage(baggage.Key, baggage.Value);
            }

            return activity.Start();
        }

        private static string GetLegacyParentId(string activityParent, HttpRequest request)
        {
            if (ActivityHelpers.IsHierarchicalRequestId(activityParent))
            {
                return activityParent;
            }

            return ActivityHelpers.ParentOperationIdHeaderName != null ?
                    StringUtilities.EnforceMaxLength(
                        request.UnvalidatedGetHeader(ActivityHelpers.ParentOperationIdHeaderName),
                        InjectionGuardConstants.RequestHeaderMaxLength) :
                    activityParent;
        }
    }
}