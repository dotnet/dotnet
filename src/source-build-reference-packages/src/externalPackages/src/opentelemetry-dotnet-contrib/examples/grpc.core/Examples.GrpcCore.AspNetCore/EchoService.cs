// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Grpc.Core;

namespace Examples.GrpcCore.AspNetCore;

/// <summary>
/// Simple implementation of the echo service.
/// </summary>
internal class EchoService : Echo.EchoBase
{
    /// <inheritdoc/>
    public override Task<EchoResponse> Echo(EchoRequest request, ServerCallContext context)
    {
        return Task.FromResult(new EchoResponse { Message = request.Message });
    }
}
