// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

public abstract partial class ProjectSnapshotManagerBenchmarkBase
{
    private class StaticTagHelperResolver : TagHelperResolver
    {
        private readonly ImmutableArray<TagHelperDescriptor> _tagHelpers;

        public StaticTagHelperResolver(ImmutableArray<TagHelperDescriptor> tagHelpers, ITelemetryReporter telemetryReporter)
            : base(telemetryReporter)
        {
            _tagHelpers = tagHelpers;
        }

        public override Task<TagHelperResolutionResult> GetTagHelpersAsync(
            Project project,
            IProjectSnapshot projectSnapshot,
            CancellationToken cancellationToken = default)
            => Task.FromResult(new TagHelperResolutionResult(_tagHelpers));
    }
}
