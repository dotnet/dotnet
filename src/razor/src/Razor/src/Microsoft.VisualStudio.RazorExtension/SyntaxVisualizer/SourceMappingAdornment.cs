// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.VisualStudio.RazorExtension.SyntaxVisualizer;

internal sealed class SourceMappingAdornment : Border
{
    internal SourceMappingAdornment(bool isStart, string toolTipText)
    {
        var lbl = new Label()
        {
            Padding = new Thickness(0),
            BorderThickness = new Thickness(0),
            HorizontalContentAlignment = HorizontalAlignment.Left,
            VerticalContentAlignment = VerticalAlignment.Top,
            VerticalAlignment = VerticalAlignment.Stretch,
            Background = Brushes.Yellow,
            Foreground = Brushes.Black,
            Width = isStart ? 15 : 10,
            Content = isStart ? "<#" : ">",
        };

        if (!string.IsNullOrWhiteSpace(toolTipText))
        {
            lbl.ToolTip = toolTipText;
        }

        Child = lbl;
    }
}
