// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using Microsoft.AspNetCore.Razor.Language.Legacy;

namespace Microsoft.AspNetCore.Razor.Language;

internal sealed class DefaultRazorTagHelperRewritePhase : RazorEnginePhaseBase
{
    protected override void ExecuteCore(RazorCodeDocument codeDocument, CancellationToken cancellationToken)
    {
        var syntaxTree = codeDocument.GetPreTagHelperSyntaxTree();
        var context = codeDocument.GetTagHelperContext();
        if (syntaxTree is null || context.TagHelpers.Length == 0)
        {
            // No descriptors, no-op.
            return;
        }

        var binder = context.GetBinder();
        var rewrittenSyntaxTree = TagHelperParseTreeRewriter.Rewrite(syntaxTree, binder, out var usedHelpers);

        codeDocument.SetReferencedTagHelpers(usedHelpers);
        codeDocument.SetSyntaxTree(rewrittenSyntaxTree);
    }
}
