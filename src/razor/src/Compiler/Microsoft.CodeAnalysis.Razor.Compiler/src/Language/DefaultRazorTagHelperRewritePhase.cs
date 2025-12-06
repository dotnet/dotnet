// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using Microsoft.AspNetCore.Razor.Language.Legacy;

namespace Microsoft.AspNetCore.Razor.Language;

internal sealed class DefaultRazorTagHelperRewritePhase : RazorEnginePhaseBase
{
    protected override void ExecuteCore(RazorCodeDocument codeDocument, CancellationToken cancellationToken)
    {
        if (!codeDocument.TryGetPreTagHelperSyntaxTree(out var syntaxTree) ||
            !codeDocument.TryGetTagHelperContext(out var context) ||
            context.TagHelpers is [])
        {
            // No descriptors, so no need to see if any are used. Without setting this though,
            // we trigger an Assert in the ProcessRemaining method in the source generator.
            codeDocument.SetReferencedTagHelpers([]);
            return;
        }

        var binder = context.GetBinder();
        using var usedHelpers = new TagHelperCollection.Builder();
        var rewrittenSyntaxTree = TagHelperParseTreeRewriter.Rewrite(syntaxTree, binder, usedHelpers, cancellationToken);

        codeDocument.SetReferencedTagHelpers(usedHelpers.ToCollection());
        codeDocument.SetSyntaxTree(rewrittenSyntaxTree);
    }
}
