// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor;

internal class TestTagHelperResolver : TagHelperResolver
{
    public TestTagHelperResolver() : base(NoOpTelemetryReporter.Instance)
    {
    }

    public TaskCompletionSource<TagHelperResolutionResult> CompletionSource { get; set; }

    public List<TagHelperDescriptor> TagHelpers { get; set; } = new List<TagHelperDescriptor>();

    public override Task<TagHelperResolutionResult> GetTagHelpersAsync(Project workspaceProject, IProjectSnapshot projectSnapshot, CancellationToken cancellationToken = default)
    {
        if (CompletionSource is null)
        {
            return Task.FromResult(new TagHelperResolutionResult(TagHelpers.ToArray(), Array.Empty<RazorDiagnostic>()));
        }
        else
        {
            return CompletionSource.Task;
        }
    }
}
