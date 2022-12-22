# Choosing a Machine Pool

## Builds
All Azure Pipelines builds should use the following agent queues
 * Pull Request validation and Public CI
   * Windows - [dotnet-external-temp]
   * Linux - [dnceng-linux-external-temp]
   * Mac - [Hosted macOS](https://docs.microsoft.com/en-us/azure/devops/pipelines/agents/hosted?view=vsts&tabs=yaml)
 * Official Signed Builds
   * Windows - [dotnet-internal-temp]
   * Linux - [dnceng-linux-internal-temp]
   * Mac - [Hosted mac Internal](https://docs.microsoft.com/en-us/azure/devops/pipelines/agents/hosted?view=vsts&tabs=yaml)

Pools for an Azure DevOps Pipeline can be specified at the build and/or job level in the yaml file ([documentation](https://docs.microsoft.com/en-us/azure/devops/pipelines/process/phases?view=vsts&tabs=yaml)).

Detailed information about the machines in an agent queue can be found in the [dotnet-helix-machines] repo. Additional dependencies not available on the machines should be bootstrapped in using our [Bootstrapping System]. If bootstrapping doesn't work for a specific dependency contact [@dotnet/dnceng] for guidance.

## Test Execution
All test execution should run through helix. An up to date list of helix queues can be obtained from the [Helix Queue Info Api] using the following steps.
 * Perform an HTTP GET of https://helix.dot.net/api/2018-03-14/info/queues with your http requesting software of choice. (A web browser works fine)
 * You will be presented with a json array containing descriptions of all the queues available in helix.
 * All of the queues in the list are available for use in helix. Detailed information about machine setup can be found in the [dotnet-helix-machines] repo.
 * Submit your test jobs to helix using the [Helix Sdk].


[Helix Sdk]: /Documentation/AzureDevOps/SendingJobsToHelix.md
[Bootstrapping System]: /Documentation/NativeToolBootstrapping.md
[@dotnet/dnceng]: https://github.com/orgs/dotnet/teams/dnceng

[dotnet-internal-temp]: https://dev.azure.com/dnceng/internal/_settings/agentqueues?queueId=67&_a=agents
[dnceng-linux-internal-temp]: https://dev.azure.com/dnceng/internal/_settings/agentqueues?queueId=61&_a=agents
[dnceng-linux-external-temp]: https://dev.azure.com/dnceng/public/_settings/agentqueues?queueId=65&_a=agents
[dotnet-external-temp]: https://dev.azure.com/dnceng/internal/_settings/agentqueues?queueId=47&_a=agents

[dotnet-helix-machines]: https://dev.azure.com/dnceng/internal/internal%20Team/_git/dotnet-helix-machines?path=%2FREADME.md&version=GBmain
[Helix Queue Info Api]: https://helix.dot.net/swagger/ui/index#!/Information/Information_QueueInfoList


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CChoosingAMachinePool.md)](https://helix.dot.net/f/p/5?p=Documentation%5CChoosingAMachinePool.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CChoosingAMachinePool.md)</sub>
<!-- End Generated Content-->
