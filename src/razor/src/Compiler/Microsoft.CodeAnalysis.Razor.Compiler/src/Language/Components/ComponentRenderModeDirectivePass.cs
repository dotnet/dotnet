// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable enable

using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Razor.Language.Components;

internal sealed class ComponentRenderModeDirectivePass : IntermediateNodePassBase, IRazorDirectiveClassifierPass
{
    private const string GeneratedRenderModeAttributeName = "__PrivateComponentRenderModeAttribute";

    protected override void ExecuteCore(RazorCodeDocument codeDocument, DocumentIntermediateNode documentNode)
    {
        var @namespace = documentNode.FindPrimaryNamespace();
        var @class = documentNode.FindPrimaryClass();
        if (@namespace == null || @class == null)
        {
            return;
        }

        var directives = documentNode.FindDirectiveReferences(ComponentRenderModeDirective.Directive);
        if (directives.Count == 0)
        {
            return;
        }

        // We don't need to worry about duplicate attributes as we have already replaced any multiples with MalformedDirective
        Debug.Assert(directives.Count == 1);

        var child = ((DirectiveIntermediateNode)directives[0].Node).Children.FirstOrDefault();
        if (child == null)
        {
            return;
        }

        // generate the inner attribute class
        var classDecl = new ClassDeclarationIntermediateNode()
        {
            ClassName = GeneratedRenderModeAttributeName,
            BaseType = $"global::{ComponentsApi.RenderModeAttribute.FullTypeName}",
        };
        classDecl.Modifiers.Add("private");
        classDecl.Modifiers.Add("sealed");
        classDecl.Children.Add(new CSharpCodeIntermediateNode()
        {
            Children =
            {
                new IntermediateToken()
                {
                    Kind = TokenKind.CSharp,
                    Content = $"private static global::{ComponentsApi.IComponentRenderMode.FullTypeName} ModeImpl => "
                },
                new CSharpCodeIntermediateNode()
                {
                    Source = child.Source,
                    Children =
                    {
                         child is not DirectiveTokenIntermediateNode directiveToken
                         ? child
                         : new IntermediateToken()
                         {
                             Kind = TokenKind.CSharp,
                             Content = directiveToken.Content
                         }
                    }
                },
                new IntermediateToken()
                {
                    Kind = TokenKind.CSharp,
                    Content = ";"
                }
            }
        });
        classDecl.Children.Add(new CSharpCodeIntermediateNode()
        {
            Children =
            {
                new IntermediateToken()
                {
                    Kind = TokenKind.CSharp,
                    Content = $"public override global::{ComponentsApi.IComponentRenderMode.FullTypeName} Mode => ModeImpl;"
                }
            }
        });
        @class.Children.Add(classDecl);

        // generate the attribute usage on top of the class
        var attributeNode = new CSharpCodeIntermediateNode();
        attributeNode.Children.Add(new IntermediateToken()
        {
            Kind = TokenKind.CSharp,
            Content = $"[global::{@namespace.Content}.{@class.ClassName}.{GeneratedRenderModeAttributeName}]",
        });

        // Insert the new attribute on top of the class
        var childCount = @namespace.Children.Count;
        for (var i = 0; i < childCount; i++)
        {
            if (object.ReferenceEquals(@namespace.Children[i], @class))
            {
                @namespace.Children.Insert(i, attributeNode);
                break;
            }
        }
        Debug.Assert(@namespace.Children.Count == childCount + 1);
    }
}
