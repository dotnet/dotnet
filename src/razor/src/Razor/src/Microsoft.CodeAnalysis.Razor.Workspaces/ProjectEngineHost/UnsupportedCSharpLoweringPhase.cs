// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.ProjectSystem;

internal class UnsupportedCSharpLoweringPhase : RazorEnginePhaseBase, IRazorCSharpLoweringPhase
{
    internal const string UnsupportedDisclaimer = "// Razor CSharp output is not supported for this project's version of Razor.";

    private static readonly SourceText s_disclaimerText = SourceText.From(UnsupportedDisclaimer, Encoding.UTF8);

    protected override void ExecuteCore(RazorCodeDocument codeDocument, CancellationToken cancellationToken)
    {
        var documentNode = codeDocument.GetDocumentIntermediateNode();
        ThrowForMissingDocumentDependency(documentNode);

        var csharpDocument = new RazorCSharpDocument(codeDocument, s_disclaimerText, documentNode.Options, diagnostics: []);
        codeDocument.SetCSharpDocument(csharpDocument);
        codeDocument.SetUnsupported();
    }
}
