# OpenTelemetry .NET Contrib - Code Review Instructions

This file provides guidance for reviewing pull requests and local changes in this
repository. It also helps guide AI coding and review agents to make them more effective.

> [!NOTE]
> The primary audience for this document is for automated pull request reviews agents.

## CHANGELOG

CHANGELOG updates are one of the most frequently corrected review items.

- Every behavioural change (bug fix, new feature, breaking change) must
  have a `CHANGELOG.md` entry in the affected component's `CHANGELOG.md` file.
- New entries go at the end of the `## Unreleased` section, after all
  existing entries.
- The required format is:

  ```markdown
  * Description of the change (sentence case, ends with a period).
    ([#NNNN](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/pull/NNNN))
  ```

- Do not add CHANGELOG entries for purely infrastructure changes (CI
  workflow updates, documentation-only fixes, test changes, etc.).
- If a single PR modifies multiple components, each affected component's
  `CHANGELOG.md` needs its own entry with the same PR link.
- "Breaking change" entries should be prefixed with `**Breaking Change**:`.

## NuGet Package References

Instrumentation packages' source projects must reference only the minimal
required OpenTelemetry packages:

- Source packages (`src/`) should reference `OpenTelemetry.Api` or
- `OpenTelemetry.Api.ProviderBuilderExtensions` **only**. Avoid references to
  `OpenTelemetry` (the SDK) from a library source project when adding or editing
  existing code.
- All package versions are centralized in `Directory.Packages.props`. Flag any
  `Version="..."` attribute on a `<PackageReference>` in a src or test `.csproj`
  (`VersionOverride` is allowed only for intentional version-range pinning
  exceptions and should be rare).
- Never add `<Version>` to a project file; use `Directory.Packages.props`

## Public API Surface

- Any new or modified public type or member must be declared in the component's
  `.publicApi/PublicAPI.Unshipped.txt` file. A build error is raised if this is
  missing.
- Flag public API changes that are not reflected in `PublicAPI.Unshipped.txt`.
- Breaking API changes (removals, signature changes, type moves) are not allowed
  without an explicit maintainer decision and a breaking-change CHANGELOG entry
- For a new component, `PublicAPI.Shipped.txt` should be empty (contains only
  `#nullable enable`) and all initial surface goes in `Unshipped.txt`. Entries
  are moved to `Shipped.txt` automatically during the release process.

## XML Documentation Comments

- Use `<para/>` as the paragraph separator in XML doc comments - not a
  blank line inside the `///` block.
- For `ArgumentException`/`ArgumentNullException`/`ArgumentOutOfRangeException`
  thrown from a property setter, the `paramName` argument should be `nameof(value)`,
  not the property's name.
- When writing XML doc comments for manually authored code, write idiomatic,
  readable English rather than copying specification text verbatim. If the
  underlying spec text is not self-explanatory to a consumer of the library,
  rephrase it. Include links to specification documents where appropriate using
  permalinks (i.e. to tags or git SHAs - avoid branch names like `main`).
- Use `<c>TypeName</c>` for type/member references inline in text when not using
  a `<see cref="..."/>` link.

## String Comparisons and Culture-Sensitive Parsing

The repository enforces these via `build/BannedSymbols.txt` (a Roslyn analyzer
will catch them, but flag them in review too):

- **Do not use** the instance `string.Equals(string)` or
  `string.Equals(string, StringComparison)` methods - use the **static**
  `string.Equals(a, b)` / `string.Equals(a, b, StringComparison)` overloads to
  avoid potential `NullReferenceException`.
- **Do not use** the culture-sensitive `TryParse(string, out T)` or
  `TryParse(ReadOnlySpan<char>, out T)` overloads on numeric/date types - always
  pass `CultureInfo.InvariantCulture` (or `NumberStyles` + `IFormatProvider`).

## Code Correctness

### Infinite loops

`while (true)` loops that do not have a provably finite exit path must include a
safety mechanism - a `maxAttempts` counter, a `Stopwatch`-based timeout, or
similar - and throw an exception if the limit is exceeded. This prevents the
code from hanging forever if an assumption is found to be invalid.

### Error handling in OpenTelemetry components

- For exporters: document (and where possible test) what happens when internal
  buffer or size limits are exceeded.

### TODO comments

Do not leave `// TODO` comments in code under review unless there is a
corresponding tracking issue. The comment should reference the issue number.

## OpenTelemetry Semantic Conventions

[Semantic Conventions](https://github.com/open-telemetry/semantic-conventions)
for OpenTelemetry are definitions for how telemetry should work.

- Sampling-sensitive span attributes (identifiers like `rpc.method`, network
  addresses, request method, etc.) must be set **before** `Activity.Start()` is
  called so that custom samplers can observe them.
- Attribute values must match the type specified in the semantic convention
  specification (e.g. a status code attribute defined as a string must not be
  written as an integer).
- Instrumentation scope must include both a `Version` and a `SchemaUrl`.
- Links to the semantic convention specification should be added where relevant
  using permalinks (i.e. to tags or git SHAs - avoid branch names like `main`).

## Instrumentation Package Conventions

- Instrumentation packages should only add tags/attributes to an activity that
  is **already started** - do not start new activities from within an
  instrumentation library unless that is its explicit purpose.
- When migrating between semantic convention versions, the
  `OTEL_SEMCONV_STABILITY_OPT_IN` environment variable controls opt-in.
  Check that both old and new attribute names are handled correctly for each
  opt-in mode.
- Schema URLs and instrumentation scope versions should be updated when semantic
  convention version support changes.

## Performance

- When making performance optimizations, measure before and after to ensure the
  change has the desired effect rather than making assumptions. The `benchmark.ps1`
  script in the root of the repository can be used to run benchmarks locally to
  consistently compare the impact of a change in a branch against the main branch.
- Any PR that claims a performance improvement should include benchmark results
  (BenchmarkDotNet output is preferred) to substantiate any claims.
- For hot-path string or value formatting, consider returning cached constant
  results for well-known common values before falling back to a runtime
  formatting call.
- Avoid unnecessary allocations in instrumentation code that runs on every
  request.

## Testing

- When using `Assert.NotNull(x)` or similar null-guard assertions, do not add
  the null-forgiving `!` operator on the same value in subsequent assertions
  - the guard already expresses the contract.
- Include the actual value in assertion failure messages where appropriate,
  e.g. `Assert.True(condition, $"Expected ... but got {actualValue}")`.
- Use `[Obsolete("...")]` on test methods that exercise obsolete APIs instead of
  suppressing the compiler warning via `#pragma`.
- When testing a refactoring (e.g. moving types between files), prefer testing
  behavior through the public API rather than testing that internal types still
  exist.
- Where relevant, prefer Verify-style tests that assert the entire output of a
  method rather than asserting individual properties of the output. This ensures
  that the output is correct as a whole and helps guard against unexpected changes
  to output. This is particularly important for serialization related tests.

## Line Length and Readability

- Lines that wrap in the GitHub web view (roughly >120 characters) should be
  broken at a logical boundary. This applies equally to code, XML doc comments,
  and CHANGELOG entries.
- Method signatures with many parameters are easier to read when each parameter
  is on its own line.

## API Design Patterns

- Prefer `ReadOnlySpan<byte>` over `ReadOnlyMemory<byte>` for constructor
  parameters when the data is only needed at construction time and is
  snapshotted internally (avoids a forced heap allocation at the call site).
- Use `left` and `right` as parameter names for `==`/`!=` operator overloads
  and `Equals(T other)` methods.
- On .NET targets (`#if NET`), use `System.HashCode` (the struct API with
  `HashCode.Combine(...)` or `Add`/`ToHashCode`) rather than the manual
  `unchecked { h1 * 31 ^ h2 }` pattern.
- `[Flags]` enums whose name represents a set of flags should have a `Flags`
  suffix in their type name.

## New Components

- A brand-new component must be introduced in at least two PRs: the first PR
  sets up the infrastructure (directory structure, `.csproj`, empty public API
  files, CHANGELOG, README, `.github/component_owners.yml` entry, CI path
  filter, solution file entry) and the second PR adds the implementation.
- Discuss the design, naming, and necessity of new components in a GitHub issue
  **before** opening a PR. New components increase the maintenance burden for all
  maintainers so there must be broad consensus on need before proceeding.
- New packages need `<MinVerTagPrefix>` and `<PackageValidationBaselineVersion>`
  set in the `.csproj` (baseline can be omitted only for brand-new packages that
  have never shipped).

## Pull Request Hygiene

- PRs should be focused and small. Thousands of changed lines in a single PR
  make meaningful review impractical; suggest splitting by concern.
- Do not merge upstream/main into a PR branch repeatedly just to stay current -
  this generates unnecessary notifications for reviewers.
- All contributors must have signed the [EasyCLA](https://easycla.lfx.linuxfoundation.org/)
  agreement before a PR can be merged. Flag any PR where the CLA check has not
  passed. Maintainers will usually not review changes at all before the CLA is signed.

## PowerShell Scripts

- Use `Write-Warning` for non-fatal advisory output, and `Write-Error` (or
  throw) for failures, rather than plain `Write-Host`. This allows callers and
  CI to distinguish severity.
- Cross-platform PowerShell scripts should start with `#!/usr/bin/env pwsh`.
- When new scripts are added, ensure they are executable (`chmod +x script.ps1`)
  so that they can be run on Linux/macOS without requiring `pwsh script.ps1`.
- Aggregate errors and report them at the end of a script (fail-at-end pattern)
  rather than stopping at the first warning, so that all issues are visible in
  one run.

## What NOT to Flag

To maintain a high signal-to-noise ratio, **do not comment on**:

- Code style, formatting, or whitespace that is already enforced by
  `dotnet format` / StyleCop / `.editorconfig` - the CI will catch those
  automatically.
- Renovate-managed dependency updates - those PRs are auto-generated.
- `otelbot` automated PRs (semantic-conventions sync) unless there is an
  obvious functional defect.
- Comments suggesting code that will not compile - the compiler will enforce this
  as part of continuous integration and this will avoid false-positive comments
  where new language features are used that AI models may not yet be aware of.

## Miscellaneous

- When reviewing changes related to OpenTelemetry semantic conventions, refer to
  the official specification repository for specific guidance and provide citations
  to justify suggestions wherever possible. The specification repository is located
  at <https://github.com/open-telemetry/semantic-conventions>. Do not provide
  speculative feedback without referencing the official specification, as it may
  lead to incorrect assumptions or misunderstandings.
- Where possible, use more performant APIs added in newer releases of .NET and use
  pre-processor directives to maintain compatibility with older versions of .NET,
  provided that this does not introduce too much code complexity relative to the
  advantages gained by doing so. For example, Use `FrozenSet<T>` instead of `HashSet<T>`
  when targeting .NET 8 or later if the hash set's values are not changed once created.
- Where appropriate, links to external documentation, such as for Microsoft Learn
  and GitHub, should not include language-specific slugs (e.g. `/en/`, `/en-us/`)
  in the URL path so that documentation is shown to the reader in their preferred
  browser language when available.
