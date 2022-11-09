# Automate SDL tools run in AzDO Pipelines

## SDL Scripts

SDL scripts are a set of powershell scripts that provide the ability to run SDL tools. The scripts are located [here](../eng/common/sdl/)

## How to add the SDL scripts to pipeline

The pre-reqs needed to run the SDL scripts are defined with default values in a template - [execute-sdl.yml](../eng/common/templates/job/execute-sdl.yml). The template is added part of [post-build.yml](../eng/common/templates/post-build/post-build.yml) and is turned-off by default and can be enabled by setting the parameter [SDLValidationParameters.enable](../eng/common/templates/post-build/post-build.yml#L6). 

## Arguments

All arguments that are not repo specific have default values specified to them, so only provide values for them if there is a need to override.

| Name                    | Type     | Description                                                  |
| ----------------------- | -------- | ------------------------------------------------------------ |
| GuardianPackageName     | string   | the name of guardian CLI package. Default is provided in post-build.yml. |
| NugetPackageDirectory   | Dir Path | directory where NuGet packages are installed . The default is provided in post-build.yml. |
| Repository              | string   | the name of the repository (e.g. dotnet/arcade). The default is populated from the current build. |
| BranchName              | string   | name of branch or version of gdn settings; The default is populated from the current build. |
| SourceDirectory         | Dir Path | the directory where source files are located. The default is populated from the current build.|
| ArtifactsDirectory      | Dir Path | the directory where build artifacts are located. The default is populated from the current build. |
| AzureDevOpsAccessToken  | string   | access token to access internal AzDO repo which maintains the baseline data |
| **SourceToolsList**     | Array    | list of SDL tools to run on source code |
| **ArtifactToolsList**   | Array    | list of SDL tools to run on build artifacts |
| TsaPublish              | bool     | true will publish results to TSA; only set to true after onboarding to TSA; TSA is the automated framework used to upload test results as bugs.|
| TsaBranchName           | string   |  TSA Parameter; The default is populated from the current build |
| TsaRepositoryName       | string   | TSA Parameter; The default is populated from the current build |
| BuildNumber             | string   | TSA Parameter; The default is populated from the current build |
| UpdateBaseline          | bool     | default value is false; if true, will update the baseline in the repository; should only be run after fixing any issues which need to be fixed |
| TsaOnboard              | bool     | TSA Parameter; default value is false; if true, will onboard the repository to TSA; should only be run once |
| **TsaInstanceUrl**      | string   | TSA Parameter; the instance-url registered with TSA;  |
| **TsaCodebaseName**     | string   | TSA Parameter; the name of the codebase registered with TSA; |
| **TsaProjectName**      | string   | TSA Parameter; the name of the project registered with TSA; |
| **TsaNotificationEmail**| string   | TSA Parameter; the email(s) which will receive notifications of TSA bug filings (e.g. alias@microsoft.com); |
| **TsaCodebaseAdmin**    | string   | TSA Parameter; the aliases which are admins of the TSA codebase (e.g. DOMAIN\alias); |
| **TsaIterationPath**    | string   | TSA Parameter; the area path where TSA will file bugs in AzDO; |
| GuardianLoggerLevel     | string   | TSA Parameter; the iteration path where TSA will file bugs in AzDO; |

**Note:**

- Items in bold are repo specific and do not carry a default.
- All TSA parameters are needed only if `TsaPublish` and / or `TsaOnBoard` is set to true.

## Usage Examples

### Arcade 
Arcade has enabled SDL runs in official-ci builds.
- [azure-pipeline.yml](https://github.com/dotnet/arcade/blob/master/azure-pipelines.yml#L192)
- [Build](https://dev.azure.com/dnceng/internal/_build/results?buildId=236348&view=logs&s=3df7d716-4c9c-5c26-9f45-11f62216640d&j=7d9eef18-6720-5c1f-4d30-89d7b76728e9)

```yml
    SDLValidationParameters:
        enable: true
        params: ' -SourceToolsList @("xyz","abc")
        -ArtifactToolsList @("def")
        -TsaInstanceURL "https://devdiv.visualstudio.com/"
        -TsaProjectName "DEVDIV"
        -TsaNotificationEmail "xxx@microsoft.com"
        -TsaCodebaseAdmin "aa\bb"
        -TsaBugAreaPath "DevDiv\NET Core "
        -TsaIterationPath "DevDiv"
        -TsaRepositoryName "Arcade"
        -TsaCodebaseName "Arcade"
        -TsaPublish $True'
```

## SDL run failures filed as bugs

If `TsaPublish` is set to true, the output of the SDL tool runs for every build will be published to the account specified under `TsaInstanceURL` and `TsaProjectName`. 

If `TsaNotificationEmail` is set, a notification email will be sent out with a link to the bugs filed for each tool run. 

[Here](https://devdiv.visualstudio.com/DevDiv/_queries/query/?wiql=%20%20%20%20SELECT%20ID%2CSeverity%2CState%2C%5BAssigned%20To%5D%2CTitle%20FROM%20WorkItem%20WHERE%20Tags%20Contains%27TSA%23178337-Arcade-PoliCheck-12345.6%27%20%20%20%20) is the link to bugs filed after a test run for Arcade.

## See Also
- [SDL Control Flow Document](https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/442/ArcadeSecurityControlFlowDocumentation)
- [Introduction to Guardian and TSA](https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/443/IntroToGuardianAndTSA)


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CHowToAddSDLRunToPipeline.md)](https://helix.dot.net/f/p/5?p=Documentation%5CHowToAddSDLRunToPipeline.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CHowToAddSDLRunToPipeline.md)</sub>
<!-- End Generated Content-->
