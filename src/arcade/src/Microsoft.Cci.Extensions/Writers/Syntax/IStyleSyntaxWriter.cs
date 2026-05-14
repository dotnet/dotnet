// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.Cci.Writers.Syntax
{
    public interface IStyleSyntaxWriter : ISyntaxWriter
    {
        IDisposable StartStyle(SyntaxStyle style, object context);
    }

    public static class StyleSyntaxWriterExtensions
    {
        public static IDisposable StartStyle(this IStyleSyntaxWriter writer, SyntaxStyle style)
        {
            return writer.StartStyle(style, null);
        }
    }
}
