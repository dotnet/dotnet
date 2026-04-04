// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Microsoft.ServiceFabric.Services.Remoting.V2;

namespace OpenTelemetry.Instrumentation.ServiceFabricRemoting.Tests;

internal class ServiceRemotingRequestMessageMock : IServiceRemotingRequestMessage
{
    private readonly IServiceRemotingRequestMessageHeader header;

    private readonly IServiceRemotingRequestMessageBody msgBody;

    public ServiceRemotingRequestMessageMock(IServiceRemotingRequestMessageHeader header, IServiceRemotingRequestMessageBody msgBody)
    {
        this.header = header;
        this.msgBody = msgBody;
    }

    public IServiceRemotingRequestMessageHeader GetHeader()
    {
        return this.header;
    }

    public IServiceRemotingRequestMessageBody GetBody()
    {
        return this.msgBody;
    }
}
