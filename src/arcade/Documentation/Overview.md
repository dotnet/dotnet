# Arcade

## Sharing Infrastructure Across .NET Core

### Overview / Introduction
We need well-understood and consistent mechanisms to consume, update, and share engineering infrastructure across the .NET Core team.

The primary concept is to break the infrastructure into “pay for play” components, such that one piece of functionality can be consumed, with minimal dependencies.  The idea is to not force unnecessary dependencies, thus making it more reasonable to consume only what is needed.

This approach publishes what amounts to “public surface area” for the shared engineering infrastructure.  These “contracts” then allow for the product teams to reason about how (or if) they participate and manage their engagement with the common infra over time.  In short, the product teams “pull” what is needed, when it’s needed.

### Methods for Consuming the .NET Core Shared Infrastructure Components

- MSBuild tasks/targets as NuGet packages
- Known "entry points" (repo API) in each repo to build, test, package, sign, and publish
- Azure DevOps extensions  (check the box in Azure DevOps)
- Hosted services with REST end points which are owned by the Engineering Services
- Toolsets (think compilers, training tools, etc) as binaries in their own setup or NuGet packages
- Machine (VM) images and/or Docker containers
- "Resources" as planned by Azure DevOps.

### Principles
- Updates and changes should always be done with all the repos (not just yours) in mind.  This implies compromise by all to achieve a better common goal.
- Simplicity and austerity are our friends.  We want to avoid clever, magic, or fancy features.
- Incremental updates where ever humanly possible.  (avoid big changes with a "switch" when possible)
- When something needs to be done across multiple repos, the extra work required to put the feature in Arcade is worth it.
- Where appropriate, testing should be in one repo before "graduating" to Arcade.  This way we can learn more what's needed, thus minimizing churn.
- When making a breaking change, compat switches or branching is required.  (largely due to servicing)

### Business Value (to remind us why we think we should do this)
-  Build on the success of others.  Namely, being able to _reasonably_ share functionality across teams and repos.
-  Control and ownership.  Repo owners/devs can manage _what_ tools are needed (and which aren't), as well as _when_ they take it.  This includes not only new functionality, but almost more importantly, updates to existing.
-  Dev work flow.  Allows devs to "plug and play" when modifying or bringing up new functionality in the build without having to re-invent the wheel.  In addition, the _how_ of build tools is largely understood - even across repos.

### Toolset NuGet Package Requirements
-	If used by the build, the tool should be packaged, deployed, and consumed as a NuGet package.  (compilers, training tools, etc are out of scope.  See above list)
-	Every package must be versioned.  (Proposal: https://github.com/AArnott/Nerdbank.GitVersioning)
- There needs to be a clear and easy way to bootstrap (get started and/or add a package).  (Proposal: https://github.com/chcosta/roslyn-tools/blob/bootstrap/docs/Toolset-Bootstrap.md)
- A dev should be able to clone, then build without worrying about VM config or other prereqs.  (It's understood that this may not be 100% achievable today, but it should be the north star.)
-	Each package is serviceable (forkable) itself, and can be easily used for servicing of the product.  The idea is that the common infra should “fork” with the product repo branch.  (see below for implementation notes/questions as this area is likely challenging to get right)
- Each package must carry key pieces of meta-data for auditing.  Example: source repo link and commit SHA
-	Each contract represents a single area of work.  In other words, there is specific “intent” for each contract/package.  (contrasted with general “helper” stuff, or lumping several things together)
-	New packages must be reviewed by the product teams in conjunction with Engineering Services.  (think API reviews…)
-	Existing consumption of common infra will be migrated to this new approach, piecemeal over time as appropriate.  No giant “switch”….  (plus, some things don’t need to migrate)
-	In addition to unit tests at the code level, end-to-end validation tests are required to ensure the contracts are still fulfilled.
- Each package should carry its own documentation that is updated when the tool/package is updated.
-	Each contract includes telemetry publishing for both a) usage and errors, and b) visibility (like Mission Control)
- There is a "core" set of tool packages defined which every participating repo get.  Other packages produced by different product teams are also available, but these "curated" packages not automatically brought down by default.
- New toolset packages should generally be extensively used in one repo, then if warranted, promoted to become more generally available.

### What shared tools should be part of the Arcade SDK and which should not?

- If a tool provides functionality which is meant to be used by all of the [Tier 1 Repos](TierOneRepos.md) then we should add it to the SDK. 
- If the provided functionality will only work for a couple of repos then these won't be part of the SDK and will have to be manually referenced in a project.
- The SDK comes with props and targets files that are imported to any repository using the SDK, or invoked by common build infrastructure.
- These files automatically import required packages via `PackageReference` (e.g. Microsoft.DotNet.SignTool, MicroBuild.Core, etc.)
- The SDK allows the repository to opt-into additional packages as needed (VSIX authoring, XUnit testing etc)

### Arcade building repos Requirements
- Arcade builds, tests, packages, signs, and publishes itself using itself and the shipping SDK/CLI
- Arcade itself must build from source.  This does not necessarily apply to the packages hosted by Arcade, but building from source should always be kept in mind.
- All tools bootstrapped in, getting as close as technically possible to 'clone and build' on a clean machine
- A repo level API is explicitly defined and implemented, not just implied
- Method exist to directly maintain and update Arcade in each participating repo
- Arcade (and its packages) is reasonably serviceable


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5COverview.md)](https://helix.dot.net/f/p/5?p=Documentation%5COverview.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5COverview.md)</sub>
<!-- End Generated Content-->
