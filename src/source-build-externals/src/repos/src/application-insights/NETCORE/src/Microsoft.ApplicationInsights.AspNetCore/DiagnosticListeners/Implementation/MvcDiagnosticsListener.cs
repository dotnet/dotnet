namespace Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.ApplicationInsights.AspNetCore.DiagnosticListeners.Implementation;
    using Microsoft.ApplicationInsights.AspNetCore.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.DependencyCollector;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// <see cref="IApplicationInsightDiagnosticListener"/> implementation that listens for events specific to AspNetCore Mvc layer.
    /// </summary>
    [Obsolete("This class was merged with HostingDiagnosticsListener to optimize Diagnostics Source subscription performance")]
    [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Class is obsolete.")]
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "Class is obsolete.")]
    [SuppressMessage("StyleCop Documentation Rules", "SA1611:ElementParametersMustBeDocumented", Justification = "Class is obsolete.")]
    [SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly", Justification = "Class is obsolete.")]
    public class MvcDiagnosticsListener : IApplicationInsightDiagnosticListener
    {
        private readonly PropertyFetcher httpContextFetcher = new PropertyFetcher("httpContext");
        private readonly PropertyFetcher routeDataFetcher = new PropertyFetcher("routeData");
        private readonly PropertyFetcher routeValuesFetcher = new PropertyFetcher("Values");

        /// <inheritdoc />
        public string ListenerName { get; } = "Microsoft.AspNetCore";

        /// <summary>
        /// Diagnostic event handler method for 'Microsoft.AspNetCore.Mvc.BeforeAction' event.
        /// </summary>
        public void OnBeforeAction(HttpContext httpContext, IDictionary<string, object> routeValues)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var telemetry = httpContext.Features.Get<RequestTelemetry>();

            if (telemetry != null && string.IsNullOrEmpty(telemetry.Name))
            {
                if (routeValues == null)
                {
                    throw new ArgumentNullException(nameof(routeValues));
                }

                string name = GetNameFromRouteContext(routeValues);
                if (!string.IsNullOrEmpty(name))
                {
                    name = httpContext.Request.Method + " " + name;
                    telemetry.Name = name;
                }
            }
        }

        /// <inheritdoc />
        public void OnSubscribe()
        {
        }

        /// <inheritdoc />
        public void OnNext(KeyValuePair<string, object> value)
        {
            try
            {
                if (value.Key == "Microsoft.AspNetCore.Mvc.BeforeAction")
                {
                    var context = this.httpContextFetcher.Fetch(value.Value) as HttpContext;
                    var routeData = this.routeDataFetcher.Fetch(value.Value);
                    var routeValues = this.routeValuesFetcher.Fetch(routeData) as IDictionary<string, object>;

                    if (context != null && routeValues != null)
                    {
                        this.OnBeforeAction(context, routeValues);
                    }
                }
            }
            catch (Exception ex)
            {
                AspNetCoreEventSource.Instance.DiagnosticListenerWarning("MvcDiagnosticsListener", value.Key, ex.Message);
            }
        }

        /// <inheritdoc />
        public void OnError(Exception error)
        {
        }

        /// <inheritdoc />
        public void OnCompleted()
        {
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }

        private static string GetNameFromRouteContext(IDictionary<string, object> routeValues)
        {
            string name = null;

            if (routeValues.Count > 0)
            {
                object controller;
                routeValues.TryGetValue("controller", out controller);
                string controllerString = (controller == null) ? string.Empty : controller.ToString();

                if (!string.IsNullOrEmpty(controllerString))
                {
                    name = controllerString;

                    object action;
                    routeValues.TryGetValue("action", out action);
                    string actionString = (action == null) ? string.Empty : action.ToString();

                    if (!string.IsNullOrEmpty(actionString))
                    {
                        name += "/" + actionString;
                    }

                    if (routeValues.Keys.Count > 2)
                    {
                        // Add parameters
                        var sortedKeys = routeValues.Keys
                            .Where(key =>
                                !string.Equals(key, "controller", StringComparison.OrdinalIgnoreCase) &&
                                !string.Equals(key, "action", StringComparison.OrdinalIgnoreCase) &&
                                !string.Equals(key, "!__route_group", StringComparison.OrdinalIgnoreCase))
                            .OrderBy(key => key, StringComparer.OrdinalIgnoreCase)
                            .ToArray();

                        if (sortedKeys.Length > 0)
                        {
                            string arguments = string.Join(@"/", sortedKeys);
                            name += " [" + arguments + "]";
                        }
                    }
                }
                else
                {
                    object page;
                    routeValues.TryGetValue("page", out page);
                    string pageString = (page == null) ? string.Empty : page.ToString();
                    if (!string.IsNullOrEmpty(pageString))
                    {
                        name = pageString;
                    }
                }
            }

            return name;
        }
    }
}