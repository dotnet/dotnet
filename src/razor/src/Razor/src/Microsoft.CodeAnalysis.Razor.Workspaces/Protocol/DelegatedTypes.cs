// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Text.Json.Serialization;

// A file for delegated record types to be put. Each individually
// should be a plain record. If more logic is needed than record
// definition please put in a separate file.

namespace Microsoft.CodeAnalysis.Razor.Protocol;

using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.LanguageServer.Protocol;

internal record DelegatedSpellCheckParams(
    [property: JsonPropertyName("identifier")] TextDocumentIdentifierAndVersion Identifier);

internal record DelegatedDiagnosticParams(
    [property: JsonPropertyName("identifier")] TextDocumentIdentifierAndVersion Identifier,
    [property: JsonPropertyName("correlationId")] Guid CorrelationId);

internal record DelegatedPositionParams(
    [property: JsonPropertyName("identifier")] TextDocumentIdentifierAndVersion Identifier,
    [property: JsonPropertyName("projectedPosition")] Position ProjectedPosition,
    [property: JsonPropertyName("projectedKind")] RazorLanguageKind ProjectedKind) : IDelegatedParams;

internal record DelegatedInlayHintParams(
    [property: JsonPropertyName("identifier")] TextDocumentIdentifierAndVersion Identifier,
    [property: JsonPropertyName("projectedRange")] Range ProjectedRange,
    [property: JsonPropertyName("projectedKind")] RazorLanguageKind ProjectedKind) : IDelegatedParams;

internal record DelegatedInlayHintResolveParams(
    [property: JsonPropertyName("identifier")] TextDocumentIdentifierAndVersion Identifier,
    [property: JsonPropertyName("inlayHint")] InlayHint InlayHint,
    [property: JsonPropertyName("projectedKind")] RazorLanguageKind ProjectedKind) : IDelegatedParams;

internal record DelegatedValidateBreakpointRangeParams(
    [property: JsonPropertyName("identifier")] TextDocumentIdentifierAndVersion Identifier,
    [property: JsonPropertyName("projectedRange")] Range ProjectedRange,
    [property: JsonPropertyName("projectedKind")] RazorLanguageKind ProjectedKind) : IDelegatedParams;

internal record DelegatedOnAutoInsertParams(
    [property: JsonPropertyName("identifier")] TextDocumentIdentifierAndVersion Identifier,
    [property: JsonPropertyName("projectedPosition")] Position ProjectedPosition,
    [property: JsonPropertyName("projectedKind")] RazorLanguageKind ProjectedKind,
    [property: JsonPropertyName("character")] string Character,
    [property: JsonPropertyName("options")] FormattingOptions Options) : IDelegatedParams;

internal record DelegatedRenameParams(
    [property: JsonPropertyName("identifier")] TextDocumentIdentifierAndVersion Identifier,
    [property: JsonPropertyName("projectedPosition")] Position ProjectedPosition,
    [property: JsonPropertyName("projectedKind")] RazorLanguageKind ProjectedKind,
    [property: JsonPropertyName("newName")] string NewName) : IDelegatedParams;

internal record DelegatedCompletionParams(
    [property: JsonPropertyName("identifier")] TextDocumentIdentifierAndVersion Identifier,
    [property: JsonPropertyName("projectedPosition")] Position ProjectedPosition,
    [property: JsonPropertyName("projectedKind")] RazorLanguageKind ProjectedKind,
    [property: JsonPropertyName("context")] VSInternalCompletionContext Context,
    [property: JsonPropertyName("provisionalTextEdit")] TextEdit? ProvisionalTextEdit,
    [property: JsonPropertyName("shouldIncludeSnippets")] bool ShouldIncludeSnippets,
    [property: JsonPropertyName("correlationId")] Guid CorrelationId) : IDelegatedParams;

internal record DelegatedMapCodeParams(
    [property: JsonPropertyName("identifier")] TextDocumentIdentifierAndVersion Identifier,
    [property: JsonPropertyName("projectedKind")] RazorLanguageKind ProjectedKind,
    [property: JsonPropertyName("mapCodeCorrelationId")] Guid MapCodeCorrelationId,
    [property: JsonPropertyName("contents")] string[] Contents,
    [property: JsonPropertyName("focusLocations")] Location[][] FocusLocations) : IDelegatedParams;

internal record DelegatedCompletionResolutionContext(
    [property: JsonPropertyName("originalRequestParams")] DelegatedCompletionParams OriginalRequestParams,
    [property: JsonPropertyName("originalCompletionListData")] object? OriginalCompletionListData) : ICompletionResolveContext;

internal record DelegatedCompletionItemResolveParams(
    [property: JsonPropertyName("identifier")] TextDocumentIdentifierAndVersion Identifier,
    [property: JsonPropertyName("completionItem")] VSInternalCompletionItem CompletionItem,
    [property: JsonPropertyName("originatingKind")] RazorLanguageKind OriginatingKind);

internal record DelegatedProjectContextsParams(
    [property: JsonPropertyName("uri")] Uri Uri);

internal record DelegatedDocumentSymbolParams(
    [property: JsonPropertyName("identifier")] TextDocumentIdentifierAndVersion Identifier);

internal record DelegatedSimplifyMethodParams(
    [property: JsonPropertyName("identifier")] TextDocumentIdentifierAndVersion Identifier,
    [property: JsonPropertyName("requiresVirtualDocument")] bool RequiresVirtualDocument,
    [property: JsonPropertyName("textEdit")] TextEdit TextEdit);
