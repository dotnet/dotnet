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
    public bool HandleError(Exception error)
    {
        return false;
    }

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

        if (!WcfInstrumentationActivitySource.Options?.RecordException ?? false)
        {
            return;
        }

        // OperationContext.Current *should* be reliable even in async calls at .NET 4.6.2+.
        // In older versions it may not be.
        var context = OperationContext.Current?.Extensions.Find<WcfOperationContext>();
        var activity = context?.Activity ?? WcfInstrumentationActivitySource.ActivitySource.StartActivity(WcfInstrumentationActivitySource.UnassociatedExceptionActivityName, ActivityKind.Internal);

        activity?.AddException(error);

        if (activity != context?.Activity)
        {
            activity?.Stop();
        }
    }
}
#endif
