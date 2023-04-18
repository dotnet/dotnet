// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable enable

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language.Legacy;

namespace Microsoft.AspNetCore.Razor.Language;
internal sealed class DefaultRazorTagHelperRewritePhase : RazorEnginePhaseBase
{
    protected override void ExecuteCore(RazorCodeDocument codeDocument)
    {
        var syntaxTree = codeDocument.GetPreTagHelperSyntaxTree() ?? codeDocument.GetSyntaxTree();
        ThrowForMissingDocumentDependency(syntaxTree);

        var context = codeDocument.GetTagHelperContext();
        if (context?.TagHelpers.Count > 0)
        {
            var rewrittenSyntaxTree = TagHelperParseTreeRewriter.Rewrite(syntaxTree, context.Prefix, context.TagHelpers, out var usedHelpers);
            codeDocument.SetSyntaxTree(rewrittenSyntaxTree);
            codeDocument.SetReferencedTagHelpers(usedHelpers);
        }
        else
        {
            codeDocument.SetReferencedTagHelpers(new HashSet<TagHelperDescriptor>());
        }
    }
}
