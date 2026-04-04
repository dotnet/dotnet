// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Examples.Wcf.Client;

public class StatusServiceClient : ClientBase<IStatusServiceContract>, IStatusServiceContract
{
    public StatusServiceClient(Binding binding, EndpointAddress remoteAddress)
        : base(binding, remoteAddress)
    {
    }

    public Task<StatusResponse> PingAsync(StatusRequest request)
        => this.Channel.PingAsync(request);

    public Task OpenAsync()
    {
        ICommunicationObject communicationObject = this;
        return Task.Factory.FromAsync(communicationObject.BeginOpen, communicationObject.EndOpen, null);
    }
}
