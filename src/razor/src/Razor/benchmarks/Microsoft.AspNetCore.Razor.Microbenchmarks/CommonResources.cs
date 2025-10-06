// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization.Json;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

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
        => JsonDataConvert.DeserializeTagHelperArray(bytes);

    private static RazorProjectInfo LoadProjectInfo(byte[] bytes)
        => JsonDataConvert.DeserializeProjectInfo(bytes);
}
