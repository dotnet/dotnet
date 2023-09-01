// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using MessagePack.Resolvers;
using MessagePack;

namespace Microsoft.AspNetCore.Razor.Serialization.MessagePack.Resolvers;

internal static class RazorResolvers
{
    public static readonly IFormatterResolver All = CompositeResolver.Create(
        ChecksumResolver.Instance,
        RazorProjectInfoResolver.Instance,
        ProjectSnapshotHandleResolver.Instance,
        TagHelperDeltaResultResolver.Instance,
        StandardResolver.Instance);
}
