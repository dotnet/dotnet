// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NuGet.Frameworks;

namespace NuGet.ProjectManagement
{
    public interface IMSBuildProjectSystem
    {
        NuGetFramework TargetFramework { get; }
        string ProjectName { get; }
        string ProjectUniqueName { get; }
        string ProjectFullPath { get; }
        string ProjectFileFullPath { get; }

        INuGetProjectContext NuGetProjectContext { get; set; }

        void AddFile(string path, Stream stream);
        void AddExistingFile(string path);
        void RemoveFile(string path);
        bool FileExistsInProject(string path);

        /// <summary>
        /// Method called when adding an assembly reference to the project.
        /// </summary>
        /// <param name="referencePath">Physical path to the assembly file relative to the project root.</param>
        Task AddReferenceAsync(string referencePath);

        Task RemoveReferenceAsync(string name);

        Task<bool> ReferenceExistsAsync(string name);

        /// <summary>
        /// Adds an assembly reference to a framework assembly (one in the GAC).
        /// </summary>
        /// <param name="name">name of the assembly</param>
        Task AddFrameworkReferenceAsync(string name, string packageId);

        void AddImport(string targetFullPath, ImportLocation location);
        void RemoveImport(string targetFullPath);
        dynamic GetPropertyValue(string propertyName);
        string ResolvePath(string path);
        bool IsSupportedFile(string path);
        void AddBindingRedirects();

        Task BeginProcessingAsync();

        /// <summary>
        /// This method can be called multiple times during a batch operation in between a single BeginProcessingAsync/EndProcessingAsync calls.
        /// </summary>
        /// <param name="files">a list of files being changed.</param>
        void RegisterProcessedFiles(IEnumerable<string> files);

        Task EndProcessingAsync();

        void DeleteDirectory(string path, bool recursive);

        // The returned file names are relative paths.
        IEnumerable<string> GetFiles(string path, string filter, bool recursive);

        /// <summary>
        /// Returns the list of full paths of the files in the project that match the file name.
        /// </summary>
        /// <param name="fileName">the file name</param>
        /// <returns>The list of full paths.</returns>
        /// <remarks>We should combine GetFiles &amp; GetFullPaths into one method.</remarks>
        IEnumerable<string> GetFullPaths(string fileName);

        /// <summary>
        /// Returns the directories under the directory <paramref name="path" />.
        /// </summary>
        /// <param name="path">The directory under which to search for subdirectories.</param>
        /// <returns>The list of subdirectories in relative path.</returns>
        IEnumerable<string> GetDirectories(string path);
    }

    public enum ImportLocation
    {
        Top,
        Bottom
    }
}
