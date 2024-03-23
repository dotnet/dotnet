// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Razor.DynamicFiles;
using StreamJsonRpc;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal partial class RazorCustomMessageTarget
{
    [JsonRpcMethod(CustomMessageNames.RazorProjectContextsEndpoint, UseSingleObjectParameterDeserialization = true)]
    public Task<VSProjectContextList?> ProjectContextsAsync(DelegatedProjectContextsParams request, CancellationToken _)
    {
        // Previously we would have asked Roslyn for their ProjectContexts, so we can make sure we pass a ProjectContext they understand
        // to them when we ask them for things. When we generate unique file names for generated files, we no longer need to do that
        // as the generated file will only be in one project, so we can just use our own ProjectContexts. This makes other things much
        // easier because we're not trying to understand Roslyn concepts.

        var projects = _projectManager.GetProjects();

        using var projectContexts = new PooledArrayBuilder<VSProjectContext>(capacity: projects.Length);

        var documentFilePath = RazorDynamicFileInfoProvider.GetProjectSystemFilePath(request.Uri);

        foreach (var project in projects)
        {
            if (project is ProjectSnapshot snapshot &&
                project.GetDocument(documentFilePath) is not null)
            {
                projectContexts.Add(new VSProjectContext
                {
                    Id = project.Key.Id,
                    Kind = VSProjectKind.CSharp,
                    Label = snapshot.HostProject.DisplayName
                });
            }
        }

        if (projectContexts.Count == 0)
        {
            return Task.FromResult<VSProjectContextList?>(null);
        }

        var result = new VSProjectContextList
        {
            DefaultIndex = 0,
            ProjectContexts = projectContexts.ToArray(),
        };
        return Task.FromResult<VSProjectContextList?>(result);
    }
}
