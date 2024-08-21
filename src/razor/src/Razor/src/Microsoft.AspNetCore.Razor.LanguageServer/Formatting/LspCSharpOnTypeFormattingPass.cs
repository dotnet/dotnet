// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.CodeActions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Formatting;

internal sealed class LspCSharpOnTypeFormattingPass(
    IDocumentMappingService documentMappingService,
    ILoggerFactory loggerFactory)
    : CSharpOnTypeFormattingPassBase(documentMappingService, loggerFactory)
{

    protected override async Task<TextEdit[]> AddUsingStatementEditsIfNecessaryAsync(CodeAnalysis.Razor.Formatting.FormattingContext context, RazorCodeDocument codeDocument, SourceText csharpText, TextEdit[] textEdits, SourceText originalTextWithChanges, TextEdit[] finalEdits, CancellationToken cancellationToken)
    {
        if (context.AutomaticallyAddUsings)
        {
            // Because we need to parse the C# code twice for this operation, lets do a quick check to see if its even necessary
            if (textEdits.Any(e => e.NewText.IndexOf("using") != -1))
            {
                var usingStatementEdits = await AddUsingsCodeActionProviderHelper.GetUsingStatementEditsAsync(codeDocument, csharpText, originalTextWithChanges, cancellationToken).ConfigureAwait(false);
                finalEdits = [.. usingStatementEdits, .. finalEdits];
            }
        }

        return finalEdits;
    }
}
