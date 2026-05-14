# Azure DevOps Tasks

## Intent

This document is intended to

- solidify agreement that tasks are a good alternative to templates for logic based work (I think there's already general agreement here)

- provide an initial list of known items which would be good task candidates.

This document is **not** intended to

- be a declaration (commitment) of work or specific deliverables

- provide a timeline

- assign ownership

- define implementation details

  - such as contracts, source repository, public / internal sources, naming conventions, versioning schema, servicing agreement

## Overview

[Tasks](https://docs.microsoft.com/en-us/azure/devops/pipelines/process/tasks?view=vsts&tabs=yaml) are building blocks for logic defined in an Azure build pipeline.  We have .NET Core engineering services which are currently being provided via templates.  Templates are about providing a consistent framework for running an Azure Pipeline build (adding steps, defining variables, etc...) and re-using logic.  Templates are not directly intended to define logic (they wrap functionality), though it's certainly possible to use them this way.  .NET Core engineering services provides some services today which are Azure DevOps specific and are provided via templates.  We would like to provide that functionality via tasks which may be plugged into templates.

## Task candidates

Here are the current templates we are providing in Azure DevOps are good candidates for moving to tasks.

- [send-to-helix](https://github.com/dotnet/arcade/blob/master/eng/common/templates/steps/send-to-helix.yml) - used to send jobs to Helix.

- [telemetry-start](https://github.com/dotnet/arcade/blob/master/eng/common/templates/steps/telemetry-start.yml) - send Helix telemetry start event

- [telemetry-end](https://github.com/dotnet/arcade/blob/master/eng/common/templates/steps/telemetry-end.yml) - send Helix telemetry end event

- [publish-build-assets](https://github.com/dotnet/arcade/blob/master/eng/common/templates/phases/publish-build-assets.yml) - publish build assets to registry.

- Bash or Cmd script - Most of our teams are cross-plat and they need to either do some Windows thing on Windows platforms or run some Unix command on non-Windows platforms.  Azure DevOps does not provide a way to only include OS specific steps.  Instead, today both Windows steps and Unix steps are included in a build definition and unapplicable steps are simply disabled (they still show up in the build definition but they are skipped).  Instead, we can provide a single task that will task both Windows and non-Windows commands and run the appropriate script at run-time so that you don't end up with disabled steps confusing your build output.

- Gather test results - a task which can be used by Arcade builds to gather test results and report them appropriately


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProjects%5CDevOps%5CTasks%5COnePager.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProjects%5CDevOps%5CTasks%5COnePager.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProjects%5CDevOps%5CTasks%5COnePager.md)</sub>
<!-- End Generated Content-->
