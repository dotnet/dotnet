// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using MessagePack;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Resolvers;

internal static class TopLevelResolvers
{
    public static readonly ImmutableArray<IFormatterResolver> All = ImmutableArray.Create<IFormatterResolver>(
        ChecksumResolver.Instance,
        FetchTagHelpersResultResolver.Instance,
        ProjectSnapshotHandleResolver.Instance,
        RazorProjectInfoResolver.Instance,
        TagHelperDeltaResultResolver.Instance);
}
