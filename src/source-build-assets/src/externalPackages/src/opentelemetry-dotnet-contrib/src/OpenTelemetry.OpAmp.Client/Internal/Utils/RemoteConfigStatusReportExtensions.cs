// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Google.Protobuf;
using OpAmp.Proto.V1;
using OpenTelemetry.OpAmp.Client.Messages;

namespace OpenTelemetry.OpAmp.Client.Internal.Utils;

internal static class RemoteConfigStatusReportExtensions
{
    public static RemoteConfigStatus ToRemoteConfigStatus(this RemoteConfigStatusReport status)
    {
        var remoteConfigStatus = new RemoteConfigStatus
        {
            LastRemoteConfigHash = ByteString.CopyFrom(status.LastRemoteConfigHash.Span),
            Status = status.Status.ToRemoteConfigStatuses(),
        };

        if (status.ErrorMessage != null)
        {
            remoteConfigStatus.ErrorMessage = status.ErrorMessage;
        }

        return remoteConfigStatus;
    }

    private static RemoteConfigStatuses ToRemoteConfigStatuses(this RemoteConfigStatusCode status)
        => status switch
        {
            RemoteConfigStatusCode.Unset => RemoteConfigStatuses.Unset,
            RemoteConfigStatusCode.Applied => RemoteConfigStatuses.Applied,
            RemoteConfigStatusCode.Applying => RemoteConfigStatuses.Applying,
            RemoteConfigStatusCode.Failed => RemoteConfigStatuses.Failed,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unsupported remote configuration status."),
        };
}
