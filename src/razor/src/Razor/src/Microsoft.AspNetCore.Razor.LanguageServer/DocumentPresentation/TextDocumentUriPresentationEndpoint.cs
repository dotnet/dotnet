// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentPresentation;

internal class TextDocumentUriPresentationEndpoint : AbstractTextDocumentPresentationEndpointBase<UriPresentationParams>, ITextDocumentUriPresentationHandler
{
    private readonly RazorComponentSearchEngine _razorComponentSearchEngine;
    private readonly DocumentContextFactory _documentContextFactory;
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly ILogger _logger;

    public TextDocumentUriPresentationEndpoint(
        RazorDocumentMappingService razorDocumentMappingService,
        RazorComponentSearchEngine razorComponentSearchEngine,
        ClientNotifierServiceBase languageServer,
        LanguageServerFeatureOptions languageServerFeatureOptions,
        DocumentContextFactory documentContextFactory,
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        ILoggerFactory loggerFactory)
        : base(razorDocumentMappingService,
             languageServer,
             languageServerFeatureOptions)
    {
        _razorComponentSearchEngine = razorComponentSearchEngine ?? throw new ArgumentNullException(nameof(razorComponentSearchEngine));
        _documentContextFactory = documentContextFactory ?? throw new ArgumentNullException(nameof(documentContextFactory));
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher ?? throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));

        _logger = loggerFactory.CreateLogger<TextDocumentUriPresentationEndpoint>();
    }

    public override string EndpointName => RazorLanguageServerCustomMessageTargets.RazorUriPresentationEndpoint;

    public override RegistrationExtensionResult GetRegistration(VSInternalClientCapabilities clientCapabilities)
    {
        const string AssociatedServerCapability = "_vs_uriPresentationProvider";

        return new RegistrationExtensionResult(AssociatedServerCapability, options: true);
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
            _logger.LogInformation("No URIs were included in the request?");
            return null;
        }

        // We only want to handle requests for a single .razor file, but when there are files nested under a .razor
        // file (for example, Goo.razor.css, Goo.razor.cs etc.) then we'll get all of those files as well, when the user
        // thinks they're just dragging the parent one, so we have to be a little bit clever with the filter here
        var razorFileUri = request.Uris.Last();
        var fileName = Path.GetFileName(razorFileUri.GetAbsoluteOrUNCPath());
        if (!fileName.EndsWith(".razor", FilePathComparison.Instance))
        {
            _logger.LogInformation("Last file in the drop was not a single razor file URI.");
            return null;
        }

        if (request.Uris.Any(uri => !Path.GetFileName(uri.GetAbsoluteOrUNCPath()).StartsWith(fileName, FilePathComparison.Instance)))
        {
            _logger.LogInformation("One or more URIs were not a child file of the main .razor file.");
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
        _logger.LogInformation("Trying to find document info for dropped uri {uri}.", uri);

        var documentContext = await _documentContextFactory.TryCreateAsync(uri, cancellationToken).ConfigureAwait(false);
        if (documentContext is null)
        {
            _logger.LogInformation("Failed to find document for component {uri}.", uri);
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var descriptor = await _razorComponentSearchEngine.TryGetTagHelperDescriptorAsync(documentContext.Snapshot, cancellationToken).ConfigureAwait(false);
        if (descriptor is null)
        {
            _logger.LogInformation("Failed to find tag helper descriptor.");
            return null;
        }

        var typeName = descriptor.GetTypeNameIdentifier();
        if (string.IsNullOrWhiteSpace(typeName))
        {
            _logger.LogWarning("Found a tag helper, {descriptorName}, but it has an empty TypeNameIdentifier.", descriptor.Name);
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

        if (descriptor.AllowedChildTags.Count > 0)
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
