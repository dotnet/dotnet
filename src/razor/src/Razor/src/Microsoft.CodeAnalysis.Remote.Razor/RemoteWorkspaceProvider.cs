// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Api;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteWorkspaceProvider : IWorkspaceProvider
{
    public static RemoteWorkspaceProvider Instance = new();

    /// <summary>
    /// Gets the remote workspace used in the Roslyn OOP process
    /// </summary>
    /// <remarks>
    /// Normally getting a workspace is possible from a document, project or solution snapshot but in the Roslyn OOP
    /// process that is explicitly denied via an exception. This method serves as a workaround when a workspace is
    /// needed (eg, the Go To Definition API requires one).
    ///
    /// This should be used sparingly and carefully, and no updates should be made to the workspace.
    /// </remarks>
    public Workspace GetWorkspace()
        => RazorBrokeredServiceImplementation.GetWorkspace();

    /// <summary>
    /// This is crap, but our EA can not have IVT to Roslyn's servicehub project, because it would cause a
    /// circular reference. This project does have IVT though, so we have to put this code here. Needless
    /// to say, this should only be called by tests.
    /// </summary>
    public static class TestAccessor
    {
        public async static Task<string?> InitializeRemoteExportProviderBuilderAsync(string localSettingsDirectory, CancellationToken cancellationToken)
        {
            return await RemoteExportProviderBuilder.InitializeAsync(localSettingsDirectory, cancellationToken).ConfigureAwait(false);
        }
    }
}
