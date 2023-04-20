// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Project;

[Shared]
[ExportWorkspaceServiceFactory(typeof(ProjectSnapshotProjectEngineFactory))]
internal class ProjectSnapshotProjectEngineFactoryFactory : IWorkspaceServiceFactory
{
    public IWorkspaceService CreateService(HostWorkspaceServices workspaceServices)
    {
        if (workspaceServices is null)
        {
            throw new ArgumentNullException(nameof(workspaceServices));
        }

        return new DefaultProjectSnapshotProjectEngineFactory(new FallbackProjectEngineFactory(), MefProjectEngineFactories.Factories);
    }
}
