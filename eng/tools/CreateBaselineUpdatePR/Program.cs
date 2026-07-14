// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.CommandLine;
using Microsoft.Extensions.Logging;

namespace CreateBaselineUpdatePR;

public class Program
{
    public static readonly Argument<string> Repo = new("repo")
    {
        Description = "The URL of the repository to create the PR in. " +
            "Supports GitHub (https://github.com/<owner>/<repo>) and " +
            "Azure DevOps (https://dev.azure.com/<account>/<project>/_git/<repo>).",
        Arity = ArgumentArity.ExactlyOne
    };

    public static readonly Argument<string> OriginalFilesDirectory = new("original-files-directory")
    {
        Description = "The directory where the original test files are located. Should be relative to the repo",
        Arity = ArgumentArity.ExactlyOne
    };

    public static readonly Argument<string> UpdatedFilesDirectory = new("updated-files-directory")
    {
        Description = "The directory containing the updated test files published by the associated test. Should be absolute or relative to the working directory of the tool.",
        Arity = ArgumentArity.ExactlyOne
    };

    public static readonly Argument<int> BuildId = new("build-id")
    {
        Description = "The id of the build that published the updated test files.",
        Arity = ArgumentArity.ExactlyOne
    };

    public static readonly Option<string> Title = new("--title", "-t")
    {
        Description = "The title of the PR.",
        Arity = ArgumentArity.ZeroOrOne,
        DefaultValueFactory = _ => "Update Test Baselines and Exclusions"
    };

    public static readonly Option<string> Branch = new("--branch", "-b")
    {
        Description = "The target branch of the PR.",
        Arity = ArgumentArity.ZeroOrOne,
        DefaultValueFactory = _ => "main"
    };

    public static readonly Option<string?> Token = new("--token", "-k")
    {
        Description = "The token used to authenticate to the target repository. " +
            "Use a GitHub PAT for github.com URLs and an Azure DevOps PAT for dev.azure.com URLs. " +
            "Falls back to the GIT_TOKEN environment variable.",
        Arity = ArgumentArity.ZeroOrOne,
        DefaultValueFactory = _ => Environment.GetEnvironmentVariable("GIT_TOKEN")
    };

    public static readonly Option<string?> Identifier = new("--identifier", "-i")
    {
        Description = "An optional discriminator folded into the generated head branch name so that " +
            "multiple jobs of the same pipeline targeting the same branch produce distinct PRs " +
            "(e.g. the reproducibility pipeline's SourceBuilt vs Msft jobs).",
        Arity = ArgumentArity.ZeroOrOne,
        DefaultValueFactory = _ => null
    };

    public static readonly Option<LogLevel> Level = new("--log-level", "-l")
    {
        Description = "The log level to run the tool in.",
        Arity = ArgumentArity.ZeroOrOne,
        DefaultValueFactory = _ => LogLevel.Information,
        Recursive = true
    };

    public static async Task<int> Main(string[] args)
    {
        var sdkDiffTestsCommand = CreateCommand("sdk", "Creates a PR that updates baselines and exclusion files published by the sdk diff tests.");
        var licenseScanTestsCommand = CreateCommand("license", "Creates a PR that updates baselines and exclusion files published by the license scan tests.");
        var reproducibilityTestsCommand = CreateCommand("reproducibility", "Creates a PR that updates baseline and exclusion files published by the reproducibility tests.");

        var rootCommand = new RootCommand("Tool for creating PRs that update baselines and exclusion files.")
        {
            Level,
            sdkDiffTestsCommand,
            licenseScanTestsCommand,
            reproducibilityTestsCommand
        };

        SetCommandAction(sdkDiffTestsCommand, Pipelines.Sdk);
        SetCommandAction(licenseScanTestsCommand, Pipelines.License);
        SetCommandAction(reproducibilityTestsCommand, Pipelines.Reproducibility);

        await rootCommand.Parse(args).InvokeAsync();

        return Log.GetExitCode();
    }

    private static Command CreateCommand(string name, string description)
    {
        return new Command(name, description)
        {
            Repo,
            OriginalFilesDirectory,
            UpdatedFilesDirectory,
            BuildId,
            Title,
            Branch,
            Token,
            Identifier
        };
    }

    private static void SetCommandAction(Command command, Pipelines pipeline)
    {
        command.SetAction(async (result, CancellationToken) =>
        {
            Log.Level = result.GetValue(Level);

            try
            {
                string repoUri = result.GetValue(Repo)!;
                string? token = result.GetValue(Token);
                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new ArgumentException(
                        "A token is required. Pass --token or set the GIT_TOKEN environment variable.");
                }
                GitClient client = GitClient.Create(repoUri, token);

                var creator = new PRCreator(client, pipeline, result.GetValue(Identifier));

                await creator.ExecuteAsync(
                    result.GetValue(OriginalFilesDirectory)!,
                    result.GetValue(UpdatedFilesDirectory)!,
                    result.GetValue(BuildId)!,
                    result.GetValue(Title)!,
                    result.GetValue(Branch)!);
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString());
            }
        });
    }
}
