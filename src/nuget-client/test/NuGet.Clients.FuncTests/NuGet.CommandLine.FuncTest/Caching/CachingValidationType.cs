namespace NuGet.CommandLine.Test.Caching
{
    public enum CachingValidationType
    {
        CommandSucceeded,
        PackageInstalled,
        PackageInGlobalPackagesFolder,
        PackageInHttpCache,
        PackageFromHttpCacheUsed,
        PackageFromSourceUsed,
        PackageFromSourceNotUsed,
        PackageFromGlobalPackagesFolderUsed,
        DirectDownloadFilesDoNotExist,
        RestoreNoOp,
    }
}
