# dotnet-helix-machines Artifact Maintenance

Epic: https://github.com/dotnet/core-eng/issues/14605

The core of this issue is that we don't have a process for updating our helix-machines artifacts on a regular cadence of any kind.
The purpose of this document is to lay out such a process for comment and ridicule.

## Goal
We want to create a process that a team of vendors can execute on a regular cadence to keep the artifacts we install on our machines up to date.
No automation is planned as part of this process.

There are two primary reasons for this work:
1. Security &ndash; keeping old artifacts can lead to potential vulnerabilities as minor version updates frequently contain security updates.
2. Stability &ndash; minor version updates often contain simple bugfixes that may require patching.

## Stakeholders
Primarily the .NET Engineering Services team, but ultimately all customers using dotnet-helix-machines since not updating artifacts on a regular
cadence can lead to security issues, and updating them to versions that have breaking changes can obviously cause issues for them as well.

## Problems
The following problems have been copied directly from the epic for reference.

* For some artifacts, we don't really know what version we get because we're asking a given source (Windows, package manager, etc) for the name directly; this is thus not logged at all.
* Given we copy many installers and packages to our own storage accounts, and these packages may require manual intervention, this copying is sometimes difficult to automate.
* Even when we do know there is a newer version, we often cannot just take major release updates to a component as this would be breaking/require a hefty re-write of the code using this dependency.
* When we can tolerate them, we need a strategy to deal with major version updates, involving making sure a validation plan is in place, and ensuring a process to communicate to product teams to let them know

### Necessary Ongoing Tasks
While automation can be improved to make this better, these sorts of things need to happen on a regular cadence:

* Check for the latest version available (will vary by OS) and compare what version we have.
* Proactively try to adopt newer versions of dependencies before required
* Teach helix machines to have more info about artifacts (such as "where do we get this one?", "reasons to avoid major version upgrades", simple update instructions, etc.). This could be as simple as a
required text file in the artifact directory.

## Proposed Implementation
I think that we should start treating artifacts in a way similar to how we now treat secrets. At a high level, this means:

* We should include metadata on updating them &ndash; every artifact should have an `README.md` document included alongside it (similar to the validation
scripts we require today). Artifacts missing these will fail the build. This document will include detailed information on updating the artifact (e.g. where
the artifact comes from, how to tell if there is an updated version, the major version to pull from, where to upload the artifact, what part of the YAML to update accordingly, etc.).
* The operations vendor team should have a scheduled monthly pass of all artifacts and follow the instructions laid out in the README.md.
* We should add as many assets to Component Governance as possible so that we receive updates on CVEs so we can do an immediate update. This could possibly be done via
an automatically generated cgmanifest.json.

### netcorenativeassets
Currently, most of our artifacts are stored in the netcorenativeassets storage account and downloaded from there. This is a good model from a security
perspective; however, we currently only update those artifacts on-demand. For these artifacts, the vendor will need to check the canonical
source for these artifacts (e.g. python.org for python) for the most recent version of the artifact, upload it to netcorenativeassets, and then create
a PR to dotnet-helix-machines with the update version. **This will only be done once newly released artifacts are mature unless there is a CVE in our
currently-used version**. "Mature" in this case means the artifact has been released for over a month.

### Package managers

The north star here is to eventually pull everything from private Azure DevOps feeds.

#### Pip
Pip should use internal feeds which upstream to public for now. Eventually, we should migrate to Terrapin. Once we're using Terrapin, we should stop verison
pinning as using latest from our private feed will be more than adequate.

#### Linux packages
Currently, the accepted model for Linux packages is to use the standard package manager's repositories. Because we do not ask for specific versions
of these packages, we make the (possibly dangerous) assumption that they are kept up-to-date. Presumably, these may eventually be moved to
internally-controlled package repositories.

### Handling Major Version Updates
As mentioned in the problems section, major version updates are a sticky problem to solve as they potentially have breaking changes for our customers
and thus require manual review. At end-of-life for a major version, we should force an upgrade to a more recent version of the artifact and remove it
from our machines. Otherwise, on requests for major versions, we can deploy them side-by-side with previous versions to reduce pain.

## The Work
In addition to further documenting the process for maintaining artifacts, the primary work for this epic will actually be writing update
docs for all of our currently-existing artifacts.

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cartifact-maintenance-core-eng-14605.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cartifact-maintenance-core-eng-14605.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cartifact-maintenance-core-eng-14605.md)</sub>
<!-- End Generated Content-->
