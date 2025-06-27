// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using MessagePack;

namespace Microsoft.CodeAnalysis.Razor.Serialization.MessagePack.Resolvers;

internal static class TopLevelResolvers
{
    public static readonly ImmutableArray<IFormatterResolver> All = ImmutableArray.Create<IFormatterResolver>(
        ChecksumResolver.Instance,
        FetchTagHelpersResultResolver.Instance,
        ProjectSnapshotHandleResolver.Instance,
        RazorProjectInfoResolver.Instance,
        TagHelperDeltaResultResolver.Instance);
}
