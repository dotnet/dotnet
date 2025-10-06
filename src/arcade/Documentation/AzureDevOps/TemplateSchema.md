# Templates Schema

Arcade provides many templates which help consumers conform to Arcade standards and provide additional functionality (additional jobs or stages that provide features).  

- [eng/common/jobs/jobs.yml](#jobs.yml)
- [eng/common/job/job.yml](#job.yml)

## Jobs.yml

Jobs.yml is a wrapper around one or more Azure DevOps [jobs](https://docs.microsoft.com/en-us/azure/devops/pipelines/yaml-schema?view=azure-devops&tabs=schema#job) but additionally adds more jobs which are commonly used by .NET Core Arcade repos. ie a job to publish asset manifests from a build and a job to generate the graph files for a build.

### Jobs schema

```yaml
parameters:
  continueOnError: boolean # 'true' if future jobs should run even if this job fails; defaults to 'false'
  enablePublishBuildArtifacts: boolean # Enables publishing build logs as an Azure DevOps artifact.
  jobs: [ jobSchema ] # see "Job schema" below
  publishBuildAssetsDependsOn: [ string ] # Override automatically derived dependsOn value for "publish build assets" job
  runAsPublic: boolean # Specify if job should run as a public build even in the internal project
```

## Job.yml

Job.yml wraps common Arcade functionality in an effort to provide automatic support for Azure DevOps builds which rely on common Arcade features / conventions.

### Job schema

```yaml
parameters:
  # accepts all job schema properties as parameters (https://docs.microsoft.com/en-us/azure/devops/pipelines/yaml-schema?view=azure-devops&tabs=schema#job)

  # additional parameters
  artifacts: { artifactsReference }
  # 'true' if Microbuild plugin should be installed for internal builds.
  #           if 'true', these "variables" must be specified in the variables object or as part of the queue matrix
  #             _TeamName - the name of your team
  #             _SignType - 'test' or 'real'  
  enableMicrobuild: boolean
  enableMicrobuildForMacAndLinux: boolean # true if build is running on a Mac or Linux agent
  enablePublishBuildArtifacts: boolean # deprecated (replaced by 'artifacts' parameter).  Enables publishing build logs as an Azure DevOps artifact.
  enablePublishBuildAssets: boolean # deprecated (replaced by 'artifacts' parameter). Enables publishing asset manifests as an Azure DevOps artifact.
  enablePublishTestResults: boolean # include publish test results task
  enableTelemetry: boolean # specifies whether to set the DOTNET_CLI_TELEMETRY_PROFILE environment variable. Default 'true', must explicitly set 'enableTelemetry: false' to disable
  name: string # Required:
  # steps to run before artifacts are downloaded task is executed.  ie, a clean step should happen before downloading artifacts.
  preSteps: [ script | bash | pwsh | powershell | task | templateReference ]
  runAsPublic: boolean
```

Find [artifactsReference](#artifact-schema) below.

### Artifact schema

The artifact parameter is used by [job.yml](#job-schema) to control what artifacts are published to Azure DevOps during a pipeline.

```yaml
artifacts:
  # 'true' to download default pipeline artifacts to default location.  Use 'downloadArtifact' to change name and/or path
  download: true | { downloadArtifactReference }
  publish:
    artifacts: true | { publishArtifactReference } # 'true' to publish to artifacts/bin and artifacts/packages to default named Azure DevOps artifact.  Use 'publishArtifact' to change Azure DevOps artifact name
    logs: true | { publishArtifactReference } # 'true' to publish logs to default named Azure DevOps artifact.  Use 'publishArtifact' to change Azure DevOps artifact name
    manifests: true | { publishArtifactReference } # 'true' to publish asset manifests to default named Azure DevOps artifact.  Use 'publishArtifact' to change Azure DevOps artifact name
```

Find [downloadArtifactReference](#downloadartifact-schema) and [publishArtifactReference](#publishartifact-schema) below.

#### Artifact Example

```yaml
jobs:
- \eng\common\templates\job\job.yml
  parameters:
    name: build
    artifacts:
      publish:
        artifacts:
          name: myartifacts # publish artifacts/bin and artifacts/packages from the build to the 'myartifacts' Azure DevOps artifact
        logs: true          # publish logs from the build to the default named artifact
        manifests: true     # publish asset manifests from the build to the default named artifact
    steps:
    - script: build.cmd

- \eng\common\templates\job\job.yml
  parameters:
    name: test
    artifacts:
      download:
        name: myartifacts # download artifacts the the 'myartifacts' Azure DevOps artifact
        path: pipelineartifacts # download to the 'pipelineartifacts' folder instead of the default ('artifacts')
      publish:
        logs: true        # publish logs to the default named artifact
    steps:
    - script: test.cmd
```

### DownloadArtifact schema

```yaml
artifacts:
  download:
    name: string # artifact name to download
    path: string # target path for artifact contents
    pattern: string # filter pattern representing files to include
```

### PublishArtifact schema

```yaml
artifacts:
  publish:
    artifacts | logs | manifests:
      name: string # Azure DevOps artifact name
```


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CAzureDevOps%5CTemplateSchema.md)](https://helix.dot.net/f/p/5?p=Documentation%5CAzureDevOps%5CTemplateSchema.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CAzureDevOps%5CTemplateSchema.md)</sub>
<!-- End Generated Content-->
