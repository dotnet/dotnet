// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Moq;
using Xunit;

namespace NuGet.VisualStudio.Test
{
    public class IVsPathContextProviderTests
    {
        // Verify interface method can be used across assembly boundaries
        [Fact]
        public void TryCreateContext_EmbeddableTest()
        {
            var provider = Mock.Of<IVsPathContextProvider>();

            IVsPathContext context;
            var result = provider.TryCreateContext(
                "path/to/project.csproj", out context);

            Assert.False(result);
        }
    }
}
