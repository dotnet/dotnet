// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Test;
using Microsoft.AspNetCore.Razor.Test.Common.Mef;
using Microsoft.AspNetCore.Razor.Test.Common.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;
using Microsoft.CodeAnalysis.Remote.Razor;
using Microsoft.VisualStudioCode.RazorExtension.Configuration;
using Microsoft.VisualStudioCode.RazorExtension.Services;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public abstract class CohostEndpointTestBase(ITestOutputHelper testOutputHelper) : CohostTestBase(testOutputHelper)
{
    private IClientSettingsManager? _clientSettingsManager;
    private VSCodeRemoteServiceInvoker? _remoteServiceInvoker;
    private IFilePathService? _filePathService;
    private ISemanticTokensLegendService? _semanticTokensLegendService;
    private Workspace? _workspace;

    private protected override IRemoteServiceInvoker RemoteServiceInvoker => _remoteServiceInvoker.AssumeNotNull();
    private protected override IClientSettingsManager ClientSettingsManager => _clientSettingsManager.AssumeNotNull();
    private protected override IFilePathService FilePathService => _filePathService.AssumeNotNull();
    private protected ISemanticTokensLegendService SemanticTokensLegendService => _semanticTokensLegendService.AssumeNotNull();
    private protected Workspace Workspace => _workspace.AssumeNotNull();

    protected override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        InProcServiceFactory.TestAccessor.SetExportProvider(OOPExportProvider);

        _workspace = CreateWorkspace();

        var workspaceProvider = new VSCodeWorkspaceProvider();
        workspaceProvider.SetWorkspace(Workspace);

        _remoteServiceInvoker = new VSCodeRemoteServiceInvoker(workspaceProvider, LoggerFactory);
        AddDisposable(_remoteServiceInvoker);

        _clientSettingsManager = new ClientSettingsManager();

        _filePathService = new VSCodeFilePathService(FeatureOptions);

        _semanticTokensLegendService = new CohostSemanticTokensLegendService(new TestClientCapabilitiesService(new VSInternalClientCapabilities() { SupportsVisualStudioExtensions = false }));
    }

    private protected override RemoteClientLSPInitializationOptions GetRemoteClientLSPInitializationOptions()
    {
        return new()
        {
            ClientCapabilities = new ClientCapabilities()
            {
                TextDocument = new TextDocumentClientCapabilities
                {
                    Completion = new CompletionSetting
                    {
                        CompletionItem = new CompletionItemSetting(),
                        CompletionItemKind = new CompletionItemKindSetting()
                        {
                            ValueSet = (CompletionItemKind[])Enum.GetValues(typeof(CompletionItemKind)),
                        },
                        CompletionListSetting = new CompletionListSetting()
                        {
                            ItemDefaults = ["commitCharacters", "editRange", "insertTextFormat", "data"]
                        },
                        ContextSupport = false,
                        InsertTextMode = InsertTextMode.AsIs,
                    }
                }
            }.ToVSInternalClientCapabilities(),
            TokenModifiers = [],
            TokenTypes = []
        };
    }

    protected override TextDocument CreateProjectAndRazorDocument(
        string contents,
        RazorFileKind? fileKind = null,
        string? documentFilePath = null,
        (string fileName, string contents)[]? additionalFiles = null,
        bool inGlobalNamespace = false,
        bool miscellaneousFile = false)
    {
        return CreateProjectAndRazorDocument(Workspace, contents, fileKind, documentFilePath, additionalFiles, inGlobalNamespace, miscellaneousFile);
    }

    private AdhocWorkspace CreateWorkspace()
    {
        var composition = TestComposition.RoslynFeatures;

        // We can't enforce that the composition is entirely valid, because we don't have a full MEF catalog, but we
        // can assume there should be no errors related to Razor, and having this array makes debugging failures a lot
        // easier.
        var errors = composition.GetCompositionErrors().ToArray();
        Assert.Empty(errors.Where(e => e.Contains("Razor")));

        var roslynExportProvider = composition.ExportProviderFactory.CreateExportProvider();
        AddDisposable(roslynExportProvider);
        var workspace = TestWorkspace.CreateWithDiagnosticAnalyzers(roslynExportProvider);
        AddDisposable(workspace);
        return workspace;
    }
}
