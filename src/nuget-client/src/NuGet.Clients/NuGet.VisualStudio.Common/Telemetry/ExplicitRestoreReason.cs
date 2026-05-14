// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio.Common
{
    /// <summary>
    /// None: The reason is not explicit.
    /// MissingPackagesBanner: Explicit restore done by PackageRestoreBar
    /// RestoreSolutionPackages: Explicit restore done by menu in Solution Explorer
    /// ProjectRetargeting: Restore done when retargeting
    /// </summary>
    public enum ExplicitRestoreReason
    {
        None,
        MissingPackagesBanner,
        RestoreSolutionPackages,
        ProjectRetargeting,
    }
}
