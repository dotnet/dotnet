// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal interface IGeneratedDocumentPublisher
{
    void PublishCSharp(ProjectKey projectKey, string filePath, SourceText sourceText, int hostDocumentVersion);
    void PublishHtml(ProjectKey projectKey, string filePath, SourceText sourceText, int hostDocumentVersion);
}
