// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor;

namespace Microsoft.CodeAnalysis;

internal static class SolutionExtensions
{
    internal static Project GetRequiredProject(this Solution solution, ProjectId projectId)
    {
        return solution.GetProject(projectId)
            ?? ThrowHelper.ThrowInvalidOperationException<Project>($"The projectId {projectId} did not exist in {solution}.");
    }

    internal static Document GetRequiredDocument(this Solution solution, DocumentId documentId)
    {
        return solution.GetDocument(documentId)
            ?? ThrowHelper.ThrowInvalidOperationException<Document>($"The document {documentId} did not exist in {solution.FilePath ?? "solution"}.");
    }
}
