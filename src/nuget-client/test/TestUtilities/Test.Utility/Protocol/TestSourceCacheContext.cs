// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;

namespace NuGet.Protocol.Test
{
    /// <summary>
    /// This is a test source cache context that should be used in places where it is not convenient to dispose the
    /// <see cref="SourceCacheContext"/>. Since <see cref="SourceCacheContext.GeneratedTempFolder"/> must be called
    /// before <see cref="SourceCacheContext.Dispose()"/> does anything meaningful, this implementation disables that
    /// property.
    /// </summary>
    public sealed class TestSourceCacheContext : SourceCacheContext
    {
        private TestDirectory _testDirectory;

        public override string GeneratedTempFolder
        {
            get
            {
                if (_testDirectory == null)
                {
                    _testDirectory = TestDirectory.Create();
                }

                return _testDirectory;
            }
        }

        protected override void Dispose(bool disposing)
        {
            _testDirectory?.Dispose();

            base.Dispose(disposing);
        }
    }
}
