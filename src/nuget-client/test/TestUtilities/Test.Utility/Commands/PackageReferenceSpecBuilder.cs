// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.Commands.Test;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.ProjectModel;

namespace Test.Utility.Commands
{
    public class PackageReferenceSpecBuilder
    {
        private readonly List<TargetFrameworkInformation> _targetFrameworks = new List<TargetFrameworkInformation>();
        private IEnumerable<string> _runtimeIdentifiers;
        private IEnumerable<string> _runtimeSupports;

        private string _projectName;
        private string _projectDirectory;
        private bool _isLockedMode;
        private bool _isRestorePackagesWithLockFile;
        private bool _centralPackageVersionsEnabled;
        private bool _centralPackageTransitivePinningEnabled;

        private PackageReferenceSpecBuilder()
        {
        }

        public static PackageReferenceSpecBuilder Create(string projectName, string projectDirectory)
        {
            return new PackageReferenceSpecBuilder()
            {
                _projectName = projectName ?? throw new ArgumentNullException(nameof(projectName)),
                _projectDirectory = projectDirectory ?? throw new ArgumentNullException(nameof(projectDirectory))
            };
        }

        public PackageReferenceSpecBuilder WithTargetFrameworks(IEnumerable<string> targetFrameworks)
        {
            _targetFrameworks.AddRange(targetFrameworks?.Select(e => new TargetFrameworkInformation { FrameworkName = NuGetFramework.Parse(e) }) ?? throw new ArgumentNullException(nameof(targetFrameworks)));
            return this;
        }

        public PackageReferenceSpecBuilder WithTargetFrameworks(IEnumerable<TargetFrameworkInformation> targetFrameworks)
        {
            _targetFrameworks.AddRange(targetFrameworks ?? throw new ArgumentNullException(nameof(targetFrameworks)));
            return this;
        }

        public PackageReferenceSpecBuilder WithRuntimeIdentifiers(IEnumerable<string> runtimeIdentifiers, IEnumerable<string> runtimeSupports)
        {
            _runtimeIdentifiers = runtimeIdentifiers ?? throw new ArgumentNullException(nameof(runtimeIdentifiers));
            _runtimeSupports = runtimeSupports ?? throw new ArgumentNullException(nameof(runtimeSupports));
            return this;
        }

        public PackageReferenceSpecBuilder WithPackagesLockFile(bool isLockedMode = false)
        {
            _isLockedMode = isLockedMode;
            _isRestorePackagesWithLockFile = true;
            return this;
        }

        public PackageReferenceSpecBuilder WithCentralPackageVersionsEnabled()
        {
            _centralPackageVersionsEnabled = true;
            return this;
        }

        public PackageReferenceSpecBuilder WithCentralPackageTransitivePinningEnabled()
        {
            _centralPackageTransitivePinningEnabled = true;
            return this;
        }

        public PackageSpec Build()
        {
            var projectPath = Path.Combine(_projectDirectory, _projectName + ".csproj");

            var packageSpec = JsonPackageSpecReader.GetPackageSpec("{ }", _projectName, projectPath);
            packageSpec.RuntimeGraph = ProjectTestHelpers.GetRuntimeGraph(_runtimeIdentifiers, _runtimeSupports);
            packageSpec.TargetFrameworks.AddRange(_targetFrameworks);

            packageSpec.RestoreMetadata = new ProjectRestoreMetadata
            {
                CrossTargeting = packageSpec.TargetFrameworks.Count > 0,
                OriginalTargetFrameworks = packageSpec.TargetFrameworks.Select(e => e.FrameworkName.GetShortFolderName()).ToList(),
                OutputPath = _projectDirectory,
                ProjectStyle = ProjectStyle.PackageReference,
                ProjectName = _projectName,
                ProjectUniqueName = _projectName,
                ProjectPath = projectPath,
                ConfigFilePaths = new List<string>(),
                RestoreLockProperties = new RestoreLockProperties(_isRestorePackagesWithLockFile.ToString(), Path.Combine(_projectDirectory, PackagesLockFileFormat.LockFileName), _isLockedMode),
                CentralPackageVersionsEnabled = _centralPackageVersionsEnabled,
                CentralPackageTransitivePinningEnabled = _centralPackageTransitivePinningEnabled,
            };

            packageSpec.RestoreMetadata.TargetFrameworks.AddRange(packageSpec.TargetFrameworks.Select(e => new ProjectRestoreMetadataFrameworkInfo { FrameworkName = e.FrameworkName }));

            return packageSpec;
        }
    }
}
