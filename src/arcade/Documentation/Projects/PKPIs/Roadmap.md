# PKPI Overview / Roadmap

[PKPI Epic](https://github.com/dotnet/arcade/issues/2778)

- [Scope](#scope)

- [Workstreams](#workstreams)

- [Phase 1 (official build health) roadmap](#phase-1---build-health)

- [Phase 2 (build state) roadmap](#phase-2---build-state)

- [Features](#features)

- [Working groups](#working-groups)

The PKPI roadmap involves accomplishing key deliverables in two separate phases.

In phase 1, we will provide PKPI data about official build health.  Official build health includes indicators about official build health (timing / reliability), and dependency update health (are dependencies flowing? how often? etc...).

In phase 2, we will provide PKPI data about official build state.  Official build state includes indicators about dependency staleness, dependency flow, and subscriptions.

[Phase 1](#phase-1---build-health) completion date: 10/21

[Phase 2](#phase-2---build-state) completion date: [TBD]

## Scope

- Provide visualizations of PKPIs for all B.A.R. Channels with the specific focus on the ".NET 5 Dev Channel"

- Provide per repo insight into official build health.  Overall product health metrics are not specifically a goal of this epic but may become available metrics as a result of achieving goals that are in scope.

- Branch update based dependency flow (maestro updates) is not yet implemented.  We can't plan for dependency flow via branch updates so this roadmap only includes features which are applicable to PR based dependency flow.

- PKPIs are based on data available from the "dnceng" account.  Some repos are still producing official builds and flowing dependencies from "devdiv".  Acquiring data from accounts other than "dnceng" is outside the scope of this work.

## Workstreams

The PKPI epic involves five different workstreams.  A "workstream" identifies a grouping for an area of work which contributes the PKPIs.

See the [ProductKPIs](#../../ProductKPIs.md) document for specific information about the PKPI workstreams.

## Phase 1 - Build health

This phase deals with understanding historical data of official builds to identify trends in build reliability, timing, and dependency update trends.

**`Build health` consists of the `official build` and `dependency updates` workstreams.**

At the end of phase 1, you should be able to answer questions like, "how long is my repo taking to build?", "what is the reliability of my official build?", "how often are dependencies seamlessly updating in my repo?", "how often are dependency updates failing in my repo?", etc...

See [features](#features) for which features are being delivered in phase 1.

### Phase 1 key dates

| Date  | Deliverable | Notes |
| ----- | ----------- | ----- |
| 9/13  | PKPI Roadmap available |
| 9/25  | Checkpoint* for phase 1 | Visualizations for dependency updates should be ready for review |
| 10/10 | LT review** ||
| 10/28 | Address LT review issues |
|       | Complete |

\* Checkpoint - meeting between WG and key stakeholders (Chris Bohm / Matt Mitchell / Jared Parsons) to review progress, acquire feedback, adjust course

\*\* LT review - demonstrate phase 1 deliverables to LTS team (Mark Wilkie, Chris Bohm, Shawn Rothlisberger, etc...)

### Phase 1 Delivered

| Issue | Notes |
| ----- | ----- |
| [Dependency updates that fail to flow seamlessly](https://github.com/dotnet/arcade/issues/4014) | Available in power bi dashboard |
| [Number of dependency updates per given time frame](https://github.com/dotnet/arcade/issues/3907) | Available in power bi dashboard |
| [Official build pass rate](https://github.com/dotnet/arcade/issues/2787) | Available in power bi dashboard |
| [Official build time](https://github.com/dotnet/arcade/issues/2786) | Available in power bi dashboard |
| [Dependency staleness](https://github.com/dotnet/arcade/issues/2782) | Available in barviz |
| [Existence of product dependency cycles](https://github.com/dotnet/arcade/issues/3905) | Available in barviz |

### Phase 1 issues resulting from LT review

These issues are based on feedback from LT review and are in varying states of completion.

| Date  | Deliverable | Notes |
| ----- | ----------- | ----- |
| ~~10/21~~ ~~10/28~~ 11/5 | [Official build time goal lines](https://github.com/dotnet/arcade/issues/4101) | Add ability to define per repo goal lines for build time |
|       | [PKPI documentation](https://github.com/dotnet/arcade/issues/4077) ||
| 10/18 | [Official build time worst case scenario](https://github.com/dotnet/arcade/issues/4103) | done |
| 10/24 | [Update official build time report to use buildchannel insert time as end time](https://github.com/dotnet/arcade/issues/4116) ||

## Phase 2 - Build state

This phase deals with understanding the current state of a repo so that we can identify potential health concerns.  These indicators are genereally interesting to repo owners, but particularly interesting during a release cycle.

**`Build state` consists of the `dependency staleness`, `dependency flow`, and `subscriptions` workstreams.**

See [features](#features) for which features are being delivered in phase 2.

### Phase 2 key dates

| Date  | Deliverable | Notes |
| ----- | ----------- | ----- |
| 11/6  | Checkpoint  | Review phase 2 deliverables with stakeholders |
| 11/14 | LT review   ||

### Phase 2 feature dates

| Date  | Issue | Notes |
| ----- | ----- | ----- |
| Done  | [Dependency staleness](https://github.com/dotnet/arcade/issues/2782) | Available in barviz |
| Done  | [Existence of product dependency cycles](https://github.com/dotnet/arcade/issues/3905) | Available in barviz |
| 10/18 | [Disabled subscriptions](https://github.com/dotnet/arcade/issues/4023) | done |
| 11/19 | [Longest build path visualization](https://github.com/dotnet/arcade/issues/4071) | Michelle McDaniel on FR 11/4-11/15 |
| ~11/1~ ~11/6~ 11/22  | [Split subscriptions view into product and toolset](https://github.com/dotnet/arcade/issues/4138) ||
| ~11/6~ 11/22  | [Superfluous input subscriptions](https://github.com/dotnet/arcade/issues/3906) ||
| ~11/13~ ~11/6~ 11/22 | [Conflicting input subscriptions](https://github.com/dotnet/arcade/issues/2801) ||


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProjects%5CPKPIs%5CRoadmap.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProjects%5CPKPIs%5CRoadmap.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProjects%5CPKPIs%5CRoadmap.md)</sub>
<!-- End Generated Content-->
