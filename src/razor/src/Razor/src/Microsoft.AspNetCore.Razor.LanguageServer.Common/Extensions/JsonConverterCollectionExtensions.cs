// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Razor.Serialization;
using Microsoft.CodeAnalysis.Razor.Workspaces.Serialization;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;

internal static class JsonConverterCollectionExtensions
{
    public static readonly IReadOnlyList<JsonConverter> RazorConverters = new List<JsonConverter>()
    {
        TagHelperDescriptorJsonConverter.Instance,
        RazorDiagnosticJsonConverter.Instance,
        RazorExtensionJsonConverter.Instance,
        RazorConfigurationJsonConverter.Instance,
        ProjectRazorJsonJsonConverter.Instance,
        RazorUriJsonConverter.Instance,
    };

    public static void RegisterRazorConverters(this IList<JsonConverter> collection)
    {
        if (collection is null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        for (var i = 0; i < RazorConverters.Count; i++)
        {
            collection.Add(RazorConverters[i]);
        }
    }
}
