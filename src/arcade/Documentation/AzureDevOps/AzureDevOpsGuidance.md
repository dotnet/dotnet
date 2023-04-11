# Azure DevOps Guidance

## Projects

There are two projects for use.  They are:

- public (https://dev.azure.com/dnceng-public/public)
  - Used for oss
  - For Pipelines only  (no source code - that's on GitHub)
  - Pipelines are allowed to pull source directly from GitHub
- internal  (https://dev.azure.com/dnceng/internal)
  - Pipelines are only allowed to pull source from internal repos
  - Public GitHub repos should be mirrored here for official Microsoft builds

We will have multiple Pipelines, effectively a mirrored set in the public and internal. We will have one set of YAML which applies to both. This will allow for CI (PR testing) in both internal and OSS venues, and well as official build production on the internal side.

## Teams

In this context, teams pretty much only affect which level the kanban (or whatever) boards are at.  While permissions can be set at the team level, we're choosing not to do so.

- $(GitHubOrg)

## Permissions

To keep things as simple (manageable) as possible, we're going to manage permissions coarsely at the project level - pointing directly to existing AD security groups managed in idweb.  **We should not be managing permission outside of this method**

- Permissions will point to existing security groups in AD which are managed in idweb.  This admin is done at the **project** level.  ([Azure DevOps link](https://dev.azure.com/dnceng/internal/_admin/_security))
- The bulk of folks will be in the 'contributors' group, with special additions for other groups (like admin)
- There are Azure DevOps permission groups that can be set out side of the project context. ([Azure DevOps link](https://dev.azure.com/dnceng/_admin/_security))   **We're not going to use those**
- It is also possible to set permissions at the team.  **We're not going to do that**

## Casing

- Casing of projects/repos/build pipelines/etc. should match as closely as possible our GitHub guidelines.  Generally, that means lower-case except where we have already used upper-case.  Examples:
  - dotnet (org name/folder name)
  - public (project name)
  - internal (project name)
  - Microsoft (folder name matching GitHub org name)
  - dotnet-runtime (Azure DevOps repo name on internal project)

## Pipelines

### Folder names for GitHub repos

For those repos which are in GitHub, the pipelines should live:

- $(GitHubOrg)/$(GitHubRepoName)/*.def

### Folders for Azure DevOps repos

For repos in Azure DevOps, the build pipelines should live:

- lower-case, no spaces, use dashes
- Put it where it makes sense (closest GitHub org), just not top-level
- Use the closest GitHub org
- Use the closet GitHub repo name/Azure DevOps repo name without the prefix
- *.def

### Pipeline file name convention

- lower-case, No spaces, use dashes
- Pattern: `<repo-scenario>`
  - Scenario:
    - code-coverage
    - slow-tests
    - fast-tests
    - internal-tools (TBD -- Nate and Matt to investigate more)
    - official
    - ci

### Example

```TEXT
public project:
  dotnet/arcade/arcade-ci
  dotnet/coreclr/coreclr-jit-stress
internal project:
  dotnet/arcade/arcade-official
  dotnet/coreclr/coreclr-ci
  dotnet/coreclr/coreclr-jit-stress
```

## Source Code

For now, everything should be in 'internal' and any code that is public should be on GitHub

### Azure DevOps repos should

- Be mirrored from GitHub, if not internal only.
- Internal-only projects should only be in the 'internal' project with no GitHub equivalent

### Naming conventions

- $(orgName)-$(repoName)
- Again - *No plan to have public repos in Azure DevOps at this time

### Example

```TEXT
Project: Public:
  Repo: dotnet/corefx - Located in GitHub
  Repo: dotnet/coreclr - Located in GitHub
Project: Internal:
  Repo: dotnet-corefx
  Repo: dotnet-coreclr
  Repo: Microsoft-visualfsharp
```

Both of these would point to the same yaml file in the forks of the repo:

- GitHub: dotnet\corefx
- Azure DevOps: dotnet-corefx
- The only differences here are:
  - Repo name: Repos are top level objects in Azure DevOps so we have an org prefix
  - Leaf folder/build pipeline name (depending on how Azure DevOps's pipelines folder work goes)

## Terms

From time to time, there are some terms you might encounter in documentation or otherwise.  Here's some I've run across so far and the interpretation.

- collection --> account --> instance (top level thing - e.g. dev.azure.com/dnceng)
- team --> group of individuals.  Largely is about the backlog, not much more.  In our case we're not using for permissions.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CAzureDevOps%5CAzureDevOpsGuidance.md)](https://helix.dot.net/f/p/5?p=Documentation%5CAzureDevOps%5CAzureDevOpsGuidance.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CAzureDevOps%5CAzureDevOpsGuidance.md)</sub>
<!-- End Generated Content-->
