// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Razor.Language.Components;

internal sealed class ComponentRenderModeLoweringPass : ComponentIntermediateNodePassBase, IRazorOptimizationPass
{
    // Run after component lowering pass
    public override int Order => 50;

    protected override void ExecuteCore(RazorCodeDocument codeDocument, DocumentIntermediateNode documentNode)
    {
        if (!IsComponentDocument(documentNode))
        {
            return;
        }

        var references = documentNode.FindDescendantReferences<TagHelperDirectiveAttributeIntermediateNode>();
        foreach (var reference in references)
        {
            if (reference is { Node: TagHelperDirectiveAttributeIntermediateNode node, Parent: IntermediateNode parentNode } && node.TagHelper.IsRenderModeTagHelper())
            {
                if (parentNode is not ComponentIntermediateNode componentNode)
                {
                    node.AddDiagnostic(ComponentDiagnosticFactory.CreateAttribute_ValidOnlyOnComponent(node.Source, node.OriginalAttributeName));
                    continue;
                }

                var expression = node.Children[0] switch
                {
                    CSharpExpressionIntermediateNode cSharpNode => cSharpNode.Children[0],
                    IntermediateNode token => token
                };

                var renderModeNode = new RenderModeIntermediateNode() { Source = node.Source, Children = { expression } };
                renderModeNode.AddDiagnosticsFromNode(node);

                if (componentNode.Component.Metadata.ContainsKey(ComponentMetadata.Component.HasRenderModeDirectiveKey))
                {
                    renderModeNode.AddDiagnostic(ComponentDiagnosticFactory.CreateRenderModeAttribute_ComponentDeclaredRenderMode(
                       node.Source,
                       componentNode.Component.Name));
                }

                reference.Replace(renderModeNode);
            }
        }
    }
}
