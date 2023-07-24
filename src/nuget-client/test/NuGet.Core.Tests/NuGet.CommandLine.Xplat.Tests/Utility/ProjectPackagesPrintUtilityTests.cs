// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Moq;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.ListPackage;
using NuGet.CommandLine.XPlat.Utility;
using NuGet.Common;
using NuGet.Configuration;
using Xunit;

namespace NuGet.CommandLine.Xplat.Tests.Utility
{
    public class ProjectPackagesPrintUtilityTests
    {
        [Theory]
        [MemberData(nameof(ReportData))]
        public void CreatesCorrectPackagesReportTableForVariousPackageCollections(
            object[] packages,
            bool printingTransitive,
            object listPackageArgs,
            object[] expectedReport)
        {
            // Arrange
            var installPackageRefs = packages as InstalledPackageReference[];
            bool autoReference = false;
            var pkgArgs = listPackageArgs as ListPackageArgs;
            var listReportPackages = installPackageRefs?
                .Select(p => ProjectPackagesPrintUtility.GetFrameworkPackageMetadata(
                    new List<InstalledPackageReference> { p },
                    printingTransitive,
                    pkgArgs?.ReportType ?? ReportType.Default,
                    ref autoReference)
                .First())
                .ToArray();

            if (listReportPackages != null &&
                expectedReport is FormattedCell[] expectedResult &&
                pkgArgs != null)
            {
                // Act
                var autoReferenceFound = false;
                IEnumerable<FormattedCell> report = ProjectPackagesPrintUtility.BuildPackagesTable(listReportPackages, printingTransitive, pkgArgs, ref autoReferenceFound);

                // Assert
                var reportArr = report.ToArray();
                Assert.Equal(expectedResult.Length, reportArr.Length);
                var autoReferenceFoundExpected = false;
                for (int i = 0; i < expectedResult.Length; i++)
                {
                    autoReferenceFoundExpected = autoReferenceFoundExpected || (reportArr[i].Value?.Contains(" (A)") ?? false);
                    Assert.Equal(expectedResult[i], reportArr[i]);
                }

                Assert.Equal(autoReferenceFoundExpected, autoReferenceFound);
            }
        }

        public static IEnumerable<object[]> ReportData =>
            new List<object[]>
            {
                new object[]
                {
                    new[] { StandardPackage },
                    false, // printing transitives
                    StandardListReportArgs,
                    ProcessExpectedReport(new[]
                    {
                    "   Top-level Package ", "   ", "   Requested", "   Resolved", Environment.NewLine,
                    "   > Package.Standard", "   ", "   2.0.0    ", "   2.0.0   ", Environment.NewLine
                    }),
                },
                new object[]
                {
                    new[] { StandardPackage },
                    true, // printing transitives
                    StandardListReportArgs,
                    ProcessExpectedReport(new[]
                    {
                    "   Transitive Package", "   ", "   Resolved", Environment.NewLine,
                    "   > Package.Standard", "   ", "   2.0.0   ", Environment.NewLine
                    }),
                },
                new object[]
                {
                    new[] { AutoReferencePackage, DeprecatedOutdatedPackage, Vulnerable1Package },
                    false, // printing transitives
                    StandardListReportArgs,
                    ProcessExpectedReport(new[]
                    {
                    "   Top-level Package           ", "      ", "   Requested", "   Resolved", Environment.NewLine,
                    "   > Package.AutoReference     ", "   (A)", "   2.0.0    ", "   2.0.0   ", Environment.NewLine,
                    "   > Package.DeprecatedOutdated", "      ", "   1.0.0    ", "   1.0.0   ", Environment.NewLine,
                    "   > Package.foo1Vulnerable    ", "      ", "   2.0.0    ", "   2.0.0   ", Environment.NewLine
                    })
                },
                new object[]
                {
                    new[] { AutoReferencePackage, DeprecatedOutdatedPackage, Vulnerable1Package },
                    true, // printing transitives
                    StandardListReportArgs,
                    ProcessExpectedReport(new[]
                    {
                    "   Transitive Package          ", "   ", "   Resolved", Environment.NewLine,
                    "   > Package.AutoReference     ", "   ", "   2.0.0   ", Environment.NewLine,
                    "   > Package.DeprecatedOutdated", "   ", "   1.0.0   ", Environment.NewLine,
                    "   > Package.foo1Vulnerable    ", "   ", "   2.0.0   ", Environment.NewLine
                    })
                },
                new object[]
                {
                    new[] { DeprecatedOutdatedPackage, OutdatedPackage },
                    false, // printing transitives
                    OutdatedReportArgs,
                    ProcessExpectedReport(new[]
                    {
                    "   Top-level Package           ", "   ", "   Requested", "   Resolved", "   Latest",  Environment.NewLine,
                    "   > Package.DeprecatedOutdated", "   ", "   1.0.0    ", "   1.0.0   ", "   2.0.0 ",  Environment.NewLine,
                    "   > Package.Outdated          ", "   ", "   1.0.0    ", "   1.0.0   ", "   2.0.0 ",  Environment.NewLine
                    })
                },
                new object[]
                {
                    new[] { DeprecatedPackage, DeprecatedOutdatedPackage },
                    false, // printing transitives
                    DeprecatedReportArgs,
                    ProcessExpectedReport(new[]
                    {
                    "   Top-level Package           ", "   ", "   Requested", "   Resolved", "   Reason(s)", "   Alternative         ", Environment.NewLine,
                    "   > Package.Deprecated        ", "   ", "   2.0.0    ", "   2.0.0   ", "   Legacy   ", "   Package.New >= 1.0.0", Environment.NewLine,
                    "   > Package.DeprecatedOutdated", "   ", "   1.0.0    ", "   1.0.0   ", "   Legacy   ", "   Package.New >= 1.0.0", Environment.NewLine
                    })
                },
                new object[]
                {
                    new[] { Vulnerable1Package, Vulnerable2OutdatedDeprecatedPackage, Vulnerable3Package },
                    false, // printing transitives
                    VulnerableReportArgs,
                    ProcessExpectedReport(new[]
                    {
                    "   Top-level Package                         ", "   ", "   Requested", "   Resolved", "   Severity", "   Advisory URL            ", Environment.NewLine,
                    "   > Package.foo1Vulnerable                  ", "   ", "   2.0.0    ", "   2.0.0   ", "   Low     ", "   http://example/advisory0", Environment.NewLine,
                    "   > Package.foo2VulnerableOutdatedDeprecated", "   ", "   1.0.0    ", "   1.0.0   ", "   Low     ", "   http://example/advisory0", Environment.NewLine,
                    "                                             ", "   ", "            ", "           ", "_y   Moderate", "   http://example/advisory1", Environment.NewLine,
                    "   > Package.foo3Vulnerable                  ", "   ", "   2.0.0    ", "   2.0.0   ", "   Low     ", "   http://example/advisory0", Environment.NewLine,
                    "                                             ", "   ", "            ", "           ", "_y   Moderate", "   http://example/advisory1", Environment.NewLine,
                    "                                             ", "   ", "            ", "           ", "_r   High    ", "   http://example/advisory2", Environment.NewLine
                    })
                },
            };

        private static ListPackageArgs StandardListReportArgsCache;
        private static ListPackageArgs StandardListReportArgs => StandardListReportArgsCache ?? (StandardListReportArgsCache =
            new ListPackageArgs(
                        path: string.Empty, packageSources: new List<PackageSource>(), frameworks: new List<string>(),
                        ReportType.Default, new ListPackageConsoleRenderer(), includeTransitive: false,
                        prerelease: false, highestPatch: false, highestMinor: false, logger: new Mock<ILogger>().Object, cancellationToken: CancellationToken.None));

        private static ListPackageArgs OutdatedReportArgsCache;
        private static ListPackageArgs OutdatedReportArgs => OutdatedReportArgsCache ?? (OutdatedReportArgsCache =
            new ListPackageArgs(
                        path: string.Empty, packageSources: new List<PackageSource>(), frameworks: new List<string>(),
                        ReportType.Outdated, new ListPackageConsoleRenderer(), includeTransitive: false,
                        prerelease: false, highestPatch: false, highestMinor: false, logger: new Mock<ILogger>().Object, cancellationToken: CancellationToken.None));

        private static ListPackageArgs DeprecatedReportArgsCache;
        private static ListPackageArgs DeprecatedReportArgs => DeprecatedReportArgsCache ?? (DeprecatedReportArgsCache =
            new ListPackageArgs(
                        path: string.Empty, packageSources: new List<PackageSource>(), frameworks: new List<string>(),
                        ReportType.Deprecated, new ListPackageConsoleRenderer(), includeTransitive: false,
                        prerelease: false, highestPatch: false, highestMinor: false, logger: new Mock<ILogger>().Object, cancellationToken: CancellationToken.None));

        private static ListPackageArgs VulnerableReportArgsCache;
        private static ListPackageArgs VulnerableReportArgs => VulnerableReportArgsCache ?? (VulnerableReportArgsCache =
            new ListPackageArgs(
                        path: string.Empty, packageSources: new List<PackageSource>(), frameworks: new List<string>(),
                        ReportType.Vulnerable, new ListPackageConsoleRenderer(), includeTransitive: false,
                        prerelease: false, highestPatch: false, highestMinor: false, logger: new Mock<ILogger>().Object, cancellationToken: CancellationToken.None));

        private static InstalledPackageReference StandardPackageCache;
        private static InstalledPackageReference StandardPackage =>
            StandardPackageCache ?? (StandardPackageCache =
            ListPackageTestHelper.CreateInstalledPackageReference(
                packageId: "Package.Standard",
                resolvedPackageVersionString: "2.0.0",
                latestPackageVersionString: "2.0.0"));

        private static InstalledPackageReference AutoReferencePackageCache;
        private static InstalledPackageReference AutoReferencePackage =>
            AutoReferencePackageCache ?? (AutoReferencePackageCache =
            ListPackageTestHelper.CreateInstalledPackageReference(
                packageId: "Package.AutoReference",
                autoReference: true,
                resolvedPackageVersionString: "2.0.0",
                latestPackageVersionString: "2.0.0"));

        private static InstalledPackageReference OutdatedPackageCache;
        private static InstalledPackageReference OutdatedPackage =>
            OutdatedPackageCache ?? (OutdatedPackageCache =
            ListPackageTestHelper.CreateInstalledPackageReference(
                packageId: "Package.Outdated",
                resolvedPackageVersionString: "1.0.0",
                latestPackageVersionString: "2.0.0"));

        private static InstalledPackageReference DeprecatedPackageCache;
        private static InstalledPackageReference DeprecatedPackage =>
            DeprecatedPackageCache ?? (DeprecatedPackageCache =
            ListPackageTestHelper.CreateInstalledPackageReference(
                packageId: "Package.Deprecated",
                resolvedPackageVersionString: "2.0.0",
                latestPackageVersionString: "2.0.0",
                isDeprecated: true));

        private static InstalledPackageReference DeprecatedOutdatedPackageCache;
        private static InstalledPackageReference DeprecatedOutdatedPackage =>
            DeprecatedOutdatedPackageCache ?? (DeprecatedOutdatedPackageCache =
            ListPackageTestHelper.CreateInstalledPackageReference(
                packageId: "Package.DeprecatedOutdated",
                resolvedPackageVersionString: "1.0.0",
                latestPackageVersionString: "2.0.0",
                isDeprecated: true));

        private static InstalledPackageReference Vulnerable1PackageCache;
        private static InstalledPackageReference Vulnerable1Package =>
            Vulnerable1PackageCache ?? (Vulnerable1PackageCache =
            ListPackageTestHelper.CreateInstalledPackageReference(
                packageId: "Package.foo1Vulnerable",  // strange name because these are sorted, and this will put it between report entries
                resolvedPackageVersionString: "2.0.0",
                latestPackageVersionString: "2.0.0",
                vulnerabilityCount: 1));

        private static InstalledPackageReference Vulnerable3PackageCache;
        private static InstalledPackageReference Vulnerable3Package =>
            Vulnerable3PackageCache ?? (Vulnerable3PackageCache =
            ListPackageTestHelper.CreateInstalledPackageReference(
                packageId: "Package.foo3Vulnerable",  // strange name because these are sorted, and this will put it between report entries
                resolvedPackageVersionString: "2.0.0",
                latestPackageVersionString: "2.0.0",
                vulnerabilityCount: 3));

        private static InstalledPackageReference Vulnerable2OutdatedDeprecatedPackageCache;
        private static InstalledPackageReference Vulnerable2OutdatedDeprecatedPackage =>
            Vulnerable2OutdatedDeprecatedPackageCache ?? (Vulnerable2OutdatedDeprecatedPackageCache =
            ListPackageTestHelper.CreateInstalledPackageReference(
                packageId: "Package.foo2VulnerableOutdatedDeprecated",  // strange name because these are sorted, and this will put it between report entries
                resolvedPackageVersionString: "1.0.0",
                latestPackageVersionString: "2.0.0",
                isDeprecated: true,
                vulnerabilityCount: 2));


        private static FormattedCell[] ProcessExpectedReport(string[] unformattedReport)
        {
            var result = new List<FormattedCell>();
            for (int cellIndex = 0; cellIndex < unformattedReport.Length; cellIndex++)
            {
                var cellText = unformattedReport[cellIndex];
                var foregroundColor = (ConsoleColor?)null;
                if (cellText.Length > 2 && cellText.StartsWith("_"))
                {
                    var format = cellText.Substring(0, 2);
                    cellText = cellText.Substring(2, cellText.Length - 2);
                    switch (format)
                    {
                        case "_r":
                            foregroundColor = ConsoleColor.Red;
                            break;
                        case "_y":
                            foregroundColor = ConsoleColor.Yellow;
                            break;
                    }
                }

                result.Add(new FormattedCell(cellText, foregroundColor));
            }

            return result.ToArray();
        }
    }
}
