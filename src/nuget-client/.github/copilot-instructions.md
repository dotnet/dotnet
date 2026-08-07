# NuGet.Client Copilot instructions

NuGet.Client is the .NET codebase for NuGet's client tooling and libraries across Visual Studio, the .NET CLI, MSBuild, and `nuget.exe`.

## Development environment

- Build and test only the projects relevant to the change.
- On Windows:
  - Run `.\configure.ps1` before the first build.
  - Build with the `dotnet` or `msbuild` CLI.
- On Linux and macOS:
  - Run `. ./configure.sh` before the first build. It must be sourced, not executed.
  - Build with `dotnet`, or run `./build.sh` to build all the cross-platform projects. Avoid building `NuGet.sln`, since it includes a few Windows-only projects.
- Run `dotnet test` to execute tests. Use `--filter` to run a subset.
- Use NuGet Central Package Management: declare package versions in `Directory.Packages.props` and add versionless `PackageReference` items to project files. Read the [package-update guidance](../docs/updating-packages.md) before changing dependency versions.

## Task-specific guidance

Before implementing code changes, identify and read the guidance relevant to the task. In particular:

- For new features and behavior changes, read the [feature guide](../docs/feature-guide.md), including its requirements for feature configuration, `SdkAnalysisLevel` gating, and restore and pack considerations.
- For public API additions, changes, removals, or shipping, read the [NuGet SDK guidance](../docs/nuget-sdk.md).
- For C# implementation and error-handling patterns, read the [C# conventions and guidelines](agent_docs/csharp.md).
- For localized resources, read the [localization guidance](agent_docs/localization.md).
- For nullable migrations, read the [nullable migration guidance](agent_docs/nullable-migrations.md).
- For performance measurements, read the [benchmarking guidance](agent_docs/benchmarking.md).
- Before creating a branch or pull request, read the [Git and pull request guidance](agent_docs/git-workflow.md).

The repository's broader development documentation is indexed in [CONTRIBUTING.md](../CONTRIBUTING.md).
