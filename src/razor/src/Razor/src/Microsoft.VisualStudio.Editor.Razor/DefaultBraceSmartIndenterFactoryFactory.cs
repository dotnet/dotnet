// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Editor.Razor;

[Shared]
[ExportLanguageServiceFactory(typeof(BraceSmartIndenterFactory), RazorLanguage.Name, ServiceLayer.Default)]
internal class DefaultBraceSmartIndenterFactoryFactory : ILanguageServiceFactory
{
    private readonly JoinableTaskContext _joinableTaskContext;
    private readonly TextBufferCodeDocumentProvider _codeDocumentProvider;
    private readonly IEditorOperationsFactoryService _editorOperationsFactory;

    [ImportingConstructor]
    public DefaultBraceSmartIndenterFactoryFactory(
        JoinableTaskContext joinableTaskContext,
        TextBufferCodeDocumentProvider codeDocumentProvider,
        IEditorOperationsFactoryService editorOperationsFactory)
    {
        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        if (codeDocumentProvider is null)
        {
            throw new ArgumentNullException(nameof(codeDocumentProvider));
        }

        if (editorOperationsFactory is null)
        {
            throw new ArgumentNullException(nameof(editorOperationsFactory));
        }

        _joinableTaskContext = joinableTaskContext;
        _codeDocumentProvider = codeDocumentProvider;
        _editorOperationsFactory = editorOperationsFactory;
    }

    public ILanguageService CreateLanguageService(HostLanguageServices languageServices)
    {
        if (languageServices is null)
        {
            throw new ArgumentNullException(nameof(languageServices));
        }

        return new DefaultBraceSmartIndenterFactory(_joinableTaskContext, _codeDocumentProvider, _editorOperationsFactory);
    }
}
