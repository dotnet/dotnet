// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

internal class CSharpVirtualDocument(ProjectKey projectKey, Uri uri, ITextBuffer textBuffer, ITelemetryReporter telemetryReporter)
    : GeneratedVirtualDocument<CSharpVirtualDocumentSnapshot>(uri, textBuffer, telemetryReporter)
{
    // NOTE: The base constructor calls GetUpdateSnapshot, so this only works because we're using primary constructors, which
    //       will initialize the field before calling the base constructor.
    private readonly ProjectKey _projectKey = projectKey;

    internal ProjectKey ProjectKey => _projectKey;

    protected override CSharpVirtualDocumentSnapshot GetUpdatedSnapshot(object? state) => new(_projectKey, Uri, TextBuffer.CurrentSnapshot, HostDocumentVersion);
}
