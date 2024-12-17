// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Razor;

namespace Microsoft.CodeAnalysis;

internal static class ProjectExtensions
{
    internal static Document GetRequiredDocument(this Project project, DocumentId documentId)
    {
        return project.GetDocument(documentId)
            ?? ThrowHelper.ThrowInvalidOperationException<Document>($"The document {documentId} did not exist in {project.Name}");
    }

    public static bool TryGetCSharpDocument(this Project project, Uri csharpDocumentUri, [NotNullWhen(true)] out Document? document)
    {
        document = null;

        var generatedDocumentIds = project.Solution.GetDocumentIdsWithUri(csharpDocumentUri);
        var generatedDocumentId = generatedDocumentIds.FirstOrDefault(d => d.ProjectId == project.Id);
        if (generatedDocumentId is null)
        {
            return false;
        }

        if (project.GetDocument(generatedDocumentId) is { } generatedDocument)
        {
            document = generatedDocument;
        }

        return document is not null;
    }

}
