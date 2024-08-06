// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal class LspProjectSnapshotManager(
    IProjectEngineFactoryProvider projectEngineFactoryProvider,
    ILoggerFactory loggerFactory)
    : ProjectSnapshotManager(projectEngineFactoryProvider, loggerFactory, initializer: AddMiscFilesProject), IProjectCollectionResolver
{
    private static void AddMiscFilesProject(Updater updater)
    {
        updater.ProjectAdded(MiscFilesHostProject.Instance);
    }

    public IEnumerable<IProjectSnapshot> EnumerateProjects(IDocumentSnapshot snapshot)
    {
        return GetProjects();
    }
}
