# Validation for Helix API and Services

## Pipelines and Jobs
The following are the pipelines and jobs currently running validation, and an example of the kinds of validation that runs during the course of the pipeline. To determine which pipeline your tests should run, compare to what is currently available and use your judgement about where your test would best fit in. 
- **PRs** : <br />
    [Build -> Validate](https://dnceng.visualstudio.com/internal/_build?definitionId=620&_a=summary)
    - *Unit and Functional Tests*: Any test that follows the [validation pattern](../Validation/ValidationProcess.md#unit-testing) for unit tests and is not categorized as "PostDeployment", "PreDeployment", or "Nightly", will run during this stage.
    - *Code Coverage*: The tests that run above will be used to determine how much code is currently being covered by tests. The Code Coverage tab can be viewed after the completion of the job in Azure DevOps. 

- **CI / Staging**: <br />
    [Build -> Validate](https://dnceng.visualstudio.com/internal/_build?definitionId=620&_a=summary) -> [Pre-Deploy -> Deploy -> Post-Deploy](https://dnceng.visualstudio.com/internal/_build?definitionId=696&_a=summary)
    - *Pre-Deployment Validation*: Powershell scripts. Types of [pre-deployment validation](https://github.com/dotnet/core-eng/blob/main/Documentation/Validation/ValidationProcess.md#pre-deployment)
      - Validate HMS Deployment
      - Validate Service Fabric Applications
      - Validate Resource Groups and Storage Accounts
    - *Post-Deployment Validation*: In **Helix.Test.Staging.PostDeployment** test project. Types of [post-deployment validation](https://github.com/dotnet/core-eng/blob/main/Documentation/Validation/ValidationProcess.md#post-deployment)
      - Helix API Tests

- **Production**:<br />
    [Build -> Validate](https://dnceng.visualstudio.com/internal/_build?definitionId=620&_a=summary) -> [Pre-Deploy -> Deploy](https://dnceng.visualstudio.com/internal/_build?definitionId=697&_a=summary)
    - No Post-Deployment Validation as they would be queued behind customer jobs. 

- **[Nightly](https://dev.azure.com/dnceng/internal/_build?definitionId=622&_a=summary)**:
    - SDL/Bin Skim
    - **Helix.Test.Staging.Nightly** project: Types of [nightly validation](https://github.com/dotnet/core-eng/blob/main/Documentation/Validation/ValidationProcess.md#nightly)
      - Sends jobs that echo "hello world" to each queue in staging (currently, only open queues)

## Where do the Tests live?
Per the [Validation Process](https://github.com/dotnet/core-eng/blob/main/Documentation/Validation/ValidationProcess.md#unit-testing) documentation, tests will live within the solution of the project being tested. 

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CValidation%5CHelixValidation.md)](https://helix.dot.net/f/p/5?p=Documentation%5CValidation%5CHelixValidation.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CValidation%5CHelixValidation.md)</sub>
<!-- End Generated Content-->
