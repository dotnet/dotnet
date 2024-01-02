// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentPresentation;

internal class TextDocumentUriPresentationEndpoint(
    IRazorDocumentMappingService razorDocumentMappingService,
    RazorComponentSearchEngine razorComponentSearchEngine,
    IClientConnection clientConnection,
    FilePathService filePathService,
    IDocumentContextFactory documentContextFactory,
    IRazorLoggerFactory loggerFactory)
    : AbstractTextDocumentPresentationEndpointBase<UriPresentationParams>(razorDocumentMappingService, clientConnection, filePathService, loggerFactory.CreateLogger<TextDocumentUriPresentationEndpoint>()), ITextDocumentUriPresentationHandler
{
    private readonly RazorComponentSearchEngine _razorComponentSearchEngine = razorComponentSearchEngine ?? throw new ArgumentNullException(nameof(razorComponentSearchEngine));
    private readonly IDocumentContextFactory _documentContextFactory = documentContextFactory ?? throw new ArgumentNullException(nameof(documentContextFactory));

    public override string EndpointName => CustomMessageNames.RazorUriPresentationEndpoint;

    public override void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.UriPresentationProvider = true;
    }

    public override TextDocumentIdentifier GetTextDocumentIdentifier(UriPresentationParams request)
    {
        return request.TextDocument;
    }

    protected override IRazorPresentationParams CreateRazorRequestParameters(UriPresentationParams request)
        => new RazorUriPresentationParams()
        {
            TextDocument = request.TextDocument,
            Range = request.Range,
            Uris = request.Uris
        };

    protected override async Task<WorkspaceEdit?> TryGetRazorWorkspaceEditAsync(RazorLanguageKind languageKind, UriPresentationParams request, CancellationToken cancellationToken)
    {
        if (languageKind is not RazorLanguageKind.Html)
        {
            // We don't do anything for HTML
            return null;
        }

        if (request.Uris is null || request.Uris.Length == 0)
        {
            Logger.LogInformation("No URIs were included in the request?");
            return null;
        }

        var razorFileUri = request.Uris.Where(
            x => Path.GetFileName(x.GetAbsoluteOrUNCPath()).EndsWith(".razor", FilePathComparison.Instance)).FirstOrDefault();

        // We only want to handle requests for a single .razor file, but when there are files nested under a .razor
        // file (for example, Goo.razor.css, Goo.razor.cs etc.) then we'll get all of those files as well, when the user
        // thinks they're just dragging the parent one, so we have to be a little bit clever with the filter here
        if (razorFileUri == null)
        {
            Logger.LogInformation("No file in the drop was a razor file URI.");
            return null;
        }

        var fileName = Path.GetFileName(razorFileUri.GetAbsoluteOrUNCPath());
        if (request.Uris.Any(uri => !Path.GetFileName(uri.GetAbsoluteOrUNCPath()).StartsWith(fileName, FilePathComparison.Instance)))
        {
            Logger.LogInformation("One or more URIs were not a child file of the main .razor file.");
            return null;
        }

        var componentTagText = await TryGetComponentTagAsync(razorFileUri, cancellationToken).ConfigureAwait(false);
        if (componentTagText is null)
        {
            return null;
        }

        return new WorkspaceEdit
        {
            DocumentChanges = new TextDocumentEdit[]
            {
                new TextDocumentEdit
                {
                    TextDocument = new()
                    {
                        Uri = request.TextDocument.Uri
                    },
                    Edits = new[]
                    {
                        new TextEdit
                        {
                            NewText = componentTagText,
                            Range = request.Range
                        }
                    }
                }
            }
        };
    }

    private async Task<string?> TryGetComponentTagAsync(Uri uri, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Trying to find document info for dropped uri {uri}.", uri);

        var documentContext = _documentContextFactory.TryCreate(uri);
        if (documentContext is null)
        {
            Logger.LogInformation("Failed to find document for component {uri}.", uri);
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var descriptor = await _razorComponentSearchEngine.TryGetTagHelperDescriptorAsync(documentContext.Snapshot, cancellationToken).ConfigureAwait(false);
        if (descriptor is null)
        {
            Logger.LogInformation("Failed to find tag helper descriptor.");
            return null;
        }

        var typeName = descriptor.GetTypeNameIdentifier();
        if (string.IsNullOrWhiteSpace(typeName))
        {
            Logger.LogWarning("Found a tag helper, {descriptorName}, but it has an empty TypeNameIdentifier.", descriptor.Name);
            return null;
        }

        // TODO: Add @using statements if required, or fully qualify (GetTypeName())

        using var _ = StringBuilderPool.GetPooledObject(out var sb);

        sb.Append('<');
        sb.Append(typeName);

        foreach (var requiredAttribute in descriptor.EditorRequiredAttributes)
        {
            sb.Append(' ');
            sb.Append(requiredAttribute.Name);
            sb.Append("=\"\"");
        }

        if (descriptor.AllowedChildTags.Length > 0)
        {
            sb.Append("></");
            sb.Append(typeName);
            sb.Append('>');
        }
        else
        {
            sb.Append(" />");
        }

        return sb.ToString();
    }
}
