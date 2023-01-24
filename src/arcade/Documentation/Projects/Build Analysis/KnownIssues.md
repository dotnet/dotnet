# Known Issues

"Known Issues" are GitHub issues that are created with the purpose of reporting and tracking known build or test errors.

[Build Analysis](https://github.com/dotnet/arcade/blob/main/Documentation/Projects/Build%20Analysis/Introduction.md) has a list of Known Issues whenever a build or test error matches an existing Known Issue. This will help the developers to know when a build break is not exclusive to their build. This information is listed at the top of Build Analysis.

All reported Known Issues can be found on [Known Issues project](https://github.com/orgs/dotnet/projects/111).

## When and how to report a Known Issue

A Known Issue should be reported when you find a build or test error that is not caused by your changes and that affects or could affect more builds.

There are two types of Known Issues:

- **Infrastructure**: An infrastructure issue is an issue that is not exclusive to your repository and that needs to be investigated by the engineering services (@dotnet/dnceng)
- **Repository**: A repository issue is an issue that is happening in a particular repository and that should be investigated by the repository owners.

There are two ways to report a Known Issue: one is via Build Analysis and the other one is manually.

### Reporting a Known Issue manually

1. Decide if you need to open a [repository issue or infrastructure issue](#decide-infrastructure-or-repository-issue)
1. Open a new issue, choosing the repository in which you are opening the issue based on following rule:
    - Infrastructure issue - arcade
    - Repository issue - In the repo in which the issue is happening
1. Add the label `Known Build Error`. (If the label is not available on the repository, follow the instructions to [get onboard](#how-to-get-onboard))
1. Copy and paste the template

    ````md
    ## Build Information
    Build: <!-- Add link to the build with the reported error. -->
    Leg Name: <!-- Add the name of the impacted leg. -->

    <!-- Error message template  -->
    ## Error Message
    <!-- Fill for repository issues. For infrastructure issues the engineering services (@dotnet/dnceng) is going to fill it. -->
    ```json 
    { 
        "ErrorMessage": "",
        "BuildRetry": false,
        "ErrorPattern": "",
        "ExcludeConsoleLog": false
    } 
    ```
    ````

1. [Fill out the JSON blob](#filling-out-known-issues-json-blob) accordingly

### Reporting Known Issues via Build Analysis

1. In Build Analysis, you will see links for the type of issue you want to open.
![Links to report Known Issue](./Resources/KnownIssuesLinks.png?raw=true)
1. Click on the link of the [type of issue that suits the situation better](#decide-infrastructure-or-repository-issue).
1. A template is going to appear for you and most of this information should be already prefilled.
1. [Fill out the JSON blob](#filling-out-known-issues-json-blob) accordingly

### Filling out Known Issues JSON blob

1. If you are opening a Repository issue you need to [fill the "Error message/pattern" section](#how-to-fill-out-a-known-issue-error-section). If you are opening an infrastructure issue, this is going to be handled by the engineering services team.
1. If the issue reported can be solved by retrying the build you can consider setting the ["Build Retry" configuration](#build-retry-functionality-in-known-issues) to `true`
1. You should set `ExcludeConsoleLog` to `true` if you want to exclude console logs from Known Issues matching
1. You are done but [what happens after a Known Issue is created?](#what-happens-after-creating-or-updating-a-known-issue)

Note: You need to escape special characters to make your JSON valid. You can use the preview tab to validate your JSON blob, GitHub will highlight invalid characters.

![Invalid JSON blob](./Resources/invalid_json_blob.png?raw=true)

## How the matching process works between a Known Issue and a build/test error

The Known Issues feature finds build and test errors and matches them with open Known Issues using `String.Contains` when there is an "ErrorMessage" property or regex matching if an "ErrorPattern" property is provided.

The strategy and limitations differ between both types of errors:

- **Build error**: For build errors the search occurs in two places:
  - Azure DevOps error messages
  - Build log of failed jobs

- **Test errors**: For test errors the analysis only occurs when the build has up to 25 failing tests. This limitation was put in place due to the cost of reviewing the logs for helix jobs. The analysis uses:
  - Error message
  - Stack trace
  - For Helix tests in the console log

## How to fill out a Known Issue error section

For the error matching to work, you need to provide an error message or an error pattern.

```json
{ 
    "ErrorMessage":"",
    "ErrorPattern": "",
}
```

The `ErrorMessage` value needs to be updated with an error message that meets the following requirements:

1. Match at least one of the error messages of the reported build. For more details see: [How matching process works](#how-the-matching-process-works-between-an-issue-and-a-build-or-test-error)
1. It shouldn't include any unique identifier of the message e.g., machine, path, file name, etc.

    For example, for the following error:

    ```log
    ##[error].dotnet/sdk/6.0.100-rc.1.21411.28/NuGet.RestoreEx.targets(19,5): error : (NETCORE_ENGINEERING_TELEMETRY=Restore) Failed to retrieve information about 'Microsoft.Extensions.Hosting.WindowsServices' from remote source 'https://pkgs.dev.azure.com/dnceng/9ee6d478-d288-47f7-aacc-f6e6d082ae6d/_packaging/c9f8ac11-6bd8-4926-8306-f075241547f7/nuget/v3/flat2/microsoft.extensions.hosting.windowsservices/index.json'.
    ```

    We could choose this part of the message: `(NETCORE_ENGINEERING_TELEMETRY=Restore) Failed to retrieve information`. This is a good option because it targets a particular problem but excludes details of the build, thus making it possible for this same error to match with other builds.

After selecting the message, fill the "ErrorMessage":

```json
{ 
    "ErrorMessage":"(NETCORE_ENGINEERING_TELEMETRY=Restore) Failed to retrieve information" 
}
```

If you need to match an error using a regex, you should use the `ErrorPattern` value.

In the following example, the regular expression `The command .+ failed` would match errors like `The command 'open explorer' failed`

```json
{
    "ErrorPattern": "The command .+ failed",
}
```

## What happens after creating or updating a Known Issue

All builds from the last 24 hours since the issue was opened or updated will be scanned with the error message provided, also the builds that fail after the creation of the issue will be scanned.

The issues analyzed are all infrastructure issues (Known Issues in dotnet/arcade) and repository issues (the Known Issues in the repository of the pull request).

Known Issues matches found will be displayed in the Build Analysis check, the example below shows that the automation found 4 matches with the issue "Tracking issue for CI build timeouts"

![Known match example](./Resources/KnownIssueMatch.png?raw=true)

### Build retry functionality in Known Issues

The build retry setting can be set to 'true' when the build failure on 'ErrorMessage' could be solved by retrying the build.

The following is a good example of the use of "BuilRetry" as the only way to fix this error is by retrying the build.

```json
{ 
    "ErrorMessage":"The agent did not connect within the alloted time",
    "BuildRetry": true
} 
```

The build retry functionality retries a build that, in its first attempt, had a failure that matches the Known Issue 'ErrorMessage' or 'ErrorPattern' and has the 'BuildRetry' set to true.

In the example stated above if a build fails with the error "The agent did not connect within the alloted time" on its first attempt this is going to be retried.

Please note that this has some limitations, meaning if the failure occurred on an attempt different from the first, the build will not be retried.

The limitation has been placed by design because of two reasons:

1. If a build has been retried on more occasions the underlying reason could be something different.
1. Many builds are analyzed and retrying them on multiple occasions can become expensive and problematic.

## How to get onboard

1. This feature is tightly related to the Build Analysis because of that it's necessary to have the `.NET Helix` GitHub application installed in the repo in which you intend to use Known Issues. </br>
To get the application installed, you can contact the [.NET Core Engineering Services team](https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/107/How-to-get-a-hold-of-Engineering-Servicing)
1. For infrastructure issues there are not additional steps because this are part of dotnet/arcade.
1. For the repository issues it's necessary to [Create a label](https://docs.github.com/en/enterprise-server@3.1/issues/using-labels-and-milestones-to-track-work/managing-labels#creating-a-label) on the repository in which you need to open the Repository issue. <br>
The name of the label needs to be `Known Build Error`

## Decide Infrastructure or Repository issue

- **Infrastructure**:
  - The issue is not exclusive to a repository
  - Needs to be investigated by the engineering services (@dotnet/dnceng)
  - It hasn't been reported on [arcade](https://github.com/dotnet/arcade/issues?q=is%3Aopen+is%3Aissue+label%3A%22Known+Build+Error%22)
- **Repository**:
  - The issue is happening in a particular repository
  - The error needs to be investigated by the repository owners.
  - The problem hasn't been reported in the `REPOSITORY`, use the following query to verify that:

    ```text
    https://github.com/dotnet/<REPOSITORY>/issues?q=is%3Aopen+is%3Aissue+label%3A%22Known+Build+Error%22
    ```

## Where to find the Known Issue on the Build Analysis

The Known Issues are listed at the top of the Build Analysis.

E.g.
![Known Issues list](./Resources/KnownIssuesListed.png?raw=true)

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProjects%5CBuild%20Analysis%5CKnownIssues.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProjects%5CBuild%20Analysis%5CKnownIssues.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProjects%5CBuild%20Analysis%5CKnownIssues.md)</sub>
<!-- End Generated Content-->

## Telemetry

Build Analysis sends Known Issues telemetry to the `engineeringdata` Kusto DB. The telemetry includes data about all the matches between build breaks and Known Issues the automation was able to find.

Two tables are used to store this data, `KnownIssues` for build related matches and `TestKnownIssues` for test related matches. You can use the following columns, in both tables, to build queries:

- Build Id
- Build repository
- Known Issue Id
- Known Issue repository
- Pull request that triggered the build (if available)

The query below returns all builds affected by [Known Issue 76454](https://github.com/dotnet/runtime/issues/76454) in the runtime repo

```kusto
KnownIssues
| where IssueId == 76454 and IssueRepository == "dotnet/runtime"
```

## Known Issues board

People can look at active Known Issues using the [.NET Core Engineering Services: Known Build Errors](https://github.com/orgs/dotnet/projects/111) GitHub project. In the board, Known Issues are divided into tabs, one for infrastructure related issues, the second one for repo specific issues and the last one for all infrastructure issue that have been created (opened and closed).

![Known Issues board](./Resources/KnownIssuesBoard.png?raw=true)
