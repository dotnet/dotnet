// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.AspNetCore.Razor.LanguageServer.Common.Extensions;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin.StrongNamed.Serialization;

public static class JsonConverterCollectionExtensions
{
    public static void RegisterOmniSharpRazorConverters(this JsonConverterCollection collection)
    {
        collection.RegisterRazorConverters();
        collection.Add(OmniSharpProjectSnapshotHandleJsonConverter.Instance);
    }
}
