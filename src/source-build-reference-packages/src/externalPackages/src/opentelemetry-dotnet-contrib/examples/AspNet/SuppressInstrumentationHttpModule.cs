// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Web;
using OpenTelemetry;

namespace Examples.AspNet;

/// <summary>
/// A demo <see cref="IHttpModule"/> which will suppress ASP.NET
/// instrumentation if a request contains "suppress=true" on the query
/// string. Suppressed spans will not be processed/exported by the
/// OpenTelemetry SDK.
/// </summary>
public class SuppressInstrumentationHttpModule : IHttpModule
{
    private IDisposable? suppressionScope;

    public void Init(HttpApplication context)
    {
        context.BeginRequest += this.Application_BeginRequest;
        context.EndRequest += this.Application_EndRequest;
    }

    public void Dispose()
    {
    }

    private void Application_BeginRequest(object sender, EventArgs e)
    {
        var context = ((HttpApplication)sender).Context;

        if (context.Request.QueryString["suppress"] == "true")
        {
            this.suppressionScope = SuppressInstrumentationScope.Begin();
        }
    }

    private void Application_EndRequest(object sender, EventArgs e)
    {
        this.suppressionScope?.Dispose();
    }
}
