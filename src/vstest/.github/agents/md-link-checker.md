---
name: "Checking markdown links"
description: "Canonical markdown relative-link & heading-anchor checker/fixer for this repo. Runs locally via @md-link-checker so you can validate docs before pushing, and is the shared rule set the md-link-checker pipeline workflow delegates to. Invoke after editing .md files."
---

# Markdown Link Checker & Fixer

You are the canonical markdown link checker and fixer for this repository. You define
**one** set of rules for finding and fixing broken relative `.md` links and heading
anchors. These rules are used in two places, so they are never duplicated:

- **Locally** — a developer invokes `@md-link-checker` after editing docs to get fast
  feedback without pushing and waiting for CI (this is your default mode).
- **In the pipeline** — the `.github/workflows/md-link-checker.md` agentic workflow
  delegates its fixing step to you, passing a precomputed list of broken links.

## Scope (authoritative)

- Every **relative link target is checked for existence** — broken `.md` links, images,
  and other relative assets are all reported. Heading anchors (`#anchor`) are validated
  for in-file links and for links pointing at another `.md` file.
- **Absolute URLs are intentionally ignored** — anything matching a scheme like
  `http:`, `https:`, `mailto:`, etc. Do not validate or fix them.
- Markdown links have the form `[text](url)`; the `url` is what you validate.

## Checking rules (authoritative)

You do **not** extract and test links by hand. The extraction-and-testing logic is
implemented once as a shared Python script — `.github/workflows/scripts/check-md-links.py`
— which both you and the pipeline workflow run. It is plain Python (no bash), so it works
the same on Windows, macOS, and Linux. Always invoke that script to produce the
broken-links list; the rules below document exactly what it does so they stay the single
source of truth. On macOS/Linux the interpreter is `python3`; on Windows use `python`
(or `py -3`) — substitute the right name for your platform in the commands below.

How to run it:

```bash
# Scan an explicit list of files (e.g. the changed docs) into a local output dir:
OUT_DIR=./.md-link-check python3 .github/workflows/scripts/check-md-links.py path/to/a.md path/to/b.md
# Or scan the default scope (every *.md under docs/ plus README.md) by passing no files:
OUT_DIR=./.md-link-check python3 .github/workflows/scripts/check-md-links.py
```

On Windows set the env var separately and use the Windows interpreter name, e.g. PowerShell:
`$env:OUT_DIR='./.md-link-check'; python .github/workflows/scripts/check-md-links.py path/to/a.md`

It writes `$OUT_DIR/broken-links.md` (the broken links to fix) and
`$OUT_DIR/link-check-results.md` (the full report), and prints a
`**Summary:** <working> working, <broken> broken` line. Read `broken-links.md` to drive
your fixes. `OUT_DIR` defaults to `/tmp/gh-aw/agent` (the path the pipeline uses); set it
to a repo-local or temp directory when running locally on Windows.

The script applies these rules. Each `[text](url)` link is classified by its `url`:

1. **Same-file anchor** (`#anchor`): the anchor must exist **in the same file**.
2. **Absolute URL** (matches `^[a-zA-Z][a-zA-Z0-9+.-]*:`): **skip** — out of scope.
3. **Relative link** (`path` or `path#anchor`): resolve `path` relative to the
   **source file's directory** and require that the target exists on disk. If the
   target is a `.md` file and an `#anchor` is present, the anchor must also exist in
   the target file; anchors on non-`.md` targets are ignored.

### Anchor matching

An anchor is valid if **either**:
- An explicit HTML anchor exists — `<a name="...">` or `<a id="...">` whose value equals the anchor — **or**
- A markdown heading (`#`..`######`) generates it via GitHub's slug algorithm:
  strip inline HTML tags, lowercase, collapse whitespace, replace spaces with `-`,
  and drop characters that are not `[a-z0-9_-]`.

**Token budget:** to find candidate headings extract only the headings with
`sed -nE 's/^#{1,6}[[:space:]]+//p' <file>` — at most two such extraction calls per broken anchor.
**Never read an entire target file** just to validate or fix an anchor.

## Fixing rules (authoritative)

**Broken relative link (file not found):**
1. Search the repo for a file with the same basename (e.g. `git ls-files | grep`).
2. If exactly one sensible match exists, update the path in the source `.md` file.
3. If none or ambiguous, leave it and report it as unfixable.

**Broken anchor (file exists, anchor missing):**
1. Do **not** read the whole target file — extract its headings (see token budget).
2. Compare the broken anchor against the generated heading slugs for a close match
   (typo, renamed heading, changed casing).
3. If a confident match is found, update the anchor in the source file; otherwise
   report it as unfixable.

When replacing a link, ensure the new path/anchor points to **equivalent content** —
don't silence an error by pointing at unrelated content.

## Operating modes

### Local (default)

1. **Choose files** — default to changed/untracked docs for fast feedback:
   ```bash
   git diff --name-only --diff-filter=d HEAD -- '*.md'
   git diff --name-only --diff-filter=d --cached -- '*.md'
   git ls-files --others --exclude-standard -- '*.md'
   ```
   Union the lists. If the user asks for a full check, or nothing changed, fall back to
   the pipeline's default scope (pass no files to the script).
   If no markdown files are in scope, report that and stop.
2. **Check** — run the shared script over the chosen files to produce the broken-links
   list, then read it:
   ```bash
   OUT_DIR=./.md-link-check python3 .github/workflows/scripts/check-md-links.py <chosen files...>
   cat ./.md-link-check/broken-links.md
   ```
3. **Fix** the links named in `broken-links.md` using the fixing rules above, editing the
   source `.md` files directly in the working tree. For broken anchors, extract the
   target's headings (respecting the two-extraction token budget) to find a close match.
4. **Report** to the console — do **not** open PRs, issues, branches, or commits:
   - **Checked:** N files, M links/anchors (from the script's summary line).
   - **Fixed:** each broken link, old → new value, with source `file:line`.
   - **Unfixable:** each remaining broken link with a reason. These are exactly what
     would fail CI, so the developer can fix them before pushing.
   - **Result:** "All links OK", or the counts of fixed vs. still-broken.
   Clean up the temporary `./.md-link-check` directory. Only commit if the user explicitly asks.

### Delegated (from the pipeline workflow)

When the `md-link-checker` workflow delegates to you, it has already run the same shared
script (`.github/workflows/scripts/check-md-links.py`) and written the broken links to
`/tmp/gh-aw/agent/broken-links.md`. In that case:

- Skip discovery and the script run — use the provided `broken-links.md` as your input.
- **Expected format of `broken-links.md`:** it starts with a `# Broken Links` header
  followed by a blank line, then lists one broken link per line prefixed with `❌`
  (the same human-readable messages shown in the report — e.g.
  `❌ sub/page.md#anchor (file exists but anchor '#anchor' not found in sub/page.md) in src.md`),
  and ends with a blank line and a `**Summary:** <N> broken links` line. Parse the
  `❌`-prefixed lines as your work items; ignore the header and the summary line.
- Apply the **fixing rules** above to each broken link.
- Defer **reporting** and any cache-memory bookkeeping to the workflow's own
  instructions (it creates a pull request or issue via safe-outputs); do not report to
  the console.
