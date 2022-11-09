# Design Contract for Searching and Acquiring Build Prerequisites

This document presents the design contract for searching and acquiring build prerequisites. Scope is limited to developer workflow in [CoreFx](https://github.com/dotnet/corefx.git) repository.

A CoreFx developer uses `build.cmd` or `build.sh` to build the repository. Build requires certain tools, for example, CMake. Build should have a specified set of locations to search a tool, and if the tool is not found in any of those locations then, acquire the tool from a specified URL. This document describes the contract between the build and scripts that search and acquire a tool.

####Tool Manifest

Details of each tool required for the build will be in a manifest file `.toolversions` located in the root of the repository. Each tool should have the following details:

 1. Name of the tool
 2. Declared version
 3. Search paths where the tool is likely to be found
 4. Acquire paths from where the tool can be obtained

An example of `.toolversions` is shown below:

----------
![toolversions.](./assets/toolversions.png?raw=true)

----------

####Probing Mechanism

Build will use a probing mechanism to get the required tool. For any tool, probing involves the following three tasks in sequence:

 1. Search the tool requested by the build. Searches the tool in locations specified in `.toolversions` If tool is found then, return the tool path to the build.
 2. If search fails to find the tool then, acquire the tool from the location specified in `.toolversions`
 3. If search and acquire fail then, return an error message to build

####Search

ISearchTool interface provides virtual and abstracts methods that accomplish searching of a tool. Default implementation would search the tool in environment path and a location within the repository specified in`.toolversions`. A tool can override the base, and have its own implementation of search.
Each tool should implement abstract methods.

####Acquire

IAcquireTool interface provides virtual and abstracts methods that accomplish the acquisition of a tool. Default implementation would download the tool from the URL specified in `.toolversions`, and extract the tool to a location within the repository specified in `.toolversions`. A tool can override the base, and have it own implementation of acquisition.
Each tool should implement abstract methods.

####Helpers

Helpers are a set of utility functions.  For example, a function that can parse `.toolversions` and get the declared version of a tool.  Probe, search and acquire scripts will use these functions.

Probe, search and acquire scripts will be in `tools-local` folder in root folder of CoreFx repository.
A short description of each folder under `tools-local` is provided in the table below.

Folder | Description
------ | -----------
unix | Shell scripts that provide default implementation of ISearchTool and IAcquireTool, probe-tool, and helpers
unix/cmake | Shell scripts that override the default implementation, and implement abstract methods for CMake.
unix/clang | Shell scripts that override the default implementation, and implement abstract methods for CMake.

Similar folder structure will be available for Windows PowerShell scripts. `Tools/downloads/<toolName>` in CoreFx repository root will be the default location for downloaded tools.

Official builds will override the default locations where the tool is searched and acquired. These override locations for official builds are specified in a copy (not available in open) of `.toolversions` file. If a path to the override is specified as a command line argument to `build.cmd` or `build.sh` then, config values from `.toolversions` file located in that specified path are used in search and acquire scripts.

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5Ccmake-design.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5Ccmake-design.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5Ccmake-design.md)</sub>
<!-- End Generated Content-->
