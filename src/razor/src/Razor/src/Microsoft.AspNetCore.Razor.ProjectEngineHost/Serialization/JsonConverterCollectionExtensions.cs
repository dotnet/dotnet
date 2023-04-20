// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;

internal static class JsonConverterCollectionExtensions
{
    private static readonly List<JsonConverter> s_projectSerializerConverters = new List<JsonConverter>(5)
    {
        TagHelperDescriptorJsonConverter.Instance,
        RazorDiagnosticJsonConverter.Instance,
        RazorExtensionJsonConverter.Instance,
        RazorConfigurationJsonConverter.Instance,
        ProjectRazorJsonJsonConverter.Instance,
    };

    public static void RegisterProjectSerializerConverters(this JsonConverterCollection collection)
    {
        if (collection is null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        foreach (var converter in s_projectSerializerConverters)
        {
            {
                collection.Add(converter);
            }
        }
    }
}
