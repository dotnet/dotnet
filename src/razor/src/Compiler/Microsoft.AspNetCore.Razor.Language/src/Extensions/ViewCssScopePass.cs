// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Razor.Language.Extensions;

internal class ViewCssScopePass : IntermediateNodePassBase, IRazorOptimizationPass
{
    // Runs after taghelpers are bound
    public override int Order => 110;

    protected override void ExecuteCore(RazorCodeDocument codeDocument, DocumentIntermediateNode documentNode)
    {
        var cssScope = codeDocument.GetCssScope();
        if (string.IsNullOrEmpty(cssScope))
        {
            return;
        }

        if (!string.Equals(documentNode.DocumentKind, "mvc.1.0.view", StringComparison.Ordinal) &&
            !string.Equals(documentNode.DocumentKind, "mvc.1.0.razor-page", StringComparison.Ordinal))
        {
            return;
        }

        var scopeWithSeparator = " " + cssScope;
        var nodes = documentNode.FindDescendantNodes<HtmlContentIntermediateNode>();
        for (var i = 0; i < nodes.Count; i++)
        {
            ProcessElement(nodes[i], scopeWithSeparator);
        }
    }

    private void ProcessElement(HtmlContentIntermediateNode node, string cssScope)
    {
        // Add a minimized attribute whose name is simply the CSS scope
        for (var i = 0; i < node.Children.Count; i++)
        {
            var child = node.Children[i];
            if (child is IntermediateToken token && token.IsHtml)
            {
                if (IsValidElement(token))
                {
                    node.Children.Insert(i + 1, new IntermediateToken()
                    {
                        Content = cssScope,
                        Kind = TokenKind.Html,
                        Source = null
                    });
                    i++;
                }
            }
        }

        static bool IsValidElement(IntermediateToken token)
        {
            var content = token.Content;
            /// <remarks>
            /// We want to avoid adding the CSS scope to elements that do not appear
            /// within the body element of the document. When this pass executes over the
            /// nodes, we don't have the ability to store whether we are a descendant of a
            /// `head` or `body` element so it is not possible to discern whether the tag
            /// is valid this way. Instead, we go for a straight-forward check on the tag
            /// name that we are currently inspecting.
            /// </remarks>
            return content.StartsWith("<", StringComparison.Ordinal)
                && !content.StartsWith("</", StringComparison.Ordinal)
                && !content.StartsWith("<!", StringComparison.Ordinal)
                && !content.Equals("<head", StringComparison.OrdinalIgnoreCase)
                && !content.Equals("<meta", StringComparison.OrdinalIgnoreCase)
                && !content.Equals("<title", StringComparison.OrdinalIgnoreCase)
                && !content.Equals("<link", StringComparison.OrdinalIgnoreCase)
                && !content.Equals("<base", StringComparison.OrdinalIgnoreCase)
                && !content.Equals("<script", StringComparison.OrdinalIgnoreCase)
                && !content.Equals("<style", StringComparison.OrdinalIgnoreCase)
                && !content.Equals("<html", StringComparison.OrdinalIgnoreCase);
        }
    }
}
