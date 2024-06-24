# Inter Branch Merge

## What is it

Inter branch merge is mechanism to create automated PRs to upstream branches when changes were made in downstream branches. For instance `release/8.0` updated with the fix. The fix is required to be in the main branch as well, hence the PR for merging `release/8.0` into `main` were created containing the fix. 

Example of the PR: https://github.com/dotnet/sdk/pull/41440

## GitHub workflow
The new approach moves from Web-hook functionality to the github actions per repository. 
Initial proposed design could be found here:
https://github.com/dotnet/dnceng/blob/main/Documentation/OnePagers/github-action-inter-branch-merge.md

### Merge branch Workflow documentation
The workflow usage https://github.com/dotnet/arcade/blob/main/.github/workflows/inter-branch-merge-base.yml

Inter-branch-merge-base parameters: 
- **configuration_file_branch**
    -  The branch from where to read the configuration file
    -  Type: string
    -  Required: false
    -  Default: main 
- **configuration_file_path**
    - The path to the configuration file
    - Type: string
    - Required: false
    - Default: github-merge-flow.jsonc
- **script_version**
    - Optional parameter which allows the target repository to use different branch of this shared workflow
    - Type: string
    - Required: false
    - Default: 'main'


The process of creating the PR once the workflow triggered: 

- Workflow will read the configuration file from the source-repository/configuration_file_branch/configuration_file_path
  - If the configuration for triggered branch does not exist, the workflow will stop
- Having that the configuration is presented and all branches are presented, the PR will be created into the configured branch 


## Onboarding
For repositories to start using the new reusable workflow will need to:

- Prepare the configuration file, which will contain the rules of upstream merging 
- Create the workflow file accordingly in **branches** where the inter-merge should be available

### Step 1: Prepare the configuration file, which will contain the rules of upstream merging
The configuration file structure example
```JS
// ONLY THE VERSION OF THIS FILE IN THE MAIN BRANCH IS USED!
{
    "merge-flow-configurations": {
        // format of this section is
        // "source-branch-name": {
        //    "MergeToBranch": "target-branch-name"
        // },
        "release/8.0.3xx": {
            // The MergeToBranch property should be presented in the object in order the merge flow to work
            "MergeToBranch": "release/8.0.4xx",
            // ExtraSwitches is an optional parameter which is accepted by the script: https://github.com/dotnet/arcade/blob/main/.github/workflows/inter-branch-merge-base.yml. Accepted values are similar to the values from the version file: https://github.com/dotnet/versions/blob/main/Maestro/subscriptions.json
            "ExtraSwitches": "-QuietComments"
        },
        "release/8.0.4xx": {
            "MergeToBranch": "main"
        }
    }
}
```
The convenient place way to create this configuration would be in **default branch**, so workflow will fetch the configurations from the specified (default) branch. 


### Step 2: Create the workflow file in branches where the inter-merge should be available
Create the workflow file in: `.github/workflows/`

```YML
name: Usage of Inter-branch merge workflow
on: 
  push:
    branches:
      - 'releases/**'

permissions:
  contents: write
  pull-requests: write

jobs:
  check-script:
    uses: dotnet/arcade/.github/workflows/inter-branch-merge-base.yml@main
    with:
      configuration_file_path: '.config/merge-flow.json'
      configuration_file_branch: 'custom-branch-if-needed'
```

Once the PR with the workflow will be merged into the specified branches:

- The workflow will fetch the configuration from the config-file.
- Will check the validity of the configuration.
- Will create the merge PR from source-to-target-branch (given that both are presented).
- The PR should not be squash and merged, it will bring conflicts in future merges.
- If there are merge conflicts, you will need to resolve them manually before merging.


### Offboard from old approach
This step could be done before or after the onboarding to the new approach.
In order to offboard from old approach, remove the items with actions = "github-dnceng-branch-merge-pr-generator" and triggerPaths = "your-repository-path"

Please be aware that in case the onboarding will start before project is offboarded then there will be two merge requests created at the same time on each change in the release configured branches.

## Creating releases with new flow

Once the onboarding is completed, and there is a need to configure new flow the steps listed below should be completed: 
- Update the configuration file configuration_file_path
- Create the needed branch 

Note: The order of steps above does not affect the outcome. 


Example: 
Existing configuration file of merge flow:
```JS
{
    "merge-flow-configurations": {
        "release/8.0.3xx": {
            "MergeToBranch": "release/8.0.4xx"
        },
        "release/8.0.4xx": {
            "MergeToBranch": "main",
        }
    }
}
```

We would like to create the new release branch `release/8.0.5xx` hence the new merge flow with changes: 
- The upstream branch of `release/8.0.4xx` should be `release/8.0.5xx`
- The upstream branch of `release/8.0.5xx` should be `main`

The new configuration will look like that:
```JS
{
    "merge-flow-configurations": {
        "release/8.0.3xx": {
            "MergeToBranch": "release/8.0.4xx"
        },
        "release/8.0.4xx": {
            "MergeToBranch": "release/8.0.5xx"
        },
        "release/8.0.5xx": {
            "MergeToBranch": "main"
        }
    }
}
```

### Example of using the workflow from different branch
In case the reusable workflow file was extended not from main branch but a specific reference then the `script_version` parameter should be specified in order to use the same script logic from the mentioned reference.

Note: Using default value `main` without specifying the reference, will allow use the latest version of the workflow with latest updates.

Example
 ```YML
name: Usage of Inter-branch merge workflow with 
on: 
  push:
    branches:
      - 'releases/**'

permissions:
  contents: write
  pull-requests: write

jobs:
  check-script:
    uses: dotnet/arcade/.github/workflows/inter-branch-merge-base.yml@custom_ref
    with:
      configuration_file_path: '.config/merge-flow.json'
      configuration_file_branch: 'custom-branch-if-needed'
      script_version: 'custom_ref'
```



## Troubleshooting 
- In case the workflow fails with the permission error, please verify that the github actions should have the permission to create the PR:
https://docs.github.com/en/repositories/managing-your-repositorys-settings-and-features/enabling-features-for-your-repository/managing-github-actions-settings-for-a-repository#preventing-github-actions-from-creating-or-approving-pull-requests

For any other issues please raise queries in FR


### Legacy flow
Legacy flow of creating the PR is based on Web-Hooks.
Repository, in order to onboard to use inter-merge automated flow, had to add the object in subscritions list of [subscriptions file](https://github.com/dotnet/versions/blob/616bf3daa90677d8315954f6477f9c78045e0f0f/Maestro/subscriptions.json):

```JSON
{
    "triggerPaths": [
        "https://github.com/dotnet/runtime/blob/release/8.0/**/*"
    ],
    "action": "github-dnceng-branch-merge-pr-generator",
    "actionArguments": {
        "vsoSourceBranch": "main",
        "vsoBuildParameters": {
            "GithubRepoOwner": "dotnet",
            "GithubRepoName": "<trigger-repo>",
            "HeadBranch": "<trigger-branch>",
            "BaseBranch": "release/8.0-staging",
            "ExtraSwitches": "-QuietComments"
        }
    }
}
```

### Why to migrate to new approach
For more information about the reasons of changing the approach please refer to the one-pager https://github.com/dotnet/dnceng/blob/main/Documentation/OnePagers/ol-maestro-deprecation.md