// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.IO;
using Microsoft.AspNetCore.Razor.Language.Syntax;

namespace Microsoft.AspNetCore.Razor.Language.Legacy;

public static class SyntaxNodeSerializer
{
    internal static string Serialize(SyntaxNode node, bool validateSpanEditHandlers)
    {
        using (var writer = new StringWriter())
        {
            var walker = new Walker(writer, validateSpanEditHandlers);
            walker.Visit(node);

            return writer.ToString();
        }
    }

    private class Walker : SyntaxNodeWalker
    {
        private readonly SyntaxNodeWriter _visitor;
        private readonly TextWriter _writer;

        public Walker(TextWriter writer, bool validateSpanEditHandlers)
        {
            _visitor = new SyntaxNodeWriter(writer, validateSpanEditHandlers);
            _writer = writer;
        }

        public TextWriter Writer { get; }

        public override SyntaxNode Visit(SyntaxNode node)
        {
            if (node == null)
            {
                return node;
            }

            if (node.IsList)
            {
                return base.DefaultVisit(node);
            }

            _visitor.Visit(node);
            _writer.WriteLine();

            if (!node.IsToken && !node.IsTrivia)
            {
                _visitor.Depth++;
                node = base.DefaultVisit(node);
                _visitor.Depth--;
            }

            return node;
        }
    }
}
