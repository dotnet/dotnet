# .NET Core Engineering DevOps V-Team Responsibilities (Tentative short name : "DncDevOps")

Note that TKapin and IlyaS have already spent some time thinking about this problem.  That document is [here](https://microsofteur-my.sharepoint.com/:w:/g/personal/tokapin_microsoft_com/EWK52KdVvIZCsfLqe6idj6QBYzML12mx82xsmwtGt6H-Ug?e=PiAiUU)

The above doc defines developer operations work as "Long lived, predictable, repetitive, and possibly highly manual work that needs to be performed periodically to keep providing healthy and secure services to our customers. For the physical world analogy, one could think of operational work as “turning the crank” of a complex machine to keep it operating."

## "North Star" statement

The .NET Core Engineering DevOps (DncDevOps) team strives to use its resources to make daily, repetitive operations undertaken by the team knowable, repeatable, automated (where possible), investigate-able, visible and secure. In doing so it will provide simple means for team members to understand existing and onboard new processes to this ownership. Processes and documentation should strive to be sufficient for vendors or new team members to be able to succeed at executing them.

### Key principles

- 'Manual' isn't a dirty word.  Document and get running first, automate later.
- Strive for simple, procedural guidance for processes.
- Anything developed by this team needs to be written down, validated by someone else than the author, and findable by all
- Don't lose issues that don't meet the below bar or FR's; keep customer needs moving until closed or pathed somewhere.
- When we ask for a document, we have a template for it and the template _says where it should be put_.

## Bar for activities owned by DncDevOps

While exceptions will be made on a case-by-case basis, the following rubric should be applied to incoming requests determining whether it's appropriate to be handled by the DncDevOps team. Note that many of the tasks that will be handled eventually by DncDevOps are handled by the first responder team today.

*Is the request for work in an established area?* DncDevOps should not be owning tasks or processes under their first round (or two, depending on complexity) of development; owned areas should be as close as possible to feature- and documentation-complete, and ready for daily usage. By definition, functionality already used in a .NET Core release shall be considered to be established.  If this is not the case, work should be handled by the feature's active v-team, or possibly the First Responders (FR) team.

*Is the work in an area under active development?* The DncDevOps team will work with epic teams to ensure that sustaining operations tasks are documented and reviewed before epic signoff, but in general while these areas are being developed it is preferable for that team to handle day-to-day tasks.

*Is the work addressing immediate customer pain?*  Regardless of how manual they may be, short-term customer pain issues should be handled by the First Responders team.  Examples include creating a custom branch in a repository, adding a new service connection to an Azure DevOps project, or a one-off investigation of a particular error being faced by the customer.  DncDevOps tasks include internal-only issues that have no short-term customer impact. They should represent tasks that repeat on at least weekly or biweekly cadence, such as (using the above "no" list as examples) monitoring the active branches used to ship a repo, keeping track of the service connections we create (why they exist, when they expire, who uses them), fixing issues found in FR investigations, or monitoring ongoing builds by a group of repositories to keep them healthy.

*Is the task pertinent to the daily functioning of the .NET Core Engineering team or a team it supports?* One should be to make a clear, concise value statement about a DncDevOps-owned area's usefulness related to daily work by dnceng or a team it supports, whether it be for reliability, security, or general hygiene purposes.

If you can reasonably answer "yes" to this list of questions, you're likely looking at something that should be housed in the "operations" epic.  There will be quite a bit of overlap between DncDevOps and First-Responders, especially in the beginning, as these teams are meant to function as a complementary pair.  Things which are not feature work but aren't devops either include FR work that addresses immediate customer pain, build failure investigations, guiding users on Teams, and really most one-off work items

## What is the process by which we onboard DncDevOps tasks?

If you've read the above bar and have something you'd like to onboard as a operational responsibility (or make changes to the existing processes), please follow the below process:

- Create an issue in the https://github.com/dotnet/core-eng repository with the 'Proposed-for-DncDevOps' label.

Suggested issue template:

```
<!-- If these statements apply, replace [ ] with [x] before filing your issue. -->
- [ ] This issue is relevant to daily .NET Core Engineering tasks
- [ ] This issue describes ownership of an ongoing responsibility pertinent to the .NET Core Engineering Services team

Related issue(s) / documentation:
<!-- Links to relevant epic work (should be in an established area), existing documentation -->

<!-- How much time per day does this cost currently to do? -->
- Requirements / estimated daily cost of ownership:

<!-- How much of this is manual, and how much can be automated? Where do we need to invest? -->
- (Known) automation debt to pay:

<!-- How do we (...if we do) meet this need today? -->
- Current state:

<!-- How will this help us do our daily routine? -->
- Benefit of DncDevOps owning this:

<!-- Write your issue description below. -->
Proposed DncDevOps ownership:
```

- At least once a week, tech leads of the FR and operations teams will review all issues with this tag.  If accepted, the issue will be either converted to an epic or directly added as a child of the "DncDevOps work" epic in its backlog, sorted by priority order at the discretion of these teams.

- If rejected for DncDevOps, we can have the conversation about sending the issue to FR or general triage process. This does not mean the issues just go to a dumping epic; issues with real consistent pain, customer or internal, should not just 'rot'. Unfortunately sometimes this will necessitate the user who wants this addressed to continue advocating for it.
- The goal is that automation will be added/found for as much of the process as reasonably possible, always striving for reuse / addition to existing systems. 

- For accepted tasks:

  - Work will be placed into the ZenHub backlog for the [Operations epic](https://github.com/dotnet/core-eng/issues/14471)
  - Using the terminology of the above linked document, at a high level the next steps are:
    - Phase 1: Gathering requirements (ideally this stage is already done, performed by whoever is authoring the issue, but the operations team will need to evaluate this and agree with it).  This may be part of the acceptance process, since the requirements directly influence cost.
    - Phase 2: Designing the routine. By the end of this stage, documentation of process should be under https://github.com/dotnet/core-eng/tree/main/Documentation/process/{area-name}. 

      - Documentation in this folder should include at least a "operations-info.md" based off [process-info-template.md](./process-info-template.md) containing:
        - Frequency of operations performed, with start/end dates as applicable
        - Details and links to any source code / 
        - Step-by-step instructions which should be signed off by one not-the-author developer with subject matter expertise and one individual who has to own/execute the task.
        - Troubleshooting section - case studies of previous issues
        - Escalation ICs - SMEs who can be contacted if the troubleshooting and instructions are insufficient.  If these people are contacted, there must be an update to the troubleshooting section even if it's just clarification of terminology.

      - Routines will be made up of pieces which fall into three general categories.
        - Fully automatable routines : For these, the work is just like any other epic, and the ongoing cost is simply to ensure that the automation is still running and react to alerts.  Processes added this way still need to have an entry under the wiki page named below.
        - Non-automatable, simple ("highly procedural") routines: Things we can't or won't automate, but can be described by a simple flow chart and troubleshooted via any search engine, at least a majority of the time.
        - Non-automatable, complex routines: Same as above but which will require significant work, consultation with subject matter experts, proof-of-concept development, etc.
    - Phase 3: Tooling and telemetry implementation - Writing the automation we think we need to support the work
    - Phase 4: Validation:  I've left out the "rollout" phase here because this kind of process is happening whether we are methodical about it or not, whatever we do. If the task fails validation, at any time, it should be reevaluated via a new issue in the operations epic tracking the work. In general this process is something like:
        - Executor of this task reads and clarifies documentation added in Phase 2 with team members before starting
        - Executor then goes through some number of iterations of the process (number at their discretion), taking notes every time they have to seek help and what the answers they got seeking such help were.
        - Operations team reviews the executor's notes as available and makes iterative improvements to documentation / automation.
        - Executor makes the final "this is ready" call; this is not to say that there won't be more clarification / addition in the future.

  - If vendors are to be used, a "statement of work" needs to be drafted (JasoWard on DDFUN has expertise) to ensure the most productive use of vendor resources. Process documentation needs to indicate clearly what success and failure indicators for this task are critical in making this process work.
  - Once deemed 'ready' to operate (generally entering Phase 4 above), ownership, a brief entry stating purpose, and links to relevant work items and process documentation will be listed under https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/107/DncEng-DncDevOps-Ownership

- At a certain point, determined primarily by the personnel funding of the DncDevOps / FR teams, taking on new ownership will necessarily entail either simplifying (further automating / reducing scope), deprioritizing, or removing older tasks, as these tasks are expected to have a perpetual ongoing cost.

## Examples

The following are some sample tasks that the DncDevOps team might own on a continuing basis, with the above rubric used to justify why these would be done. At the moment these ideas mostly represent brainstorming but are listed in descending priority order.

### Ensure all pipelines owned by the DncEng infrastructure team are kept in a passing, "green" state, with a playbook for taking action when broken.

  - **Established area?** Yes, (assuming we manage the list / dashboard for this correctly)
  - **Area under active development?** Mostly no: Current epics will break the builds somewhat but that's indirect.
  - **Repeating work?** Yes, it never ends.
  - **Pertinent to the daily functioning?** Yes; broken builds mean lost productivity.

### Enforce our team's OS and artifact versioning / lifetime

Track and drive patching (both operating systems and artifacts installed thereon), end-of-life-ing dead OSes, and updating physical machines used by the team. Keep base images used by Helix clients updated on https://github.com/dotnet/dotnet-buildtools-prereqs-docker, delete end-of-life OSes from this repo, and respond to non-customer requirements for these (security updates, moving to other repos, adding new images). Communicate at least two weeks in advance (via dncpartners dl) end-of-life for Helix queues and fully remove them from support.

  - **Established area?** Yes, dotnet-helix-machines is quite old
  - **Area under active development?** No epics occurring in machine management.
  - **Repeating work?** Yes, it never ends.
  - **Pertinent to the daily functioning?** Yes, very much. Keeping the images we use up-to-date improves security, testing (since we want to catch issues before the users), and establishing a process for this lets us uptake new functionality and versions needed by partner teams proactively.

###  Manage DncEng-owned service connections in a documented fashion (and possibly variable groups)

  - **Established area?** Yes or N/A: we've used them a long time but they're just an artifact we depend on.
  - **Area under active development?** No, they are a piece of how our inter-connected AzDO / GitHub projects function.
  - **Repeating work?** Since the PATs backing them always expire and require logging in as dn-bot to cycle, it never ends.
  - **Pertinent to the daily functioning?** Yes; broken connection means broken builds means lost productivity.

###  Regularly find and delete vestigial objects in Azure (old image galleries, defunct services, old forgotten repro VMs, etc).  Automate reporting of this.

  - **Established area?** Yes: represents all the existing deployed production work we do in Azure
  - **Area under active development?** Typically not; this represents work that is deployed to production.
  - **Repeating work?** Yes; users may add random new objects to Azure at any time, leading to insecurity or spending waste.
  - **Pertinent to the daily functioning?** Yes, it is possible for these objects to consume limited Azure resources we expect to be reserved for what we expect to be there.  Further, the objects may represent a security concern if we don't know why they're there, so knowing of their existence is very helpful.

### Some general ideas for tasks outside of this but which would support operational quality:
- Document investigation processes via core-eng wiki or elsewhere for vendor execution in non-exceptional circumstances.
- Make templates and help communicate important changes in services provided by the DncEng team

## Process / Rules for DncDevOps team

The DncDevOps team will be focused on 'predictable work' - stuff that doesn't have an SLA for hours.  Since the work will be largely done by the First Responders team in the beginning, the hours would likely follow that with some coordination with the Prague team when special requirements make it preferable.

### Primary goals

1) Inventory, monitor, and observe key pieces of .NET Core Engineering services infrastructure to keep the system in a secure, usable state
2) Communicate important changes to customers with a consistent format and pattern via email and Teams
3) Triage issues proposed to be operational using the bar above, and coordinate with the team for rejected issues outside this (goal being to not let issues "rot" whatever they are)
4) Document and offload as much repetitive / manual work as can be to vendor ICs, freeing up time for investigation and improvement.
5) Regularly engage with (and redirect where needed) customers who not seeking short-term redress to hear out their challenges / issues and represent this.  Maybe hold office hours?

### Organization

- Unknown - this wil depend on the scope of tasks owned. Since the tasks are much more of the "continuously ongoing" type though versus responding to incoming customer pain, it makes sense to not cycle through ICs every week. I believe 1 or 2 FTE ICs and some number of vendors would likely serve the cause well.

### Synchronization and Hand Off

- As the First-responders team (FR) will be intimately involved with the DncDevOps team, it makes sense to unify the meeting used for both purposes. If this not acceptable by the FR lead, a regular morning-time (to allow Prague to participate) 15 minute daily Teams standup would be held.
- Whenever feasible, high-priority customer-facing work should be handed off to the First Responders team as this would be outside the DncDevOps charter.

## Filing Issues

- DncDevOps issues need to be created in the dotnet/core-eng repository. The usual process of filing an issue with only a link to a devdiv.vs Azure DevOps TFS work item would be followed for any issues considered "security sensitive".
- Use necessary tracking system for vendor work, which may be IcM or other non-GitHub system.
- Issues should be tagged with the "DncOp" label and added to the DncDevOps ongoing epic.

- Guidance for issue creation
  - Description of the task or bug 
  - Where in the code base (which repo / project/ etc) the work should be done, if known.
  - Link to instructional documentation if a vendor task.

## Notes

This is a draft: This list is currently a set of ideas, with no specific order. Primarily, I am trying to write down things that keep the system going that either don't have a clear ownership today, or for which ownership could be transferred.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5COperations%5Cdnceng-operational-responsibilities.md)](https://helix.dot.net/f/p/5?p=Documentation%5COperations%5Cdnceng-operational-responsibilities.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5COperations%5Cdnceng-operational-responsibilities.md)</sub>
<!-- End Generated Content-->
