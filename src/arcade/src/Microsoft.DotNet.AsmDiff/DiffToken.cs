// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.AsmDiff
{
    public sealed class DiffToken
    {
        public DiffStyle Style { get; private set; }
        public DiffTokenKind Kind { get; set; }
        public string Text { get; private set; }

        public DiffToken(DiffStyle style, DiffTokenKind kind, string text)
        {
            Style = style;
            Text = text;
            Kind = kind;
        }

        public bool HasStyle(DiffStyle diffStyle)
        {
            // Special case the zero-flag.
            if (diffStyle == DiffStyle.None)
                return Style == DiffStyle.None;

            return (Style & diffStyle) == diffStyle;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
