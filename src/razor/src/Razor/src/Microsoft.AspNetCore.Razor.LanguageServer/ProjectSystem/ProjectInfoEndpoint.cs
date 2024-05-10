// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Serialization;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

/// <summary>
/// Used to receive project system info updates from the client that were discovered OOB.
/// </summary>
[RazorLanguageServerEndpoint(LanguageServerConstants.RazorProjectInfoEndpoint)]
internal class ProjectInfoEndpoint(
    ProjectConfigurationStateManager stateManager,
    IRazorProjectInfoFileSerializer serializer)
    : IRazorNotificationHandler<ProjectInfoParams>
{
    private readonly ProjectConfigurationStateManager _stateManager = stateManager;
    private readonly IRazorProjectInfoFileSerializer _serializer = serializer;

    public bool MutatesSolutionState => false;

    public async Task HandleNotificationAsync(ProjectInfoParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var count = request.ProjectKeyIds.Length;

        for (var i = 0; i < count; i++)
        {
            var projectKey = new ProjectKey(request.ProjectKeyIds[i]);

            RazorProjectInfo? projectInfo = null;

            if (request.FilePaths[i] is string filePath)
            {
                projectInfo = await _serializer.DeserializeFromFileAndDeleteAsync(filePath, cancellationToken).ConfigureAwait(false);
            }

            await _stateManager.ProjectInfoUpdatedAsync(projectKey, projectInfo, cancellationToken).ConfigureAwait(false);
        }
    }
}
