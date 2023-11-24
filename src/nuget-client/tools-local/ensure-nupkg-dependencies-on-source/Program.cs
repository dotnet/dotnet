// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.CommandLine;
using ensure_nupkg_dependencies_on_source;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        var rootCommand = new CliRootCommand("Check that nupkg dependencies are available on specified source(s).");

        var nupkgsArgument = new CliArgument<List<FileInfo>>("nupkgs");
        nupkgsArgument.Arity = ArgumentArity.OneOrMore;
        nupkgsArgument.Description = "Nupkgs to check";
        rootCommand.Add(nupkgsArgument);

        var sourcesOption = new CliOption<List<string>>("--source");
        sourcesOption.Aliases.Add("-s");
        sourcesOption.Required = true;
        sourcesOption.Arity = ArgumentArity.OneOrMore;
        rootCommand.Add(sourcesOption);

        rootCommand.SetAction(async (ParseResult, CancellationToken) =>
        {
            var files = ParseResult.GetValue<List<FileInfo>>(nupkgsArgument);
            var sourcesList = ParseResult.GetValue<List<string>>(sourcesOption);
            if (files is not null && sourcesList is not null)
            {
                await ExecuteAsync(files, sourcesList);
            }
        });
        var exitCode = await rootCommand.Parse(args).InvokeAsync();
        return exitCode;
    }

    private static async Task<int> ExecuteAsync(List<FileInfo> files, List<string> sourcesList)
    {
        if (!CheckAllFilesExist(files))
        {
            return -1;
        }

        IReadOnlyList<NuGetFeed> sources = GetSources(sourcesList);
        List<string> messages = new();
        IReadOnlyDictionary<string, PackageInfo> nupkgs = GetNupkgInfo(files, messages);

        await CheckDependenciesExistAsync(nupkgs, sources, messages);

        if (messages.Count == 0)
        {
            Console.WriteLine("No missing dependencies found");
            return 0;
        }
        else
        {
            foreach (var message in messages)
            {
                Console.WriteLine(message);
            }
            return -1;
        }
    }

    private static async Task CheckDependenciesExistAsync(
        IReadOnlyDictionary<string, PackageInfo> nupkgs,
        IReadOnlyList<NuGetFeed> sources,
        List<string> messages)
    {
        Dictionary<PackageIdentity, bool> checkedPackages = new();

        // The packages provided are intended to be pushed, so pretend they're already there
        foreach (var packageInfo in nupkgs.Values)
        {
            checkedPackages.Add(packageInfo.PackageIdentity, true);
        }

        using (var cacheContext = new SourceCacheContext())
        {
            foreach (var (nupkgPath, packageInfo) in nupkgs)
            {
                foreach (var dependency in packageInfo.Dependencies)
                {
                    bool exists;
                    if (!checkedPackages.TryGetValue(dependency, out exists))
                    {
                        foreach (var source in sources)
                        {
                            FindPackageByIdResource resource = await source.GetFindPackageByIdResourceAsync();
                            exists = await resource.DoesPackageExistAsync(dependency.Id, dependency.Version, cacheContext, NullLogger.Instance, CancellationToken.None);
                            if (exists)
                            {
                                break;
                            }
                        }

                        checkedPackages.Add(dependency, exists);
                    }

                    if (!exists)
                    {
                        var message = $"{nupkgPath}: Dependency {dependency} could not be found";
                        messages.Add(message);
                    }
                }
            }
        }
    }

    private static IReadOnlyDictionary<string, PackageInfo> GetNupkgInfo(List<FileInfo> files, List<string> messages)
    {
        Dictionary<string, PackageInfo> packages = new();

        foreach (FileInfo file in files)
        {
            PackageInfo packageInfo = GetNupkgInfo(file, messages);
            packages.Add(file.FullName, packageInfo);
        }

        return packages;
    }

    private static PackageInfo GetNupkgInfo(FileInfo file, List<string> messages)
    {
        using (var package = new PackageArchiveReader(file.FullName))
        {
            var nuspecReader = package.NuspecReader;
            var packageIdentity = nuspecReader.GetIdentity();

            HashSet<PackageIdentity> packageDependencies = new();
            foreach (PackageDependencyGroup? tfmGroup in nuspecReader.GetDependencyGroups())
            {
                if (tfmGroup == null) { continue; }
                foreach (var packageDependency in tfmGroup.Packages)
                {
                    var versionRange = packageDependency.VersionRange;
                    if (versionRange == null)
                    {
                        var message = $"{file.FullName}: dependency {packageDependency.Id} does not have a version range";
                        messages.Add(message);
                    }
                    else if (versionRange.MinVersion == null)
                    {
                        var message = $"{file.FullName}: dependency {packageDependency.Id} does not have a min version";
                    }
                    else
                    {
                        var dependencyIdentity = new PackageIdentity(packageDependency.Id, versionRange.MinVersion);
                        packageDependencies.Add(dependencyIdentity);
                    }
                }
            }

            var result = new PackageInfo(packageIdentity, packageDependencies.OrderBy(d => d.Id).ToList());
            return result;
        }
    }

    private static IReadOnlyList<NuGetFeed> GetSources(List<string> sourcesList)
    {
        List<NuGetFeed> sources = new();

        foreach (var packageSource in sourcesList)
        {
            SourceRepository source = Repository.Factory.GetCoreV3(packageSource);
            NuGetFeed feed = new(source);
            sources.Add(feed);
        }

        return sources;
    }

    private static bool CheckAllFilesExist(List<FileInfo> files)
    {
        var allExist = true;
        var hasGlob = false;
        foreach (var file in files)
        {
            if (!file.Exists)
            {
                allExist = false;
                Console.WriteLine(file.FullName + " does not exist.");
                if (file.Name.Contains("*"))
                {
                    hasGlob = true;
                }
            }
        }

        if (hasGlob)
        {
            Console.WriteLine("This app does not support file globbing. Please use your shell to expand the file list.");
        }

        return allExist;
    }
}
