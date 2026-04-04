// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel;
#if NETFRAMEWORK
using System.ServiceModel.Web;
#endif

namespace Examples.Wcf;

[ServiceContract(Namespace = "http://opentelemetry.io/", Name = "StatusService", SessionMode = SessionMode.Allowed)]
public interface IStatusServiceContract
{
#if NETFRAMEWORK
    [WebInvoke]
#endif
    [OperationContract]
    Task<StatusResponse> PingAsync(StatusRequest request);
}
