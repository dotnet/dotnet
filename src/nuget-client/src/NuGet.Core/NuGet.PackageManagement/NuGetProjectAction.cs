// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.PackageManagement
{
    /// <summary>
    /// NuGetProjectAction
    /// </summary>
    [DebuggerDisplay("{NuGetProjectActionType} {PackageIdentity}")]
    public class NuGetProjectAction
    {
        /// <summary>
        /// PackageIdentity on which the action is performed
        /// </summary>
        public PackageIdentity PackageIdentity { get; private set; }

        public VersionRange VersionRange { get; private set; }

        /// <summary>
        /// Type of NuGetProjectAction. Install/Uninstall
        /// </summary>
        public NuGetProjectActionType NuGetProjectActionType { get; private set; }

        /// <summary>
        /// For NuGetProjectActionType.Install, SourceRepository from which the package should be installed
        /// For NuGetProjectActionType.Uninstall, this will be null
        /// </summary>
        public SourceRepository SourceRepository { get; private set; }

        /// <summary>
        /// NugetProject for which the action is created
        /// </summary>
        public NuGetProject Project { get; private set; }

        protected NuGetProjectAction(PackageIdentity packageIdentity, NuGetProjectActionType nuGetProjectActionType, NuGetProject project, SourceRepository sourceRepository = null)
            : this(packageIdentity, nuGetProjectActionType, project, sourceRepository, versionRange: null)
        {
        }

        protected NuGetProjectAction(PackageIdentity packageIdentity, NuGetProjectActionType nuGetProjectActionType, NuGetProject project, SourceRepository sourceRepository, VersionRange versionRange)
        {
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            PackageIdentity = packageIdentity;
            NuGetProjectActionType = nuGetProjectActionType;
            SourceRepository = sourceRepository;
            Project = project;
            VersionRange = versionRange;
        }

        public static NuGetProjectAction CreateInstallProjectAction(PackageIdentity packageIdentity, SourceRepository sourceRepository, NuGetProject project)
        {
            return new NuGetProjectAction(packageIdentity, NuGetProjectActionType.Install, project, sourceRepository);
        }

        public static NuGetProjectAction CreateInstallProjectAction(PackageIdentity packageIdentity, SourceRepository sourceRepository, NuGetProject project, VersionRange versionRange)
        {
            return new NuGetProjectAction(packageIdentity, NuGetProjectActionType.Install, project, sourceRepository, versionRange);
        }

        public static NuGetProjectAction CreateUninstallProjectAction(PackageIdentity packageIdentity, NuGetProject project)
        {
            return new NuGetProjectAction(packageIdentity, NuGetProjectActionType.Uninstall, project, null);
        }
    }
}
