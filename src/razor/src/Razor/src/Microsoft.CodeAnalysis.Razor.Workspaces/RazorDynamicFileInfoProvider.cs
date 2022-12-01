// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal abstract class RazorDynamicFileInfoProvider : ProjectSnapshotChangeTrigger
{
    public abstract void UpdateLSPFileInfo(Uri documentUri, DynamicDocumentContainer documentContainer);

    public abstract void UpdateFileInfo(string projectFilePath, DynamicDocumentContainer documentContainer);

    public abstract void SuppressDocument(string projectFilePath, string documentFilePath);
}
