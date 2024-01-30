// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.Text.Adornments;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Tooltip;

internal abstract class VSLSPTagHelperTooltipFactory(ISnapshotResolver snapshotResolver) : TagHelperTooltipFactoryBase(snapshotResolver)
{
    public abstract Task<ContainerElement?> TryCreateTooltipContainerAsync(string documentFilePath, AggregateBoundElementDescription elementDescriptionInfo, CancellationToken cancellationToken);

    public abstract bool TryCreateTooltip(AggregateBoundAttributeDescription attributeDescriptionInfo, [NotNullWhen(true)] out ContainerElement? tooltipContent);

    public abstract Task<ClassifiedTextElement?> TryCreateTooltipAsync(string documentFilePath, AggregateBoundElementDescription elementDescriptionInfo, CancellationToken cancellationToken);

    public abstract bool TryCreateTooltip(AggregateBoundAttributeDescription attributeDescriptionInfo, [NotNullWhen(true)] out ClassifiedTextElement? tooltipContent);
}
