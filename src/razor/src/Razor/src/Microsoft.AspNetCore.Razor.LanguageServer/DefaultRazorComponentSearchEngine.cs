// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DefaultRazorComponentSearchEngine(
    IProjectSnapshotManager projectManager,
    ILoggerFactory loggerFactory)
    : RazorComponentSearchEngine
{
    private readonly IProjectSnapshotManager _projectManager = projectManager;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<DefaultRazorComponentSearchEngine>();

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
            _logger.LogWarning($"Could not split namespace and type for name {tagHelper.Name}.");
            return null;
        }

        var lookupSymbolName = RemoveGenericContent(typeName.AsMemory());

        var projects = _projectManager.GetProjects();

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
                if (!documentSnapshot.IsPathCandidateForComponent(lookupSymbolName))
                {
                    continue;
                }

                var razorCodeDocument = await documentSnapshot.GetGeneratedOutputAsync().ConfigureAwait(false);
                if (razorCodeDocument is null)
                {
                    continue;
                }

                // Make sure we have the right namespace of the fully qualified name
                if (!razorCodeDocument.ComponentNamespaceMatches(namespaceName))
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
}
