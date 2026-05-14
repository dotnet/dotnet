# Choosing a Machine Pool

## Builds

Arcade repos should use the shared **pool provider variables** from the repo templates instead of hard-coding legacy queue names.

### Pull Request validation and public CI

Import:

```yaml
variables:
- template: /eng/common/templates/variables/pool-providers.yml
```

Use:

- **Pool**: `$(DncEngPublicBuildPool)`
- This resolves to:
  - `NetCore-Public` for `main` and other non-release branches
  - `NetCore-Svc-Public` for `release/*` branches

Typical images used in this repo:

- **Windows**: `windows.vs2026.amd64.open`
- **Linux**: `build.azurelinux.3.amd64.open`
- **Mac**: [Hosted macOS](https://docs.microsoft.com/en-us/azure/devops/pipelines/agents/hosted?view=vsts&tabs=yaml)

Example:

```yaml
pool:
  name: $(DncEngPublicBuildPool)
  demands: ImageOverride -equals windows.vs2026.amd64.open
```

### Official / signed builds

Import:

```yaml
variables:
- template: /eng/common/templates-official/variables/pool-providers.yml
```

Use:

- **Pool**: `$(DncEngInternalBuildPool)`
- This resolves to:
  - `NetCore1ESPool-Internal` for `main` and other non-release branches
  - `NetCore1ESPool-Svc-Internal` for `release/*` branches

Typical images used in this repo:

- **Windows**: `windows.vs2026.amd64`
- **Linux**: `build.azurelinux.3.amd64`
- **Mac**: [Hosted mac Internal](https://docs.microsoft.com/en-us/azure/devops/pipelines/agents/hosted?view=vsts&tabs=yaml)

For official 1ES pipelines, prefer the `image` syntax:

```yaml
pool:
  name: $(DncEngInternalBuildPool)
  image: windows.vs2026.amd64
  os: windows
```

For job templates that still use queue demands, use the equivalent `ImageOverride` demand instead.

## Choosing an image

- Prefer the same image already used by similar jobs in this repo.
- Linux images generally boot faster than Windows images.
- For a live list of approved images, see [helix.dot.net/#1esPools](https://helix.dot.net/#1esPools).
- For official builds (with the exception of macOS), **avoid hosted images** (such as `windows-latest` or `ubuntu-latest`).

## Test Execution

All test execution should run through **Helix**.

To view the available Helix queues:

1. Perform an HTTP GET of `https://helix.dot.net/api/2018-03-14/info/queues`.
2. Review the returned JSON array of queue descriptions.
3. Use the [dotnet-helix-machines] repo and the queue info API to confirm machine capabilities.
4. Submit test jobs through the [Helix Sdk].

[Helix Sdk]: /Documentation/AzureDevOps/SendingJobsToHelix.md
[Bootstrapping System]: /Documentation/NativeToolBootstrapping.md
[@dotnet/dnceng]: https://github.com/orgs/dotnet/teams/dnceng
[dotnet-helix-machines]: https://dev.azure.com/dnceng/internal/internal%20Team/_git/dotnet-helix-machines?path=%2FREADME.md&version=GBmain
[Helix Queue Info Api]: https://helix.dot.net/swagger/ui/index#!/Information/Information_QueueInfoList

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CChoosingAMachinePool.md)](https://helix.dot.net/f/p/5?p=Documentation%5CChoosingAMachinePool.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CChoosingAMachinePool.md)</sub>
<!-- End Generated Content-->
