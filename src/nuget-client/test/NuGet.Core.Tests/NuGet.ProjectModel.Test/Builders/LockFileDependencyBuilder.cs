// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace NuGet.ProjectModel.Test.Builders
{
    internal class LockFileDependencyBuilder
    {
        private string _id = "PackageA";
        private VersionRange _requestedVersion = new VersionRange(new NuGetVersion(1, 0, 0));
        private NuGetVersion _resolvedVersion = new NuGetVersion(1, 0, 0);
        private PackageDependencyType _type = PackageDependencyType.Direct;
        private string _contentHash = "ABC";
        private IList<PackageDependency> _dependencies = new List<PackageDependency>();

        public LockFileDependencyBuilder WithId(string id)
        {
            _id = id;
            return this;
        }

        public LockFileDependencyBuilder WithRequestedVersion(VersionRange requestedVersion)
        {
            _requestedVersion = requestedVersion;
            return this;
        }

        public LockFileDependencyBuilder WithResolvedVersion(NuGetVersion resolvedVersion)
        {
            _resolvedVersion = resolvedVersion;
            return this;
        }

        public LockFileDependencyBuilder WithType(PackageDependencyType type)
        {
            _type = type;
            return this;
        }

        public LockFileDependencyBuilder WithContentHash(string contentHash)
        {
            _contentHash = contentHash;
            return this;
        }

        public LockFileDependencyBuilder WithDependency(PackageDependency dependency)
        {
            _dependencies.Add(dependency);
            return this;
        }

        public LockFileDependency Build()
        {
            if (_type == PackageDependencyType.Project)
            {
                return new LockFileDependency()
                {
                    Id = _id,
                    RequestedVersion = _requestedVersion,
                    Type = _type,
                    Dependencies = _dependencies,
                };
            }
            return new LockFileDependency()
            {
                Id = _id,
                RequestedVersion = _requestedVersion,
                ResolvedVersion = _resolvedVersion,
                Type = _type,
                ContentHash = _contentHash,
                Dependencies = _dependencies,
            };
        }
    }
}
