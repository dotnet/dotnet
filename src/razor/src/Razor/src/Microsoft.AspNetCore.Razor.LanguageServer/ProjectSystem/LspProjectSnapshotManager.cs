// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal class LspProjectSnapshotManager(
    IProjectEngineFactoryProvider projectEngineFactoryProvider,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    ILoggerFactory loggerFactory)
    : ProjectSnapshotManager(projectEngineFactoryProvider, languageServerFeatureOptions.ToCompilerOptions(), loggerFactory, initializer: AddMiscFilesProject)
{
    private static void AddMiscFilesProject(Updater updater)
    {
        updater.AddProject(MiscFilesProject.HostProject);
    }
}
