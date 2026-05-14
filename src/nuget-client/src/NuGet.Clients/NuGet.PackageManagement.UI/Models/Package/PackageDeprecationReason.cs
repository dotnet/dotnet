// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement.UI.Models.Package
{
    public enum PackageDeprecationReason
    {
        Unknown = -1,
        Legacy = 0,
        CriticalBugs = 1,
        LegacyAndCriticalBugs = 2
    }

    public static class PackageDeprecationReasonConstants
    {
        public const string CriticalBugs = nameof(CriticalBugs);
        public const string Legacy = nameof(Legacy);
    }
}
