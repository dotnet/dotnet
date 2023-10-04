// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal class CSharpVirtualDocument(ProjectKey projectKey, Uri uri, ITextBuffer textBuffer, ITelemetryReporter telemetryReporter)
    : GeneratedVirtualDocument<CSharpVirtualDocumentSnapshot>(uri, textBuffer, telemetryReporter)
{
    // NOTE: The base constructor calls GetUpdateSnapshot, so this only works because we're using primary constructors, which
    //       will initialize the field before calling the base constructor.
    private readonly ProjectKey _projectKey = projectKey;

    internal ProjectKey ProjectKey => _projectKey;

    protected override CSharpVirtualDocumentSnapshot GetUpdatedSnapshot(object? state) => new(_projectKey, Uri, TextBuffer.CurrentSnapshot, HostDocumentVersion);
}
