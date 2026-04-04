// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel;
using System.ServiceModel.Channels;

namespace OpenTelemetry.Instrumentation.Wcf.Tests;

public class ServiceClient : ClientBase<IServiceContract>, IServiceContract
{
    public ServiceClient(Binding binding, EndpointAddress remoteAddress)
        : base(binding, remoteAddress)
    {
    }

    public Task<ServiceResponse> ExecuteAsync(ServiceRequest request)
        => this.Channel.ExecuteAsync(request);

    public ServiceResponse ExecuteSynchronous(ServiceRequest request)
        => this.Channel.ExecuteSynchronous(request);

    public Task<ServiceResponse> ExecuteWithEmptyActionNameAsync(ServiceRequest request)
        => this.Channel.ExecuteWithEmptyActionNameAsync(request);

    public void ExecuteWithOneWay(ServiceRequest request)
        => this.Channel.ExecuteWithOneWay(request);

    public Task ErrorAsync()
        => this.Channel.ErrorAsync();

    public void ErrorSynchronous()
        => this.Channel.ErrorSynchronous();
}
