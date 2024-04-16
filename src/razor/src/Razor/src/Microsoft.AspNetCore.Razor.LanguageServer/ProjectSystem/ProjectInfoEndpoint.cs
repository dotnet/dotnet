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
using Microsoft.CodeAnalysis.Razor.Workspaces.Protocol.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

/// <summary>
/// Used to receive project system info updates from the client that were discovered OOB.
/// </summary>
[RazorLanguageServerEndpoint(LanguageServerConstants.RazorProjectInfoEndpoint)]
internal class ProjectInfoEndpoint : IRazorNotificationHandler<ProjectInfoParams>
{
    private readonly ProjectConfigurationStateManager _projectConfigurationStateManager;

    public ProjectInfoEndpoint(ProjectConfigurationStateManager projectConfigurationStateManager)
    {
        _projectConfigurationStateManager = projectConfigurationStateManager;
    }

    public bool MutatesSolutionState => false;

    public Task HandleNotificationAsync(ProjectInfoParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        RazorProjectInfo? razorProjectInfo = null;

        // ProjectInfo will be null if project is being deleted and should be removed
        if (request.ProjectInfo is string projectInfoBase64)
        {
            var projectInfoBytes = Convert.FromBase64String(projectInfoBase64);
            using var stream = new MemoryStream(projectInfoBytes);
            razorProjectInfo = RazorProjectInfoDeserializer.Instance.DeserializeFromStream(stream);
        }

        var projectKey = ProjectKey.FromString(request.ProjectKeyId);

        return _projectConfigurationStateManager.ProjectInfoUpdatedAsync(projectKey, razorProjectInfo, cancellationToken);
    }
}
