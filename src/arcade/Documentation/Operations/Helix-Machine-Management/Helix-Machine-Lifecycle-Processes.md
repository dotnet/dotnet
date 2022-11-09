# .NET Core Engineering Services Operations info: - Lifecycle management for Helix Queues and .NET Core Engineering Build Images

## Process Details

### Glossary of terms:

- **End of life (EOL)**: The date at which an operating system is no longer supported by its publisher. Publishers will often provide more than one timeline for this (often charging customers money for longer terms). For .NET Core's purposes, this is usally the longest possible period where security vulnerabilities will be addressed as this is what customers running .NET on these operating systems expect, as we will need to be able to continue testing the shipped product on these operating systems until the final days of their support lifecycle.

- **Estimated Removal Date (ERD)**: The date that .NET Core Engineering Services intends to remove a test queue or build image.
This date is meant to force conversations to be had and actions to be taken, and is not meant to indicate a customer promise of .NET Core's OS support.  The date may be before, or after the EOL date (preferably before) at DncEng's discretion. With the exception of the dotnet-helix-machines build where it can become an error once elapsed, this time is only used to inform warnings to users. Estimated removal dates thus can be arbitrarily extended with sufficient cause (leaving history behind in Git commits of who did it and hopefully why), but the goal is to never have unsupported and un-patch-able operating systems managed by the team.

- **Update Required Date (URD)**: The date that .NET Core Engineering Services will next need to take action to update an image used in Helix test machines or 1ES hosted build pool images.  This can be any date up to the estimated removal date, but not after it.  This date is designed to allow the .NET Core Engineering Services team an opportunity to update images that need no user action (for instance: Updating to a newer, non-EOL, OS version with all the same artifacts where the users don't need to have direct communication about this.) When an existing image includes version information that makes this impossible (e.g. "19H1" is in a queue name but its corresponding Windows version is EOL) the Update Required date should be set to the same value as EstimatedRemovalDate.

- **Matrix of Truth**: ([Epic issue link](https://github.com/dotnet/core-eng/issues/11077)) Ongoing work to provide a single source of information for operating system test matrix and life cycle for .NET Core Engineering systems.

- **Helix Queue**: Set of machines, whether they are an Azure VM Scaleset or physical machines, which execute test work items.

- **Build image**: Azure Compute Gallery image used to populate 1ES Pool provider instances used for all .NET Core builds. Due to executive order, we must perform all builds through images pushed to this system.

### Summary:

#### Why does this "process" exist in the first place?

Previously, removing old Helix queues and images was a best-effort process. This has several major problems including:

- It causes us to continue using images that no longer receive security patching, leading to potential attacks as well as definitely causing monitoring applications to detect these machines and cause us to react to this.
- Helix VM and on-prem machine capacity is divided amongst whatever capacity we have.  If we keep around old Helix queues (on-prem or in Azure), this limits the ability to provide this capacity in still-supported OSes.
- Users would frequently only find out on rollout that their queue/image had been removed.

#### Who benefits? 
Helix users benefit from secure, patched machines and a regular communication of when what they use will be deprecated. The .NET Core product teams get the most secure and accurate-to-real-users'-machines images (that is, containing all the recent patches and updates to operating systems and components which could affect test / product behaviors) possible to run their tests or builds on.

#### What happens if it doesn't happen?
Without a regular process, we will be bogged down with responding to alerts for VMs or other resources using 'old' images, and even ignoring any security-related implications (reasonable for most Helix test machines, less so for build images), we will continue to pay for storage and compute costs for no-longer-supported operating systems.

### Process Boundaries

- Related repositories:
   - https://dnceng.visualstudio.com/internal/_git/dotnet-helix-machines --> Source control for all Helix VMs, 1ES Pool provider images, and Helix on-premises machine setup functionality
   - https://github.com/dotnet/dotnet-buildtools-prereqs-docker --> Source control for a wide variety of docker images automatedly published to Microsoft Container Registry (MCR). Images used for testing Helix "docker" images come from here. Work will eventually be done to move most of this image generation into the dotnet-helix-machines repo.
- Task scope:

  - In scope:
    - Monitor [dotnet-helix-machines](https://dnceng.visualstudio.com/internal/_build?definitionId=596) pipeline and respond to warnings / errors produced by EstimatedRemovalDate and UpdateRequiredDate times elapsing by either updating the images referenced and/or extending these dates, or removing these queues.
    - Whenever extending a date further out than the lifespan of an existing operating system, comments should be added above this (or at least in commit messages) explaining the extension.
    - Coordinate with rollout owners to ensure that upcoming removals and image refreshes (any queue that has produced a warning in the dotnet-helix-machines "Validate" stage) are included in rollout status emails. 
    - On a monthly cadence, review and update the EstimatedRemovalDate / UpdateRequiredDate values of images generated by the dotnet-helix-machines repository for accuracy. Use .NET release PM team and the internet for deciding dates.

  - Not in scope:
    - Deciding the OSes for which we will provide images. (tracked by https://github.com/dotnet/arcade/issues/8832)
    - Patching of Operating systems or updating of artifacts (tracked by https://github.com/dotnet/arcade/issues/8813)

- Contacts for non-owned parts of the process: For external ownership, who can we talk to?
  - For end-of-life operating systems, the release PM team owns the final word of when we can actually get rid of support. Certain operating systems can and will be maintained outside their publicly-communicated lifespan, usually owing to some important customer need.  The release PM team is composed of Jamshed Damkewala (jamshedd), Rich Lander (rlander), Lee Coward (leecow), and Rahul Bhandari (rbhanda); contact them for any questions in this space. Some examples of operating systems we support beyond their normal end-of-life include Ubuntu 16.04 and Windows 7 / Server 2k8R2, but there will be more exceptions.
  - DncEng "matrix of truth": IlyaS, general SME : MattGal

### Process Inputs / Outputs

#### SME information:

Descriptions of what/where the inputs to the process come from (the answer to "what do I or the automated process neeed to consider to perform this task?"), and what performing the below steps correctly achieves ("what comes out the other side?")

Inputs:
- The OS "Matrix of Truth" (future output from https://github.com/dotnet/arcade/issues/8832).  Until this matrix exists, what we have is considered "in matrix" but we need to be removing unsupported OSes.
- The .NET Core release team's input (they receieve notifications for OS removals from build/test support via the partners DL, and can veto this with justification)
- EstimatedRemovalDate warnings / errors from the dotnet-helix-machines-ci pipeline

Outputs:
- (Where needed) Removal of Helix image definition from dotnet-helix-machines repository.
- (Where needed) Updates of Helix base image or image references in the dotnet-helix-machines repository, or tracking issues for this work
- Communication with the release PM team ensuring any "first time" removals of a given OS are acceptable; this team may veto this and will provide new estimated removal dates if they do
  - Once the "Matrix of Truth" epic is complete and this is approved by the release PM team, we may consider no longer notifying them)
  - If not removing all instances of an OS at once (e.g. if removing build images while leaving test queues), mark the remaining instances of the OS with a comment indicating removal has already been approved so this step may be skipped
- Communication blurb following the below template in weekly rollout email.  As these warnings are designed to appear in the dotnet-helix-machines official build starting 3 weeks prior to expiration, it should consistently allow the current week's rollout (or "not rolling out" mail) to indicate that users will soon need to take action.

#### Example Communication

The following Helix Queues and/or Build images will be removed on the Wednesday rollout following the estimated date. Please remove usage of these queues/images before this date to keep your pipelines and tests functional. 

Helix Queues:

| Queue Name | Estimated removal date |
| - | - |
| Some.Helix.Queue | 03/14/2022 |
| Some.Other.Helix.Queue | 03/10/2022 |

1ES Hosted Pool Images

| Image Name | Estimated removal date |
| - | - |
| Build.OperatingSystem.KindOfImage | 03/21/2022 |
| Build.DifferentOperatingSystem.KindOfImage | 03/22/2022 |

Removing no-longer-supported operating systems on a regular cadence both allows us to be as secure as possible and use more of the resources we have available more for still-supported platforms.
If you feel this removal is in error, or believe a specific expiration should be extended, please email dnceng@microsoft.com with your concerns.

### Execution Steps

#### Fully-automatable routines:
- EstimatedRemovalDate & UpdateRequiredDate notifications - Part of the dotnet-helix-machines-ci pipeline, these must be both in the future and estimated removal date be >= update required date or the build will fail.
- Links:
  - Documentation: https://dnceng.visualstudio.com/internal/_git/dotnet-helix-machines?path=/definitions/readme.md
  - Pipeline: https://dnceng.visualstudio.com/internal/_build?definitionId=596
  - ImageFactory Wiki (includes access instructions): https://dev.azure.com/devdiv/XlabImageFactory/_wiki/wikis/XlabImageFactory.wiki (includes access instructions)
  - .NET PM team's OS Version Management calendar: https://dev.azure.com/devdiv/DevDiv/_wiki/wikis/DevDiv.wiki/12624/OS-Version-Management-Calendar-2022 (Adjust year for the current year)

- Known issues impacting the area: None
- Known tech debt: [Matrix of Truth](https://github.com/dotnet/arcade/issues/8832) work is not complete yet; once this is done we need to make sure this integrates into the existing system for update/removal.

#### Manual processes:

##### Daily:

- Review [the day's main pipeline executions](https://dnceng.visualstudio.com/internal/_build?definitionId=596&_a=summary&repositoryFilter=3&branchFilter=152037)
- If any warnings about EOL queues arise (see guidance for specific types below):
  - Check whether this removal represents the "last" of this operating system (no other queues have this OS). If so, get confirmation from the Release PM team or confirmation from “Matrix of Truth” to ensure its removal is acceptable and mention this in issues.
    - If removal is deemed inappropriate, make pull requests to the dotnet-helix-machines repo extending the time to a new, agreed-upon, date.
    - If pull requests are created, monitor subsequent builds in the pipeline until it has succeeded; 
  - Open issues in the dotnet/arcade for all actions, with the "First Responder" tag. Add to list of queues for end-of-week update.
  - Create pull requests removing these test or build images after the current week's rollout in the week they will be removed, and follow these until merged.

- If any warnings about "Update-required" images arise: (e.g. "`##[warning]<image.identifier> has update-required date: YYYY-MM-DD, in the next three weeks. Please either update the image to newer, file an issue requesting this, or extend the date with a comment explaining why if no action is taken.`")
  - Check whether updated images exist:
    - Refer to the yaml for these images, found under the "[definitions](https://dnceng.visualstudio.com/internal/_git/dotnet-helix-machines?path=/definitions)" folder of [the dotnet-helix-machines repo](https://dnceng.visualstudio.com/internal/_git/dotnet-helix-machines).  Some windows images may be found in [definition-base\windows.base.yaml](https://dnceng.visualstudio.com/internal/_git/dotnet-helix-machines?path=/definition-base/windows.base.yaml)
    - Find the image referenced in the yaml, or directly inside definitions\shared in the dotnet-helix-machines repository.
    - Run the osob CLI. The following commands assume you have .NET Core runtime on the path and are inside the `tools\OsobCli` folder of this repository.
      - `dotnet run list-image-versions -l westus -d ..\..\definitions`
      - `dotnet run list-imagefactory-image-versions -d ..\..\definitions`
      - Images with newer versions available will look like:
```
For <Image Name>:
   Current version:          <Version string from yaml>
   Latest available version: <Version string from Azure / DDFUN Image gallery>
   ** Upgrade! **
```
  - If there are updated versions of the image:
    - Wherever "Current Version" and "Latest Version" do not match, modify the image version to match the version printed out by the OSOB CLI tool above
    - Set the new `UpdateRequiredDate` to 90 days after the previous date, or the `EstimatedRemovalDate`, (whichever comes first).
    - Create a pull request with these changes and follow until merged.
  - If there are no updated versions of the image, simply set the update-required date to 90 days past the previous one
  - After merging any pull requests where multiple images are updated, as usual monitor "main" branch builds until they are successful.

- If the main build has failed (red): 
  - Ping [DncEng First Responders Teams Channel](https://teams.microsoft.com/l/channel/19%3aafba3d1545dd45d7b79f34c1821f6055%40thread.skype/First%2520Responders?groupId=4d73664c-9f2f-450d-82a5-c2f02756606d&tenantId=72f988bf-86f1-41af-91ab-2d7cd011db47) and ask for next steps. 
  - Optionally, create issue in dotnet/arcade with the "First Responders" label requesting investigation.

##### Weekly (Currently, most rollouts are on Wednesdays):
- Find the rollout pipeline.  Rollout occurs around 9 AM in the [dotnet-helix-machines-ci pipeline](https://dnceng.visualstudio.com/internal/_build?definitionId=596&_a=summary) with branch 'production'. Contact the dnceng team if this is not occurring or you lack permissions.
- Even if there are no tasks to be done, always create an issue using [this template](https://dnceng.visualstudio.com/internal/_workitems/create/Task?templateId=7599e1ed-4c83-45cd-ad97-10ce36dbbb20&ownerId=3f024d6c-9884-4f38-a598-025fca9dfcd2) and put it into the "active" state.
- Fill out the sections of the template:
  - Link to completed pipeline
  - Status of the rollout (completed normally, aborted, rolled back, etc)
  - List of any images/queues producing warnings in the pipeline (errors will stop us from running it)
    - Create new issues in [dotnet/arcade](https://github.com/dotnet/arcade/) identifying the queue and linking to the build. Include links to these issues under the "queues that will expire in the next 7 days" or "after the next 7 days" headings as appropriate.
- When all the above is complete for a given week, you may close the issue.

##### Monthly (closest business day to the 15th):
**Request DDFUN updates for on-premises Helix machines**

We depend on our partner team DevDiv Fundamentals ("DDFUN") for machine maintenance tasks. DDFUN vendors use table "OsobInventory" in storage account 'helixscripts2' to view the machines.  We want any machine that shows up in HelixEnvironment "Production" with Partition Key not equal to "Not in Helix" to be at least reviewed for update monthly. See @MattGal or @Ilyas1974 if you believe you need to gain read access to this table. If new queues are found in the table that are not below, please make a pull request to update the list.

We need to ensure and drive that the machines we are running are updated to the latest possible versions. Since we cannot rely on AzSecPack and automated reporting for these updates, we'll generate IcM tickets requesting this. For each of the three categories of operating system below (Linux, MacOS, and Windows) please create an IcM ticket [here](https://ddfunlandingpagev120210311150648.azurewebsites.net/) using the "[Other](https://portal.microsofticm.com/imp/v3/incidents/create?tmpl=E3619N)" template.

After creating such an IcM for each of the three groups of machines, use [this template](https://dnceng.visualstudio.com/internal/_workitems/create/Task?templateId=5ad0c1bc-5e95-45a1-b40b-de81c12b5b4a&ownerId=3f024d6c-9884-4f38-a598-025fca9dfcd2) to link the three IcM tickets, and keep the issue open until all three are resolved.


Suggested IcM Description:
```
Machines in the following queues need to be updated to their latest patch versions.

<List of queues from below>

- No major version updates should occur in operating systems (e.g. do not allow a Windows 10 system to update to Windows 11, or OSX 10.15 to update to 11.0/12.0)
- For windows, Windows update should be executed until it stops prompting for changes.
- For MacOS, use the provided UI to take system updates while remaining in the same release band.
- Where possible (use judgment), if linux package managers have recommended updates these should be taken.
```

Linux machines (raspbian 9 iot devices should NOT get updated)

- alpine.amd64.tiger.perf
- ubuntu.1804.amd64.owl.perf
- ubuntu.1804.amd64.tiger.perf
- ubuntu.1804.amd64.tiger.perf.open
- ubuntu.1804.arm64.perf
- ubuntu.1804.armarch
- ubuntu.1804.armarch.open

MacOS Machines

- osx.1015.amd64
- osx.1015.amd64.appletv.open
- osx.1015.amd64.iphone.open
- osx.1015.amd64.iphone.perf
- osx.1015.amd64.open
- osx.1100.amd64
- osx.1100.amd64.appletv.open
- osx.1100.amd64.open
- osx.1100.amd64.scouting.open
- osx.1100.arm64
- osx.1100.arm64.appletv.open
- osx.1100.arm64.open
- osx.1200.amd64.iphone.open
- osx.1200.amd64.open
- osx.1200.arm64
- osx.1200.arm64.open

Windows Machines

- windows.10.amd64.19h1.tiger.perf
- windows.10.amd64.19h1.tiger.perf.open
- windows.10.amd64.20h2.owl.perf
- windows.10.amd64.android.open
- windows.10.amd64.galaxy.perf
- windows.10.amd64.pixel.perf
- windows.10.arm32
- windows.10.arm32.iot
- windows.10.arm32.iot.open
- windows.10.arm32.open
- windows.10.arm64
- windows.10.arm64.appcompat
- windows.10.arm64.open
- windows.10.arm64.perf
- windows.10.arm64.perf.surf
- windows.10.arm64.tof
- windows.10.arm64v8.open
- windows.11.amd64.cet
- windows.11.amd64.cet.open

##### Possible issues you may encounter:

When the dotnet-helix-machines-ci build to be rolled out fails with `{QueueOrImageName} has estimated removal date: {c.EstimatedRemovalDate}, which has elapsed.  Either extend this date in the yaml definition or remove it from usage to proceed.`:

1. Review the date and confirm using internet search / .NET Release PM Team calendar.  **Note the "Matrix of Truth" data, once it exists, supersedes any data in "Estimated Removal dates"
2. If the date is incorrect, consult with DncEng Operations v-team (or, use judgment) to determine a new date and extend it via pull-request to this repository
3. If the date is valid, remove the definition via pull request to this repository.  If we have erroneously never communicated this state, you may use discretion to set a date sometime in the future to keep the current status quo, but please include a comment over the definition explaining why.


When the build to be rolled out contains warnings with `{QueueOrImageName} has estimated removal date: {Date}, in the next three weeks. Please include this in communications about upcoming rollouts.`:

1. Review the date and confirm using internet search / .NET Release PM Team calendar and release notes.  Example: [.NET Core 6.0 release notes](https://github.com/dotnet/core/blob/main/release-notes/6.0/supported-os.md)
2. If the date is incorrect, consult with DncEng operations v-team to determine a new date.
3. If the date is valid, work with the .NET Engineering Services Rollout team ([Teams Channel](https://teams.microsoft.com/l/channel/19%3a72e283b51f9e4567ba24a35328562df4%40thread.skype/Rollout?groupId=147df318-61de-4f04-8f7b-ecd328c256bb&tenantId=72f988bf-86f1-41af-91ab-2d7cd011db47)) to ensure that this is communicated with every partners "rollout" email until the removal has occurred.

The similar warnings/errors for Update Required look like the below.  Their only real difference is that these updates only are shared with the rollout team / customers in the case where a "significant" change occurs (e.g. updating the semi-annual Windows version).

`{QueueOrImageName} has update-required date: {Date}, which has elapsed.  Either extend this date in the yaml definition (add comments if relevant), or remove it from usage to proceed.`
`{QueueOrImageName} has update-required date: {Date}, in the next three weeks. Please either update the image to newer, file an issue requesting this, or extend the date with a comment explaining why if no action is taken.`

#### Known issues impacting the area: 
- We regularly have difficulty generating "novel" new images; when this occurs whoever is driving the process should extend dates (with comments why) as proactively as possible, since we'd like to minimize communication to users that implies actions are needed on their part if we know we don't have something for them to upgrade to.
- Known tech debt: Completion of "Matrix of Truth" work (needed for unified tracking of expiration dates)
- Troubleshooting guide per-step, ideally tested by execution by an individual unfamiliar with the feature area(s) involved: None for now, can add as users hit issues.

#### Troubleshooting:

- Ensure all EstimatedRemovalDate/UpdateRequiredDate values are expressed in YYYY-MM-DD format.
- Use DncEng team for guidance when investigating errors

### Validation Steps

After completing manual steps: (cadence TBD, but probably weekly before rollouts), perform the following checks and make a note in https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/512/Helix-Machine-Management-Operations-Notes

Template (insert at top of wiki mentioned abov)
```
Date:
Executor of manual checks: (Github or MS alias)
Link to Production rollout pipeline: 

Pipeline state:
- Monitor the next dotnet-helix-machines pipeline execution.  No warnings or errors related to EstimatedRemovalDate time elapsing should be seen.  Provide a link to this pipeline execution.  Validate that all queues expected to be deleted did actually get deleted (this can be seen in the "Run DeployQueues" step of the Deploy Queues job).
- Monitor First Responders Teams channel for surprised users; in the case of erroneous deprecation, work with DncEng team for a hot fix.  In the case of expected removal, use discretion and help unblock the user.

Notes: 
- Anything interesting or unusual that happened as part of this week's check-in.
- Issue(s) falling out of the process for this week:
```

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5COperations%5CHelix-Machine-Management%5CHelix-Machine-Lifecycle-Processes.md)](https://helix.dot.net/f/p/5?p=Documentation%5COperations%5CHelix-Machine-Management%5CHelix-Machine-Lifecycle-Processes.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5COperations%5CHelix-Machine-Management%5CHelix-Machine-Lifecycle-Processes.md)</sub>
<!-- End Generated Content-->
