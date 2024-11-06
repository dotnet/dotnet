// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal class LspProjectSnapshotManager(
    IProjectEngineFactoryProvider projectEngineFactoryProvider,
    ILoggerFactory loggerFactory)
    : ProjectSnapshotManager(projectEngineFactoryProvider, loggerFactory, initializer: AddMiscFilesProject)
{
    private static void AddMiscFilesProject(Updater updater)
    {
        updater.ProjectAdded(MiscFilesHostProject.Instance);
    }
}
