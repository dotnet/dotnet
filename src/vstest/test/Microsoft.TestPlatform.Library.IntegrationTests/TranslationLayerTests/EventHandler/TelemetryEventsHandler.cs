// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.TestPlatform.Library.IntegrationTests.TranslationLayerTests.EventHandler;

internal class TelemetryEventsHandler : ITelemetryEventsHandler
{
    public ConcurrentList<TelemetryEvent> Events { get; private set; } = new ConcurrentList<TelemetryEvent>();

    public void HandleTelemetryEvent(TelemetryEvent telemetryEvent)
    {
        Events.Add(telemetryEvent);
    }
}
