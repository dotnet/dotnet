// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.Wcf.Implementation;

internal sealed class TelemetryContextMessageProperty
{
    public const string Name = "OpenTelemetry.Instrumentation.Wcf.Implementation.TelemetryContextMessageProperty";

    public TelemetryContextMessageProperty(IDictionary<string, ActionMetadata> actionMappings)
    {
        this.ActionMappings = actionMappings;
    }

    public IDictionary<string, ActionMetadata> ActionMappings { get; set; }
}
