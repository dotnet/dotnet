// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.Razor.Remote;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;
using Microsoft.CodeAnalysis.Remote.Razor;
using Microsoft.VisualStudio.Composition;
using Microsoft.VisualStudioCode.RazorExtension.Configuration;
using Microsoft.VisualStudioCode.RazorExtension.Services;
using Xunit.Abstractions;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

public abstract class CohostEndpointTestBase(ITestOutputHelper testOutputHelper) : CohostTestBase(testOutputHelper)
{
    private IClientSettingsManager? _clientSettingsManager;
    private VSCodeRemoteServiceInvoker? _remoteServiceInvoker;

    private protected override IRemoteServiceInvoker RemoteServiceInvoker => _remoteServiceInvoker.AssumeNotNull();

    private protected override IClientSettingsManager ClientSettingsManager => _clientSettingsManager.AssumeNotNull();

    /// <summary>
    /// The export provider for Roslyn "devenv" services, if tests opt-in to using them
    /// </summary>
    private protected ExportProvider? RoslynDevenvExportProvider { get; private set; }

    protected override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        InProcServiceFactory.TestAccessor.SetExportProvider(OOPExportProvider);

        var workspaceProvider = new VSCodeWorkspaceProvider();
        var remoteWorkspace = RemoteWorkspaceProvider.Instance.GetWorkspace();
        workspaceProvider.SetWorkspace(remoteWorkspace);

        _remoteServiceInvoker = new VSCodeRemoteServiceInvoker(workspaceProvider, LoggerFactory);
        AddDisposable(_remoteServiceInvoker);

        _clientSettingsManager = new ClientSettingsManager();
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
}
