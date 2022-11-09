// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Frameworks;
using NuGet.ProjectManagement;
using NuGet.Test.Utility;

namespace Test.Utility
{
    public class TestMSBuildNuGetProjectSystem : IMSBuildProjectSystem
    {
        private const string TestProjectName = "TestProjectName";
        private const string TestProjectFileName = "Test.csproj";

        public Dictionary<string, string> References { get; }
        public HashSet<string> FrameworkReferences { get; }
        public HashSet<string> Files { get; }
        private HashSet<string> FilesInProcessing { get; set; }
        public HashSet<string> ProcessedFiles { get; private set; }
        public HashSet<string> Imports { get; }
        public int BindingRedirectsCallCount { get; private set; }
        public INuGetProjectContext NuGetProjectContext { get; set; }
        public int BatchCount { get; private set; }
        public Action<string> AddReferenceAction { get; set; }
        public Action<string> RemoveReferenceAction { get; set; }
        private Dictionary<string, dynamic> KnownProperties { get; }

        public TestMSBuildNuGetProjectSystem(
            NuGetFramework targetFramework,
            INuGetProjectContext nuGetProjectContext,
            string projectFullPath = null,
            string projectName = null,
            string projectFileFullPath = null)
        {
            TargetFramework = targetFramework;
            References = new Dictionary<string, string>();
            FrameworkReferences = new HashSet<string>();
            Files = new HashSet<string>();
            Imports = new HashSet<string>();
            NuGetProjectContext = nuGetProjectContext;
            ProjectFullPath = string.IsNullOrEmpty(projectFullPath) ? Environment.CurrentDirectory : projectFullPath;
            ProcessedFiles = new HashSet<string>();
            ProjectName = projectName ?? TestProjectName;
            ProjectFileFullPath = projectFileFullPath ?? Path.Combine(ProjectFullPath, ProjectName);
            AddReferenceAction = AddReferenceImplementation;
            RemoveReferenceAction = RemoveReferenceImplementation;
            KnownProperties = new Dictionary<string, dynamic>()
            {
                { "NuGetLockFilePath", null },
                { "RestorePackagesWithLockFile", "false" }
            };
        }

        public void AddFile(string path, Stream stream)
        {
            if (string.IsNullOrEmpty(path)
                || string.IsNullOrEmpty(Path.GetFileName(path)))
            {
                return;
            }

            using (var streamReader = new StreamReader(stream))
            {
                Files.Add(path);
                var fullPath = Path.Combine(ProjectFullPath, path);
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                File.WriteAllText(fullPath, streamReader.ReadToEnd());
            }
        }

        public Task AddFrameworkReferenceAsync(string name, string packageId)
        {
            if (FrameworkReferences.Contains(name))
            {
                throw new InvalidOperationException("Cannot add existing reference. That would be a COMException in VS");
            }
            FrameworkReferences.Add(name);
            return Task.CompletedTask;
        }

        public void AddImport(string targetFullPath, ImportLocation location)
        {
            Imports.Add(targetFullPath);
        }

        public Task AddReferenceAsync(string referencePath)
        {
            AddReferenceAction(referencePath);
            return Task.CompletedTask;
        }

        public void RemoveFile(string path)
        {
            Files.Remove(path);
            var fullPath = Path.Combine(ProjectFullPath, path);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public string ProjectFullPath { get; }

        public string ProjectFileFullPath { get; }

        public string ProjectName { get; }

        public string ProjectUniqueName
        {
            get { return ProjectName; }
        }

        public Task<bool> ReferenceExistsAsync(string name)
        {
            return Task.FromResult(References.ContainsKey(name) || FrameworkReferences.Contains(name));
        }

        public void RemoveImport(string targetFullPath)
        {
            Imports.Remove(targetFullPath);
        }

        public Task RemoveReferenceAsync(string name)
        {
            RemoveReferenceAction(name);
            return Task.CompletedTask;
        }

        public NuGetFramework TargetFramework { get; }

        public bool FileExistsInProject(string path)
        {
            return Files.Any(c => path.Equals(c, StringComparison.OrdinalIgnoreCase));
        }

        public void SetPropertyValue(string propertyName, dynamic value)
        {
            KnownProperties[propertyName] = value;
        }

        public dynamic GetPropertyValue(string propertyName)
        {
            if (KnownProperties.TryGetValue(propertyName, out var value))
            {
                return value;
            }

            // Real builds have lots of properties added by the project file and maybe the build system. Returning null here to
            // signal an undefined property might not match real-world results of this method, so throw an exception so tests
            // can't accidently rely on test-only behaviour.
            throw new NotImplementedException();
        }

        public string ResolvePath(string path)
        {
            return path;
        }

        public bool IsSupportedFile(string path)
        {
            return true;
        }

        public void AddExistingFile(string path)
        {
            Files.Add(path);
        }

        public void AddBindingRedirects()
        {
            BindingRedirectsCallCount++;
        }

        public Task BeginProcessingAsync()
        {
            return Task.FromResult(0);
        }

        public void RegisterProcessedFiles(IEnumerable<string> files)
        {
            if (FilesInProcessing == null)
            {
                FilesInProcessing = new HashSet<string>(files);
            }

            foreach (var file in files)
            {
                FilesInProcessing.Add(file);
            }
        }

        public Task EndProcessingAsync()
        {
            ++BatchCount;
            ProcessedFiles = FilesInProcessing;
            FilesInProcessing = null;

            return Task.FromResult(0);
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            var fullPath = Path.Combine(ProjectFullPath, path);

            // this will delete recursive regadless of input, doesn't matter for testing
            TestFileSystemUtility.DeleteRandomTestFolder(fullPath);
        }

        public IEnumerable<string> GetFiles(string path, string filter, bool recursive)
        {
            return Files.Where(f => f.StartsWith(path));
        }

        public IEnumerable<string> GetFullPaths(string fileName)
        {
            return Files.Where(f => f.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                .Select(f => Path.Combine(ProjectFullPath, f));
        }

        public IEnumerable<string> GetDirectories(string path)
        {
            return GetFiles(path, "*.*", recursive: true);
        }

        private void AddReferenceImplementation(string referencePath)
        {
            var referenceAssemblyName = Path.GetFileName(referencePath);
            if (References.ContainsKey(referenceAssemblyName))
            {
                throw new InvalidOperationException("Cannot add existing reference. That would be a COMException in VS");
            }
            References.Add(referenceAssemblyName, referencePath);
        }

        private void RemoveReferenceImplementation(string name)
        {
            if (References.ContainsKey(name))
            {
                References.Remove(name);
            }
        }
    }
}
