// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

internal class RazorLanguageQueryResponse
{
    public RazorLanguageKind Kind { get; init; }

    public int PositionIndex { get; init; }

    public required Position Position { get; init; }

    public int? HostDocumentVersion { get; init; }
}
