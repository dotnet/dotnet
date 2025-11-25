# Defining the Bar for Processes in the VMR Official Build

## Purpose

To establish clear criteria and a decision-making framework for determining which processes should be included in the VMR official build. This ensures that the build is efficient, reliable, and maintains the necessary quality standards for shipping assets, while supporting code flow velocity and minimizing unnecessary delays.

## Principles

- **Shippability** - The offical build must produce assets and information to ship .NET in a confident and compliant manner.
- **Velocity**: The build must meet our SLAs (currently 4 hours unsigned, 7 hours signed).
- **Reliability**: The official build must be reliable, with minimal flakiness or unnecessary failures that could block code flow.

## Description of the Bar

Processes that do not contribute to the essential production or validation of shipping assets, or that introduce unnecessary delays, flakiness, or complexity, should be excluded. The goal is that we should never stage (ready for release) any failed build. *Note that this does not mean that a green build is ready for release. Plenty of additional validation processes may be performed (e.g. VS insertion, CTI signoff etc.)*. Any process excluded from the official build must have a compensating mechanism to ensure its results are still considered before shipping or sign-off.

## Rubric

Evaluate each of the questions in order. You reach the end without answering "yes", then the process might be excluded.

- Is the process required to produce assets necessary for shipping the product or downstream builds? **If so, it should be included.**
- Does the process directly support the integrity and readiness and compliance of shipping assets for release, without putting the official build SLAs or reliability at risk? **If so, it should be included.**
- Even if inclusion puts SLAs at risk, is there no possible alternative or compensating mechanism to ensure the process’s results are considered before shipping or sign-off?  **If so, it should be included.**

## Examples

### Example: Processes That Meet the Bar

- **Source-build CentOS jobs**
  - These jobs produce assets used by distro partners to bootstrap their builds. They can't ship without them.

- **Scenario Tests**
  - Scenario tests aren't required to produce shipping assets.
  - These tests **do** validate the basic quality of the build. A failure means that the build is not shippable. They're also quick and reliable.


### Example: Process That Does Not Meet the Bar

- **Source build License scanning**
  - Does not produce shipping assets
  - Is critical validation, but is very expensive.
  - Can be run async in a separate pipelione
- **SDK Diff testing**
  - Does not produce shipping assets
  - Is necessary validation, but failure does not necessarily mean an unshippable product.
  - Validation can be run as a separate, async pipeline.
- **Mono Source Build Validation Legs**:
  - Mono Source build legs do not produce shipping assets.
  - Mono Source build legs do validate our distro partners' ability to bootstrap on non-MS supported archictures, but the builds can be flaky.
  - There is a possibility of a compensating mechanism, a separate validation job which runs the mono validation.
