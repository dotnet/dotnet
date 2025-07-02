// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;

internal class LspProjectSnapshotManager(
    IProjectEngineFactoryProvider projectEngineFactoryProvider,
    LanguageServerFeatureOptions languageServerFeatureOptions,
    ILoggerFactory loggerFactory)
    : ProjectSnapshotManager(projectEngineFactoryProvider, languageServerFeatureOptions.ToCompilerOptions(), languageServerFeatureOptions, loggerFactory, initializer: AddMiscFilesProject)
{
    private static void AddMiscFilesProject(Updater updater)
    {
        updater.AddProject(MiscFilesProject.HostProject);
    }
}
