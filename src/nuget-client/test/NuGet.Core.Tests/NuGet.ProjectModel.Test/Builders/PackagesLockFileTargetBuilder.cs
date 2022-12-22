// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Frameworks;

namespace NuGet.ProjectModel.Test.Builders
{
    internal class PackagesLockFileTargetBuilder
    {
        private NuGetFramework _framework;
        private List<LockFileDependency> _dependencies = new List<LockFileDependency>();

        public PackagesLockFileTargetBuilder WithFramework(NuGetFramework framework)
        {
            _framework = framework;
            return this;
        }

        public PackagesLockFileTargetBuilder WithDependency(Action<LockFileDependencyBuilder> action)
        {
            var dep = new LockFileDependencyBuilder();
            action(dep);
            _dependencies.Add(dep.Build());
            return this;
        }

        public PackagesLockFileTarget Build()
        {
            return new PackagesLockFileTarget()
            {
                TargetFramework = _framework,
                Dependencies = _dependencies
            };
        }
    }
}
