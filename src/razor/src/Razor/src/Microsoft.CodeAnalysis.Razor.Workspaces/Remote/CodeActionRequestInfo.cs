// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Protocol.CodeActions;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal record CodeActionRequestInfo(
    [property: JsonPropertyName("languageKind")] RazorLanguageKind LanguageKind,
    [property: JsonPropertyName("csharpRequest")] VSCodeActionParams? CSharpRequest);
