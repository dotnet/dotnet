// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.Wcf;

internal static class WcfInstrumentationConstants
{
    public const string AttributeSoapMessageVersion = "soap.message_version";
    public const string AttributeSoapReplyAction = "soap.reply_action";
    public const string AttributeSoapVia = "soap.via";
    public const string AttributeWcfChannelScheme = "wcf.channel.scheme";
    public const string AttributeWcfChannelPath = "wcf.channel.path";

    public const string WcfSystemValue = "dotnet_wcf";

    public const string ErrorTypeOther = "_OTHER";

    /// <summary>
    /// Gets the fully-qualified logical method name for the <c>rpc.method</c> attribute as required by
    /// version 1.42.0 of the RPC semantic conventions, which no longer defines a separate <c>rpc.service</c>
    /// attribute. The contract name (when available) takes the place of the former <c>rpc.service</c> value.
    /// </summary>
    /// <param name="contractName">The optional contract name.</param>
    /// <param name="operationName">The operation name.</param>
    /// <returns>The fully-qualified logical method name.</returns>
    public static string GetRpcMethod(string? contractName, string operationName)
        => string.IsNullOrEmpty(contractName) ? operationName : string.Concat(contractName, "/", operationName);
}
