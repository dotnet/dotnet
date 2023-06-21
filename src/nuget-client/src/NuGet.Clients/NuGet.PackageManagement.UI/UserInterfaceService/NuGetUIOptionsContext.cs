// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.Composition;

namespace NuGet.PackageManagement.UI
{
    [Export(typeof(INuGetUIOptionsContext))]
    public sealed class NuGetUIOptionsContext : INuGetUIOptionsContext
    {
        public string SelectedPackageId { get; set; }
    }
}
