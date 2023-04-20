// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;
using Microsoft.VisualStudio.LanguageServices.Razor.Serialization;
using Newtonsoft.Json;

namespace Microsoft.CodeAnalysis.Razor;

internal static class JsonConverterCollectionExtensions
{
    public static JsonConverterCollection RegisterRazorConverters(this JsonConverterCollection collection)
    {
        if (collection is null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        collection.Add(ProjectSnapshotHandleJsonConverter.Instance);

        collection.RegisterProjectSerializerConverters();

        return collection;
    }
}
