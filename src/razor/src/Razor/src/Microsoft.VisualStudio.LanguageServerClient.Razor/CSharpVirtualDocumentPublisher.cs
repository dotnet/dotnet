// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using CodeAnalysisWorkspace = Microsoft.CodeAnalysis.Workspace;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

[Export(typeof(LSPDocumentChangeListener))]
[ContentType(RazorConstants.RazorLSPContentTypeName)]
internal class CSharpVirtualDocumentPublisher : LSPDocumentChangeListener
{
    private readonly RazorDynamicFileInfoProvider _dynamicFileInfoProvider;
    private readonly LSPDocumentMappingProvider _lspDocumentMappingProvider;

    [ImportingConstructor]
    public CSharpVirtualDocumentPublisher(RazorDynamicFileInfoProvider dynamicFileInfoProvider, LSPDocumentMappingProvider lspDocumentMappingProvider)
    {
        if (dynamicFileInfoProvider is null)
        {
            throw new ArgumentNullException(nameof(dynamicFileInfoProvider));
        }

        if (lspDocumentMappingProvider is null)
        {
            throw new ArgumentNullException(nameof(lspDocumentMappingProvider));
        }

        _dynamicFileInfoProvider = dynamicFileInfoProvider;
        _lspDocumentMappingProvider = lspDocumentMappingProvider;
    }

    // Internal for testing
    public override void Changed(LSPDocumentSnapshot? old, LSPDocumentSnapshot? @new, VirtualDocumentSnapshot? virtualOld, VirtualDocumentSnapshot? virtualNew, LSPDocumentChangeKind kind)
    {
        // We need the below check to address a race condition between when a request is sent to the C# server
        // for a generated document and when the C# server receives a document/didOpen notification. This race
        // condition may occur when the Razor server finishes initializing before C# receives and processes the
        // document open request.
        // This workaround adds the Razor client name to the generated document so the C# server will recognize
        // it, despite the document not being formally opened. Note this is meant to only be a temporary
        // workaround until a longer-term solution is implemented in the future.
        if (kind == LSPDocumentChangeKind.Added && _dynamicFileInfoProvider is DefaultRazorDynamicFileInfoProvider defaultProvider)
        {
            defaultProvider.PromoteBackgroundDocument(@new!.Uri, CSharpDocumentPropertiesService.Instance);
        }

        if (kind != LSPDocumentChangeKind.VirtualDocumentChanged)
        {
            return;
        }

        if (virtualNew is CSharpVirtualDocumentSnapshot)
        {
            var csharpContainer = new CSharpVirtualDocumentContainer(_lspDocumentMappingProvider, @new!, virtualNew.Snapshot);
            _dynamicFileInfoProvider.UpdateLSPFileInfo(@new!.Uri, csharpContainer);
        }
    }

    private class CSharpVirtualDocumentContainer : DynamicDocumentContainer
    {
        private readonly ITextSnapshot _textSnapshot;
        private readonly LSPDocumentMappingProvider _lspDocumentMappingProvider;
        private readonly LSPDocumentSnapshot _documentSnapshot;
        private IRazorSpanMappingService? _mappingService;
        private IRazorDocumentExcerptServiceImplementation? _excerptService;

        public override string FilePath => _documentSnapshot.Uri.LocalPath;

        public override bool SupportsDiagnostics => true;

        public CSharpVirtualDocumentContainer(LSPDocumentMappingProvider lspDocumentMappingProvider, LSPDocumentSnapshot documentSnapshot, ITextSnapshot textSnapshot)
        {
            if (lspDocumentMappingProvider is null)
            {
                throw new ArgumentNullException(nameof(lspDocumentMappingProvider));
            }

            if (textSnapshot is null)
            {
                throw new ArgumentNullException(nameof(textSnapshot));
            }

            if (documentSnapshot is null)
            {
                throw new ArgumentNullException(nameof(documentSnapshot));
            }

            _lspDocumentMappingProvider = lspDocumentMappingProvider;

            _textSnapshot = textSnapshot;
            _documentSnapshot = documentSnapshot;
        }

        public override IRazorDocumentExcerptServiceImplementation GetExcerptService()
        {
            if (_excerptService is null)
            {
                var mappingService = GetMappingService();
                _excerptService = new CSharpDocumentExcerptService(mappingService, _documentSnapshot);
            }

            return _excerptService;
        }

        public override IRazorSpanMappingService GetMappingService()
        {
            if (_mappingService is null)
            {
                _mappingService = new RazorLSPSpanMappingService(_lspDocumentMappingProvider, _documentSnapshot, _textSnapshot);
            }

            return _mappingService;
        }

        public override IRazorDocumentPropertiesService GetDocumentPropertiesService()
        {
            return CSharpDocumentPropertiesService.Instance;
        }

        public override TextLoader GetTextLoader(string filePath)
        {
            var sourceText = _textSnapshot.AsText();
            var textLoader = new SourceTextLoader(sourceText, filePath);
            return textLoader;
        }

        private sealed class SourceTextLoader : TextLoader
        {
            private readonly SourceText _sourceText;
            private readonly string _filePath;

            public SourceTextLoader(SourceText sourceText, string filePath)
            {
                if (sourceText is null)
                {
                    throw new ArgumentNullException(nameof(sourceText));
                }

                if (filePath is null)
                {
                    throw new ArgumentNullException(nameof(filePath));
                }

                _sourceText = sourceText;
                _filePath = filePath;
            }

            public override Task<TextAndVersion> LoadTextAndVersionAsync(CodeAnalysisWorkspace? workspace, DocumentId? documentId, CancellationToken cancellationToken)
            {
                return Task.FromResult(TextAndVersion.Create(_sourceText, VersionStamp.Default, _filePath));
            }
        }
    }
}
