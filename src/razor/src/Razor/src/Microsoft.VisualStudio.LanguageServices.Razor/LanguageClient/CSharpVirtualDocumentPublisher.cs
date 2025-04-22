// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Razor.DynamicFiles;
using Microsoft.VisualStudio.Razor.LanguageClient.DocumentMapping;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

[Export(typeof(LSPDocumentChangeListener))]
[ContentType(RazorConstants.RazorLSPContentTypeName)]
internal class CSharpVirtualDocumentPublisher : LSPDocumentChangeListener
{
    private readonly IRazorDynamicFileInfoProviderInternal _dynamicFileInfoProvider;
    private readonly LSPDocumentMappingProvider _lspDocumentMappingProvider;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;

    [ImportingConstructor]
    public CSharpVirtualDocumentPublisher(IRazorDynamicFileInfoProviderInternal dynamicFileInfoProvider, LSPDocumentMappingProvider lspDocumentMappingProvider, LanguageServerFeatureOptions languageServerFeatureOptions)
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
        _languageServerFeatureOptions = languageServerFeatureOptions;
    }

    // Internal for testing
    public override void Changed(LSPDocumentSnapshot? old, LSPDocumentSnapshot? @new, VirtualDocumentSnapshot? virtualOld, VirtualDocumentSnapshot? virtualNew, LSPDocumentChangeKind kind)
    {
        if (_languageServerFeatureOptions.UseRazorCohostServer)
        {
            return;
        }

        // We need the below check to address a race condition between when a request is sent to the C# server
        // for a generated document and when the C# server receives a document/didOpen notification. This race
        // condition may occur when the Razor server finishes initializing before C# receives and processes the
        // document open request.
        // This workaround adds the Razor client name to the generated document so the C# server will recognize
        // it, despite the document not being formally opened. Note this is meant to only be a temporary
        // workaround until a longer-term solution is implemented in the future.
        if (kind == LSPDocumentChangeKind.Added && _dynamicFileInfoProvider is RazorDynamicFileInfoProvider defaultProvider)
        {
            defaultProvider.PromoteBackgroundDocument(@new.AssumeNotNull().Uri, CSharpDocumentPropertiesService.Instance);
        }

        if (kind != LSPDocumentChangeKind.VirtualDocumentChanged)
        {
            return;
        }

        if (virtualNew is CSharpVirtualDocumentSnapshot)
        {
            Assumes.NotNull(@new);
            var csharpContainer = new CSharpVirtualDocumentContainer(_lspDocumentMappingProvider, @new, virtualNew.Snapshot);
            _dynamicFileInfoProvider.UpdateLSPFileInfo(@new.Uri, csharpContainer);
        }
    }

    private sealed class CSharpVirtualDocumentContainer(
        LSPDocumentMappingProvider lspDocumentMappingProvider,
        LSPDocumentSnapshot documentSnapshot,
        ITextSnapshot textSnapshot) : IDynamicDocumentContainer
    {
        private readonly LSPDocumentMappingProvider _lspDocumentMappingProvider = lspDocumentMappingProvider;
        private readonly LSPDocumentSnapshot _documentSnapshot = documentSnapshot;
        private readonly ITextSnapshot _textSnapshot = textSnapshot;

        private IRazorMappingService? _mappingService;
        private IRazorDocumentExcerptServiceImplementation? _excerptService;

        public string FilePath => _documentSnapshot.Uri.LocalPath;

        public bool SupportsDiagnostics => true;

        public void SetSupportsDiagnostics(bool value)
        {
            // This dynamic document container always supports diagnostics, so we don't allow disabling them.
        }

        public IRazorDocumentExcerptServiceImplementation GetExcerptService()
            => _excerptService ?? InterlockedOperations.Initialize(ref _excerptService,
                new CSharpDocumentExcerptService(GetMappingService(), _documentSnapshot));

        public IRazorDocumentPropertiesService GetDocumentPropertiesService()
            => CSharpDocumentPropertiesService.Instance;

        public TextLoader GetTextLoader(string filePath)
            => new SourceTextLoader(_textSnapshot.AsText(), filePath);

        public IRazorMappingService GetMappingService()
            => _mappingService ?? InterlockedOperations.Initialize(ref _mappingService,
                new RazorLSPMappingService(_lspDocumentMappingProvider, _documentSnapshot, _textSnapshot));

        private sealed class SourceTextLoader(SourceText sourceText, string filePath) : TextLoader
        {
            public override Task<TextAndVersion> LoadTextAndVersionAsync(LoadTextOptions options, CancellationToken cancellationToken)
                => Task.FromResult(TextAndVersion.Create(sourceText, VersionStamp.Default, filePath));
        }
    }
}
