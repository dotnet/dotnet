// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Razor.Language.Components;

internal class ComponentSplatLoweringPass : ComponentIntermediateNodePassBase, IRazorOptimizationPass
{
    // Run after component lowering pass
    public override int Order => 50;

    protected override void ExecuteCore(RazorCodeDocument codeDocument, DocumentIntermediateNode documentNode)
    {
        if (!IsComponentDocument(documentNode))
        {
            return;
        }

        foreach (var reference in documentNode.FindDescendantReferences<TagHelperDirectiveAttributeIntermediateNode>())
        {
            var node = (TagHelperDirectiveAttributeIntermediateNode)reference.Node;
            if (node.TagHelper.IsSplatTagHelper())
            {
                reference.Replace(RewriteUsage(reference.Parent, node));
            }
        }
    }

    private IntermediateNode RewriteUsage(IntermediateNode parent, TagHelperDirectiveAttributeIntermediateNode node)
    {
        var result = new SplatIntermediateNode()
        {
            Source = node.Source,
        };

        result.Children.AddRange(node.FindDescendantNodes<IntermediateToken>().Where(t => t.IsCSharp));
        result.AddDiagnosticsFromNode(node);
        return result;
    }
}
