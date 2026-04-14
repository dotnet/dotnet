# Contributing to opentelemetry-dotnet-contrib

The OpenTelemetry .NET special interest group (SIG) meets regularly. See the
OpenTelemetry [community](https://github.com/open-telemetry/community#net-sdk)
repo for information on this and other language SIGs.

See the [public meeting
notes](https://docs.google.com/document/d/1yjjD6aBcLxlRazYrawukDgrhZMObwHARJbB9glWdHj8/edit?usp=sharing)
for a summary description of past meetings. To request edit access, join the
meeting or get in touch on
[Slack](https://cloud-native.slack.com/archives/C01N3BC2W7Q).

Anyone may contribute but there are benefits of being a member of our community.
See the [community membership
document](https://github.com/open-telemetry/community/blob/main/community-membership.md)
on how to become a
[**Member**](https://github.com/open-telemetry/community/blob/main/guides/contributor/membership.md#member),
[**Triager**](https://github.com/open-telemetry/community/blob/main/guides/contributor/membership.md#triager),
[**Approver**](https://github.com/open-telemetry/community/blob/main/guides/contributor/membership.md#approver),
and
[**Maintainer**](https://github.com/open-telemetry/community/blob/main/guides/contributor/membership.md#maintainer).

## Give feedback

We are always looking for your feedback.

You can do this by [submitting a GitHub issue](https://github.com/open-telemetry/opentelemetry-dotnet/issues/new).

You may also prefer writing on [#otel-dotnet Slack channel](https://cloud-native.slack.com/archives/C01N3BC2W7Q).

### Report a bug

Reporting bugs is an important contribution. Please make sure to include:

* Expected and actual behavior;
* OpenTelemetry, OS, and .NET versions you are using;
* Steps to reproduce;
* [Minimal, reproducible example](https://stackoverflow.com/help/minimal-reproducible-example).

### Request a feature

If you would like to work on something that is not listed as an issue
(e.g. a new feature or enhancement) please create an issue and describe your proposal.

## Find a buddy and get started quickly

If you are looking for someone to help you find a starting point and be a
resource for your first contribution, join our Slack channel and find a buddy!

1. Create your [CNCF Slack account](http://slack.cncf.io/) and join the
   [otel-dotnet](https://cloud-native.slack.com/archives/C01N3BC2W7Q) channel.
2. Post in the room with an introduction to yourself, what area you are
   interested in (check issues marked with [help
   wanted](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/labels/help%20wanted)),
   and say you are looking for a buddy. We will match you with someone who has
   experience in that area.

Your OpenTelemetry buddy is your resource to talk to directly on all aspects of
contributing to OpenTelemetry: providing context, reviewing PRs, and helping
those get merged. Buddies will not be available 24/7, but are committed to
responding during their normal working hours.

## Development Environment

You can contribute to this project from a Windows, macOS, or Linux machine.

On all platforms, the minimum requirements are:

* Git client and command line tools
* [.NET SDK (latest stable version)](https://dotnet.microsoft.com/download)

Please note that individual project requirements may vary.

### Linux or MacOS

* Visual Studio for Mac or Visual Studio Code

Mono might be required by your IDE but is not required by this project. This is
because unit tests targeting .NET Framework (`net452`, `net46`, `net461` etc.)
are disabled outside of Windows.

### Windows

* Visual Studio 2022+ or Visual Studio Code
* .NET Framework 4.6.2+

## Public API validation

It is critical to **NOT** make breaking changes to public APIs which have been
released in stable builds. We also strive to keep a minimal public API surface.
This repository is using
[Microsoft.CodeAnalysis.PublicApiAnalyzers](https://github.com/dotnet/roslyn-analyzers/blob/main/src/PublicApiAnalyzers/PublicApiAnalyzers.Help.md)
and [Package
validation](https://learn.microsoft.com/dotnet/fundamentals/apicompat/package-validation/overview)
to validate public APIs.

For details about working with these packages and updating "public API files"
when new APIs are added see: [OpenTelemetry .NET > Contributing to
opentelemetry-dotnet > Public API
validation](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/CONTRIBUTING.md#public-api-validation).

## Pull Requests

### How to create pull requests

Everyone is welcome to contribute code to `opentelemetry-dotnet-contrib` via
GitHub pull requests (PRs).

To create a new PR, fork the project in GitHub and clone the upstream repo:

```sh
git clone https://github.com/open-telemetry/opentelemetry-dotnet-contrib.git
```

Navigate to the repo root:

```sh
cd opentelemetry-dotnet-contrib
```

Add your fork as an origin:

```sh
git remote add fork https://github.com/YOUR_GITHUB_USERNAME/opentelemetry-dotnet-contrib.git
```

Run build:

```sh
dotnet build
```

Run tests:

```sh
dotnet test --no-build
```

If you made changes to the Markdown documents (`*.md` files), install the latest
[`markdownlint-cli`](https://github.com/igorshubovych/markdownlint-cli) and run:

```sh
markdownlint .
```

If you made changes to any YAML files (`*.yaml` or `*.yml` files), install the latest
[`yamllint`](https://github.com/adrienverge/yamllint) and run:

```sh
yamllint --no-warnings .
```

Check out a new branch, make modifications and push the branch to your fork:

```sh
$ git checkout -b feature
# edit files
$ git commit
$ git push fork feature
```

Open a pull request against the main `opentelemetry-dotnet-contrib` repo.

#### Tips and best practices for pull requests

* If the PR is not ready for review, please mark it as
  [`draft`](https://github.blog/2019-02-14-introducing-draft-pull-requests/).
* Make sure CLA is signed and all required CI checks are clear.
* Submit small, focused PRs addressing a single
  concern/issue.
* Make sure the PR title reflects the contribution.
* Write a summary that helps understand the change.
* Include usage examples in the summary, where applicable.
* Include benchmarks (before/after) in the summary, for contributions that are
  performance enhancements.

### How to get pull requests merged

A PR is considered to be **ready to merge** when:

* It has received an approval either from one of the
  [Approvers](https://github.com/open-telemetry/community/blob/master/community-membership.md#approver)
  /
  [Maintainers](https://github.com/open-telemetry/community/blob/master/community-membership.md#maintainer)
  or the respective component owner.
* Major feedback/comments are resolved.
* It has been open for review for at least one working day. This gives people
  reasonable time to review.
  * Trivial change (typo, cosmetic, doc, etc.) doesn't have to wait for one day.
  * Urgent fix can take exception as long as it has been actively communicated.

Any maintainer can merge PRs once they are **ready to merge** however
maintainers might decide to wait on merging changes until there are more
approvals and/or discussion, or based on other factors such as release timing and
risk to users. For example if a stable release is planned and a new change is
introduced adding public API(s) or behavioral changes it might be held until the
next alpha/beta release.

## Release process

For details about the release process and information about how to request a
release for a component in this repository see: [Requesting a
release](./build/RELEASING.md#requesting-a-release).

## Style guide

This project includes a
[`.editorconfig`](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/blob/master/.editorconfig)
file which is supported by all the IDEs/editors mentioned above. It works with
the IDE/editor only and does not affect the actual build of the project.

This repository also includes [stylecop ruleset
files](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/tree/master/build).
These files are used to configure the _StyleCop.Analyzers_ which runs during
build. Breaking the rules will result in a build failure.

## New projects

This repo is a great place to contribute exporters, instrumentation libraries,
resource detectors, samplers, or any other kind of extension/component not
explicitly defined in the OpenTelemetry Specification. Please refer to [this
page](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/docs/trace/extending-the-sdk/README.md#extending-the-opentelemetry-net-sdk)
for help writing your component.

When contributing a new project you are expected to assign either yourself or
someone else who would take ownership of the component you are contributing. The
owner should at least be an [OpenTelemetry
Member](https://github.com/open-telemetry/community/blob/main/guides/contributor/membership.md#member)
and be member of [@open-telemetry/dotnet-contrib-contributors](https://github.com/orgs/open-telemetry/teams/dotnet-contrib-contributors)
to be eligible to assigned as component owner.
This is required to ensure that reviews can be automatically requested from
the owners. Once the owner is identified, please update [component_owners](./.github/component_owners.yml)
file for the new project. The component owner(s) are expected to respond to
issues and review PRs affecting their component.

Although the projects within this repo share some properties and configurations,
they are built and released independently. So if you are creating a new project
within `/src` and corresponding test project within `/test`, here are a few
things you should do to ensure that your project is automatically built and
shipped through CI.

> [!NOTE]
> It is generally helpful to reference a previous pull request when adding a new
  project to the repository. A good example to follow is the pull request which
  added the `OpenTelemetry.OpAmp.Client` project:
  [#2917](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/2917).

* Based on what your project is, you may need to depend on the [OpenTelemetry
  SDK](https://www.nuget.org/packages/OpenTelemetry) or the [OpenTelemetry
  API](https://www.nuget.org/packages/OpenTelemetry.Api) Include the necessary
  package in your project. You can choose the version that you want to depend
  on. Usually, it is a good idea to use the latest stable version. This is defined
  centrally using [Central Package Management](https://learn.microsoft.com/nuget/consume-packages/central-package-management).
  For example:

   ```xml
   <ItemGroup>
       <PackageReference Include="OpenTelemetry" />
   </ItemGroup>
   ```

* If your component relies on new features not yet part of the stable release,
  you can refer to the latest pre-release version and override the version.

   ```xml
   <ItemGroup>
       <PackageReference Include="OpenTelemetry" VersionOverride="..." />
   </ItemGroup>
   ```

* The assembly and nuget versioning is managed through
  [MinVer](https://github.com/adamralph/minver) for all the projects in the
  repo. MinVer will assign the version to your project based on the tag prefix
  specified by you. To ensure your project is versioned appropriately, specify a
  `<MinVerTagPrefix>` property in your project file. If your project is named as
  "OpenTelemetry.Instrumentation.FooBar", the MinVerTagPrefix must be
  "Instrumentation.FooBar-". Example:

   ```xml
   <PropertyGroup>
       <MinVerTagPrefix>Instrumentation.FooBar-</MinVerTagPrefix>
   </PropertyGroup>
   ```

* The public API surface of all packages is analyzed by
  [Microsoft.CodeAnalysis.PublicApiAnalyzers](https://github.com/dotnet/roslyn-analyzers/blob/main/src/PublicApiAnalyzers/Microsoft.CodeAnalysis.PublicApiAnalyzers.md).
  This analyzer requires a specific file structure to store the information
  about public API. See [this
  doc](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/CONTRIBUTING.md#enable-public-api-validation-in-new-projects)
  for help setting up public API analysis.

* Update the [CI
  workflow](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/blob/main/.github/workflows/ci.yml)
  so that it builds your new project and update the [code
  coverage](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/blob/main/.github/codecov.yml)
  definition for your new component (it should match what you define in the CI
  workflow).

* Add your component name to the [issue
  templates](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/tree/main/.github/ISSUE_TEMPLATE/)
  in your PR. The maintainer will help to create a new ["comp:"
  label](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/labels?q=comp%3A)
  once the PR is merged.

* Add a `README.md` file for your project describing how to install and use your
  package. Every project's README file needs to have a link to the Nuget
  package. You can use the below snippet for reference:

  ```md
  [![NuGet version badge](https://img.shields.io/nuget/v/{your_package_name})](https://www.nuget.org/packages/{your_package_name})
  [![NuGet download count badge](https://img.shields.io/nuget/dt/{your_package_name})](https://www.nuget.org/packages/{your_package_name})
  ```

* Add a `CHANGELOG.md` file for your project to track changes made after the
  initial pull request.

### Guidance for components on supporting target frameworks

* SHOULD support the [.NET
  versions](https://github.com/open-telemetry/opentelemetry-dotnet/tree/main#supported-net-versions)
  which are supported by the OpenTelemetry main repo packages.
* MUST document the target frameworks supported in the main README.md file.
* MUST NOT support out-of-support .NET runtimes (e.g.: `.NET Framework 4.5.2`,
  `.NET Core 2.1` etc). CI checks in this repository will not be run against out
  of support versions.
* Whenever a .NET version reaches end of support, components MUST drop support
  for it as well. Note: This change does not require major version bump. For
  reference see
  [this](https://github.com/open-telemetry/opentelemetry-dotnet/pull/3351).
