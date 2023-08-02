// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Moq;
using Xunit;

namespace NuGet.VisualStudio.Test
{
    public class IVsPathContextTests
    {
        // Verify interface method can be used across assembly boundaries
        [Fact]
        public void TryResolveReference_EmbeddableTest()
        {
            var context = Mock.Of<IVsPathContext>();

            string packageDirectoryPath;
            var result = context.TryResolvePackageAsset(
                "path/to/reference.dll", out packageDirectoryPath);

            Assert.False(result);
        }
    }
}
