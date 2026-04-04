// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace OpenTelemetry.Instrumentation.Wcf.Tests.Tools;

internal class ErrorHandler : IErrorHandler
{
    private readonly EventWaitHandle handle;
    private readonly Action<Exception> log;

    public ErrorHandler(EventWaitHandle handle, Action<Exception> log)
    {
        this.handle = handle;
        this.log = log;
    }

    public bool HandleError(Exception error)
    {
        this.log(error);
        this.handle.Set();

        return true;
    }

    public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
    {
    }
}

#endif
