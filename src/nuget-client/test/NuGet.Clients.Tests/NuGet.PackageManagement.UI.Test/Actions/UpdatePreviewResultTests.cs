// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.PackageManagement.UI.Test
{
    public class UpdatePreviewResultTests
    {
        [Fact]
        public void AutomationName_WhenCultureIsNeutral_ReturnsMessage()
        {
            var oldPkgId = "updated.package";
            var newPkgId = "updated.package";
            var oldVersion = new NuGetVersion(0, 0, 1);
            var newVersion = new NuGetVersion(0, 0, 2);
            var previewResult = new UpdatePreviewResult(
                new PackageIdentity(oldPkgId, oldVersion),
                new PackageIdentity(newPkgId, newVersion));

            Assert.Equal($"{oldPkgId} version {oldVersion.ToNormalizedString()} to {newPkgId} version {newVersion.ToNormalizedString()}",
                previewResult.AutomationName);
        }
    }
}
