// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Razor.Language.Extensions;

internal class ImplementsDirectivePass : IntermediateNodePassBase, IRazorDirectiveClassifierPass
{
    protected override void ExecuteCore(RazorCodeDocument codeDocument, DocumentIntermediateNode documentNode)
    {
        var @class = documentNode.FindPrimaryClass();
        if (@class == null)
        {
            return;
        }

        foreach (var implements in documentNode.FindDirectiveReferences(ImplementsDirective.Directive))
        {
            var token = ((DirectiveIntermediateNode)implements.Node).Tokens.FirstOrDefault();
            if (token != null)
            {
                var source = codeDocument.ParserOptions.DesignTime ? null : token.Source;
                @class.Interfaces.Add(IntermediateToken.CreateCSharpToken(token.Content, source));
            }
        }
    }
}
