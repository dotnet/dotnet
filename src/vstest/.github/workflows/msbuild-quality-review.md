---
name: MSBuild Quality Review
description: >-
  Scheduled scan of `.props`, `.targets`, `Directory.Build.*`,
  `Directory.Packages.props`, and NuGet `build/`, `buildTransitive/`,
  `buildMultiTargeting/` extensions for authoring anti-patterns, correctness
  issues, and adherence to canonical patterns. Delegates to the
  `msbuild-reviewer` agent and posts an issue (or a draft PR for safe fixes).

on:
  schedule: weekly
  workflow_dispatch:

permissions:
  contents: read
  issues: read
  pull-requests: read
  copilot-requests: write

imports:
  - shared/msbuild-review-shared.md

timeout-minutes: 30

network:
  allowed:
    - defaults
    - dotnet
---

<!--
  Body provided by shared/msbuild-review-shared.md.

  Full review rules and operating modes live in the reusable agent at
  .github/agents/msbuild-reviewer.agent.md.
-->
