// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpAmp.Proto.V1;
using OpenTelemetry.OpAmp.Client.Internal.Services.Heartbeat;
using OpenTelemetry.OpAmp.Client.Messages;

namespace OpenTelemetry.OpAmp.Client.Internal;

internal interface IFrameBuilder
{
    IFrameBuilder AddAgentDescription();

    IFrameBuilder AddHealth(HealthReport health);

    IFrameBuilder AddAgentDisconnect();

    IFrameBuilder AddCapabilities();

    IFrameBuilder AddCustomCapabilities(IEnumerable<string> capabilities);

    IFrameBuilder AddEffectiveConfig(IEnumerable<EffectiveConfigFile> files);

    IFrameBuilder AddCustomMessage(string capability, string type, ReadOnlyMemory<byte> data);

    AgentToServer Build();
}
