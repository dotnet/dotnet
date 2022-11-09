// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Packaging.Core;

namespace NuGet.ProjectManagement
{
    public class PackageEventArgs : EventArgs
    {
        /// <summary>
        /// Default constructor for events where no info is known
        /// </summary>
        public PackageEventArgs()
            : this(null, null, null)
        {
        }

        public PackageEventArgs(NuGetProject project, PackageIdentity identity, string installPath)
        {
            Identity = identity;
            InstallPath = installPath;
            Project = project;
        }

        /// <summary>
        /// Package identity
        /// </summary>
        public PackageIdentity Identity { get; }

        /// <summary>
        /// Folder path of the package
        /// </summary>
        public string InstallPath { get; }

        /// <summary>
        /// Project where the action occurred
        /// </summary>
        public NuGetProject Project { get; }
    }
}
