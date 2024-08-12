// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Buffers;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor;

internal sealed class CompilationTagHelperResolver(ITelemetryReporter telemetryReporter)
{
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;

    public async ValueTask<ImmutableArray<TagHelperDescriptor>> GetTagHelpersAsync(
        Project project,
        RazorProjectEngine projectEngine,
        CancellationToken cancellationToken)
    {
        var providers = projectEngine.Engine.Features
            .OfType<ITagHelperDescriptorProvider>()
            .OrderBy(static f => f.Order)
            .ToImmutableArray();

        if (providers is [])
        {
            return [];
        }

        var compilation = await project.GetCompilationAsync(cancellationToken).ConfigureAwait(false);
        if (compilation is null || !CompilationTagHelperFeature.IsValidCompilation(compilation))
        {
            return [];
        }

        using var pooledHashSet = HashSetPool<TagHelperDescriptor>.GetPooledObject(out var results);
        using var pooledWatch = StopwatchPool.GetPooledObject(out var watch);
        using var pooledSpan = ArrayPool<Property>.Shared.GetPooledArraySpan(minimumLength: providers.Length, out var properties);

        var context = new TagHelperDescriptorProviderContext(compilation, results)
        {
            ExcludeHidden = true,
            IncludeDocumentation = true
        };

        for (var i = 0; i < providers.Length; i++)
        {
            var provider = providers[i];

            watch.Restart();
            provider.Execute(context);
            watch.Stop();

            properties[i] = new($"{provider.GetType().Name}.elapsedtimems", watch.ElapsedMilliseconds);
        }

        _telemetryReporter.ReportEvent("taghelperresolver/gettaghelpers", Severity.Normal, properties);

        return [.. results];
    }
}
