// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.ExternalAccess.Razor.Api;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal static class RemoteWorkspaceAccessor
{
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
    public static Workspace GetWorkspace()
        => RazorBrokeredServiceImplementation.GetWorkspace();
}
