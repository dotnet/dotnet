---
name: license-scan-diff-investigation
description: >-
  Investigate source-build license scan diffs and "Update Source-Build License
  Scan Baselines and Exclusions" pull requests in
  dotnet/dotnet. USE THIS whenever a user asks to review license baseline
  changes, inspect ScanCode results, add verified free and open-source licenses to
  `s_allowedLicenseExpressions`, choose a scoped exclusion, retain a baseline,
  or identify false-positive detections. Also use it when a source-build license
  scan pipeline fails because a `Licenses.*.json` file changed. DO NOT USE for
  general source-build failures unrelated to licenses, generic legal advice,
  binary/prebuilt detection, or third-party-notice authoring.
---

# License Scan Diff Investigation

Investigate every new or changed license finding before accepting a generated
baseline. A pull request that updates the baseline records what ScanCode
reported. The pull request does not determine whether the finding applies to
the source, uses a free and open-source license, is a false positive, or
indicates a licensing problem.

This workflow is specific to these parts of the VMR license scan:

- Allowed-license policy and scan orchestration:
  `test/Microsoft.DotNet.SourceBuild.Tests/LicenseScanTests.cs`
- Baseline directory:
  `test/Microsoft.DotNet.SourceBuild.Tests/assets/LicenseScanTests`
  It contains `LicenseExclusions.txt` and `Licenses.<target>.json`.
- Pipeline:
  `eng/pipelines/templates/stages/vmr-license-scan.yml`

## Safety and scope

- Do not push commits, update a pull request, or post comments without explicit
  user approval.
- Do not broaden an exclusion merely to make the scan pass.
- Do not accept a baseline change until you examine the raw ScanCode evidence
  and affected source.
- Preserve unrelated user changes in the worktree.

## Collect the starting information

Start from one of:

- A dotnet/dotnet pull request that updates license baselines or exclusions.
- An Azure DevOps `source-build-license-scan` build URL.
- A diff involving `Licenses.<target>.json` or `LicenseExclusions.txt`.

If you start from a pull request, read its title, body, commits, and complete
diff. The pull request body normally contains the Azure DevOps build URL that
produced the proposed files.

## Understand the filtering model

Read the current `LicenseScanTests.cs` and `LicenseScanPolicy.cs` before
classifying findings. The test owns the allowed-license list and filtering
orchestration; `ExclusionsHelper` owns exclusion parsing and matching; and the
license policy owns expression decomposition and path normalization.

The current test evaluates each identifier in a mixed expression independently.
For example, if `mit` is allowed but `unknown-license-reference` represents
non-applicable metadata, exclude only `unknown-license-reference`.

Paths use two different roots:

- Exclusion paths are rooted at the VMR, such as
  `src/runtime/src/tests/GC/Scenarios/GCBench/THIRD-PARTY-NOTICES`.
- Baseline paths are relative to the scan target, such as
  `src/tests/GC/Scenarios/GCBench/THIRD-PARTY-NOTICES`.

## Run the deterministic analyzer

Use the `scripts/LicenseScanAnalyzer.cs` file-based app instead of manually
parsing baseline, exclusion, and allowed-license changes. For a pull request,
identify the changed scan targets, download their CI artifacts, and include all
available ScanCode results in the analyzer invocation:

```powershell
$repo = git rev-parse --show-toplevel
$app = Join-Path $repo `
    '.github\skills\license-scan-diff-investigation\scripts\LicenseScanAnalyzer.cs'
$dotnet = Join-Path $repo 'eng\common\dotnet.ps1'
$appArgs = @(
    '--repository-root', $repo
    '--base-ref', '<base-ref>'
    '--output', '<analysis.json>'
    '--fail-on-issues'
)

# Add these arguments when analyzing a pull request commit and its CI evidence.
$appArgs += @('--to-ref', '<generated-commit>')
$appArgs += @('--scancode', 'roslyn=<roslyn-scancode-results.json>')
$appArgs += @('--scancode', 'runtime=<runtime-scancode-results.json>')

& $dotnet run --file $app @appArgs
```

Keep `--repository-root` pointed at the checkout being analyzed. `--base-ref`
is required. Omit `--to-ref` to compare the base with the working tree; pass the
pull request's original generated commit to inspect that proposal. Repeat
`--scancode <target>=<scancode-results.json>` for each downloaded raw result.
Download each result from the matching
`LicenseScan <target>_BuildLogs_Attempt<N>` artifact as described in
[Locate the failing scan jobs](#2-locate-the-failing-scan-jobs); use the
baseline target name on the left side of `=`.
The app infers `src/<repo>` for normal targets and
`src/<repo>/src/<subdirectory>` for split targets; use
`--scan-root <target>=<vmr-path>` only to override that layout.
`--fail-on-issues` makes mechanical consistency issues produce exit code 2.
The Arcade `eng\common\dotnet.ps1` wrapper installs the SDK requested by
`global.json` into the repository's `.dotnet` directory when necessary, then
runs the app with that SDK.

The JSON output contains:

- Allowed-license additions and removals, including adjacent references.
- Parsed exclusion additions and removals.
- Added, removed, and changed baseline findings.
- Each expression split using the shared `LicenseScanPolicy` rules.
- Allowed and remaining-disallowed identifiers for each finding.
- Baseline-relative and VMR-rooted paths.
- Matching literal or globbed exclusions and summarized raw ScanCode matches.
- Mechanical consistency issues, such as a baseline finding that is fully
  allowed or fully excluded, or a scoped exclusion whose identifier does not
  occur in the supplied ScanCode result.

The app does not decide whether matched text applies to source or whether a
license is genuinely free and open source. Make those decisions from the source
context and authoritative license evidence.

## Investigation workflow

### 1. Establish the exact generated delta

Read the pull request diff and list the changed baseline targets, exclusions,
and allowed-license entries. Use the target names to locate the corresponding
scan jobs and artifacts.

If later commits replaced the pull request's original diff, pass the original
generated commit to `--to-ref`. Investigate the generated proposal as well as
the corrected final state.

### 2. Locate the failing scan jobs

Use Azure DevOps test and timeline data to find the `LicenseScan <target>` jobs
whose baseline comparisons failed. Test-run summaries can report
`failedTests: 0` because Azure DevOps stores individual outcomes separately and
some clients render a missing aggregate field as zero. If `passedTests` is less
than `totalTests`, query the individual results instead of trusting the
aggregate count.

Each scan job publishes an artifact named like:

```text
LicenseScan <target>_BuildLogs_Attempt<N>
```

The useful files are under copied `artifacts/TestResults/` paths:

```text
scancode-results.json
UpdatedLicenses.<target>.json
UpdatedLicenseExclusions.<target>.txt
```

Use the latest relevant attempt. Download only the necessary artifacts. If
authentication prevents access, report the blocked evidence explicitly. Do not
infer raw ScanCode matches from the generated baseline.

For an internal build, download one artifact with:

```powershell
az pipelines runs artifact download `
    --organization https://dev.azure.com/dnceng `
    --project internal `
    --run-id <build-id> `
    --artifact-name 'LicenseScan <target>_BuildLogs_Attempt<N>' `
    --path <destination>
```

### 3. Analyze the diff and raw ScanCode evidence

Run the analyzer with each target's `scancode-results.json`. Use the
`baseline_findings`, `exclusion_delta`, and `allowed_license_delta` as the
investigation inventory. Use the
`scancode_evidence` attached to every changed finding: it includes the detected
expression, individual matches, rule identifier, matcher, score, line range,
and matched text when ScanCode emits it.

Do not stop at the normalized expression. `unknown-license-reference`,
`proprietary-license`, and similar identifiers require source context to
distinguish a real license from a placeholder, test fixture, package metadata,
or incidental prose. If the result omits `matched_text`, use its line range to
read the exact scanned source.

### 4. Inspect the source in context

Read the exact source version scanned by the build when possible. Examine the
matched lines and enough surrounding context to determine what the text applies
to.

Common non-applicable cases include:

- Template placeholders such as `LICENSE_URL_HERE_OR_DELETE_THIS_LINE`.
- Test data that intentionally contains license-like text.
- A lockfile's metadata that describes dependency licenses rather than
  licensing the lockfile itself.
- Installer EULAs or packaged license assets that do not license source code.
- Documentation or third-party-notice text that mentions a license without
  applying it to the file.

Do not classify solely from the file extension. A `.json`, `.csproj`, or
`.rtf` file can contain applicable licensing terms or non-applicable metadata.

### 5. Verify the license

If a finding names a real license, verify its terms using authoritative sources:

- The license text in the affected source or dependency.
- SPDX or OSI records for standard licenses.
- The upstream project's repository and package metadata.
- ScanCode's license or rule definition for tool-specific identifiers.

Distinguish "free to use" from "free and open source." A license may permit use
without royalties while restricting modification or redistribution. Record
those restrictions instead of calling the license free and open source.

For compound expressions, classify every identifier. An allowed identifier
needs no new baseline or exclusion merely because it appears beside a noisy
identifier.

### 6. Allow verified free and open-source licenses

Treat a genuine free and open-source license as a global policy decision, not a
file-specific exception. If its ScanCode identifier is absent from
`s_allowedLicenseExpressions`:

1. Verify the terms through an authoritative source such as OSI, SPDX, or the
   license publisher. Confirm that the license is free and open source.
2. Confirm that the ScanCode identifier refers to those terms rather than an
   ambiguous family, proprietary variant, exception, or unknown detection.
3. Add the exact identifier to `s_allowedLicenseExpressions` in
   `LicenseScanTests.cs`. Follow the existing ordering and comment style.
4. Add an authoritative URL in the adjacent comment.
5. Remove generated baseline entries whose only remaining identifier is the
   newly allowed license.

This filtering is intentionally global. After the project accepts a free and
open-source license, future occurrences should not require individual baselines
or exclusions.

Do not add an identifier merely because a license is royalty-free,
source-available, or permits use. Restrictions on modification or
redistribution can make it non-open-source even when no fee is required.

## Decision hierarchy

Choose the first applicable resolution:

1. **Already allowed open-source license:** Make no change.
2. **Verified free and open-source license missing from the allowed list:** Add
   its exact ScanCode identifier to `s_allowedLicenseExpressions` with an
   authoritative reference. Remove affected generated baseline entries.
3. **File category that should never be scanned:** Add a narrowly justified
   ignored file pattern when a general pattern identifies the category safely.
4. **False positive or non-applicable text:** Add a scoped exclusion for test
   or configuration data, package metadata, installer-only assets, and similar
   cases.
5. **Intentional reportable finding:** Keep or add a baseline entry when the
   finding is neither globally allowed nor non-applicable.
6. **Applicable disallowed license:** Fix or remove the content. Do not hide
   the finding with a baseline or exclusion.

A baseline records an intentional reportable finding; it does not approve the
license. If every generated finding is allowed or excluded correctly, no
generated baseline addition should remain.

### Prefer license-scoped exclusions

Use:

```text
<vmr-path>|<license-identifier>
```

when only one detected identifier is false or non-applicable. Exclude the
entire path only when every possible license finding in the file is
non-applicable.

For example, a lockfile expression might contain several allowed dependency
licenses and one `unknown-license-reference` caused by a package's license URL.
Excluding only `unknown-license-reference` preserves detection of future
unexpected licenses in the same file.

## Applying corrections

If the user asks for edits, apply the resolution selected from the decision
hierarchy:

1. For an accepted free and open-source license, add its identifier and
   authoritative reference to `s_allowedLicenseExpressions`.
2. For a false positive or non-applicable finding, add a scoped entry and
   explanatory comment to the appropriate section of
   `LicenseExclusions.txt`.
3. For an intentional reportable finding, retain or add only its baseline entry.
4. For an applicable disallowed license, fix or remove the affected content
   instead of suppressing the finding.
5. Remove generated baseline entries that the selected resolution filters out.
6. If a new baseline file would contain only an empty `files` array, delete it
   when no baseline existed on the base branch.
7. Preserve unrelated baseline entries and formatting.
8. Compare the final diff with the base branch, not only with the original
   generated commit.
9. Rerun the analyzer against the working tree with `--fail-on-issues`.

Do not leave the same finding in both the exclusion file and a baseline. After
filtering, an excluded finding should no longer appear in the baseline.

## Validation

Use the analyzer's output to verify the mechanical invariants:

- No fully allowed or fully excluded finding remains newly baselined.
- Added exclusion paths are VMR-rooted.
- Newly allowed identifiers have adjacent URL comments.
- Expected baseline, exclusion, and allowed-license deltas match the final diff.

Then perform the judgment-based checks the app cannot make:

- Every generated finding has an evidence-backed classification.
- Scoped exclusions match the actual ScanCode identifier and source context.
- Newly allowed licenses are genuinely free and open source, and their
  identifiers match the verified terms.
- Applicable disallowed licenses are remediated rather than suppressed.

If the full license scan cannot run locally, state that limitation. Validation
against raw CI results, current filtering code, exact source matches, and the
resulting diff is valuable, but it is not equivalent to rerunning ScanCode.

## Report format

Lead with whether the generated baseline diff is correct. Then use:

| Finding | Evidence | Classification | Resolution |
|---|---|---|---|
| `<path>` | `<evidence>` | `<class>` | `<resolution>` |

Use one of these classifications: already allowed open-source license, new free
and open-source license, false positive, non-applicable, legitimate baseline,
or disallowed.

After the table, report:

- **Baseline outcome:** Which additions remain, are removed, or require remediation.
- **Changes:** Files edited locally, if any.
- **Validation limits:** Missing artifacts, inaccessible source versions, or
  scans not rerun.
- **Confidence:** A numeric confidence such as `high (9/10)` and the evidence
  that supports it.

Keep claims finding-specific. If evidence is incomplete, lower confidence and
say exactly what is missing.
