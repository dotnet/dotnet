// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Composition;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

[Shared]
[ExportLanguageServiceFactory(typeof(ProjectSnapshotManager), RazorLanguage.Name)]
internal class DefaultProjectSnapshotManagerFactory : ILanguageServiceFactory
{
    private readonly IEnumerable<ProjectSnapshotChangeTrigger> _triggers;
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;

    [ImportingConstructor]
    public DefaultProjectSnapshotManagerFactory(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        [ImportMany] IEnumerable<ProjectSnapshotChangeTrigger> triggers)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (triggers is null)
        {
            throw new ArgumentNullException(nameof(triggers));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _triggers = triggers;
    }

    public ILanguageService CreateLanguageService(HostLanguageServices languageServices)
    {
        if (languageServices is null)
        {
            throw new ArgumentNullException(nameof(languageServices));
        }

        return new DefaultProjectSnapshotManager(
            _projectSnapshotManagerDispatcher,
            languageServices.WorkspaceServices.GetRequiredService<ErrorReporter>(),
            _triggers,
            languageServices.WorkspaceServices.Workspace);
    }
}
