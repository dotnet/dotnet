// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Linq;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Razor.Language.Components;

// We don't support 'complex' content for components (mixed C# and markup) right now.
// It's not clear yet if components will have a good scenario to use these constructs.
//
// This is where a lot of the complexity in the Razor/TagHelpers model creeps in and we
// might be able to avoid it if these features aren't needed.
internal class ComponentComplexAttributeContentPass : ComponentIntermediateNodePassBase, IRazorOptimizationPass
{
    // Run before other Component passes
    public override int Order => -1000;

    protected override void ExecuteCore(RazorCodeDocument codeDocument, DocumentIntermediateNode documentNode)
    {
        if (!IsComponentDocument(documentNode))
        {
            return;
        }

        var nodes = documentNode.FindDescendantNodes<TagHelperIntermediateNode>();
        for (var i = 0; i < nodes.Count; i++)
        {
            ProcessAttributes(nodes[i]);
        }
    }

    private void ProcessAttributes(TagHelperIntermediateNode node)
    {
        for (var i = node.Children.Count - 1; i >= 0; i--)
        {
            if (node.Children[i] is TagHelperPropertyIntermediateNode propertyNode &&
                node.TagHelpers.Any(t => t.IsComponentTagHelper))
            {
                ProcessAttribute(node, propertyNode, propertyNode.AttributeName);
            }
            else if (node.Children[i] is TagHelperHtmlAttributeIntermediateNode htmlNode &&
                node.TagHelpers.Any(t => t.IsComponentTagHelper))
            {
                ProcessAttribute(node, htmlNode, htmlNode.AttributeName);
            }
            else if (node.Children[i] is TagHelperDirectiveAttributeIntermediateNode directiveAttributeNode)
            {
                ProcessAttribute(node, directiveAttributeNode, directiveAttributeNode.OriginalAttributeName);
            }
        }
    }

    private static void ProcessAttribute(IntermediateNode parent, IntermediateNode node, string attributeName)
    {
        var removeNode = false;
        var issueDiagnostic = false;

        if (node.Children is [HtmlAttributeIntermediateNode { Children.Count: > 1 }])
        {
            // This case can be hit for a 'string' attribute
            removeNode = true;
            issueDiagnostic = true;
        }
        else if (node.Children is [CSharpExpressionIntermediateNode { Children.Count: > 1 } cSharpNode])
        {
            // This case can be hit when the attribute has an explicit @ inside, which
            // 'escapes' any special sugar we provide for codegen.
            //
            // There's a special case here for explicit expressions. See https://github.com/aspnet/Razor/issues/2203
            // handling this case as a tactical matter since it's important for lambdas.
            if (cSharpNode.Children is [IntermediateToken { Content: "(" }, _, IntermediateToken { Content: ")" }])
            {
                cSharpNode.Children.RemoveAt(2);
                cSharpNode.Children.RemoveAt(0);
            }
            else
            {
                removeNode = true;
                issueDiagnostic = true;
            }
        }
        else if (node.Children is [CSharpCodeIntermediateNode])
        {
            // This is the case when an attribute contains a code block @{ ... }
            // We don't support this.
            removeNode = true;
            issueDiagnostic = true;
        }
        else if (node.Children is [CSharpExpressionIntermediateNode, HtmlContentIntermediateNode { Children: [IntermediateToken { Content: "." }] }])
        {
            // This is the case when an attribute contains something like "@MyEnum."
            // We simplify this to remove the "." so that tooling can provide completion on "MyEnum"
            // in case the user is in the middle of typing
            node.Children.RemoveAt(1);

            // We still want to issue a diagnostic, even though we simplified, because ultimately
            // we don't support this, so if the user isn't typing, we can't let this through
            issueDiagnostic = true;
        }
        else if (node.Children.Count > 1)
        {
            // This is the common case for 'mixed' content
            removeNode = true;
            issueDiagnostic = true;
        }

        if (issueDiagnostic)
        {
            parent.Diagnostics.Add(ComponentDiagnosticFactory.Create_UnsupportedComplexContent(
                node,
                attributeName));
        }
        if (removeNode)
        {
            parent.Children.Remove(node);
        }
    }
}
