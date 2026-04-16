# OpenTelemetry .NET Contrib

[![Slack](https://img.shields.io/badge/slack-@cncf/otel/dotnet-brightgreen.svg?logo=slack)](https://cloud-native.slack.com/archives/C01N3BC2W7Q)
[![Build](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/actions/workflows/ci.yml)

[![OpenSSF Scorecard](https://api.scorecard.dev/projects/github.com/open-telemetry/opentelemetry-dotnet-contrib/badge)](https://scorecard.dev/viewer/?uri=github.com/open-telemetry/opentelemetry-dotnet-contrib)
[![OpenSSF Best Practices](https://www.bestpractices.dev/projects/10370/badge)](https://www.bestpractices.dev/projects/10370)
[![FOSSA License Status](https://app.fossa.com/api/projects/custom%2B162%2Fgithub.com%2Fopen-telemetry%2Fopentelemetry-dotnet-contrib.svg?type=shield&issueType=license)](https://app.fossa.com/projects/custom%2B162%2Fgithub.com%2Fopen-telemetry%2Fopentelemetry-dotnet-contrib?ref=badge_shield&issueType=license)
[![FOSSA Security Status](https://app.fossa.com/api/projects/custom%2B162%2Fgithub.com%2Fopen-telemetry%2Fopentelemetry-dotnet-contrib.svg?type=shield&issueType=security)](https://app.fossa.com/projects/custom%2B162%2Fgithub.com%2Fopen-telemetry%2Fopentelemetry-dotnet-contrib?ref=badge_shield&issueType=security)

This project is intended to provide helpful libraries and standalone
OpenTelemetry-based utilities that don't fit the express scope of the
[OpenTelemetry .NET](https://github.com/open-telemetry/opentelemetry-dotnet) or
[OpenTelemetry .NET Automatic
Instrumentation](https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation)
projects.

## Contributing

For information on how to contribute, consult [the contributing
guidelines](./CONTRIBUTING.md).

## Project status

This repository is a collection of components maintained by different
authors and groups. As such, components shipped from this repository (via
[NuGet](https://www.nuget.org/)) may be at different stability/maturity levels.
The status for each individual component is mentioned in its respective
`README.md` file and will fall into one of the following categories:

### Development

Component is currently in development and is NOT available on
[NuGet](https://www.nuget.org/).

### Alpha

The component is ready to be used for limited non-critical workloads and the
authors of this component would welcome your feedback. Bugs and performance
problems should be reported, but component owners might not work on them right
away. Components can go through significant breaking changes and there are no
backward compatibility guarantees. Package versions in this status have the
`-alpha` extension (eg: opentelemetry.exporter.abc-1.0.0-alpha.1).

### Beta

Same as Alpha, but comparatively more stable. Package versions in this status
have the `-beta` extension (eg: opentelemetry.exporter.abc-1.0.0-beta.1).

### Release candidate

Component is close to stability. There may be minimal breaking changes between
releases. A component at this stage is expected to have had exposure to
non-critical production workloads already during its **Alpha/Beta** phase(s),
making it suitable for broader usage. Package versions in this status have the
`-rc` extension (eg: opentelemetry.exporter.abc-1.0.0-rc.1).

### Stable

The component is ready for general availability. Bugs and performance problems
should be reported and the component owner(s) SHOULD triage and/or resolve them
in a timely manner. The package versions MUST follow [SemVer
V2](https://semver.org/spec/v2.0.0.html).

## Support

This repository is maintained by [.NET Contrib maintainers](#maintainers) team
and [.NET Contrib approvers](#approvers) who can help with reviews and code
approval. However, as individual components are developed by numerous
contributors, approvers and maintainers are not expected to directly contribute
to every component. The list of owners for each component can be found in
component's `Readme.md` file.

### Triagers

[@open-telemetry/dotnet-contrib-triagers](https://github.com/orgs/open-telemetry/teams/dotnet-contrib-triagers):

* [Martin Thwaites](https://github.com/martinjt), Honeycomb

*Find more about the triager role in [community
repository](https://github.com/open-telemetry/community/blob/main/guides/contributor/membership.md#triager).*

### Approvers

[@open-telemetry/dotnet-contrib-approvers](https://github.com/orgs/open-telemetry/teams/dotnet-contrib-approvers):

* [Mikel Blanchard](https://github.com/CodeBlanch), Microsoft
* [Timothy "Mothra" Lee](https://github.com/TimothyMothra)

*Find more about the approver role in [community
repository](https://github.com/open-telemetry/community/blob/main/guides/contributor/membership.md#approver).*

### Maintainers

[@open-telemetry/dotnet-contrib-maintainers](https://github.com/orgs/open-telemetry/teams/dotnet-contrib-maintainers):

* [Alan West](https://github.com/alanwest), New Relic
* [Martin Costello](https://github.com/martincostello), Grafana Labs
* [Piotr Kie&#x142;kowicz](https://github.com/Kielek), Splunk
* [Rajkumar Rangaraj](https://github.com/rajkumar-rangaraj), Microsoft

*Find more about the maintainer role in [community
repository](https://github.com/open-telemetry/community/blob/main/guides/contributor/membership.md#maintainer).*

### Emeritus Maintainer/Approver/Triager

* [Cijo Thomas](https://github.com/cijothomas)
* [Prashant Srivastava](https://github.com/srprash)
* [Sergey Kanzhelev](https://github.com/SergeyKanzhelev)
* [Utkarsh Umesan Pillai](https://github.com/utpilla)
* [Vishwesh Bankwar](https://github.com/vishweshbankwar)

For more information about the emeritus role, see the [community repository](https://github.com/open-telemetry/community/blob/main/guides/contributor/membership.md#emeritus-maintainerapprovertriager).

Even though, anybody can contribute, there are benefits of being a member of our
community. See to the [community membership
document](https://github.com/open-telemetry/community/blob/master/community-membership.md)
on how to become a
[**Member**](https://github.com/open-telemetry/community/blob/master/community-membership.md#member),
[**Triager**](https://github.com/open-telemetry/community/blob/main/community-membership.md#triager),
[**Approver**](https://github.com/open-telemetry/community/blob/master/community-membership.md#approver),
and
[**Maintainer**](https://github.com/open-telemetry/community/blob/master/community-membership.md#maintainer).

## Releases

For details about upcoming planned releases see:
[Milestones](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/milestones).
The dates and features described in issues and milestones are estimates and
subject to change.

For highlights and announcements for stable releases see the `CHANGELOG.md` file
for individual NuGet packages under the [src](./src) directory.

To access packages, source code, and/or view a list of changes for all
components in a release see:
[Releases](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/releases).

Nightly builds from this repo are published to [MyGet](https://www.myget.org),
and can be installed using the
`https://www.myget.org/F/opentelemetry/api/v3/index.json` source.

## Attestation

Starting with the `1.14.*` releases the DLLs included in the packages pushed to
NuGet are attested using [GitHub Artifact attestations](https://docs.github.com/actions/concepts/security/artifact-attestations).

To verify the attestation of a DLL inside a NuGet package use the [GitHub CLI](https://cli.github.com/).

For example:

```bash
gh attestation verify --owner open-telemetry .\OpenTelemetry.Instrumentation.Http.dll
```

> [!NOTE]
> A successful verification outputs `Verification succeeded!`.

For more verification options please refer to the [`gh attestation verify`
documentation](https://cli.github.com/manual/gh_attestation_verify).

## Thanks to all the people who have contributed

[![contributors](https://contributors-img.web.app/image?repo=open-telemetry/opentelemetry-dotnet-contrib)](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/graphs/contributors)
