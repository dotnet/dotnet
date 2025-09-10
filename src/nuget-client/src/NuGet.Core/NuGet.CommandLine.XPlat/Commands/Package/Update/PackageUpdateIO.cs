// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.CommandLine.XPlat.Utility;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using static NuGet.CommandLine.XPlat.Commands.Package.Update.PackageUpdateCommandRunner;

namespace NuGet.CommandLine.XPlat.Commands.Package.Update;

/// <summary>
/// Implementation of IPackageUpdateIO that handles package updates by performing restore operations.
/// </summary>
internal class PackageUpdateIO : IPackageUpdateIO
{
    private readonly MSBuildAPIUtility _msbuildUtility;
    private readonly IEnvironmentVariableReader _environmentVariableReader;

    public PackageUpdateIO(MSBuildAPIUtility msbuildUtility, IEnvironmentVariableReader environmentVariableReader)
    {
        _msbuildUtility = msbuildUtility;
        _environmentVariableReader = environmentVariableReader;
    }

    public ISettings LoadSettings(string projectDirectory) => Settings.LoadDefaultSettings(projectDirectory);

    public DependencyGraphSpec GetDependencyGraphSpec(string project)
    {
        string tempFile = Path.GetTempFileName();
        try
        {
            if (!RunMsbuildTarget(project, tempFile))
            {
                return null;
            }

            DependencyGraphSpec result = DependencyGraphSpec.Load(tempFile);

            return result;
        }
        finally
        {
            File.Delete(tempFile);
        }

        bool RunMsbuildTarget(string project, string tempFile)
        {
            // When being run from the dotnet CLI, use the same dotnet executable, just in case the dotnet on the PATH is different
            // But when NuGet.CommandLine.XPlat is being called directly, call dotnet on the path, so this code is debuggable.
            string dotnetPath = _environmentVariableReader.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet";

            // don't redirect stdout or stderr, so errors are output. But use quiet verbosity, so that success has no output.
            ProcessStartInfo processStartInfo = new ProcessStartInfo(dotnetPath)
            {
                Arguments = $"msbuild " +
                $"\"{project}\" " +
                $"-restore:false " +
                $"-target:GenerateRestoreGraphFile " +
                $"-property:RestoreGraphOutputPath=\"{tempFile}\" " +
                $"-property:RestoreRecursive=false " +
                $"-nologo " +
                $"-verbosity:quiet " +
                $"-tl:false " +
                $"-noautoresponse",
                UseShellExecute = false,
            };

            using var process = Process.Start(processStartInfo);
            process.WaitForExit();

            return process.ExitCode == 0;
        }
    }

    public async Task<IPackageUpdateIO.RestoreResult> PreviewUpdatePackageReferenceAsync(
        DependencyGraphSpec dgSpec,
        SourceCacheContext cacheContext,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var providerCache = new RestoreCommandProvidersCache();

        // Restore outputs a lot of messages at normal verbosity, which update doesn't want.
        var restoreLogger = new RemappedLevelLogger(
            logger,
            new RemappedLevelLogger.Mapping
            {
                Information = LogLevel.Verbose,
                Minimal = LogLevel.Verbose,
            });

        // Pre-loaded request provider containing the graph file
        var providers = new List<IPreLoadedRestoreRequestProvider>
            {
                new DependencyGraphSpecRequestProvider(providerCache, dgSpec)
            };

        var globalPackagesFolder = dgSpec.GetProjectSpec(dgSpec.Restore.Single()).RestoreMetadata.PackagesPath;

        var restoreContext = new RestoreArgs()
        {
            CacheContext = cacheContext,
            Log = restoreLogger,
            MachineWideSettings = new XPlatMachineWideSetting(),
            GlobalPackagesFolder = globalPackagesFolder,
            PreLoadedRequestProviders = providers
            // Sources : No need to pass it, because SourceRepositories contains the already built SourceRepository objects
        };

        // Generate Restore Requests. There will always be 1 request here since we are restoring for 1 project.
        var restoreRequests = await RestoreRunner.GetRequests(restoreContext);

        // Run restore without commit. This will always return 1 Result pair since we are restoring for 1 request.
        var restoreResult = await RestoreRunner.RunWithoutCommitAsync(restoreRequests, restoreContext, cancellationToken);

        var result = new RestoreResult
        {
            RestoreResultPair = restoreResult.Single()
        };
        return result;
    }

    public async Task CommitAsync(IPackageUpdateIO.RestoreResult restorePreviewResult, CancellationToken none)
    {
        await RestoreRunner.CommitAsync(((RestoreResult)restorePreviewResult).RestoreResultPair, CancellationToken.None);
    }

    public void UpdatePackageReference(PackageSpec updatedPackageSpec, IPackageUpdateIO.RestoreResult restorePreviewResult, List<string> packageTfmAliases, PackageToUpdate packageToUpdate, ILogger logger)
    {
        PackageDependency packageDependency = new PackageDependency(packageToUpdate.Id, packageToUpdate.NewVersion);

        List<NuGetFramework> packageTfms = new List<NuGetFramework>(packageTfmAliases.Count);
        foreach (var alias in packageTfmAliases)
        {
            var targetFramework = updatedPackageSpec.TargetFrameworks.Single(tfm => tfm.TargetAlias == alias);
            packageTfms.Add(targetFramework.FrameworkName);
        }

        if (!AddPackageReferenceCommandRunner.TryFindResolvedVersion(packageTfms,
            packageDependency.Id,
            ((RestoreResult)restorePreviewResult).RestoreResultPair.Result, logger, out NuGetVersion resolvedVersion))
        {
            return;
        }

        // Generate the LibraryDependency using the same logic as AddPackageReferenceCommandRunner
        var libraryDependency = AddPackageReferenceCommandRunner.GenerateLibraryDependency(
            updatedPackageSpec,
            customPackagesPath: null,
            packageDependency,
            resolvedVersion);

        // Determine whether to add package reference conditionally or unconditionally
        if (packageTfms.Count == updatedPackageSpec.TargetFrameworks.Count)
        {
            // package is used by all project TFMs (no condition)
            _msbuildUtility.AddPackageReference(updatedPackageSpec.FilePath, libraryDependency, true);
        }
        else
        {
            var frameworkAliases = packageTfms
                .Select(e => AddPackageReferenceCommandRunner.GetAliasForFramework(updatedPackageSpec, e))
                .Where(originalFramework => originalFramework != null);

            _msbuildUtility.AddPackageReferencePerTFM(updatedPackageSpec.FilePath, libraryDependency, frameworkAliases, true);
        }
    }

    private class RestoreResult : IPackageUpdateIO.RestoreResult
    {
        internal required RestoreResultPair RestoreResultPair { get; init; }

        public override bool Success => RestoreResultPair.Result.Success;
    }
}
