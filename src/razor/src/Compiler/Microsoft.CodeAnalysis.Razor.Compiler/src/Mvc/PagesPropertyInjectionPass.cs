// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Mvc.Razor.Extensions;

public class PagesPropertyInjectionPass : IntermediateNodePassBase, IRazorOptimizationPass
{
    protected override void ExecuteCore(RazorCodeDocument codeDocument, DocumentIntermediateNode documentNode)
    {
        if (documentNode.DocumentKind != RazorPageDocumentClassifierPass.RazorPageDocumentKind)
        {
            return;
        }

        // We only nullable-enable razor page `@model` for RazorLangVersion 9+ to avoid breaking users.
        var razor9OrHigher = codeDocument.ParserOptions.LanguageVersion >= RazorLanguageVersion.Version_9_0;
        var nullableEnabled = razor9OrHigher && !codeDocument.CodeGenerationOptions.SuppressNullabilityEnforcement;

        var modelType = ModelDirective.GetModelType(documentNode);
        var visitor = new Visitor();
        visitor.Visit(documentNode);

        var @class = visitor.Class;

        var viewDataType = $"global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<{modelType.Content}>";
        var vddProperty = new CSharpCodeIntermediateNode();
        vddProperty.Children.Add(new IntermediateToken()
        {
            Kind = TokenKind.CSharp,
            Content = nullableEnable(nullableEnabled, $"public {viewDataType} ViewData => ({viewDataType})PageContext?.ViewData"),
        });
        @class.Children.Add(vddProperty);

        if (codeDocument.CodeGenerationOptions.DesignTime || !razor9OrHigher)
        {
            var modelProperty = new CSharpCodeIntermediateNode();
            modelProperty.Children.Add(new IntermediateToken()
            {
                Kind = TokenKind.CSharp,
                Content = nullableEnable(nullableEnabled, $"public {modelType.Content} Model => ViewData.Model"),
            });
            @class.Children.Add(modelProperty);
        }
        else
        {
            @class.Children.Add(new PropertyDeclarationIntermediateNode()
            {
                Modifiers = { "public" },
                PropertyName = "Model",
                PropertyType = modelType,
                PropertyExpression = "ViewData.Model"
            });
        }

        static string nullableEnable(bool nullableEnabled, string code)
        {
            if (!nullableEnabled)
            {
                return code + ";";
            }

            return $"#nullable restore\r\n{code}!;\r\n#nullable disable";
        }
    }

    private class Visitor : IntermediateNodeWalker
    {
        public ClassDeclarationIntermediateNode Class { get; private set; }

        public override void VisitClassDeclaration(ClassDeclarationIntermediateNode node)
        {
            if (Class == null)
            {
                Class = node;
            }

            base.VisitClassDeclaration(node);
        }
    }
}
