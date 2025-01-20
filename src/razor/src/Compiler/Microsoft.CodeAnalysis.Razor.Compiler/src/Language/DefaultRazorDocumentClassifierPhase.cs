// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Threading;

namespace Microsoft.AspNetCore.Razor.Language;

internal class DefaultRazorDocumentClassifierPhase : RazorEnginePhaseBase, IRazorDocumentClassifierPhase
{
    public ImmutableArray<IRazorDocumentClassifierPass> Passes { get; private set; }

    protected override void OnInitialized()
    {
        Passes = Engine.GetFeatures<IRazorDocumentClassifierPass>().OrderByAsArray(p => p.Order);
    }

    protected override void ExecuteCore(RazorCodeDocument codeDocument, CancellationToken cancellationToken)
    {
        var irDocument = codeDocument.GetDocumentIntermediateNode();
        ThrowForMissingDocumentDependency(irDocument);

        foreach (var pass in Passes)
        {
            pass.Execute(codeDocument, irDocument);
        }

        codeDocument.SetDocumentIntermediateNode(irDocument);
    }
}
