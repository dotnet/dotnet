// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor;

namespace Microsoft.CodeAnalysis;

internal static class ProjectExtensions
{
    internal static Document GetRequiredDocument(this Project project, DocumentId documentId)
    {
        return project.GetDocument(documentId)
            ?? ThrowHelper.ThrowInvalidOperationException<Document>($"The document {documentId} did not exist in {project.Name}");
    }
}
