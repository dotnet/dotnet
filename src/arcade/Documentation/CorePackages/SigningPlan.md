# Signing Plan
The goal of the signing plan is to provide guidance on how the tier 1 (T1) product repositories should sign binaries that are going to be published.

## Requirements
- Leverage pre-existing solutions so that work isn't duplicated.
- Where possible, avoid dependency on software installed on the machine to facilitate the consumption of the tool across the .NET Core repositories.
- The consumption method of the SignTool should follow the [Methods for Consuming the .NET Core Shared Infrastructure Components](https://github.com/dotnet/arcade/blob/master/Documentation/Overview.md#methods-for-consuming-the-net-core-shared-infrastructure-components).
- Signtool will need a list of containers in order to know what to sign.

## Roadmap
1. (S137) Migrate SignTool from Repo tool set to Arcade.
2. (S138) Add SignTool to Arcade SDK.
3. (S138) Arcade should sign its packages using the SignTool that is in the SDK.
4. (S138 - S139) Refactor the SignTool to accommodate scenarios for other repositories. Examples of the changes are:
    - Convert to MsBuild task.
    - Accept a list of containers that need to be signed. Those containers will be expanded and nested assemblies signed. Current support will be to VSIX and NuGet packages.
    - Read the strong name from the metadata of the file.
5. (S140) Onboard one repository from T1 to use the SignTool from the SDK to sign its binaries.
6. (S140) Once validated in a repository, start onboarding the other T1 repositories.

## Sprint to dates
- S137: 6/11 - 6/29 
- S138: 7/2 - 7/20 
- S139: 7/23 - 8/10 
- S140: 8/13 - 8/31 


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CCorePackages%5CSigningPlan.md)](https://helix.dot.net/f/p/5?p=Documentation%5CCorePackages%5CSigningPlan.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CCorePackages%5CSigningPlan.md)</sub>
<!-- End Generated Content-->
