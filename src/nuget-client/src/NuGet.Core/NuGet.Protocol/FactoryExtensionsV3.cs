// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public static class FactoryExtensionsV3
    {
        public static SourceRepository GetCoreV3(this Repository.RepositoryFactory factory, string source, FeedType type)
        {
            return Repository.CreateSource(Repository.Provider.GetCoreV3(), source, type);
        }

        public static SourceRepository GetCoreV3(this Repository.RepositoryFactory factory, string source)
        {
            return Repository.CreateSource(Repository.Provider.GetCoreV3(), source);
        }

        public static SourceRepository GetCoreV3(this Repository.RepositoryFactory factory, PackageSource source)
        {
            return Repository.CreateSource(Repository.Provider.GetCoreV3(), source);
        }

        public static SourceRepository GetCoreV2(this Repository.RepositoryFactory factory, PackageSource source)
        {
            return Repository.CreateSource(Repository.Provider.GetCoreV3(), source);
        }

        public static IEnumerable<Lazy<INuGetResourceProvider>> GetCoreV3(this Repository.ProviderFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            return factory.GetCoreV3();
        }
    }
}
