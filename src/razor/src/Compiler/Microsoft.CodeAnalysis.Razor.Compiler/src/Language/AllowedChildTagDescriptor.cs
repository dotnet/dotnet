// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.Language;

public sealed class AllowedChildTagDescriptor : TagHelperObject<AllowedChildTagDescriptor>
{
    public string Name { get; }
    public string DisplayName { get; }

    internal AllowedChildTagDescriptor(string name, string displayName, ImmutableArray<RazorDiagnostic> diagnostics)
        : base(diagnostics)
    {
        Name = name;
        DisplayName = displayName;
    }

    private protected override void BuildChecksum(in Checksum.Builder builder)
    {
        builder.AppendData(Name);
        builder.AppendData(DisplayName);
    }

    public override string ToString()
        => DisplayName ?? base.ToString()!;
}
