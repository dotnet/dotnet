// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Mvc.Razor.Extensions;

public sealed record ViewComponentMetadata() : MetadataObject(MetadataKind.ViewComponent)
{
    public required string Name { get; init; }

    internal override bool HasDefaultValue => false;

    private protected override void BuildChecksum(in Checksum.Builder builder)
    {
        builder.AppendData(Name);
    }

    public ref struct Builder
    {
        public string? Name { get; set; }

        public readonly ViewComponentMetadata Build()
            => new()
            {
                Name = Name.AssumeNotNull()
            };
    }
}
