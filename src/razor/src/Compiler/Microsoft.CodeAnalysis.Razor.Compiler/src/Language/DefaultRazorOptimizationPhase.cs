// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Threading;

namespace Microsoft.AspNetCore.Razor.Language;

internal class DefaultRazorOptimizationPhase : RazorEnginePhaseBase, IRazorOptimizationPhase
{
    public ImmutableArray<IRazorOptimizationPass> Passes { get; private set; }

    protected override void OnInitialized()
    {
        Passes = Engine.GetFeatures<IRazorOptimizationPass>().OrderByAsArray(static x => x.Order);
    }

    protected override void ExecuteCore(RazorCodeDocument codeDocument, CancellationToken cancellationToken)
    {
        var documentNode = codeDocument.GetDocumentIntermediateNode();
        ThrowForMissingDocumentDependency(documentNode);

        foreach (var pass in Passes)
        {
            pass.Execute(codeDocument, documentNode);
        }

        codeDocument.SetDocumentIntermediateNode(documentNode);
    }
}
