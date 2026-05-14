using System.Runtime.Versioning;

namespace NuGet.PackageManagement.VisualStudio
{
    public interface IScriptPackageFile
    {
        /// <summary>
        /// Gets the full path of the file inside the package.
        /// </summary>
        string Path
        {
            get;
        }

        FrameworkName TargetFramework
        {
            get;
        }
    }
}
