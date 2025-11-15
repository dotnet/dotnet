// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

internal static partial class Resources
{
    internal static class Tooling
    {
        private const string Prefix = $"{Resources.Prefix}.Tooling";

        private static ImmutableArray<TagHelperDescriptor>? s_blazorServerApp;
        private static ImmutableArray<TagHelperDescriptor>? s_telerikMvc;
        private static ImmutableArray<TagHelperDescriptor>? s_legacy;

        public static ImmutableArray<TagHelperDescriptor> BlazorServerApp
            => s_blazorServerApp ??= ReadTagHelpersFromResource($"{Prefix}.BlazorServerApp.TagHelpers.json");

        public static ImmutableArray<TagHelperDescriptor> TelerikMvc
            => s_telerikMvc ??= ReadTagHelpersFromResource($"{Prefix}.Telerik.Kendo.Mvc.Examples.taghelpers.json");

        public static ImmutableArray<TagHelperDescriptor> Legacy
            => s_legacy ??= ReadTagHelpersFromResource($"{Prefix}.taghelpers.json");
    }
}
