// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;
using Microsoft.CodeAnalysis.Razor.Workspaces.Serialization;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;

internal static class JsonConverterCollectionExtensions
{
    private static readonly IReadOnlyList<JsonConverter> s_razorConverters = new List<JsonConverter>()
    {
        RazorUriJsonConverter.Instance,
    };

    public static void RegisterRazorConverters(this JsonConverterCollection collection)
    {
        if (collection is null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        for (var i = 0; i < s_razorConverters.Count; i++)
        {
            collection.Add(s_razorConverters[i]);
        }

        collection.RegisterProjectSerializerConverters();
    }
}
