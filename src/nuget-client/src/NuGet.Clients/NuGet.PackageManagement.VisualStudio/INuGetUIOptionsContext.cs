// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Packaging.Core;

namespace NuGet.PackageManagement.VisualStudio
{
    public interface INuGetUIOptionsContext
    {
        PackageIdentity SelectedPackage { get; }
    }
}
