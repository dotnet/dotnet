// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Threading;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem.Sources;

internal sealed class GeneratedOutputSource
{
    private readonly SemaphoreSlim _gate = new(initialCount: 1);

    private RazorCodeDocument? _output;

    public bool TryGetValue([NotNullWhen(true)] out RazorCodeDocument? result)
    {
        result = _output;
        return result is not null;
    }

    public async ValueTask<RazorCodeDocument> GetValueAsync(DocumentSnapshot document, CancellationToken cancellationToken)
    {
        if (TryGetValue(out var result))
        {
            return result;
        }

        using (await _gate.DisposableWaitAsync(cancellationToken).ConfigureAwait(false))
        {
            if (TryGetValue(out result))
            {
                return result;
            }

            var project = document.Project;
            var projectEngine = project.ProjectEngine;
            var compilerOptions = project.CompilerOptions;

            var importItems = await project.GetImportItemsAsync(document.HostDocument, cancellationToken).ConfigureAwait(false);

            _output = await CompilationHelpers
                .GenerateCodeDocumentAsync(document, projectEngine, importItems, compilerOptions, cancellationToken)
                .ConfigureAwait(false);

            return _output;
        }
    }
}
