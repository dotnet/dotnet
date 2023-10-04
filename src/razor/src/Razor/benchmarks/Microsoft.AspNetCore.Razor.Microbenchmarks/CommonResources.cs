// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.IO;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Serialization.Json;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

internal static class CommonResources
{
    public static readonly byte[] LegacyTagHelperJsonBytes = Resources.GetResourceBytes("taghelpers.json");
    public static readonly ImmutableArray<TagHelperDescriptor> LegacyTagHelpers = LoadTagHelpers(LegacyTagHelperJsonBytes);

    public static readonly byte[] LegacyProjectInfoJsonBytes = Resources.GetResourceBytes("project.razor.json");
    public static readonly RazorProjectInfo LegacyProjectInfo = LoadProjectInfo(LegacyProjectInfoJsonBytes);

    public static readonly byte[] TelerikTagHelperJsonBytes = Resources.GetResourceBytes("Kendo.Mvc.Examples.taghelpers.json", folder: "Telerik");
    public static readonly ImmutableArray<TagHelperDescriptor> TelerikTagHelpers = LoadTagHelpers(TelerikTagHelperJsonBytes);

    public static readonly byte[] TelerikProjectInfoJsonBytes = Resources.GetResourceBytes("Kendo.Mvc.Examples.project.razor.json", folder: "Telerik");
    public static readonly RazorProjectInfo TelerikProjectInfo = LoadProjectInfo(TelerikProjectInfoJsonBytes);

    private static ImmutableArray<TagHelperDescriptor> LoadTagHelpers(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        using var reader = new StreamReader(stream);

        return JsonDataConvert.DeserializeData(reader,
            static r => r.ReadImmutableArray(
                static r => ObjectReaders.ReadTagHelper(r, useCache: false))).NullToEmpty();
    }

    private static RazorProjectInfo LoadProjectInfo(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        using var reader = new StreamReader(stream);

        return JsonDataConvert.DeserializeData(reader,
            static r => r.ReadNonNullObject(ObjectReaders.ReadProjectInfoFromProperties));
    }
}
