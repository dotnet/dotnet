// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

internal static class TextContentChangedEventArgsExtensions
{
    public static bool TextChangeOccurred(this TextContentChangedEventArgs args, out (ITextChange firstChange, ITextChange lastChange, string newText, string oldText) changeInformation)
    {
        if (args.Changes.Count > 0)
        {
            var firstChange = args.Changes[0];
            var lastChange = args.Changes[args.Changes.Count - 1];
            var oldLength = lastChange.OldEnd - firstChange.OldPosition;
            var newLength = lastChange.NewEnd - firstChange.NewPosition;
            var newText = args.After.GetText(firstChange.NewPosition, newLength);
            var oldText = args.Before.GetText(firstChange.OldPosition, oldLength);

            var wasChanged = true;
            if (oldLength == newLength)
            {
                wasChanged = !string.Equals(oldText, newText, StringComparison.Ordinal);
            }

            if (wasChanged)
            {
                changeInformation = (firstChange, lastChange, newText, oldText);
                return true;
            }
        }

        changeInformation = default((ITextChange, ITextChange, string, string));
        return false;
    }
}
