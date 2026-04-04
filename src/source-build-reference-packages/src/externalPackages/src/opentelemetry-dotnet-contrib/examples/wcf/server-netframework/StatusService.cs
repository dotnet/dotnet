// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.ServiceModel;

namespace Examples.Wcf.Server;

[ServiceBehavior(
    Namespace = "http://opentelemetry.io/",
    ConcurrencyMode = ConcurrencyMode.Multiple,
    InstanceContextMode = InstanceContextMode.Single,
    UseSynchronizationContext = false,
    Name = "StatusService")]
internal class StatusService : IStatusServiceContract
{
    public Task<StatusResponse> PingAsync(StatusRequest request)
    {
        return Task.FromResult(
            new StatusResponse
            {
                ServerTime = DateTimeOffset.UtcNow,
            });
    }
}
