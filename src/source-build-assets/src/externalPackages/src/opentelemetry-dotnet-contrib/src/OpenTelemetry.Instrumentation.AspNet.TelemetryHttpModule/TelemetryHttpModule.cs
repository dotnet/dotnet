// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Reflection;
using System.Web;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.AspNet;

/// <summary>
/// Http Module sets ambient state using Activity API from DiagnosticsSource package.
/// </summary>
public class TelemetryHttpModule : IHttpModule
{
    // ServerVariable set only on rewritten HttpContext by URL Rewrite module.
    private const string UrlRewriteRewrittenRequest = "IIS_WasUrlRewritten";

    // ServerVariable set on every request if URL module is registered in HttpModule pipeline.
    private const string UrlRewriteModuleVersion = "IIS_UrlRewriteModule";

    private static readonly MethodInfo? OnExecuteRequestStepMethodInfo = typeof(HttpApplication).GetMethod("OnExecuteRequestStep");

    /// <summary>
    /// Gets the <see cref="TelemetryHttpModuleOptions"/> applied to requests processed by the handler.
    /// </summary>
    public static TelemetryHttpModuleOptions Options { get; } = new();

    /// <inheritdoc />
    public void Dispose()
    {
    }

    /// <inheritdoc />
    public void Init(HttpApplication context)
    {
        Guard.ThrowIfNull(context);

        context.BeginRequest += this.Application_BeginRequest;
        context.EndRequest += this.Application_EndRequest;
        context.Error += this.Application_Error;

        if (HttpRuntime.UsingIntegratedPipeline && OnExecuteRequestStepMethodInfo != null)
        {
            // OnExecuteRequestStep is available starting with 4.7.1
            try
            {
                OnExecuteRequestStepMethodInfo.Invoke(context, [(Action<HttpContextBase, Action>)this.OnExecuteRequestStep]);
            }
            catch (Exception e)
            {
                AspNetTelemetryEventSource.Log.OnExecuteRequestStepInvocationError(e.Message);
            }
        }
    }

    private void Application_BeginRequest(object sender, EventArgs e)
    {
        AspNetTelemetryEventSource.Log.TraceCallback("Application_BeginRequest");
        ActivityHelper.StartAspNetActivity(Options.TextMapPropagator, new HttpContextWrapper(((HttpApplication)sender).Context), Options.OnRequestStartedCallback);
    }

    private void OnExecuteRequestStep(HttpContextBase context, Action step)
    {
        // Called only on 4.7.1+ runtimes
        ActivityHelper.RestoreContextIfNeeded(new HttpContextWrapper(context.ApplicationInstance.Context));
        step();
    }

    private void Application_EndRequest(object sender, EventArgs e)
    {
        AspNetTelemetryEventSource.Log.TraceCallback("Application_EndRequest");
        var trackActivity = true;

        var context = new HttpContextWrapper(((HttpApplication)sender).Context);

        if (!ActivityHelper.HasStarted(context, out var aspNetActivity))
        {
            // Rewrite: In case of rewrite, a new request context is created, called the child request, and it goes through the entire IIS/ASP.NET integrated pipeline.
            // The child request can be mapped to any of the handlers configured in IIS, and it's execution is no different than it would be if it was received via the HTTP stack.
            // The parent request jumps ahead in the pipeline to the end request notification, and waits for the child request to complete.
            // When the child request completes, the parent request executes the end request notifications and completes itself.
            // Do not create activity for parent request. Parent request has IIS_UrlRewriteModule ServerVariable with success response code.
            // Child request contains an additional ServerVariable named - IIS_WasUrlRewritten.
            // Track failed response activity: Different modules in the pipeline has ability to end the response. For example, authentication module could set HTTP 401 in OnBeginRequest and end the response.
            if (context.Request.ServerVariables != null && context.Request.ServerVariables[UrlRewriteRewrittenRequest] == null && context.Request.ServerVariables[UrlRewriteModuleVersion] != null && context.Response.StatusCode == 200)
            {
                trackActivity = false;
            }
            else
            {
                // Activity has never been started
                aspNetActivity = ActivityHelper.StartAspNetActivity(Options.TextMapPropagator, context, Options.OnRequestStartedCallback);
            }
        }

        if (trackActivity)
        {
            ActivityHelper.StopAspNetActivity(Options.TextMapPropagator, aspNetActivity, context, Options.OnRequestStoppedCallback);
        }
    }

    private void Application_Error(object sender, EventArgs e)
    {
        AspNetTelemetryEventSource.Log.TraceCallback("Application_Error");

        var context = new HttpContextWrapper(((HttpApplication)sender).Context);

        var exception = context.Error;
        if (exception != null)
        {
            if (!ActivityHelper.HasStarted(context, out var aspNetActivity))
            {
                aspNetActivity = ActivityHelper.StartAspNetActivity(Options.TextMapPropagator, context, Options.OnRequestStartedCallback);
            }

            ActivityHelper.WriteActivityException(aspNetActivity, context, exception, Options.OnExceptionCallback);
        }
    }
}
