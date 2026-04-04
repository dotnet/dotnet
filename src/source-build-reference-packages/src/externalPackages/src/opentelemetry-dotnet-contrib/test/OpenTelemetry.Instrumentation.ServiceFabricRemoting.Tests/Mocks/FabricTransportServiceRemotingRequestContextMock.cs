// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.ServiceFabric.Services.Remoting.V2.Runtime;

namespace OpenTelemetry.Instrumentation.ServiceFabricRemoting.Tests;

internal class FabricTransportServiceRemotingRequestContextMock : IServiceRemotingRequestContext
{
    public FabricTransportServiceRemotingRequestContextMock()
    {
    }

    public IServiceRemotingCallbackClient? GetCallBackClient()
    {
        return null;
    }
}
