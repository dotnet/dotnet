# VMR Repository Onboarding Guide

This document provides a step-by-step guide for onboarding repositories into the VMR (Virtual Monolithic Repository).
For background on VMR concepts and architecture, see [VMR Design and Operation](./VMR-Design-And-Operation.md).

## Prerequisites

Before onboarding a repository to the VMR, ensure the following are met:

- Repository is a dependency of the .NET product (e.g. a dependency of the SDK)
- Repository is onboarded to the [Arcade SDK](https://github.com/dotnet/arcade/blob/main/Documentation/StartHere.md) and its builds are registered in [BAR](https://github.com/dotnet/arcade-services/blob/main/docs/Darc.md#locating-the-bar-build-id-for-a-build).
  - If not, see if it can be added via an [external source-build-reference-package](https://github.com/dotnet/source-build-reference-packages/blob/main/README.md#external)
- Repository is compatible with [source build](https://github.com/dotnet/source-build/blob/main/Documentation/sourcebuild-in-repos/README.md)
- Open an [issue](https://github.com/dotnet/dotnet/issues/new/choose) tracking this work and mention [@dotnet/product-construction](https://github.com/orgs/dotnet/teams/product-construction)

## Onboarding Steps

### Step 1: Configure Source Mappings

Add your repository to `/src/source-mappings.json` and open a PR to merge the change.
For detailed configuration options, see [Repository Source Mappings](./VMR-Full-Code-Flow.md#repository-source-mappings).

### Step 2: Define the Darc Code Flow Subscriptions

Define the appropriate Darc [code flow subscriptions](./Codeflow-PRs.md).
After defined manually trigger the forward flow subscription via `darc` or [Maestro](https://maestro.dot.net/subscriptions).
This will open a [VMR PR](https://github.com/dotnet/dotnet/pulls?q=is%3Apr+is%3Aopen+%22Source+code+updates%22) that brings in the repo's source code.
Add the `NO-MERGE` label to the PR while completing the next steps necessary to integrate the repo into the VMR.

#### Step 3: Ensure no Checked in Binaries

The VMR has restrictions on checked-in binaries.
PR and CI validation exists to ensure the [binary policy](./VMR-Permissible-Sources.md#binary-policy) is satisfied.
If the code flow PR is green, the repo complies with the binary policy.

#### Step 4: Validate OSS Compliant Licenses

The VMR has restrictions on allowed licenses.
Before the code flow PR is merged, queue a run of the [license scan pipeline](https://dev.azure.com/dnceng/internal/_build?definitionId=1490) (internal Microsoft link) to validate the repo complies with the [license policy](./VMR-Permissible-Sources.md#license-policy).

### Step 5: Create Repository Project File

Create a `/repo-projects/<your-repo-name>.proj` that integrates the repo's build into to the VMR's build.
Browse the existing [`repo-projects`](https://github.com/dotnet/dotnet/tree/main/repo-projects) for examples.
Ensure the correct dependencies are defined within the new and existing projects.

### Step 6: Build and Validate

[Build](./README.md#building) the VMR with the new repo.
Resolve any build issues that arise.
Push changes to the PR to get complete valdation.
This may require adjusting the [repository project file](#step-5-create-repository-project-file) and utilizing the [VMR controls](./VMR-Controls.md) to adjust how the repo is built within the VMR.

Validate the assets produced from the build by checking the `/artifacts` directory contents.

### Step 5: Merge PR

Once your PR is green and the artifacts have been validated, remove the `NO-MERGE` label from the PR.
Get approval from the repo experts and [@dotnet/product-construction](https://github.com/orgs/dotnet/teams/product-construction) before merging the PR.

### Step 6: Post Merge Validation

After your PR has been merged validate the following:

1. Validate the [CI builds](https://dev.azure.com/dnceng/internal/_build?definitionId=1330) are passing.
1. Ensure the [forward and back code flows](https://maestro.dot.net) work correctly

### Step 7: Enable Repo Level VMR PR Validation

Consider enabling [repo level VMR PR validation](https://github.com/dotnet/arcade/blob/main/Documentation/VmrValidation.md) as either an optional or required check.

## Getting Help

- Utilize the issue created in [Repository Requirements](#prerequisites) to discuss any issues encountered.
- For source build specific issues mention [@dotnet/source-build](https://github.com/orgs/dotnet/teams/source-build) in your issue.
