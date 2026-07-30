# Guidance for Testing MSBuild functionality

We are currently working on ways to improve our testability of Arcade, including the MSBuild projects and tasks. As we implement functionality to make that easier for folks contributing to Arcade, here is some guidance for validating changes made to MSBuild functionality in the interim: 

- Since it is known how to create mocks and work with dependency injection in C# code, please move as much logic out of MSBuild proj files and into the C# tasks (or related classes) so that it can be unit tested.
- Use mocks and dependency injection where applicable when writing unit tests for the MSBuild tasks in C#. If the code does not have an entry point to implement the ASP.NET Core Dependency Injection framework, use the Setter Injection pattern.
- All MSBuild tasks in dotnet/arcade should use the `Runtime="NET"` task host feature, which allows .NETCoreApp-compiled tasks to run in Visual Studio MSBuild. With this, tasks should target only .NETCoreApp.
- Also see .NET Engineering Services' [Validation Principles and Policy](../Validation.md)

## How to Validate a Private Build

If you want to also validate your private build of Arcade using a repository, follow these steps. 

1. Run a build of your Arcade branch on the [arcade-official-ci](https://dev.azure.com/dnceng/internal/_build?definitionId=6) Azure DevOps Pipeline
2. [Promote your build](../Darc.md) to the "General Testing" Maestro channel.
3. Create a branch in the repository you wish to validate your private build of Arcade with. 
4. Add the `general-testing` feed to your `NuGet.config`:   
    ```xml
    <add key="general-testing" value="https://pkgs.dev.azure.com/dnceng/public/_packaging/general-testing/nuget/v3/index.json" />
    ```
    Your private build of Arcade will be pushed to that feed when you publish it to the `General Testing` channel.
5. Using darc, run `update-dependencies` ([update-dependencies documentation](../Darc.md)) on your branch to use the build of Arcade you just created in the previous steps.
6. Build your project and run the project's unit tests locally, and/or build your branch with your project's Azure DevOps pipeline. Ensure that the build pipeline executes any tests (unit, integration, scenario, et cetera).
