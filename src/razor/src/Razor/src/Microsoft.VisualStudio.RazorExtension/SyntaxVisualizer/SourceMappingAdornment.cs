// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
