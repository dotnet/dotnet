// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.LanguageServer.Protocol;

internal class MapCodeMapping
{
    /// <summary>
    /// Gets or sets identifier for the document the contents are supposed to be mapped into.
    /// </summary>
    [DataMember(Name = "textDocument")]
    public TextDocumentIdentifier? TextDocument { get; set; }

    /// <summary>
    /// Gets or sets strings of code/text to map into TextDocument.
    /// </summary>
    [DataMember(Name = "contents")]
    public required string[] Contents
    {
        get;
        set;
    }

    /// <summary>
    /// Prioritized Locations to be used when applying heuristics. For example, cursor location,
    /// related classes (in other documents), viewport, etc. Earlier items should be considered
    /// higher priority.
    /// </summary>
    [DataMember(Name = "focusLocations")]
    public Location[][]? FocusLocations
    {
        get;
        set;
    }
}
