# The Unified Build Almanac (TUBA) - Managing SDK Bands

## Purpose

This document describes the problematics of managing multiple .NET SDK bands and discusses how we propose to solve this in the Unified Build world during the .NET 9 timeframe using the new build methodology and the full VMR. The document first gives context and explains how we do it today. Then there are two possible solutions discussed and compared.

## Terminology

This section presents more precise definitions of common terms used in this document that may be prone to confusion. Also see the [Unified Build terminology](./Terminology.md) for more.

- **Individual/Source repository** – One of the current development repositories, e.g., `dotnet/runtime`. An "individual product repository" is then one that contains code for part of the product (but no individual repository contains code to build the whole .NET Core product).
- **VMR (Virtual Monolithic Repository)** – A repository containing code layout that produces the official build product of .NET Core. The repository contains individual product repositories plus tooling to enable a full build of the product.
- **VMR-lite** – present state where changes only flow one way into the VMR which is read-only.
- **Full VMR (Backflow)** – future final state where changes can be made in the VMR too and flow both ways. Dependency flow at that time will be only between VMR and individual repos, not between individual repos themselves.
- **Source-Build** – A set of sources and a process which allows to build the entire product end to end including all its dependencies in offline mode, excluding native dependencies from the source.
- **Microsoft build** – The current build methodology used to assemble the final product that Microsoft ships binaries from.
- **SDK branch** – A git branch related to a specific SDK band, e.g. `release/8.0.1xx`.
- **Non-SDK branch** – A git branch common for all associated SDK bands, e.g. `release/8.0`.
- **Build output packages** – Packaged build products of each of the individual repositories either built in their individual repo source-build or during the build of each individual repository component within the full VMR build. These are used during package flow between the VMR and the individual repositories, and in the VMR build itself.
- **Shared component** - A component that is shared between multiple SDK bands. For example, the .NET runtime is shared between all SDK bands.
- **Band-specific component** - The opposite of a *shared component*. A component whose version differs between SDK bands.
- **Maestro** - a service used by the .NET team to manage dependency flow between repositories.

## SDK bands

To align with new Visual Studio releases, .NET SDK updates sometimes include new features or new versions of components such as Roslyn or MSBuild. These new features or components may be incompatible with the versions that shipped in previous SDK updates for the same major or minor version. To differentiate such updates, the .NET SDK uses the concept of feature bands. While these bands differ in their feature set they share some common parts such as the .NET runtime.

### Shared vs band-specific components

A shared component is a component that is shared between multiple SDK bands. For instance, the .NET runtime is a good example of a shared component while the Roslyn compiler would typically differ between bands.

During the development cycle, it can happen that shared components require band-specific changes and they can become band-specific for some time. Usually, this is a point in time event and the component becomes shared again after some time. A good example of this is the Arcade repository which contains build tools/infrastructure. However, there are no strict rules about this and it is possible that a component remains band-specific.

### Example

To best illustrate how SDK bands are developed and released in practice, let’s imagine the following timeline for repositories with SDK branches (e.g., `dotnet/sdk`):

```mermaid
%%{init: { 'gitGraph': {'showCommitLabel': false }}}%%
gitGraph
    commit
    commit
    branch release/8.0.1xx
    checkout release/8.0.1xx
    commit
    commit type: HIGHLIGHT tag: "Release #1 – SDK 8.0.100"
    commit
    checkout main
    commit
    commit
    commit
    checkout release/8.0.1xx
    branch release/8.0.2xx
    checkout release/8.0.2xx
    commit
    commit
    commit type: HIGHLIGHT tag: "Release #2 – SDK 8.0.205"
    checkout main
    commit
    checkout release/8.0.1xx
    commit type: HIGHLIGHT tag: "Release #2 – SDK 8.0.109"
    commit
    commit
    checkout main
    commit
    checkout release/8.0.1xx
    commit
    commit
    checkout release/8.0.2xx
    commit
    checkout release/8.0.1xx
    commit
    checkout main
    commit
    commit
    commit
    checkout release/8.0.2xx
    branch release/8.0.3xx
    checkout release/8.0.3xx
    commit
    commit
    checkout release/8.0.2xx
    commit type: HIGHLIGHT tag: "Release #3 – SDK 8.0.207"
    checkout release/8.0.1xx
    commit type: HIGHLIGHT tag: "Release #3 – SDK 8.0.111"
    checkout release/8.0.3xx
    commit type: HIGHLIGHT tag: "Release #3 – SDK 8.0.302"
    checkout main
    commit
```

In parallel, this would represent the state of a shared repository (e.g., `dotnet/runtime`) that is not specific to any particular SDK band:

```mermaid
%%{init: { 'logLevel': 'debug', 'theme': 'base', 'gitGraph': {'showCommitLabel': false, 'tagLabelFontSize': '24px'}} }%%
gitGraph
    commit
    commit
    branch release/8.0
    checkout release/8.0
    commit
    commit type: HIGHLIGHT tag: "Release #1 – Runtime 8.0.0"
    commit
    checkout main
    commit
    commit
    commit
    checkout release/8.0
    commit
    commit
    commit type: HIGHLIGHT tag: "Release #2 – Runtime 8.0.1"
    checkout main
    commit
    commit
    commit
    checkout release/8.0
    commit
    commit
    checkout main
    commit
    checkout release/8.0
    commit
    commit
    checkout main
    commit
    commit
    commit
    checkout release/8.0
    commit
    commit type: HIGHLIGHT tag: "Release #3 – Runtime 8.0.2"
    checkout main
    commit
    commit
```

On the image you can see timelines of branches of two different repositories – `dotnet/sdk` and `dotnet/runtime`. As noted previously, each (servicing) release of .NET may contain multiple SDK bands but **only** one runtime. Each individual repository of each component that needs to differ per band would then have these so-called *“SDK branches”* named `release/Z.0.Yxx` while repositories that are shared per release have the non-SDK `release/Z.0` branches. As shown in the example, the development of the single runtime would happen in the `release/8.0` branch while the various SDK bands are stored in the following SDK branches (e.g., `release/8.0.1xx` represents the “100th band”).

Once we hit each release day (denoted with red vertical lines), we take the latest commit of each of those branches (that already exist) and release those together. For the releases in the example diagram, these are the released compilations:

| Release |   Runtime   |               SDKs              |
|:-------:|:-----------:|:-------------------------------:|
|     #1  |     `8.0.0` |            `8.0.100`            |
|     #2  |   `8.0.1`   |       `8.0.109`, `8.0.205`      |
| #3      |   `8.0.2`   | `8.0.111`, `8.0.207`, `8.0.302` |

### Current code flow

To organize what ends up in each band and to drive the code flow between the repositories, we utilize the Maestro dependency flow, namely the Maestro channels (see [Channels, Branches and Subscriptions](../BranchesChannelsAndSubscriptions.md) for details):

- **VS-centric channels** – To better match how teams operate, some repositories align their build outputs with the Visual Studio versions, e.g. `dotnet/roslyn`. Outputs of repositories like that would end up in a channel named based on the version of VS, e.g. `17.5`.
- **SDK band channels** – The repositories that are closer to how we organize the final release are then targeting channels named based on the band version, e.g. `.NET 7.0.3xx SDK`.
- **Shared component channels** – Lastly, repositories with shared components and tooling repositories target channels named based on the major .NET version, e.g. `.NET 7` or `.NET 7 Eng`.

The following diagram shows a simplified example (some relationships are left out for brevity such as tooling leading to all repositories):

```mermaid
flowchart TD
    classDef Channel fill:#2487DF,stroke:#fff,stroke-width:1px,color:#fff;

    roslyn174[dotnet/roslyn<br />dev/17.4]
    channel174[(VS 17.4 channel)]
    sdk2xx[dotnet/sdk<br />release/7.0.2xx]
    channel2xx[(.NET 7.0.2xx SDK<br />channel)]

    subgraph Shared components
        runtime[dotnet/runtime<br />release/7.0]
        aspnetcore[dotnet/aspnetcore<br />release/7.0]
        arcade[dotnet/arcade<br />release/7.0]
        channel7[(.NET 7<br />channel)]
        channel7Eng[(.NET 7 Eng<br />channel)]
    end

    roslyn175[dotnet/roslyn<br />dev/17.5]
    channel175[(VS 17.5 channel)]
    sdk3xx[dotnet/sdk<br />release/7.0.3xx]
    channel3xx[(.NET 7.0.3xx SDK<br />channel)]

    roslyn174-->channel174
    channel174-->sdk2xx
    sdk2xx-->channel2xx
    channel2xx-->installer2xx

    runtime-->channel7
    aspnetcore-->channel7
    arcade-->channel7Eng

    channel7-->installer3xx
    channel7-->installer2xx
    channel7Eng-->installer2xx
    channel7Eng-->installer3xx

    roslyn175-->channel175
    channel175-->sdk3xx
    sdk3xx-->channel3xx
    channel3xx-->installer3xx

    installer2xx[dotnet/installer<br />release/7.0.2xx]
    installer3xx[dotnet/installer<br />release/7.0.3xx]

    class channel174,channel175,channel2xx,channel3xx,channel7,channel7Eng Channel
```

This setup makes sure that the latest version of each shared component (e.g., runtime) eventually flows to all SDK products. Over time, the SDK products become coherent. **We call the SDK bands coherent when the versions of all shared components of each band are the same**.

### Band lifecycle

As described above, the band lifecycle is tightly coupled with the releases of Visual Studio. The exception is 100th band that ships on .NET's annual schedule and VS snaps to it. For repositories that target the VS-centric bands, the `main` branch usually targets the next VS version which is in preview. Once a version of VS is released (is GA-ed), we create a new branch named `dev/XX.Y` where `XX.Y` is the version of the released VS. The `main` branch then targets the next VS version in preview

As an example, let's say we have the following setup:
- VS `17.1` is the latest stable version of VS and is associated with the `7.0.1xx` band.
- VS `17.2` is in preview and is associated with the `7.0.2xx` band.
- VS-centric repositories would have a `dev/17.1` branch targeting the `17.1` channel and a `main` branch targeting `17.2`.
- The `17.1` channel would then flow into the `7.0.1xx` branches and the `17.2` channel would flow into the `7.0.2xx`.
- The `7.0.1xx` branches would then flow into the `7.0.1xx` SDK channel and the `7.0.2xx` branches would flow into the `7.0.2xx` SDK channel.
- The `7.0` channels would collect builds of shared repositories and flow into `7.0.Yxx` branches that are currently in servicing.
- The `7.0 Eng` channels would collect builds of shared eng repositories and flow into their respective all `7.0.Yxx` branches, including previews.

For this setup, we'd say the 100th band is in **servicing** and the 200th band is in **preview**. It is important to also note that **while a band is in preview, it uses the most recently released .NET runtime** while **the servicing band revs with the `7.0` channel**.

When we would be ready to release VS `17.2`, we'd flow the latest shared components into the 200th band branches so that it becomes coherent with the 100th band. Then we'd release the coherent bands and after we would do the following:

- We'd create the `dev/17.2` branch in VS-centric repositories, point it to the `17.2` channel and retarget `main` to `dev/17.3`.
- We'd snap branches of SDK repositories by branching `7.0.3xx` from `7.0.2xx`. While doing that, we'd update the runtime version of `7.0.3xx` to the just released version.
- We would set up the `7.0` shared channels to start flowing into the `7.0.2xx` branches as the runtime there would start revving.

Technically, the steps above could happen in a different order based on repository needs. The important part is the alignment of repository's development branches with the Maestro channels.

### Full code backflow and Maestro

Currently, the [VMR is synchronized](./VMR-Design-And-Operation.md#source-synchronization-process) based on the `dotnet/installer` repository mapping its commits 1:1 with `dotnet/installer`. This will have to change once we switch over to the full code backflow model.

To re-iterate what the planned code flow looks like for .NET 9 (with full VMR back flow) – the individual repositories only receive and send updates from/to the VMR and not between each other, so the situation looks like this (see [VMR Code and Build Workflow](./VMR-Code-And-Build-Workflow.md) for details):

```mermaid
flowchart TD
    VMR[VMR]
    arcade[dotnet/arcade]
    runtime[dotnet/runtime]
    roslyn[dotnet/roslyn]
    sdk[dotnet/sdk]
    other[...]

    arcade-->VMR
    runtime-->VMR
    roslyn-->VMR
    sdk-->VMR
    other-->VMR
    VMR-->arcade
    VMR-->runtime
    VMR-->roslyn
    VMR-->sdk
    VMR-->other
```

The updates of the VMR will no longer happen when `dotnet/installer` is updated but rather whenever a new build appears in one of the channels. The information making the builds of the `dev/17.4` branch of `dotnet/roslyn` end up in the `7.0.3xx` SDK band is stored in the configuration of Maestro subscriptions between those branches. The Maestro service will have to follow this configuration and update the corresponding sources (the right folder of the right branch) of the VMR accordingly. It will also have to flow changes the other way too when a change is made in the VMR or when VMR produces a new build output package. **This is all new functionality that Maestro will have to implement.** That being said, both proposed solutions seem orthogonal to this and the impact on the Maestro changes needed should be minimal.

### Release process

The dependency flow eventually flows all the bits into the `dotnet/installer` repository which also uses the SDK branching. Each of those branches then produces an official build – so one build per band – and we release those. The exact process is that a dedicated person selects all the right official builds which are coherent on the shared bits (so each has the same of the runtime for instance) and inputs the IDs of these builds into the staging pipeline called `Stage-DotNet`. **During this process, it is important that the shared bits are only built once officially and then re-used in the respective band builds.**

The long-term plan is to transition to building and releasing using the Virtual Monolithic Repository which is a repository where each commit denotes a full set of sources needed for building .NET. The sources of this repository are synchronized from the individual repositories based on the contents of the `dotnet/installer` repository. The goal of this document is to discuss how this will be done with regards to both the different bands as well as the shared components.

## Proposed solutions

Currently, we end up with SDK branches in the `dotnet/installer` repository and the release process makes sure to package those into the final product. With releasing from the VMR, we have two ways we can approach this:

- **SDK branches** - [📄 Detailed description of the proposal](./VMR-Managing-SDK-Bands-SDK-branches.md)  
    Keep using SDK branches in the VMR the same way we have them today. This is, in fact, what we’re currently already doing with today’s read-only VMR-lite where we synchronize the SDK branches of `dotnet/installer`. Each branch/commit of the VMR would then keep producing a single SDK. However, we need to make sure the shared bits are the same in each released SDK branch – we’d say the SDK branches would be coherent then. We also need to make sure that changes made to the shared components in VMR’s SDK branches are flown everywhere appropriately.

- **Side-by-Side folders in the VMR** - [📄 Detailed description of the proposal](./VMR-Managing-SDK-Bands-Side-by-Side-folders.md)  
    The second proposed solution would be to take the inverse approach and, instead of having SDK branches, we’d organize VMR’s branches based on the shared bits (e.g. `release/9.0`) and place the different bands of the SDK components side by side in the VMR, e.g. `src/sdk/9.0.1xx`. This makes sure that the shared bits exist only once and each commit of the VMR contains all bands which are coherent.

## Proposal comparison

To compare the two proposals, we identified several areas which might be impacted by the selected architecture:

- **Build** – what would build of a single and of multiple bands look like with regards to Source Build
- **Code flow** – what does a breaking change mean; how do we (back-)flow the code between the VMR and the individual repositories
- **Developer experience** – impact on developer lives and how they work with the VMR; their options for making changes that span multiple repositories
- **Release** – how do we compile the final Microsoft release
- **Validation** – what do we validate (build/test) and when
- **VMR size & performance** – impact of selected architecture on the git repository
- **Community, 3rd parties & upstream/downstream story** – what does it mean for partners to build their own SDK(s)
- **Implementation and maintenance complexity** – risks and costs associated with the future

### Build

The current (Microsoft) way of building the SDKs is based on re-using previously built artifacts which come into the build as NuGet packages. The components are flown as already compiled packages. This means that when building each SDK band, we only restore the shared components which were built only once at some point in the past during the official build of their source repository.  
In .NET 9.0, when the full VMR code flow is in place (see [VMR Code and Build Workflow](./VMR-Code-And-Build-Workflow.md) for details), we’ll be building the individual repositories on more occasions:

- During the rolling build of their source repository – this will use other repo’s build output packages whenever they depend on another repo as if they were just built from source.
- During the official build of the VMR – when we build the whole product from source. This will end up producing an build output package per each individual repository built as part of the whole build.

This means that in several places, we’ll be building both the shared components and the SDK band components from source where their dependencies will be either freshly built or restored in a local NuGet cache.  
Regardless of how this will happen, there’s no real difference whether we’d build the SDK bands from folders which are side by side in a folder of a checked-out branch or which are checkouts of different SDK branches.

The various situations can be summarized as follows:
- For individual repository builds, the build process will restore the dependencies from build output packages.
- For the VMR build shared components are built with the first band and put in a local NuGet feed. Other bands restore shared components from the feed.

Upon inspection of the proposals, the above seems to work for both proposals as we'd be able to supply all of the sources. However, the difference lies in the build process itself.  
For SDK branches, this does not really differ as the layout stays roughly the same. For side-by-side folders, the build would get more complicated than today as each component would have to know which SDK folder of its dependency it should use. It seems quite error prone and difficult to figure out where dependencies came from once we have built everything.  
For this reason, the SDK branches solution wins but the impact on the final architecture is not as big as this would be a one-time cost.

### Code flow

Code flow is where the two approaches differ dramatically. The biggest difference is during breaking changes in shared components and how/when these get resolved. For a simple forward flow where a shared component is changed, the code flow needed to update all branches does not differ much as shown in the detailed designs of each of the proposals.

The situation gets more interesting for breaking changes. However, that does not happen often once we have multiple bands out already. This would mean API changes which does not really happen or rarely when dependencies EOL or infrastructural changes are needed. Regardless, the side-by-side solution shows much more resiliency to breaking changes as those need to be dealt with immediately when we do the initial shared component change. The VMR won’t ever get into an inconsistent state as the bands live within a single commit. Whereas in the SDK branch solution, the breaking change is created and is dealt with in a follow-up step once code flows to the branch of the other band.

Other difference is in the number of steps in the flow to reach a coherent state. This is lower for side-by-side as incoherency is impossible from the start and the system does not need to deal with it. The number of changes needed is not that much higher though as we still need to flow changes to the same number of branches of all individual repositories that are part of the change. This does not differ much whether we flow folders from one or more branches.

### Developer experience

Important area to consider is how the day-to-day interactions of .NET developers with the VMR would look like. We identified the following key actions the developers are interested in:

- **Working with sources** – working with the source files such as searching, usual investigations such as checking file history & looking for symbols, and finally code editing and building itself
- **Git operation complexity** – actions such as checking file history, diffing bands, backporting changes between bands..
- **Git operation performance** – duration of operations such as `git status`. This area is considered separately [later in the document](#vmr-size--performance).

**_That said, it’s important to realize that most of the work and the VMR is the most active in the preview time where we only have one SDK band._**

It is obvious that the SDK branch proposal wins in most of these categories. It stays more true to git by using commits/branches for file versioning rather than folders with version using in their name as it is with the side-by-side layout. This works well with all kinds of tooling and workflows:

- File history breaks with side-by-side folders once we snap the bands as files are copied in a new path.
- File and symbol search might be confusing when going through almost identical side-by-side folders.
- Backporting a change between bands sounds less error prone with SDK branches as it’s just porting commits. Making a change in several bands at once can lead to omitting a change in one of the bands, harder-to-review pull requests and overall it seems to be easier to transfer changes between bands using git-native approaches. Of course it’s possible to utilize patches but once a change is up for review, making sure all bands stay in sync might be problematic.
- Diffing bands sounds easier to do with SDK branches as well as you don’t need to worry which all components are shared and which are not – diffing side-by-side folders might become tedious once we need to compare several repositories at once.

Some points of interest are rather a matter of personal preference – is it better for a developer to make a change in one SDK branch, open a PR and then backport to other branches, or is it better to do everything at once and build/validate it together? Over all it seems that storing code based on how we work with it rather than how we release it sounds better and the SDK branches proposal seems as the superior approach in this area since it optimizes for development rather than bend the layout to how we release the product.

### Release

There are few key parts of the release process:

- **Figuring out what to release** - we need to flow dependencies to the right places and determine which sources are coherent enough to be released.
- **Compiling the binary release** - we need to be able to build all the sources in such a way that shared components do not get built more than once.
- **Publishing and communicating the release of the sources** - publishing of sources so they are easily consumed by 3rd party partners.

#### Figuring out what to release

For side-by-side, we only need to identify a single commit which is easier than SDK branches, for which we need to identify a commit per each band where also the commits of the non-1xx band reference the build output packages of the 1xx band commit.

#### Compiling the binary release

For the release, we'd just collect outputs of the official VMR build(s) so it's quite similar to today's staging pipeline behavior which does that for `dotnet/installer` builds already. This also makes sure we only build the shared components once and we have tested those exact binaries already.

It seems that both proposals would mean we have an official VMR build to take the products from. We currently don't have the build infrastructure to build several bands together but that would happen for both proposals.

#### Publishing and communicating the release of the sources

Last part of the release would be the so-called Source Build release where we'd need to collect and publish the sources representing the release for the .NET distro maintainers. The side-by-side proposal would contain all of the sources within one commit which makes things easier. However, for anyone who only cares about a single band, we'd need to be able to provide some trimmed version of this commit.  
For SDK branches, the situation is a bit more complicated. For a single SDK band release, only the 1xx band branch would contain the sources in such a way that you could build directly. The non-1xx band branches do not contain the source code of the shared components and only reference them as build output packages. This means that we'd need to compile the sources by restoring them from the 1xx band branch. For releases of multiple SDKs together, we'd also need to compile the full set of sources by bringing the branches together.

It seems that while the SDK branches approach brings a bit more complexity, we'll have to create new processes of how to get the sources to our partners for both approaches.

#### Mean time to release

There is one more metric important to consider connected to releases and that is *mean time to release*. This says how long it takes from making an arbitrary change in the product to releasing it. This is very important is it says how reactive we might be in situations like security fixes.

The side-by-side solution needs fewer steps to flow a change between the VMR and the individual repositories but this difference is not dramatic. The flow still needs to happen to/from the same amount of individual repository branches. It only happens for one VMR branch as opposed to all SDK band branches as with the SDK branches proposal.  
The key improvement that Unified Build brings is flattening the dependency graph which will have a big impact and improve this metric regardless of what we choose here.

### Validation

By validation we mean the process of running some set of tests over a changed code. For individual repositories, there is no change compared to today. There is a difference though for changes made directly in the VMR as we need to specify what needs to be built and validated.

It is unclear what the validation story should be when we have all SDKs in folders side-by-side and how it would impact developers. On one hand, validating a shared component change in all SDK branches adds up to more compute time as the shared components will get re-built in each branch. On the other hand, building all non-shared components always impacts every build and that might have negative impact on developer productivity.

<table>

<tr>
<th> SDK branches </th>
<th> Side-by-Side folders </th>
</tr>

<tr>
<th colspan=2> SDK-band-specific component changed </th>
</tr>

<tr>
  <td>We only rebuild 1 band/branch where the change happens. Change is flown to source repo and re-validated there.</td>
  <td>We’d need to detect that we don’t need to build all bands. Change is flown to source repo and re-validated there.</td>
</tr>
<tr>
<th colspan=2> Shared component changed </th>
</tr>

<tr>
  <td>We only rebuild 1 band – the branch this happened. Possible breaking changes with other bands which are detected after we try to flow the change back.</td>
  <td>We’d need to detect that and build/test all bands, validating the change.</td>
</tr>
</table>

### VMR size & performance

By size and performance we mean the implications of each proposed repository layout onto these metrics onto few key metrics that affect common operations we do with the VMR.

#### Git repository size

The expectation is that the size of the overall repository won’t differ much between the two proposed layouts. This is due to the mechanism of how git stores data internally. For illustration of this expectation, imagine a situation where we have two SDK bands which differ in one file only – `src/sdk/foo/bar.txt`. Let’s look at what is inside of the git object database:

- For SDK branches, we have the two commits that describe the two branches. These are equal at first, when we freshly snap the band branches.
- For side-by-side folders, the situation isn’t much different with the two git trees for each of the band folders – `src/sdk/9.0.1xx` and `src/sdk/9.0.2xx`. These are also the same at first – only the parent tree representing the sdk folder is an object created after the snap.
- Once we change `bar.txt`, we’d get a new object for the file itself and then some other:

- For SDK branches, we have the new git trees that describe the parent folders of the changed file leading up to the root of the repository (`/` → `src` → `sdk` → `foo`).
- For side-by-side folders, the situation is again not much different – we get a new set of git trees describing the path from foo through the new `src/sdk/9.0.2xx`.

Looking at this simple example, it hints that whatever solution we pick, the number of git objects we’ll create to capture the changes are around the same.

#### Single SDK source tarball size

Another interesting metric is the archive of sources needed to build a single SDK. It seems that when 3rd parties that only care about one SDK band would be downloading sources of VMR commits, the side-by-side layout incurs quite a big toll on the overall size as we’d need to download the sources of all bands always. The release process might be customized, and we could omit other bands from the tarball.

#### Release source tarball size

By release source tarball we mean an archive of all sources needed to build a whole release containing several SDK bands.
For side-by-side folders, this would simply equal to a VMR commit. For SDK branches, we’d have to do something about compiling the release archive as the shared components need to appear in the tarball just once. For that, we'd have to specify what such a layout would look like and how we would build that as there is no immediate plan for that in the SDK branches proposal.

> 🚧 TODO: It’s a question whether there would be a thing such as “tarball for the whole release with all bands” and what the flow for distro maintaners would be. If there wasn't a need for this, the SDK branch proposal would benefit from this but it would still need a story for assembling sources of a single non-1xx band.

> 🚧 TODO: Resiliency to band explosion – keeping bands in branches seems more resilient to outer requirements such as a sudden increase in the number of bands due to Visual Studio speeding up its release cycle.

#### Git operation performance

We care about the duration of common git operation duration such as checkout or status. We also need to consider scenarios in which we use the VMR – do we care about more bands than one?

SDK branches seem to have the innate benefit of not having to check out all the bands always. This seems like a big win for scenarios where we for instance make changes to one band only. Checking out mostly the same versions of the same files but in few copies will take its toll on the performance of git operations.

#### Summary

From the analysis above, it seems that to declare a winner, we need to consider how often we deal with a single vs multiple bands. Both solutions are a good fit for one or the other, never both.

<table>

<tr>
<th> SDK branches </th>
<th> Side-by-Side folders </th>
</tr>

<tr>
<th colspan=2> Git repository size </th>
</tr>

<tr>
  <td colspan=2>Roughly the same overhead</td>
</tr>

<tr>
<th colspan=2> Single SDK source tarball size </th>
</tr>
<tr>
  <td>Each VMR commit gives us this.</td>
  <td>Release process would have to be customized and other bands omitted.</td>
</tr>

<tr>
<th colspan=2> Release source tarball size </th>
</tr>

<tr>
  <td>Release process would have to be customized and shared components included just once.</td>
  <td>Each VMR commit gives us this.</td>
</tr>

<tr>
<th colspan=2> Git operation performance </th>
</tr>

<tr>
  <td>Ideal for scenarios concerning 1 band. Worse off for multi-band scenarios.</td>
  <td>Ideal for scenarios concerning multiple bands. Worse off for single-band scenarios.</td>
</tr>

</table>

### Community, 3rd parties & upstream/downstream story

There are quite big implications of how we lay the bands out in the VMR on the outside world. 3rd parties consuming .NET might or might not care about building multiple bands. Overall, the fact that we even need to have different SDK bands is native to Microsoft’s rhythm and way of bundling releases. It is a question whether we will impose more pain on 3rd parties by having to build multiple band branches or by having to deal with the side-by-side layout.

For SDK branches, nothing really changes in this regard as you can keep building the branch as you were doing until now and get the SDK you care about.  
For side-by-side, the situation is quite different. We’re suddenly influencing everyone’s experience with the VMR by projecting how we bundle releases into the layout of the code. This has negative implications such as having to check out all the bands always which would for instance prolong all repo operations.

Additionally, both proposals have the problem of locking the preview band on the latest runtime. The SDK branch proposal is more intuitive in this as the SDK branch of the preview band doesn't contain code for shared components. This is better than the side-by-side design which has the sources laid out but they are not used as the preview band will restore them from an build output package. This will cause confusion.

Regardless of the chosen solution, we must be clear on how to interact with the VMR/repositories (e.g. where do we expect the community to upstream their changes to) and we must have communicate it well.

### Implementation and maintenance complexity

There will be several areas where we’ll need to implement new functionality to make the above work:

- **Code/dependency flow** - changes required to flow the code and the build output packages between the VMR and the individual repositories.
- **Source Build** - tooling and infrastructure to build either one or multiple bands from the new layout of the sources.
- **Product lifecycle processes** - tooling connected to processes such as branching, band snapping, servicing, etc.

#### Code/dependency flow

Both solutions will require us to extend the Maestro service so that it understands flowing both sources and build output package versions between the VMR and the individual repositories. Most of the work will be captured in extending the configuration of Maestro subscriptions and working through the problems of the backflow process itself.
For the side-by-side solution, we’ll need to further implement a new code flow model which will allow us to target specific folders within the VMR. This will also slightly complicate several aspects such as configuration of the source mappings in the VMR and how we keep track of which sources are presently in the VMR.

#### Source Build

Both solutions will require us to implement new behaviours into the Source Build infrastructure that will allow us:
- Build one or multiple bands from the new layout.
- Swap between restoring shared components from an build output package and building them from source.

The SDK branch solution is closer to what we have today as the layout of files would stay the same while switching between restoring and building shared components would be common for both proposals. It is also already partially implemented.

#### Product lifecycle processes

Both solutions will require us to define, document and support processes such as snapping the bands. It doesn't look like these would differ much between the two.

#### Maintenance

Both solutions will require the Maestro dependency (back-)flow system to work. The solution will not differ much between the two as we'll be synchronizing code into the VMR and back based on some rules. It's a detail whether those are folders in one branch or multiple.

#### Summary

The SDK folder solution is much closer to where we are these days as the layout of the VMR won't really change. That being said, the complexity of the implementation should not have a high priority when making the decision as it is a one-time cost.

### Comparison summary

|                       Comparison area                       |     Preferred solution      | Impact on decision |
|-------------------------------------------------------------|:---------------------------:|:------------------:|
|     Build                                                   |         SDK branches        |        low         |
|     Code flow                                               |         Side-by-side        |    low/medium\*    |
|     Developer experience                                    |         SDK branches        |      **high**      |
|     Community, 3rd parties & upstream/downstream story      |         SDK branches        |      **high**      |
|     Release                                                 |         Side-by-side        |       medium       |
|     Validation                                              |             tie             |       medium       |
|     VMR size & performance                                  |         SDK branches        |   **medium/high**  |
|     Implementation and maintenance complexity               |         SDK branches        |        low\*\*     |

> \* The impact of code flow may be low, given that most changes in shared components that require changes in the SDK happen when the 1xx band is the only band. So the code flow is not really affected by this edge case. Over all the simplification we are getting with using the VMR is massive regardless of the chosen solution.  
> \*\* The implementation complexity is a one-time cost (but much lower for SDK branches). Maintenance seems to be similar for both.solutions.

## Comparison evaluation

Both approaches seem to have pros and cons. To choose the best approach, we should assign importance to the evaluation areas on which we were comparing these and see which approach seems better.

When doing so, we should take into account the product lifecycle. At first, the most active busy development happens in the preview time (on main branches). Only after the release, we move into servicing and only after then we branch out and snap the bands. We expect the servicing period to last very long but with less activity. During active development, we should prioritize **developer experience** and **code flow** as that has impact on product construction.  
During servicing we need the system to be as frictionless as possible so that we’re able to react to external impulses fast and release fixes fast which hints at prioritizing **code flow**, **release**, and **maintenance complexity**. Some areas should be important in both periods such as **community impact**.

## Conclusion

From the above, it seems that the SDK branches proposal brings more flexibility and benefits over the side-by-side folders. It will mean much easier development experience for the developers and the community. It will also mean that we can keep the VMR size and performance better in check. The only area where the side-by-side folders seem to be better is the release process but that is not a high priority area. The implementation complexity for both code flow infrastructure as well as Source Build is much lower with SDK branches even though this is a one-time cost.
