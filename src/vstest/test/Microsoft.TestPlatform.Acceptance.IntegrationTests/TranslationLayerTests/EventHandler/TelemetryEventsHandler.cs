// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.TestPlatform.AcceptanceTests.TranslationLayerTests;

internal class TelemetryEventsHandler : ITelemetryEventsHandler
{
    public ConcurrentBag<TelemetryEvent> Events { get; private set; } = new ConcurrentBag<TelemetryEvent>();

    public void HandleTelemetryEvent(TelemetryEvent telemetryEvent)
    {
        Events.Add(telemetryEvent);
    }
}
