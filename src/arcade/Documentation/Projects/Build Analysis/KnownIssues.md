# Known issues
The "known issues" are GitHub issues that were created with the purpose of reporting known build or test errors.

Whenever a build has an error that matches with one of the already existing known issues, this is going to be listed on the [build analysis](https://github.com/dotnet/arcade/blob/main/Documentation/Projects/Build%20Analysis/Introduction.md), helping the developer to know when a build break is not exclusive to their build. 
This information is going to be listed at the top of the build analysis.

 All the reported known issues can be found on [known issues project](https://github.com/orgs/dotnet/projects/111).

## When and how to report a known issue
A known issue should be reported when you find a build or test error that is not caused by your changes and that is affecting or could affect more builds. 

There are two types of known issues:
- **Infrastructure**: An infrastructure issue is an issue that is not exclusive to your repository and that needs to be investigated by the engineering services (@dotnet/dnceng)
- **Repository**: A repository issue is an issue that is happening in a particular repository and that should be investigated by the repository owners.

There are two ways to report a known issue, one is via the build analysis and the other is manually.

## How the matching process works between an issue and a build or test error
The known issues feature can find build and test errors, in both cases the match is resolved by a **contains** comparison. 

The strategy and limitations differ between both types of errors:

- **Build error**: For build errors the search occurs in two places using “contains” to compare them:
    - AzDO error message 
    - Logs for failing jobs

- **Test errors**: For test errors the analysis only occurs when the build has up to 25 failing tests. This limitation was put in place due to the cost of reviewing the logs for helix jobs, the errors are searched using “contains” in: 
    - Error message
    - Stack trace
    - For Helix tests in the console log

### Reporting known issue manually
1. Decide if you need to open a [repository issue or infrastructure issue](#decide-infrastructure-or-repository-issue)
1. Open a new issue, choosing the repository in which you are opening the issue based on following rule:
    - Infrastructure issue - arcade
    - Repository issue - In the repo in which the issue is happening
1. Add the label `Known Build Error`. (If the label is not available on the repository follow the instructions to [get on board](#how-to-get-onboard))
1. Copy and paste the template
    ````md 
    ## Build Information
    Build: <!-- Add link to the build with the reported error. -->
    Leg Name: <!-- Add the name of the impacted leg. -->

    ## Error Message
    <!-- Fill for repository issues. For infrastructure issues the engineering services (@dotnet/dnceng) is going to fill it. -->
    ```json 
    { "ErrorMessage":"",
      "BuildRetry": false
    } 
    ```
    ````
1. If you are opening a Repository issue you need to [fill the "Error message" section](#how-to-fill-out-a-known-issue-error-message-section"). If you are opening an infrastructure issue, this is going to be handled by the engineering services team.
1. If the issue reported on the "ErrorMessage" section can be solved by retrying the build you can consider setting the ["Build Retry" configuration](###build-retry-functionality-in-known-issues) to ``true``
1. You are done but [what happens after a knonw issue is created?](#what-happens-after-creating-or-updating-a-known-issue)

### Reporting known issue via build analysis
1. On the build analysis you will see links for the type of issue you want to open. 
![](./Resources/KnownIssuesLinks.png?raw=true)
1. Click on the link of the [type of issue that suits better the situation](#decide-infrastructure-or-repository-issue).
1. A template is going to appear for you and most of this information should be already prefilled.
1. If you are opening a Repository issue you need to [fill the "Error message" section](#how-to-fill-out-a-known-issue-error-message-section"). If you are opening an infrastructure issue, this is going to be handled by the engineering services team.
1. If the issue reported on the "ErrorMessage" section can be solved by retrying the build you can consider setting the ["Build Retry" configuration](###build-retry-functionality-in-known-issues) to ``true``
1. You are done but [what happens after a knonw issue is created?](#what-happens-after-creating/updating-a-known-issue)

## How to fill out a known issue error message section
The "ErrorMessage section" is on the next form:
```json 
{ "ErrorMessage":"" } 

```

And the "ErrorMessage" value needs to be updated with an error message 
that meets the following requeriments:
1. Match at least one of the error messages of the reported build. For more details see: [How matching process works](#how-the-matching-process-works-between-an-issue-and-a-build-or-test-error)
1. It shouldn't include any unique identifier of the message e.g., machine, path, file name, etc.


    For example, for the following error:

    ```log
    ##[error].dotnet/sdk/6.0.100-rc.1.21411.28/NuGet.RestoreEx.targets(19,5): error : (NETCORE_ENGINEERING_TELEMETRY=Restore) Failed to retrieve information about 'Microsoft.Extensions.Hosting.WindowsServices' from remote source 'https://pkgs.dev.azure.com/dnceng/9ee6d478-d288-47f7-aacc-f6e6d082ae6d/_packaging/c9f8ac11-6bd8-4926-8306-f075241547f7/nuget/v3/flat2/microsoft.extensions.hosting.windowsservices/index.json'.
    ```

    We could choose this part of the message: `(NETCORE_ENGINEERING_TELEMETRY=Restore) Failed to retrieve information`. This is a good option because it targets a particular problem but excludes details of the build, thus making it possible for this same error to match with other builds. 

After selecting the message, fill the "ErrorMessage";

```json 
{ 
    "ErrorMessage":"(NETCORE_ENGINEERING_TELEMETRY=Restore) Failed to retrieve information" 
}
```

## What happens after creating or updating a known issue 
All builds from the last 24 hours since the issue was opened or updated will be scanned for the error message, also 
the builds that fail after the creation of the issue will be scanned.

The issues analyzed are all the infrastructure issues (the known issues on dotnet/arcade) and the repository issues (the known issues in the repository of the pull request).

### Build retry functionality in known issues
The build retry setting can be set to 'true' when the build failure on 'ErrorMessage' could be solved by retrying the build.

The following is a good example of the use of "BuilRetry" as the only way to fix this error is by retrying the build 
```json 
{ 
    "ErrorMessage":"The agent did not connect within the alloted time",
    "BuildRetry": true
} 
```

The build retry functionality retries a build that, in its first attempt, had a failure that matched the known issue 'ErrorMessage' and has the 'BuildRetry' set to true. 

In the example stated above if a build fails with the error "The agent did not connect within the alloted time" on its first attempt this is going to be retried.

Please note that this has some limitations, meaning if the failure occurred on an attempt different from the first, the build will not be retried. 

The limitation has been placed by design because of two reasons:
1. If a build has been retried on more occasions the underlying reason could be something different. 
1. Many builds are analyzed and retrying them on multiple occasions can become expensive and problematic. 

## How to get onboard
1. This feature is tightly related to the build analysis because of that it's necessary to have the `.NET Helix` GitHub application installed in the repo in which you intend to use known issues. </br>
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
    - It hasn't been reported in the `REPOSITORY`, use the following query to verify that:
        ```
        https://github.com/dotnet/<REPOSITORY>/issues?q=is%3Aopen+is%3Aissue+label%3A%22Known+Build+Error%22
        ```

## Where to find the known issue on the build analysis
The known issues are listed at the top of the build analysis. <br>
E.g.
![](./Resources/KnownIssuesListed.png?raw=true)


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProjects%5CBuild%20Analysis%5CKnownIssues.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProjects%5CBuild%20Analysis%5CKnownIssues.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProjects%5CBuild%20Analysis%5CKnownIssues.md)</sub>
<!-- End Generated Content-->
