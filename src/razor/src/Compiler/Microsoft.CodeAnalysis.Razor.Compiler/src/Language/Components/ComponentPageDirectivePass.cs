// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Razor.Language.Components;

internal class ComponentPageDirectivePass : IntermediateNodePassBase, IRazorDirectiveClassifierPass
{
    protected override void ExecuteCore(RazorCodeDocument codeDocument, DocumentIntermediateNode documentNode)
    {
        if (codeDocument == null)
        {
            throw new ArgumentNullException(nameof(codeDocument));
        }

        if (documentNode == null)
        {
            throw new ArgumentNullException(nameof(documentNode));
        }

        var @namespace = documentNode.FindPrimaryNamespace();
        var @class = documentNode.FindPrimaryClass();
        if (@namespace == null || @class == null)
        {
            return;
        }

        var directives = documentNode.FindDirectiveReferences(ComponentPageDirective.Directive);
        if (directives.Count == 0)
        {
            return;
        }

        // We don't allow @page directives in imports
        for (var i = 0; i < directives.Count; i++)
        {
            var directive = directives[i];
            if (FileKinds.IsComponentImport(codeDocument.FileKind) || directive.Node.IsImported())
            {
                directive.Node.Diagnostics.Add(ComponentDiagnosticFactory.CreatePageDirective_CannotBeImported(directive.Node.Source.GetValueOrDefault()));
            }
        }

        // Insert the attributes 'on-top' of the class declaration, since classes don't directly support attributes.
        var index = 0;
        for (; index < @namespace.Children.Count; index++)
        {
            if (object.ReferenceEquals(@class, @namespace.Children[index]))
            {
                break;
            }
        }

        for (var i = 0; i < directives.Count; i++)
        {
            var pageDirective = (DirectiveIntermediateNode)directives[i].Node;

            // The parser also adds errors for invalid syntax, we just need to not crash.
            var routeToken = pageDirective.Tokens.First();

            if (routeToken is not { Content: ['"', '/', .., '"'] })
            {
                pageDirective.Diagnostics.Add(ComponentDiagnosticFactory.CreatePageDirective_MustSpecifyRoute(pageDirective.Source));
            }

            if (!codeDocument.CodeGenerationOptions.DesignTime || pageDirective.Diagnostics.Count == 0)
            {
                @namespace.Children.Insert(index++, new RouteAttributeExtensionNode(routeToken.Content) { Source = routeToken.Source });
            }
        }
    }
}
