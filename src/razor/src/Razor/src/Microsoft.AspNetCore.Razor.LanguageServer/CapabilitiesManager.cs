// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.RpcContracts.Settings;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class CapabilitiesManager : IInitializeManager<InitializeParams, InitializeResult>, IClientCapabilitiesService, IWorkspaceRootPathProvider
{
    private readonly ILspServices _lspServices;
    private readonly TaskCompletionSource<InitializeParams> _initializeParamsTaskSource;
    private readonly VisualStudio.Threading.AsyncLazy<string> _lazyRootPath;

    public bool HasInitialized => _initializeParamsTaskSource.Task.IsCompleted;

    public bool CanGetClientCapabilities => HasInitialized;

    public VSInternalClientCapabilities ClientCapabilities => GetInitializeParams().Capabilities.ToVSInternalClientCapabilities();

    public CapabilitiesManager(ILspServices lspServices)
    {
        _lspServices = lspServices;

        _initializeParamsTaskSource = new();

#pragma warning disable VSTHRD012 // Provide JoinableTaskFactory where allowed
        _lazyRootPath = new(ComputeRootPathAsync);
#pragma warning restore VSTHRD012
    }

    public InitializeParams GetInitializeParams()
    {
        return _initializeParamsTaskSource.Task.VerifyCompleted();
    }

    public InitializeResult GetInitializeResult()
    {
        var initializeParams = GetInitializeParams();
        var clientCapabilities = initializeParams.Capabilities;
        var vsClientCapabilities = clientCapabilities.ToVSInternalClientCapabilities();

        var serverCapabilities = new VSInternalServerCapabilities();

        var capabilitiesProviders = _lspServices.GetRequiredServices<ICapabilitiesProvider>();
        foreach (var provider in capabilitiesProviders)
        {
            provider.ApplyCapabilities(serverCapabilities, vsClientCapabilities);
        }

        return new InitializeResult
        {
            Capabilities = serverCapabilities,
        };
    }

    public void SetInitializeParams(InitializeParams request)
    {
        if (_initializeParamsTaskSource.Task.IsCompleted)
        {
            throw new InvalidOperationException($"{nameof(SetInitializeParams)} already called.");
        }

        _initializeParamsTaskSource.TrySetResult(request);
    }

    private async Task<string> ComputeRootPathAsync()
    {
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
        var initializeParams = await _initializeParamsTaskSource.Task.ConfigureAwaitRunInline();
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks

        if (initializeParams.RootUri is Uri rootUri)
        {
            return rootUri.GetAbsoluteOrUNCPath();
        }

        // RootUri was added in LSP3, fall back to RootPath

#pragma warning disable CS0618 // Type or member is obsolete
        return initializeParams.RootPath.AssumeNotNull();
#pragma warning restore CS0618 // Type or member is obsolete
    }

    public Task<string> GetRootPathAsync(CancellationToken cancellationToken)
        => _lazyRootPath.GetValueAsync(cancellationToken);
}
