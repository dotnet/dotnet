# Writing Azure DevOps Build Pipelines

The purpose of this document is to detail how Azure DevOps YAML Pipelines should be written to integrate into the .NET Core Ecosystem and take advantage of the scaffolding available in the Arcade repository.

## General Goals For Pipelines

Pipelines for the .NET Core Ecosystem should be written with the following goals in mind:

- **A Pipeline integrates with telemetry systems**

  - Builds should integrate with telemetry systems to enable a number of scenarios important to building a more effective .NET Core infrastructure ecosystem, including:
    - Enable build status tracking.
    - Enable pass/failure analysis.
    - Enable test failure analysis.
    - Enable efficient product code flow via Maestro# and Speculative Package Flow.
    - Enable efficient asset tracking.

- **A Pipeline can be used for both OSS and internal development** - To reduce the cost of developing in both the open and closed worlds, sharing the same pipeline logic is ideal.  A definition should be able to be run in both OSS and internal contexts and produce results accordingly (e.g. an internal run signs, an OSS run doesn't).  The definition should also be usable in both CI and Official builds (open or closed).

- **Pipeline results are easy to reproduce locally** - Pipelines, especially when being viewed through the context of a recent build log, should be able to be reproduced locally (if possible).  Ambient state dependencies should be minimized where possible.

- **A Pipeline can be serviced and altered globally to maintain consistency across the .NET Core ecosystem** - The number of pipelines is large, and making extensive changes across the ecosystem must be efficient.  For example, changes or rollout of new telemetry should be relatively painless.

- **A Pipeline change must be testable in an isolated environment** - A pipeline change should be testable without altering the state of other in-progress changes.

- **A Pipeline should branch with the code it serves and maintain its functionality over the servicing lifetime of the code** - Ensure that not only does the Pipeline not drift from the source it serves, but also does not become non-functional over time due to changes in external systems.

## General Rules

- **A pipeline is defined in code (YAML)** - Defining as much Pipeline configuration in source ensures a testable, serviceable, and versionable infrastructure ecosystem.

- **A pipeline does not rely on input variables in the environment** - Environment variables cause lots of issues in pipelines and should be avoided as a means of passing input to a build where possible.
  - They may not be obvious to readers of logs.
  - Case-insensitivity differences between operating systems and tools is a cause of issues.
  - Environment block size limits can occasionally be a problem.
  - Reproducing results can be more difficult.

- **A pipeline utilizes provided templates where possible** - Utilizing templates gives infrastructure developers extension points or abstraction layers to alter or extend behavior.  For example, templates can:
  - Ensure the right telemetry systems are contacted during key points in the build.
  - Allow usage of EOL operating systems to be removed or at least identified more easily.
  - Simultaneously roll out new changes to repo scripting and pipeline logic.
  - Update older servicing branches to ensure their pipelines are not broken by changes in external systems.
