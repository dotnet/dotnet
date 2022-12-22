// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.DocumentPresentation;

internal interface IPresentationParams
{
    TextDocumentIdentifier TextDocument { get; set; }
    Range Range { get; set; }
}

internal interface IRazorPresentationParams : IPresentationParams
{
    int HostDocumentVersion { get; set; }
    RazorLanguageKind Kind { get; set; }
}
