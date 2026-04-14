// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Diagnostics;
using System.ServiceModel;

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

internal class WcfOperationContext : IExtension<OperationContext>
{
    public WcfOperationContext(Activity activity)
    {
        this.Activity = activity;
    }

    public Activity Activity { get; }

    public void Attach(OperationContext owner)
    {
    }

    public void Detach(OperationContext owner)
    {
    }
}
#endif
