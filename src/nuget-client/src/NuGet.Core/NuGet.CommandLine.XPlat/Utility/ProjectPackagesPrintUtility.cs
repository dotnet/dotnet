// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Versioning;

namespace NuGet.CommandLine.XPlat.Utility
{
    /// <summary>
    /// A static class used to print the packages information for list command
    /// </summary>
    internal static class ProjectPackagesPrintUtility
    {
        /// <summary>
        /// A function that prints all the package references of a project
        /// </summary>
        /// <param name="packages">A list of framework packages. Check <see cref="FrameworkPackages"/></param>
        /// <param name="projectName">The project name</param>
        /// <param name="listPackageArgs">Command line options</param>
        /// <param name="hasAutoReference">At least one discovered package is autoreference</param>
        internal static void PrintPackages(
            IEnumerable<FrameworkPackages> packages, string projectName, ListPackageArgs listPackageArgs, ref bool hasAutoReference)
        {
            switch (listPackageArgs.ReportType)
            {
                case ReportType.Outdated:
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.ListPkg_ProjectUpdatesHeaderLog, projectName));
                    break;
                case ReportType.Deprecated:
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.ListPkg_ProjectDeprecationsHeaderLog, projectName));
                    break;
                case ReportType.Vulnerable:
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.ListPkg_ProjectVulnerabilitiesHeaderLog, projectName));
                    break;
                case ReportType.Default:
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.ListPkg_ProjectHeaderLog, projectName));
                    break;
            }

            hasAutoReference = false;
            foreach (var frameworkPackages in packages)
            {
                var frameworkTopLevelPackages = frameworkPackages.TopLevelPackages;
                var frameworkTransitivePackages = frameworkPackages.TransitivePackages;

                // If no packages exist for this framework, print the
                // appropriate message
                if (!frameworkTopLevelPackages.Any() && !frameworkTransitivePackages.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Blue;

                    switch (listPackageArgs.ReportType)
                    {
                        case ReportType.Outdated:
                            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "   [{0}]: " + Strings.ListPkg_NoUpdatesForFramework, frameworkPackages.Framework));
                            break;
                        case ReportType.Deprecated:
                            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "   [{0}]: " + Strings.ListPkg_NoDeprecationsForFramework, frameworkPackages.Framework));
                            break;
                        case ReportType.Vulnerable:
                            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "   [{0}]: " + Strings.ListPkg_NoVulnerabilitiesForFramework, frameworkPackages.Framework));
                            break;
                        case ReportType.Default:
                            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "   [{0}]: " + Strings.ListPkg_NoPackagesForFramework, frameworkPackages.Framework));
                            break;
                    }

                    Console.ResetColor();
                }
                else
                {
                    // Print name of the framework
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "   [{0}]: ", frameworkPackages.Framework));
                    Console.ResetColor();

                    // Print top-level packages
                    if (frameworkTopLevelPackages.Any())
                    {
                        var tableHasAutoReference = false;
                        var tableToPrint = BuildPackagesTable(
                            frameworkTopLevelPackages, printingTransitive: false, listPackageArgs, ref tableHasAutoReference);
                        if (tableToPrint != null)
                        {
                            PrintPackagesTable(tableToPrint);
                            hasAutoReference = hasAutoReference || tableHasAutoReference;
                        }
                    }

                    // Print transitive packages
                    if (listPackageArgs.IncludeTransitive && frameworkTransitivePackages.Any())
                    {
                        var tableHasAutoReference = false;
                        var tableToPrint = BuildPackagesTable(
                            frameworkTransitivePackages, printingTransitive: true, listPackageArgs, ref tableHasAutoReference);
                        if (tableToPrint != null)
                        {
                            PrintPackagesTable(tableToPrint);
                            hasAutoReference = hasAutoReference || tableHasAutoReference;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Given a list of packages, this function will print them in a table
        /// </summary>
        /// <param name="packages">The list of packages</param>
        /// <param name="printingTransitive">Whether the function is printing transitive packages information.</param>
        /// <param name="listPackageArgs">Command line options.</param>
        /// <param name="tableHasAutoReference">Flagged if an autoreference marker was printer</param>
        /// <returns>The table as a string</returns>
        internal static IEnumerable<FormattedCell> BuildPackagesTable(
            IEnumerable<InstalledPackageReference> packages,
            bool printingTransitive,
            ListPackageArgs listPackageArgs,
            ref bool tableHasAutoReference)
        {
            var autoReferenceFlagged = false;

            if (!packages.Any())
            {
                return null;
            }

            packages = packages.OrderBy(p => p.Name);

            var headers = BuildTableHeaders(printingTransitive, listPackageArgs);

            var valueSelectors = new List<Func<InstalledPackageReference, object>>
            {
                p => new FormattedCell(p.Name),
                p => new FormattedCell(GetAutoReferenceMarker(p, printingTransitive, ref autoReferenceFlagged)),
            };

            // Include "Requested" version column for top level package list
            if (!printingTransitive)
            {
                valueSelectors.Add(p => new FormattedCell(p.OriginalRequestedVersion));
            }

            // "Resolved" version
            valueSelectors.Add(p => new FormattedCell(GetPackageVersion(p)));

            switch (listPackageArgs.ReportType)
            {
                case ReportType.Outdated:
                    // "Latest" version
                    valueSelectors.Add(p => new FormattedCell(GetPackageVersion(p, useLatest: true)));
                    break;
                case ReportType.Deprecated:
                    valueSelectors.Add(p => new FormattedCell(
                        PrintDeprecationReasons(p.ResolvedPackageMetadata.GetDeprecationMetadataAsync().Result)));
                    valueSelectors.Add(p => new FormattedCell(
                        PrintAlternativePackage((p.ResolvedPackageMetadata.GetDeprecationMetadataAsync().Result)?.AlternatePackage)));
                    break;
                case ReportType.Vulnerable:
                    valueSelectors.Add(p => PrintVulnerabilitiesSeverities(p.ResolvedPackageMetadata.Vulnerabilities));
                    valueSelectors.Add(p => PrintVulnerabilitiesAdvisoryUrls(p.ResolvedPackageMetadata.Vulnerabilities));
                    break;
            }


            var tableToPrint = packages.ToStringTable(headers, valueSelectors.ToArray());

            tableHasAutoReference = autoReferenceFlagged;
            return tableToPrint;
        }

        internal static void PrintPackagesTable(IEnumerable<FormattedCell> tableToPrint)
        {
            foreach (var formattedCell in tableToPrint)
            {
                if (formattedCell.ForegroundColor.HasValue)
                {
                    Console.ForegroundColor = formattedCell.ForegroundColor.Value;
                }

                Console.Write(formattedCell.Value);
                Console.ResetColor();
            }

            Console.WriteLine();
        }

        internal static IEnumerable<FormattedCell> PrintVulnerabilitiesSeverities(
            IEnumerable<PackageVulnerabilityMetadata> vulnerabilityMetadata)
        {
            return vulnerabilityMetadata == null || !vulnerabilityMetadata.Any()
                ? new List<FormattedCell> { new FormattedCell(string.Empty, foregroundColor: null) }
                : vulnerabilityMetadata.Select(VulnerabilityToSeverityFormattedCell);
        }

        internal static IEnumerable<FormattedCell> PrintVulnerabilitiesAdvisoryUrls(
            IEnumerable<PackageVulnerabilityMetadata> vulnerabilityMetadata)
        {
            return vulnerabilityMetadata == null || !vulnerabilityMetadata.Any()
                ? new List<FormattedCell> { new FormattedCell(string.Empty, foregroundColor: null) }
                : vulnerabilityMetadata.Select(v => new FormattedCell(v.AdvisoryUrl?.ToString() ?? string.Empty, foregroundColor: null));
        }

        private static FormattedCell VulnerabilityToSeverityFormattedCell(PackageVulnerabilityMetadata vulnerability)
        {
            switch (vulnerability?.Severity ?? -1)
            {
                case 0: return new FormattedCell("Low", foregroundColor: null); // default color for low severity
                case 1: return new FormattedCell("Moderate", foregroundColor: ConsoleColor.Yellow);
                case 2: return new FormattedCell("High", foregroundColor: ConsoleColor.Red);
                case 3: return new FormattedCell("Critical", foregroundColor: ConsoleColor.Red);
            }

            return new FormattedCell(string.Empty, foregroundColor: null);
        }

        private static string GetAutoReferenceMarker(
            InstalledPackageReference package,
            bool printingTransitive,
            ref bool autoReferenceFound)
        {
            if (printingTransitive) // we don't mark these on transitive package reports
            {
                return string.Empty;
            }

            if (package.AutoReference)
            {
                autoReferenceFound = true;
                return "(A)";
            }
            return string.Empty;
        }

        private static string PrintDeprecationReasons(PackageDeprecationMetadata deprecationMetadata)
        {
            return deprecationMetadata == null
                ? string.Empty
                : string.Join(",", deprecationMetadata.Reasons);
        }

        private static string PrintAlternativePackage(AlternatePackageMetadata alternatePackageMetadata)
        {
            if (alternatePackageMetadata == null)
            {
                return string.Empty;
            }

            var versionRangeString = VersionRangeFormatter.Instance.Format(
                "p",
                alternatePackageMetadata.Range,
                VersionRangeFormatter.Instance);

            return $"{alternatePackageMetadata.PackageId} {versionRangeString}";
        }

        /// <summary>
        /// Print user-friendly representation of a NuGet version.
        /// </summary>
        /// <param name="package">The package reference having its version printed.</param>
        /// <param name="useLatest"><c>True</c> if we're printing the latest version; otherwise <c>False</c>.</param>
        private static string GetPackageVersion(
            InstalledPackageReference package,
            bool useLatest = false)
        {
            if (package == null)
            {
                return string.Empty;
            }

            var output = useLatest ?
                package.LatestPackageMetadata?.Identity?.Version?.ToString() :
                package.ResolvedPackageMetadata?.Identity?.Version?.ToString();
            if (output == null)
            {
                return Strings.ListPkg_NotFoundAtSources;
            }

            return output;
        }

        /// <summary>
        /// Prepares the headers for the tables that will be printed
        /// </summary>
        /// <param name="printingTransitive">Whether the table is for transitive or not</param>
        /// <param name="listPackageArgs">Command line options</param>
        /// <returns></returns>
        internal static string[] BuildTableHeaders(bool printingTransitive, ListPackageArgs listPackageArgs)
        {
            var result = new List<string>();

            if (printingTransitive)
            {
                result.Add(Strings.ListPkg_TransitiveHeader);
                result.Add(string.Empty);
            }
            else
            {
                result.Add(Strings.ListPkg_TopLevelHeader);
                result.Add(string.Empty);
                result.Add(Strings.ListPkg_Requested);
            }

            result.Add(Strings.ListPkg_Resolved);

            switch (listPackageArgs.ReportType)
            {
                case ReportType.Outdated:
                    result.Add(Strings.ListPkg_Latest);
                    break;
                case ReportType.Deprecated:
                    result.Add(Strings.ListPkg_DeprecationReasons);
                    result.Add(Strings.ListPkg_DeprecationAlternative);
                    break;
                case ReportType.Vulnerable:
                    result.Add(Strings.ListPkg_VulnerabilitySeverity);
                    result.Add(Strings.ListPkg_VulnerabilityAdvisoryUrl);
                    break;
            }

            return result.ToArray();
        }

        internal static void PrintSources(IEnumerable<PackageSource> packageSources)
        {
            foreach (var source in packageSources)
            {
                Console.WriteLine("   " + source.Source);
            }
        }
    }
}
