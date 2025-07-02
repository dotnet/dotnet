// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
