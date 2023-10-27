// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
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

internal class CompilationTagHelperResolver(ITelemetryReporter? telemetryReporter)
{
    private readonly ITelemetryReporter? _telemetryReporter = telemetryReporter;

    public async ValueTask<ImmutableArray<TagHelperDescriptor>> GetTagHelpersAsync(
        Project workspaceProject,
        RazorProjectEngine projectEngine,
        CancellationToken cancellationToken)
    {
        if (workspaceProject is null)
        {
            throw new ArgumentNullException(nameof(workspaceProject));
        }

        if (projectEngine is null)
        {
            throw new ArgumentNullException(nameof(projectEngine));
        }

        var providers = projectEngine.Engine.Features.OfType<ITagHelperDescriptorProvider>().OrderBy(f => f.Order).ToArray();
        if (providers.Length == 0)
        {
            return ImmutableArray<TagHelperDescriptor>.Empty;
        }

        using var _ = HashSetPool<TagHelperDescriptor>.GetPooledObject(out var results);
        var context = TagHelperDescriptorProviderContext.Create(results);
        context.ExcludeHidden = true;
        context.IncludeDocumentation = true;

        var compilation = await workspaceProject.GetCompilationAsync(cancellationToken).ConfigureAwait(false);
        if (CompilationTagHelperFeature.IsValidCompilation(compilation))
        {
            context.SetCompilation(compilation);
        }

        ExecuteProviders(providers, context, _telemetryReporter);

        return results.ToImmutableArray();

        static void ExecuteProviders(ITagHelperDescriptorProvider[] providers, TagHelperDescriptorProviderContext context, ITelemetryReporter? telemetryReporter)
        {
            using var _ = StopwatchPool.GetPooledObject(out var watch);

            Property[]? properties = null;

            for (var i = 0; i < providers.Length; i++)
            {
                var provider = providers[i];
                watch.Restart();
                provider.Execute(context);
                watch.Stop();

                if (telemetryReporter is not null)
                {
                    properties ??= new Property[providers.Length];
                    var propertyName = $"{provider.GetType().Name}.elapsedtimems";
                    properties[i] = new(propertyName, watch.ElapsedMilliseconds);
                }
            }

            telemetryReporter?.ReportEvent("taghelperresolver/gettaghelpers", Severity.Normal, properties.AssumeNotNull());
        }
    }
}
