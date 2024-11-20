using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.CommandLine
{
    /// <summary>
    /// Provides a common facility for locating extensions
    /// </summary>
    public class ExtensionLocator : IExtensionLocator
    {
        private static readonly string ExtensionsDirectoryRoot =
            Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
                "NuGet",
                "Commands");

        private static readonly string CredentialProvidersDirectoryRoot =
            Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
                "NuGet",
                "CredentialProviders");

        private const string CredentialProviderPattern = "CredentialProvider*.exe";

        public readonly static string ExtensionsEnvar = "NUGET_EXTENSIONS_PATH";
        public readonly static string CredentialProvidersEnvar = "NUGET_CREDENTIALPROVIDERS_PATH";

        /// <summary>
        /// Find paths to all extensions
        /// </summary>
        public IEnumerable<string> FindExtensions()
        {
            return FindExtensions(EnvironmentVariableWrapper.Instance);
        }

        internal static IEnumerable<string> FindExtensions(IEnvironmentVariableReader environmentVariableReader)
        {
            var customPaths = ReadPathsFromEnvar(ExtensionsEnvar, environmentVariableReader);
            return FindAll(
                ExtensionsDirectoryRoot,
                customPaths,
                "*.dll",
                "*Extensions.dll"
                );
        }

        /// <summary>
        /// Find paths to all credential providers
        /// </summary>
        public IEnumerable<string> FindCredentialProviders()
        {
            return FindCredentialProviders(EnvironmentVariableWrapper.Instance);
        }

        internal static IEnumerable<string> FindCredentialProviders(IEnvironmentVariableReader environmentVariableReader)
        {
            var customPaths = ReadPathsFromEnvar(CredentialProvidersEnvar, environmentVariableReader);
            return FindAll(
                CredentialProvidersDirectoryRoot,
                customPaths,
                CredentialProviderPattern,
                CredentialProviderPattern
                );
        }

        /// <summary>
        /// Helper method to locate extensions and credential providers.
        /// The following search locations will be checked in this order:
        /// 1) all directories under a path set by environment variable
        /// 2) all directories under a global path, e.g. %localappdata%\nuget\commands
        /// 3) the directory nuget.exe is located in
        /// </summary>
        /// <param name="globalRootDirectory">The global directory to search.  Will
        /// also check subdirectories.</param>
        /// <param name="customPaths">User-defined search paths.
        /// Will also check subdirectories.</param>
        /// <param name="assemblyPattern">The filename pattern to search for.</param>
        /// <param name="nugetDirectoryAssemblyPattern">The filename pattern to search for
        /// when looking in the nuget.exe directory. This is more restrictive so we do not
        /// accidentally pick up NuGet dlls.</param>
        /// <returns>An IEnumerable of paths to files matching the pattern in all searched
        /// directories.</returns>
        private static IEnumerable<string> FindAll(
            string globalRootDirectory,
            IEnumerable<string> customPaths,
            string assemblyPattern,
            string nugetDirectoryAssemblyPattern)
        {
            var directories = new List<string>();

            // Add all directories from the environment variable if available.
            directories.AddRange(customPaths);

            // add the global root
            directories.Add(globalRootDirectory);

            var paths = new List<string>();
            foreach (var directory in directories.Where(Directory.Exists))
            {
                paths.AddRange(Directory.EnumerateFiles(directory, assemblyPattern, SearchOption.AllDirectories));
            }

            // Add the nuget.exe directory, but be more careful since it contains non-extension assemblies.
            // Ideally we want to look for all files. However, using MEF to identify imports results in assemblies
            // being loaded and locked by our App Domain which could be slow, might affect people's build systems
            // and among other things breaks our build.
            // Consequently, we'll use a convention - only binaries ending in the name Extensions would be loaded.
            var nugetDirectory = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            if (nugetDirectory == null)
            {
                return paths;
            }

            paths.AddRange(Directory.EnumerateFiles(nugetDirectory, nugetDirectoryAssemblyPattern));

            return paths;
        }

        private static IEnumerable<string> ReadPathsFromEnvar(string key, IEnvironmentVariableReader environmentVariableReader)
        {
            var result = new List<string>();
            var paths = environmentVariableReader.GetEnvironmentVariable(key);
            if (!string.IsNullOrEmpty(paths))
            {
                result.AddRange(
                    paths.Split(new[] { Path.PathSeparator },
                    StringSplitOptions.RemoveEmptyEntries));
            }
            return result;
        }
    }
}
