# The Unified Build Almanac (TUBA) - VMR Design and Operation

The VMR (**VMR** - **V**irtual **M**onolithic **R**epository) is a source layout projection of a set of separate input repositories into a single repository.
Unlike a repository with a set of submodules, this repository is a source copy of the input repositories.

The VMR aims to serve as the build repository for the .NET Unified Build product.
Rather than building as a set of independent repositories and flowing dependencies among them, the entire coherent source of the product is cloned at a single time and built using the [source-build methodology](https://github.com/dotnet/source-build).

## Contents

- [Contents](#contents)
- [Structure](#structure)
  - [Goals](#goals)
  - [General Components](#general-components)
    - [Component Source Types](#component-source-types)
    - [Special Note: SDK Band Layout](#special-note-sdk-band-layout)
  - [Examples](#examples)
  - [Layout](#layout)
  - [Repository Source Inclusion](#repository-source-inclusion)
- [Moving Code and Dependencies between the VMR and Development Repos](#moving-code-and-dependencies-between-the-vmr-and-development-repos)
  - [Forward Flow](#forward-flow)
    - [Dealing with Conflicts](#dealing-with-conflicts)
  - [Back Flow](#back-flow)
  - [Automation](#automation)

## Structure

This section describes how we structure the monolithic repository, how we track what sources and components are synchronized inside and other pieces of the infrastructure needed for the build.

### Goals

- Simple, easy to understand layout.
- The repository contains all source code necessary to build the whole .NET product from source contained in any given commit.
- Other organizations and contributors can use their own *Continuous Integration* systems (e.g. Microsoft uses AzDO, while the Contoso Company might use GitHub Actions).
- Allow for multiple versions of the same code to be used (e.g. multiple SDKs or multiple Newtonsoft.Json versions).

### General Components

The VMR contains 5 primary components:
- **Engineering and infrastructure sources** - These sources cover the infrastructure tooling for the VMR, both in general, org-agnostic cases as well as org-specific functionality. Examples:
    - **Organization agnostic** - General build scripting and root projects (`Build.cmd`, `build.sh`, ..).
    - **Organization specific** - YAML for building the VMR with signing in Microsoft’s AzDO orchestration systems. YAML for building using CircleCI.
- **Documentation** - Documentation covering the VMR repository. Developer guides, etc.
- **Component sources** - Sources for individual component (development) repositories. `aspnetcore`, `NuGet.Client`, `msbuild`.
- **End-to-end tests** - Product scope tests that validate end-to-end scenarios.
- **Reference package sources** - Text-only sources that are required to build the product. For instance, the product requires the netstandard reference libraries. Some .NET distro maintainers cannot obtain these using a pre-built binary package. Instead, they are checked in as IL (viewed as hand-editable), then assembled for use as input to component builds.

#### Component Source Types

There are two types of component sources:
- **.NET** - These are components that are inherently part of the .NET project. Examples include: `dotnet/wpf`, `dotnet/installer`, `dotnet/aspnetcore`, `dotnet/msbuild`.
- **External** - These components are redistributed or referenced by the .NET product but are not developed as part of the .NET project. In a ‘traditional’ non-source build methodology, they would be brought in via binary dependency flow. Build requirements from partners prohibit pre-build binaries, so they are included in the VMR sources instead. Examples include: `Newtonsoft.Json`.

#### Special Note: SDK Band Layout

SDK bands, e.g. 8.0.1xx and 8.0.2xx are laid out under the `src/sdk` directory, with a directory for each specific band that is active for the given branch. In all cases except post-RTM-Band preview SDKs, all active SDKs are laid out in parallel in the VMR. They are not kept in other branches in the VMR, or in a separate VMR. This is for the following reasons:
- **Runtime:SDK is a one:many relationship** - For each .NET distro maintainer, a single runtime is shipped in at least one SDK. Ensuring that the identity of those runtime files is identical between the SDKs is less confusing for SDK consumers.
- **Separate VMRs would require stable binary flow** -  Stable binary flow violates the Unified Build Rule "There may be no pre-release, non-final stable binary flow". This avoids complexity and ensures that our partners do not have to violate their build requirements.
- **We already have multiple versions of the same source in the VMR** - There is precedent for this situation already, as there are occasionally multiple versions of the same repository (e.g. `Newtonsoft.Json`) shipped within the product.
For post-RTM preview SDKs, they shall exist in their expected location in the VMR (e.g. `src/sdk/8.0.2xx`) in a separate branch of the VMR, with the non-SDK elements of the VMR removed. This VMR may only depend on released .NET runtimes. When this SDK goes to RTM, it shall be merged into the parent `MAJOR.MINOR` servicing branch.

### Examples

|  Scenario                                       |  VMR branch            |  SDK directories                      |  Notes                                              |
|-------------------------------------------------|------------------------|---------------------------------------|-----------------------------------------------------|
|  8.0   pre-RTM (development)                    |  main                  |  src/sdk/8.0.1xx                      |                                                     |
|  8.0 Preview 2                                  |  release/8.0-preview2  |  src/sdk/8.0.1xx                      |                                                     |
|  8.0 RTM                                        |  release/8.0           |  src/sdk/8.0.1xx                      |                                                     |
|  8.0.2xx Preview SDK                            |  release/8.0.2xx       |  src/sdk/8.0.2xx                      |  VMR contains SDK-relevant directories only (e.g. runtime directories are removed as the preview SDK typically builds against shipped runtimes |
|  8.0 at   8.0.2xx RTM                           |  release/8.0           |  src/sdk/8.0.1xx<br />src/sdk/8.0.2xx |  release/8.0.2xx VMR integrated into release/8.0    |
|  8.0 at 8.0.3xx RTM (8.0.2xx out of servicing)  |  release/8.0           |  src/sdk/8.0.1xx<br />src/sdk/8.0.3xx |  release/sdk/8.0.2xx deleted                        |

### Layout

| Directory                              | Purpose                                                                                          |
|----------------------------------------|--------------------------------------------------------------------------------------------------|
| `src/source-mappings.json`              | Source subset mappings definition. See [Repository source mappings](#repository-source-mappings) |
| `src/source-manifest.json`              | An always up-to-date list of all original sources, paths where and versions which are synchronized into the VMR (in that given commit). See [Source Manifest](#source-manifest) |
| `src/<repo>/`                           | Product source for `<repo>` (e.g. `src/runtime`). See [Repository source inclusion](#repository-source-inclusion) for what does and does not get included |
| `src/<repo>/<version>`                  | If multiple versions of a repository must be built in the VMR, then subdirectories for each required version are placed under `<repo>`.<br />- `src/Newtonsoft.Json/13.0/`<br />- `src/Newtonsoft.Json/12.0/` |
| `src/sdk/<sdk band>/`                   | Active SDK bands that are not in preview are laid out in parallel, like the layout of development repo versions above. When a band goes out of active servicing, it is removed from the VMR. See SDK Band Layout for more details. |
| <nobr>`src/source-build-reference-packages/`</nobr> | Reference packages required to bootstrap the product build. |
| `eng/`                                  | Top level directory for engineering functionality:<br />- Build scripting (e.g. `Versions.props`) |
| `eng/keys/`                             | Strong name keys |
| `eng/tools`                             | Organization agnostic tooling. E.g.<br />- Tooling for VMR management<br />- Tooling for change tracing |
| `eng/org/<org or orchestration system>` | Organization or orchestration system specific infrastructure for <org>. E.g.<br />- `eng/msft`<br />- `eng/travisci`<br />- `eng/github` |
| `eng/msft/`                             | Microsoft-specific infra<br />- `eng/msft/pipelines` – AzDO Pipelines<br />- `eng/msft/scripts` – Microsoft specific scripts |
| `documentation/`                        | Documentation<br />- Engineering system documentation<br />- Build environment requirements.<br />- VMR architecture info<br />- ... |

Additional rules:
- Repositories shall maintain the original casing of their development repository name.
- Organization specific engineering shall be placed in an organization (or directory indicative of the type of engineering) directory.

### Repository Source Inclusion

A development repository does not need to contribute all its sources to the VMR.
The sources it contributes are defined by the following rules:
- **The repository shall not contribute any sources that would cause VMR input/output flow to continue forever** – The VMR has two-way flow during mainline development.
    - Sources flow into the VMR from development repositories
    - VMR outputs (source build intermediate packages) and any additional source changes flow back out of the VMR and into the development repositories.
      Maestro is used for backflow.
      This means that each time the VMR back flows into the development repositories, the Version.Details.xml file will be updated in the development repository.
      If files that are always changed with VMR backflow also generate forward flow, the VMR flow will cycle forever.
      We wish to avoid this.

        ```mermaid
        flowchart TD
            Step1[VMR Forward Flow]
            Step2[VMR Build]
            Step3[VMR Back Flow]
            Step4[Development Repo Change]

            Step1-->Step2
            Step2-->Step3
            Step3-->Step4
            Step4-. avoid this .->Step1

            linkStyle default fill:none,stroke-width:3px
            linkStyle 3 stroke:red,color:red
        ```
    *<p align="center">Avoiding cyclical flow</p>*

- **The repository only need contribute sources required to build and test the desired VMR outputs**
    A repository doesn’t need to contribute all of its sources (e.g. roslyn might cloak VS IDE sources), but it does need to contribute enough to build the desired outputs of the VMR.
    Tests may also be included in these sources.

## Moving Code and Dependencies between the VMR and Development Repos

Information may move in two directions for a VMR that is still connected to its development repositories:
- **Forward Flow** - Source from mapped files in a development repository to a VMR.
- **Back Flow** - Source, intermediate packages, and product binaries from a component of a VMR to a development repository.

When moving code in either direction, the VMR tooling must take care to:
- Indicate the VMR/development repository commit that represents the latest sync.
- Preserve the desired granularity of commits.
- For each commit moved, preserve the source commit information.
- Add extra information into each commit message in the VMR to indicate its corresponding commit(s) in a development repository, and vice versa
    > Note: Tooling should use such information for information purposes only, since it could be incorrectly populated, formatted, or simply lost by squashes, rewriting of commit metadata, etc.
- Allow for merge conflict resolution.

> Note: .NET 8 version of the VMR will only support automated forward flow. The [back flow process will be manual](#manual-back-flow).

### Forward Flow

During mainline development, most changes occur in development repositories.

> Note: It will be possible to work solely in the VMR and may even be desirable for some changes.

When source code changes in a repository branch that maps onto a VMR, this source needs to flow into the VMR.
This is completed using the following steps:

1. Trigger Update
    - Manual trigger/local update
    - On commit to tracked development repo branch
2. Create patch(es)
    - Create diff(s) from set of target commits to be moved.
    - Modify patch to exclude unmapped sources, deal with renames, etc.
    - Format commit message(s)
3. Apply patch
    - Apply patch to target VMR
4. Open PR
    - N/A for local-only updates
5. Verify, Modify and Approve PR
    - N/A for local-only updates
    - PR validation
    - Resolve conflicts
    - Modify as necessary for breaking changes
6. Merge PR
    - N/A for local-only updates
    - Typically merge commit, not a squash

#### Dealing with Conflicts

In case a file in the VMR changes outside of the *Forward Flow* (e.g., directly in the VMR), it may happen that the synchronization process will fail due to conflicts with changes in the development repository.

In this case, the development repository will cease to synchronize into the VMR without human intervention.
The other repositories will keep synchronizing as they are dealt with separately.
A conflict needs to be dealt with as soon as possible not to fall behind and this should be the main driving metric of the conflict resolution process.

In case of a conflict the tooling should:
- Create a branch off the top of the VMR, synchronize the problematic commit and open a PR using this branch.
- Assign reviewers to the PR – ideally author(s) of the original commit and additionally some group such as QB who will be responsible for driving the resolution.

### Back Flow

Back flow is used to keep development repositories up to date with respect to their dependencies.
This is the replacement for classic dependency flow in .NET 6 and prior releases.

To explain, let’s look at `dotnet/aspnetcore`.
`dotnet/aspnetcore` has dependencies on artifacts that are developed in the `dotnet/runtime` source code.
In .NET 6 and prior, `dotnet/runtime` would produce an official build, and those artifacts would flow from `dotnet/runtime` build to `dotnet/aspnetcore`'s development branch, allowing it to react to breaking changes and take advantage of new APIs.
In Unified Build, the `dotnet/runtime` development repository does not produce any official artifacts.
Instead, its source flows into the VMR via *Forward Flow*, which then produces official artifacts and intermediate packages representing the build outputs of each component.
To code against a newly available feature in the product repository, a development repository must build against intermediate packages that support that feature.
Similarly, if a `dotnet/runtime` breaking change flows into the VMR, reaction in the aspnetcore component must occur in that flow PR.
Then, when those intermediate outputs of `dotnet/runtime` back flow into `dotnet/aspnetcore`, the modified VMR source for the aspnetcore component must be patched onto the development repository.

Back flow is completed using the following steps:

1. Trigger Update
    - Manual trigger/local update
    - On new build of VMR
2. Prepare a diff between the VMR and each development repo
    - Checkout the development repo at revision that matches last forward flow into the VMR
    - Copy the development repo onto VMR (adhere to cloaking rules, resolve source-build patches, ...)
    - If there are any diffs, submit PR to development repo with inverse diff
4. Open PR or update an existing one
    - N/A for local-only updates
5. Verify, Modify and Approve PR
    - N/A for local-only updates
    - PR validation
    - Resolve conflicts
6. Merge PR
    - N/A for local-only updates
    - Typically a squash commit, not a merge

### Automation

The automation of the code flow will largely be active only for mainline development where development repositories are in use.
When the product switches to public servicing, when a VMR is being used for a private full-stack development, or being used for closed-source development, automation will not be used except in cases of repositories that dual insert into Visual Studio. In those cases, repos must maintain an official build for VS insertion, but will insert source into the VMR. They can **choose** to maintain a development repo for servicing workflows, though this is not required.

The automation shall provide the following functionality:
- Automated forward flow from development repositories to the VMR, by opening PRs in the VMR repository with updated sources for review, testing, and merge.
- Automated back flow between a VMR and development repositories, opening PRs in the development repositories with updated sources  and input binaries (intermediates and updated SDKs).
- Ability to configure the frequency of updates.
- Ability to trigger updates to/from the VMR (via command line tooling).
- Provide functionality to pause updates.

In addition to VMR code-flow, we will preserve the already present binary flow to the VMR and to development repositories as necessary.
However, due to rules regarding what is and is not required to be part of the VMR (see [Scope - "Who Participates?"](./Foundational-Concepts.md#scope---who-participates)), it is expected that the number of cases shall be small and focused on constrained leaf scenarios (e.g. `wpf-int`, IBC/PGO data).

> Note: It is important to remember that the product shall not be dependent on orchestration systems like Maestro or Dependabot to be reliably constructable.
> Dependencies should be self-evident and hand-editable. See the following Rules of Unified Build for more information:
> - There may be no pre-release, non-final stable binary flow.
> - The build shall not require any orchestration to build artifacts for a specific platform distribution.
> - Public open-source .NET releases must be buildable by .NET distro maintainers from a single commit in the upstream repository.
