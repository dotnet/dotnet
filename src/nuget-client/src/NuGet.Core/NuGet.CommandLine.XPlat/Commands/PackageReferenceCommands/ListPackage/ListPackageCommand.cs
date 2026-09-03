// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using NuGet.CommandLine.XPlat.ListPackage;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Credentials;

namespace NuGet.CommandLine.XPlat
{
    internal static class ListPackageCommand
    {
        internal static void Register(
            Command parent,
            Func<ILoggerWithColor> getLogger,
            Action<LogLevel> setLogLevel,
            Func<IListPackageCommandRunner> getCommandRunner,
            TextWriter? consoleOut = null,
            TextWriter? consoleError = null)
        {
            var listCommand = new Command("list", Strings.ListPkg_Description);

            // The .NET SDK always resolves and forwards a concrete project/solution path to this command
            // (both `dotnet package list` and the classic `dotnet list package` route through the SDK's
            // PackageListCommand, which defaults to the current directory and resolves it to a file before
            // invoking NuGet). ExactlyOne therefore only affects direct invocation of this executable, where
            // omitting the path previously threw an ArgumentNullException; it now produces a clean parse error.
            var path = new Argument<string>("<PROJECT | SOLUTION>")
            {
                Description = Strings.ListPkg_PathDescription,
                Arity = ArgumentArity.ExactlyOne
            };

            var framework = new Option<string[]>("--framework")
            {
                Description = Strings.ListPkg_FrameworkDescription,
                Arity = ArgumentArity.OneOrMore
            };

            var deprecatedReport = new Option<bool>("--deprecated")
            {
                Description = Strings.ListPkg_DeprecatedDescription,
                Arity = ArgumentArity.Zero
            };

            var outdatedReport = new Option<bool>("--outdated")
            {
                Description = Strings.ListPkg_OutdatedDescription,
                Arity = ArgumentArity.Zero
            };

            var vulnerableReport = new Option<bool>("--vulnerable")
            {
                Description = Strings.ListPkg_VulnerableDescription,
                Arity = ArgumentArity.Zero
            };

            var includeTransitive = new Option<bool>("--include-transitive")
            {
                Description = Strings.ListPkg_TransitiveDescription,
                Arity = ArgumentArity.Zero
            };

            var prerelease = new Option<bool>("--include-prerelease")
            {
                Description = Strings.ListPkg_PrereleaseDescription,
                Arity = ArgumentArity.Zero
            };

            var highestPatch = new Option<bool>("--highest-patch")
            {
                Description = Strings.ListPkg_HighestPatchDescription,
                Arity = ArgumentArity.Zero
            };

            var highestMinor = new Option<bool>("--highest-minor")
            {
                Description = Strings.ListPkg_HighestMinorDescription,
                Arity = ArgumentArity.Zero
            };

            var source = new Option<string[]>("--source")
            {
                Description = Strings.ListPkg_SourceDescription,
                Arity = ArgumentArity.OneOrMore
            };

            var config = new Option<string>("--config")
            {
                Description = Strings.ListPkg_ConfigDescription,
                Arity = ArgumentArity.ZeroOrOne
            };

            var outputFormat = new Option<string>("--format")
            {
                Description = Strings.ListPkg_OutputFormatDescription,
                Arity = ArgumentArity.ZeroOrOne
            };

            var outputVersion = new Option<string>("--output-version")
            {
                Description = Strings.ListPkg_OutputVersionDescription,
                Arity = ArgumentArity.ZeroOrOne
            };

            var interactive = new Option<bool>("--interactive")
            {
                Description = Strings.NuGetXplatCommand_Interactive,
                Arity = ArgumentArity.Zero
            };

            var verbosity = new Option<string>("--verbosity", "-v")
            {
                Description = Strings.Verbosity_Description,
                Arity = ArgumentArity.ZeroOrOne
            };

            listCommand.Arguments.Add(path);
            listCommand.Options.Add(framework);
            listCommand.Options.Add(deprecatedReport);
            listCommand.Options.Add(outdatedReport);
            listCommand.Options.Add(vulnerableReport);
            listCommand.Options.Add(includeTransitive);
            listCommand.Options.Add(prerelease);
            listCommand.Options.Add(highestPatch);
            listCommand.Options.Add(highestMinor);
            listCommand.Options.Add(source);
            listCommand.Options.Add(config);
            listCommand.Options.Add(outputFormat);
            listCommand.Options.Add(outputVersion);
            listCommand.Options.Add(interactive);
            listCommand.Options.Add(verbosity);

            listCommand.SetAction(async (parseResult, cancellationToken) =>
            {
                var logger = getLogger();

                var verbosityValue = parseResult.GetValue(verbosity) ?? string.Empty;
                setLogLevel(XPlatUtility.MSBuildVerbosityToNuGetLogLevel(verbosityValue));

                var pathValue = parseResult.GetValue(path) ?? string.Empty;
                var configValue = parseResult.GetValue(config);
                var hasConfig = !string.IsNullOrEmpty(configValue);

                var settings = ProcessConfigFile(configValue, pathValue);
                var sourceValues = parseResult.GetValue(source) ?? Array.Empty<string>();

                var packageSources = GetPackageSources(settings, sourceValues, hasConfig);

                var reportType = GetReportType(
                    isOutdated: parseResult.GetValue(outdatedReport),
                    isDeprecated: parseResult.GetValue(deprecatedReport),
                    isVulnerable: parseResult.GetValue(vulnerableReport));

                IReportRenderer reportRenderer = GetOutputType(consoleOut ?? Console.Out, consoleError ?? Console.Error, parseResult.GetValue(outputFormat), outputVersionOption: parseResult.GetValue(outputVersion));
                var provider = new PackageSourceProvider(settings);
                var frameworkValues = parseResult.GetValue(framework) ?? Array.Empty<string>();
                var packageRefArgs = new ListPackageArgs(
                    pathValue,
                    packageSources,
                    frameworkValues.ToList(),
                    reportType,
                    reportRenderer,
                    parseResult.GetValue(includeTransitive),
                    parseResult.GetValue(prerelease),
                    parseResult.GetValue(highestPatch),
                    parseResult.GetValue(highestMinor),
                    provider.LoadAuditSources(),
                    logger,
                    CancellationToken.None);

                WarnAboutIncompatibleOptions(packageRefArgs, reportRenderer);

                DefaultCredentialServiceUtility.SetupDefaultCredentialService(getLogger(), !parseResult.GetValue(interactive));

                var listPackageCommandRunner = getCommandRunner();
                return await listPackageCommandRunner.ExecuteCommandAsync(packageRefArgs);
            });

            parent.Subcommands.Add(listCommand);
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

        private static IReportRenderer GetOutputType(TextWriter consoleOut, TextWriter consoleError, string? outputFormatOption, string? outputVersionOption)
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
                if (!string.IsNullOrEmpty(outputVersionOption))
                {
                    throw new ArgumentException(string.Format(Strings.ListPkg_OutputVersionNotApplicable));
                }
                return new ListPackageConsoleRenderer(consoleOut, consoleError);
            }

            IReportRenderer jsonReportRenderer;

            var currentlySupportedReportVersions = new List<string> { "1" };
            if (!string.IsNullOrEmpty(outputVersionOption) && !currentlySupportedReportVersions.Contains(outputVersionOption))
            {
                throw new ArgumentException(string.Format(Strings.ListPkg_InvalidOutputVersion, outputVersionOption, string.Join(" ,", currentlySupportedReportVersions)));
            }
            else
            {
                jsonReportRenderer = new ListPackageJsonRenderer(consoleOut);
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

        private static ISettings ProcessConfigFile(string? configFile, string? projectOrSolution)
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

        private static List<PackageSource> GetPackageSources(ISettings settings, IEnumerable<string> sources, bool hasConfig)
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

            if (packageSources.Count == 0 || hasConfig)
            {
                packageSources.AddRange(availableSources);
            }

            return packageSources;
        }

        private static string GetEnumValues<T>() where T : struct, Enum
        {
            var enumValues = Enum.GetValues<T>()
               .Select(x => x.ToString());

            return string.Join(", ", enumValues).ToLower(CultureInfo.CurrentCulture);
        }
    }
}
