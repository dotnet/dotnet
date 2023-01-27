// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable enable

using Microsoft.AspNetCore.Razor.Language.Legacy;

namespace Microsoft.AspNetCore.Razor.Language;
internal sealed class DefaultRazorTagHelperRewritePhase : RazorEnginePhaseBase
{
    protected override void ExecuteCore(RazorCodeDocument codeDocument)
    {
        var syntaxTree = codeDocument.GetPreTagHelperSyntaxTree();
        var context = codeDocument.GetTagHelperContext();
        if (syntaxTree is null || context.TagHelpers.Count == 0)
        {
            // No descriptors, no-op.
            return;
        }

        var rewrittenSyntaxTree = TagHelperParseTreeRewriter.Rewrite(syntaxTree, context.Prefix, context.TagHelpers, out var usedHelpers);

        codeDocument.SetReferencedTagHelpers(usedHelpers);
        codeDocument.SetSyntaxTree(rewrittenSyntaxTree);
    }
}
