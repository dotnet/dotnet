// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Moq;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;

public abstract class LanguageServerTestBase : ToolingTestBase
{
    private protected IRazorSpanMappingService SpanMappingService { get; }
    private protected IFilePathService FilePathService { get; }

    protected JsonSerializer Serializer { get; }

    protected LanguageServerTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        SpanMappingService = new ThrowingRazorSpanMappingService();

        Serializer = new JsonSerializer();
        Serializer.AddVSInternalExtensionConverters();
        Serializer.AddVSExtensionConverters();

        FilePathService = new LSPFilePathService(TestLanguageServerFeatureOptions.Instance);
    }

    private protected override ProjectSnapshotManagerDispatcher CreateDispatcher()
    {
        var dispatcher = new LSPProjectSnapshotManagerDispatcher(ErrorReporter);
        AddDisposable(dispatcher);

        return dispatcher;
    }
    private protected TestProjectSnapshotManager CreateProjectSnapshotManager()
        => CreateProjectSnapshotManager(ProjectEngineFactories.DefaultProvider);

    private protected TestProjectSnapshotManager CreateProjectSnapshotManager(IProjectEngineFactoryProvider projectEngineFactoryProvider)
        => new(projectEngineFactoryProvider, Dispatcher, DisposalToken);

    internal RazorRequestContext CreateRazorRequestContext(VersionedDocumentContext? documentContext, ILspServices? lspServices = null)
    {
        lspServices ??= new Mock<ILspServices>(MockBehavior.Strict).Object;

        var requestContext = new RazorRequestContext(documentContext, lspServices, "lsp/method", uri: null);

        return requestContext;
    }

    protected static RazorCodeDocument CreateCodeDocument(string text, ImmutableArray<TagHelperDescriptor> tagHelpers = default, string? filePath = null)
    {
        filePath ??= "test.cshtml";

        var fileKind = FileKinds.GetFileKindFromFilePath(filePath);
        tagHelpers = tagHelpers.NullToEmpty();

        if (fileKind == FileKinds.Component)
        {
            tagHelpers = tagHelpers.AddRange(RazorTestResources.BlazorServerAppTagHelpers);
        }

        var sourceDocument = TestRazorSourceDocument.Create(text, filePath: filePath, relativePath: filePath);
        var projectEngine = RazorProjectEngine.Create(RazorExtensions.Register);
        var importDocumentName = fileKind == FileKinds.Legacy ? "_ViewImports.cshtml" : "_Imports.razor";
        var defaultImportDocument = TestRazorSourceDocument.Create(
            """
                @using BlazorApp1
                @using BlazorApp1.Pages
                @using BlazorApp1.Shared
                @using System;
                @using Microsoft.AspNetCore.Components
                @using Microsoft.AspNetCore.Components.Authorization
                @using Microsoft.AspNetCore.Components.Forms
                @using Microsoft.AspNetCore.Components.Routing
                @using Microsoft.AspNetCore.Components.Web
                """,
            RazorSourceDocumentProperties.Create(importDocumentName, importDocumentName));
        var codeDocument = projectEngine.ProcessDesignTime(sourceDocument, fileKind, ImmutableArray.Create(defaultImportDocument), tagHelpers);
        return codeDocument;
    }

    internal static IDocumentContextFactory CreateDocumentContextFactory(Uri documentPath, string sourceText)
    {
        var codeDocument = CreateCodeDocument(sourceText);
        return CreateDocumentContextFactory(documentPath, codeDocument);
    }

    internal static VersionedDocumentContext CreateDocumentContext(Uri documentPath, RazorCodeDocument codeDocument)
    {
        return TestDocumentContext.From(documentPath.GetAbsoluteOrUNCPath(), codeDocument, hostDocumentVersion: 1337);
    }

    internal static IDocumentContextFactory CreateDocumentContextFactory(
        Uri documentPath,
        RazorCodeDocument codeDocument,
        bool documentFound = true)
    {
        var documentContextFactory = documentFound
            ? new TestDocumentContextFactory(documentPath.GetAbsoluteOrUNCPath(), codeDocument, version: 1337)
            : new TestDocumentContextFactory();

        return documentContextFactory;
    }

    internal static VersionedDocumentContext CreateDocumentContext(Uri uri, IDocumentSnapshot snapshot)
    {
        return new VersionedDocumentContext(uri, snapshot, projectContext: null, version: 0);
    }

    internal static IOptionsMonitor<RazorLSPOptions> GetOptionsMonitor(bool enableFormatting = true, bool autoShowCompletion = true, bool autoListParams = true, bool formatOnType = true, bool autoInsertAttributeQuotes = true, bool colorBackground = false, bool codeBlockBraceOnNextLine = false, bool commitElementsWithSpace = true)
    {
        var monitor = new Mock<IOptionsMonitor<RazorLSPOptions>>(MockBehavior.Strict);
        monitor.SetupGet(m => m.CurrentValue).Returns(new RazorLSPOptions(enableFormatting, true, InsertSpaces: true, TabSize: 4, autoShowCompletion, autoListParams, formatOnType, autoInsertAttributeQuotes, colorBackground, codeBlockBraceOnNextLine, commitElementsWithSpace));
        return monitor.Object;
    }

    private class ThrowingRazorSpanMappingService : IRazorSpanMappingService
    {
        public Task<ImmutableArray<RazorMappedSpanResult>> MapSpansAsync(Document document, IEnumerable<TextSpan> spans, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
