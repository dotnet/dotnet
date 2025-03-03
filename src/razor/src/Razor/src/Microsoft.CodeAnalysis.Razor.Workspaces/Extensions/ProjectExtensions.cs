// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

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

    /// <summary>
    /// Finds source generated documents by iterating through all of them. In OOP there are better options!
    /// </summary>
    public static async Task<SourceGeneratedDocument?> TryGetSourceGeneratedDocumentFromHintNameAsync(this Project project, string? hintName, CancellationToken cancellationToken)
    {
        // TODO: use this when the location is case-insensitive on windows (https://github.com/dotnet/roslyn/issues/76869)
        //var generator = typeof(RazorSourceGenerator);
        //var generatorAssembly = generator.Assembly;
        //var generatorName = generatorAssembly.GetName();
        //var generatedDocuments = await _project.GetSourceGeneratedDocumentsForGeneratorAsync(generatorName.Name!, generatorAssembly.Location, generatorName.Version!, generator.Name, cancellationToken).ConfigureAwait(false);

        var generatedDocuments = await project.GetSourceGeneratedDocumentsAsync(cancellationToken).ConfigureAwait(false);
        return generatedDocuments.SingleOrDefault(d => d.HintName == hintName);
    }

    /// <summary>
    /// Finds source generated documents by iterating through all of them. In OOP there are better options!
    /// </summary>
    public static async Task<string?> TryGetHintNameFromGeneratedDocumentUriAsync(this Project project, Uri generatedDocumentUri, CancellationToken cancellationToken)
    {
        // TODO: Call SourceGeneratedDocumentUri.DeserializeIdentity: https://github.com/dotnet/razor/issues/11557

        var generatedDocuments = await project.GetSourceGeneratedDocumentsAsync(cancellationToken).ConfigureAwait(false);
        var document = generatedDocuments.SingleOrDefault(d => generatedDocumentUri.Equals(d.CreateUri()));
        return document?.HintName;
    }
}
