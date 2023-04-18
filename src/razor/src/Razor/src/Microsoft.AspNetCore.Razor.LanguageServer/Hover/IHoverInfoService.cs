// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hover;

internal interface IHoverInfoService
{
    VSInternalHover? GetHoverInfo(RazorCodeDocument codeDocument, SourceLocation location, VSInternalClientCapabilities clientCapabilities);
}
