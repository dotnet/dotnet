# VMR Repository Onboarding Guide

This document provides a step-by-step guide for onboarding new repositories into the VMR (Virtual Monolithic Repository).
For background on VMR concepts and architecture, see [VMR Design and Operation](./VMR-Design-And-Operation.md).

## Prerequisites

Before onboarding a repository to the VMR, ensure the following are met:


- Repository is a dependency of the .NET product (e.g. a dependency of the SDK)
- Repository is onboarded to the [Arcade SDK](https://github.com/dotnet/arcade/blob/main/Documentation/StartHere.md)
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

### Step 3: Create Repository Project File

Once the code flows into the VMR, create `/repo-projects/<your-repo-name>.proj` to define build behavior.
Browse the existing [repo-project](https://github.com/dotnet/dotnet/tree/main/repo-projects) for examples.
Ensure the correct dependencies are defined within the new and existing projects.

### Step 4: Build and Validate

[Build](./README.md#building) the VMR with the new repo.
Resolve any build issues that arise.
This may require adjusting the [repository project file](#step-3-create-repository-project-file) and utilizing the [VMR controls](./VMR-Controls.md) to adjust how the repo is built within the VMR.

Validate the assets produced from the build by checking the `/artifacts` directory contents.

### Step 5: Open a PR

Open a PR which will trigger the PR validation.
Address any issues that may be surfaced.

#### Step 5a: Resolve Source Build Prebuilts

The source build legs that run as part of PR validation may surface [prebuilts](https://github.com/dotnet/source-build/blob/main/Documentation/eliminating-pre-builts.md#what-is-a-prebuilt).
These will need to be [eliminated](https://github.com/dotnet/source-build/blob/main/Documentation/eliminating-pre-builts.md#eliminating-pre-builts) before the PR can be merged.

#### Step 5b: Handle License and Binary Validation

The VMR has restrictions on allowed licenses and checked-in binaries.
PR and CI validation exists to ensure the policies are satisfied.
For policy details, see [VMR Permissible Sources](./VMR-Permissible-Sources.md).

### Step 6: Validate

After your PR has been merged validate the following:

1. Validate the [CI builds](https://dev.azure.com/dnceng/internal/_build?definitionId=1330) are passing.
1. Ensure the [code flow](https://maestro.dot.net) works correctly

### Step 7: Enable Repo Level VMR PR Validation

Consider enabling [repo level VMR PR validation](https://github.com/dotnet/arcade/blob/main/Documentation/VmrValidation.md) as either an optional or required check.

## Getting Help

- Utilize the issue created in [Repository Requirements](#repository-requirements) to discuss any issues encountered.
- For source build specific issues mention [@dotnet/source-build](https://github.com/orgs/dotnet/teams/source-build) in your issue.
