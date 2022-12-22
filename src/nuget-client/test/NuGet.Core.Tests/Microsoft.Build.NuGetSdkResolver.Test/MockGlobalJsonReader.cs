// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace Microsoft.Build.NuGetSdkResolver.Test
{
    /// <summary>
    /// Represents a mock implementation of <see cref="IGlobalJsonReader" />.
    /// </summary>
    internal class MockGlobalJsonReader : IGlobalJsonReader
    {
        private readonly Dictionary<string, string> _sdkVersions;

        public MockGlobalJsonReader(Dictionary<string, string> sdkVersions)
        {
            _sdkVersions = sdkVersions;
        }

        public Dictionary<string, string> GetMSBuildSdkVersions(SdkResolverContext context, string fileName = "global.json")
        {
            return _sdkVersions;
        }
    }
}
