// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language.Components;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Diagnostics;

internal sealed class MissingTagHelperTelemetryReporter(ITelemetryReporter telemetryReporter)
{
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;
    private ImmutableDictionary<ProjectKey, int> _lastReportedProjectTagHelperCount = ImmutableDictionary<ProjectKey, int>.Empty;

    /// <summary>
    /// Reports telemetry for RZ10012 "Found markup element with unexpected name" to help track down potential issues
    /// with taghelpers being discovered (or lack thereof)
    /// </summary>
    public async ValueTask ReportRZ10012TelemetryAsync(DocumentContext documentContext, IEnumerable<Diagnostic> razorDiagnostics, CancellationToken cancellationToken)
    {
        var relevantDiagnosticsCount = razorDiagnostics.Count(d => d.Code == ComponentDiagnosticFactory.UnexpectedMarkupElement.Id);
        if (relevantDiagnosticsCount == 0)
        {
            return;
        }

        var tagHelpers = await documentContext.Project.GetTagHelpersAsync(cancellationToken).ConfigureAwait(false);
        var tagHelperCount = tagHelpers.Length;
        var shouldReport = false;

        ImmutableInterlocked.AddOrUpdate(
            ref _lastReportedProjectTagHelperCount,
            documentContext.Project.Key,
            (k) =>
            {
                shouldReport = true;
                return tagHelperCount;
            },
            (k, currentValue) =>
            {
                shouldReport = currentValue != tagHelperCount;
                return tagHelperCount;
            });

        if (shouldReport)
        {
            _telemetryReporter.ReportEvent(
                "RZ10012",
                Severity.Low,
                new("tagHelpers", tagHelperCount),
                new("RZ10012errors", relevantDiagnosticsCount),
                new("project", documentContext.Project.Key.Id));
        }
    }
}
