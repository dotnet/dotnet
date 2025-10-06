// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.Text.Adornments;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Completion;

internal interface IVisualStudioDescriptionFactory
{
    ContainerElement CreateClassifiedDescription(AggregateBoundAttributeDescription description);
}
