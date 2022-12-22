# Security Builds for .NET Core

This document describes security builds of .NET Core.

- [How to setup a security build](#how-to-setup-a-security-build)
- [How to kickoff a security build](#how-to-kickoff-a-security-build)
- [How to get values for queue variables](#how-to-get-values-for-queue-variables)
- [How to access and resolve security issues](#how-to-access-and-resolve-security-issues)


## How to setup a security build

Security Development Lifecycle ([SDL](http://sdl/)) specifies the minimum security requirements that must be satisfied before making a Microsoft software or service available to customers. To help product teams fulfill the security requirements, SDL team provides a few tools and services, in addition to detailed guidance and a dedicated support team. Some these tools and services are available as a VSTS extension called Secure Development Tools ([SDT](https://www.1eswiki.com/wiki/Secure_Development_Tools_VSTS_Extension)), which is a collection of build tasks. These build tasks can be added to a VSTS build definition. 

Trust Services Automation ([TSA](http://sql/wiki/Trust_Services_Automation_%28TSA%29)) is a service that analyzes the logs produced for security tools, identifies regressions, creates workitems to track the regressions, and generates a detailed report. One of the tasks in SDT extension is to collect logs from security tools and upload them for processing at TSA. This allows product teams to setup a VSTS build definition that acquire the latest version of security tools, run the tools against the product, gather and analyze logs, detect regressions, and prepare reports.

Security build for .NET Core is a VSTS build definition that uses SDT extension. A security build does not involve building the product from source. This build operates on build artifacts of an official build.  The approach for security build can be summarized as follows.

 1. Download the packages, using `sync` command, for the specified official build Id
 2. Extract assemblies and symbols from the packages
 3. Run security tasks that scan assemblies. Use APIScan and BinSkim tasks from SDT extension.
 4. Get the sources at the SHA specified in `version.txt`, which is obtained when packages are extracted at step #2
 5. Run security tasks that scan source code. Use CredScan and PoliCheck tasks from SDT extension.
 6. Gather logs and upload to TSA. Use publish task in SDT extension.

SDT extension currently support 4 tools that are applicable to .NET Core. A short description of each tool is shown in the table below.

|Tool|Description|
|:---|:----------|
| BinSkim | Validates compiler/linker settings and other security-relevant binary characteristics.|
| APIScan | Determines whether or not the software complies with the API Usage Standard of the Interoperability Policy.|
| CredScan | Index and scan for credentials or other sensitive content.|
| PoliCheck | Scan code, code comments, and content for words that may be sensitive for legal, cultural, or geopolitical reasons.|


.NET Core security build definitions and link to the report is listed in the table below.


|Build Definition|TSA Report|
|:---------------|:---------|
| [CoreFx](https://devdiv.visualstudio.com/DevDiv/_build/index?context=allDefinitions&path=%5CDotNet%5CSecurity&definitionId=6552&_a=completed) | [CoreFx-master](http://aztsa/api/Result/CodeBase/DotNet-CoreFx-Trusted_master/Summary) |
| [CoreCLR](https://devdiv.visualstudio.com/DevDiv/_build/index?context=allDefinitions&path=%5CDotNet%5CSecurity&definitionId=6598&_a=completed) | [CoreCLR-master](http://aztsa/api/Result/CodeBase/DotNet-CoreCLR-Trusted_master/Summary) |
| [Core-Setup](https://devdiv.visualstudio.com/DevDiv/_build/index?context=allDefinitions&path=%5CDotNet%5CSecurity&definitionId=6658&_a=completed) | [Core-Setup-master](http://aztsa/api/Result/CodeBase/DotNet-Core-Setup-Trusted_master/Summary) |
| [CLI](https://devdiv.visualstudio.com/DevDiv/_build/index?context=allDefinitions&path=%5CDotNet%5CSecurity&definitionId=6698&_a=completed) | [CLI-master](http://aztsa/api/Result/CodeBase/DotNet-CLI-Trusted_master/Summary) |

In the current setup, a security build is triggered manually. Official Id and corresponding Azure container name  needs to be provided at the time of queuing the build. In near future, Maestro will be extend to determine the Official Id and container name, and trigger a security build automatically.

TSA is configured to send an email report for each scan or security build to [dncsec](dncsec@microsoft.com) that include .NET Core repository owners responsible for security issues. Repository owners should focus on new issues and regressions highlighted in the report, and take necessary action to resolve those issues.

## How to kickoff a security build

Kickoff of a security build is as simple as queuing a VSTS build definition. While queuing, values for four input variables need to be provided. These variables are as follows:

 - *PB_BuildNumber* - official build Id of the repository.
 - *PB_CloudDropContainer* - name of the Azure container from where the packages published from the official build (*PB_BuildNumber*) can be downloaded.
 - *CodeBase* - TSA codebase that corresponds to the branch. For example, `master` or `2.0.0`.
 - *NotificationAlias* - A comma separated email Ids where the TSA report should be sent.

For example, a recent build Id of CoreCLR `2.0.0` branch is `20170621-01`. Packages produced from this build were published to Azure container named `coreclr-preview3-20170621-01` . To launch a security build that will scan the assemblies and source code from that official build, perform the following steps:

 1. Navigate to CoreCLR security build [definition](https://devdiv.visualstudio.com/DevDiv/_build/index?context=allDefinitions&path=%5CDotNet%5CSecurity&definitionId=6598&_a=completed)
 2. Click "Queue new build"
 3. Enter the variable values:
      - *PB_BuildNumber* = `20170621-01`
      - *PB_CloudDropContainer* = `coreclr-preview3-20170621-01` 
      - *CodeBase* = `2.0.0`
      - *NotificationAlias*  = `dncsec@microsoft.com,joc@microsoft.com`
      
      Refer to the screenshot below. See [how to get values for queue variables](#how-to-get-values-for-queue-variables)
 4. Click OK to start the build 

----------
![QueueSecurityBuild.](./assets/QueueSecurityBuild.png?raw=true)

----------

#### Core-Setup

Core-Setup requires an additional queue variable called `PB_BlobName`, which is the name of the Azure Storage blob that contains the packages produced from the official build under test. This blob is under the default container named `dotnet`. 

----------
![QueueCoreSetup.](./assets/QueueCoreSetup.png?raw=true)

----------

#### CLI

CLI builds are fully automated. This means no variable needs to be set at the time of queuing.
A build is triggerred everyday around midnight. Build downloads the latest packages (zip) from Azure Storage corresponding to the branch. For example, latest packages of `master` branch are downloaded from (https://dotnetcli.blob.core.windows.net/dotnet/Sdk/master). SHA and build number are read from `latest.version` file (https://dotnetcli.blob.core.windows.net/dotnet/Sdk/master/latest.version).


As described in the earlier section, when the build finishes successfully, an email report of the security build is sent to listed email Ids. The same report can be viewed online. For example, report for CoreCLR `2.0.0` will be at TSA [website](http://aztsa/api/Result/CodeBase/DotNet-CoreCLR-Trusted_2.0.0/Summary)


### How to get values for queue variables

Team dashboard [MC](https://mc.dot.net) is the place to begin when looking for details about .NET Core builds. In the dashboard, navigate to .NET Core release branch such as [2.0.0](https://mc.dot.net/#/product/netcore/200) to get the summary of most recent builds. Described below is how to get the values for queue variables for each .NET Core repository's security build. 

*PB_BuildNumber* is the official build number, and is a required variable in security build of all four repositories. To determine this build number for a repository, navigate to the dashboard, identify the most recent build under the corresponding repository. Build number is usually in a year-month-day format. For example, `20170622.01`. Replace the dot with a hyphen.  In this example,  *PB_BuildNumber* is `20170622-01`. 

*PB_CloudDropContainer* is the name of the container where the packages produced from *PB_BuildNumber* build are stored. To get this container name, in the dashboard, click on the build number link or button. In the details, click the URL against `buildUri` to navigate to VSTS build. Navigate to the log for `PipeBuild.exe` task in this VSTS build, and locate the container name as described below.


#### CoreFx

In case of CoreFx, container name is the value against `PB_Label`. Shown below is a portion of `PipeBuild.exe` task log showing the container name.

>OfficialBuildId=20170622-01 PB_SignType=real PB_Label=**corefx-preview1-20170622-01** SourceVersion...


#### CoreCLR

In case of CoreCLR, container name is `Label`. Shown below is a portion of `PipeBuild.exe` task log showing the container name.

>OfficialBuildId=20170622-01 SignType=real Label=**coreclr-preview3-20170622-01** SourceVersion...

#### Core-Setup

In Core-Setup, the default container name (*PB_CloudDropContainer*) is `dotnet`. An additional variable named `PB_BlobName` is required for security build of Core-Setup.  To locate this value, open `PipeBuild.exe` task log, search for the build leg named `Core-Setup-Publish`, and click the URL against this to navigate to the build leg. Example fragment from the log is shown below.

 >Core-Setup-Publish - https://devdiv.visualstudio.com/DefaultCollection/DevDiv/_build?_a=summary&buildId=820812...

In the build leg, locate text similar to the fragment shown below.

>Downloading **Runtime/2.0.0-preview3-25422-01**...


`Runtime/2.0.0-preview3-25422-01` is the value for `PB_BlobName`.

----------


## How to access and resolve security issues

For each successful build, TSA analyzes the logs from the build, and create issues, which are VSTS workitems. Query to the workitems will be in the email report sent to `dncsec`. As mentioned earlier, the report can be accessed at TSA reports, whose URL is in the format - `http://aztsa/api/Result/CodeBase/<codebase-name>/Summary`. For example, CoreFx master is at `http://aztsa/api/Result/CodeBase/DotNet-CoreFx-Trusted_master/Summary`

Repository owner is responsible to triage, and drive towards resolving all security issues logged against the codebase. There are certain cases where an issue cannot be fixed, a few of them are summarized below.

#### Case #1: External 

Say an issue is with an assembly that is not owned or built by the repository, then resolve the issue by setting the following attribute-value in the workitem.

|Attribute|Value|
|:--------|:----|
| State|Done |
| Reason | Work Finished |
| Status | Resolved |
| Resolution | Will not Fix |

TSA will stop reporting such issues in future builds.

#### Case #2: Configuration 

Say there was a configuration error while launching the build. For example, the branch name was set to `2.0.0`  instead of `master`. This will pollute TSA codebase with issues from other branch. So, to cleanup the codebase, resolve such configuration issues by setting the following attribute-value in the workitem.

|Attribute|Value|
|:--------|:----|
| State|Done |
| Reason | Work Finished |
| Status | Resolved |
| Resolution | Configuration/Environment |


For any questions about security builds, please contact [dncsec](dncsec@microsoft.com).


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5Csecurity-builds.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5Csecurity-builds.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5Csecurity-builds.md)</sub>
<!-- End Generated Content-->
