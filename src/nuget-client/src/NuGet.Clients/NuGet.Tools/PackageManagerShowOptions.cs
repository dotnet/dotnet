// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.PackageManagement.UI;

namespace NuGetVSExtension
{
    internal record PackageManagerShowOptions
    {
        public string? SearchText { get; set; }
        public ItemFilter? ItemFilter { get; set; }
        public PackageFilterOptions? PackageFilterOptions { get; set; }
    }
}
