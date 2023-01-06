// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

public abstract partial class ProjectSnapshotManagerBenchmarkBase
{
    private class StaticTagHelperResolver : TagHelperResolver
    {
        private readonly IReadOnlyList<TagHelperDescriptor> _tagHelpers;

        public StaticTagHelperResolver(IReadOnlyList<TagHelperDescriptor> tagHelpers, ITelemetryReporter telemetryReporter)
            : base(telemetryReporter)
        {
            _tagHelpers = tagHelpers;
        }

        public override Task<TagHelperResolutionResult> GetTagHelpersAsync(
            Project project,
            ProjectSnapshot projectSnapshot,
            CancellationToken cancellationToken = default)
            => Task.FromResult(new TagHelperResolutionResult(_tagHelpers, Array.Empty<RazorDiagnostic>()));
    }
}
