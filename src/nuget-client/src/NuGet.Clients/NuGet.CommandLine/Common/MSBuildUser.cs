using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NuGet.Common
{
    public abstract class MSBuildUser
    {
        // the Microsoft.Build.dll assembly
        protected Assembly _msbuildAssembly;

        // the Microsoft.Build.Framework.dll assembly
        protected Assembly _frameworkAssembly;

        // The type of class Microsoft.Build.Evaluation.Project
        protected Type _projectType;

        // The type of class Microsoft.Build.Evaluation.ProjectCollection
        protected Type _projectCollectionType;

        protected string _msbuildDirectory;

        // msbuildDirectory is the directory containing the msbuild to be used. E.g. C:\Program Files (x86)\MSBuild\15.0\Bin
        public void LoadAssemblies(string msbuildDirectory)
        {
            if (string.IsNullOrEmpty(msbuildDirectory))
            {
                throw new ArgumentNullException(nameof(msbuildDirectory));
            }

            string microsoftBuildDllPath = Path.Combine(msbuildDirectory, "Microsoft.Build.dll");

            if (!File.Exists(microsoftBuildDllPath))
            {
                throw new FileNotFoundException(message: null, microsoftBuildDllPath);
            }

            _msbuildDirectory = msbuildDirectory;
            _msbuildAssembly = Assembly.LoadFrom(microsoftBuildDllPath);
            _frameworkAssembly = Assembly.LoadFrom(Path.Combine(msbuildDirectory, "Microsoft.Build.Framework.dll"));

            LoadTypes();
        }

        public void LoadTypes()
        {
            _projectType = _msbuildAssembly.GetType(
                "Microsoft.Build.Evaluation.Project",
                throwOnError: true);
            _projectCollectionType = _msbuildAssembly.GetType(
                "Microsoft.Build.Evaluation.ProjectCollection",
                throwOnError: true);
        }

        // This handler is called only when the common language runtime tries to bind to the assembly and fails
        protected Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (string.IsNullOrEmpty(_msbuildDirectory))
            {
                return null;
            }

            var failingAssemblyFilename = args
                .Name
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();

            var assemblyPath = string.Empty;
            var resourceDir = string.Empty;

            // If we're failing to load a resource assembly, we need to find it in the appropriate subdir
            if (failingAssemblyFilename.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
            {
                resourceDir = new[] {
                    Path.Combine(_msbuildDirectory, CultureInfo.CurrentCulture.TwoLetterISOLanguageName),
                    Path.Combine(_msbuildDirectory, "en") }
                    .FirstOrDefault(d => Directory.Exists(d));
            }
            else
            {
                // Non-resource DLL - attempt to load from MSBuild directory
                resourceDir = _msbuildDirectory;
            }

            if (string.IsNullOrEmpty(resourceDir))
            {
                return null; // no resource directory or fallback-to-en resource directory - fail
            }

            assemblyPath = Path.Combine(resourceDir, failingAssemblyFilename + ".dll");

            if (!File.Exists(assemblyPath))
            {
                return null; // no dll present - fail
            }

            return Assembly.LoadFrom(assemblyPath);
        }
    }
}
