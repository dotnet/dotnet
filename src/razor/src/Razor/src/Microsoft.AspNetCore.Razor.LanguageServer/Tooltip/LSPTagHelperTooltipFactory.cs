// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Tooltip;

internal abstract class LSPTagHelperTooltipFactory(ISnapshotResolver snapshotResolver) : TagHelperTooltipFactoryBase(snapshotResolver)
{
    public abstract Task<MarkupContent?> TryCreateTooltipAsync(string documentFilePath, AggregateBoundElementDescription elementDescriptionInfo, MarkupKind markupKind, CancellationToken cancellationToken);

    public abstract bool TryCreateTooltip(AggregateBoundAttributeDescription attributeDescriptionInfo, MarkupKind markupKind, [NotNullWhen(true)] out MarkupContent? tooltipContent);
}
