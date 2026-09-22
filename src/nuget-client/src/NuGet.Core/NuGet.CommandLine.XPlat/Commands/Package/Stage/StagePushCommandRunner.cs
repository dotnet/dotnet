// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NuGet.CommandLine.XPlat.Commands.Package.Stage
{
    internal static class StagePushCommandRunner
    {
        private const string ApiKeyEnvironmentVariableName = "NUGET_API_KEY";
        private const int FailureExitCode = 1;
        private static readonly TimeSpan RequestTimeout = TimeSpan.FromMinutes(5);
        private static readonly Regex GroupIdRegex = new("^[A-Za-z0-9](?:[A-Za-z0-9._-]{0,62}[A-Za-z0-9])?$");

        internal static Task<int> RunAsync(
            StagePushCommandArgs args,
            ISettings settings,
            IPackageSourceProvider packageSourceProvider,
            ISourceRepositoryProvider sourceRepositoryProvider)
        {
            return RunAsync(
                args,
                settings,
                packageSourceProvider,
                sourceRepositoryProvider,
                EnvironmentVariableWrapper.Instance,
                allowInsecureConnections: false);
        }

        internal static async Task<int> RunAsync(
            StagePushCommandArgs args,
            ISettings settings,
            IPackageSourceProvider packageSourceProvider,
            ISourceRepositoryProvider sourceRepositoryProvider,
            IEnvironmentVariableReader environmentVariableReader,
            bool allowInsecureConnections)
        {
            string packagePath = ValidatePackagePath(args.PackagePath);
            ValidateGroupId(args.GroupId);

            PackageSource packageSource = ResolvePackageSource(packageSourceProvider, args.Source);

            if (packageSource.IsHttp && !packageSource.IsHttps)
            {
                if (!allowInsecureConnections)
                {
                    throw new ArgumentException(string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Error_HttpServerUsage,
                        "stage push",
                        packageSource.Source));
                }
            }

            SourceRepository sourceRepository = sourceRepositoryProvider.CreateRepository(packageSource);
            PackageStagingResourceV3? stagingResource = await sourceRepository
                .GetResourceAsync<PackageStagingResourceV3>(args.CancellationToken);

            if (stagingResource is null)
            {
                throw new FatalProtocolException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.StagePushCommand_Error_ResourceNotFound,
                    packageSource.Source));
            }

            string? apiKey = args.ApiKey;
            apiKey ??= environmentVariableReader.GetEnvironmentVariable(ApiKeyEnvironmentVariableName);
            apiKey ??= SettingsUtility.GetApiKey(settings, stagingResource.SourceUri.AbsoluteUri, packageSource.Source);

            if (IsSymbolsPackage(packagePath))
            {
                await stagingResource.PushSymbolsAsync(
                    packagePath,
                    apiKey,
                    args.GroupId,
                    RequestTimeout,
                    allowInsecureConnections,
                    args.Logger,
                    args.CancellationToken);

                LogSymbolsStaged(args.Logger, packagePath);
                return ExitCodes.Success;
            }

            await stagingResource.PushPackageAsync(
                packagePath,
                apiKey,
                args.GroupId,
                RequestTimeout,
                allowInsecureConnections,
                args.Logger,
                args.CancellationToken);

            args.Logger.LogMinimal(string.Format(
                CultureInfo.CurrentCulture,
                Strings.StagePushCommand_PackageStaged,
                packagePath));

            if (args.NoSymbols)
            {
                return ExitCodes.Success;
            }

            string? symbolsPath = FindSymbolsPackage(packagePath);
            if (symbolsPath is null)
            {
                return ExitCodes.Success;
            }

            try
            {
                await stagingResource.PushSymbolsAsync(
                    symbolsPath,
                    apiKey,
                    args.GroupId,
                    RequestTimeout,
                    allowInsecureConnections,
                    args.Logger,
                    args.CancellationToken);
            }
            catch (FatalProtocolException ex)
            {
                return ReportSymbolsFailure(args.Logger, symbolsPath, ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return ReportSymbolsFailure(args.Logger, symbolsPath, ex.Message);
            }

            LogSymbolsStaged(args.Logger, symbolsPath);
            return ExitCodes.Success;
        }

        internal static string? FindSymbolsPackage(string packagePath)
        {
            string packageStem = packagePath[..^".nupkg".Length];
            string snupkgPath = packageStem + ".snupkg";
            if (File.Exists(snupkgPath))
            {
                return snupkgPath;
            }

            string legacySymbolsPath = packageStem + ".symbols.nupkg";
            return File.Exists(legacySymbolsPath) ? legacySymbolsPath : null;
        }

        private static string ValidatePackagePath(string packagePath)
        {
            if (string.IsNullOrWhiteSpace(packagePath))
            {
                throw new ArgumentException(Strings.StagePushCommand_Error_PackagePathRequired);
            }

            string fullPath = Path.GetFullPath(packagePath);
            if (!File.Exists(fullPath))
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.StagePushCommand_Error_PackageNotFound,
                    packagePath));
            }

            if (!IsPackage(fullPath) && !IsSymbolsPackage(fullPath))
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.StagePushCommand_Error_UnsupportedPackage,
                    packagePath));
            }

            return fullPath;
        }

        private static void ValidateGroupId(string? groupId)
        {
            if (!IsValidGroupId(groupId))
            {
                throw new ArgumentException(Strings.StagePushCommand_Error_InvalidGroup);
            }
        }

        internal static bool IsValidGroupId(string? groupId)
        {
            if (groupId is null)
            {
                return true;
            }

            Match match = GroupIdRegex.Match(groupId);
            return match.Success && match.Length == groupId.Length;
        }

        private static bool IsPackage(string packagePath)
        {
            return packagePath.EndsWith(".nupkg", StringComparison.OrdinalIgnoreCase)
                && !packagePath.EndsWith(".symbols.nupkg", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsSymbolsPackage(string packagePath)
        {
            return packagePath.EndsWith(".snupkg", StringComparison.OrdinalIgnoreCase)
                || packagePath.EndsWith(".symbols.nupkg", StringComparison.OrdinalIgnoreCase);
        }

        private static PackageSource ResolvePackageSource(
            IPackageSourceProvider sourceProvider,
            string? source)
        {
            source ??= sourceProvider.DefaultPushSource;

            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentException(Strings.StagePushCommand_Error_MissingSource);
            }

            IEnumerable<PackageSource> enabledSources = sourceProvider
                .LoadPackageSources()
                .Where(packageSource => packageSource.IsEnabled);

            return PackageSourceProviderExtensions.ResolveSource(enabledSources, source);
        }

        private static void LogSymbolsStaged(ILogger logger, string symbolsPath)
        {
            logger.LogMinimal(string.Format(
                CultureInfo.CurrentCulture,
                Strings.StagePushCommand_SymbolsStaged,
                symbolsPath));
        }

        private static int ReportSymbolsFailure(ILogger logger, string symbolsPath, string errorMessage)
        {
            logger.LogError(string.Format(
                CultureInfo.CurrentCulture,
                Strings.StagePushCommand_SymbolsFailed,
                symbolsPath,
                errorMessage));

            return FailureExitCode;
        }
    }
}
