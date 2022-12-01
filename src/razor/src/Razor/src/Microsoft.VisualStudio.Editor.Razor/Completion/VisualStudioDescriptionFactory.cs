// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.Text.Adornments;

namespace Microsoft.VisualStudio.Editor.Razor.Completion;

internal abstract class VisualStudioDescriptionFactory
{
    public abstract ContainerElement CreateClassifiedDescription(AggregateBoundAttributeDescription completionDescription);
}
