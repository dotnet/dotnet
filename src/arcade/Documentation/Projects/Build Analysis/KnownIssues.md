# Known Issues

Known Issues are GitHub issues that are created to report and track known build or test errors.

[Build Analysis](https://github.com/dotnet/arcade/blob/main/Documentation/Projects/Build%20Analysis/Introduction.md) shows a list of Known Issues for any build or test error that matches an existing Known Issue. This helps the developers to identify when a build break is not specific to their build. This information is displayed at the top of Build Analysis.

All reported Known Issues can be found on [Known Issues project](https://github.com/orgs/dotnet/projects/111).

## When and how to report a Known Issue

A Known Issue should be reported when you find a build or test error that is not caused by your changes and that affects or could affect more builds.

There are two types of Known Issues:

- **Infrastructure Known Issue**: An infrastructure issue is an issue that is not specific to your repository and that requires investigation by the engineering services (@dotnet/dnceng)
- **Repository Known Issue**: A repository issue is an issue that occurs in a particular repository and that should be investigated by the repository owners.

There are two ways to report a Known Issue: one is via Build Analysis and the other one is manually.

### Reporting Known Issues via Build Analysis

1. In Build Analysis, you will see links for the type of issue you want to open.
![Links to report Known Issue](./Resources/KnownIssuesLinks.png?raw=true)
1. Click on the link of the [type of issue that suits the situation better](#decide-infrastructure-or-repository-issue).
1. A template is going to appear for you and most of this information should be already prefilled.
1. [Fill out the JSON blob](#filling-out-known-issues-json-blob) accordingly

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

### Filling out Known Issues JSON blob

1.  [Fill the "Error message / Error pattern" section](#how-to-fill-out-a-known-issue-error-section).
1. If the issue reported can be solved by retrying the build you can consider setting the ["Build Retry" configuration](#build-retry-functionality-in-known-issues) to `true`
1. You should set `ExcludeConsoleLog` to `true` if you want to exclude console logs from Known Issues matching
1. You are done but [what happens after a Known Issue is created?](#what-happens-after-creating-or-updating-a-known-issue)

Note: You need to escape special characters to make your JSON valid, to do it you can use online tools like [freeformatter](https://www.freeformatter.com/json-escape.html).

For example, if your error message is `text "using" quotes`

![Unescaped text example](./Resources/UnescapedText.png?raw=true)

You need to enter `text \"using\" quotes` as the `ErrorMessage` value

![Escaped text example](./Resources/EscapedText.png?raw=true)

You can use the preview tab to validate your JSON blob, GitHub will highlight invalid characters.

![Invalid JSON blob](./Resources/invalid_json_blob.png?raw=true)

## How the matching process works between a Known Issue and a build/test error

The Known Issues feature identifies build and test errors and matches them with open Known Issues. It uses `String.Contains` to compare the errors with the “ErrorMessage” property of Known Issues, or regex matching if an “ErrorPattern” property is provided.

The matching process and its limitations depend on the type of error:

- **Build error**: For build errors, the feature searches for matches in two sources:
  - Azure DevOps error messages
  - Build log of failed jobs

- **Test errors**:  For test errors, the feature analyzes the errors only when the build has 1000 or fewer failing tests. It also limits the analysis to 100 Helix logs. This limitation is due to the high cost of analyzing tests and Helix logs. The feature uses the following information to find matches:
  - Error message
  - Stack trace
  - Helix log for Helix tests

## How to fill out a Known Issue error section

For the error matching to work, you need to provide an error message or an error pattern.

```json
{
    "ErrorMessage":"",
    "ErrorPattern": "",
}
```

### String matching

The `ErrorMessage` value needs to be updated with an error message that meets the following requirements:

1. Match at least one of the error messages of the reported build. For more details see: [How matching process works](#how-the-matching-process-works-between-an-issue-and-a-build-or-test-error)
1. It shouldn't include any unique identifier of the message e.g., machine, path, file name, etc.

    For example, for the following error:

    ```log
    ##[error].dotnet/sdk/6.0.100-rc.1.21411.28/NuGet.RestoreEx.targets(19,5): error : (NETCORE_ENGINEERING_TELEMETRY=Restore) Failed to retrieve information about 'Microsoft.Extensions.Hosting.WindowsServices' from remote source 'https://pkgs.dev.azure.com/dnceng/9ee6d478-d288-47f7-aacc-f6e6d082ae6d/_packaging/c9f8ac11-6bd8-4926-8306-f075241547f7/nuget/v3/flat2/microsoft.extensions.hosting.windowsservices/index.json'.
    ```

    We could choose this part of the message: `(NETCORE_ENGINEERING_TELEMETRY=Restore) Failed to retrieve information`. This is a good option because it targets a particular problem but excludes details of the build, thus making it possible for this same error to match with other builds.

After selecting the message, fill `ErrorMessage` value:

```json
{ 
    "ErrorMessage":"(NETCORE_ENGINEERING_TELEMETRY=Restore) Failed to retrieve information" 
}
```

### Regex matching

If you need to match an error using a regex, you should use the `ErrorPattern` value.

In the following example, the regular expression `The command .+ failed` would match errors like `The command 'open explorer' failed`

```json
{
    "ErrorPattern": "The command .+ failed",
}
```

We recommend you test your regular expression, to do it you can use [regex101 tester](https://regex101.com/) (choose `.NET (C#)` flavor) with the following regex options:

- Single line
- Insentitive
- No backtracking

## What happens after creating or updating a Known Issue

Known Issues scan all builds from the last 24 hours since the issue was opened or updated with the error message provided. It also scans all builds that fail after the issue is created.

Known issues analyzes both infrastructure issues (Known Issues in dotnet/arcade) and repository issues (Known Issues in the pull request’s repository).

Additionally displays the Known Issues matches in the Build Analysis check. The example below shows that the feature found 4 matches for the issue “Tracking issue for CI build timeouts”.

![Known match example](./Resources/KnownIssueMatch.png?raw=true)

### Build retry functionality in Known Issues

Build Analysis can retry a build that fails with an error that matches a Known Issue. To enable this functionality, the Known Issue must have the ‘BuildRetry’ property set to true. This property indicates that the build failure may be solved by retrying the build.

The following is an example of a Known Issue with the ‘BuildRetry’ property:

```json
{ 
    "ErrorMessage":"The agent did not connect within the alloted time",
    "BuildRetry": true
} 
```

In this example, if a build fails with the error “The agent did not connect within the allotted time” on its first attempt, the feature will retry the build.

However, this functionality has some limitations. It only retries the build if the failure occurs on the first attempt. If the failure happens on a later attempt, the build will not be retried.

This limitation is designed for two reasons:
1. If a build fails multiple times, the underlying cause may be something different.
1. Many builds are analyzed and retrying them repeatedly can be costly and problematic.

## How to get onboard

1. This feature depends on the Build Analysis feature. Therefore, you need to have the `Build Analysis` GitHub application installed in the repo where you want to use Known Issues. </br> To install the application, you can contact the [.NET Core Engineering Services team](https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/107/How-to-get-a-hold-of-Engineering-Servicing)
1. For infrastructure issues, there are no additional steps because they are part of dotnet/arcade.
1. For repository issues, you need to [Create a label](https://docs.github.com/en/enterprise-server@3.1/issues/using-labels-and-milestones-to-track-work/managing-labels#creating-a-label) on the repo where you want to open the Repository issue. <br> The name of the label must be `Known Build Error`

## Decide Infrastructure or Repository issue

- **Infrastructure**:
  - The issue is not exclusive to a repository
  - Needs to be investigated by the engineering services (@dotnet/dnceng)
  - It hasn't been reported on [arcade](https://github.com/orgs/dotnet/projects/111/views/1)
- **Repository**:
  - The issue is happening in a particular repository
  - The error needs to be investigated by the repository owners.
  - The problem hasn't been reported in the `REPOSITORY`, use the following query to verify that:

    ```text
    https://github.com/dotnet/<REPOSITORY>/issues?q=is%3Aopen+is%3Aissue+label%3A%22Known+Build+Error%22
    ```
    or see all repository issues at [Repository issues board](https://github.com/orgs/dotnet/projects/111/views/2)

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
