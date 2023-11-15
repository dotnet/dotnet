// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Build.Tasks.Workloads.Wix
{
    /// <summary>
    /// Defines WiX preprocessor variable names.
    /// </summary>
    public static class PreprocessorDefinitionNames
    {
        public static readonly string DependencyProviderKeyName = nameof(DependencyProviderKeyName);
        public static readonly string EulaRtf = nameof(EulaRtf);
        public static readonly string InstallDir = nameof(InstallDir);
        public static readonly string InstallationRecordKey = nameof(InstallationRecordKey);
        public static readonly string ManifestId = nameof(ManifestId);
        public static readonly string Manufacturer = nameof(Manufacturer);
        public static readonly string PackKind = nameof(PackKind);
        public static readonly string PackageId = nameof(PackageId);
        public static readonly string PackageVersion = nameof(PackageVersion);
        public static readonly string Platform = nameof(Platform);
        public static readonly string ProductCode = nameof(ProductCode);
        public static readonly string ProductName = nameof(ProductName);
        public static readonly string ProductVersion = nameof(ProductVersion);
        public static readonly string SdkFeatureBandVersion = nameof(SdkFeatureBandVersion);
        public static readonly string SourceDir = nameof(SourceDir);
        public static readonly string UpgradeCode = nameof(UpgradeCode);
        public static readonly string WorkloadSetVersion = nameof(WorkloadSetVersion);
    }
}
