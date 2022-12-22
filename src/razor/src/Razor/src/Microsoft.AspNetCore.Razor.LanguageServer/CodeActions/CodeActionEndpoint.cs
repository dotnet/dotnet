// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

internal class CodeActionEndpoint : IVSCodeActionEndpoint
{
    private readonly RazorDocumentMappingService _documentMappingService;
    private readonly IEnumerable<RazorCodeActionProvider> _razorCodeActionProviders;
    private readonly IEnumerable<CSharpCodeActionProvider> _csharpCodeActionProviders;
    private readonly IEnumerable<HtmlCodeActionProvider> _htmlCodeActionProviders;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
    private readonly ClientNotifierServiceBase _languageServer;

    internal bool _supportsCodeActionResolve = false;

    private readonly ImmutableHashSet<string> _allAvailableCodeActionNames;

    public bool MutatesSolutionState { get; } = false;

    public CodeActionEndpoint(
        RazorDocumentMappingService documentMappingService,
        IEnumerable<RazorCodeActionProvider> razorCodeActionProviders,
        IEnumerable<CSharpCodeActionProvider> csharpCodeActionProviders,
        IEnumerable<HtmlCodeActionProvider> htmlCodeActionProviders,
        ClientNotifierServiceBase languageServer,
        LanguageServerFeatureOptions languageServerFeatureOptions)
    {
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
        _razorCodeActionProviders = razorCodeActionProviders ?? throw new ArgumentNullException(nameof(razorCodeActionProviders));
        _csharpCodeActionProviders = csharpCodeActionProviders ?? throw new ArgumentNullException(nameof(csharpCodeActionProviders));
        _htmlCodeActionProviders = htmlCodeActionProviders ?? throw new ArgumentNullException(nameof(htmlCodeActionProviders));
        _languageServer = languageServer ?? throw new ArgumentNullException(nameof(languageServer));
        _languageServerFeatureOptions = languageServerFeatureOptions ?? throw new ArgumentNullException(nameof(languageServerFeatureOptions));

        _allAvailableCodeActionNames = GetAllAvailableCodeActionNames();
    }
    public async Task<SumType<Command, CodeAction>[]?> HandleRequestAsync(CodeActionParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        var razorCodeActionContext = await GenerateRazorCodeActionContextAsync(request, documentContext.Snapshot).ConfigureAwait(false);
        if (razorCodeActionContext is null)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var razorCodeActions = await GetRazorCodeActionsAsync(razorCodeActionContext, cancellationToken).ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        var delegatedCodeActions = await GetDelegatedCodeActionsAsync(documentContext, razorCodeActionContext, cancellationToken).ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        var codeActions = Enumerable.Concat(
            razorCodeActions ?? Array.Empty<RazorVSInternalCodeAction>(),
            delegatedCodeActions ?? Array.Empty<RazorVSInternalCodeAction>());

        if (!codeActions.Any())
        {
            return null;
        }

        // We must cast the RazorCodeAction into a platform compliant code action
        // For VS (SupportsCodeActionResolve = true) this means just encapsulating the RazorCodeAction in the `CommandOrCodeAction` struct
        // For VS Code (SupportsCodeActionResolve = false) we must convert it into a CodeAction or Command before encapsulating in the `CommandOrCodeAction` struct.
        var commandsOrCodeActions = codeActions.Select(c =>
            _supportsCodeActionResolve ? c : c.AsVSCodeCommandOrCodeAction());

        return commandsOrCodeActions.ToArray();
    }

    public RegistrationExtensionResult GetRegistration(VSInternalClientCapabilities clientCapabilities)
    {
        _supportsCodeActionResolve = clientCapabilities.TextDocument?.CodeAction?.ResolveSupport != null;

        const string ServerCapability = "codeActionProvider";

        var options = new CodeActionOptions
        {
            CodeActionKinds = new[] {
                    CodeActionKind.RefactorExtract,
                    CodeActionKind.QuickFix,
                    CodeActionKind.Refactor
                },
            ResolveProvider = true,
        };
        return new RegistrationExtensionResult(ServerCapability, new SumType<bool, CodeActionOptions>(options));
    }

    // internal for testing
    internal async Task<RazorCodeActionContext?> GenerateRazorCodeActionContextAsync(CodeActionParams request, DocumentSnapshot documentSnapshot)
    {
        var codeDocument = await documentSnapshot.GetGeneratedOutputAsync().ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            return null;
        }

        var sourceText = await documentSnapshot.GetTextAsync().ConfigureAwait(false);

        // VS Provides `CodeActionParams.Context.SelectionRange` in addition to
        // `CodeActionParams.Range`. The `SelectionRange` is relative to where the
        // code action was invoked (ex. line 14, char 3) whereas the `Range` is
        // always at the start of the line (ex. line 14, char 0). We want to utilize
        // the relative positioning to ensure we provide code actions for the appropriate
        // context.
        //
        // Note: VS Code doesn't provide a `SelectionRange`.
        var vsCodeActionContext = (VSInternalCodeActionContext)request.Context;
        if (vsCodeActionContext.SelectionRange != null)
        {
            request.Range = vsCodeActionContext.SelectionRange;
        }

        var linePosition = new LinePosition(
            request.Range.Start.Line,
            request.Range.Start.Character);
        var hostDocumentIndex = sourceText.Lines.GetPosition(linePosition);
        var location = new SourceLocation(
            hostDocumentIndex,
            request.Range.Start.Line,
            request.Range.Start.Character);

        var context = new RazorCodeActionContext(
            request,
            documentSnapshot,
            codeDocument,
            location,
            sourceText,
            _languageServerFeatureOptions.SupportsFileManipulation,
            _supportsCodeActionResolve);

        return context;
    }

    private async Task<IEnumerable<RazorVSInternalCodeAction>?> GetDelegatedCodeActionsAsync(DocumentContext documentContext, RazorCodeActionContext context, CancellationToken cancellationToken)
    {
        var languageKind = _documentMappingService.GetLanguageKind(context.CodeDocument, context.Location.AbsoluteIndex, rightAssociative: false);

        // No point delegating if we're in a Razor context
        if (languageKind == RazorLanguageKind.Razor)
        {
            return null;
        }

        var codeActions = await GetCodeActionsFromLanguageServerAsync(languageKind, documentContext, context, cancellationToken);
        if (codeActions is null || !codeActions.Any())
        {
            return null;
        }

        IEnumerable<ICodeActionProvider> providers;
        if (languageKind == RazorLanguageKind.CSharp)
        {
            codeActions = ExtractCSharpCodeActionNamesFromData(codeActions);
            providers = _csharpCodeActionProviders;
        }
        else
        {
            providers = _htmlCodeActionProviders;
        }

        return await FilterCodeActionsAsync(context, codeActions, providers, cancellationToken);
    }

    private IEnumerable<RazorVSInternalCodeAction> ExtractCSharpCodeActionNamesFromData(IEnumerable<RazorVSInternalCodeAction> codeActions)
    {
        return codeActions.Where(codeAction =>
        {
            // Note: we may see a perf benefit from using a JsonConverter
            var tags = ((JToken?)codeAction.Data)?["CustomTags"]?.ToObject<string[]>();
            if (tags is null || tags.Length == 0)
            {
                return false;
            }

            foreach (var tag in tags)
            {
                if (_allAvailableCodeActionNames.Contains(tag))
                {
                    codeAction.Name = tag;
                    break;
                }
            }

            if (string.IsNullOrEmpty(codeAction.Name))
            {
                return false;
            }

            return true;
        }).ToArray();
    }

    private async Task<IEnumerable<RazorVSInternalCodeAction>?> FilterCodeActionsAsync(
        RazorCodeActionContext context,
        IEnumerable<RazorVSInternalCodeAction> codeActions,
        IEnumerable<ICodeActionProvider> providers,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var tasks = new List<Task<IReadOnlyList<RazorVSInternalCodeAction>?>>();

        foreach (var provider in providers)
        {
            var result = provider.ProvideAsync(context, codeActions, cancellationToken);
            if (result != null)
            {
                tasks.Add(result);
            }
        }

        return await ConsolidateCodeActionsFromProvidersAsync(tasks, cancellationToken);
    }

    // Internal for testing
    internal async Task<IEnumerable<RazorVSInternalCodeAction>> GetCodeActionsFromLanguageServerAsync(RazorLanguageKind languageKind, DocumentContext documentContext, RazorCodeActionContext context, CancellationToken cancellationToken)
    {
        if (languageKind == Protocol.RazorLanguageKind.CSharp)
        {
            // For C# we have to map the ranges to the generated document
            if (!_documentMappingService.TryMapToProjectedDocumentRange(
                    context.CodeDocument,
                    context.Request.Range,
                    out var projectedRange))
            {
                return Array.Empty<RazorVSInternalCodeAction>();
            }

            var newContext = context.Request.Context;
            if (context.Request.Context is VSInternalCodeActionContext vsContext &&
                vsContext.SelectionRange is not null &&
                _documentMappingService.TryMapToProjectedDocumentRange(
                    context.CodeDocument,
                    vsContext.SelectionRange,
                    out var selectionRange))
            {
                vsContext.SelectionRange = selectionRange;
                newContext = vsContext;
            }

            context.Request.Range = projectedRange;
            context.Request.Context = newContext;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var delegatedParams = new DelegatedCodeActionParams()
        {
            HostDocumentVersion = documentContext.Version,
            CodeActionParams = context.Request,
            LanguageKind = languageKind
        };

        return await _languageServer.SendRequestAsync<DelegatedCodeActionParams, RazorVSInternalCodeAction[]>(RazorLanguageServerCustomMessageTargets.RazorProvideCodeActionsEndpoint, delegatedParams, cancellationToken);
    }

    private async Task<IEnumerable<RazorVSInternalCodeAction>?> GetRazorCodeActionsAsync(RazorCodeActionContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var tasks = new List<Task<IReadOnlyList<RazorVSInternalCodeAction>?>>();

        foreach (var provider in _razorCodeActionProviders)
        {
            var result = provider.ProvideAsync(context, cancellationToken);
            if (result != null)
            {
                tasks.Add(result);
            }
        }

        return await ConsolidateCodeActionsFromProvidersAsync(tasks, cancellationToken);
    }

    private static async Task<IEnumerable<RazorVSInternalCodeAction>?> ConsolidateCodeActionsFromProvidersAsync(
        List<Task<IReadOnlyList<RazorVSInternalCodeAction>?>> tasks,
        CancellationToken cancellationToken)
    {
        var results = await Task.WhenAll(tasks).ConfigureAwait(false);
        var codeActions = new List<RazorVSInternalCodeAction>();

        cancellationToken.ThrowIfCancellationRequested();

        for (var i = 0; i < results.Length; i++)
        {
            var result = results[i];

            if (result is not null)
            {
                codeActions.AddRange(result);
            }
        }

        return codeActions;
    }

    private static ImmutableHashSet<string> GetAllAvailableCodeActionNames()
    {
        var availableCodeActionNames = new HashSet<string>();

        var refactoringProviderNames = typeof(RazorPredefinedCodeRefactoringProviderNames)
            .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public)
            .Where(property => property.PropertyType == typeof(string))
            .Select(property => property.GetValue(null) as string)
            .WithoutNull();
        var codeFixProviderNames = typeof(RazorPredefinedCodeFixProviderNames)
            .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public)
            .Where(property => property.PropertyType == typeof(string))
            .Select(property => property.GetValue(null) as string)
            .WithoutNull();

        availableCodeActionNames.UnionWith(refactoringProviderNames);
        availableCodeActionNames.UnionWith(codeFixProviderNames);
        availableCodeActionNames.Add(LanguageServerConstants.CodeActions.CodeActionFromVSCode);

        return availableCodeActionNames.ToImmutableHashSet();
    }

    public TextDocumentIdentifier GetTextDocumentIdentifier(CodeActionParams request)
    {
        return request.TextDocument;
    }
}
