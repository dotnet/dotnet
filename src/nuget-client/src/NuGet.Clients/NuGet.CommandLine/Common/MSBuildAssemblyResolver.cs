// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NuGet.Common
{
    /// <summary>
    /// Represents an assembly resolver when loading MSBuild assemblies from a specific directory.
    /// </summary>
    internal sealed class MSBuildAssemblyResolver : IDisposable
    {
        private readonly Lazy<Assembly> _microsoftBuildAssemblyLazy;
        private readonly Lazy<Assembly> _microsoftBuildFrameworkAssemblyLazy;
        private readonly Lazy<Type> _projectCollectionTypeLazy;
        private readonly Lazy<Type> _projectTypeLazy;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSBuildAssemblyResolver" /> class.
        /// </summary>
        /// <param name="msbuildDirectory">The full path to the MSBuild directory.</param>
        /// <exception cref="ArgumentNullException"><paramref name="msbuildDirectory" /> is <see langword="null" /> or an empty string.</exception>
        public MSBuildAssemblyResolver(string msbuildDirectory)
        {
            if (string.IsNullOrEmpty(msbuildDirectory))
            {
                throw new ArgumentNullException(nameof(msbuildDirectory));
            }

            MSBuildDirectory = msbuildDirectory.TrimEnd(Path.DirectorySeparatorChar);

            if (msbuildDirectory.EndsWith("MSBuild.exe", StringComparison.OrdinalIgnoreCase))
            {
                MSBuildDirectory = Path.GetDirectoryName(msbuildDirectory);
            }

            MSBuildAssemblyDirectory = MSBuildDirectory;

            // 64-bit MSBuild is under an "amd64" folder but dependencies are in the parent folder.
            // Use the parent folder if the path to MSBuild ends in amd64 or arm64 so that the assemblies can be found
            foreach (string processorSpecificDir in new[] { "amd64", "arm64" })
            {
                if (MSBuildAssemblyDirectory.EndsWith(processorSpecificDir))
                {
                    string parentDirectoryName = Path.GetDirectoryName(MSBuildAssemblyDirectory);

                    if (string.IsNullOrWhiteSpace(parentDirectoryName))
                    {
                        continue;
                    }

                    MSBuildAssemblyDirectory = parentDirectoryName;

                    break;
                }
            }

            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;

            _microsoftBuildAssemblyLazy = new Lazy<Assembly>(() => LoadMSBuildAssembly("Microsoft.Build"));
            _microsoftBuildFrameworkAssemblyLazy = new Lazy<Assembly>(() => LoadMSBuildAssembly("Microsoft.Build.Framework"));

            _projectTypeLazy = new Lazy<Type>(() => _microsoftBuildAssemblyLazy.Value.GetType("Microsoft.Build.Evaluation.Project", throwOnError: true));
            _projectCollectionTypeLazy = new Lazy<Type>(() => _microsoftBuildAssemblyLazy.Value.GetType("Microsoft.Build.Evaluation.ProjectCollection", throwOnError: true));
        }

        /// <summary>
        /// Gets an <see cref="Assembly" /> representing the Microsoft.Build.dll assembly.
        /// </summary>
        public Assembly MicrosoftBuildAssembly => _microsoftBuildAssemblyLazy.Value;

        /// <summary>
        /// Gets the full path to the directory containing MSBuild assemblies.
        /// </summary>
        public string MSBuildAssemblyDirectory { get; private set; }

        /// <summary>
        /// Gets the full path to the directory containing MSBuild.exe.
        /// </summary>
        public string MSBuildDirectory { get; private set; }

        /// <summary>
        /// Gets the <see cref="Type" /> of <c>Microsoft.Build.Evaluation.ProjectCollection</c>.
        /// </summary>
        public Type ProjectCollectionType => _projectCollectionTypeLazy.Value;

        /// <summary>
        /// Gets the <see cref="Type" /> of <c>Microsoft.Build.Evaluation.Project</c>.
        /// </summary>
        public Type ProjectType => _projectTypeLazy.Value;

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
        }

        /// <summary>
        /// Loads an MSBuild assembly.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly to load, without the extension.</param>
        /// <returns>An <see cref="Assembly" /> instance representing the specified assembly.</returns>
        /// <exception cref="FileNotFoundException">An assembly with the specified name could not be found.</exception>
        private Assembly LoadMSBuildAssembly(string assemblyName)
        {
            if (!TryLoadMSBuildAssembly(assemblyName, out Assembly assembly))
            {
                throw new FileNotFoundException(message: null, Path.Combine(MSBuildAssemblyDirectory, assemblyName + ".dll"));
            }

            return assembly;
        }

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName assemblyName = new(args.Name);

            string assemblyNameString = assemblyName.Name;

            TryLoadMSBuildAssembly(assemblyNameString, out Assembly assembly);

            return assembly;
        }

        /// <summary>
        /// Attempts to load an MSBuild assembly from the currently configured location.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly to load, without the extension.</param>
        /// <param name="assembly">Receives an <see cref="Assembly" /> instance representing the specified assembly if one was found, otherwise <see langword="null" />.</param>
        /// <returns><see langword="true" /> if the specified assembly was loaded, otherwise <see langword="false" />.</returns>
        private bool TryLoadMSBuildAssembly(string assemblyName, out Assembly assembly)
        {
            assembly = null;

            if (string.IsNullOrEmpty(MSBuildAssemblyDirectory))
            {
                return false;
            }

            string assemblyDirectory = MSBuildAssemblyDirectory;

            // Look for resource assemblies under a subfolder for the current culture and fallback to "en"
            if (assemblyName.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
            {
                string cultureSpecificDirectory = new[] {
                    Path.Combine(assemblyDirectory, CultureInfo.CurrentCulture.TwoLetterISOLanguageName),
                    Path.Combine(assemblyDirectory, "en") }
                    .FirstOrDefault(Directory.Exists);

                if (!string.IsNullOrWhiteSpace(cultureSpecificDirectory))
                {
                    assemblyDirectory = cultureSpecificDirectory;
                }
            }

            string candidateAssemblyPath = Path.Combine(assemblyDirectory, assemblyName + ".dll");

            if (File.Exists(candidateAssemblyPath))
            {
                assembly = Assembly.LoadFrom(candidateAssemblyPath);

                return true;
            }

            return false;
        }
    }
}
