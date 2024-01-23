# Arcade

## Overview

Arcade is intended to provide well-understood and consistent mechanisms for consuming, updating, and sharing infrastructure across the .NET Core team. For more details about Arcade, please see the [Overview](./Documentation/Overview.md) documentation.

## Build & Test Status

Status of Arcade public CI builds: [![Build Status](https://dev.azure.com/dnceng-public/public/_apis/build/status/dotnet/arcade/arcade-ci?branchName=main)](https://dev.azure.com/dnceng-public/public/_build/latest?definitionId=13&branchName=main)

## Validation & Dependency Flow Status

**[Arcade validation policy and process](Documentation/Validation/Overview.md)**

### Current Version of Arcade in `.NET Eng - Latest`

[Link](https://maestro.dot.net/2/https:%2F%2Fgithub.com%2Fdotnet%2Farcade/latest/graph) to BARViz

### Latest Version of Arcade Being Validated

[Link](https://maestro.dot.net/9/https:%2F%2Fdev.azure.com%2Fdnceng%2Finternal%2F_git%2Fdotnet-arcade/latest/graph) to BARViz. 

### Build Statuses

|Repo Name|Current Build Status|
|---|---|
|Arcade Official Build|[![Build Status](https://dnceng.visualstudio.com/internal/_apis/build/status/dotnet/arcade/arcade-official-ci?branchName=main)](https://dnceng.visualstudio.com/internal/_build/latest?definitionId=6&branchName=main)| 
|Arcade Validation|[![Build Status](https://dnceng.visualstudio.com/internal/_apis/build/status/dotnet/arcade-validation/dotnet-arcade-validation-official?branchName=main)](https://dnceng.visualstudio.com/internal/_build/latest?definitionId=282&branchName=main)|

### Status of Latest Version of Arcade Being Validated

- Please see the [Arcade channel on Teams](https://teams.microsoft.com/l/channel/19%3a1dad2081c8634f34915d88dce6220265%40thread.skype/Arcade?groupId=4d73664c-9f2f-450d-82a5-c2f02756606d&tenantId=72f988bf-86f1-41af-91ab-2d7cd011db47) for the latest status regarding Arcade promotions. 

## Getting Started

Packages are published daily to our tools feed:

> `https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-eng/nuget/v3/index.json`

This feed is browsable from here:

> https://dev.azure.com/dnceng/public/_packaging?_a=feed&feed=dotnet-eng

### Source Code

`git clone https://github.com/dotnet/arcade.git`

### How to use Arcade

Documentation, tutorials, and guides may be found in the [Start Here](Documentation/StartHere.md) index. 

### How to contribute

- [How to contribute to Arcade guide](Documentation/Policy/ArcadeContributorGuidance.md)

- [Pull requests](https://github.com/dotnet/arcade/pulls): [Open](https://github.com/dotnet/arcade/pulls?q=is%3Aopen+is%3Apr)/[Closed](https://github.com/dotnet/arcade/pulls?q=is%3Apr+is%3Aclosed)

- [Issues](https://github.com/dotnet/arcade/issues)

### License

.NET Core (including the Arcade repo) is licensed under the [MIT license](LICENSE.TXT). 
