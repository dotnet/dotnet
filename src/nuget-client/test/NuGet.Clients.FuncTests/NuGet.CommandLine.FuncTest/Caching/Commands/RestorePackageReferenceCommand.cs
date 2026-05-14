using NuGet.Packaging.Core;

namespace NuGet.CommandLine.Test.Caching
{
    public class RestorePackageReferenceCommand : ICachingCommand
    {
        public string Description => "Executes a nuget.exe restore on a project.json";

        public string GetInstalledPackagePath(CachingTestContext context, PackageIdentity identity)
        {
            return context.GetPackagePathInGlobalPackagesFolder(identity);
        }

        public bool IsPackageInstalled(CachingTestContext context, PackageIdentity identity)
        {
            return context.IsPackageInGlobalPackagesFolder(identity);
        }

        public string PrepareArguments(CachingTestContext context, PackageIdentity identity)
        {
            context.WriteProject(identity);

            var args = $"restore {context.ProjectPath} -verbosity detailed";

            return context.FinishArguments(args);
        }
    }
}
