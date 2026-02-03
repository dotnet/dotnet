// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Reflection;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Serialization.Json;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

internal static partial class Resources
{
    private const string Prefix = "Microsoft.AspNetCore.Razor.Microbenchmarks.Compiler.Resources";

    private static ImmutableArray<TagHelperDescriptor> ReadTagHelpersFromResource(string resourceName)
    {
        using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        Assumed.NotNull(resourceStream);

        var length = (int)resourceStream.Length;
        var bytes = new byte[length];
        resourceStream.ReadExactly(bytes.AsSpan(0, length));

        return JsonDataConvert.DeserializeTagHelperArray(bytes);
    }
}
