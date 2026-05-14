# Signing plan

## Goals

- Design a build system which supports our goal of a two hour build time

- Minimize the differences between official and PR builds, thus increasing the reliability of official builds

- Minimize the impact of unreliable systems such as signing

## Overview

Signing is a requirement for artifacts that are officially available for our end customers. It is not required for artifacts that exist solely as an intermediate stage in our official build process. Signing is also not required immediately on artifacts which will eventually be available to customers but instead merely needs to be done before we advertise / push the artifact to a channel that we communicate to customers as a supported channel.

Removing signing as an intermediate step is both important for the throughput of our builds as well as reliability. Presently the time variance of signing for dotnet/runtime is over one and a half hours. That means a normal variance in our signing process guarantees the two hour build goal will not be met. Giving us flexibility in when signing occurs, and mostly making it an asynchronous process, is required for us to be successful.  

Going forward the expectation is that most repositories will remove signing from their official builds. This will be a hard requirement for any repository on the critical build path of .NET. For other repositories it will be strongly recommended but not required.  Signing will be done at the end of the product build instead of during the build pipeline.

Moving signing to a promotion ring will improve reliablity, and improve signing times by allowing us to flow dependencies faster and sign asynchronously from testing.

## Signing in repositories

The responsibility of individual repository builds is to produce artifacts that either ship to customers (shipping repository) or are consumed by other repositories for their own builds (dependency flow repositories).

### Participating repositories

- will not real sign

- will disable signing validation (if they have fully stopped producing signed assemblies)

- will publish a build manifest to Azure Artifacts which includes signing information

- will publish to a different (unsigned) artifact feed

- will change their subscriptions to consume from the unsigned feed

### Signing via promotion

- a repository may choose to promote a build to be signed.

- *will real sign shipping packages, MSI's, debs, rpm's, etc... by extracting the contents, signing binaries, and re-packaging.

- signed builds will be published to the signed feed

- the signing process will use darc to determine what additional assets are required from other builds and need to be signed

- we will not sign NonShipping packages

- we will perform signing validation on promoted builds.

\* *Initial investigation and exploration has shown that it **should** be possible to extract an MSI's contents, sign, and repackage them.  The current plan is contingent upon being able to repackage installers.  We have begun a proof of concept on repackaging MSI's, but that work is still in progress. If that proves not to be possible, then we will scrap this plan and pursue other options (such as a queue/await model) to reduce sign times during official builds.*

## Moving in stages

Repositories may choose to opt in to the signing promotion model.  If a repository does not opt in, they will continue to sign via current methods.

Repositories targeted for signing promotion model are:

- runtime

- winforms

- wpf

- wpf-int

- windowsdesktop

- aspnetcore

- aspnetcore-tooling

- sdk

- core-sdk

Stage 1:

- Switch repos to consume / publish to unsigned feed and publish to signed feed

- Validate repackaging MSI's is viable via a full proof of concept

- Produce a build manifest

- Disable signing validation in repo

Stage 2:

- Enable MSI signing via signing promotion

While, currently, there are less blockers to moving package signing to core-sdk than moving MSI signing, this entire plan is dependent on the ability to repackage / sign MSI's in core-sdk, so moving packages first may result in throw away work.

Stage 3:

- Enable package signing via signing promotion

- Disable publish to signed feeds

- Modify signing validation to support process changes.

  - Enable signing validation in promoted build

Stage 4:

- Move RPM / deb / etc signing into signing promotion

- Enable signing validation of test signed bits in repo builds.

## Notes

- The ability to repack MSI's is the biggest risk to this plan.  If this does not prove to be possible, then we will provide another plan which adjusts accordingly.

- TBD: With respect to signing validation, what does this mean for repos which are producing more than just nupkg's but aren't signing nupkgs?  

  - Do we need to modify signing validation to support validating some (not all) artifacts?

  - Do we just modify people's builds to publish signed artifacts to one Azure DevOps build artifact and unsigned to another?

- Investigation into signing MSI's is underway

- We have not begun exploring signing RPM's, Deb's, etc... yet but it is not expected that is a high risk part of the plan.

- Mac notarization is not yet automated, after it is automated, we should add that process to the signing promotion stage.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProjects%5CSigning%5CSigningPlan.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProjects%5CSigning%5CSigningPlan.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProjects%5CSigning%5CSigningPlan.md)</sub>
<!-- End Generated Content-->
