namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Web;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Web.Implementation;

    /// <summary>
    /// Listens to ASP.NET DiagnosticSource and enables instrumentation with Activity: let ASP.NET create root Activity for the request.
    /// </summary>
    public class AspNetDiagnosticTelemetryModule : IObserver<DiagnosticListener>, IDisposable, ITelemetryModule
    {
        private const string AspNetListenerName = "Microsoft.AspNet.TelemetryCorrelation";
        private const string IncomingRequestEventName = "Microsoft.AspNet.HttpReqIn";
        private const string IncomingRequestStartEventName = "Microsoft.AspNet.HttpReqIn.Start";
        private const string IncomingRequestStopEventName = "Microsoft.AspNet.HttpReqIn.Stop";

        private IDisposable allListenerSubscription;
        private RequestTrackingTelemetryModule requestModule;
        private ExceptionTrackingTelemetryModule exceptionModule;

        private IDisposable aspNetSubscription;

        /// <summary>
        /// Indicates if module initialized successfully.
        /// </summary>
        private bool isEnabled = true;

        /// <summary>
        /// Initializes the telemetry module.
        /// </summary>
        /// <param name="configuration">Telemetry configuration to use for initialization.</param>
        public void Initialize(TelemetryConfiguration configuration)
        {
            try
            {
                foreach (var module in TelemetryModules.Instance.Modules)
                {
                    if (module is RequestTrackingTelemetryModule requestTrackingModule)
                    {
                        this.requestModule = requestTrackingModule;
                    }
                    else if (module is ExceptionTrackingTelemetryModule exceptionTracingModule)
                    {
                        this.exceptionModule = exceptionTracingModule;
                    }
                }
            }
            catch (Exception exc)
            {
                this.isEnabled = false;
                WebEventSource.Log.WebModuleInitializationExceptionEvent(exc.ToInvariantString());
            }

            this.allListenerSubscription = DiagnosticListener.AllListeners.Subscribe(this);
        }

        /// <summary>
        /// Implements IObserver OnNext callback, subscribes to AspNet DiagnosticSource.
        /// </summary>
        /// <param name="value">DiagnosticListener value.</param>
        public void OnNext(DiagnosticListener value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (this.isEnabled && value.Name == AspNetListenerName)
            {
                var eventListener = new AspNetEventObserver(this.requestModule, this.exceptionModule);
                this.aspNetSubscription = value.Subscribe(eventListener, AspNetEventObserver.IsEnabled, AspNetEventObserver.OnActivityImport);
            }
        }

        /// <summary>
        /// Disposes all subscriptions to DiagnosticSources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        #region IObserver

        /// <summary>
        /// IObserver OnError callback.
        /// </summary>
        /// <param name="error">Exception instance.</param>
        public void OnError(Exception error)
        {
        }

        /// <summary>
        /// IObserver OnCompleted callback.
        /// </summary>
        public void OnCompleted()
        {
        }

        #endregion

        /// <summary>
        /// Implements IDisposable pattern. Dispose() should call Dispose(true), and the finalizer should call Dispose(false).
        /// </summary>
        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                this.aspNetSubscription?.Dispose();
                this.allListenerSubscription?.Dispose();
            }
        }

        private class AspNetEventObserver : IObserver<KeyValuePair<string, object>>
        {
            private const string FirstRequestFlag = "Microsoft.ApplicationInsights.FirstRequestFlag";
            private readonly RequestTrackingTelemetryModule requestModule;
            private readonly ExceptionTrackingTelemetryModule exceptionModule;

            public AspNetEventObserver(RequestTrackingTelemetryModule requestModule, ExceptionTrackingTelemetryModule exceptionModule)
            {
                this.requestModule = requestModule;
                this.exceptionModule = exceptionModule;
            }

            public static Func<string, object, object, bool> IsEnabled => (name, activityObj, _) => 
            {
                if (HttpContext.Current == null)
                {
                    // should not happen
                    WebEventSource.Log.NoHttpContextWarning();
                    return false;
                }

                Activity currentActivity = Activity.Current;

                if (name == IncomingRequestEventName && 
                    activityObj is Activity && 
                    currentActivity != null && 
                    currentActivity.OperationName == IncomingRequestEventName)
                {
                    // this is a first IsEnabled call without context that ensures that Activity instrumentation is on
                    // and Activity was created by TelemetryCorrelation module
                    // If module is added twice or we get multiple BeginRequest, we already have Activity and RequestTelemetry
                    // and don't want second Activity to be created so we return false here.
                    return HttpContext.Current.GetRequestTelemetry() == null;
                }

                return true;
            };

            public static Action<Activity, object> OnActivityImport => (activity, _) =>
            {
                try
                {
                    if (activity == null)
                    {
                        // should not happen
                        WebEventSource.Log.NoHttpContextWarning();
                        return;
                    }

                    // ParentId is null, means that there were no W3C/Request-Id header, which means we have to look for AppInsights/custom headers
                    if (activity.ParentId == null)
                    {
                        var context = HttpContext.Current;
                        if (context == null)
                        {
                            WebEventSource.Log.NoHttpContextWarning();
                            return;
                        }

                        HttpRequest request = null;
                        try
                        {
                            request = context.Request;
                        }
                        catch (Exception ex)
                        {
                            WebEventSource.Log.HttpRequestNotAvailable(ex.Message, ex.StackTrace);
                            return;
                        }

                        // parse custom headers if enabled
                        if (ActivityHelpers.RootOperationIdHeaderName != null)
                        {
                            var rootId = StringUtilities.EnforceMaxLength(
                                request.UnvalidatedGetHeader(ActivityHelpers.RootOperationIdHeaderName),
                                InjectionGuardConstants.RequestHeaderMaxLength);
                            if (rootId != null)
                            {
                                activity.SetParentId(rootId);
                            }
                        }

                        // even if there was no parent, parse Correlation-Context
                        // length requirements are in https://osgwiki.com/index.php?title=CorrelationContext&oldid=459234
                        request.Headers.ReadActivityBaggage(activity);
                    }
                }
                catch (Exception e)
                {
                    WebEventSource.Log.UnknownError(e.ToString());
                }
            };

            public void OnNext(KeyValuePair<string, object> value)
            {
                try
                {
                    var context = HttpContext.Current;

                    if (value.Key == IncomingRequestStartEventName)
                    {
                        this.requestModule?.OnBeginRequest(context);
                    }
                    else if (value.Key == IncomingRequestStopEventName)
                    {
                        // AppInsights overrides Activity to allow Request-Id and legacy headers backward compatible mode
                        // it it was overriden, we need to restore it here.
                        var overrideActivity = (Activity)context.Items[ActivityHelpers.RequestActivityItemName];
                        if (overrideActivity != null && Activity.Current != overrideActivity)
                        {
                            Activity.Current = overrideActivity;
                        }

                        if (IsFirstRequest(context))
                        {
                            this.exceptionModule?.OnError(context);
                            this.requestModule?.OnEndRequest(context);
                        }
                    }
                }
                catch (Exception e)
                {
                    WebEventSource.Log.UnknownError(e.ToString());
                }
            }

            #region IObserver

            public void OnError(Exception error)
            {
            }

            public void OnCompleted()
            {
            }

            #endregion

            private static bool IsFirstRequest(HttpContext context)
            {
                var firstRequest = true;
                try
                {
                    if (context != null)
                    {
                        firstRequest = context.Items[FirstRequestFlag] == null;
                        if (firstRequest)
                        {
                            context.Items.Add(FirstRequestFlag, true);
                        }
                    }
                }
                catch (Exception exc)
                {
                    WebEventSource.Log.FlagCheckFailure(exc.ToInvariantString());
                }

                return firstRequest;
            }
        }
    }
}
