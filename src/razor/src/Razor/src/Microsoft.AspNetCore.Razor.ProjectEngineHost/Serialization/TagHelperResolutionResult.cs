// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Language;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

[JsonConverter(typeof(TagHelperResolutionResultJsonConverter))]
internal sealed class TagHelperResolutionResult
{
    internal static readonly TagHelperResolutionResult Empty = new(Array.Empty<TagHelperDescriptor>(), Array.Empty<RazorDiagnostic>());

    public TagHelperResolutionResult(IReadOnlyCollection<TagHelperDescriptor>? descriptors, IReadOnlyList<RazorDiagnostic>? diagnostics)
    {
        Descriptors = descriptors;
        Diagnostics = diagnostics;
    }

    public IReadOnlyCollection<TagHelperDescriptor>? Descriptors { get; }

    public IReadOnlyList<RazorDiagnostic>? Diagnostics { get; }
}
