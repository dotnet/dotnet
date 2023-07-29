// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal abstract class GeneratedDocumentPublisher : IProjectSnapshotChangeTrigger
{
    public abstract void Initialize(ProjectSnapshotManagerBase projectManager);

    public abstract void PublishCSharp(ProjectKey projectKey, string filePath, SourceText sourceText, int hostDocumentVersion);

    public abstract void PublishHtml(ProjectKey projectKey, string filePath, SourceText sourceText, int hostDocumentVersion);
}
