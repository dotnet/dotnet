// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Repositories
{
    public class LocalPackageSourceInfo
    {
        public NuGetv3LocalRepository Repository { get; }

        public LocalPackageInfo Package { get; }

        public LocalPackageSourceInfo(NuGetv3LocalRepository repository, LocalPackageInfo package)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            Repository = repository;
            Package = package;
        }
    }
}
