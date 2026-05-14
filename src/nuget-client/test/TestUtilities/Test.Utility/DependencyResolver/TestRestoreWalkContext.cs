// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Common;
using NuGet.Configuration;
using NuGet.DependencyResolver;
using NuGet.Protocol.Test;

namespace Test.Utility
{
    public class TestRemoteWalkContext : RemoteWalkContext
    {
        public TestRemoteWalkContext() :
            base(new TestSourceCacheContext(), PackageSourceMapping.GetPackageSourceMapping(NullSettings.Instance), NullLogger.Instance)
        {
        }

        public TestRemoteWalkContext(PackageSourceMapping packageSourceMapping, ILogger logger) :
            base(new TestSourceCacheContext(), packageSourceMapping, logger)
        {
        }
    }
}
