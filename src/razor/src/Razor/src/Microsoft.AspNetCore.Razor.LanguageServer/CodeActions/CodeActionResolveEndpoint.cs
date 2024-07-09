// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;

[RazorLanguageServerEndpoint(Methods.CodeActionResolveName)]
internal sealed class CodeActionResolveEndpoint(
    IEnumerable<IRazorCodeActionResolver> razorCodeActionResolvers,
    IEnumerable<CSharpCodeActionResolver> csharpCodeActionResolvers,
    IEnumerable<HtmlCodeActionResolver> htmlCodeActionResolvers,
    ILoggerFactory loggerFactory) : IRazorDocumentlessRequestHandler<CodeAction, CodeAction>
{
    private readonly ImmutableDictionary<string, IRazorCodeActionResolver> _razorCodeActionResolvers = CreateResolverMap(razorCodeActionResolvers);
    private readonly ImmutableDictionary<string, BaseDelegatedCodeActionResolver> _csharpCodeActionResolvers = CreateResolverMap<BaseDelegatedCodeActionResolver>(csharpCodeActionResolvers);
    private readonly ImmutableDictionary<string, BaseDelegatedCodeActionResolver> _htmlCodeActionResolvers = CreateResolverMap<BaseDelegatedCodeActionResolver>(htmlCodeActionResolvers);
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<CodeActionResolveEndpoint>();

    public bool MutatesSolutionState => false;

    public async Task<CodeAction> HandleRequestAsync(CodeAction request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        if (request.Data is not JsonElement paramsObj)
        {
            _logger.LogError($"Invalid CodeAction Received '{request.Title}'.");
            return request;
        }

        var resolutionParams = paramsObj.Deserialize<RazorCodeActionResolutionParams>();
        if (resolutionParams is null)
        {
            throw new ArgumentOutOfRangeException($"request.Data should be convertible to {nameof(RazorCodeActionResolutionParams)}");
        }

        var codeActionId = GetCodeActionId(resolutionParams);
        _logger.LogInformation($"Resolving workspace edit for action {codeActionId}.");

        // If it's a special "edit based code action" then the edit has been pre-computed and we
        // can extract the edit details and return to the client. This is only required for VSCode
        // as it does not support Command.Edit based code actions anymore.
        if (resolutionParams.Action == LanguageServerConstants.CodeActions.EditBasedCodeActionCommand)
        {
            request.Edit = (resolutionParams.Data as JsonElement?)?.Deserialize<WorkspaceEdit>();
            return request;
        }

        switch (resolutionParams.Language)
        {
            case LanguageServerConstants.CodeActions.Languages.Razor:
                return await ResolveRazorCodeActionAsync(
                    request,
                    resolutionParams,
                    cancellationToken).ConfigureAwait(false);
            case LanguageServerConstants.CodeActions.Languages.CSharp:
                return await ResolveCSharpCodeActionAsync(
                    request,
                    resolutionParams,
                    cancellationToken).ConfigureAwait(false);
            case LanguageServerConstants.CodeActions.Languages.Html:
                return await ResolveHtmlCodeActionAsync(
                    request,
                    resolutionParams,
                    cancellationToken).ConfigureAwait(false);
            default:
                _logger.LogError($"Invalid CodeAction.Data.Language. Received {codeActionId}.");
                return request;
        }
    }

    // Internal for testing
    internal async Task<CodeAction> ResolveRazorCodeActionAsync(
        CodeAction codeAction,
        RazorCodeActionResolutionParams resolutionParams,
        CancellationToken cancellationToken)
    {
        if (!_razorCodeActionResolvers.TryGetValue(resolutionParams.Action, out var resolver))
        {
            var codeActionId = GetCodeActionId(resolutionParams);
            _logger.LogWarning($"No resolver registered for {codeActionId}");
            Debug.Fail($"No resolver registered for {codeActionId}.");
            return codeAction;
        }

        if (resolutionParams.Data is not JsonElement data)
        {
            return codeAction;
        }

        var edit = await resolver.ResolveAsync(data, cancellationToken).ConfigureAwait(false);
        codeAction.Edit = edit;
        return codeAction;
    }

    // Internal for testing
    internal Task<CodeAction> ResolveCSharpCodeActionAsync(CodeAction codeAction, RazorCodeActionResolutionParams resolutionParams, CancellationToken cancellationToken)
        => ResolveDelegatedCodeActionAsync(_csharpCodeActionResolvers, codeAction, resolutionParams, cancellationToken);

    // Internal for testing
    internal Task<CodeAction> ResolveHtmlCodeActionAsync(CodeAction codeAction, RazorCodeActionResolutionParams resolutionParams, CancellationToken cancellationToken)
        => ResolveDelegatedCodeActionAsync(_htmlCodeActionResolvers, codeAction, resolutionParams, cancellationToken);

    private async Task<CodeAction> ResolveDelegatedCodeActionAsync(ImmutableDictionary<string, BaseDelegatedCodeActionResolver> resolvers, CodeAction codeAction, RazorCodeActionResolutionParams resolutionParams, CancellationToken cancellationToken)
    {
        if (resolutionParams.Data is not JsonElement csharpParamsObj)
        {
            _logger.LogError($"Invalid CodeAction Received.");
            Debug.Fail($"Invalid CSharp CodeAction Received.");
            return codeAction;
        }

        var csharpParams = csharpParamsObj.Deserialize<CodeActionResolveParams>();
        if (csharpParams is null)
        {
            throw new ArgumentOutOfRangeException($"Data was not convertible to {nameof(CodeActionResolveParams)}");
        }

        codeAction.Data = csharpParams.Data;

        if (!resolvers.TryGetValue(resolutionParams.Action, out var resolver))
        {
            var codeActionId = GetCodeActionId(resolutionParams);
            _logger.LogWarning($"No resolver registered for {codeActionId}");
            Debug.Fail($"No resolver registered for {codeActionId}.");
            return codeAction;
        }

        var resolvedCodeAction = await resolver.ResolveAsync(csharpParams, codeAction, cancellationToken).ConfigureAwait(false);
        return resolvedCodeAction;
    }

    private static ImmutableDictionary<string, T> CreateResolverMap<T>(IEnumerable<T> codeActionResolvers)
        where T : ICodeActionResolver
    {
        using var _ = DictionaryPool<string, T>.GetPooledObject(out var resolverMap);

        foreach (var resolver in codeActionResolvers)
        {
            if (resolverMap.ContainsKey(resolver.Action))
            {
                Debug.Fail($"Duplicate resolver action for {resolver.Action} of type {typeof(T)}.");
            }

            resolverMap[resolver.Action] = resolver;
        }

        return resolverMap.ToImmutableDictionary();
    }

    private static string GetCodeActionId(RazorCodeActionResolutionParams resolutionParams) =>
        $"`{resolutionParams.Language}.{resolutionParams.Action}`";
}
