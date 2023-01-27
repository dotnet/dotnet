// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.ColorPresentation;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions.Models;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;
using Microsoft.AspNetCore.Razor.LanguageServer.DocumentColor;
using Microsoft.AspNetCore.Razor.LanguageServer.DocumentPresentation;
using Microsoft.AspNetCore.Razor.LanguageServer.Folding;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic;
using Microsoft.AspNetCore.Razor.LanguageServer.Semantic.Models;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServerClient.Razor.WrapWithTag;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using ImplementationResult = Microsoft.VisualStudio.LanguageServer.Protocol.SumType<
    Microsoft.VisualStudio.LanguageServer.Protocol.Location[],
    Microsoft.VisualStudio.LanguageServer.Protocol.VSInternalReferenceItem[]>;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal abstract class RazorLanguageServerCustomMessageTarget
{
    // Called by the Razor Language Server to retrieve the user's latest settings.
    // NOTE: This method is a polyfill for VS. We only intend to do it this way until VS formally
    // supports sending workspace configuration requests.
    [JsonRpcMethod(Methods.WorkspaceConfigurationName, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<object[]> WorkspaceConfigurationAsync(ConfigurationParams configParams, CancellationToken cancellationToken);

    // Called by the Razor Language Server to update the contents of the virtual CSharp buffer.
    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorUpdateCSharpBufferEndpoint, UseSingleObjectParameterDeserialization = true)]
    public abstract Task UpdateCSharpBufferAsync(UpdateBufferRequest token, CancellationToken cancellationToken);

    // Called by the Razor Language Server to update the contents of the virtual Html buffer.
    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorUpdateHtmlBufferEndpoint, UseSingleObjectParameterDeserialization = true)]
    public abstract Task UpdateHtmlBufferAsync(UpdateBufferRequest token, CancellationToken cancellationToken);

    // Called by the Razor Language Server to invoke a textDocument/formatting request
    // on the virtual Html/CSharp buffer.
    [JsonRpcMethod(LanguageServerConstants.RazorDocumentFormattingEndpoint, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<RazorDocumentRangeFormattingResponse> RazorDocumentFormattingAsync(VersionedDocumentFormattingParams token, CancellationToken cancellationToken);

    // Called by the Razor Language Server to invoke a textDocument/onTypeFormatting  request
    // on the virtual Html buffer.
    [JsonRpcMethod(LanguageServerConstants.RazorDocumentOnTypeFormattingEndpoint, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<RazorDocumentRangeFormattingResponse> HtmlOnTypeFormattingAsync(RazorDocumentOnTypeFormattingParams token, CancellationToken cancellationToken);

    // Called by the Razor Language Server to invoke a textDocument/rangeFormatting request
    // on the virtual Html/CSharp buffer.
    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorRangeFormattingEndpoint, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<RazorDocumentRangeFormattingResponse> RazorRangeFormattingAsync(RazorDocumentRangeFormattingParams token, CancellationToken cancellationToken);

    // Called by the Razor Language Server to provide code actions from the platform.
    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorProvideCodeActionsEndpoint, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<IReadOnlyList<VSInternalCodeAction>?> ProvideCodeActionsAsync(DelegatedCodeActionParams codeActionParams, CancellationToken cancellationToken);

    // Called by the Razor Language Server to resolve code actions from the platform.
    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorResolveCodeActionsEndpoint, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<VSInternalCodeAction?> ResolveCodeActionsAsync(RazorResolveCodeActionParams codeAction, CancellationToken cancellationToken);

    // Called by the Razor Language Server to provide ranged semantic tokens from the platform.
    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorProvideSemanticTokensRangeEndpoint, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<ProvideSemanticTokensResponse?> ProvideSemanticTokensRangeAsync(ProvideSemanticTokensRangeParams semanticTokensParams, CancellationToken cancellationToken);

    // Called by Visual Studio to wrap the current selection with a tag
    [JsonRpcMethod(LanguageServerConstants.RazorWrapWithTagEndpoint, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<VSInternalWrapWithTagResponse> RazorWrapWithTagAsync(VSInternalWrapWithTagParams wrapWithParams, CancellationToken cancellationToken);

    // Called by the Razor Language Server to provide inline completions from the platform.
    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorInlineCompletionEndpoint, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<VSInternalInlineCompletionList?> ProvideInlineCompletionAsync(RazorInlineCompletionRequest inlineCompletionParams, CancellationToken cancellationToken);

    // Called by the Razor Language Server to provide document colors from the platform.
    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorProvideHtmlDocumentColorEndpoint, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<IReadOnlyList<ColorInformation>?> ProvideHtmlDocumentColorAsync(DelegatedDocumentColorParams documentColorParams, CancellationToken cancellationToken);

    // Called by the Razor Language Server to provide color presentation from the platform.
    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorProvideHtmlColorPresentationEndpoint, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<IReadOnlyList<ColorPresentation>> ProvideHtmlColorPresentationAsync(DelegatedColorPresentationParams colorPresentationParams, CancellationToken cancellationToken);

    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorFoldingRangeEndpoint, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<RazorFoldingRangeResponse?> ProvideFoldingRangesAsync(RazorFoldingRangeRequestParam foldingRangeParams, CancellationToken cancellationToken);

    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorTextPresentationEndpoint, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<WorkspaceEdit?> ProvideTextPresentationAsync(RazorTextPresentationParams presentationParams, CancellationToken cancellationToken);

    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorUriPresentationEndpoint, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<WorkspaceEdit?> ProvideUriPresentationAsync(RazorUriPresentationParams presentationParams, CancellationToken cancellationToken);

    [JsonRpcMethod(LanguageServerConstants.RazorCompletionEndpointName, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<JToken?> ProvideCompletionsAsync(DelegatedCompletionParams completionParams, CancellationToken cancellationToken);

    [JsonRpcMethod(LanguageServerConstants.RazorCompletionResolveEndpointName, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<JToken?> ProvideResolvedCompletionItemAsync(DelegatedCompletionItemResolveParams completionResolveParams, CancellationToken cancellationToken);

    [JsonRpcMethod(LanguageServerConstants.RazorGetFormattingOptionsEndpointName, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<FormattingOptions?> GetFormattingOptionsAsync(TextDocumentIdentifier document, CancellationToken cancellationToken);

    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorRenameEndpointName, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<WorkspaceEdit?> RenameAsync(DelegatedRenameParams request, CancellationToken cancellationToken);

    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorHoverEndpointName, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<VSInternalHover?> HoverAsync(DelegatedPositionParams request, CancellationToken cancellationToken);

    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorDefinitionEndpointName, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<Location[]?> DefinitionAsync(DelegatedPositionParams request, CancellationToken cancellationToken);

    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorDocumentHighlightEndpointName, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<DocumentHighlight[]?> DocumentHighlightAsync(DelegatedPositionParams request, CancellationToken cancellationToken);

    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorSignatureHelpEndpointName, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<SignatureHelp?> SignatureHelpAsync(DelegatedPositionParams request, CancellationToken cancellationToken);

    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorImplementationEndpointName, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<ImplementationResult> ImplementationAsync(DelegatedPositionParams request, CancellationToken cancellationToken);

    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorOnAutoInsertEndpointName, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<VSInternalDocumentOnAutoInsertResponseItem?> OnAutoInsertAsync(DelegatedOnAutoInsertParams request, CancellationToken cancellationToken);

    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorValidateBreakpointRangeName, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<Range?> ValidateBreakpointRangeAsync(DelegatedValidateBreakpointRangeParams request, CancellationToken cancellationToken);

    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorPullDiagnosticEndpointName, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<RazorPullDiagnosticResponse?> DiagnosticsAsync(DelegatedDiagnosticParams request, CancellationToken cancellationToken);

    [JsonRpcMethod(RazorLanguageServerCustomMessageTargets.RazorReferencesEndpointName, UseSingleObjectParameterDeserialization = true)]
    public abstract Task<VSInternalReferenceItem[]?> ReferencesAsync(DelegatedPositionParams request, CancellationToken cancellationToken);
}
