// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Google.Protobuf;

namespace OpenTelemetry.OpAmp.Client.Internal.Transport;

internal interface IOpAmpTransport
{
    Task SendAsync<T>(T message, CancellationToken token)
        where T : IMessage<T>;
}
