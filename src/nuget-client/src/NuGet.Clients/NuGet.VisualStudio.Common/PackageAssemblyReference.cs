using System.IO;

namespace NuGet.PackageManagement.VisualStudio
{
    internal class PackageAssemblyReference : IPackageAssemblyReference
    {
        private string _path;

        public PackageAssemblyReference(string path)
        {
            _path = path;
        }

        public string Name
        {
            get
            {
                return Path.GetFileName(_path);
            }
        }
    }
}
