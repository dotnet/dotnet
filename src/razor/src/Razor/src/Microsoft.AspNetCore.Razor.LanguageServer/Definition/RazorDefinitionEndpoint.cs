// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Syntax;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Razor.Workspaces.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using DefinitionResult = Microsoft.VisualStudio.LanguageServer.Protocol.SumType<
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalLocation,
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalLocation[],
    Microsoft.VisualStudio.LanguageServer.Protocol.DocumentLink[]>;
using SyntaxKind = Microsoft.AspNetCore.Razor.Language.SyntaxKind;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Definition;

internal class RazorDefinitionEndpoint : AbstractRazorDelegatingEndpoint<TextDocumentPositionParamsBridge, DefinitionResult?>, IDefinitionEndpoint
{
    private readonly RazorComponentSearchEngine _componentSearchEngine;
    private readonly RazorDocumentMappingService _documentMappingService;

    public RazorDefinitionEndpoint(
        RazorComponentSearchEngine componentSearchEngine,
        RazorDocumentMappingService documentMappingService,
        LanguageServerFeatureOptions languageServerFeatureOptions,
        ClientNotifierServiceBase languageServer,
        ILoggerFactory loggerFactory)
        : base(languageServerFeatureOptions, documentMappingService, languageServer, loggerFactory.CreateLogger<RazorDefinitionEndpoint>())
    {
        _componentSearchEngine = componentSearchEngine ?? throw new ArgumentNullException(nameof(componentSearchEngine));
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
    }

    protected override bool PreferCSharpOverHtmlIfPossible => true;

    protected override string CustomMessageTarget => RazorLanguageServerCustomMessageTargets.RazorDefinitionEndpointName;

    public RegistrationExtensionResult GetRegistration(VSInternalClientCapabilities clientCapabilities)
    {
        const string ServerCapability = "definitionProvider";
        var option = new SumType<bool, DefinitionOptions>(new DefinitionOptions());

        return new RegistrationExtensionResult(ServerCapability, option);
    }

    protected async override Task<DefinitionResult?> TryHandleAsync(TextDocumentPositionParamsBridge request, RazorRequestContext requestContext, Projection projection, CancellationToken cancellationToken)
    {
        requestContext.Logger.LogInformation("Starting go-to-def endpoint request.");
        var documentContext = requestContext.GetRequiredDocumentContext();

        if (!FileKinds.IsComponent(documentContext.FileKind))
        {
            requestContext.Logger.LogInformation("FileKind '{fileKind}' is not a component type.", documentContext.FileKind);
            return default;
        }

        var originTagDescriptor = await GetOriginTagHelperBindingAsync(documentContext, projection.AbsoluteIndex, requestContext.Logger, cancellationToken).ConfigureAwait(false);
        if (originTagDescriptor is null)
        {
            requestContext.Logger.LogInformation("Origin TagHelper descriptor is null.");
            return default;
        }

        var originComponentDocumentSnapshot = await _componentSearchEngine.TryLocateComponentAsync(originTagDescriptor).ConfigureAwait(false);
        if (originComponentDocumentSnapshot is null)
        {
            requestContext.Logger.LogInformation("Origin TagHelper document snapshot is null.");
            return default;
        }

        requestContext.Logger.LogInformation("Definition found at file path: {filePath}", originComponentDocumentSnapshot.FilePath);

        var originComponentUri = new UriBuilder
        {
            Path = originComponentDocumentSnapshot.FilePath,
            Scheme = Uri.UriSchemeFile,
            Host = string.Empty,
        }.Uri;

        return new[]
        {
            new VSInternalLocation
            {
                Uri = originComponentUri,
                // When navigating from a start or end tag, we just take the user to the top of the file.
                Range = new Range { Start = new Position(0, 0), End = new Position(0, 0) }
            },
        };
    }

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(TextDocumentPositionParamsBridge request, RazorRequestContext requestContext, Projection projection, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();
        return Task.FromResult<IDelegatedParams?>(new DelegatedPositionParams(
                documentContext.Identifier,
                projection.Position,
                projection.LanguageKind));
    }

    protected async override Task<DefinitionResult?> HandleDelegatedResponseAsync(DefinitionResult? response, TextDocumentPositionParamsBridge originalRequest, RazorRequestContext requestContext, Projection projection, CancellationToken cancellationToken)
    {
        if (response is null)
        {
            return null;
        }

        if (response.Value.TryGetFirst(out var location))
        {
            (location.Uri, location.Range) = await _documentMappingService.MapFromProjectedDocumentRangeAsync(location.Uri, location.Range, cancellationToken).ConfigureAwait(false);
        }
        else if (response.Value.TryGetSecond(out var locations))
        {
            foreach (var loc in locations)
            {
                (loc.Uri, loc.Range) = await _documentMappingService.MapFromProjectedDocumentRangeAsync(loc.Uri, loc.Range, cancellationToken).ConfigureAwait(false);
            }
        }
        else if (response.Value.TryGetThird(out var links))
        {
            foreach (var link in links)
            {
                if (link.Target is not null)
                {
                    (link.Target, link.Range) = await _documentMappingService.MapFromProjectedDocumentRangeAsync(link.Target, link.Range, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        return response;
    }

    internal static async Task<TagHelperDescriptor?> GetOriginTagHelperBindingAsync(
        DocumentContext documentContext,
        int absoluteIndex,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var owner = await documentContext.GetSyntaxNodeAsync(absoluteIndex, cancellationToken).ConfigureAwait(false);
        if (owner is null)
        {
            logger.LogInformation("Could not locate owner.");
            return null;
        }

        var node = owner.Ancestors().FirstOrDefault(n =>
            n.Kind == SyntaxKind.MarkupTagHelperStartTag ||
            n.Kind == SyntaxKind.MarkupTagHelperEndTag);
        if (node is null)
        {
            logger.LogInformation("Could not locate ancestor of type MarkupTagHelperStartTag or MarkupTagHelperEndTag.");
            return null;
        }

        var name = GetStartOrEndTagName(node);
        if (name is null)
        {
            logger.LogInformation("Could not retrieve name of start or end tag.");
            return null;
        }

        if (!name.Span.IntersectsWith(absoluteIndex))
        {
            logger.LogInformation("Tag name's span does not intersect with location's absolute index ({absoluteIndex}).", absoluteIndex);
            return null;
        }

        if (node.Parent is not MarkupTagHelperElementSyntax tagHelperElement)
        {
            logger.LogInformation("Parent of start or end tag is not a MarkupTagHelperElement.");
            return null;
        }

        var originTagDescriptor = tagHelperElement.TagHelperInfo.BindingResult.Descriptors.FirstOrDefault(d => !d.IsAttributeDescriptor());
        if (originTagDescriptor is null)
        {
            logger.LogInformation("Origin TagHelper descriptor is null.");
            return null;
        }

        return originTagDescriptor;
    }

    private static SyntaxNode? GetStartOrEndTagName(SyntaxNode node)
    {
        return node switch
        {
            MarkupTagHelperStartTagSyntax tagHelperStartTag => tagHelperStartTag.Name,
            MarkupTagHelperEndTagSyntax tagHelperEndTag => tagHelperEndTag.Name,
            _ => null
        };
    }
}
