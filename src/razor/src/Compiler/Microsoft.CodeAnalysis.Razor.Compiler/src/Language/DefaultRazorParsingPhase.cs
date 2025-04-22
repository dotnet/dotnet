// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language;

internal class DefaultRazorParsingPhase : RazorEnginePhaseBase, IRazorParsingPhase
{
    protected override void ExecuteCore(RazorCodeDocument codeDocument, CancellationToken cancellationToken)
    {
        var options = codeDocument.ParserOptions;
        var syntaxTree = RazorSyntaxTree.Parse(codeDocument.Source, options);
        codeDocument.SetSyntaxTree(syntaxTree);

        using var importSyntaxTrees = new PooledArrayBuilder<RazorSyntaxTree>(codeDocument.Imports.Length);

        foreach (var import in codeDocument.Imports)
        {
            importSyntaxTrees.Add(RazorSyntaxTree.Parse(import, options));
        }

        codeDocument.SetImportSyntaxTrees(importSyntaxTrees.DrainToImmutable());
    }
}
