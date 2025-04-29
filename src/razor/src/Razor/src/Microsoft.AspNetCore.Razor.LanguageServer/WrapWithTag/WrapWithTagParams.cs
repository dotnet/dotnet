// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.WrapWithTag;

/// <summary>
/// Class representing the parameters sent for a textDocument/_vsweb_wrapWithTag request.
/// </summary>
internal class WrapWithTagParams(TextDocumentIdentifier textDocument)
{
    /// <summary>
    /// Gets or sets the identifier for the text document to be operate on.
    /// </summary>
    [JsonPropertyName("_vs_textDocument")]
    public TextDocumentIdentifier TextDocument { get; set; } = textDocument;

    /// <summary>
    /// Gets or sets the selection range to be wrapped.
    /// </summary>
    [JsonPropertyName("_vs_range")]
    public Range? Range { get; set; }

    /// <summary>
    /// Gets or sets the wrapping tag name.
    /// </summary>
    [JsonPropertyName("_vs_tagName")]
    public string? TagName { get; set; }

    /// <summary>
    /// Gets or sets the formatting options.
    /// </summary>
    [JsonPropertyName("_vs_options")]
    public FormattingOptions? Options { get; set; }
}

internal class DelegatedWrapWithTagParams : WrapWithTagParams
{
    public DelegatedWrapWithTagParams(VersionedTextDocumentIdentifier identifier, WrapWithTagParams parameters) : base(identifier)
    {
        TextDocument = identifier;
        Range = parameters.Range;
        TagName = parameters.TagName;
        Options = parameters.Options;
    }

    [JsonPropertyName("_vs_textDocument")]
    public new VersionedTextDocumentIdentifier TextDocument { get; set; }
}
