// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.Serialization.Converters;

internal static class JsonConverterCollectionExtensions
{
    private static readonly ImmutableArray<JsonConverter> s_converters = ImmutableArray.CreateRange(
        new JsonConverter[]
        {
            ChecksumJsonConverter.Instance,
            ProjectRazorJsonJsonConverter.Instance,
            ProjectSnapshotHandleJsonConverter.Instance,
            TagHelperDeltaResultJsonConverter.Instance,
        });

    public static void RegisterRazorConverters(this ICollection<JsonConverter> collection)
    {
        if (collection is null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        foreach (var converter in s_converters)
        {
            collection.Add(converter);
        }
    }
}
