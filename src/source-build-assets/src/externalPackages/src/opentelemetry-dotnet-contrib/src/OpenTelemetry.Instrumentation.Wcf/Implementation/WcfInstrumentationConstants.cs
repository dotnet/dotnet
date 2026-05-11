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
}
