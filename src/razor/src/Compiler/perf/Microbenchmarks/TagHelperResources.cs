// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Reflection;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization.Json;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

internal static class TagHelperResources
{
    private const string ResourceNameBase = "Microsoft.AspNetCore.Razor.Microbenchmarks.Compiler.Resources";
    private const string BlazorServerAppResourceName = $"{ResourceNameBase}.BlazorServerApp.TagHelpers.json";
    private const string TelerikMvcResourceName = $"{ResourceNameBase}.Telerik.Kendo.Mvc.Examples.taghelpers.json";

    private static readonly Lazy<ImmutableArray<TagHelperDescriptor>> s_lazyBlazorServerApp = new(() => ReadTagHelpersFromResource(BlazorServerAppResourceName));
    private static readonly Lazy<ImmutableArray<TagHelperDescriptor>> s_lazyTelerikMvc = new(() => ReadTagHelpersFromResource(TelerikMvcResourceName));

    private static ImmutableArray<TagHelperDescriptor> ReadTagHelpersFromResource(string resourceName)
    {
        using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        Assumed.NotNull(resourceStream);

        var length = (int)resourceStream.Length;
        var bytes = new byte[length];
        resourceStream.ReadExactly(bytes.AsSpan(0, length));

        return JsonDataConvert.DeserializeTagHelperArray(bytes);
    }

    public static ImmutableArray<TagHelperDescriptor> BlazorServerApp => s_lazyBlazorServerApp.Value;
    public static ImmutableArray<TagHelperDescriptor> TelerikMvc => s_lazyTelerikMvc.Value;
}
