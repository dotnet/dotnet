// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts.WrapWithTag;

/// <summary>
/// Class representing the parameters sent for a textDocument/_vsweb_wrapWithTag request.
/// </summary>
internal class WrapWithTagParams
{
    /// <summary>
    /// Gets or sets the identifier for the text document to be operate on.
    /// </summary>
    [JsonProperty("_vs_textDocument")]
    public TextDocumentIdentifier TextDocument { get; set; }

    /// <summary>
    /// Gets or sets the selection range to be wrapped.
    /// </summary>
    [JsonProperty("_vs_range")]
    public Range? Range { get; set; }

    /// <summary>
    /// Gets or sets the wrapping tag name.
    /// </summary>
    [JsonProperty("_vs_tagName")]
    public string? TagName { get; set; }

    /// <summary>
    /// Gets or sets the formatting options.
    /// </summary>
    [JsonProperty("_vs_options")]
    public FormattingOptions? Options { get; set; }

    public WrapWithTagParams(TextDocumentIdentifier textDocument)
    {
        TextDocument = textDocument;
    }
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

    [JsonProperty("_vs_textDocument")]
    public new VersionedTextDocumentIdentifier TextDocument { get; set; }
}
