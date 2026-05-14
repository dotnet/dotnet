// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using Microsoft.Build.Utilities;
using NuGet.ProjectManagement;
using XmlUtility = NuGet.Shared.XmlUtility;

namespace NuGet.PackageManagement.VisualStudio
{
    public static class FrameworkAssemblyResolver
    {
        // (dotNetFrameworkVersion + dotNetFrameworkProfile) is the key
        private static readonly ConcurrentDictionary<string, List<FrameworkAssembly>> FrameworkAssembliesDictionary = new ConcurrentDictionary<string, List<FrameworkAssembly>>();
        private const string NETFrameworkIdentifier = ".NETFramework";
        private const string NETFrameworkFacadesDirectoryName = "Facades";
        internal const string FrameworkListFileName = "RedistList\\FrameworkList.xml";

        /// <summary>
        /// This function checks if there is a framework assembly of assemblyName and of version > availableVersion
        /// in targetFramework. NOTE that this function is only applicable for .NETFramework and returns false for
        /// all the other targetFrameworks
        /// </summary>
        public static bool IsHigherAssemblyVersionInFramework(string simpleAssemblyName, Version availableVersion, FrameworkName targetFrameworkName)
        {
            return IsHigherAssemblyVersionInFramework(simpleAssemblyName, availableVersion, targetFrameworkName,
                ToolLocationHelper.GetPathToReferenceAssemblies, FrameworkAssembliesDictionary);
        }

        /// <summary>
        /// This function checks if there is a framework assembly of assemblyName and of version > availableVersion
        /// in targetFramework. NOTE that this function is only applicable for .NETFramework and returns false for
        /// all the other targetFrameworks
        /// This is non-private only to facilitate unit testing.
        /// </summary>
        internal static bool IsHigherAssemblyVersionInFramework(string simpleAssemblyName,
            Version availableVersion,
            FrameworkName targetFrameworkName,
            Func<FrameworkName, IList<string>> getPathToReferenceAssembliesFunc,
            ConcurrentDictionary<string, List<FrameworkAssembly>> frameworkAssembliesDictionary)
        {
            if (string.IsNullOrEmpty(simpleAssemblyName))
            {
                throw new ArgumentException(Strings.Argument_Cannot_Be_Null_Or_Empty, nameof(simpleAssemblyName));
            }

            if (availableVersion == null)
            {
                throw new ArgumentNullException(nameof(availableVersion));
            }

            if (targetFrameworkName == null)
            {
                throw new ArgumentNullException(nameof(targetFrameworkName));
            }

            if (getPathToReferenceAssembliesFunc == null)
            {
                throw new ArgumentNullException(nameof(getPathToReferenceAssembliesFunc));
            }

            if (frameworkAssembliesDictionary == null)
            {
                throw new ArgumentNullException(nameof(frameworkAssembliesDictionary));
            }

            if (!string.Equals(targetFrameworkName.Identifier, NETFrameworkIdentifier, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var dotNetFrameworkVersion = targetFrameworkName.Version + targetFrameworkName.Profile;

            if (!frameworkAssembliesDictionary.ContainsKey(dotNetFrameworkVersion))
            {
                var frameworkListFiles = getPathToReferenceAssembliesFunc(targetFrameworkName);
                var frameworkAssemblies = GetFrameworkAssemblies(frameworkListFiles);

                frameworkAssembliesDictionary.AddOrUpdate(dotNetFrameworkVersion, frameworkAssemblies, (d, f) => frameworkAssemblies);
            }

            // Find a frameworkAssembly with the same name as assemblyName.
            // If one exists, see if its version is greater than that of the available version.
            return frameworkAssembliesDictionary[dotNetFrameworkVersion].Any(p =>
                string.Equals(p.AssemblyName.Name, simpleAssemblyName, StringComparison.OrdinalIgnoreCase)
                && p.AssemblyName.Version >= availableVersion);
        }

        /// <summary>
        /// Determines if there is a facade with the simple assembly name for the given target .NET Framework.
        /// </summary>
        /// <param name="simpleAssemblyName">The simple assembly name (e.g.:  System.Runtime) without file extension.</param>
        /// <param name="targetFrameworkName">The target .NET Framework.</param>
        /// <returns><see langword="true" /> if the assembly is a .NET Framework facade assembly; otherwise, <see langword="false" />.</returns>
        public static bool IsFrameworkFacade(string simpleAssemblyName, FrameworkName targetFrameworkName)
        {
            return IsFrameworkFacade(simpleAssemblyName, targetFrameworkName,
                ToolLocationHelper.GetPathToReferenceAssemblies, FrameworkAssembliesDictionary);
        }

        /// <summary>
        /// Determines if there is a facade with the simple assembly name for the given target .NET Framework.
        /// This is non-private only to facilitate unit testing.
        /// </summary>
        /// <param name="simpleAssemblyName">The simple assembly name (e.g.:  System.Runtime) without file extension.</param>
        /// <param name="targetFrameworkName">The target .NET Framework.</param>
        /// <param name="getPathToReferenceAssembliesFunc">A function that returns .NET Framework reference assemblies directories.</param>
        /// <param name="frameworkAssembliesDictionary">A dictionary mapping frameworks to lists of assemblies.
        /// The dictionary may be mutated by this function.</param>
        /// <returns><see langword="true" /> if the assembly is a .NET Framework facade assembly; otherwise, <see langword="false" />.</returns>
        internal static bool IsFrameworkFacade(string simpleAssemblyName,
            FrameworkName targetFrameworkName,
            Func<FrameworkName, IList<string>> getPathToReferenceAssembliesFunc,
            ConcurrentDictionary<string, List<FrameworkAssembly>> frameworkAssembliesDictionary)
        {
            if (string.IsNullOrEmpty(simpleAssemblyName))
            {
                throw new ArgumentException(Strings.Argument_Cannot_Be_Null_Or_Empty, nameof(simpleAssemblyName));
            }

            if (targetFrameworkName == null)
            {
                throw new ArgumentNullException(nameof(targetFrameworkName));
            }

            if (getPathToReferenceAssembliesFunc == null)
            {
                throw new ArgumentNullException(nameof(getPathToReferenceAssembliesFunc));
            }

            if (frameworkAssembliesDictionary == null)
            {
                throw new ArgumentNullException(nameof(frameworkAssembliesDictionary));
            }

            if (!string.Equals(targetFrameworkName.Identifier, NETFrameworkIdentifier, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var dotNetFrameworkVersion = targetFrameworkName.Version + targetFrameworkName.Profile;

            if (!frameworkAssembliesDictionary.ContainsKey(dotNetFrameworkVersion))
            {
                var frameworkListFiles = getPathToReferenceAssembliesFunc(targetFrameworkName);
                var frameworkAssemblies = GetFrameworkAssemblies(frameworkListFiles);

                frameworkAssembliesDictionary.AddOrUpdate(dotNetFrameworkVersion, frameworkAssemblies, (d, f) => frameworkAssemblies);
            }

            return frameworkAssembliesDictionary[dotNetFrameworkVersion].Any(p =>
                p.IsFacade && string.Equals(p.AssemblyName.Name, simpleAssemblyName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns the list of framework assemblies as specified in  RedistList\FrameworkList.xml under the
        /// ReferenceAssemblies directory for .NETFramework. If the file is not present, an empty list is returned.
        /// </summary>
        private static List<FrameworkAssembly> GetFrameworkAssemblies(IList<string> referenceAssembliesPaths)
        {
            var frameworkAssemblies = new List<FrameworkAssembly>();

            foreach (var referenceAssembliesPath in referenceAssembliesPaths)
            {
                if (!string.IsNullOrEmpty(referenceAssembliesPath))
                {
                    frameworkAssemblies.AddRange(GetFrameworkAssemblies(referenceAssembliesPath));
                }
            }

            return frameworkAssemblies;
        }

        /// <summary>
        /// Given a file system path containing RedistList\FrameworkList.xml file
        /// return the list of assembly names read out from the XML file.
        /// </summary>
        private static List<FrameworkAssembly> GetFrameworkAssemblies(string referenceAssembliesPath)
        {
            var frameworkAssemblies = new List<FrameworkAssembly>();

            try
            {
                if (FileSystemUtility.FileExists(referenceAssembliesPath, FrameworkListFileName))
                {
                    var facadeNames = GetFrameworkFacadeNames(referenceAssembliesPath);

                    using (var stream = File.OpenRead(FileSystemUtility.GetFullPath(referenceAssembliesPath, FrameworkListFileName)))
                    {
                        var document = XmlUtility.Load(stream);
                        var root = document.Root;
                        if (root.Name.LocalName.Equals("FileList", StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (var element in root.Elements("File"))
                            {
                                var simpleAssemblyName = element.GetOptionalAttributeValue("AssemblyName");
                                var version = element.GetOptionalAttributeValue("Version");
                                if (simpleAssemblyName == null
                                    || version == null)
                                {
                                    // Skip this file. Return an empty list
                                    // Clear frameworkAssemblies since we don't want partial results
                                    frameworkAssemblies.Clear();
                                    break;
                                }

                                var assemblyName = new AssemblyName();
                                assemblyName.Name = simpleAssemblyName;
                                assemblyName.Version = new Version(version);

                                var isFacade = facadeNames.Contains(simpleAssemblyName);
                                var frameworkAssembly = new FrameworkAssembly(assemblyName, isFacade);

                                frameworkAssemblies.Add(frameworkAssembly);
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return frameworkAssemblies;
        }

        private static ISet<string> GetFrameworkFacadeNames(string frameworkDirectory)
        {
            var facadeNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (!FileSystemUtility.DirectoryExists(frameworkDirectory, NETFrameworkFacadesDirectoryName))
            {
                return facadeNames;
            }

            foreach (var facadeName in FileSystemUtility.GetFiles(frameworkDirectory, NETFrameworkFacadesDirectoryName, "*.dll", recursive: false)
                                                        .Select(filePath => Path.GetFileNameWithoutExtension(filePath)))
            {
                facadeNames.Add(facadeName);
            }

            return facadeNames;
        }
    }
}
