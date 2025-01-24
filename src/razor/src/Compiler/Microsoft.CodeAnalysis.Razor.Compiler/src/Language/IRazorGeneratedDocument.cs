// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.Language;

internal interface IRazorGeneratedDocument
{
    RazorCodeDocument CodeDocument { get; }
    SourceText Text { get; }
    RazorCodeGenerationOptions Options { get; }
    ImmutableArray<SourceMapping> SourceMappings { get; }
}
