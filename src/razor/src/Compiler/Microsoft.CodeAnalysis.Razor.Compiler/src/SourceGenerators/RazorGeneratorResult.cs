// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.NET.Sdk.Razor.SourceGenerators
{
    internal sealed class RazorGeneratorResult(IReadOnlyList<TagHelperDescriptor> tagHelpers, ImmutableDictionary<string, (string hintName, RazorCodeDocument document)> documents)
    {
        public IReadOnlyList<TagHelperDescriptor> TagHelpers => tagHelpers;

        public RazorCodeDocument? GetCodeDocument(string physicalPath) => documents.TryGetValue(physicalPath, out var pair) ? pair.document : null;

        public string? GetHintName(string physicalPath) => documents.TryGetValue(physicalPath, out var pair) ? pair.hintName : null;
    }
}
