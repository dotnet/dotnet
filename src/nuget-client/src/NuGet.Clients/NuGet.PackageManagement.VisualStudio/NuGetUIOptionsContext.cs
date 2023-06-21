// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using NuGet.Packaging.Core;

namespace NuGet.PackageManagement.VisualStudio
{
    [Export(typeof(INuGetUIOptionsContext))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class NuGetUIOptionsContext : INuGetUIOptionsContext
    {
        public PackageIdentity SelectedPackage { get; set; }
    }
}
