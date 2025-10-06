// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.UI.ViewModels
{
    public interface IPackageManagerControlViewModel
    {
        ItemFilter ActiveFilter { get; set; }
        bool IsSolution { get; }
        bool IsReadmeTabEnabled { get; }
    }
}
