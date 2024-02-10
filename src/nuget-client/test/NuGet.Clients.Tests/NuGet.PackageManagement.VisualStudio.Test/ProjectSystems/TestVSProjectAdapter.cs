// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using Moq;
using NuGet.Frameworks;
using NuGet.ProjectManagement;
using NuGet.RuntimeModel;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    internal class TestVSProjectAdapter : IVsProjectAdapter
    {
        private readonly string _targetFrameworkString;
        private readonly string _restorePackagesWithLockFile;
        private readonly string _nuGetLockFilePath;
        private readonly bool _restoreLockedMode;
        private readonly bool _isCPVMEnabled;
        private readonly IEnumerable<(string PackageId, string Version)> _projectPackageVersions;
        private readonly string _isCentralPackageVersionOverrideEnabled;
        private readonly string _CentralPackageTransitivePinningEnabled;

        public TestVSProjectAdapter(
            string fullProjectPath,
            ProjectNames projectNames,
            string targetFrameworkString,
            string restorePackagesWithLockFile = null,
            string nuGetLockFilePath = null,
            bool restoreLockedMode = false,
            IEnumerable<(string PackageId, string Version)> projectPackageVersions = null,
            string isCentralPackageVersionOverrideEnabled = null,
            string CentralPackageTransitivePinningEnabled = null
            )
        {
            FullProjectPath = fullProjectPath;
            ProjectNames = projectNames;
            _targetFrameworkString = targetFrameworkString;
            _restorePackagesWithLockFile = restorePackagesWithLockFile;
            _nuGetLockFilePath = nuGetLockFilePath;
            _restoreLockedMode = restoreLockedMode;
            _isCPVMEnabled = projectPackageVersions?.Any() == true;
            _projectPackageVersions = projectPackageVersions;
            _isCentralPackageVersionOverrideEnabled = isCentralPackageVersionOverrideEnabled;
            _CentralPackageTransitivePinningEnabled = CentralPackageTransitivePinningEnabled;

#pragma warning disable CS0618 // Type or member is obsolete
            Mock.Get(BuildProperties)
                .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.ManagePackageVersionsCentrally))))
                .Returns(_isCPVMEnabled.ToString());

            Mock.Get(BuildProperties)
                .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.CentralPackageVersionOverrideEnabled))))
                .Returns(_isCentralPackageVersionOverrideEnabled ?? string.Empty);

            Mock.Get(BuildProperties)
                .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.CentralPackageTransitivePinningEnabled))))
                .Returns(_CentralPackageTransitivePinningEnabled ?? string.Empty);

            Mock.Get(BuildProperties)
                .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.NuGetLockFilePath))))
                .Returns(_nuGetLockFilePath);

            Mock.Get(BuildProperties)
                .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.RestorePackagesWithLockFile))))
                .Returns(_restorePackagesWithLockFile);

            Mock.Get(BuildProperties)
                .Setup(x => x.GetPropertyValueWithDteFallback(It.Is<string>(x => x.Equals(ProjectBuildProperties.RestoreLockedMode))))
                .Returns(_restoreLockedMode.ToString());
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public string GetMSBuildProjectExtensionsPath()
        {
            return Path.Combine(ProjectDirectory, "obj");
        }

        public IVsProjectBuildProperties BuildProperties { get; } = Mock.Of<IVsProjectBuildProperties>();

        public string CustomUniqueName => ProjectNames.CustomUniqueName;

        public string FullName => ProjectNames.FullName;

        public string FullProjectPath { get; private set; }

        public bool IsDeferred => false;

        public Task<bool> IsSupportedAsync() => Task.FromResult(true);

        public Project Project { get; } = Mock.Of<Project>();

        public string ProjectId
        {
            get
            {
                return Guid.Empty.ToString();
            }
        }

        public string ProjectDirectory => Path.GetDirectoryName(FullProjectPath);

        public string ProjectName => ProjectNames.ShortName;

        public ProjectNames ProjectNames { get; private set; }

        public string UniqueName => ProjectNames.UniqueName;

        public string Version
        {
            get
            {
                return "1.0.0";
            }
        }

        public IVsHierarchy VsHierarchy { get; } = Mock.Of<IVsHierarchy>();

        public Task<FrameworkName> GetDotNetFrameworkNameAsync()
        {
            return Task.FromResult(new FrameworkName(_targetFrameworkString));
        }

        public Task<string[]> GetProjectTypeGuidsAsync()
        {
            return Task.FromResult(Array.Empty<string>());
        }

        public Task<IEnumerable<string>> GetReferencedProjectsAsync()
        {
            return Task.FromResult(Enumerable.Empty<string>());
        }

        public Task<IEnumerable<RuntimeDescription>> GetRuntimeIdentifiersAsync()
        {
            return Task.FromResult(Enumerable.Empty<RuntimeDescription>());
        }

        public Task<IEnumerable<CompatibilityProfile>> GetRuntimeSupportsAsync()
        {
            return Task.FromResult(Enumerable.Empty<CompatibilityProfile>());
        }

        public Task<NuGetFramework> GetTargetFrameworkAsync()
        {
            return Task.FromResult(NuGetFramework.Parse(_targetFrameworkString));
        }

        public Task<IEnumerable<(string PackageId, string Version)>> GetPackageVersionInformationAsync()
        {
            return Task.FromResult(_projectPackageVersions);
        }

        public async Task<IEnumerable<(string ItemId, string[] ItemMetadata)>> GetBuildItemInformationAsync(string itemName, params string[] metadataNames)
        {
            if (itemName == "PackageVersion")
            {
                return await Task.FromResult(_projectPackageVersions.Select(x => (ItemId: x.PackageId, ItemMetadata: new string[] { x.Version })));
            }

            return Enumerable.Empty<(string ItemId, string[] ItemMetadata)>();
        }

        public Task<bool> IsCapabilityMatchAsync(string capabilityExpression)
        {
            throw new NotImplementedException();
        }
    }
}
