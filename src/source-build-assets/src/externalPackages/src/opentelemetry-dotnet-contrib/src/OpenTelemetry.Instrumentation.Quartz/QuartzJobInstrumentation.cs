// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Instrumentation.Quartz.Implementation;

namespace OpenTelemetry.Instrumentation.Quartz;

internal class QuartzJobInstrumentation : IDisposable
{
    internal const string QuartzDiagnosticListenerName = "Quartz";
    private readonly DiagnosticSourceSubscriber diagnosticSourceSubscriber;

    public QuartzJobInstrumentation()
        : this(new QuartzInstrumentationOptions())
    {
    }

    public QuartzJobInstrumentation(QuartzInstrumentationOptions options)
    {
        this.diagnosticSourceSubscriber = new DiagnosticSourceSubscriber(
            name => new QuartzDiagnosticListener(name, options),
            listener => listener.Name == QuartzDiagnosticListenerName,
            null,
            QuartzInstrumentationEventSource.Log.UnknownErrorProcessingEvent);
        this.diagnosticSourceSubscriber.Subscribe();
    }

    public void Dispose()
    {
        this.diagnosticSourceSubscriber?.Dispose();
    }
}
