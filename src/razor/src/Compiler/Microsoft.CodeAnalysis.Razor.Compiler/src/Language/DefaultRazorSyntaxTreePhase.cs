// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Threading;

namespace Microsoft.AspNetCore.Razor.Language;

internal class DefaultRazorSyntaxTreePhase : RazorEnginePhaseBase, IRazorSyntaxTreePhase
{
    public ImmutableArray<IRazorSyntaxTreePass> Passes { get; private set; }

    protected override void OnInitialized()
    {
        Passes = Engine.GetFeatures<IRazorSyntaxTreePass>().OrderByAsArray(static x => x.Order);
    }

    protected override void ExecuteCore(RazorCodeDocument codeDocument, CancellationToken cancellationToken)
    {
        var syntaxTree = codeDocument.GetSyntaxTree();
        ThrowForMissingDocumentDependency(syntaxTree);

        foreach (var pass in Passes)
        {
            syntaxTree = pass.Execute(codeDocument, syntaxTree);
        }

        codeDocument.SetSyntaxTree(syntaxTree);
    }
}
