// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal record DirectiveAttributeCompletionContext
{
    public required string SelectedAttributeName { get; init; }
    public string? SelectedParameterName { get; init; }
    public ImmutableArray<string> ExistingAttributes { get; init => field = value.NullToEmpty(); } = [];
    public bool UseSnippets { get; init; } = true;
    public bool InAttributeName { get; init; } = true;
    public bool InParameterName { get; init; }
    public RazorCompletionOptions Options { get; init; }
}
