// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

internal class CompilationTagHelperResolver
{
    private readonly ITelemetryReporter? _telemetryReporter;

    public CompilationTagHelperResolver(ITelemetryReporter? telemetryReporter)
    {
        _telemetryReporter = telemetryReporter;
    }

    public async Task<TagHelperResolutionResult> GetTagHelpersAsync(Project workspaceProject, RazorProjectEngine engine, CancellationToken cancellationToken)
    {
        if (workspaceProject is null)
        {
            throw new ArgumentNullException(nameof(workspaceProject));
        }

        if (engine is null)
        {
            throw new ArgumentNullException(nameof(engine));
        }

        var providers = engine.Engine.Features.OfType<ITagHelperDescriptorProvider>().OrderBy(f => f.Order).ToArray();
        if (providers.Length == 0)
        {
            return TagHelperResolutionResult.Empty;
        }

        var results = new HashSet<TagHelperDescriptor>();
        var context = TagHelperDescriptorProviderContext.Create(results);
        context.ExcludeHidden = true;
        context.IncludeDocumentation = true;

        var compilation = await workspaceProject.GetCompilationAsync(cancellationToken).ConfigureAwait(false);
        if (CompilationTagHelperFeature.IsValidCompilation(compilation))
        {
            context.SetCompilation(compilation);
        }

        var timingDictionary = new Dictionary<string, long>();
        for (var i = 0; i < providers.Length; i++)
        {
            var provider = providers[i];
            var stopWatch = Stopwatch.StartNew();

            provider.Execute(context);

            stopWatch.Stop();
            var propertyName = $"{provider.GetType().Name}.elapsedtimems";
            Debug.Assert(!timingDictionary.ContainsKey(propertyName));
            timingDictionary[propertyName] = stopWatch.ElapsedMilliseconds;
        }

        _telemetryReporter?.ReportEvent("taghelperresolver/gettaghelpers", Severity.Normal, timingDictionary.ToImmutableDictionary());
        return new TagHelperResolutionResult(results, Array.Empty<RazorDiagnostic>());
    }
}
