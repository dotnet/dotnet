// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Tooltip;

internal abstract class LSPTagHelperTooltipFactory : TagHelperTooltipFactoryBase
{
    public abstract bool TryCreateTooltip(AggregateBoundElementDescription elementDescriptionInfo, MarkupKind markupKind, [NotNullWhen(true)] out MarkupContent? tooltipContent);

    public abstract bool TryCreateTooltip(AggregateBoundAttributeDescription attributeDescriptionInfo, MarkupKind markupKind, [NotNullWhen(true)] out MarkupContent? tooltipContent);
}
