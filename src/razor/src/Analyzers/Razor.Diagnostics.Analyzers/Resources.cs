// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis;

namespace Razor.Diagnostics.Analyzers;

internal partial class Resources
{
    private static readonly Type s_resourcesType = typeof(Resources);

    public static LocalizableResourceString CreateLocalizableResourceString(string nameOfLocalizableResource)
        => new(nameOfLocalizableResource, ResourceManager, s_resourcesType);

    public static LocalizableResourceString CreateLocalizableResourceString(string nameOfLocalizableResource, params string[] formatArguments)
        => new(nameOfLocalizableResource, ResourceManager, s_resourcesType, formatArguments);
}
