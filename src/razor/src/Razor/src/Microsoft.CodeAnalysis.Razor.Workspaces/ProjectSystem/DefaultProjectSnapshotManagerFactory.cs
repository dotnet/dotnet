// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Composition;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

[Shared]
[ExportLanguageServiceFactory(typeof(IProjectSnapshotManager), RazorLanguage.Name)]
[method: ImportingConstructor]
internal class DefaultProjectSnapshotManagerFactory(
    [ImportMany] IEnumerable<IProjectSnapshotChangeTrigger> triggers,
    IProjectEngineFactoryProvider projectEngineFactoryProvider,
    ProjectSnapshotManagerDispatcher dispatcher,
    IErrorReporter errorReporter) : ILanguageServiceFactory
{
    public ILanguageService CreateLanguageService(HostLanguageServices languageServices)
        => new DefaultProjectSnapshotManager(
            triggers,
            projectEngineFactoryProvider,
            dispatcher,
            errorReporter);
}
