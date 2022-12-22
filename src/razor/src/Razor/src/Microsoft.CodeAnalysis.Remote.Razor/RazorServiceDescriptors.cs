// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor;
using Newtonsoft.Json;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal static class RazorServiceDescriptors
{
    private const string ComponentName = "Razor";

    private static readonly ImmutableArray<JsonConverter> s_jsonConverters = new JsonConverterCollection()
        .RegisterRazorConverters()
        .ToImmutableArray();

    public static readonly RazorServiceDescriptorsWrapper TagHelperProviderServiceDescriptors = new(ComponentName, _ => "Razor TagHelper Provider", s_jsonConverters, new (Type, Type?)[] { (typeof(IRemoteTagHelperProviderService), null) });
}
