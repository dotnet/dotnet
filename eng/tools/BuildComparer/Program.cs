// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using System.CommandLine.Parsing;

/// <summary>
/// Tool for comparing Microsoft builds with VMR (Virtual Mono Repo) builds.
/// Identifies missing assets, misclassified assets, and assembly version mismatches
/// Identifies differences in signing status between assets.
/// </summary>
public class Program
{
    private record ComparerCommand(string Name, string Description, Type ComparerType, List<Option> Options);

    private static Option clean = new Option<bool>("-clean")
    {
        Description = "Clean up each artifact after comparison.",
    };

    private static Option assetType = new Option<AssetType?>("-assetType")
    {
        Description = "Type of asset to compare. If not specified, all asset types will be compared.",
        Required = false
    };

    private static Option vmrAssetBasePath = new Option<string>("-vmrAssetBasePath")
    {
        Description = "Path to the VMR asset base path",
        Required = true
    };

    private static Option msftAssetBasePath = new Option<string>("-msftAssetBasePath")
    {
        Description = "Path to the asset base path",
        Required = true
    };

    private static Option issuesReport = new Option<string>("-issuesReport")
    {
        Description = "Path to output xml file for non-baselined issues.",
        Required = true
    };

    private static Option noIssuesReport = new Option<string>("-noIssuesReport") 
    {
        Description = "Path to output xml file for baselined issues and assets without issues.",
        Required = true
    };

    private static Option parallel = new Option<int>("-parallel")
    {
        Description = "Amount of parallelism used while analyzing the builds.",
        DefaultValueFactory = _ => 8,
        Required = false
    };

    private static Option baseline = new Option<string>("-baseline")
    {
        Description = "Path to the baseline build manifest.",
        Required = true
    };

    private static Option exclusions = new Option<string>("-exclusions")
    {
        Description = "Path to the exclusions file.",
        Required = true
    };

    private static Option sdkTaskScript = new Option<string>("-sdkTaskScript")
    {
        Description = "Path to the SDK task script.",
        Required = true
    };

    private static Option includedRepositories = new Option<string[]>("--includedRepositories")
    {
        Description = "Comma separated list of repositories to include in the comparison.",
        Required = true,
        CustomParser = ParseIncludedRepositories,
        AllowMultipleArgumentsPerToken = true,
        Arity = ArgumentArity.OneOrMore,
    };

    /// <summary>
    /// Entry point for the build comparison tool.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <returns>Return code indicating success (0) or failure (non-zero).</returns>
    static int Main(string[] args)
    {
        var rootCommand = new RootCommand("Tool for comparing Microsoft builds with VMR builds.");
        var subCommands = new List<ComparerCommand>
        {
            new(
                "assets",
                "Compares asset manifests and outputs missing or misclassified assets",
                typeof(AssetComparer),
                [clean, assetType, vmrAssetBasePath, msftAssetBasePath, issuesReport, noIssuesReport, parallel, baseline, includedRepositories]),
            new(
                "signing",
                "Compares signing status between builds and outputs assets with differences.",
                typeof(SigningComparer),
                [clean, assetType, vmrAssetBasePath, msftAssetBasePath, issuesReport, noIssuesReport, parallel, baseline, exclusions, sdkTaskScript, includedRepositories]),
        };

        foreach (var command in CreateComparerCommands(subCommands))
        {
            rootCommand.Add(command);
        }

        return rootCommand.Parse(args).InvokeAsync().GetAwaiter().GetResult();
    }

    private static IEnumerable<Command> CreateComparerCommands(List<ComparerCommand> commands)
    {
        foreach (var command in commands)
        {
            var subCommand = new Command(command.Name, command.Description);

            foreach (var option in command.Options)
            {
                subCommand.Options.Add(option);
            }

            subCommand.SetAction((result) =>
            {
                var options = new List<object>();
                foreach (var option in command.Options)
                {
                    var value = result.GetValue((dynamic)option);
                    options.Add(value);
                }
                var comparerInstance = (BuildComparer)Activator.CreateInstance(command.ComparerType, options.ToArray());
                return comparerInstance.Compare().GetAwaiter().GetResult();
            });

            yield return subCommand;
        }
    }

    private static string[] ParseIncludedRepositories(ArgumentResult argumentResult)
    {
        List<string> args = [];
        foreach (var token in argumentResult.Tokens)
        {
            args.AddRange(token.Value.Split(','));
        }

        return [.. args];
    }
}
