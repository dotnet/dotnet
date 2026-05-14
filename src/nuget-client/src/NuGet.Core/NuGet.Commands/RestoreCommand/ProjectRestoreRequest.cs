// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Packaging;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;

namespace NuGet.Commands
{
    internal class ProjectRestoreRequest
    {
        public ProjectRestoreRequest(
            RestoreRequest request,
            PackageSpec packageSpec,
            LockFile existingLockFile,
            RestoreCollectorLogger log)
        {
            CacheContext = request.CacheContext;
            Log = log;
            PackagesDirectory = request.PackagesDirectory;
            ExistingLockFile = existingLockFile;
            MaxDegreeOfConcurrency = request.MaxDegreeOfConcurrency;
            Project = packageSpec;
            PackageExtractionContext = new PackageExtractionContext(
                request.PackageSaveMode,
                request.XmlDocFileSaveMode,
                request.ClientPolicyContext,
                log)
            {
                SignedPackageVerifier = request.SignedPackageVerifier
            };
        }

        public SourceCacheContext CacheContext { get; }
        public RestoreCollectorLogger Log { get; }
        public string PackagesDirectory { get; }
        public int MaxDegreeOfConcurrency { get; }
        public LockFile ExistingLockFile { get; }
        public PackageSpec Project { get; }
        public PackageExtractionContext PackageExtractionContext { get; }
        public Guid ParentId { get; set; }
    }
}
