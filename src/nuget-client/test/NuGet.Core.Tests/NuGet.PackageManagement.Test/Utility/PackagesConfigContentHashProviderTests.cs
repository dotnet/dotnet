// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using Moq;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using Xunit;

namespace NuGet.PackageManagement.Test
{
    public class PackagesConfigContentHashProviderTests
    {
        [Fact]
        public void GetNupkgPath_WithCancellationToken_Throws()
        {
            var obj = new PackagesConfigContentHashProvider(It.IsAny<FolderNuGetProject>());

            Assert.Throws<OperationCanceledException>(() =>
            {
                obj.GetContentHash(It.IsAny<PackageIdentity>(), new CancellationToken(canceled: true));
            });
        }
    }
}
