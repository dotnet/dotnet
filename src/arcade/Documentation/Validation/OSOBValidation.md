# Validation for OS OnBoarding (OSOB)

## Pipeline and Jobs

**PRs** : <br />
    [Build -> Build Images -> Approval -> Pre-Deployment -> Deploy Queues & Deploy AutoScale Service -> Validate & Post-Deployment](https://dnceng.visualstudio.com/internal/_build?definitionId=596&_a=summary)
- Runs unit tests in the `Helix.Machines` solution (e.g. testing scripts)
- Collects and reports code coverage
- Artifact validation on selected images in two phases: once just after artifacts has been installed in ImageFactory, once as a part of test job for a test queue
- Validate whether the yamls are properly constructed ie if the artifacts and Images are correct and they exist.
- Create images, deploy images and artifacts (in staging), update test OnPrem machines with Helix Client, and send jobs to selected queues. (This is broken right now - issue tracked [here](https://github.com/dotnet/core-eng/issues/7984))
  - Supported OnPrem Queues: found in file `validation\onprem.pr.queues.txt`

**CI** : <br />
    [Build -> Build Images -> Approval -> Pre-Deployment -> Deploy Queues & Deploy AutoScale Service -> Validate & Post-Deployment & Cleanup](https://dnceng.visualstudio.com/internal/_build?definitionId=596&_a=summary)
- Everything that runs in PRs but for all queues
- Create images, deploy images and artifacts, update test OnPrem machines with Helix Client, and send jobs to all the queues. 
  - Supported OnPrem Queues: found in file `validation\onprem.staging.queues.txt`
- Runs clean up stage to clean up old queues on staging

## Where do the Tests live? 
Per the [Validation Process](https://github.com/dotnet/core-eng/blob/main/Documentation/Validation/ValidationProcess.md#unit-testing) documentation, tests will live within the solution of the project being tested. 


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CValidation%5COSOBValidation.md)](https://helix.dot.net/f/p/5?p=Documentation%5CValidation%5COSOBValidation.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CValidation%5COSOBValidation.md)</sub>
<!-- End Generated Content-->
