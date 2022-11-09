// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NuGet.Frameworks;
using NuGet.Packaging.Core;

namespace NuGet.Commands
{
    public class CompatibilityIssue : IEquatable<CompatibilityIssue>
    {
        public CompatibilityIssueType Type { get; }
        public NuGetFramework Framework { get; }
        public string RuntimeIdentifier { get; }
        public string AssemblyName { get; }
        public PackageIdentity Package { get; }
        public List<NuGetFramework> AvailableFrameworks { get; }
        public List<FrameworkRuntimePair> AvailableFrameworkRuntimePairs { get; }

        private CompatibilityIssue(
            CompatibilityIssueType type,
            PackageIdentity package,
            string assemblyName,
            NuGetFramework framework,
            string runtimeIdentifier,
            IEnumerable<NuGetFramework> availableFrameworks,
            IEnumerable<FrameworkRuntimePair> availableFrameworkRuntimePairs)
        {
            Type = type;
            AssemblyName = assemblyName;
            Package = package;
            Framework = framework;
            RuntimeIdentifier = runtimeIdentifier;
            AvailableFrameworks = availableFrameworks.ToList();
            AvailableFrameworkRuntimePairs = availableFrameworkRuntimePairs.ToList();
        }

        public static CompatibilityIssue ReferenceAssemblyNotImplemented(string assemblyName, PackageIdentity referenceAssemblyPackage, NuGetFramework framework, string runtimeIdentifier)
        {
            return new CompatibilityIssue(
                CompatibilityIssueType.ReferenceAssemblyNotImplemented,
                referenceAssemblyPackage,
                assemblyName,
                framework,
                runtimeIdentifier,
                Enumerable.Empty<NuGetFramework>(),
                Enumerable.Empty<FrameworkRuntimePair>());
        }

        public static CompatibilityIssue IncompatiblePackage(
            PackageIdentity referenceAssemblyPackage,
            NuGetFramework framework,
            string runtimeIdentifier,
            IEnumerable<NuGetFramework> packageFrameworks)
        {
            return new CompatibilityIssue(
                CompatibilityIssueType.PackageIncompatible,
                referenceAssemblyPackage,
                string.Empty,
                framework,
                runtimeIdentifier,
                packageFrameworks,
                Enumerable.Empty<FrameworkRuntimePair>());
        }

        public static CompatibilityIssue IncompatiblePackageWithDotnetTool(PackageIdentity referenceAssemblyPackage)
        {
            return new CompatibilityIssue(
                type: CompatibilityIssueType.IncompatiblePackageWithDotnetTool,
                package: referenceAssemblyPackage,
                assemblyName: string.Empty,
                framework: null,
                runtimeIdentifier: null,
                availableFrameworks: Enumerable.Empty<NuGetFramework>(),
                availableFrameworkRuntimePairs: Enumerable.Empty<FrameworkRuntimePair>());
        }

        public static CompatibilityIssue ToolsPackageWithExtraPackageTypes(PackageIdentity referenceAssemblyPackage)
        {
            return new CompatibilityIssue(
                type: CompatibilityIssueType.ToolsPackageWithExtraPackageTypes,
                package: referenceAssemblyPackage,
                assemblyName: string.Empty,
                framework: null,
                runtimeIdentifier: null,
                availableFrameworks: Enumerable.Empty<NuGetFramework>(),
                availableFrameworkRuntimePairs: Enumerable.Empty<FrameworkRuntimePair>());
        }

        public static CompatibilityIssue IncompatibleToolsPackage(PackageIdentity packageIdentity, NuGetFramework framework, string runtimeIdentifier, HashSet<FrameworkRuntimePair> available)
        {
            return new CompatibilityIssue(
                CompatibilityIssueType.PackageToolsAssetsIncompatible,
                packageIdentity,
                string.Empty,
                framework,
                runtimeIdentifier,
                Enumerable.Empty<NuGetFramework>(),
                available);
        }

        public static CompatibilityIssue IncompatibleProject(
            PackageIdentity project,
            NuGetFramework framework,
            string runtimeIdentifier,
            IEnumerable<NuGetFramework> projectFrameworks)
        {
            return new CompatibilityIssue(
                CompatibilityIssueType.ProjectIncompatible,
                project,
                string.Empty,
                framework,
                runtimeIdentifier,
                projectFrameworks,
                Enumerable.Empty<FrameworkRuntimePair>());
        }

        public static CompatibilityIssue IncompatibleProjectType(
            PackageIdentity project)
        {
            return new CompatibilityIssue(
                CompatibilityIssueType.ProjectWithIncorrectDependencyCount,
                package: project,
                assemblyName: string.Empty,
                framework: null,
                runtimeIdentifier: null,
                availableFrameworks: Enumerable.Empty<NuGetFramework>(),
                availableFrameworkRuntimePairs: Enumerable.Empty<FrameworkRuntimePair>());
        }


        internal static CompatibilityIssue IncompatiblePackageType(
            PackageIdentity packageIdentity,
            NuGetFramework framework,
            string runtimeIdentifier)
        {
            return new CompatibilityIssue(
                CompatibilityIssueType.PackageTypeIncompatible,
                packageIdentity,
                string.Empty,
                framework,
                runtimeIdentifier,
                Enumerable.Empty<NuGetFramework>(),
                Enumerable.Empty<FrameworkRuntimePair>());
        }

        public override string ToString()
        {
            // NOTE(anurse): Why not just use Format's implementation as ToString? I feel like ToString should be
            // reserved for debug output, and just because they will be the same here doesn't mean it isn't useful
            // to have a separate method for the, semantically different, behavior of rendering a message for user
            // display, even though the results are the same.
            return Format();
        }

        public string Format()
        {
            switch (Type)
            {
                case CompatibilityIssueType.ReferenceAssemblyNotImplemented:
                    {
                        if (string.IsNullOrEmpty(RuntimeIdentifier))
                        {
                            return string.Format(CultureInfo.CurrentCulture, Strings.Log_MissingImplementationFx, Package.Id, Package.Version, AssemblyName, Framework);
                        }
                        return string.Format(CultureInfo.CurrentCulture, Strings.Log_MissingImplementationFxRuntime, Package.Id, Package.Version, AssemblyName, Framework, RuntimeIdentifier);
                    }
                case CompatibilityIssueType.PackageIncompatible:
                    {
                        var message = string.Format(CultureInfo.CurrentCulture,
                        Strings.Log_PackageNotCompatibleWithFx,
                        Package.Id,
                        Package.Version.ToNormalizedString(),
                        FormatFramework(Framework, RuntimeIdentifier));

                        var supports = string.Format(CultureInfo.CurrentCulture,
                                    Strings.Log_PackageNotCompatibleWithFx_Supports,
                                    Package.Id,
                                    Package.Version.ToNormalizedString());

                        var noSupports = string.Format(CultureInfo.CurrentCulture,
                                    Strings.Log_PackageNotCompatibleWithFx_NoSupports,
                                    Package.Id,
                                    Package.Version.ToNormalizedString());

                        return FormatMessage(message, supports, noSupports);
                    }
                case CompatibilityIssueType.ProjectIncompatible:
                    {
                        var message = string.Format(CultureInfo.CurrentCulture,
                       Strings.Log_ProjectNotCompatibleWithFx,
                       Package.Id,
                       FormatFramework(Framework, RuntimeIdentifier));

                        var supports = string.Format(CultureInfo.CurrentCulture,
                                    Strings.Log_ProjectNotCompatibleWithFx_Supports,
                                    Package.Id);

                        var noSupports = string.Format(CultureInfo.CurrentCulture,
                                    Strings.Log_ProjectNotCompatibleWithFx_NoSupports,
                                    Package.Id);

                        return FormatMessage(message, supports, noSupports);
                    }
                case CompatibilityIssueType.PackageToolsAssetsIncompatible:
                    {
                        var message = string.Format(CultureInfo.CurrentCulture,
                            Strings.Log_PackageNotCompatibleWithFx,
                            Package.Id,
                            Package.Version,
                            FormatFramework(Framework, RuntimeIdentifier));

                        var supports = string.Format(CultureInfo.CurrentCulture,
                                 Strings.Log_PackageNotCompatibleWithFx_Supports,
                                 Package.Id,
                                 Package.Version.ToNormalizedString());

                        var noSupports = string.Format(CultureInfo.CurrentCulture,
                                    Strings.Log_PackageNotCompatibleWithFx_NoSupports,
                                    Package.Id,
                                    Package.Version.ToNormalizedString());

                        return FormatMessage(message, supports, noSupports);
                    }
                case CompatibilityIssueType.ProjectWithIncorrectDependencyCount:
                    {
                        var message = string.Format(CultureInfo.CurrentCulture,
                               Strings.Error_ProjectWithIncorrectDependenciesCount,
                               Package.Id,
                               1);

                        return FormatMessage(message, string.Empty, string.Empty);
                    }
                case CompatibilityIssueType.IncompatiblePackageWithDotnetTool:
                    {
                        var message = string.Format(CultureInfo.CurrentCulture,
                               Strings.Error_InvalidProjectPackageCombo,
                               Package.Id,
                               Package.Version.ToNormalizedString());

                        return FormatMessage(message, string.Empty, string.Empty);
                    }
                case CompatibilityIssueType.ToolsPackageWithExtraPackageTypes:
                    {
                        var message = string.Format(CultureInfo.CurrentCulture,
                               Strings.Error_ToolsPackageWithExtraPackageTypes,
                               Package.Id,
                               Package.Version.ToNormalizedString());
                        return FormatMessage(message, string.Empty, string.Empty);
                    }
                case CompatibilityIssueType.PackageTypeIncompatible:
                    {
                        var message = string.Format(CultureInfo.CurrentCulture,
                        Strings.Error_IncompatiblePackageType,
                        Package.Id,
                        Package.Version.ToNormalizedString(),
                        PackageType.DotnetPlatform.Name,
                        FormatFramework(Framework, RuntimeIdentifier));

                        return FormatMessage(message, string.Empty, string.Empty);
                    }
                default:
                    return null;
            }
        }

        /// <summary>
        /// Build a incompatible error message for either a package or project
        /// </summary>
        private string FormatMessage(string message, string supports, string noSupports)
        {
            var sb = new StringBuilder();

            sb.Append(message);
            sb.Append(" ");

            if (AvailableFrameworks.Any())
            {
                sb.AppendFormat(CultureInfo.CurrentCulture, supports);

                if (AvailableFrameworks.Count > 1)
                {
                    // Write multiple frameworks on new lines
                    foreach (var framework in AvailableFrameworks.Select(FormatFramework)
                        .OrderBy(s => s, StringComparer.CurrentCultureIgnoreCase))
                    {
                        sb.Append(Environment.NewLine).Append($"  - {framework}");
                    }
                }
                else
                {
                    // Write single frameworks on the same line.
                    sb.Append($" {FormatFramework(AvailableFrameworks.Single())}");
                }
            }
            else if (AvailableFrameworkRuntimePairs.Any())
            {
                sb.AppendFormat(CultureInfo.CurrentCulture, supports);

                if (AvailableFrameworkRuntimePairs.Count > 1)
                {
                    // Write multiple frameworks on new lines
                    foreach (var framework in AvailableFrameworkRuntimePairs.Select(e => FormatFramework(e.Framework, e.RuntimeIdentifier))
                        .OrderBy(s => s, StringComparer.CurrentCultureIgnoreCase))
                    {
                        sb.Append(Environment.NewLine).Append($"  - {framework}");
                    }
                }
                else
                {
                    // Write single frameworks on the same line.
                    var frp = AvailableFrameworkRuntimePairs.Single();
                    sb.Append($" {FormatFramework(frp.Framework, frp.RuntimeIdentifier)}");
                }
            }
            else
            {
                // No frameworks
                sb.Append(noSupports);
            }

            return sb.ToString();
        }

        private static string FormatFramework(NuGetFramework framework)
        {
            return string.Format(CultureInfo.CurrentCulture,
                Strings.Log_FrameworkDisplay,
                framework.GetShortFolderName(),
                framework.DotNetFrameworkName);
        }

        private static string FormatFramework(NuGetFramework framework, string runtimeId)
        {
            if (string.IsNullOrEmpty(runtimeId))
            {
                return FormatFramework(framework);
            }
            else
            {
                return string.Format(CultureInfo.CurrentCulture,
                    Strings.Log_FrameworkRIDDisplay,
                    framework.GetShortFolderName(),
                    framework.DotNetFrameworkName,
                    runtimeId);
            }
        }

        public bool Equals(CompatibilityIssue other)
        {
            return other != null &&
                Equals(Type, other.Type) &&
                Equals(Framework, other.Framework) &&
                string.Equals(RuntimeIdentifier, other.RuntimeIdentifier, StringComparison.Ordinal) &&
                string.Equals(AssemblyName, other.AssemblyName, StringComparison.Ordinal) &&
                Equals(Package, other.Package) &&
                new HashSet<NuGetFramework>(AvailableFrameworks, NuGetFramework.Comparer)
                    .SetEquals(other.AvailableFrameworks);
        }
    }

    public enum CompatibilityIssueType
    {
        ReferenceAssemblyNotImplemented,
        PackageIncompatible,
        ProjectIncompatible,
        PackageToolsAssetsIncompatible,
        ProjectWithIncorrectDependencyCount,
        IncompatiblePackageWithDotnetTool,
        ToolsPackageWithExtraPackageTypes,
        PackageTypeIncompatible
    }
}
