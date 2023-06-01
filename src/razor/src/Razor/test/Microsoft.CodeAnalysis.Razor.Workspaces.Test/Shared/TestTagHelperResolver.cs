// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor;

internal class TestTagHelperResolver : TagHelperResolver
{
    public TestTagHelperResolver() : base(NoOpTelemetryReporter.Instance)
    {
    }

    public ImmutableArray<TagHelperDescriptor> TagHelpers { get; set; } = ImmutableArray<TagHelperDescriptor>.Empty;

    public override Task<TagHelperResolutionResult> GetTagHelpersAsync(Project workspaceProject, IProjectSnapshot projectSnapshot, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new TagHelperResolutionResult(TagHelpers.ToImmutableArray()));
    }
}
