// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Utilities;

namespace Microsoft.AspNetCore.Razor.Language;

public sealed class RequiredAttributeDescriptor : TagHelperObject<RequiredAttributeDescriptor>
{
    public string Name { get; }
    public NameComparisonMode NameComparison { get; }
    public string? Value { get; }
    public ValueComparisonMode ValueComparison { get; }
    public string DisplayName { get; }
    public bool CaseSensitive { get; }

    public MetadataCollection Metadata { get; }

    internal RequiredAttributeDescriptor(
        string name,
        NameComparisonMode nameComparison,
        bool caseSensitive,
        string? value,
        ValueComparisonMode valueComparison,
        string displayName,
        ImmutableArray<RazorDiagnostic> diagnostics,
        MetadataCollection metadata)
        : base(diagnostics)
    {
        Name = name;
        NameComparison = nameComparison;
        CaseSensitive = caseSensitive;
        Value = value;
        ValueComparison = valueComparison;
        DisplayName = displayName;
        Metadata = metadata ?? MetadataCollection.Empty;
    }

    private protected override void BuildChecksum(in Checksum.Builder builder)
    {
        builder.AppendData(Name);
        builder.AppendData((int)NameComparison);
        builder.AppendData(Value);
        builder.AppendData((int)ValueComparison);
        builder.AppendData(DisplayName);
        builder.AppendData(CaseSensitive);
        builder.AppendData(Metadata.Checksum);
    }

    public override string ToString()
    {
        return DisplayName ?? base.ToString()!;
    }

    /// <summary>
    /// Acceptable <see cref="Name"/> comparison modes.
    /// </summary>
    public enum NameComparisonMode
    {
        /// <summary>
        /// HTML attribute name case insensitively matches <see cref="Name"/>.
        /// </summary>
        FullMatch,

        /// <summary>
        /// HTML attribute name case insensitively starts with <see cref="Name"/>.
        /// </summary>
        PrefixMatch,
    }

    /// <summary>
    /// Acceptable <see cref="Value"/> comparison modes.
    /// </summary>
    public enum ValueComparisonMode
    {
        /// <summary>
        /// HTML attribute value always matches <see cref="Value"/>.
        /// </summary>
        None,

        /// <summary>
        /// HTML attribute value case sensitively matches <see cref="Value"/>.
        /// </summary>
        FullMatch,

        /// <summary>
        /// HTML attribute value case sensitively starts with <see cref="Value"/>.
        /// </summary>
        PrefixMatch,

        /// <summary>
        /// HTML attribute value case sensitively ends with <see cref="Value"/>.
        /// </summary>
        SuffixMatch,
    }
}
