// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.IO;

namespace Microsoft.DotNet.WindowsDesktop.App.Tests;

internal class RepoDirectoriesProvider
{
    private readonly string _testContextVariableFilePath;
    private readonly ImmutableDictionary<string, string> _testContextVariables;

    public string BuildRID { get; }
    public string MicrosoftNETCoreAppVersion { get; }
    public string Configuration { get; }
    public string RepoRoot { get; }
    public string BaseArtifactsFolder { get; }

    public RepoDirectoriesProvider(
        string repoRoot = null,
        string artifacts = null,
        string microsoftNETCoreAppVersion = null)
    {
        RepoRoot = repoRoot ?? GetRepoRootDirectory();
        _testContextVariableFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestContextVariables.txt");

        _testContextVariables = File.ReadAllLines(_testContextVariableFilePath)
            .ToImmutableDictionary(
                line => line[..line.IndexOf('=')],
                line => line[(line.IndexOf('=') + 1)..],
                StringComparer.OrdinalIgnoreCase);

        BaseArtifactsFolder = artifacts ?? Path.Combine(RepoRoot, "artifacts");
        BuildRID = GetTestContextVariable("BUILDRID");
        MicrosoftNETCoreAppVersion = microsoftNETCoreAppVersion ?? GetTestContextVariable("MNA_VERSION");
        Configuration = GetTestContextVariable("BUILD_CONFIGURATION");
    }

    public string GetTestContextVariable(string name)
    {
        // Allow env var override, although normally the test context variables file is used.
        // Don't accept NUGET_PACKAGES env override specifically: Arcade sets this and it leaks
        // in during build.cmd/sh runs, replacing the test-specific dir.
        if (!name.Equals("NUGET_PACKAGES", StringComparison.OrdinalIgnoreCase))
        {
            if (Environment.GetEnvironmentVariable(name) is string envValue)
            {
                return envValue;
            }
        }

        if (_testContextVariables.TryGetValue(name, out string value))
        {
            return value;
        }

        throw new ArgumentException($"Unable to find variable '{name}' in test context variable file '{_testContextVariableFilePath}'");
    }

    private static string GetRepoRootDirectory()
    {
        string currentDirectory = Directory.GetCurrentDirectory();

        while (currentDirectory != null)
        {
            string gitDirOrFile = Path.Combine(currentDirectory, ".git");
            if (Directory.Exists(gitDirOrFile) || File.Exists(gitDirOrFile))
            {
                break;
            }
            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        }

        if (currentDirectory == null)
        {
            throw new Exception("Cannot find the git repository root");
        }

        return currentDirectory;
    }
}
