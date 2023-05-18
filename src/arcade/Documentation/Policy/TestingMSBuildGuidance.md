# Guidance for Testing MSBuild functionality

We are currently working on ways to improve our testability of Arcade, including the MSBuild projects and tasks. As we implement functionality to make that easier for folks contributing to Arcade, here is some guidance for validating changes made to MSBuild functionality in the interim: 

- Since it is known how to create mocks and work with dependency injection in C# code, please move as much logic out of MSBuild proj files and into the C# tasks (or related classes) so that it can be unit tested.
- Use mocks and dependency injection where applicable when writing unit tests for the MSBuild tasks in C#. If the code does not have an entry point to implement the ASP.NET Core Dependency Injection framework, use the Setter Injection pattern. 
- Implement scenario tests for MSBuild projects in [Arcade Validation](https://github.com/dotnet/arcade-validation/). Validate your changes by [running a private build](#how-to-validate-a-private-build) of Arcade in Arcade Validation. Include a link of the Arcade Validation run on your Arcade PR. 
- Also see .NET Engineering Services' [Validation Principles and Policy](https://github.com/dotnet/arcade/blob/main/Documentation/Validation/README.md)

## Long-Term Solutions

- We are currently [investigating](https://github.com/dotnet/core-eng/issues/11271) how to unit tests MSBuild projects. 
- We plan to [implement a process](https://github.com/dotnet/core-eng/issues/11273) that will run Arcade Validation as a part of Arcade PRs in the future. The guidance in this document is in place until that work is completed. 
- We want to provide the ability to bootstrap dependency injection into MSBuild tasks through [inheritence](https://github.com/dotnet/arcade/issues/6580).

## How to Validate a Private Build

1. Run a build of your Arcade branch on the [arcade-official-ci](https://dnceng.visualstudio.com/internal/_build?definitionId=6) Azure DevOps Pipeline
2. [Promote your build](../Darc.md#add-build-to-channel) to the "General Testing" Maestro channel. 
3. Create a branch of [Arcade Validation](https://github.com/dotnet/arcade-validation)
4. Using darc, run `update-dependencies` ([update-dependencies documentation](../Darc.md#updating-dependencies-in-your-local-repository)) on your Arcade Validation branch to use the build of Arcade you just created in the previous steps. 
5. Push your branch up to Azure DevOps Arcade Validation repository and run a build of your branch on the [dotnet-arcade-validation-official](https://dnceng.visualstudio.com/internal/_build?definitionId=282) to verify your changes. 
6. It's not necessary to merge your Arcade Validation branch into the repo's main branch, so feel free to delete it when you're done validating your changes.

If you want to also validate your private build of Arcade using a repository other than Arcade Validation, follow these steps. 

1. Provided you have done at least steps 1 and 2 above to create your private build of Arcade and have it published to the "General Testing" channel...
2. Create a branch in the repository you wish to validate your private build of Arcade with. 
3. Add the `general-testing` feed to your `NuGet.config`:   
    ```xml
    <add key="general-testing" value="https://pkgs.dev.azure.com/dnceng/public/_packaging/general-testing/nuget/v3/index.json" />
    ```
    Your private build of Arcade will be pushed to that feed when you publish it to the `General Testing` channel.
4. Using darc, run `update-dependencies` ([update-dependencies documentation](../Darc.md#updating-dependencies-in-your-local-repository)) on your branch to use the build of Arcade you just created in the previous steps. 
5. Build your project and run the project's unit tests locally, and/or build your branch with your project's Azure DevOps pipeline. Ensure that the build pipeline excutes any tests (unit, integration, scenario, et cetera). 

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CPolicy%5CTestingMSBuildGuidance.md)](https://helix.dot.net/f/p/5?p=Documentation%5CPolicy%5CTestingMSBuildGuidance.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CPolicy%5CTestingMSBuildGuidance.md)</sub>
<!-- End Generated Content-->
