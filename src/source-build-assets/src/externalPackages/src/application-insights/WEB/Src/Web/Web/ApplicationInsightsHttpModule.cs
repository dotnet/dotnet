namespace Microsoft.ApplicationInsights.Web
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Web;
    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.Web.Implementation;

    /// <summary>
    /// Platform agnostic module for web application instrumentation.
    /// </summary>
    public sealed class ApplicationInsightsHttpModule : IHttpModule
    {
        private static readonly MethodInfo OnStepMethodInfo;
        private static readonly bool AddOnSendingHeadersMethodExists;

        // Delegate preferred over Invoke to gain performance, only in NET452 or above as ISubscriptionToken is not available in Net40
        private static readonly Func<HttpResponse, Action<HttpContext>, ISubscriptionToken> OpenDelegateForInvokingAddOnSendingHeadersMethod;

        private readonly RequestTrackingTelemetryModule requestModule;
        private readonly Action<HttpContext> addOnSendingHeadersMethodParam;
        private readonly object[] onExecuteActionParam = { (Action<HttpContextBase, Action>)OnExecuteRequestStep };

        private object[] addOnSendingHeadersMethodParams;

        /// <summary>
        /// Indicates if module initialized successfully.
        /// </summary>
        private bool isEnabled = true;

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Enforcing static fields initialization.")]
        static ApplicationInsightsHttpModule()
        {
            // We use reflection here because 'AddOnSendingHeaders' is only available post .net framework 4.5.2. Hence we call it if we can find it.
            // Not using reflection would result in MissingMethodException when 4.5 or 4.5.1 is present. 
            var addOnSendingHeadersMethod = typeof(HttpResponse).GetMethod("AddOnSendingHeaders");

            if (addOnSendingHeadersMethod != null)
            {
                AddOnSendingHeadersMethodExists = true;
                OpenDelegateForInvokingAddOnSendingHeadersMethod = (Func<HttpResponse, Action<HttpContext>, ISubscriptionToken>)Delegate.CreateDelegate(
                    typeof(Func<HttpResponse, Action<HttpContext>, ISubscriptionToken>),
                    null,
                    addOnSendingHeadersMethod,
                    true);
            }

            // OnExecuteRequestStep is available starting with 4.7.1
            // If this is executed in 4.7.1 runtime (regardless of targeted .NET version),
            // we will use it to restore lost activity, otherwise keep PreRequestHandlerExecute
            OnStepMethodInfo = typeof(HttpApplication).GetMethod("OnExecuteRequestStep");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInsightsHttpModule"/> class.
        /// </summary>
        public ApplicationInsightsHttpModule()
        {
            try
            {
                // The call initializes TelemetryConfiguration that will create and Initialize modules
                TelemetryConfiguration configuration = TelemetryConfiguration.Active;
                foreach (var module in TelemetryModules.Instance.Modules)
                {
                    if (module is RequestTrackingTelemetryModule telemetryModule)
                    {
                        this.requestModule = telemetryModule;
                    }
                }

                if (AddOnSendingHeadersMethodExists)
                {
                    this.addOnSendingHeadersMethodParam = (httpContext) =>
                    {
                        try
                        {
                            this.requestModule?.AddTargetHashForResponseHeader(httpContext);
                        }
                        catch (Exception ex)
                        {
                            WebEventSource.Log.AddTargetHeaderFailedWarning(ex.ToInvariantString());
                        }
                    };
                    this.addOnSendingHeadersMethodParams = new object[] { this.addOnSendingHeadersMethodParam };
                }
            }
            catch (Exception exc)
            {
                this.isEnabled = false;
                WebEventSource.Log.WebModuleInitializationExceptionEvent(exc.ToInvariantString());
            }
        }

        /// <summary>
        /// Initializes module for a given application.
        /// </summary>
        /// <param name="context">HttpApplication instance.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Context cannot be null")]
        public void Init(HttpApplication context)
        {
            if (this.isEnabled)
            {
                try
                {
                    context.BeginRequest += this.OnBeginRequest;

                    // OnExecuteRequestStep is available starting with 4.7.1
                    // If this is executed in 4.7.1 runtime (regardless of targeted .NET version),
                    // we will use it to restore lost activity, otherwise keep PreRequestHandlerExecute
                    if (OnStepMethodInfo != null && HttpRuntime.UsingIntegratedPipeline)
                    {
                        try
                        {
                            OnStepMethodInfo.Invoke(context, this.onExecuteActionParam);
                        }
                        catch (Exception e)
                        {
                            WebEventSource.Log.OnExecuteRequestStepInvocationError(e.Message);
                        }
                    }
                    else
                    {
                        context.PreRequestHandlerExecute += this.Application_PreRequestHandlerExecute;
                    }
                }
                catch (Exception exc)
                {
                    this.isEnabled = false;
                    WebEventSource.Log.WebModuleInitializationExceptionEvent(exc.ToInvariantString());
                }
            }
        }

        /// <summary>
        /// Required IDisposable implementation.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Restores Activity before each pipeline step if it was lost.
        /// </summary>
        /// <param name="context">HttpContext instance.</param>
        /// <param name="step">Step to be executed.</param>
        private static void OnExecuteRequestStep(HttpContextBase context, Action step)
        {
            if (context == null)
            {
                WebEventSource.Log.NoHttpContextWarning();
                return;
            }

            TraceCallback(nameof(OnExecuteRequestStep), context.ApplicationInstance);
            if (context.CurrentNotification == RequestNotification.ExecuteRequestHandler && !context.IsPostNotification)
            {
                ActivityHelpers.RestoreActivityIfNeeded(context.Items);
            }

            step();
        }

        private static void TraceCallback(string callback, HttpApplication application)
        {
            if (WebEventSource.IsVerboseEnabled)
            {
                try
                {
                    if (application?.Context != null)
                    {
                        // Url.ToString internally builds local member once and then always returns it
                        // During serialization we will anyway call same ToString() so we do not force unnecessary formatting just for tracing 
                        var url = application.Context.Request.UnvalidatedGetUrl();
                        string logUrl = (url != null) ? url.ToString() : string.Empty;

                        WebEventSource.Log.WebModuleCallback(callback, logUrl);
                    }
                }
                catch (Exception exc)
                {
                    WebEventSource.Log.TraceCallbackFailure(callback, exc.ToInvariantString());
                }
            }
        }

        private void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            var httpApplication = (HttpApplication)sender;

            if (httpApplication == null)
            {
                WebEventSource.Log.NoHttpApplicationWarning();
                return;
            }

            TraceCallback(nameof(this.Application_PreRequestHandlerExecute), httpApplication);
            ActivityHelpers.RestoreActivityIfNeeded(httpApplication.Context?.Items);
        }

        private void OnBeginRequest(object sender, EventArgs eventArgs)
        {
            if (this.isEnabled)
            {
                HttpApplication httpApplication = (HttpApplication)sender;

                TraceCallback("OnBegin", httpApplication);

                if (this.requestModule != null)
                {
                    if (this.requestModule.SetComponentCorrelationHttpHeaders)
                    {
                        this.AddCorrelationHeaderOnSendRequestHeaders(httpApplication);
                    }
                }
            }
        }

        /// <summary>
        /// When sending the response headers, allow request module to add the IKey's target hash.
        /// </summary>
        /// <param name="httpApplication">HttpApplication instance.</param>
        private void AddCorrelationHeaderOnSendRequestHeaders(HttpApplication httpApplication)
        {
            try
            {
                if (httpApplication?.Response != null && AddOnSendingHeadersMethodExists)
                {                                     
                    // Faster delegate based invocation.
                    OpenDelegateForInvokingAddOnSendingHeadersMethod.Invoke(httpApplication.Response, this.addOnSendingHeadersMethodParam);
                }
            }
            catch (Exception ex)
            {
                WebEventSource.Log.HookAddOnSendingHeadersFailedWarning(ex.ToInvariantString());
            }
        }
    }
}
