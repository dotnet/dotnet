# Source Build Orchestration

## Overview

.NET Core contains two logical products: Shared Framework (which is a runtime +
a set of managed libraries) and an SDK (which is managed code running on top of
the shared framework to provide commands like: `dotnet run`, `dotnet restore`,
`dotnet build`, etc.

Each of these products is made up from multiple git projects which operate
independently from one another. We use NuGet packages and zip/tar.gz files to
hand off artifacts across project boundaries.

At a high level, composing these projects into products is done by computing the
dependency relationship between all the projects then building from the bottom
of the dependency tree upwards. As we build, we use the locally built artifacts
(nupkgs and zip/tar.gz files) instead of versions we would obtain from NuGet, MyGet or
Azure Blob Storage to satisfy the needs of projects which depend on a specific
component.

In this way, we may elide building certain projects when building a product (for
example, a CoreCLR developer may want to use a source built version of CoreCLR,
but use the existing set of packages for the rest of .NET Core).

## Bootstrapping

We require an existing .NET Core in order to build .NET Core. This is due to the
dependencies on `dotnet restore` (to consume NuGet packages) as well as
dependencies on MSBuild and Roslyn for compiling the managed code in the
product. We already have a set of scripts which can be used to bootstrap an
existing version of the .NET Core SDK on a new Linux distribution (by rebuilding
the native code from source) which we use when bringing new distributions
online.

Some projects carry their own copies of CoreCLR (instead of obtaining a copy of
the .NET Core SDK and then running on top of that) which is a practice we should
stop. Long term, we need to get to a world where all projects uses a single
shared version of the Shared Framework and SDK when they need to invoke managed
code as part of their build. Ideally, this will always be the last released
version of the .NET Core SDK from the LTS train (e.g. when building 1.0.X and
1.1.X we rely on a pre-existing .NET Core 1.0.0 SDK) so that a source built
version of the previous product can be used without having to do any
bootstrapping.

At the start of a source build, if an existing toolchain is not present, we'll
use the bootstrapping script (Rover) to get a working toolchain and then use
that to start building.

## Expressing dependencies with NuGet packages

We'll continue to use NuGet packages as a way of handing off artifacts between
projects. Note that NuGet packages should **only** be used for cross project
dependencies. When depending on a component built out of the same git
repository, project references should be used instead. The rationale for this
rule is that without it we can't build all of a repositories dependencies from
source, since it depends on a previous version of itself.

We use NuGet packages instead of some other format (e.g. building to a shared
directory with a well known convention for artifacts) because it provides the
most flexibility across projects. For better or worse we understand how to use
NuGet across repositories to manage dependencies.

It is possible that the NuGet packages that we consume as part of a composed
build do not match the NuGet packages we would ship. For example, a NuGet
package may provide both Desktop and .NET Core versions of an asset. A project
may choose not to building Desktop artifacts if just a .NET Core build was
requested. In this case, the repository should produce a "partial package" which
contains only a subset of the assets. This package should however have the same
identity and version as the full package. We have already added support in
BuildTools for projects that wish to do this.

## Building Projects

When the "repo api" is implemented fully across all the projects that make up
.NET Core, we can use it to construct a build graph and start building
dependencies. In the short term, we'll hard code the layering diagram of our
repositories into build scripts themselves.

To begin, we'll start by building the `dotnet/standard` repository to produce
the set of .NET Standard 2.0 reference assemblies. If there are additional sets
of reference assemblies we'll need during the build, we should introduce a
`dotnet/reference-assemblies` style repositories which can built from source a
set of refs that can be used to target other profiles.

After a project has been built we move all of the nupkgs it produced into a
package fallback location and use the normal NuGet APIs to consume them. In
addition, we'll use the repo api "change" command to update the versions that
dependent projects consume.

We continue building projects and updating dependencies until the entire product
has been built.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5Csource-build-orchestration.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5Csource-build-orchestration.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5Csource-build-orchestration.md)</sub>
<!-- End Generated Content-->
