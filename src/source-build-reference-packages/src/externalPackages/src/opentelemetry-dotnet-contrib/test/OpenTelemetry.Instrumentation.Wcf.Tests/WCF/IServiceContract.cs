// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel;

namespace OpenTelemetry.Instrumentation.Wcf.Tests;

[ServiceContract(Namespace = "http://opentelemetry.io/", Name = "Service", SessionMode = SessionMode.Allowed)]
public interface IServiceContract
{
    [OperationContract]
    Task<ServiceResponse> ExecuteAsync(ServiceRequest request);

    [OperationContract]
    ServiceResponse ExecuteSynchronous(ServiceRequest request);

    [OperationContract(Action = "")]
    Task<ServiceResponse> ExecuteWithEmptyActionNameAsync(ServiceRequest request);

    [OperationContract(IsOneWay = true)]
    void ExecuteWithOneWay(ServiceRequest request);

    [OperationContract]
    void ErrorSynchronous();

    [OperationContract]
    Task ErrorAsync();
}
