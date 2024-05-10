// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal interface IGeneratedDocumentPublisher
{
    void PublishCSharp(ProjectKey projectKey, string filePath, SourceText sourceText, int hostDocumentVersion);
    void PublishHtml(ProjectKey projectKey, string filePath, SourceText sourceText, int hostDocumentVersion);
}
