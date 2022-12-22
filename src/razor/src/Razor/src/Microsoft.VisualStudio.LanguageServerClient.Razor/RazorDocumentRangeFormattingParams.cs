// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal class RazorDocumentRangeFormattingParams
{
    public RazorLanguageKind Kind { get; init; }

    public required string HostDocumentFilePath { get; init; }

    public required Range ProjectedRange { get; init; }

    public required FormattingOptions Options { get; init; }

    public int HostDocumentVersion { get; set; }
}
