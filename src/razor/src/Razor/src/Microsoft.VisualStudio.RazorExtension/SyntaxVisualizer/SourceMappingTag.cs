// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.Text.Tagging;

namespace Microsoft.VisualStudio.RazorExtension.SyntaxVisualizer;

/// <summary>
/// Data tag indicating that the tagged text represents a color.
/// </summary>
/// <remarks>
/// Note that this tag has nothing directly to do with adornments or other UI.
/// This sample's adornments will be produced based on the data provided in these tags.
/// This separation provides the potential for other extensions to consume color tags
/// and provide alternative UI or other derived functionality over this data.
/// </remarks>
internal class SourceMappingTag : ITag
{
    internal SourceMappingTag(bool isStart, string toolTipText = "")
    {
        IsStart = isStart;
        ToolTipText = toolTipText;
    }

    internal readonly bool IsStart;
    internal readonly string ToolTipText;
}
