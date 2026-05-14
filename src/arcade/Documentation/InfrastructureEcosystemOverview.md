# .NET Core Infrastructure Ecosystem Overview

This document provides an overview of the infrastructure ecosystem for future .NET Core product development.

## Background

.NET Core as a product has been around for over 3 years as of early 2018. The original infrastructure systems were a necessary departure from the .NET Framework infrastructure systems, but have been showing significant weaknesses for years now. This document is intended to provide an overarching overview for the long-term future.  If .NET Core is to have an efficient and scalable development and shipping story, large changes need to be made.  The infrastructure and ecosystem needs to be reworked from the ground up.

.NET Core is a significant departure from traditional Microsoft products in many ways:
- OSS development
- Isolated git repositories
- Cross-platform
- Sub-components are shipped in many different vehicles.
- Even sub-components are an amalgam.
- Multiple shipping products (e.g. runtime vs. SDK)
- Must build from source in certain scenarios.
- Components are developed independently but must quickly integrate.
- Releases happens in diverse environments (e.g. RedHat vs. Microsoft builds)

When .NET Core development was brought up, Microsoft had little experience with this new world.  Most ecosystem and infrastructure decisions were made to solve the problem at hand at the time the problem existed, with little regard to the downstream effects. This approach can be problematic even in the best of situations, but in .NET Core the sheer complexity of the business and product requirements means that decisions made in one area of the infrastructure ecosystem tends to have significant knock-on effects in others. It is difficult to talk about changing one area without assessing how this change will affect the rest of the ecosystem.

### Example: Source Build
A good example of this issue shows up with source build.  The source build is the attempt to build the interconnected product from source using a base toolset on a single machine disconnected from the internet.  Today this is done specifically for RedHat.  

When this scenario was being developed, it was a significant departure from .NET Core’s mode of product construction at the time.  In that model, each component repository contributes to the product by building and publishing independently and pulling new dependencies from other components as desired (effectively on-demand).  Each repository is free to operate its build in whatever way it wants, using widely varied toolsets, different build processes, etc.

The original source build effort was done on an accelerated timeline, which made designing an overarching solution difficult. An attempt was made to augment the builds of the various repositories and then combine them into a single repository using submodules.  Effectively, source build was bolted onto the side of the existing process.  This causes a couple major issues:
- The model of moving forward dependencies between repositories does not match the model used by individual repositories (selective pulling)
- The toolset used by the source build may not match what the repository build in isolation was using at that SHA.

### An Interconnected Ecosystem

The hard part is that an attempt to solve these problems often brings in the need to change other parts of the ecosystem not directly related to source build.  Some examples (not a full list):
- If there is need for a product build that matches the source build, then that may mean the toolsets should be the same between the two scenarios (Build Infrastructure)
- Individual repos now have necessarily less autonomy in choosing their build toolset (Build Infrastructure).
- Because the components of the product are isolated git repositories, flowing a new build toolset dependency is a number of atomic commits that ideally happen within a short period of time.  This will need to be done efficiently and automatically (Dependency Flow Automation).
- If there is a need for a consistent toolset, then that means a need for a new toolset (e.g. new C# compiler for ASPNet) will “pull” the rest of the repositories forward. Until toolset dependency versions match, individual repos cannot pull new versions of other repositories’ assets. (Dependency Flow Automation )
- A .NET Core source build may not have the same asset outputs as a traditional build (e.g. corefx produces .NET Framework binaries).  In a system where each of the repositories contributes to the overall product in a consistent manner, this means that a consistent repo API that takes into account ‘source-build-ability’ is required (Repository API)
- If we change the repository API, then all the automation systems currently using existing APIs (or lack thereof) must also change. (Build/CI Automation)
- If the API becomes consistent, it makes sense to redesign the Build/CI automation to take advantage of this commonality (Build/CI Automation)

## The Repository Dependency Graph
Creating a coherent product or set of products from isolated repos connected requires understanding the repository dependency graph. Simply taking the head of the master branch of each repository, then building and publishing will not always result in a coherent product.  The product is not a collection of repositories viewed at a point in time (as a monolithic repo might be).  Instead, there is a two level versioning scheme that defines the product, expressed as a series of git changes in each repository, some of which alter the inter-repo dependency state.

For example, since corefx has a dependency on coreclr, we can say that in effect a particular SHA in corefx contains the state of coreclr at the SHA that was built into the version of coreclr that corefx references. If we lay out all inter-dependencies of those repositories that make up the product, we create a set of graphs.  These graphs can then be used to determine what makes up .NET Core or subsection of .NET Core at any repository SHA, by walking nodes (connected by version edges) that implicitly reference that SHA.  The graph has the following properties:
- Beginning at a root (e.g. CLI) at a SHA and walking forward on all edges (X depends on Y) will always produce a full coherent product.
- Beginning at a non-root node at a SHA and walking forward (X depends on Y) on any number of edges will always produce a coherent sub-product.
- It is possible to evaluate the graph backwards, effectively asking what SHAs at X depend on a specific SHA at Y.

## Product Business Requirements

The following are presented the high-level requirements for .NET Core. From these follows a set of infrastructure requirements necessary to achieve the product requirements.
- **Must be able to independently develop and ship product components** – Some components of .NET Core are relatively useless outside of the ‘boxed’ product, but many components ship in multiple vehicles.  Thus, product components need to be independently developed.
- **Must be able to rapidly make changes to product components** – Part of the reasoning behind the componentized model of the product is the desire to rapidly iterate on individual product components to improve overall product quality.
- **Must be able to ship product components in multiple vehicles with potentially different shipping processes** – Some product components cannot be exclusively tied to .NET build and shipping processes. E.g. NuGet.client is an integral part of the product, but their “#1” customer is Visual Studio. Some components may not even be controlled by Microsoft (e.g. newtonsoft.json) We must be able to easily work with these components.
- **Must be able to trace what has been shipped or integrated** – We must be able to determine where source code changes/packages/assets flow.  For example, if a bug in corefx is discovered, we must be able to trace where the packages and assets built from that source went, what repositories took in those changes, and what shipping vehicles pulled in those sources.
- **Must be able to rapidly makes changes to the product as a whole** – Because the product is a set of interconnected components (with several layers), it can take commits across several repos to propagate dependencies to have a change present in a product build.  This is a hindrance to development velocity, especially at the end of the product cycle. We must be able to quickly propagate changes from individual repos into the final product.
- **Must be able to service the product** – We must be able to service the product, in both internal and OSS scenarios.
- **Must be able to build the entire product from source for a single platform** – We must be able to produce the binaries required for shipping by external partners (e.g. RedHat) without an internet connection. This product but must be functionally equivalent for the target platform.
- **Must be able to build individual components from source** – Just as we need to be able to build the entire product from source, we also need to be able to build an individual component from source.
- **Must be able to build product components on an OSS developer machine** – Internal and external developers must be able to produce product components functionally identical to the official build lab.
- **Must be able to determine the quality of the product and its components** – We must be able to effectively test the product whole as well as its individual components in isolation.
- **Must be able to determine the provenance of the product and its components** – We must be able to determine the provenance of tools, sources and other dependencies used as part of the product and isolated component builds.

## Infrastructure Ecosystem Requirements

Specific infrastructure ecosystem requirements are implied from the set of product requirements.  The list below identifies the requirements as well as the components of the ecosystem involved in satisfying the requirement.
- **Given an asset that is part of .NET Core, we must be able to determine the SHA and repo at which that asset was produced** – After a product has shipped, it is often necessary to identify where an asset came from for the purposes of servicing or failure repro. Where possible, the SHA and repo that produced the asset should be embedded within the asset.
  - Implements Requirements
    - Must be able to trace what has been shipped or integrated
    - Must be able to service the product
  - Affects Components
    - Repository Tooling
    - Repository Contracts
- **Given a SHA and repository that produced an asset, a functionally identical package should be producible by checking out that SHA and building** - Reproducible builds are important, for servicing and development.
  - Implements Requirements
    - Must be able to service the product
    - Must be able to able to rapidly make changes to individual components. 
  - Affects Components
    - Repository Contracts
- **For a repository, package dependencies should be described such that the package version, the repo and SHA are all specified in source** - Developers must be able to locate exactly what a repository depends on.
  - Implements Requirements
    - Must be able to service the product
    - Must be able to determine the provenance of the product and its components
    - Must be able to build the product from source
  - Affects Components
    - Repository Contracts
- **Given a repository at a SHA, we must be able to gather the transitive set of repositories+SHA combinations that produce the assets referenced by that repository at the specified SHA.** - We must know all sha/repo combinations that are needed to produce a repo's assets, for use in producing a source layout or tracking what commits a SHA contains. 
  - Implements Requirements
    - Must be able to trace what has been shipped or integrated
    - Must be able to service the product
    - Must be able to build the product from source
  - Affects Components
     - Repository Contracts
     - Repository Tooling
- **Given a SHA or asset, we must be able to determine the set of sha/repository combinations that reference that sha/asset** - We must know where an asset has flowed, to determine the state of the product or what assets may needs servicing.
  - Implements Requirements
    - Must be able to trace what has been shipped or integrated
    - Must be able to service the product
  - Affects Components
    - Repository Tooling
    - Repository Contracts
- **Dependencies must be programmatically alterable or queryable without executing any repository specific code** - Operating on metadata alone, we must be able to evaluate the product makeup.
  - Implements Requirements
    - Must be able to build the product from source
  - Affects Components
    - Repository Tooling
    - Repository Contracts
- **Version dependency update must be automated, resilient and configurable by policy** – We must use automated systems to flow dependencies, rather than manual intervention to ensure that product construction is predictable and efficient. 
  - Implements Requirements
    - Must be able to ship product components in multiple vehicles with potentially different shipping processes
    - Must be able to rapidly make changes to product components
    - Must be able to rapidly makes changes to the product as a whole
  - Affects Components
    - CI/Build Automation
    - Repository Tooling
    - Dependency Flow Automation
- **OSS and internal changes can be built and validated with the same infrastructure** - We must be able to work effectively internally and in the OSS world.  We must have support for building, testing and constructing the product in the same way regardless of the internal or OSS nature of the constituent repos. 
  - Implements Requirements
    - Must be able to service the product
    - Must be able to rapidly make changes to product components
  - Affects Components
    - CI/Build Automation
- **Build/validation automation can be generated off repository metadata** – We must utilize config-as-code as much as possible to ensure we can rapidly iterate on process and feel confident that changes to process will not damage serviceability.
  - Implements Requirements
    - Must be able to rapidly make changes to product components
    - Must be able to rapidly makes changes to the product as a whole
    - Must be able to service the product
  - Affects Components
    - CI/Build Automation
    - Dependency Flow Automation
    - Repository Contracts
    - Build System
- **Repositories should separate build, test, package and publish phases of their builds** - Separating out the phases of the build enables better reporting and telemetry, more efficient product construction, and more predictable repository behavior for developers.
  - Implements Requirements
    - Must be able to rapidly makes changes to the product as a whole
    - Must be able to build the product from source
  - Affects Components
    - CI/Build Automation
    - Repository Contracts
    - Build System
- **Packages updates should be able to move quickly through the system** - The repository graph is deep and complex.  Ensuring that changes flow quickly through the graph through version updates is critical to efficient product construction.
  - Implements Requirements
    - Must be able to rapidly makes changes to the product as a whole
  - Affects Components
    - CI/Build Automation
     - Dependency Flow Automation
     - Repository contracts
- **All automation should be instrumented** – Infrastructure systems must be reliable, and instrumentation is also used to ensure that reporting and quality assessments are available for all components.
  - Implements Requirements
    - All
  - Affects Components
    - CI/Build Automation
    - Dependency Flow System
- **Users should be able to ascertain the state of the product across releases/versions, etc.** - To ensure that we can ship and service the product, we must have reporting systems that let us ascertain the state of the product.
  - Implements Requirements
    - Must be able to service the product
  - Affects Components
    - Visualization/Reporting System
- **All official automation should pull inputs from known and trusted locations**
  - Implements Requirements
    - Must be able to determine the provenance of the product and its components.
  - Affects Components
    - Repository Contracts

## System Components
Given the infrastructure ecosystem requirements, this section identifies the set of system components and features that implement those requirements.  The scope of these components is described below, followed by detailed information
- **Repository Contracts** – A set of rules that repositories must adhere to in order to be part of the .NET Core Infrastructure Ecosystem.  There are differing sets of rules depending on the desired/required level of participation in the ecosystem.
- **Build/Test System** – The mechanics of building/testing a repository within a single host environment.  The build system is not concerned with environment procurement, host CI systems, etc.
- **CI/Build Automation** – The set of software and services that automates the procurement of environments, scheduling of CI jobs, feedback to GitHub, etc.  Generally, this is Azure DevOps with a set of additional functionality provided by .NET Core.
- **Repository Tooling** – A set of standalone tooling that is used to interact with the repository.  Examples include tooling to read repository package information, tooling to update repository package info, etc.
- **Repository API** – A set of primitive commands common to each repository used to perform typical repository actions.
- **Dependency Flow Automation** – A set of tools and services used to automate the movement of dependencies among repositories based on configurable policies. Maestro++ implements most of this functionality, utilizing the repository tooling.
- **Visualization/Reporting System** – A set of services used to report the status of the product.  Currently implemented in Azure DevOps Tests.



<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CInfrastructureEcosystemOverview.md)](https://helix.dot.net/f/p/5?p=Documentation%5CInfrastructureEcosystemOverview.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CInfrastructureEcosystemOverview.md)</sub>
<!-- End Generated Content-->
