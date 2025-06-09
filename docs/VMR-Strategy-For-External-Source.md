# The Unified Build Almanac (TUBA) - Strategy for managing external source dependencies

## Purpose

This document serves as a summary of the current situation around consuming of external dependencies needed to source-build the .NET product. Furthermore, the document also proposes and compares several ways of managing external dependencies in the VMR. Finally, it briefly discusses future standardization of how to approach external dependencies across the whole product. 

It is important to note that this document takes on the problem from the point of view of the Source-Build effort and the issues of building the product from source or from the VMR.

## External dependency ingestion approaches

Currently, the individual product repositories contain several dozen external dependencies which are ingested in different ways: 

- **Git submodule (also just “submodule”)** – Git native process where we reference a specific commit of a remote repository and checkout it out as if it had been a full git repository. The host repository builds the submodule as if it was part of its own infrastructure. Submodules may be a part of individual product repositories or in the source-build-externals repo, which gathers together a number of common dependencies and builds them using arcade's infrastructure.
  
  Examples: https://github.com/dotnet/aspnetcore/blob/main/.gitmodules and https://github.com/dotnet/source-build-externals/tree/main/repos
- **Custom fork** – we maintain our own fork which is then included the same way as if it was an individual product repository. 
  
  Example: https://github.com/dotnet/llvm-project 

- **Vendoring** – The process by which a component is integrated as part of another, rather than integrated as a dependency. The bottom line of this approach is that the sources of the external dependency are hard copied into the repository that needs the dependency. 

  Example: https://github.com/dotnet/runtime/tree/main/src/native/external 

## Requirements for external dependencies

To build the product successfully and safely from source (regardless of whether from tarball or the VMR), we have several requirements towards external dependencies: 

1. **Serviceability** – We must be able to apply any patch onto any dependency, behind closed doors, if necessary (to allow security patches). Furthermore, it is expected it might not be possible to upstream all the patches as they might be customizations related to integration of the component into Source-Build and would not be suitable for upstreaming. Additionally, we should have a clear audit trail of changes made. 
2. **Business continuity** – Thinking long-term, we must ensure the ability to build non-current versions of the product for servicing reasons. Even in a case when a third-party dependency is no longer available. 
3. **Source-Buildability** – It must be possible to build each external dependency that is part of the Source-Build from source. 
    1. The source files of the dependency must be stripped from disallowed artifacts (binary files). 
    2. We validate the component builds from source early in the pipeline (ideally at the point of ingestion). 
4. **Secure supply chain** – We should not assume that owners of the dependencies adhere to the same level of due diligence as us. Ultimately, this means we scan the sources of the dependencies for security threats at the point of ingestion. 
5. **Licensing compliance** – We must ensure that third party components are compliant from a licensing perspective. 
6. **Auditability** – For each source-build dependency introduced into the product, we need to keep records of its origin so that origin of the source code can be exactly identified. Additionally, we need to keep track of all changes applied to the original source code by us. 
7. **Frequent synchronization with upstream** – The goal is to stay as close as possible with the dependency’s upstream to ensure the code flows both ways: 
    1. Downstream to consume feature updates, security patches and bug fixes. 
    2. Upstream to honor the OSS playbook to contribute back to the original project. 

Additionally, the following would bring value but were not evaluated as strict requirements: 

1. **Get rid of all Source-Build patches** – It is preferrable to flow all Source-Build patches upstream (either into an individual repo or to an external upstream) as having some custom file transformation as part of the VMR construction is not viable. 

## Meeting the requirements

This section of the document elaborates on how we will approach the problem of getting from where we are into a situation where we comply with the requirements above. We must consider the fact that we are in a situation where we already have several dozen dependencies, and we ingest them using different mechanisms. Further, each situation has a slightly different context and ideal solution: 

- Projects that need a lot of customization work well for us in the form of a custom fork (e.g., LLVM project). 
- Small, stable libraries are ideal for Vendoring
- Frequently updated and well-maintained projects are suitable for ingestion as a submodule directly from the source (e.g., googletest). 

When coming up with a proposal of the policy for external dependency management, we should respect the fact that there might not be a silver bullet solution that fits all scenarios. Instead of forcing everyone onto one way of ingestion of dependencies (e.g., vendoring), we should rather provide tooling and guidance on how to make each approach comply with the requirements above and then mandate it. 

Following subsections explain how we can meet all requirements while letting product repositories use any ingestion approach. 

### Implications for the VMR

Prior to Unified Build, source-build was delivered to .NET partners via a tarball of all sources, rather than a git repository. The VMR brings some problems that were non-existent with the tarball. For instance, the fact that the VMR should be directly source-buildable (no binaries inside, patches applied, …) does not mix well with some types of dependency references. Let us compare a case when the VMR contains git submodules pointing to external (3rd party) repositories with some of the requirements: 

- **Patching submodules (1)** – With the requirement of source-building the VMR directly, there is no place for additional patch application on top of external git submodules. We could apply patches during the final build process, but we have decided this is not viable as it breaks mechanisms such as source-linking. 
- **Business continuity (2)** – One of the drawbacks of git submodules is that the build stops working when the remote repository (or the referenced SHA within the repository) is no longer available. 
- **Submodules with binaries (3a)** – When an individual repository references an external submodule that contains binaries, having the submodule as-is in the VMR presents an issue as the clone (+submodule restore) of the VMR inevitably contains these binaries. 

From the above, it is quite clear that the **VMR should not contain submodules**. If it did, they would have to be our custom forks (just because of 1 & 3a). Mandating forks for all submoduled dependencies would not scale though as we would have to transitively fork all submodules within the submodules. It also does not scale from the maintainability point of view where having a fork is very costly. 

### Vendoring submodules on VMR ingestion

VMR not being able to contain submodules itself does not mean that we need to ban submodules in individual product repositories too. Instead, we can modify the VMR ingestion process (when we pull individual repositories into the VMR) so that it would vendor (inline) submodules into the VMR as a hard copy. This has several implications:
- We can use the same cloaking mechanism, we use for individual repos, for the submodules too. This means we can deal with requirement _(3a)_. 
- We can utilize the Source-Build patches mechanism for the sources of the inlined submodules as well. Further (closed-door) patches can also be done in the VMR, if necessary. This satisfies _(1)_. 
- Having the code in the VMR satisfies also _(2)_ as we have a copy. 
- It should also be trivially easy to support audit trail _(6)_ in the VMR as the VMR always keeps track of the last synced commits from each individual repo. However, we need to ensure this for the vendored code as well. 

### Policies for external dependencies

Considering we have the VMR-lite built, we have satisfied requirements _1-3a_. What is left is _3b-7_. These can be dealt with on the side of the individual repositories based on the type of dependency ingestion used. To achieve this, there will have to be: 
- Tooling created by the .NET Engineering Services team to help manage and maintain the dependencies. This will constitute validations to cover requirements _(3b)_, _(4)_ and _(5)_. 
- Teams owning a dependency will be mandated to comply with a few maintenance rules on top of what they are currently doing – mostly configuring provided tooling to register the dependencies. 
- Guidance for bringing a new dependency explaining ways and their benefits/pitfalls – teams should be able to make an educated decision on which type of ingestion they want. 
- Guidance for switching from one type of ingestion into another (imagine a case when upstream stops accepting updates and we need to switch from an external submodule to a fork). 
- Automation that will identify unrecognized dependencies, which are not properly registered, and bring it to attention (exact method/channel is an implementation detail depending on how we design the dev UX). Note: not all dependencies might be recognizable by automation (e.g., vendoring) so our automation might need to rely on dev input. 

Continuing further, if we consider requirements _(3b)_, _(4)_ and _(5)_, these can all be dealt with if we assure there is some sort of automation run over the ingested sources (regardless of whether they come from a submodule or are vendored). The policies we want to mandate are then obvious. 

#### Present Infrastructure

To police external dependencies, we will utilize infrastructure that is already in place. This constitutes of: 

- **Source-Build build leg** – a build leg that we mandate already for every individual repo, and which runs with every PR and validates source-buildability. 
- **The source-build-externals repository** – This repository contains submodules pointing to external repositories and verifies we can build them from source. The repo also contains patches in case we need to customize these before building. This repo then builds these packages Source-Build and populates the local cache for the individual repos to consume them during their build. 

#### Policy for external submodules

For submodules referencing external repositories, we can leverage the source-build-externals repository and mandate a registration there. We can run appropriate automation as part of this repository’s CI. Further, it should be trivial to add a check to the VMR ingestion process and verify that each resolved external submodule has its counterpart in source-build-externals and police the requirement this way.

#### Policy for non-forked/non-vendored external dependencies

External dependencies that are required for source-build but are not forked or submodules of a product repository should continue to be included as submodules in the source-build-externals repository. This ensures that these dependencies remain source-buildable using arcade's infrastructure and new versions of source-build intermediate packages can flow out to product repos for any required updates.

#### Policy for submodules of forks 

When we own a fork of an external dependency, we will mandate CI to run with the fork and again validate source-buildability, secure supply chain and licensing there. Further, we might mandate source-buildability check within the Source-Build leg as well to assure the integration with the individual repository works. 

#### Policy for vendored dependencies 

For vendored dependencies, we can utilize the Source-Build leg running with the builds of the individual repositories and add whatever validation there. We will have to manually catalogue all vendored dependencies as this might be impossible to do with automation. 

Additionally, to this validation, we also need to set policies for storing vendored metadata so that we can track the origin of the code to satisfy the requirement _(6)_. 

### Dealing with upstream 

The last requirement _(7)_ is about us being able to flow code efficiently between our product and the original external upstream. Whatever technical solution of ingesting the dependency, being able to pull latest updates from and to push changes to the upstream must be an integral part of the solution. 

For submodules, this is baked into the practices of working with these and requires no further engineering support. For vendoring, we will need to standardize how to handle ingestion and what kind of metadata needs to be stored with the vendored code. This is to satisfy _(6)_ but also to be effectively flow code. We will also have to standardize how we log down changes done to the code post-ingestion. Example solution can be storing git patches with the code so that the originally ingested code is available. 