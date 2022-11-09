using NuGet.Packaging.Core;

namespace NuGet.CommandLine.Test.Caching
{
    public class RestorePackagesConfigCommand : ICachingCommand
    {
        public string Description => "Executes a nuget.exe restore on a packages.config";

        public string GetInstalledPackagePath(CachingTestContext context, PackageIdentity identity)
        {
            return context.GetPackagePathInOutputDirectory(identity);
        }

        public bool IsPackageInstalled(CachingTestContext context, PackageIdentity identity)
        {
            return context.IsPackageInOutputDirectory(identity);
        }

        public string PrepareArguments(CachingTestContext context, PackageIdentity identity)
        {
            context.WritePackagesConfig(identity);

            var args = $"restore {context.PackagesConfigPath} -PackagesDirectory {context.OutputPackagesPath}";

            return context.FinishArguments(args);
        }
    }
}
