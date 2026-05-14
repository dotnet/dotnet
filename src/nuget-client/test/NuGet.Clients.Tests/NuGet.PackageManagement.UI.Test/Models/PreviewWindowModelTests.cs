// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Text;
using NuGet.Packaging.Core;
using Xunit;

namespace NuGet.PackageManagement.UI
{
    public class PreviewWindowModelTests
    {
        [Fact]
        public void ToString_Always_ReturnsPreviewDescription()
        {
            const string ProjectName = "ProjectA";

            var added = new List<AccessiblePackageIdentity>();
            var deleted = new List<AccessiblePackageIdentity>();
            var updated = new List<UpdatePreviewResult>();

            added.Add(new AccessiblePackageIdentity(new PackageIdentity("PkgA", new Versioning.NuGetVersion("1.2.3"))));
            deleted.Add(new AccessiblePackageIdentity(new PackageIdentity("PkgB", new Versioning.NuGetVersion("3.2.1"))));
            updated.Add(new UpdatePreviewResult(
                new PackageIdentity("PkgC", new Versioning.NuGetVersion("1.0.0")),
                new PackageIdentity("PkgC", new Versioning.NuGetVersion("2.0.0"))));

            var previewResult = new PreviewResult(ProjectName, added, deleted, updated);
            var allResults = new List<PreviewResult>();
            allResults.Add(previewResult);
            var model = new PreviewWindowModel(allResults);
            var sb = new StringBuilder();

            sb.AppendLine(ProjectName);
            sb.AppendLine();
            sb.AppendLine(Resources.Label_UninstalledPackages);
            sb.AppendLine();

            foreach (AccessiblePackageIdentity packageIdentity in deleted)
            {
                sb.AppendLine(packageIdentity.ToString());
            }

            sb.AppendLine();
            sb.AppendLine(Resources.Label_UpdatedPackages);
            sb.AppendLine();

            foreach (UpdatePreviewResult result in updated)
            {
                sb.AppendLine(result.ToString());
            }

            sb.AppendLine();
            sb.AppendLine(Resources.Label_InstalledPackages);
            sb.AppendLine();

            foreach (AccessiblePackageIdentity packageIdentity in added)
            {
                sb.AppendLine(packageIdentity.ToString());
            }

            sb.AppendLine();

            Assert.Equal(sb.ToString(), model.ToString());
        }
    }
}
