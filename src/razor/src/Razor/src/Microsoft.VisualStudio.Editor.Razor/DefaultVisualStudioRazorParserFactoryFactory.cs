// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Editor.Razor;

[Shared]
[ExportLanguageServiceFactory(typeof(VisualStudioRazorParserFactory), RazorLanguage.Name, ServiceLayer.Default)]
[method: ImportingConstructor]
internal class DefaultVisualStudioRazorParserFactoryFactory(
    JoinableTaskContext joinableTaskContext,
    IProjectEngineFactoryProvider projectEngineFactoryProvider,
    IErrorReporter errorReporter) : ILanguageServiceFactory
{
    public ILanguageService CreateLanguageService(HostLanguageServices languageServices)
    {
        var completionBroker = languageServices.GetRequiredService<VisualStudioCompletionBroker>();

        return new DefaultVisualStudioRazorParserFactory(
            joinableTaskContext,
            errorReporter,
            completionBroker,
            projectEngineFactoryProvider);
    }
}
