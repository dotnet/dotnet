// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Text.RegularExpressions;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation;

internal static partial class GrpcTagHelper
{
    public const string RpcSystemGrpc = "grpc";

    // The Grpc.Net.Client library adds its own tags to the activity.
    // These tags are used to source the tags added by the OpenTelemetry instrumentation.
    public const string GrpcMethodTagName = "grpc.method";
    public const string GrpcStatusCodeTagName = "grpc.status_code";

    public static string? GetGrpcMethodFromActivity(Activity activity)
        => activity.GetTagValue(GrpcMethodTagName) as string;

    public static bool TryGetGrpcStatusCodeFromActivity(Activity activity, out int statusCode)
    {
        statusCode = -1;
        var grpcStatusCodeTag = activity.GetTagValue(GrpcStatusCodeTagName);
        return grpcStatusCodeTag != null && int.TryParse(grpcStatusCodeTag as string, out statusCode);
    }

    public static bool TryParseRpcServiceAndRpcMethod(string grpcMethod, out string rpcService, out string rpcMethod)
    {
        var match = GrpcMethodRegex().Match(grpcMethod);
        if (match.Success)
        {
            rpcService = match.Groups["service"].Value;
            rpcMethod = match.Groups["method"].Value;
            return true;
        }
        else
        {
            rpcService = string.Empty;
            rpcMethod = string.Empty;
            return false;
        }
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

        if (typeof(GrpcStatusCanonicalCode).IsEnumDefined(statusCode))
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
    /// to https://github.com/open-telemetry/semantic-conventions/blob/main/docs/rpc/grpc.md#server.
    /// This method is for server spans where only specific status codes are considered errors:
    /// UNKNOWN, DEADLINE_EXCEEDED, UNIMPLEMENTED, INTERNAL, UNAVAILABLE, and DATA_LOSS.
    /// </summary>
    /// <param name="statusCode">RPC status code.</param>
    /// <returns>Resolved span <see cref="Status"/> for the Grpc status code.</returns>
    public static ActivityStatusCode ResolveSpanStatusForGrpcStatusCodeOnServer(int statusCode)
    {
        if (typeof(GrpcStatusCanonicalCode).IsEnumDefined(statusCode))
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

#if NET
    [GeneratedRegex(@"^/?(?<service>.*)/(?<method>.*)$")]
    private static partial Regex GrpcMethodRegex();
#else
#pragma warning disable SA1201 // A field should not follow a method
    private static readonly Regex GrpcMethodRegexField = new(@"^/?(?<service>.*)/(?<method>.*)$", RegexOptions.Compiled);
#pragma warning restore SA1201 // A field should not follow a method

    private static Regex GrpcMethodRegex() => GrpcMethodRegexField;
#endif

}
