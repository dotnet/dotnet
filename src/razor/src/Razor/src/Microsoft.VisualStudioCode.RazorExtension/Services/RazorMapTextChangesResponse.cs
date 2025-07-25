﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

internal sealed class RazorMapTextChangesResponse
{
    [JsonPropertyName("razorDocument")]
    public required TextDocumentIdentifier RazorDocument { get; set; }

    [JsonPropertyName("mappedTextChanges")]
    public required RazorTextChange[] MappedTextChanges { get; set; }
}

