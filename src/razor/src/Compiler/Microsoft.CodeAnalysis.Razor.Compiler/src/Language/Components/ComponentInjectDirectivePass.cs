// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Razor.Language.Components;

internal class ComponentInjectDirectivePass : IntermediateNodePassBase, IRazorDirectiveClassifierPass
{
    protected override void ExecuteCore(
        RazorCodeDocument codeDocument,
        DocumentIntermediateNode documentNode)
    {
        var visitor = new Visitor();
        visitor.Visit(documentNode);

        var properties = new HashSet<string>(StringComparer.Ordinal);
        var classNode = documentNode.FindPrimaryClass();

        for (var i = visitor.Directives.Count - 1; i >= 0; i--)
        {
            var directive = visitor.Directives[i];
            var tokens = directive.Tokens.ToArray();
            if (tokens.Length < 2)
            {
                continue;
            }

            var typeName = tokens[0].Content;
            var typeSpan = tokens[0].Source;
            var memberName = tokens[1].Content;
            var memberSpan = tokens[1].Source;

            if (!properties.Add(memberName))
            {
                continue;
            }

            classNode.Children.Add(new ComponentInjectIntermediateNode(typeName, memberName, typeSpan, memberSpan));
        }
    }

    private class Visitor : IntermediateNodeWalker
    {
        public IList<DirectiveIntermediateNode> Directives { get; }
            = new List<DirectiveIntermediateNode>();

        public override void VisitDirective(DirectiveIntermediateNode node)
        {
            if (node.Directive == ComponentInjectDirective.Directive)
            {
                Directives.Add(node);
            }
        }
    }
}
