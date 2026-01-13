// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.ProjectSystem;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;

public abstract class LanguageServerTestBase(ITestOutputHelper testOutput) : ToolingTestBase(testOutput)
{
    private protected IRazorMappingService SpanMappingService { get; } = new ThrowingRazorMappingService();
    private protected IFilePathService FilePathService { get; } = new LSPFilePathService(TestLanguageServerFeatureOptions.Instance);
    private protected JsonSerializerOptions SerializerOptions { get; } = JsonHelpers.JsonSerializerOptions;

    private protected override TestProjectSnapshotManager CreateProjectSnapshotManager(
        IProjectEngineFactoryProvider projectEngineFactoryProvider, LanguageServerFeatureOptions languageServerFeatureOptions)
        => new(
            projectEngineFactoryProvider,
            languageServerFeatureOptions,
            LoggerFactory,
            DisposalToken,
            initializer: static updater => updater.AddProject(MiscFilesProject.HostProject));

    private protected static RazorRequestContext CreateRazorRequestContext(
        DocumentContext? documentContext,
        LspServices? lspServices = null)
        => new(documentContext, lspServices ?? LspServices.Empty, "lsp/method", uri: null);

    protected static RazorCodeDocument CreateCodeDocument(
        string text,
        TagHelperCollection? tagHelpers = null,
        string? filePath = null,
        string? rootNamespace = null)
    {
        filePath ??= "test.cshtml";

        var fileKind = FileKinds.GetFileKindFromPath(filePath);
        tagHelpers ??= [];

        if (fileKind == RazorFileKind.Component)
        {
            tagHelpers = TagHelperCollection.Merge(tagHelpers, [.. RazorTestResources.BlazorServerAppTagHelpers]);
        }

        var sourceDocument = TestRazorSourceDocument.Create(text, filePath: filePath, relativePath: filePath);
        var projectEngine = RazorProjectEngine.Create(b =>
        {
            if (rootNamespace != null)
            {
                b.SetRootNamespace(rootNamespace);
            }

            b.ConfigureCodeGenerationOptions(builder =>
            {
                builder.UseEnhancedLinePragma = true;
            });

            RazorExtensions.Register(b);

            b.ConfigureParserOptions(builder =>
            {
                builder.UseRoslynTokenizer = true;
            });
        });

        var importDocumentName = fileKind == RazorFileKind.Legacy ? "_ViewImports.cshtml" : "_Imports.razor";
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

        return projectEngine.Process(sourceDocument, fileKind, [defaultImportDocument], tagHelpers);
    }

    private protected static IDocumentContextFactory CreateDocumentContextFactory(Uri documentPath, string sourceText)
    {
        var codeDocument = CreateCodeDocument(sourceText);
        return CreateDocumentContextFactory(documentPath, codeDocument);
    }

    private protected static DocumentContext CreateDocumentContext(Uri documentPath, RazorCodeDocument codeDocument)
    {
        return TestDocumentContext.Create(documentPath.GetAbsoluteOrUNCPath(), codeDocument);
    }

    private protected static IDocumentContextFactory CreateDocumentContextFactory(
        Uri documentPath,
        RazorCodeDocument codeDocument,
        bool documentFound = true)
    {
        var documentContextFactory = documentFound
            ? new TestDocumentContextFactory(documentPath.GetAbsoluteOrUNCPath(), codeDocument)
            : new TestDocumentContextFactory();

        return documentContextFactory;
    }

    private protected static DocumentContext CreateDocumentContext(Uri uri, IDocumentSnapshot snapshot)
    {
        return new DocumentContext(uri, snapshot, projectContext: null);
    }

    private protected static RazorLSPOptionsMonitor GetOptionsMonitor(
        bool enableFormatting = true,
        bool autoShowCompletion = true,
        bool autoListParams = true,
        bool formatOnType = true,
        bool autoInsertAttributeQuotes = true,
        bool colorBackground = false,
        bool codeBlockBraceOnNextLine = false,
        bool commitElementsWithSpace = true,
        bool formatOnPaste = true)
    {
        var configService = StrictMock.Of<IConfigurationSyncService>();

        var options = new RazorLSPOptions(
            GetFormattingFlags(enableFormatting, formatOnType, formatOnPaste),
            true,
            InsertSpaces: true,
            TabSize: 4,
            autoShowCompletion,
            autoListParams,
            autoInsertAttributeQuotes,
            colorBackground,
            codeBlockBraceOnNextLine,
            commitElementsWithSpace,
            TaskListDescriptors: []);
        var optionsMonitor = new RazorLSPOptionsMonitor(configService, options);
        return optionsMonitor;
    }

    private static FormattingFlags GetFormattingFlags(bool enableFormatting, bool formatOnType, bool formatOnPaste)
    {
        var flags = FormattingFlags.Disabled;

        if (enableFormatting)
        {
            flags |= FormattingFlags.Enabled;
        }

        if (formatOnType)
        {
            flags |= FormattingFlags.OnType;
        }

        if (formatOnPaste)
        {
            flags |= FormattingFlags.OnPaste;
        }

        return flags;
    }

    private class ThrowingRazorMappingService : IRazorMappingService
    {
        public Task<ImmutableArray<RazorMappedSpanResult>> MapSpansAsync(Document document, IEnumerable<TextSpan> spans, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ImmutableArray<RazorMappedEditResult>> MapTextChangesAsync(Document oldDocument, Document newDocument, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
