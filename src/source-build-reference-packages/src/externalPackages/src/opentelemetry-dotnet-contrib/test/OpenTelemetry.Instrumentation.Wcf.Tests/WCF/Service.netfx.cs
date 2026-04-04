// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.ServiceModel;

namespace OpenTelemetry.Instrumentation.Wcf.Tests;

[ServiceBehavior(
    Namespace = "http://opentelemetry.io/",
    ConcurrencyMode = ConcurrencyMode.Multiple,
    InstanceContextMode = InstanceContextMode.Single,
    UseSynchronizationContext = false,
    Name = "Service")]
public class Service : IServiceContract
{
    public Task ErrorAsync()
    {
        throw new Exception();
    }

    public void ErrorSynchronous()
    {
        throw new Exception();
    }

    public Task<ServiceResponse> ExecuteAsync(ServiceRequest request)
    {
        return Task.FromResult(
            new ServiceResponse(
                payload: $"RSP: {request.Payload}"));
    }

    public ServiceResponse ExecuteSynchronous(ServiceRequest request)
    {
        return new ServiceResponse(
                payload: $"RSP: {request.Payload}");
    }

    public Task<ServiceResponse> ExecuteWithEmptyActionNameAsync(ServiceRequest request)
    {
        return Task.FromResult(
            new ServiceResponse(
                payload: $"RSP: {request.Payload}"));
    }

    public void ExecuteWithOneWay(ServiceRequest request)
    {
    }
}

#endif
