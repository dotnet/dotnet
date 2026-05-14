// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;

public class Product
{
    public static readonly string? Version = GetProductVersion();

    private static string? GetProductVersion()
    {
        var attr = typeof(Product)
            .Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        return attr?.InformationalVersion;
    }
}
