
# Codeflow

This tooling is designed to monitor and maintain codeflow to and from the VMR (forward- and
back-flow). Many tools will require auth permissions restricted to a subset of Microsoft employees.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) or later (for `dotnet run` file-based apps)
- [GitHub CLI (`gh`)](https://cli.github.com/)
- [Azure CLI (`az`)](https://learn.microsoft.com/cli/azure/install-azure-cli)

## Authentication Setup

### GitHub

Log in with the GitHub CLI:

```sh
gh auth login
```

This is used to query PR metadata from GitHub.

### Azure / Maestro (PCS)

Log in with the Azure CLI to the Microsoft tenant:

```sh
az login --tenant 72f988bf-86f1-41af-91ab-2d7cd011db47
```

This is used to authenticate with the Product Construction Service (Maestro) at
`maestro.dot.net`.

## Usage

```sh
# List backflow PRs for .NET 11 (default)
dotnet fetch_prs.cs

# List backflow PRs for .NET 10
dotnet fetch_prs.cs net10
```
