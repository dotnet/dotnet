// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DefaultRazorComponentSearchEngine : RazorComponentSearchEngine
{
    private readonly ProjectSnapshotManager _projectSnapshotManager;
    private readonly ILogger _logger;

    public DefaultRazorComponentSearchEngine(
        ProjectSnapshotManagerAccessor projectSnapshotManagerAccessor,
        IRazorLoggerFactory loggerFactory)
    {
        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _projectSnapshotManager = projectSnapshotManagerAccessor?.Instance ?? throw new ArgumentNullException(nameof(projectSnapshotManagerAccessor));
        _logger = loggerFactory.CreateLogger<DefaultRazorComponentSearchEngine>();
    }

    public async override Task<TagHelperDescriptor?> TryGetTagHelperDescriptorAsync(IDocumentSnapshot documentSnapshot, CancellationToken cancellationToken)
    {
        // No point doing anything if its not a component
        if (documentSnapshot.FileKind != FileKinds.Component)
        {
            return null;
        }

        var razorCodeDocument = await documentSnapshot.GetGeneratedOutputAsync().ConfigureAwait(false);
        if (razorCodeDocument is null)
        {
            return null;
        }

        var projects = _projectSnapshotManager.GetProjects();

        foreach (var project in projects)
        {
            // If the document is an import document, then it can't be a component
            if (project.IsImportDocument(documentSnapshot))
            {
                return null;
            }

            // If the document isn't in this project, then no point searching for components
            // This also avoids the issue of duplicate components
            if (!project.DocumentFilePaths.Contains(documentSnapshot.FilePath))
            {
                return null;
            }

            // If we got this far, we can check for tag helpers
            foreach (var tagHelper in project.TagHelpers)
            {
                // Check the typename and namespace match
                if (IsPathCandidateForComponent(documentSnapshot, tagHelper.GetTypeNameIdentifier().AsMemory()) &&
                    ComponentNamespaceMatchesFullyQualifiedName(razorCodeDocument, tagHelper.GetTypeNamespace().AsSpan()))
                {
                    return tagHelper;
                }
            }
        }

        return null;
    }

    /// <summary>Search for a component in a project based on its tag name and fully qualified name.</summary>
    /// <remarks>
    /// This method makes several assumptions about the nature of components. First, it assumes that a component
    /// a given name `Name` will be located in a file `Name.razor`. Second, it assumes that the namespace the
    /// component is present in has the same name as the assembly its corresponding tag helper is loaded from.
    /// Implicitly, this method inherits any assumptions made by TrySplitNamespaceAndType.
    /// </remarks>
    /// <param name="tagHelper">A TagHelperDescriptor to find the corresponding Razor component for.</param>
    /// <returns>The corresponding DocumentSnapshot if found, null otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="tagHelper"/> is null.</exception>
    public override async Task<IDocumentSnapshot?> TryLocateComponentAsync(TagHelperDescriptor tagHelper)
    {
        if (tagHelper is null)
        {
            throw new ArgumentNullException(nameof(tagHelper));
        }

        var typeName = tagHelper.GetTypeNameIdentifier();
        var namespaceName = tagHelper.GetTypeNamespace();
        if (typeName == null || namespaceName == null)
        {
            _logger.LogWarning("Could not split namespace and type for name {tagHelperName}.", tagHelper.Name);
            return null;
        }

        var lookupSymbolName = RemoveGenericContent(typeName.AsMemory());

        var projects = _projectSnapshotManager.GetProjects();

        foreach (var project in projects)
        {
            foreach (var path in project.DocumentFilePaths)
            {
                // Get document and code document
                if (project.GetDocument(path) is not { } documentSnapshot)
                {
                    continue;
                }

                // Rule out if not Razor component with correct name
                if (!IsPathCandidateForComponent(documentSnapshot, lookupSymbolName))
                {
                    continue;
                }

                var razorCodeDocument = await documentSnapshot.GetGeneratedOutputAsync().ConfigureAwait(false);
                if (razorCodeDocument is null)
                {
                    continue;
                }

                // Make sure we have the right namespace of the fully qualified name
                if (!ComponentNamespaceMatchesFullyQualifiedName(razorCodeDocument, namespaceName.AsSpan()))
                {
                    continue;
                }

                return documentSnapshot;
            }
        }

        return null;
    }

    internal static ReadOnlyMemory<char> RemoveGenericContent(ReadOnlyMemory<char> typeName)
    {
        var genericSeparatorStart = typeName.Span.IndexOf('<');

        return genericSeparatorStart > 0
            ? typeName[..genericSeparatorStart]
            : typeName;
    }

    private static bool IsPathCandidateForComponent(IDocumentSnapshot documentSnapshot, ReadOnlyMemory<char> path)
    {
        if (documentSnapshot.FileKind != FileKinds.Component)
        {
            return false;
        }

        var fileName = Path.GetFileNameWithoutExtension(documentSnapshot.FilePath);
        return fileName.AsSpan().Equals(path.Span, FilePathComparison.Instance);
    }

    private bool ComponentNamespaceMatchesFullyQualifiedName(RazorCodeDocument razorCodeDocument, ReadOnlySpan<char> namespaceName)
    {
        var namespaceNode = (NamespaceDeclarationIntermediateNode)razorCodeDocument
            .GetDocumentIntermediateNode()
            .FindDescendantNodes<IntermediateNode>()
            .First(n => n is NamespaceDeclarationIntermediateNode);

        var namespacesMatch = namespaceNode.Content.AsSpan().Equals(namespaceName, StringComparison.Ordinal);
        if (!namespacesMatch)
        {
            _logger.LogInformation("Namespace name {namespaceNodeContent} does not match namespace name {namespaceName}.", namespaceNode.Content, namespaceName.ToString());
        }

        return namespacesMatch;
    }
}
