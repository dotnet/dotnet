// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Serialization.Json;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Newtonsoft.Json;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal static class RazorServiceDescriptors
{
    private const string ComponentName = "Razor";

    private static readonly ImmutableArray<JsonConverter> s_jsonConverters = GetJsonConverters();

    private static ImmutableArray<JsonConverter> GetJsonConverters()
    {
        var builder = ImmutableArray.CreateBuilder<JsonConverter>();

        builder.RegisterRazorConverters();

        return builder.ToImmutableArray();
    }

    public static readonly RazorServiceDescriptorsWrapper TagHelperProviderServiceDescriptors = new(
        ComponentName,
        featureDisplayNameProvider: _ => "Razor TagHelper Provider",
        s_jsonConverters,
        interfaces: new (Type, Type?)[] { (typeof(IRemoteTagHelperProviderService), null) });
}
