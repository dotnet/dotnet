// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentPresentation;

internal class TextDocumentTextPresentationEndpoint : AbstractTextDocumentPresentationEndpointBase<TextPresentationParams>, ITextDocumentTextPresentationHandler
{
    public TextDocumentTextPresentationEndpoint(
        RazorDocumentMappingService razorDocumentMappingService,
        ClientNotifierServiceBase languageServer,
        LanguageServerFeatureOptions languageServerFeatureOptions)
        : base(razorDocumentMappingService,
             languageServer,
             languageServerFeatureOptions)
    {
    }

    public override string EndpointName => RazorLanguageServerCustomMessageTargets.RazorTextPresentationEndpoint;

    public override RegistrationExtensionResult GetRegistration(VSInternalClientCapabilities clientCapabilities)
    {
        const string AssociatedServerCapability = "_vs_textPresentationProvider";

        return new RegistrationExtensionResult(AssociatedServerCapability, options: true);
    }

    public override TextDocumentIdentifier GetTextDocumentIdentifier(TextPresentationParams request)
    {
        return request.TextDocument;
    }

    protected override IRazorPresentationParams CreateRazorRequestParameters(TextPresentationParams request)
        => new RazorTextPresentationParams()
        {
            TextDocument = request.TextDocument,
            Range = request.Range,
            Text = request.Text
        };

    protected override Task<WorkspaceEdit?> TryGetRazorWorkspaceEditAsync(
        RazorLanguageKind languageKind,
        TextPresentationParams request,
        CancellationToken cancellationToken)
    {
        // We don't do anything special with text
        return Task.FromResult<WorkspaceEdit?>(null);
    }
}
