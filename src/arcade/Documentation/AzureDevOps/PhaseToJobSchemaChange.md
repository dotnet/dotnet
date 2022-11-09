# Phase to Job Schema Change

- [Overview](#overview)
- [Definitions](#definitions)
- [Schemas reference](#azure-devops-schemas)
- Arcade's templates
  - [Why use Arcade's templates](#why)
  - [What are Arcade's templates](#what)
  - [How do you transition to the new templates](#how)

---

## Overview

The Azure DevOps schema originally defined a schema which included [phase](#phase) and [queue](#queue) objects.  Azure DevOps modified their schema and replaced the [phase](#phase) object with the [job](https://docs.microsoft.com/en-us/azure/devops/pipelines/yaml-schema?view=vsts&tabs=schema#job) object, and the [queue](#queue) with a [pool](https://docs.microsoft.com/en-us/azure/devops/pipelines/yaml-schema?view=vsts&tabs=schema#pool).  This change is not a direct one-to-one change as "queue" was a member of "phase" but now "pool" is not a member of "job". Additionally, a "matrix" is no longer a part of the "queue/pool" object, it is now a member of the "job" (defined as a [strategy](https://docs.microsoft.com/en-us/azure/devops/pipelines/yaml-schema?view=vsts&tabs=schema#strategies)).

---

## Definitions

[Phase schema](#phase-schema) - The Azure pipeline schema originally included [phase](#phase) and [queue](#queue) objects.  That schema which refers to a "phase" object is called the "phase schema".

[Job schema](#job-schema) - Azure DevOps replaced the "phase" object with a "job" object and changed the members of that object.  Authoring yaml which refer to a job object instead of a phase object is referred to as the "job schema".  "Job" and "phase" are not interchangeable objects, they are mutually exclusive.

---

## Why

### - Why use Arcade's templates

- Using Arcade's templates allows you to write "cleaner" pipeline files by conditionally adding tasks depending on the context your build is running in.  You can use the same yaml file for both PR and official builds.  ie, the templates will add the "install Microbuild plugin" step if you are running in an official build context.

- Arcade's templates enable sending telemetry so that we can gather information about build reliability and we can use that information to target what areas should be improved to help our customers be impacted by infrastructure less often.

- Arcade's templates simplify using Helix

- Arcade's templates simplify enabling dependency flow

### - Why change from phase to job schema

- Azure DevOps no longer supports the phase schema

- BYOC (scalable agent pools) is not supported by the phase schema

---

## What

### - What is the eng\common\templates\job\job.yml template

job.yml is the base.yml replacement which uses the "job" schema instead of the "phase" schema.

See the [job](https://github.com/dotnet/arcade/blob/master/eng/common/templates/job/job.yml) template.

Example:

In the main pipeline you would reference the template like this...

```yaml
jobs:
- template: /eng/common/templates/job/job.yml
  parameters:
    enableMicrobuild: true
    enablePublishBuildArtifacts: true
    name: Windows_NT_Job
    pool: dotnet-external-temp
    steps:
    - script: echo Hello World!
- template: /eng/common/templates/job/job.yml
  parameters:
    enableMicrobuild: true
    enablePublishBuildArtifacts: true
    name: Linux_Job
    pool: dnceng-linux-external-temp
    steps:
    - script: echo Hello World!
```

Additional example of the [phase to job schema transition](#phase-to-job-schema-transition)

### - What is the eng\common\templates\jobs\jobs.yml template

The "jobs" template wraps the "[job](https://github.com/dotnet/arcade/blob/93b39c3209a5929662190c7e85b43b4f2a32bab1/eng/common/templates/jobs/jobs.yml#L48)" template, providing all of the functionality of the ["job" template](https://github.com/dotnet/arcade/blob/93b39c3209a5929662190c7e85b43b4f2a32bab1/eng/common/templates/jobs/job), but additionally adding a dependent job; the [publish-build-assets](https://github.com/dotnet/arcade/blob/93b39c3209a5929662190c7e85b43b4f2a32bab1/eng/common/templates/jobs/jobs.yml#L63) job.  Previously, repos participating in Arcade's dependency flow model had to explicitly add this [job](https://github.com/dotnet/arcade/blob/991182ca723410c7f4898368a67744943e7891fb/.vsts-ci.yml#L67) to their yml file and define its dependencies.  If you're referencing the `jobs.yml` template, you can just specify [enablePublishBuildAssets](https://github.com/dotnet/arcade/blob/21cea9cd115a1efa1955d44ebbc5248a318de00f/azure-pipelines.yml#L15) as "true" and that job will be included in your build.

If you are participating in dependency flow (publishing assets), it is recommended that you use the "jobs" template.

See the [jobs](https://github.com/dotnet/arcade/blob/master/eng/common/templates/jobs/jobs.yml) template.

Example:

In the main pipeline you would reference the template like this...

```yaml
jobs:
- template: /eng/common/templates/jobs/jobs.yml
  parameters:
    enableMicrobuild: true
    enablePublishBuildArtifacts: true
    jobs:
    - job: Windows_NT_Job
      pool: dotnet-external-temp
      steps:
      - script: echo Hello World!
    - job: Linux_Job
      pool: dnceng-linux-external-temp
      steps:
      - script: echo Hello World!
```

Additional example of the [phase to jobs schema transition](#phase-to-jobs-schema-transition)

---

## How

### Phase to job schema transition

- The "job" template makes use of variable array syntax.  The array syntax is the only way to reference "variable groups" in yaml and so, that is the supported way to list variables in yaml.

  Sample array syntax:

  ```yaml
  variables:
  # Create a "foo" variable
  - name: foo
    value: bar
  # Reference a variable group
  - group: myvargroup
  ```

  If your variables are defined using the object syntax, they will **not** work with `job.yml`/`jobs.yml`.

  Sample object syntax (not valid when referencing `job.yml` or `jobs.yml`):

  ```yaml
  # will not evaluate properly if you're using the job.yml or jobs.yml templates
  variables:
    foo: bar
  ```

- There are a couple of parameters which `job.yml` supports that `base.yml` did not.

  - [enablePublishBuildArtifacts](https://github.com/dotnet/arcade/blob/93b39c3209a5929662190c7e85b43b4f2a32bab1/eng/common/templates/job/job.yml#L35) - Adds the "PublishBuildArtifacts" task to your steps.

  - [enablePublishTestResults](https://github.com/dotnet/arcade/blob/93b39c3209a5929662190c7e85b43b4f2a32bab1/eng/common/templates/job/job.yml#L38) - Adds the "PublishTestResults" task to your steps.

  - [helixRepo](https://github.com/dotnet/arcade/blob/93b39c3209a5929662190c7e85b43b4f2a32bab1/eng/common/templates/job/job.yml#L51) - defines your source repo and is used to automatically determine the `_HelixSource` variable

  - [helixType](https://github.com/dotnet/arcade/blob/93b39c3209a5929662190c7e85b43b4f2a32bab1/eng/common/templates/job/job.yml#L51) - define the helix telemetry type, ie. build/product/

Transition example:

Original yaml referencing `eng\common\templates\phases\base.yml`.

azure-pipelines.yml

```yml
variables:
  Build.Repository.Clean: true
  _HelixType: build/product/
  _HelixSource: pr/dotnet/arcade-minimalci-sample/$(Build.SourceBranch)

trigger:
- master
pr:
- master

phases:
- template: /eng/common/templates/phases/base.yml
  parameters:
    name: Windows_NT
    enableTelemetry: true
    queue:
      name: dotnet-external-temp
      parallel: 99
      matrix:
        debug_configuration:
          _BuildConfig: Debug
        release_configuration:
          _BuildConfig: Release
    steps:
    - script: eng\common\cibuild.cmd
        -configuration $(_BuildConfig)
        -prepareMachine
      name: Build
      displayName: Build
      condition: succeeded()
    - task: PublishBuildArtifacts@1
      displayName: Publish Logs to VSTS
      inputs:
        PathtoPublish: '$(Build.SourcesDirectory)/artifacts/log/$(_BuildConfig)'
        PublishLocation: Container
        ArtifactName: $(Agent.Os)_$(Agent.JobName)
      continueOnError: true
      condition: always()
    variables:
      _HelixBuildConfig: $(_BuildConfig)
```

Updated yaml referencing `eng\common\templates\job\job.yml`.

azure-pipelines.yml

```yml
trigger:
- master
pr:
- master

jobs:
- template: /eng/common/templates/job/job.yml
  parameters:
    name: Windows_NT
    enableTelemetry: true
    # Allow job.yml to add the "PublishBuildArtifacts" step - https://github.com/dotnet/arcade/blob/93b39c3209a5929662190c7e85b43b4f2a32bab1/eng/common/templates/job/job.yml#L38
    enablePublishBuildArtifacts: true
    helixRepo: dotnet/arcade-minimalci-sample
    pool:
      name: dotnet-external-temp
    strategy:
      matrix:
        debug_configuration:
          _BuildConfig: Debug
        release_configuration:
          _BuildConfig: Release
    steps:
    - checkout: self
      clean: true
    - script: eng\common\cibuild.cmd
        -configuration $(_BuildConfig)
        -prepareMachine
      name: Build
      displayName: Build
      condition: succeeded()
```

### Phase to jobs schema transition

Transition example:

Original yaml referencing `eng\common\templates\phases\base.yml`.

azure-pipelines.yml

```yml
variables:
  Build.Repository.Clean: true
  _HelixType: build/product/
  _HelixSource: pr/dotnet/arcade-minimalci-sample/$(Build.SourceBranch)

trigger:
- master
pr:
- master

phases:
- template: /eng/common/templates/phases/base.yml
  parameters:
    name: Windows_NT
    enableTelemetry: true
    queue:
      name: dotnet-external-temp
      parallel: 99
      matrix:
        debug_configuration:
          _BuildConfig: Debug
        release_configuration:
          _BuildConfig: Release
    steps:
    - script: eng\common\cibuild.cmd
        -configuration $(_BuildConfig)
        -prepareMachine
      name: Build
      displayName: Build
      condition: succeeded()
    - task: PublishBuildArtifacts@1
      displayName: Publish Logs to VSTS
      inputs:
        PathtoPublish: '$(Build.SourcesDirectory)/artifacts/log/$(_BuildConfig)'
        PublishLocation: Container
        ArtifactName: $(Agent.Os)_$(Agent.JobName)
      continueOnError: true
      condition: always()
    variables:
      _HelixBuildConfig: $(_BuildConfig)
```

Updated yaml referencing `eng\common\templates\jobs\jobs.yml`.

azure-pipelines.yml

```yml
trigger:
- master
pr:
- master

jobs:
- template: /eng/common/templates/jobs/jobs.yml
  parameters:
    enableTelemetry: true
    enablePublishBuildArtifacts: true
    # Add a dependent job which publishes build assets for dependency flow
    enablePublishBuildAssets: true
    helixRepo: dotnet/arcade-minimalci-sample
    # define jobs in the jobs object
    jobs:
    - job: Windows_NT
      pool:
        name: dotnet-external-temp
      strategy:
        matrix:
          debug_configuration:
            _BuildConfig: Debug
          release_configuration:
            _BuildConfig: Release
      steps:
      - checkout: self
        clean: true
      - script: eng\common\cibuild.cmd
          -configuration $(_BuildConfig)
          -prepareMachine
        name: Build
        displayName: Build
        condition: succeeded()
```

Notes:

- The `jobs.yml` template, unlike `job.yml`, does not have support for handling explicit templates.

  Example:

  ```yaml
  jobs:
  - template: /eng/common/templates/jobs/jobs.yml
    parameters:
      jobs:
      - job: Windows_NT
        steps:
        - script: echo Hello world!
      # This won't work, you can't pass a template as a job to jobs.yml
      - template: sayhi.yml
  ```

  If you need to reference a template, then you should use `job.yml` to wrap each individual job rather than using `jobs.yml` to wrap all jobs.

- See https://github.com/dotnet/arcade for an example of a repo that uses the "jobs" template.

## Azure DevOps schemas

### Phase schema

#### phase

The phase schema is no longer supported or documented.  Including the schema format here for reference.

```yaml
- phase: string # name
  displayName: string
  dependsOn: string | [ string ]
  condition: string
  continueOnError: true | false
  queue: string | queue
  server: true | server
  variables: { string: string } | [ variable ]
  steps: [ script | bash | powershell | checkout | task | templateReference ]
```

#### queue

```yaml
name: string
demands: string | [ string ] # Supported by private pools
timeoutInMinutes: number
cancelTimeoutInMinutes: number
parallel: number
matrix: { string: { string: string } }
```

### Job schema

#### job

The job schema has replaced the phase schema and is publicly [documented](https://docs.microsoft.com/en-us/azure/devops/pipelines/yaml-schema?view=vsts&tabs=schema#job)


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CAzureDevOps%5CPhaseToJobSchemaChange.md)](https://helix.dot.net/f/p/5?p=Documentation%5CAzureDevOps%5CPhaseToJobSchemaChange.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CAzureDevOps%5CPhaseToJobSchemaChange.md)</sub>
<!-- End Generated Content-->
