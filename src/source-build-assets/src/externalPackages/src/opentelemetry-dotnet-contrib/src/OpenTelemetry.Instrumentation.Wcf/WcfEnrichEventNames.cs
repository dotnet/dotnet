// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.Wcf;

/// <summary>
/// Constants used for event names when enriching an activity.
/// </summary>
public static class WcfEnrichEventNames
{
#if NETFRAMEWORK
    /// <summary>
    /// WCF service activity, event happens before WCF service method is invoked.
    /// </summary>
    public const string AfterReceiveRequest = "AfterReceiveRequest";

    /// <summary>
    /// WCF service activity, event happens after the WCF service method is invoked but before the reply is sent back to the client.
    /// </summary>
    public const string BeforeSendReply = "BeforeSendReply";
#endif

    /// <summary>
    /// WCF client activity, event happens before the request is sent across the wire.
    /// </summary>
    public const string BeforeSendRequest = "BeforeSendRequest";

    /// <summary>
    /// WCF client activity, event happens after a reply from the WCF service is received.
    /// </summary>
    public const string AfterReceiveReply = "AfterReceiveReply";
}
