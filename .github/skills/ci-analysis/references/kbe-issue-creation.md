# Creating Known Build Error Issues

When Build Analysis shows **unmatched failures** (check is red) and investigation confirms the failure is not PR-specific, file a Known Build Error (KBE) issue so Build Analysis can track it across all affected builds.

## When to File

File a KBE issue when **all** of these are true:

1. The failure is **not caused by the PR's changes** (verified via target-branch comparison or PR correlation)
2. The failure is **not already tracked** — search first (see [Duplicate Check](#duplicate-check))
3. The failure **affects or could affect multiple builds** (not a one-off environment glitch)
4. Build Analysis check is **red** (at least one failure is unmatched)

Do **not** file a KBE issue when:
- The failure is clearly caused by the PR's code changes (`LIKELY_PR_RELATED`)
- An existing KBE issue already covers the error pattern
- The failure occurred once and is not reproducible

## Infrastructure vs Repository Issue

| Type | When | Where to file | Who investigates |
|------|------|---------------|------------------|
| **Infrastructure** | Not repo-specific; affects multiple repos; needs engineering services | [dotnet/dnceng](https://github.com/dotnet/dnceng) | @dotnet/dnceng |
| **Repository** | Repo-specific; should be investigated by repo owners | The affected repo (e.g., `dotnet/runtime`) | Repo owners |

**Examples:**
- Agent connection timeout, Helix machine pool issues, NuGet feed outage → **Infrastructure**
- Test `FileSystemWatcher.Tests` crashes on linux-x64 in dotnet/runtime → **Repository**

## Duplicate Check

Before filing, search for existing KBE issues:

```bash
# Search in the affected repo
gh issue list --repo dotnet/<REPO> --label "Known Build Error" --state open --search "<error-text>"

# Search infrastructure issues
gh issue list --repo dotnet/dnceng --label "Known Build Error" --state open --search "<error-text>"

# Browse all active KBE issues
# https://github.com/orgs/dotnet/projects/111
```

Also check the `knownIssues` array from the `[CI_ANALYSIS_SUMMARY]` JSON — Build Analysis may have already matched the failure to an existing issue.

## Issue Template

The issue body must contain a `## Build Information` section, a `## Error Details` section with the full exception/stack trace, and a `## Error Message` section with a JSON blob that Build Analysis parses for matching.

````markdown
## Build Information
Build: <!-- Link to the AzDO build with the error -->
Leg Name: <!-- Name of the failing job/leg -->

## Error Details
<!-- Paste the full stack trace or exception output below so readers can understand the failure at a glance.
     This section is for humans — Build Analysis only parses the ## Error Message section. -->
```
<full exception / stack trace here>
```

## Error Message
<!-- The JSON blob below is parsed by Build Analysis for automatic matching -->
```json
{
    "ErrorMessage": "",
    "BuildRetry": false,
    "ErrorPattern": "",
    "ExcludeConsoleLog": false
}
```
````

**Required label:** `Known Build Error`

> If the label doesn't exist in the target repo, the repo needs to be onboarded to Build Analysis. See [How to get onboard](https://github.com/dotnet/arcade/blob/main/Documentation/Projects/Build%20Analysis/KnownIssues.md#how-to-get-onboard).

## Filling Out the JSON Blob

Use **either** `ErrorMessage` (string matching) **or** `ErrorPattern` (regex matching) — not both. Remove the unused field or leave it empty.

### String Matching (`ErrorMessage`)

Build Analysis uses `String.Contains` (case-sensitive, ordinal) to match each log line against the value.

**Rules for choosing the error string:**
1. Must match at least one line in the build/test error output
2. Strip unique identifiers: machine names, paths, timestamps, GUIDs, port numbers
3. **Be as specific as possible** — the pattern must match every occurrence of *this particular failure* but **never** match unrelated failures. Don't worry about pattern length; specificity matters more than brevity
4. **Prefer multi-line patterns** (array syntax) when a single line could plausibly match unrelated errors. Include the test name, exception type, and key assertion details as separate lines
5. Only use a short single-line pattern when it is genuinely unique to this failure (e.g., a specific telemetry tag + unique error text)

**Example — single-line is sufficient** (the telemetry tag + specific package name makes it unique):
```
##[error].dotnet/sdk/6.0.100-rc.1.21411.28/NuGet.RestoreEx.targets(19,5): error : (NETCORE_ENGINEERING_TELEMETRY=Restore) Failed to retrieve information about 'Microsoft.Extensions.Hosting.WindowsServices'
```

```json
{
    "ErrorMessage": "(NETCORE_ENGINEERING_TELEMETRY=Restore) Failed to retrieve information about 'Microsoft.Extensions.Hosting.WindowsServices'"
}
```

**Example — multi-line needed** (a generic assertion like `Assert.True() Failure` alone would match many unrelated tests):
```
Failed test: System.Net.Http.Tests.HttpClientHandlerTest.GetAsync_UnknownHost_Throws
   System.Net.Http.HttpRequestException : Name or service not known (nonexistent.example.com:443)
   Assert.True() Failure
   Expected: True
   Actual:   False
```

Use multi-line to pin down the specific test and exception:
```json
{
    "ErrorMessage": [
        "System.Net.Http.Tests.HttpClientHandlerTest.GetAsync_UnknownHost_Throws",
        "System.Net.Http.HttpRequestException : Name or service not known"
    ]
}
```

> ⚠️ **Each array element must match a different log line.** If two substrings appear on the same line, combine them into one element.

### Regex Matching (`ErrorPattern`)

Build Analysis uses `Regex` with options: **Singleline**, **IgnoreCase**, **NonBacktracking**, and a 50ms timeout per line.

The same specificity rules apply: **be as specific as possible**. Use multi-line array syntax if needed to avoid matching unrelated failures.

```json
{
    "ErrorPattern": "The command .+ failed"
}
```

Test your regex at a site like regex101.com with **.NET (C#)** flavor, options: single line, insensitive, no backtracking.

### Multi-Line Matching (Array Syntax)

Both `ErrorMessage` and `ErrorPattern` accept an **array of strings** for matching multiple lines in order (AND condition, positional):

```json
{
    "ErrorMessage": ["Assert.True() Failure", "Actual:   False"]
}
```

**How it works:**
- Each string matches against a single line (not multi-line)
- The first string is searched first; if it matches, the second is searched in subsequent lines
- **All** strings must match, in order, for the issue to match
- Lines between matches are allowed (they don't need to be consecutive)

Do **not** mix `ErrorMessage` and `ErrorPattern` in the same array.

### `BuildRetry`

Set to `true` when the failure is transient and a retry is likely to succeed (e.g., agent connection timeouts, intermittent network failures). Build Analysis will automatically retry the build on first occurrence.

Limitations: only retries on the **first** attempt. If the failure recurs on retry, no further retries are attempted.

### `ExcludeConsoleLog`

Set to `true` to exclude Helix console logs from the matching scope. Use when the error pattern could falsely match console log output that isn't the actual error.

## JSON Escaping

Special characters in `ErrorMessage`/`ErrorPattern` values must be JSON-escaped:
- `"` → `\"`
- `\` → `\\`
- Newlines → `\n`

Use GitHub's issue preview tab to validate — invalid JSON is highlighted.

For regex patterns, remember **double escaping**: a literal dot needs `\\.` in the JSON value (the JSON parser consumes one backslash, leaving `\.` for the regex engine).

## Validating Patterns Before Filing (MANDATORY)

> 🚨 **This step is NOT optional.** You MUST validate every pattern against the actual failure log before drafting a KBE issue. Patterns that appear correct frequently fail due to invisible characters, inconsistent whitespace, log line prefixes, or timestamp formatting. Never draft a `gh issue create` command without a `RESULT: PASS` from this script.

**Steps:**
1. Download or extract the failure log (from Helix console log, AzDO build log, or script output saved to a file)
2. Run the validation script with the candidate pattern and log file
3. If `RESULT: FAIL` — refine the pattern and re-test (the script shows what didn't match)
4. If `RESULT: PASS` — use the script's "Validated JSON blob" and "Issue body template" output directly

```powershell
# Test string matching against a log file
./scripts/Test-KnownIssuePattern.ps1 -ErrorMessage "Failed to retrieve information" -LogFile ./failure.log

# Test regex matching
./scripts/Test-KnownIssuePattern.ps1 -ErrorPattern "The command .+ failed" -LogContent "The command 'dotnet build' failed with exit code 1"

# Test multi-line matching
./scripts/Test-KnownIssuePattern.ps1 -ErrorMessage "Assert.True() Failure","Actual:   False" -LogFile ./test-output.log

# Outputs: PASS/FAIL, matched lines with line numbers, and validated JSON blob
```

The script uses the **same matching logic** as Build Analysis (case-sensitive `String.Contains` for `ErrorMessage`; `Regex` with `Singleline | IgnoreCase | NonBacktracking` for `ErrorPattern`).

## Filing the Issue

> 🚨 **NEVER run `gh issue create` directly.** Always present the full `gh issue create` command to the user as a draft and **wait for their explicit approval** before executing it. The user must review the title, labels, target repo, and JSON blob before any issue is created on GitHub.

Once the pattern is validated, **present** the following command to the user for review (do NOT execute it):

```bash
# Repository issue
gh issue create --repo dotnet/<REPO> \
  --label "Known Build Error" \
  --title "<Test or build step name>: <brief failure description>" \
  --body "## Build Information
Build: <AzDO build URL>
Leg Name: <failing job name>

## Error Details
\`\`\`
<full stack trace or exception output>
\`\`\`

## Error Message
\`\`\`json
{
    \"ErrorMessage\": \"<validated pattern>\",
    \"BuildRetry\": false
}
\`\`\`"

# Infrastructure issue
gh issue create --repo dotnet/dnceng \
  --label "Known Build Error" \
  --title "<brief infra failure description>" \
  --body "..."
```

## What Happens After Filing

- Build Analysis scans all builds from the **last 24 hours** and matches them against the new issue
- All future failing builds are also matched automatically
- Matches appear in the Build Analysis check on PRs
- The issue is tracked on the [Known Build Errors project board](https://github.com/orgs/dotnet/projects/111)

## Reference

Full upstream documentation: [dotnet/arcade — Known Issues](https://github.com/dotnet/arcade/blob/main/Documentation/Projects/Build%20Analysis/KnownIssues.md)
