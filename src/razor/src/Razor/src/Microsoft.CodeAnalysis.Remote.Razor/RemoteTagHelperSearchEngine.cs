// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Remote.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Remote.Razor;

[Export(typeof(ITagHelperSearchEngine)), Shared]
internal sealed class RemoteTagHelperSearchEngine : ITagHelperSearchEngine
{
    public async Task<LspLocation?> TryLocateTagHelperDefinitionAsync(TagHelperDescriptor boundTagHelper, BoundAttributeDescriptor? boundAttribute, IDocumentSnapshot documentSnapshot, ISolutionQueryOperations solutionQueryOperations, CancellationToken cancellationToken)
    {
        if (boundTagHelper.IsComponentTagHelper)
        {
            return null;
        }

        var typeName = boundTagHelper.GetTypeName();
        if (typeName is null)
        {
            return null;
        }

        Debug.Assert(documentSnapshot is RemoteDocumentSnapshot);

        var project = ((RemoteDocumentSnapshot)documentSnapshot).TextDocument.Project;
        var compilation = await project.GetCompilationAsync(cancellationToken).ConfigureAwait(false);
        if (compilation is null)
        {
            return null;
        }

        foreach (var type in compilation.GetTypesByMetadataName(typeName))
        {
            var locations = type.Locations;
            if (boundAttribute is { PropertyName: string propertyName } &&
                type.GetMembers(propertyName) is [{ } property])
            {
                locations = property.Locations;
            }

            foreach (var location in locations)
            {
                if (location.IsInSource &&
                    project.Solution.GetDocument(location.SourceTree) is { } document)
                {
                    var text = await document.GetTextAsync(cancellationToken).ConfigureAwait(false);
                    return new LspLocation
                    {
                        DocumentUri = document.CreateDocumentUri(),
                        Range = text.GetRange(location.SourceSpan)
                    };
                }
            }
        }

        return null;
    }
}
