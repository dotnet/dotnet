// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Instrumentation.EntityFrameworkCore.Implementation;

namespace OpenTelemetry.Instrumentation.EntityFrameworkCore;

internal class EntityFrameworkInstrumentation : IDisposable
{
    private readonly DiagnosticSourceSubscriber diagnosticSourceSubscriber;

    public EntityFrameworkInstrumentation(EntityFrameworkInstrumentationOptions? options)
    {
        this.diagnosticSourceSubscriber = new DiagnosticSourceSubscriber(
            name => new EntityFrameworkDiagnosticListener(name, options),
            listener => listener.Name == EntityFrameworkDiagnosticListener.DiagnosticSourceName,
            null,
            EntityFrameworkInstrumentationEventSource.Log.UnknownErrorProcessingEvent);
        this.diagnosticSourceSubscriber.Subscribe();
    }

    public void Dispose()
    {
        this.diagnosticSourceSubscriber?.Dispose();
    }
}
