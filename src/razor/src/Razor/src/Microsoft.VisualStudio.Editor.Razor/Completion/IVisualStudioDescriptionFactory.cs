// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Tooltip;
using Microsoft.VisualStudio.Text.Adornments;

namespace Microsoft.VisualStudio.Editor.Razor.Completion;

internal interface IVisualStudioDescriptionFactory
{
    ContainerElement CreateClassifiedDescription(AggregateBoundAttributeDescription description);
}
