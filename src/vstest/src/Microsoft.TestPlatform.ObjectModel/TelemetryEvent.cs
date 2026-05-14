// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

public sealed class TelemetryEvent
{
    /// <summary>
    /// Initialize an TelemetryEvent
    /// </summary>
    /// <param name="name">Telemetry event name</param>
    /// <param name="properties">Telemetry event properties</param>
    public TelemetryEvent(string name, IDictionary<string, object> properties)
    {
        Name = name;
        Properties = properties;
    }

    /// <summary>
    /// Telemetry event name.
    /// </summary>
    [DataMember]
    public string Name { get; private set; }

    /// <summary>
    /// Telemetry event properties.
    /// </summary>
    [DataMember]
    public IDictionary<string, object> Properties { get; private set; }
}
