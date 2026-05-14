# .NET 5 Build Shape

This document is intended to cover the desired shape of builds in .NET 5, and what repositories will need to do achieve this.

## .NET 5 Overall Build Goals

### Two-Hour Build

For .NET 5 any change should be able to propagate from any repository, to all places it needs to go, to produce shipping products in 2 hours or less. To do this, we are taking a many-pronged approach to improve today's build times:
- Official builds will not run tests (tests can be run in parallel in a separate CI job)
- Reduce long polls in various repository builds.
- Reduce signing time by (most likely) move signing out of the individual repo builds and into the build promotion pipelines.
- In release channels (primarily when inter-repo interfaces are stable), dependency flow will eliminate PR builds at every step, instead attempting an official build immediately, and merging the update into the mainline if possible.
- Move other tasks non-essential to individual repo builds into the build promotion pipeline (e.g. SDL).
- Use more powerful build machines.
- Consolidate some repositories to reduce the number of build steps.

### A First Class Source-Build Experience

In addition to build times, we will working to ensure that Linux has true first-class support:
- Sustainable builds of .NET Core for inclusion in Linux Distros​
- Source-build is a part of the official build​
- Eliminate a separate “source-build” repo.

While some of these tasks will be largely taken care of by the infrastructure team (e.g. enabling the ability for signing to move out of the builds), we'll need your help for the other tasks.

## Shape Of A Repository's Official Build In .NET 5

Repository builds in .NET 5 must be leaner and more efficient than any of our prior releases. They must build as much in parallel as possible and do no work not essential to the production of outputs necessary to create the product. They must also ensure that source build is treated like a first class citizen rather than a bolt-on. Practically, this means:
- **Official builds should** only create what is necessary for the shipping product or downstream repos (for dependency flow). This includes not building tests.
- **Official builds should not** build or run tests. Tests should be built and run in separate CI jobs.
- **Official builds should not** sign outputs if that repo is part of the "core" .NET stack (tooling repos are excluded as they ship to VS too).
- **Official builds should** have minimal long poles.
- **Official builds should** utilize higher powered machines where possible (when available).
- **Official builds should** build a source build job in parallel with 'traditional' outputs.

## Work Item For Repository Owners

### Two-hour build

- **Remove tests from your official build** - Tests should be run separate CI pipelines that run on each commit or a cadence of your choosing. If you need guidance, contact the engineering services team or take a look at what .NET runtime has done.
- **Work with the engineering team to improve signing** - Signing will be moving out of individual repo builds and into the post-build pipelines. This overall should be relatively painless, may require coordination in some cases.
- **Take regular arcade updates** - We will be steadily improving some of our infrastructure (e.g. moving some validation into the post-build pipelines). Taking regular arcade updates will help propagate those changes more quickly.
- **Analyze build long poles and improve or remove** - The end to end build needs to be < 2 hours. Your build needs to be quite a bit shorter to achieve this. Concrete goals
- **Complete repository consolidations in a timely manner** - Repository consolidation is a large part of the overall build improvement. There are still a few places where consolidation is not yet complete. Every consolidation removes a serial sequence point in the overall build.
- **Ensure your official build has high pass rates with minimal flakiness** - Retries greatly increase the end to end time for shipping the product. High pass rates are necessary for short, predictable, end to end builds.

### First class source-build experience

- **Assist with issues when building 5.0​**
- **Add source-build PR validation to repo based on Arcade .yml template​**
- **Resolve issues when failures occur​**
- **Add official build validation to repo based on Arcade .yml template​**
- **Incorporate source-build patches into source​**
- **Review and approve PRs for incoming source-build implementation​**


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CNet5Builds.md)](https://helix.dot.net/f/p/5?p=Documentation%5CNet5Builds.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CNet5Builds.md)</sub>
<!-- End Generated Content-->
