// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


namespace NuGet.ProjectManagement
{
    /// <summary>
    /// Collection of constants representing project items names.
    /// </summary>
    public static class ProjectItems
    {
        public const string FrameworkReference = nameof(FrameworkReference);
        public const string PackageDownload = nameof(PackageDownload);
        public const string PackageReference = nameof(PackageReference);
        public const string PackageVersion = nameof(PackageVersion);
        public const string ProjectReference = nameof(ProjectReference);
        public const string NuGetAuditSuppress = nameof(NuGetAuditSuppress);
        public const string PrunePackageReference = nameof(PrunePackageReference);
    }
}
