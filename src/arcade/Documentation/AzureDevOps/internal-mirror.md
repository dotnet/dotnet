# Mirroring GitHub to dev.azure.com/dnceng and dev.azure.com/devdiv

Public code should be mirrored to dev.azure.com/dnceng/internal or dev.azure.com/dnceng/devdiv, depending on where your pipelines live. (see [Azure DevOps Guidance](https://github.com/dotnet/arcade/blob/master/Documentation/AzureDevOps/AzureDevOpsGuidance.md)).  These are the steps for setting up your GitHub repo for mirroring.

1. Make sure you have a repo in the dev.azure.com/dnceng/internal project with a name in the format "{org}-{repo}" (replace  any `/` with `-` in the GitHub repo name). Please follow up with dnceng if a repository does not exist. For DevDiv repos, the pattern is {org}-{repo}-Trusted.
    - Example: github.com/dotnet/arcade => dotnet-arcade
2. Create a PR to the `dotnet-mirroring` internal repo which adds data for repo and branches that you want mirrored, to the [dnceng subscriptions json](https://dev.azure.com/dnceng/internal/_git/dotnet-mirroring?path=/dnceng-subscriptions.jsonc) or [devdiv subscriptions json](https://dev.azure.com/dnceng/internal/_git/dotnet-mirroring?path=/devdiv-subscriptions.jsonc) files. Specifically, add a URI for your GitHub repository to the `repos` object, then types of mirroring and regex branch patterns. Please alphabetize.

Example:
```json
    "https://github.com/dotnet/source-indexer": {
      "fastForward": [
        "main"
      ]
    },
    "https://github.com/dotnet/sourcelink": {
      "fastForward": [
        "main", // Fast forward main -> main
        "release/.*"
      ]
    },
    "https://github.com/dotnet/spa-templates": {
      "fastForward": [
        // GitHubBranchNotFound "main",
        "release/.*"
      ],
      "internalMerge": [
        "release/.*" // Merge release/.* -> internal/release/.*
      ]
    },
```


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CAzureDevOps%5Cinternal-mirror.md)](https://helix.dot.net/f/p/5?p=Documentation%5CAzureDevOps%5Cinternal-mirror.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CAzureDevOps%5Cinternal-mirror.md)</sub>
<!-- End Generated Content-->
