// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Threading;

namespace Microsoft.AspNetCore.Razor.Language;

internal class DefaultRazorDirectiveClassifierPhase : RazorEnginePhaseBase, IRazorDirectiveClassifierPhase
{
    public ImmutableArray<IRazorDirectiveClassifierPass> Passes { get; private set; }

    protected override void OnInitialized()
    {
        Passes = Engine.GetFeatures<IRazorDirectiveClassifierPass>().OrderByAsArray(static x => x.Order);
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
