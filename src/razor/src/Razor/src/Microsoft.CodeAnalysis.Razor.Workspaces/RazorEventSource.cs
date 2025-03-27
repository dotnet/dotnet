// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.Tracing;

namespace Microsoft.AspNetCore.Razor;

[EventSource(Name = "RazorEventSource")]
internal sealed class RazorEventSource : EventSource
{
    public static readonly RazorEventSource Instance = new();

    private RazorEventSource()
    {
    }

    [Event(1, Level = EventLevel.Informational)]
    public void BackgroundDocumentGeneratorIdle() => WriteEvent(1);
}
