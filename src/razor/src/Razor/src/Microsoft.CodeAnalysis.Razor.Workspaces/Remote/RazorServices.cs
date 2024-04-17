// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Razor.Serialization.MessagePack.Resolvers;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.CodeAnalysis.Razor.Remote;

internal static class RazorServices
{
    private const string ComponentName = "Razor";

    public static readonly RazorServiceDescriptorsWrapper Descriptors = new(
        ComponentName,
        featureDisplayNameProvider: feature => $"Razor {feature} Feature",
        additionalFormatters: ImmutableArray<IMessagePackFormatter>.Empty,
        additionalResolvers: TopLevelResolvers.All,
        interfaces:
        [
            (typeof(IRemoteTagHelperProviderService), null),
            (typeof(IRemoteClientInitializationService), null),
            (typeof(IRemoteSemanticTokensService), null),
        ]);
}
