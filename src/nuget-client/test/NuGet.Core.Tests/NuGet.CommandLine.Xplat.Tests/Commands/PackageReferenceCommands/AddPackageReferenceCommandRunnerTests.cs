// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Globalization;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.CommandLine.XPlat.Tests
{
    public class AddPackageReferenceCommandRunnerTests
    {
        [Theory]
        [InlineData("Contoso.Utilities")]
        [InlineData("_internal")]
        [InlineData("My.Package-1.0")]
        [InlineData("A")]
        public void LogPackageIdWarningIfNeeded_CompliantPackageId_DoesNotLogWarning(string packageId)
        {
            var logger = new TestLogger();

            AddPackageReferenceCommandRunner.LogPackageIdWarningIfNeeded(packageId, logger);

            Assert.Empty(logger.WarningMessages);
        }

        [Theory]
        [InlineData("Contöso.Utilities")]
        [InlineData("\u0421ontoso.Utilities")]
        [InlineData("パッケージ")]
        [InlineData("Contoso.Utilities.")]
        public void LogPackageIdWarningIfNeeded_NonCompliantPackageId_LogsUncodedWarning(string packageId)
        {
            var logger = new TestLogger();

            AddPackageReferenceCommandRunner.LogPackageIdWarningIfNeeded(packageId, logger);

            string warning = Assert.Single(logger.WarningMessages);
            string expectedWarning = string.Format(
                CultureInfo.CurrentCulture,
                Strings.Warn_AddPkgNonCompliantPackageId,
                packageId);
            Assert.Equal(expectedWarning, warning);
            Assert.DoesNotMatch(@"NU\d{4}", warning);
            Assert.Empty(logger.LogMessages);
        }

        [Fact]
        public void LogPackageIdWarningIfNeeded_PackageIdOverMaximumLength_LogsWarning()
        {
            string packageId = "A" + new string('b', 100);
            var logger = new TestLogger();

            AddPackageReferenceCommandRunner.LogPackageIdWarningIfNeeded(packageId, logger);

            Assert.Single(logger.WarningMessages);
        }

        [Fact]
        public void LogPackageIdWarningIfNeeded_PackageIdAtMaximumLength_DoesNotLogWarning()
        {
            string packageId = "A" + new string('b', 99);
            var logger = new TestLogger();

            AddPackageReferenceCommandRunner.LogPackageIdWarningIfNeeded(packageId, logger);

            Assert.Empty(logger.WarningMessages);
        }
    }
}
