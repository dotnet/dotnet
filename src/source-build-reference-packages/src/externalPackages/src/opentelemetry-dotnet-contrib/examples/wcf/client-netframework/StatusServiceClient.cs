// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel;

namespace Examples.Wcf.Client;

internal class StatusServiceClient : ClientBase<IStatusServiceContract>, IStatusServiceContract
{
    public StatusServiceClient(string name)
        : base(name)
    {
    }

    public Task<StatusResponse> PingAsync(StatusRequest request)
        => this.Channel.PingAsync(request);

    public Task OpenAsync()
    {
        ICommunicationObject communicationObject = this;
        return Task.Factory.FromAsync(communicationObject.BeginOpen, communicationObject.EndOpen, null);
    }

    public Task CloseAsync()
    {
        ICommunicationObject communicationObject = this;
        return Task.Factory.FromAsync(communicationObject.BeginClose, communicationObject.EndClose, null);
    }
}
