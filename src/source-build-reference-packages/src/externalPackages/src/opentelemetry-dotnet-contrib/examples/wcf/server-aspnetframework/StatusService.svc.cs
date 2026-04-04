// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace Examples.Wcf.Server.AspNetFramework;

public class StatusService : IStatusServiceContract
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
