// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Commands.Test
{
    public class PushCommandTests
    {
        // Skipping linux: https://github.com/NuGet/Home/issues/9299
        [PlatformFact(Platform.Windows, Platform.Darwin)]
        public async Task PushCommand_AbsolutePathSourceAsync()
        {
            using (var workingDir = TestDirectory.Create())
            {
                // Arrange (create a test package)
                var packagePushDest = new DirectoryInfo(Path.Combine(workingDir, "packagePushDest"));
                packagePushDest.Create();

                var packageSources = new List<PackageSource>
                {
                    new PackageSource(packagePushDest.FullName)
                };

                var packageInfoCollection = new[]
                {
                    await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "test1", "1.0.0"),
                    await SimpleTestPackageUtility.CreateFullPackageAsync(workingDir, "test2", "1.0.0")
                };

                // Act
                await PushRunner.Run(
                    Settings.LoadDefaultSettings(null, null, null),
                    new TestPackageSourceProvider(packageSources),
                    new[] { packageInfoCollection[0].FullName, packageInfoCollection[1].FullName },
                    packagePushDest.FullName,
                    null, // api key
                    null, // symbols source
                    null, // symbols api key
                    0, // timeout
                    false, // disable buffering
                    false, // no symbols,
                    false, // enable server endpoint
                    false, // no skip duplicate
                    new TestLogger());

                // Assert
                foreach (var packageInfo in packageInfoCollection)
                {
                    var destFile = Path.Combine(packagePushDest.FullName, packageInfo.Name);
                    Assert.True(File.Exists(destFile));
                }
            }
        }
    }
}
