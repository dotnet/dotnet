// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.Language;

internal interface IRazorGeneratedDocument
{
    string GeneratedCode { get; }
    RazorCodeGenerationOptions Options { get; }
    ImmutableArray<SourceMapping> SourceMappings { get; }
    RazorCodeDocument? CodeDocument { get; }
}
