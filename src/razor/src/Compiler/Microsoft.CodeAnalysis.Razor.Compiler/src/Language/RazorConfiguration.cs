// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.Language;

public sealed record class RazorConfiguration(
    RazorLanguageVersion LanguageVersion,
    string ConfigurationName,
    ImmutableArray<RazorExtension> Extensions,
    bool UseConsolidatedMvcViews = true,
    bool SuppressAddComponentParameter = false,
    LanguageServerFlags? LanguageServerFlags = null)
{
    public static readonly RazorConfiguration Default = new(
        RazorLanguageVersion.Latest,
        ConfigurationName: "unnamed",
        Extensions: [],
        UseConsolidatedMvcViews: true,
        SuppressAddComponentParameter: false,
        LanguageServerFlags: null);

    public bool Equals(RazorConfiguration? other)
        => other is not null &&
           LanguageVersion == other.LanguageVersion &&
           ConfigurationName == other.ConfigurationName &&
           SuppressAddComponentParameter == other.SuppressAddComponentParameter &&
           LanguageServerFlags == other.LanguageServerFlags &&
           UseConsolidatedMvcViews == other.UseConsolidatedMvcViews &&
           Extensions.SequenceEqual(other.Extensions);

    public override int GetHashCode()
    {
        var hash = HashCodeCombiner.Start();
        hash.Add(LanguageVersion);
        hash.Add(ConfigurationName);
        hash.Add(Extensions);
        hash.Add(SuppressAddComponentParameter);
        hash.Add(UseConsolidatedMvcViews);
        hash.Add(LanguageServerFlags);
        return hash;
    }
}
