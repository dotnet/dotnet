// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Logging;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

[Shared]
[ExportLspMethod(Methods.InitializeName)]
internal class InitializeHandler : IRequestHandler<InitializeParams, InitializeResult>
{
    private InitializeResult? _initializeResult;
    private readonly JoinableTaskFactory _joinableTaskFactory;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
    private readonly ILanguageClientBroker _languageClientBroker;
    private readonly ILanguageServiceBroker2 _languageServiceBroker;
    private readonly List<(ILanguageClient Client, VSInternalServerCapabilities Capabilities)> _serverCapabilities;
    private readonly JsonSerializer _serializer;
    private readonly ILogger _logger;

    [ImportingConstructor]
    public InitializeHandler(
        LanguageServerFeatureOptions languageServerFeatureOptions,
        JoinableTaskContext joinableTaskContext,
        ILanguageClientBroker languageClientBroker,
        ILanguageServiceBroker2 languageServiceBroker,
        HTMLCSharpLanguageServerLogHubLoggerProvider loggerProvider)
    {
        if (languageServerFeatureOptions is null)
        {
            throw new ArgumentNullException(nameof(languageServerFeatureOptions));
        }

        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        if (languageClientBroker is null)
        {
            throw new ArgumentNullException(nameof(languageClientBroker));
        }

        if (languageServiceBroker is null)
        {
            throw new ArgumentNullException(nameof(languageServiceBroker));
        }

        if (loggerProvider is null)
        {
            throw new ArgumentNullException(nameof(loggerProvider));
        }

        _languageServerFeatureOptions = languageServerFeatureOptions;
        _joinableTaskFactory = joinableTaskContext.Factory;
        _languageClientBroker = languageClientBroker;
        _languageServiceBroker = languageServiceBroker;

        _logger = loggerProvider.CreateLogger(nameof(InitializeHandler));

        _serverCapabilities = new List<(ILanguageClient, VSInternalServerCapabilities)>();

        _serializer = new JsonSerializer();
        _serializer.AddVSInternalExtensionConverters();
    }

    public Task<InitializeResult?> HandleRequestAsync(InitializeParams request, ClientCapabilities clientCapabilities, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Providing initialization configuration.");

        _initializeResult = new()
        {
            Capabilities = new VSInternalServerCapabilities
            {
                OnAutoInsertProvider = new VSInternalDocumentOnAutoInsertOptions()
                {
                    TriggerCharacters = new[] { "'", "/", "\n", "=" }
                },
                HoverProvider = true,
                DefinitionProvider = true,
                DocumentHighlightProvider = true,
                RenameProvider = true,
                ReferencesProvider = true,
                SignatureHelpProvider = new SignatureHelpOptions()
                {
                    TriggerCharacters = new[] { "(", ",", "<" },
                    RetriggerCharacters = new[] { ">", ")" }
                },
                ImplementationProvider = true,
                SupportsDiagnosticRequests = true,
            }
        };

        // If we're in single server mode, turn off some capabilities that we want to let the Razor server support
        if (_languageServerFeatureOptions.SingleServerSupport)
        {
            _initializeResult.Capabilities.RenameProvider = false;
            _initializeResult.Capabilities.HoverProvider = false;
            _initializeResult.Capabilities.DefinitionProvider = false;
            _initializeResult.Capabilities.DocumentHighlightProvider = false;
            _initializeResult.Capabilities.SignatureHelpProvider = null;
            _initializeResult.Capabilities.ImplementationProvider = false;
            _initializeResult.Capabilities.ReferencesProvider = false;

            ((VSInternalServerCapabilities)_initializeResult.Capabilities).OnAutoInsertProvider = null;
            ((VSInternalServerCapabilities)_initializeResult.Capabilities).SupportsDiagnosticRequests = false;
        }

        if (!_languageServerFeatureOptions.SingleServerCompletionSupport)
        {
            // Feature flag is not on, fallback to utilizing completion handler

            _initializeResult.Capabilities.CompletionProvider = new CompletionOptions()
            {
                AllCommitCharacters = new[] { " ", "{", "}", "[", "]", "(", ")", ".", ",", ":", ";", "+", "-", "*", "/", "%", "&", "|", "^", "!", "~", "=", "<", ">", "?", "@", "#", "'", "\"", "\\" },
                ResolveProvider = true,
                TriggerCharacters = CompletionHandler.AllTriggerCharacters.ToArray()
            };
        }

        VerifyMergedLanguageServerCapabilities();

        return Task.FromResult<InitializeResult?>(_initializeResult);
    }

    [Conditional("DEBUG")]
    private void VerifyMergedLanguageServerCapabilities()
    {
        _ = Task.Run(async () =>
        {
            var containedLanguageServerClients = await EnsureContainedLanguageServersInitializedAsync().ConfigureAwait(false);

            var mergedCapabilities = GetMergedServerCapabilities(containedLanguageServerClients);

            await VerifyMergedCompletionOptionsAsync(mergedCapabilities);

            await VerifyMergedHoverAsync(mergedCapabilities);

            await VerifyMergedOnAutoInsertAsync(mergedCapabilities);

            await VerifyMergedSignatureHelpOptionsAsync(mergedCapabilities);

            await VerifyMergedDefinitionProviderAsync(mergedCapabilities);

            await VerifyMergedReferencesProviderAsync(mergedCapabilities);

            await VerifyMergedRenameProviderAsync(mergedCapabilities);
        }).ConfigureAwait(false);
    }

    private VSInternalServerCapabilities GetMergedServerCapabilities(List<ILanguageClient> relevantLanguageClients)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        foreach (var languageClientInstance in _languageServiceBroker.ActiveLanguageClients)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            if (relevantLanguageClients.Contains(languageClientInstance.Client))
            {
                var resultToken = languageClientInstance.InitializeResult;
                var initializeResult = resultToken.ToObject<InitializeResult>(_serializer);
                if (initializeResult is null)
                {
                    throw new JsonSerializationException($"Failed to serialize to {nameof(InitializeResult)}");
                }

                _serverCapabilities.Add((languageClientInstance.Client, (VSInternalServerCapabilities)initializeResult.Capabilities));
            }
        }

        var serverCapabilities = new VSInternalServerCapabilities
        {
            CompletionProvider = GetMergedCompletionOptions(),
            TextDocumentSync = GetMergedTextDocumentSyncOptions(),
            HoverProvider = GetMergedHoverProvider(),
            OnAutoInsertProvider = GetMergedVSInternalDocumentOnAutoInsertOptions(),
            SignatureHelpProvider = GetMergedSignatureHelpOptions(),
            DefinitionProvider = GetMergedDefinitionProvider(),
            ReferencesProvider = GetMergedReferencesProvider(),
            RenameProvider = GetMergedRenameProvider(),
            DocumentOnTypeFormattingProvider = GetMergedOnTypeFormattingProvider(),
        };

        return serverCapabilities;
    }

    private DocumentOnTypeFormattingOptions GetMergedOnTypeFormattingProvider()
    {
        var documentOnTypeFormattingProviderOptions = _serverCapabilities.Where(s => s.Capabilities.DocumentOnTypeFormattingProvider != null).Select(s => s.Capabilities.DocumentOnTypeFormattingProvider!);
        var triggerChars = new HashSet<string>();

        foreach (var options in documentOnTypeFormattingProviderOptions)
        {
            if (options.FirstTriggerCharacter != null)
            {
                triggerChars.Add(options.FirstTriggerCharacter);
            }

            if (options.MoreTriggerCharacter != null)
            {
                triggerChars.UnionWith(options.MoreTriggerCharacter);
            }
        }

        return new DocumentOnTypeFormattingOptions()
        {
            MoreTriggerCharacter = triggerChars.ToArray(),
        };
    }

    private bool GetMergedHoverProvider()
    {
        return _serverCapabilities.Any(s => s.Capabilities.HoverProvider?.Value is bool isHoverSupported && isHoverSupported);
    }

    private VSInternalDocumentOnAutoInsertOptions GetMergedVSInternalDocumentOnAutoInsertOptions()
    {
        var allVSInternalDocumentOnAutoInsertOptions = _serverCapabilities.Where(s => s.Capabilities.OnAutoInsertProvider != null).Select(s => s.Capabilities.OnAutoInsertProvider!);
        var triggerChars = new HashSet<string>();

        foreach (var documentOnAutoInsertOptions in allVSInternalDocumentOnAutoInsertOptions)
        {
            if (documentOnAutoInsertOptions.TriggerCharacters != null)
            {
                triggerChars.UnionWith(documentOnAutoInsertOptions.TriggerCharacters);
            }
        }

        return new VSInternalDocumentOnAutoInsertOptions()
        {
            TriggerCharacters = triggerChars.ToArray(),
        };
    }

    private TextDocumentSyncOptions GetMergedTextDocumentSyncOptions()
    {
        var allTextDocumentSyncOptions = _serverCapabilities.Where(s => s.Capabilities.TextDocumentSync != null).Select(s => s.Capabilities.TextDocumentSync!);

        var openClose = false;

        foreach (var curTextDocumentSyncOptions in allTextDocumentSyncOptions)
        {
            openClose |= curTextDocumentSyncOptions.OpenClose;
        }

        var textDocumentSyncOptions = new TextDocumentSyncOptions()
        {
            OpenClose = openClose,
            Change = TextDocumentSyncKind.Incremental,
        };

        return textDocumentSyncOptions;
    }

    private CompletionOptions GetMergedCompletionOptions()
    {
        var allCompletionOptions = _serverCapabilities.Where(s => s.Capabilities.CompletionProvider != null).Select(s => s.Capabilities.CompletionProvider!);

        var commitChars = new HashSet<string>();
        var triggerChars = new HashSet<string>();
        var resolveProvider = false;

        foreach (var curCompletionOptions in allCompletionOptions)
        {
            if (curCompletionOptions.AllCommitCharacters != null)
            {
                commitChars.UnionWith(curCompletionOptions.AllCommitCharacters);
            }

            if (curCompletionOptions.TriggerCharacters != null)
            {
                triggerChars.UnionWith(curCompletionOptions.TriggerCharacters);
            }

            resolveProvider |= curCompletionOptions.ResolveProvider;
        }

        var completionOptions = new CompletionOptions()
        {
            AllCommitCharacters = commitChars.ToArray(),
            ResolveProvider = resolveProvider,
            TriggerCharacters = triggerChars.ToArray(),
        };

        return completionOptions;
    }

    private SignatureHelpOptions GetMergedSignatureHelpOptions()
    {
        var allSignatureHelpOptions = _serverCapabilities.Where(s => s.Capabilities.SignatureHelpProvider != null).Select(s => s.Capabilities.SignatureHelpProvider!);

        var triggerCharacters = new HashSet<string>();
        var retriggerChars = new HashSet<string>();
        var workDoneProgress = false;

        foreach (var curSignatureHelpOptions in allSignatureHelpOptions)
        {
            if (curSignatureHelpOptions.TriggerCharacters != null)
            {
                triggerCharacters.UnionWith(curSignatureHelpOptions.TriggerCharacters);
            }

            if (curSignatureHelpOptions.RetriggerCharacters != null)
            {
                retriggerChars.UnionWith(curSignatureHelpOptions.RetriggerCharacters);
            }

            workDoneProgress |= curSignatureHelpOptions.WorkDoneProgress;
        }

        var signatureHelpOptions = new SignatureHelpOptions()
        {
            TriggerCharacters = triggerCharacters.ToArray(),
            RetriggerCharacters = retriggerChars.ToArray(),
            WorkDoneProgress = workDoneProgress,
        };

        return signatureHelpOptions;
    }

    private bool GetMergedDefinitionProvider()
    {
        return _serverCapabilities.Any(s => s.Capabilities.DefinitionProvider?.Value is bool isDefinitionSupported && isDefinitionSupported);
    }

    private bool GetMergedReferencesProvider()
    {
        return _serverCapabilities.Any(s => s.Capabilities.ReferencesProvider?.Value is bool isFindAllReferencesSupported && isFindAllReferencesSupported);
    }

    private bool GetMergedRenameProvider()
    {
        return _serverCapabilities.Any(s => s.Capabilities.RenameProvider?.Value is bool isRenameSupported && isRenameSupported);
    }

    private async Task VerifyMergedOnAutoInsertAsync(VSInternalServerCapabilities mergedCapabilities)
    {
        var triggerCharEnumeration = mergedCapabilities.OnAutoInsertProvider?.TriggerCharacters ?? Enumerable.Empty<string>();
        var purposefullyRemovedTriggerCharacters = new[]
        {
            ">", // https://github.com/dotnet/aspnetcore-tooling/pull/3797
            "-", // Typically used to auto-insert HTML comments, now provided by language-configuration.json
        };
        triggerCharEnumeration = triggerCharEnumeration.Except(purposefullyRemovedTriggerCharacters);
        var onAutoInsertMergedTriggerChars = new HashSet<string>(triggerCharEnumeration);
        if (!onAutoInsertMergedTriggerChars.SetEquals(triggerCharEnumeration))
        {
            await _joinableTaskFactory.SwitchToMainThreadAsync();

            Debug.Fail("on auto insert contained langauge server capabilities mismatch");
        }
    }

    private async Task VerifyMergedHoverAsync(VSServerCapabilities mergedCapabilities)
    {
        if (_initializeResult is null)
        {
            throw new InvalidOperationException("Initialize was not called prior to validating sub-language options.");
        }

        if (mergedCapabilities.HoverProvider != _initializeResult.Capabilities.HoverProvider)
        {
            await _joinableTaskFactory.SwitchToMainThreadAsync();

            Debug.Fail("hover contained langauge server capabilities mismatch");
        }
    }

    private async Task VerifyMergedCompletionOptionsAsync(VSServerCapabilities mergedCapabilities)
    {
        if (_initializeResult is null)
        {
            throw new InvalidOperationException("Initialize was not called prior to validating sub-language options.");
        }

        var mergedAllCommitCharEnumeration = mergedCapabilities.CompletionProvider?.AllCommitCharacters ?? Enumerable.Empty<string>();
        var mergedTriggerCharEnumeration = mergedCapabilities.CompletionProvider?.TriggerCharacters ?? Enumerable.Empty<string>();
        var mergedCommitChars = new HashSet<string>(mergedAllCommitCharEnumeration);
        var purposefullyRemovedTriggerCharacters = new[]
        {
            "_", // https://github.com/dotnet/aspnetcore-tooling/pull/2827

            // C# uses '>' as a trigger character for pointer operations. This conflicts heavily with HTML's auto-closing support
            // Therefore, for perf reasons we purposefully remove the trigger character since using pointers in Razor is quite rare.
            ">"
        };
        mergedTriggerCharEnumeration = mergedTriggerCharEnumeration.Except(purposefullyRemovedTriggerCharacters);
        var mergedTriggerChars = new HashSet<string>(mergedTriggerCharEnumeration);

        if (!mergedCommitChars.SetEquals(_initializeResult.Capabilities.CompletionProvider?.AllCommitCharacters!) ||
            !mergedTriggerChars.SetEquals(_initializeResult.Capabilities.CompletionProvider?.TriggerCharacters!))
        {
            await _joinableTaskFactory.SwitchToMainThreadAsync();

            Debug.Fail("completion merged contained langauge server capabilities mismatch");
        }
    }

    private async Task VerifyMergedSignatureHelpOptionsAsync(VSServerCapabilities mergedCapabilities)
    {
        if (_initializeResult is null)
        {
            throw new InvalidOperationException("Initialize was not called prior to validating sub-language options.");
        }

        var mergedTriggerCharEnumeration = mergedCapabilities.SignatureHelpProvider?.TriggerCharacters ?? Enumerable.Empty<string>();
        var mergedTriggerChars = new HashSet<string>(mergedTriggerCharEnumeration);
        var mergedRetriggerCharEnumeration = mergedCapabilities.SignatureHelpProvider?.RetriggerCharacters ?? Enumerable.Empty<string>();
        var mergedRetriggerChars = new HashSet<string>(mergedRetriggerCharEnumeration);
        var mergedWorkDoneProgress = mergedCapabilities.SignatureHelpProvider?.WorkDoneProgress;

        if (!mergedTriggerChars.SetEquals(_initializeResult.Capabilities.SignatureHelpProvider?.TriggerCharacters!) ||
            !mergedRetriggerChars.SetEquals(_initializeResult.Capabilities.SignatureHelpProvider?.RetriggerCharacters!) ||
            mergedWorkDoneProgress != _initializeResult.Capabilities.SignatureHelpProvider?.WorkDoneProgress)
        {
            await _joinableTaskFactory.SwitchToMainThreadAsync();

            Debug.Fail("signature help merged contained langauge server capabilities mismatch");
        }
    }

    private async Task VerifyMergedDefinitionProviderAsync(VSServerCapabilities mergedCapabilities)
    {
        if (_initializeResult is null)
        {
            throw new InvalidOperationException("Initialize was not called prior to validating sub-language options.");
        }

        if (mergedCapabilities.DefinitionProvider != _initializeResult.Capabilities.DefinitionProvider)
        {
            await _joinableTaskFactory.SwitchToMainThreadAsync();

            Debug.Fail("definition provider contained langauge server capabilities mismatch");
        }
    }

    private async Task VerifyMergedReferencesProviderAsync(VSServerCapabilities mergedCapabilities)
    {
        if (_initializeResult is null)
        {
            throw new InvalidOperationException("Initialize was not called prior to validating sub-language options.");
        }

        if (mergedCapabilities.ReferencesProvider != _initializeResult.Capabilities.ReferencesProvider)
        {
            await _joinableTaskFactory.SwitchToMainThreadAsync();

            Debug.Fail("references provider contained langauge server capabilities mismatch");
        }
    }

    private async Task VerifyMergedRenameProviderAsync(VSServerCapabilities mergedCapabilities)
    {
        if (_initializeResult is null)
        {
            throw new InvalidOperationException("Initialize was not called prior to validating sub-language options.");
        }

        if (mergedCapabilities.RenameProvider != _initializeResult.Capabilities.RenameProvider)
        {
            await _joinableTaskFactory.SwitchToMainThreadAsync();

            Debug.Fail("rename provider contained langauge server capabilities mismatch");
        }
    }

    // Ensures all contained language servers that we rely on are started.
    private async Task<List<ILanguageClient>> EnsureContainedLanguageServersInitializedAsync()
    {
        var relevantLanguageClients = new List<ILanguageClient>();
        var clientLoadTasks = new List<Task>();

#pragma warning disable CS0618 // Type or member is obsolete
        foreach (var languageClientAndMetadata in _languageServiceBroker.LanguageClients)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            if (languageClientAndMetadata.Metadata is not ILanguageClientMetadata metadata)
            {
                continue;
            }

            if (metadata is IIsUserExperienceDisabledMetadata userExperienceDisabledMetadata &&
                userExperienceDisabledMetadata.IsUserExperienceDisabled)
            {
                continue;
            }

            if (IsCSharpApplicable(metadata) ||
                metadata.ContentTypes.Contains(RazorLSPConstants.HtmlLSPDelegationContentTypeName))
            {
                relevantLanguageClients.Add(languageClientAndMetadata.Value);

                var loadAsyncTask = _languageClientBroker.LoadAsync(metadata, languageClientAndMetadata.Value);
                clientLoadTasks.Add(loadAsyncTask);
            }
        }

        await Task.WhenAll(clientLoadTasks).ConfigureAwait(false);

        return relevantLanguageClients;

        static bool IsCSharpApplicable(ILanguageClientMetadata metadata)
        {
            return metadata.ContentTypes.Contains(RazorLSPConstants.CSharpContentTypeName) &&
                metadata.ClientName == CSharpVirtualDocumentFactory.CSharpClientName;
        }
    }
}
