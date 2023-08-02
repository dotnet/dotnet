// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.VsTestConsole.TranslationLayer;

internal class NoOpTelemetryEventsHandler : ITelemetryEventsHandler
{
    public void HandleTelemetryEvent(TelemetryEvent telemetryEvent)
    {
        // no op
    }
}
