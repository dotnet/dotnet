# About This Project

This project contains the xUnit.net assertion library source code, intended to be used as a Git submodule (or via the `xunit.v3.assert.source` NuGet package).

Code here is built with `netstandard2.0` and `net8.0` within xUnit.net v3. At a minimum the code needs to be able to support `net472` and later for .NET Framework, and `net8.0` and later for .NET. The minimum (and default) C# version is 7.3, unless specific features require targeting later compilers. Additionally, we compile with the full Roslyn analyzer set enabled when building for v3, so you will frequently see conditional code and/or rules being disabled as appropriate. These constraints are supported by the [suggested contribution workflow](#suggested-contribution-workflow), which aims to make it easy to know when you've used unavailable features.

This code includes assertions for immutable collections as well as the `Span` and `Memory` family of types. If you experience compiler errors related to these types, you may need to add references to the following NuGet packages:

```xml
<ItemGroup>
    <PackageReference Include="System.Collections.Immutable" Version="6.0.0" />
    <PackageReference Include="System.Memory" Version="4.5.5" />
</ItemGroup>
```

> _**Note:** If your PR requires a newer target framework or a newer C# language to build, please start a discussion in the related issue(s) before starting any work. PRs that arbitrarily use newer target frameworks and/or newer C# language features will need to be fixed; you may be asked to fix them, or we may fix them for you, or we may decline the PR (at our discretion)._

To open an issue for this project, please visit the [core xUnit.net project issue tracker](https://github.com/xunit/xunit/issues).

## Annotations

Whether you are using this repository via Git submodule or via the [source-based NuGet package](https://www.nuget.org/packages/xunit.assert.source), the following pre-processor directives can be used to influence the code contained in this repository:

### `XUNIT_AOT` (min: C# 13, .NET 9)

Define this compilation symbol to use assertions that are compatible with Native AOT.

_Note: you must add_ `<PublishAot>true</PublishAot>` _to the property group of your project file._

### `XUNIT_NULLABLE` (min: C# 9.0)

Define this compilation symbol to opt-in to support for nullable reference types and to enable the relevant nullability analysis annotations on method signatures.

_Note: you must add_ `<Nullable>enable</Nullable>` _to the property group of your project file._

### `XUNIT_OVERLOAD_RESOLUTION_PRIORITY` (min: C# 13.0)

Define this compilation symbol to opt-in to decorating assertion functions with [`[OverloadResolutionPriority]`](https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.overloadresolutionpriorityattribute) to help the compiler resolve competing ambiguous overloads.

### `XUNIT_POINTERS`

Define this compilation symbol to enable support for assertions related to unsafe pointers.

_Note: you must add_ `<AllowUnsafeBlocks>true</AllowUnsafeBlocks>` _to the property group of your project file._

### `XUNIT_VISIBILITY_INTERNAL`

By default, the `Assert` class has `public` visibility. This is appropriate for the default usage (as a shipped library). If your consumption of `Assert` via source is intended to be local to a single library, you should define `XUNIT_VISIBILITY_INTERNAL` to move the visibility of the `Assert` class to `internal`.

## Suggested Contribution Workflow

The pull request workflow for the assertion library is more complex than a typical single-repository project. The source code for the assertions live in this repository, and the source code for the unit tests live in the main repository: [`xunit/xunit`](https://github.com/xunit/xunit).

This workflow makes it easier to work in your branches as well as ensuring that your PR build has a higher chance of succeeding.

You will need a fork of both `xunit/assert.xunit` (this repository) and `xunit/xunit` (the main repository for xUnit.net). You will also need a local clone of `xunit/xunit`, which is where you will be doing all your work. _You do not need a clone of your `xunit/assert.xunit` fork, because we use Git submodules to bring both repositories together into a single folder._

### Before you start working

1. In a command prompt, from the root of the repository, run:

   * `git submodule update --init` to ensure the Git submodule in `/src/xunit.v3.assert/Asserts` is initialized.
   * `git switch main`
   * `git pull origin --ff-only` to ensure that `main` is up to date.
   * `git remote add fork https://github.com/yourusername/xunit` to point to your fork (update the URL as appropriate).
   * `git fetch fork` to ensure that your `fork` remote is working.
   * `git switch -c my-branch-name` to create a new branch for `xunit/xunit`.

   _Replace `my-branch-name` with whatever branch name you want. We suggest you put the general feature and the `xunit/xunit` issue number into the name, to help you track the work if you're planning to help with multiple issues. An example branch name might be something like `add-support-for-IAsyncEnumerable-2367`._

1. In a command prompt, from `/src/xunit.v3.assert/Asserts`, run:

   * `git switch main`
   * `git pull origin --ff-only` to ensure that `main` is up to date.
   * `git remote add fork https://github.com/yourusername/assert.xunit` to point to your fork (update the URL as appropriate).
   * `git fetch fork` to ensure that your `fork` remote is working.
   * `git switch -c my-branch-name` to create a new branch for `xunit/assert.xunit`.

   _You may use the same branch name that you used above, as these branches are in two different repositories; identical names won't conflict, and may help you keep your work straight if you are working on multiple issues._

### Create the code and test

Open the solution in Visual Studio (or your preferred editor/IDE), and create your changes. The assertion changes will live in `/src/xunit.v3.assert/Asserts` and the tests will live in `/src/xunit.v3.assert.tests/Asserts`. In Visual Studio, the two projects you'll be working in are named `xunit.v3.assert` and `xunit.v3.assert.tests`. (You will see several `xunit.v3.assert.*` projects which ensure that the code you're writing correctly compiles in all the supported scenarios.)

When the changes are complete, you can run `./build` from the root of the repository to run the full test suite that would normally be run by a PR.

### When you're ready to submit the pull requests

1. In a command prompt, from `/src/xunit.v3.assert/Asserts`, run:

   * `git add -A`
   * `git commit`
   * `git push fork my-branch-name`

   _This pushes the branch up to your fork for you to create the PR for `xunit/assert.xunit`. The push message will give you a link (something like `https://github.com/yourusername/assert.xunit/pull/new/my-new-branch`) to start the PR process. You may do that now. We do this folder first, because we need for the source to be pushed to get a commit reference for the next step._

1. In a command prompt, from the root of the repository, run the same three commands:

   * `git add -A`
   * `git commit`
   * `git push fork my-branch-name`

   _Just like the previous steps did, this pushes up your branch for the PR for `xunit/xunit`. Only do this after you have pushed your PR-ready changes for `xunit/assert.xunit`. You may now start the PR process for `xunit/xunit` as well, and it will include the reference to the new assertion code that you've already pushed._

A maintainer will review and merge your PRs, and automatically create equivalent updates to the `v2` branch so that your assertion changes will be made available for any potential future xUnit.net v2.x releases.

_Please remember that all PRs require associated unit tests. You may be asked to write the tests if you create a PR without them. If you're not sure how to test the code in question, please feel free to open the PR and then mention that in the PR description, and someone will help you with this._

# About xUnit.net

xUnit.net is a free, open source, community-focused unit testing tool for C#, F#, and Visual Basic.

xUnit.net works with the [.NET SDK](https://dotnet.microsoft.com/download) command line tools, [Visual Studio](https://visualstudio.microsoft.com/), [Visual Studio Code](https://code.visualstudio.com/), [JetBrains Rider](https://www.jetbrains.com/rider/), [NCrunch](https://www.ncrunch.net/), and any development environment compatible with [Microsoft Testing Platform](https://learn.microsoft.com/dotnet/core/testing/microsoft-testing-platform-intro) (xUnit.net v3) or [VSTest](https://github.com/microsoft/vstest) (all versions of xUnit.net).

xUnit.net is part of the [.NET Foundation](https://www.dotnetfoundation.org/) and operates under their [code of conduct](https://www.dotnetfoundation.org/code-of-conduct). It is licensed under [Apache 2](https://opensource.org/licenses/Apache-2.0) (an OSI approved license). The project is [governed](https://xunit.net/governance) by a Project Lead.

For project documentation, please visit the [xUnit.net project home](https://xunit.net/).

* _New to xUnit.net? Get started with the [.NET SDK](https://xunit.net/docs/getting-started/v3/getting-started)._
* _Need some help building the source? See [BUILDING.md](https://github.com/xunit/xunit/tree/main/BUILDING.md)._
* _Want to contribute to the project? See [CONTRIBUTING.md](https://github.com/xunit/.github/tree/main/CONTRIBUTING.md)._
* _Want to contribute to the assertion library? See the [suggested contribution workflow](https://github.com/xunit/assert.xunit/tree/main/README.md#suggested-contribution-workflow) in the assertion library project, as it is slightly more complex due to code being spread across two GitHub repositories._

[![Powered by NDepend](https://raw.github.com/xunit/media/main/powered-by-ndepend-transparent.png)](http://www.ndepend.com/)

## Latest Builds

|                             | Latest stable                                                                                                                            | Latest CI ([how to use](https://xunit.net/docs/using-ci-builds))                                                                                                                                                              | Build status
| --------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------
| `xunit.v3`                  | [![](https://img.shields.io/nuget/v/xunit.v3.svg?logo=nuget)](https://www.nuget.org/packages/xunit.v3)                                   | [![](https://img.shields.io/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit.v3/latest&logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.v3)                                   | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/xunit/badge%3Fref%3Dmain&amp;label=build)](https://actions-badge.atrox.dev/xunit/xunit/goto?ref=main)
| `xunit`                     | [![](https://img.shields.io/nuget/v/xunit.svg?logo=nuget)](https://www.nuget.org/packages/xunit)                                         | [![](https://img.shields.io/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit/latest&logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit)                                         | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/xunit/badge%3Fref%3Dv2&amp;label=build)](https://actions-badge.atrox.dev/xunit/xunit/goto?ref=v2)
| `xunit.analyzers`           | [![](https://img.shields.io/nuget/v/xunit.analyzers.svg?logo=nuget)](https://www.nuget.org/packages/xunit.analyzers)                     | [![](https://img.shields.io/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit.analyzers/latest&logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.analyzers)                     | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/xunit.analyzers/badge%3Fref%3Dmain&amp;label=build)](https://actions-badge.atrox.dev/xunit/xunit.analyzers/goto?ref=main)
| `xunit.runner.visualstudio` | [![](https://img.shields.io/nuget/v/xunit.runner.visualstudio.svg?logo=nuget)](https://www.nuget.org/packages/xunit.runner.visualstudio) | [![](https://img.shields.io/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit.runner.visualstudio/latest&logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.runner.visualstudio) | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/visualstudio.xunit/badge%3Fref%3Dmain&amp;label=build)](https://actions-badge.atrox.dev/xunit/visualstudio.xunit/goto?ref=main)

*For complete CI package lists, please visit the [feedz.io package search](https://feedz.io/org/xunit/repository/xunit/search). A free login is required.*

## Sponsors

Help support this project by becoming a sponsor through [GitHub Sponsors](https://github.com/sponsors/xunit).
