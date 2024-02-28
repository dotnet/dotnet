// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.PooledObjects;

namespace Microsoft.AspNetCore.Razor.Language;

#pragma warning disable CS0618 // Type or member is obsolete
internal class DefaultRazorParsingPhase : RazorEnginePhaseBase, IRazorParsingPhase
{
    private IRazorParserOptionsFeature? _optionsFeature;

    protected override void OnInitialized()
    {
        _optionsFeature = GetRequiredFeature<IRazorParserOptionsFeature>();
    }

    protected override void ExecuteCore(RazorCodeDocument codeDocument)
    {
        var options = codeDocument.GetParserOptions() ?? _optionsFeature.AssumeNotNull().GetOptions();
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
