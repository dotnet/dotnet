// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol.VisualStudio
{
    public static class FactoryExtensionsVS
    {
        public static SourceRepository GetVisualStudio(this Repository.RepositoryFactory factory, string source)
        {
            return Repository.CreateSource(Repository.Provider.GetVisualStudio(), source);
        }

        public static SourceRepository GetVisualStudio(this Repository.RepositoryFactory factory, Configuration.PackageSource source)
        {
            return Repository.CreateSource(Repository.Provider.GetVisualStudio(), source);
        }

        /// <summary>
        /// Core V2 + Core V3 + VS
        /// </summary>
        public static IEnumerable<Lazy<INuGetResourceProvider>> GetVisualStudio(this Repository.ProviderFactory factory)
        {
            foreach (var provider in Repository.Provider.GetCoreV3())
            {
                yield return provider;
            }

            yield break;
        }
    }
}
