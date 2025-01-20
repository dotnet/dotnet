// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Buffers;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.CodeAnalysis;

internal static class ProjectExtensions
{
    private const string GetTagHelpersEventName = "taghelperresolver/gettaghelpers";
    private const string PropertySuffix = ".elapsedtimems";

    /// <summary>
    ///  Gets the available <see cref="TagHelperDescriptor">tag helpers</see> from the specified
    ///  <see cref="Project"/> using the given <see cref="RazorProjectEngine"/>.
    /// </summary>
    /// <remarks>
    ///  A telemetry event will be reported to <paramref name="telemetryReporter"/>.
    /// </remarks>
    public static async ValueTask<ImmutableArray<TagHelperDescriptor>> GetTagHelpersAsync(
        this Project project,
        RazorProjectEngine projectEngine,
        ITelemetryReporter telemetryReporter,
        CancellationToken cancellationToken)
    {
        var providers = GetTagHelperDescriptorProviders(projectEngine);

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

        var writeProperties = properties;

        foreach (var provider in providers)
        {
            watch.Restart();
            provider.Execute(context);
            watch.Stop();

            writeProperties[0] = new(provider.GetType().Name + PropertySuffix, watch.ElapsedMilliseconds);
            writeProperties = writeProperties[1..];
        }

        telemetryReporter.ReportEvent(GetTagHelpersEventName, Severity.Normal, properties);

        return [.. results];
    }

    private static ImmutableArray<ITagHelperDescriptorProvider> GetTagHelperDescriptorProviders(RazorProjectEngine projectEngine)
        => projectEngine.Engine.GetFeatures<ITagHelperDescriptorProvider>().OrderByAsArray(static x => x.Order);
}
