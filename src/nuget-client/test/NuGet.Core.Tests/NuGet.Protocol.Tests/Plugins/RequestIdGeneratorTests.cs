// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class RequestIdGeneratorTests
    {
        [Fact]
        public void GenerateUniqueId()
        {
            const int iterations = 10;

            var ids = new HashSet<string>();
            var generator = new RequestIdGenerator();

            for (var i = 0; i < iterations; ++i)
            {
                ids.Add(generator.GenerateUniqueId());
            }

            Assert.Equal(iterations, ids.Count);
        }
    }
}
