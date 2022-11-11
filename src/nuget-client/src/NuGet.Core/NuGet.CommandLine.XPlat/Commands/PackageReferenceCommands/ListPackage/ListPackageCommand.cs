// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.CommandLineUtils;
using NuGet.CommandLine.XPlat.ListPackage;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Credentials;
using NuGet.Frameworks;

namespace NuGet.CommandLine.XPlat
{
    internal static class ListPackageCommand
    {
        public static void Register(
            CommandLineApplication app,
            Func<ILogger> getLogger,
            Action<LogLevel> setLogLevel,
            Func<IListPackageCommandRunner> getCommandRunner)
        {
            app.Command("list", listpkg =>
            {
                listpkg.Description = Strings.ListPkg_Description;
                listpkg.HelpOption(XPlatUtility.HelpOption);

                listpkg.Option(
                    CommandConstants.ForceEnglishOutputOption,
                    Strings.ForceEnglishOutput_Description,
                    CommandOptionType.NoValue);

                var path = listpkg.Argument(
                    "<PROJECT | SOLUTION>",
                    Strings.ListPkg_PathDescription,
                    multipleValues: false);

                var framework = listpkg.Option(
                    "--framework",
                    Strings.ListPkg_FrameworkDescription,
                    CommandOptionType.MultipleValue);

                var deprecatedReport = listpkg.Option(
                    "--deprecated",
                    Strings.ListPkg_DeprecatedDescription,
                    CommandOptionType.NoValue);

                var outdatedReport = listpkg.Option(
                    "--outdated",
                    Strings.ListPkg_OutdatedDescription,
                    CommandOptionType.NoValue);

                var vulnerableReport = listpkg.Option(
                    "--vulnerable",
                    Strings.ListPkg_VulnerableDescription,
                    CommandOptionType.NoValue);

                var includeTransitive = listpkg.Option(
                    "--include-transitive",
                    Strings.ListPkg_TransitiveDescription,
                    CommandOptionType.NoValue);

                var prerelease = listpkg.Option(
                    "--include-prerelease",
                    Strings.ListPkg_PrereleaseDescription,
                    CommandOptionType.NoValue);

                var highestPatch = listpkg.Option(
                    "--highest-patch",
                    Strings.ListPkg_HighestPatchDescription,
                    CommandOptionType.NoValue);

                var highestMinor = listpkg.Option(
                    "--highest-minor",
                    Strings.ListPkg_HighestMinorDescription,
                    CommandOptionType.NoValue);

                var source = listpkg.Option(
                    "--source",
                    Strings.ListPkg_SourceDescription,
                    CommandOptionType.MultipleValue);

                var config = listpkg.Option(
                    "--config",
                    Strings.ListPkg_ConfigDescription,
                    CommandOptionType.SingleValue);

                var outputFormat = listpkg.Option(
                    "--format",
                    Strings.ListPkg_OutputFormatDescription,
                    CommandOptionType.SingleValue);

                var outputVersion = listpkg.Option(
                    "--output-version",
                    Strings.ListPkg_OutputVersionDescription,
                    CommandOptionType.SingleValue);

                var interactive = listpkg.Option(
                    "--interactive",
                    Strings.NuGetXplatCommand_Interactive,
                    CommandOptionType.NoValue);

                var verbosity = listpkg.Option(
                    "-v|--verbosity",
                    Strings.Verbosity_Description,
                    CommandOptionType.SingleValue);

                listpkg.OnExecute(async () =>
                {
                    var logger = getLogger();

                    setLogLevel(XPlatUtility.MSBuildVerbosityToNuGetLogLevel(verbosity.Value()));

                    var settings = ProcessConfigFile(config.Value(), path.Value);
                    var sources = source.Values;

                    var packageSources = GetPackageSources(settings, sources, config);

                    VerifyValidFrameworks(framework);

                    var reportType = GetReportType(
                        isOutdated: outdatedReport.HasValue(),
                        isDeprecated: deprecatedReport.HasValue(),
                        isVulnerable: vulnerableReport.HasValue());

                    IReportRenderer reportRenderer = GetOutputType(outputFormat.Value(), outputVersionOption: outputVersion.Value());

                    var packageRefArgs = new ListPackageArgs(
                        path.Value,
                        packageSources,
                        framework.Values,
                        reportType,
                        reportRenderer,
                        includeTransitive.HasValue(),
                        prerelease.HasValue(),
                        highestPatch.HasValue(),
                        highestMinor.HasValue(),
                        logger,
                        CancellationToken.None);

                    WarnAboutIncompatibleOptions(packageRefArgs, reportRenderer);

                    DefaultCredentialServiceUtility.SetupDefaultCredentialService(getLogger(), !interactive.HasValue());

                    var listPackageCommandRunner = getCommandRunner();
                    return await listPackageCommandRunner.ExecuteCommandAsync(packageRefArgs);
                });
            });
        }

        private static ReportType GetReportType(bool isDeprecated, bool isOutdated, bool isVulnerable)
        {
            var mutexCount = 0;
            mutexCount += isDeprecated ? 1 : 0;
            mutexCount += isOutdated ? 1 : 0;
            mutexCount += isVulnerable ? 1 : 0;
            if (mutexCount == 0)
            {
                return ReportType.Default;
            }
            else if (mutexCount == 1)
            {
                return isDeprecated ? ReportType.Deprecated : isOutdated ? ReportType.Outdated : ReportType.Vulnerable;
            }

            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.ListPkg_InvalidOptions));
        }

        private static IReportRenderer GetOutputType(string outputFormatOption, string outputVersionOption)
        {
            ReportOutputFormat outputFormat = ReportOutputFormat.Console;
            if (!string.IsNullOrEmpty(outputFormatOption) &&
                !Enum.TryParse(outputFormatOption, ignoreCase: true, out outputFormat))
            {
                string currentlySupportedFormat = GetEnumValues<ReportOutputFormat>();
                throw new ArgumentException(string.Format(Strings.ListPkg_InvalidOutputFormat, outputFormatOption, currentlySupportedFormat));
            }

            if (outputFormat == ReportOutputFormat.Console)
            {
                return new ListPackageConsoleRenderer();
            }

            IReportRenderer jsonReportRenderer;

            var currentlySupportedReportVersions = new List<string> { "1" };
            // If customer pass unsupported version then error out instead of defaulting to version probably unsupported by customer machine.
            if (!string.IsNullOrEmpty(outputVersionOption) && !currentlySupportedReportVersions.Contains(outputVersionOption))
            {
                throw new ArgumentException(string.Format(Strings.ListPkg_InvalidOutputVersion, outputVersionOption, string.Join(" ,", currentlySupportedReportVersions)));
            }
            else
            {
                jsonReportRenderer = new ListPackageJsonRenderer();
            }

            return jsonReportRenderer;
        }

        private static void WarnAboutIncompatibleOptions(ListPackageArgs packageRefArgs, IReportRenderer reportRenderer)
        {
            if (packageRefArgs.ReportType != ReportType.Outdated &&
                (packageRefArgs.Prerelease || packageRefArgs.HighestMinor || packageRefArgs.HighestPatch))
            {
                reportRenderer.AddProblem(ProblemType.Warning, Strings.ListPkg_VulnerableIgnoredOptions);
            }
        }

        private static void VerifyValidFrameworks(CommandOption framework)
        {
            var frameworks = framework.Values.Select(f =>
                                NuGetFramework.Parse(f.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray()[0]));
            if (frameworks.Any(f => f.Framework.Equals("Unsupported", StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException(Strings.ListPkg_InvalidFramework, nameof(framework));
            }
        }

        private static ISettings ProcessConfigFile(string configFile, string projectOrSolution)
        {
            if (string.IsNullOrEmpty(configFile))
            {
                return Settings.LoadDefaultSettings(projectOrSolution);
            }

            var configFileFullPath = Path.GetFullPath(configFile);
            var directory = Path.GetDirectoryName(configFileFullPath);
            var configFileName = Path.GetFileName(configFileFullPath);
            return Settings.LoadDefaultSettings(
                directory,
                configFileName,
                machineWideSettings: new XPlatMachineWideSetting());

        }

        private static IEnumerable<PackageSource> GetPackageSources(ISettings settings, IEnumerable<string> sources, CommandOption config)
        {
            var availableSources = PackageSourceProvider.LoadPackageSources(settings).Where(source => source.IsEnabled);
            var uniqueSources = new HashSet<string>();

            var packageSources = new List<PackageSource>();
            foreach (var source in sources)
            {
                if (!uniqueSources.Contains(source))
                {
                    uniqueSources.Add(source);
                    packageSources.Add(PackageSourceProviderExtensions.ResolveSource(availableSources, source));
                }
            }

            if (packageSources.Count == 0 || config.HasValue())
            {
                packageSources.AddRange(availableSources);
            }

            return packageSources;
        }

        private static string GetEnumValues<T>() where T : Enum
        {
            var enumValues = ((T[])Enum.GetValues(typeof(T)))
               .Select(x => x.ToString());

            return string.Join(", ", enumValues).ToLower(CultureInfo.CurrentCulture);
        }
    }
}
