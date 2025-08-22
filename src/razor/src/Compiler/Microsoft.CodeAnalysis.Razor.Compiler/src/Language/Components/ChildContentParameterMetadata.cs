// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.Language.Components;

public sealed class ChildContentParameterMetadata : MetadataObject
{
    public static readonly ChildContentParameterMetadata Default = new();

    private ChildContentParameterMetadata()
        : base(MetadataKind.ChildContentParameter)
    {
    }

    private protected override void BuildChecksum(in Checksum.Builder builder)
    {
    }
}
