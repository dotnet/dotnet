// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Threading;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem.Sources;

internal sealed class GeneratedOutputSource(DocumentSnapshot document)
{
    private readonly DocumentSnapshot _document = document;
    private readonly SemaphoreSlim _gate = new(initialCount: 1);

    // Hold the output in a WeakReference to avoid memory leaks in the case of a long-lived
    // document snapshots. In particular, the DynamicFileInfo system results in the Roslyn
    // workspace holding onto document snapshots.
    private WeakReference<RazorCodeDocument>? _output;

    public bool TryGetValue([NotNullWhen(true)] out RazorCodeDocument? result)
    {
        var output = _output;
        if (output is null)
        {
            result = null;
            return false;
        }

        return output.TryGetTarget(out result);
    }

    public async ValueTask<RazorCodeDocument> GetValueAsync(CancellationToken cancellationToken)
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

            var project = _document.Project;
            var projectEngine = project.ProjectEngine;
            var compilerOptions = project.CompilerOptions;

            result = await CompilationHelpers
                .GenerateCodeDocumentAsync(_document, projectEngine, compilerOptions, cancellationToken)
                .ConfigureAwait(false);

            if (_output is null)
            {
                _output = new(result);
            }
            else
            {
                _output.SetTarget(result);
            }

            return result;
        }
    }
}
