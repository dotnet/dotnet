# The Unified Build Almanac (TUBA) - Terminology

This document contains terminology used in the Unified Build effort.

- **Build** – The processes in which artifacts are produced, given a set of input resources (e.g. source code, packages, input libraries etc.) and an environment (e.g. docker host, VM, local machine).
- **Cloaking** – Process of filtering files when copying/ingesting them into the VMR. This is better described in the document describing the Unified Build (part about source-mapping.json).
- **Development Repository** - A repository which exists only a development area for a section of code. It does not produce official build artifacts.
- **Incoherency** - A state of the traditional product build graph in which multiple versions of the same component are transitively referenced, and where the difference in those versions does or could have meaningful impact on product functionality. For instance, if two `dotnet/runtime` versions are referenced within the same SDK, one may be packaged with the SDK, but the other may have been what `dotnet/aspnetcore` was built against. If there are meaningful differences between those two runtimes, `dotnet/aspnetcore` may not operate as expected.
- **Publishing** – The processes in which artifacts produced by the Build and information about the Build are propagated for consumption outside the build (e.g. publishing packages to a feed).
- **Source-Build** – A set of sources and a process which allows to build the entire product end to end including all its dependencies in offline mode, excluding ambient dependencies.
- **Source-Buildable component** – A component that can be built from source. Binary artifacts such as the build toolset which can also be built from source are allowed. 
- **Source-Build patches** – Set of git patches that are applied on top of the collected individual repositories to remove some problems with the Source-Build. 
- **Upstream (captialized)** - Repositories (especially the VMR), which serve as a common development location and community hub for .NET. For example, https://github.com/dotnet/dotnet. Other repositories are typically downstream of Upstream, and pull new releases and code from Upstream.
- **Vendoring** - The process by which a component is integrated as part of another, rather than integrated as a dependency. See [How to vendor a git into another git by George Shuklin](https://medium.com/opsops/git-vendor-295db4bcec3a) for a good example.
- **VMR** - **V**irtual **M**onolithic **R**epository. A monolithic projection of a set of individual repositories that build together as an aggregate to create a coherent product.
- **VMR-lite** – A first MVP (minimum viable product) version of the full VMR that is read-only for regular developers (no back-flow to individual repositories but supports changes such as closed-door security patches) and supports source-building the product from itself.
