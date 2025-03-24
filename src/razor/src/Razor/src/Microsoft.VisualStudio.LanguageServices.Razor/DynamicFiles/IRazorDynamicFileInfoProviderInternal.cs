// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

internal interface IRazorDynamicFileInfoProviderInternal
{
    void UpdateLSPFileInfo(Uri documentUri, IDynamicDocumentContainer documentContainer);
    void UpdateFileInfo(ProjectKey projectKey, IDynamicDocumentContainer documentContainer);
    void SuppressDocument(DocumentKey documentKey);
}
