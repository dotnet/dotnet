// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Globalization;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation;

internal static class GrpcTagHelper
{
    public const string RpcSystemGrpc = "grpc";

    // The value used for rpc.method when the gRPC method is not recognized, in which case
    // the original value is preserved in rpc.method_original.
    // See https://github.com/open-telemetry/semantic-conventions/blob/v1.42.0/docs/rpc/grpc.md
    public const string RpcMethodOther = "_OTHER";

    // The value used by the gRPC libraries for grpc.method when the method is not recognized.
    // It maps to the "_OTHER" value used for rpc.method.
    // See https://github.com/open-telemetry/semantic-conventions/blob/v1.42.0/docs/non-normative/compatibility/grpc.md#attribute-mapping
    public const string GrpcMethodOther = "other";

    // The Grpc.Net.Client library adds its own tags to the activity.
    // These tags are used to source the tags added by the OpenTelemetry instrumentation.
    // See https://github.com/open-telemetry/semantic-conventions/blob/v1.42.0/docs/non-normative/compatibility/grpc.md#attribute-mapping
    public const string GrpcMethodTagName = "grpc.method";
    public const string GrpcStatusTagName = "grpc.status";
    public const string GrpcStatusCodeTagName = "grpc.status_code";
    public const string GrpcTargetTagName = "grpc.target";

    public static string? GetGrpcMethodFromActivity(Activity activity)
        => activity.GetTagValue(GrpcMethodTagName) as string;

    public static void SetGrpcSystemName(Activity activity)
        => activity.SetTag(SemanticConventions.AttributeRpcSystemName, RpcSystemGrpc);

    public static void SetGrpcMethodAndDisplayNameFromActivity(Activity activity, string? grpcMethod = null)
    {
        grpcMethod ??= activity.GetTagValue(GrpcMethodTagName) as string;

        if (grpcMethod == null)
        {
            return;
        }

        var trimmedMethod = grpcMethod.Trim('/');

        if (string.Equals(trimmedMethod, GrpcMethodOther, StringComparison.Ordinal))
        {
            // The gRPC libraries use "other" when the method is not recognized. This maps to
            // rpc.method "_OTHER" with the original value preserved in rpc.method_original, and the
            // span is named after the RPC system as rpc.method is not a usable span name.
            // See https://github.com/open-telemetry/semantic-conventions/blob/v1.42.0/docs/non-normative/compatibility/grpc.md#attribute-mapping
            activity.DisplayName = RpcSystemGrpc;
            activity.SetTag(SemanticConventions.AttributeRpcMethod, RpcMethodOther);
            activity.SetTag(SemanticConventions.AttributeRpcMethodOriginal, trimmedMethod);
        }
        else
        {
            // The RPC semantic conventions indicate the span name should be rpc.method when it is available.
            activity.DisplayName = trimmedMethod;
            activity.SetTag(SemanticConventions.AttributeRpcMethod, trimmedMethod);
        }

        // Remove the grpc.method tag added by the gRPC .NET library, if present.
        activity.SetTag(GrpcMethodTagName, null);
    }

    public static bool TryGetGrpcStatusCodeFromActivity(Activity activity, out int statusCode)
    {
        statusCode = -1;
        var grpcStatusCodeTag = activity.GetTagValue(GrpcStatusCodeTagName);
        return grpcStatusCodeTag != null &&
               int.TryParse(grpcStatusCodeTag as string, NumberStyles.None, CultureInfo.InvariantCulture, out statusCode);
    }

    /// <summary>
    /// Helper method that populates span properties from RPC status code according
    /// to https://github.com/open-telemetry/semantic-conventions/blob/main/docs/rpc/grpc.md#client.
    /// This method is for client spans where all non-OK status codes are considered errors.
    /// </summary>
    /// <param name="statusCode">RPC status code.</param>
    /// <returns>Resolved span <see cref="Status"/> for the Grpc status code.</returns>
    public static ActivityStatusCode ResolveSpanStatusForGrpcStatusCodeOnClient(int statusCode)
    {
        var status = ActivityStatusCode.Error;

        if (statusCode is >= 0 and <= (int)GrpcStatusCanonicalCode.MaxValue)
        {
            status = (GrpcStatusCanonicalCode)statusCode switch
            {
                GrpcStatusCanonicalCode.Aborted => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.AlreadyExists => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.Cancelled => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.DataLoss => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.DeadlineExceeded => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.FailedPrecondition => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.Internal => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.InvalidArgument => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.NotFound => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.Ok => ActivityStatusCode.Unset,
                GrpcStatusCanonicalCode.OutOfRange => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.PermissionDenied => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.ResourceExhausted => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.Unauthenticated => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.Unavailable => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.Unimplemented => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.Unknown => ActivityStatusCode.Error,
                _ => ActivityStatusCode.Error,
            };
        }

        return status;
    }

    /// <summary>
    /// Helper method that populates span properties from RPC status code according
    /// to https://github.com/open-telemetry/semantic-conventions/blob/v1.42.0/docs/rpc/grpc.md.
    /// This method is for server spans where only specific status codes are considered errors:
    /// UNKNOWN, DEADLINE_EXCEEDED, UNIMPLEMENTED, INTERNAL, UNAVAILABLE, and DATA_LOSS.
    /// </summary>
    /// <param name="statusCode">RPC status code.</param>
    /// <returns>Resolved span <see cref="Status"/> for the Grpc status code.</returns>
    public static ActivityStatusCode ResolveSpanStatusForGrpcStatusCodeOnServer(int statusCode)
    {
        if (statusCode is >= 0 and <= (int)GrpcStatusCanonicalCode.MaxValue)
        {
#pragma warning disable IDE0072 // Add missing cases
            return (GrpcStatusCanonicalCode)statusCode switch
            {
                GrpcStatusCanonicalCode.DataLoss => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.DeadlineExceeded => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.Internal => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.Unavailable => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.Unimplemented => ActivityStatusCode.Error,
                GrpcStatusCanonicalCode.Unknown => ActivityStatusCode.Error,
                _ => ActivityStatusCode.Unset,
            };
#pragma warning restore IDE0072 // Add missing cases
        }

        // Unknown status code, treat as error
        return ActivityStatusCode.Error;
    }

    /// <summary>
    /// Gets the string representation of a gRPC status code to use for the
    /// <c>rpc.response.status_code</c> and <c>error.type</c> attributes.
    /// </summary>
    /// <remarks>
    /// See https://github.com/grpc/grpc/blob/v1.81.1/doc/statuscodes.md and
    /// https://github.com/open-telemetry/semantic-conventions/blob/v1.42.0/docs/rpc/grpc.md.
    /// </remarks>
    /// <param name="statusCode">The numeric gRPC status code.</param>
    /// <returns>The canonical gRPC status code name (e.g. <c>OK</c>, <c>DEADLINE_EXCEEDED</c>),
    /// or the numeric value as a string if the code is not recognized.</returns>
    public static string GetGrpcStatusCodeName(int statusCode) => statusCode switch
    {
        (int)GrpcStatusCanonicalCode.Ok => "OK",
        (int)GrpcStatusCanonicalCode.Cancelled => "CANCELLED",
        (int)GrpcStatusCanonicalCode.Unknown => "UNKNOWN",
        (int)GrpcStatusCanonicalCode.InvalidArgument => "INVALID_ARGUMENT",
        (int)GrpcStatusCanonicalCode.DeadlineExceeded => "DEADLINE_EXCEEDED",
        (int)GrpcStatusCanonicalCode.NotFound => "NOT_FOUND",
        (int)GrpcStatusCanonicalCode.AlreadyExists => "ALREADY_EXISTS",
        (int)GrpcStatusCanonicalCode.PermissionDenied => "PERMISSION_DENIED",
        (int)GrpcStatusCanonicalCode.ResourceExhausted => "RESOURCE_EXHAUSTED",
        (int)GrpcStatusCanonicalCode.FailedPrecondition => "FAILED_PRECONDITION",
        (int)GrpcStatusCanonicalCode.Aborted => "ABORTED",
        (int)GrpcStatusCanonicalCode.OutOfRange => "OUT_OF_RANGE",
        (int)GrpcStatusCanonicalCode.Unimplemented => "UNIMPLEMENTED",
        (int)GrpcStatusCanonicalCode.Internal => "INTERNAL",
        (int)GrpcStatusCanonicalCode.Unavailable => "UNAVAILABLE",
        (int)GrpcStatusCanonicalCode.DataLoss => "DATA_LOSS",
        (int)GrpcStatusCanonicalCode.Unauthenticated => "UNAUTHENTICATED",
        _ => statusCode.ToString(CultureInfo.InvariantCulture),
    };
}
