// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost;

internal class UnsupportedCSharpLoweringPhase : RazorEnginePhaseBase, IRazorCSharpLoweringPhase
{
    internal const string UnsupportedDisclaimer = "// Razor CSharp output is not supported for this project's version of Razor.";

    protected override void ExecuteCore(RazorCodeDocument codeDocument)
    {
        var documentNode = codeDocument.GetDocumentIntermediateNode();
        ThrowForMissingDocumentDependency(documentNode);

        var csharpDocument = new RazorCSharpDocument(codeDocument, UnsupportedDisclaimer, documentNode.Options, diagnostics: []);
        codeDocument.SetCSharpDocument(csharpDocument);
        codeDocument.SetUnsupported();
    }
}
