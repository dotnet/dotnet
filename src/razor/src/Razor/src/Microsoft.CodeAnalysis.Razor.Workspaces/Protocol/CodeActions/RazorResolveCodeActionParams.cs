// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;

internal record RazorResolveCodeActionParams(
    [property: JsonPropertyName("identifier")] TextDocumentIdentifier Identifier,
    [property: JsonPropertyName("hostDocumentVersion")] int HostDocumentVersion,
    [property: JsonPropertyName("languageKind")] RazorLanguageKind LanguageKind,
    [property: JsonPropertyName("codeAction")] CodeAction CodeAction);
