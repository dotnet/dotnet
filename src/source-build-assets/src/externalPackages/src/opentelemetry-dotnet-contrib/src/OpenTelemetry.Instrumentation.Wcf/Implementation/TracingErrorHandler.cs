// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

internal class TracingErrorHandler : IErrorHandler
{
    public bool HandleError(Exception error) => false;

    public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
    {
        // By rights this should be in `HandleError` instead, which would keep it from
        // interfering with the response to the client.
        // However, by the time `HandleError` fires, the Activity has already been stopped
        // so the error appears after the Activity has completed.  Additionally, sometimes
        // the context has already been disposed or otherwise lost, preventing association
        // at all.
        // Also it becomes very difficult to unit-test, because there is no easy `ErrorsHandled`
        // event to listen for before checking to see whether the error was logged.
        var options = WcfInstrumentationActivitySource.Options;

        if (options?.RecordException != true)
        {
            return;
        }

        // TelemetryDispatchMessageInspector adds WcfOperationContext only after a request
        // has passed filtering. Without it, exception recording must not create telemetry
        // for requests that were intentionally filtered out.
        var context = OperationContext.Current?.Extensions.Find<WcfOperationContext>();
        if (context == null)
        {
            return;
        }

        var activity = context.Activity;

        activity ??= WcfInstrumentationActivitySource.Get(options).StartActivity(
            WcfInstrumentationActivitySource.UnassociatedExceptionActivityName,
            ActivityKind.Internal);

        if (activity != null)
        {
            activity.AddException(error);

            if (activity != context.Activity)
            {
                activity.Stop();
            }
        }
    }
}
#endif
