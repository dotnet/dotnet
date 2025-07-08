// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

internal interface IRazorDynamicFileInfoProviderInternal
{
    void UpdateLSPFileInfo(Uri documentUri, IDynamicDocumentContainer documentContainer);
    void UpdateFileInfo(ProjectKey projectKey, IDynamicDocumentContainer documentContainer);
    void SuppressDocument(DocumentKey documentKey);
}
