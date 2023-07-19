// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Range = Microsoft.VisualStudio.LanguageServer.Protocol.Range;

// A file for delegated record types to be put. Each individually
// should be a plain record. If more logic is needed than record
// definition please put in a separate file.

namespace Microsoft.AspNetCore.Razor.LanguageServer.Protocol;

internal record DelegatedSpellCheckParams(
    VersionedTextDocumentIdentifier HostDocument);

internal record DelegatedDiagnosticParams(
    VersionedTextDocumentIdentifier HostDocument,
    Guid CorrelationId);

internal record DelegatedPositionParams(
    VersionedTextDocumentIdentifier HostDocument,
    Position ProjectedPosition,
    RazorLanguageKind ProjectedKind,
    VSProjectContext? ProjectContext) : IDelegatedParams;

internal record DelegatedValidateBreakpointRangeParams(
    VersionedTextDocumentIdentifier HostDocument,
    Range ProjectedRange,
    RazorLanguageKind ProjectedKind) : IDelegatedParams;

internal record DelegatedOnAutoInsertParams(
    VersionedTextDocumentIdentifier HostDocument,
    Position ProjectedPosition,
    RazorLanguageKind ProjectedKind,
    string Character,
    FormattingOptions Options) : IDelegatedParams;

internal record DelegatedRenameParams(
    VersionedTextDocumentIdentifier HostDocument,
    Position ProjectedPosition,
    RazorLanguageKind ProjectedKind,
    string NewName,
    VSProjectContext? ProjectContext) : IDelegatedParams;

internal record DelegatedCompletionParams(
    VersionedTextDocumentIdentifier HostDocument,
    Position ProjectedPosition,
    RazorLanguageKind ProjectedKind,
    VSInternalCompletionContext Context,
    TextEdit? ProvisionalTextEdit,
    Guid CorrelationId) : IDelegatedParams;

internal record DelegatedCompletionResolutionContext(
    DelegatedCompletionParams OriginalRequestParams,
    object? OriginalCompletionListData);

internal record DelegatedCompletionItemResolveParams(
    VersionedTextDocumentIdentifier HostDocument,
    VSInternalCompletionItem CompletionItem,
    RazorLanguageKind OriginatingKind);

internal record DelegatedProjectContextsParams(
    VersionedTextDocumentIdentifier HostDocument);

internal record DelegatedDocumentSymbolParams(
    TextDocumentIdentifier TextDocument,
    int Version);
