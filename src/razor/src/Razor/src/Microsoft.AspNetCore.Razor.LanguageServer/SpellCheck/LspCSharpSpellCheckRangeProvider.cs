// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.SpellCheck;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer.SpellCheck;

internal sealed class LspCSharpSpellCheckRangeProvider(
    LanguageServerFeatureOptions languageServerFeatureOptions,
    IClientConnection clientConnection) : ICSharpSpellCheckRangeProvider
{
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly IClientConnection _clientConnection = clientConnection;

    public async Task<ImmutableArray<SpellCheckRange>> GetCSharpSpellCheckRangesAsync(DocumentContext documentContext, CancellationToken cancellationToken)
    {
        if (!_languageServerFeatureOptions.SingleServerSupport)
        {
            return [];
        }

        var delegatedParams = new DelegatedSpellCheckParams(documentContext.GetTextDocumentIdentifierAndVersion());
        var delegatedResponse = await _clientConnection.SendRequestAsync<DelegatedSpellCheckParams, VSInternalSpellCheckableRangeReport[]?>(
            CustomMessageNames.RazorSpellCheckEndpoint,
            delegatedParams,
            cancellationToken).ConfigureAwait(false);

        if (delegatedResponse is not [_, ..] response)
        {
            return [];
        }

        // Most common case is we'll get one report back from Roslyn, so we'll use that as the initial capacity.
        var initialCapacity = response[0].Ranges?.Length ?? 4;

        using var ranges = new PooledArrayBuilder<SpellCheckRange>(initialCapacity);
        foreach (var report in delegatedResponse)
        {
            if (report.Ranges is not { } csharpRanges)
            {
                continue;
            }

            // Since we get C# tokens that have relative starts, we need to convert them back to absolute indexes
            // so we can sort them with the Razor tokens later
            var absoluteCSharpStartIndex = 0;
            for (var i = 0; i < csharpRanges.Length; i += 3)
            {
                var kind = csharpRanges[i];
                var start = csharpRanges[i + 1];
                var length = csharpRanges[i + 2];

                absoluteCSharpStartIndex += start;

                ranges.Add(new(kind, absoluteCSharpStartIndex, length));

                absoluteCSharpStartIndex += length;
            }
        }

        return ranges.ToImmutableAndClear();
    }
}
