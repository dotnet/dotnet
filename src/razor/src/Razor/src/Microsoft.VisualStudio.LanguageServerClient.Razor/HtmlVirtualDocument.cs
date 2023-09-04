// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal class HtmlVirtualDocument(Uri uri, ITextBuffer textBuffer, ITelemetryReporter telemetryReporter)
    : GeneratedVirtualDocument<HtmlVirtualDocumentSnapshot>(uri, textBuffer, telemetryReporter)
{
    protected override HtmlVirtualDocumentSnapshot GetUpdatedSnapshot(object? state) => new(Uri, TextBuffer.CurrentSnapshot, HostDocumentVersion);
}
