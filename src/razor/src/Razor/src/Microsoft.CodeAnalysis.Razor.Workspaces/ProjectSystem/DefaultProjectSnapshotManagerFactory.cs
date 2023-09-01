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
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly IEnumerable<IProjectSnapshotChangeTrigger> _triggers;

    [ImportingConstructor]
    public DefaultProjectSnapshotManagerFactory(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        [ImportMany] IEnumerable<IProjectSnapshotChangeTrigger> triggers)
    {
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher ?? throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        _triggers = triggers ?? throw new ArgumentNullException(nameof(triggers));
    }

    public ILanguageService CreateLanguageService(HostLanguageServices languageServices)
    {
        if (languageServices is null)
        {
            throw new ArgumentNullException(nameof(languageServices));
        }

        return new DefaultProjectSnapshotManager(
            languageServices.WorkspaceServices.GetRequiredService<IErrorReporter>(),
            _triggers,
            languageServices.WorkspaceServices.Workspace,
            _projectSnapshotManagerDispatcher);
    }
}
