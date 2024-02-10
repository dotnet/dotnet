// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Razor.Serialization.MessagePack.Resolvers;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal static class RazorServiceDescriptors
{
    private const string ComponentName = "Razor";

    public static readonly RazorServiceDescriptorsWrapper TagHelperProviderServiceDescriptors = new(
        ComponentName,
        featureDisplayNameProvider: _ => "Razor TagHelper Provider",
        additionalFormatters: ImmutableArray<IMessagePackFormatter>.Empty,
        additionalResolvers: TopLevelResolvers.All,
        interfaces: new (Type, Type?)[] { (typeof(IRemoteTagHelperProviderService), null) });
}
