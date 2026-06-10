---
description: Weekly automated link checker that finds and fixes broken links in documentation files
on:
  schedule: weekly on Friday
permissions: read-all
timeout-minutes: 60
network:
  allowed:
    - github
steps:
  - name: Checkout repository
    uses: actions/checkout@v4
    with:
      fetch-depth: 0
      persist-credentials: false

  - name: Check and test all documentation links
    id: link-check
    run: |
      echo "# Link Check Results" > /tmp/gh-aw/agent/link-check-results.md
      echo "" >> /tmp/gh-aw/agent/link-check-results.md
      echo "# Broken Links" > /tmp/gh-aw/agent/broken-links.md
      echo "" >> /tmp/gh-aw/agent/broken-links.md
      
      # Find all markdown files in docs directory and README
      echo "Finding all markdown files..."

      if ! find docs README.md -type f -name "*.md" -print0 2>/dev/null | grep -qz .; then
        echo "No markdown files found"
        echo "no_files=true" >> $GITHUB_OUTPUT
        exit 0
      fi

      # Extract all links from markdown files
      echo "## Links Found" >> /tmp/gh-aw/agent/link-check-results.md
      echo "" >> /tmp/gh-aw/agent/link-check-results.md

      # Use grep to find markdown links
      # Format for relative links: "source_file|url" to allow path resolution
      find docs README.md -type f -name "*.md" -print0 2>/dev/null | while IFS= read -r -d '' file; do
        echo "Checking $file..."
        # Extract markdown links [text](url)
        grep -oP '\[([^\]]+)\]\(([^\)]+)\)' "$file" | grep -oP '\(([^\)]+)\)' | tr -d '()' | while IFS= read -r link; do
          echo "$file|$link" >> /tmp/gh-aw/agent/all-links.txt
        done 2>/dev/null || true
      done

      # Remove duplicates and sort
      if [ -f /tmp/gh-aw/agent/all-links.txt ]; then
        sort -u /tmp/gh-aw/agent/all-links.txt > /tmp/gh-aw/agent/unique-links.txt
        LINK_COUNT=$(wc -l < /tmp/gh-aw/agent/unique-links.txt)
        echo "Found $LINK_COUNT unique links" >> /tmp/gh-aw/agent/link-check-results.md
        echo "" >> /tmp/gh-aw/agent/link-check-results.md
      else
        echo "No links found" >> /tmp/gh-aw/agent/link-check-results.md
        echo "no_links=true" >> $GITHUB_OUTPUT
        exit 0
      fi

      # Helper: check if an explicit HTML anchor or markdown heading anchor exists in a file
      check_anchor() {
        local file="$1"
        local anchor="$2"
        local html_anchor heading generated

        while IFS= read -r html_anchor; do
          if [[ "$html_anchor" == "$anchor" ]]; then
            return 0
          fi
        done < <(grep -oiP "<a\\b[^>]*\\b(?:name|id)\\s*=\\s*['\"]\\K[^'\"]+(?=['\"])" "$file" 2>/dev/null)

        while IFS= read -r heading; do
          generated=$(printf '%s' "$heading" | sed -E 's/<[^>]*>//g' | sed -E 's/[[:space:]]+/ /g; s/^ //; s/ $//' | tr '[:upper:]' '[:lower:]' | sed 's/ /-/g' | sed 's/[^a-z0-9_-]//g')
          if [[ "$generated" == "$anchor" ]]; then
            return 0
          fi
        done < <(grep -oP '^#{1,6}\s+\K.*' "$file" 2>/dev/null)

        return 1
      }
      # Test each link
      echo "## Link Test Results" >> /tmp/gh-aw/agent/link-check-results.md
      echo "" >> /tmp/gh-aw/agent/link-check-results.md
      echo "Testing links..." >> /tmp/gh-aw/agent/link-check-results.md

      BROKEN_COUNT=0
      WORKING_COUNT=0
      while IFS='|' read -r source_file url; do
        if [[ "$url" == "#"* ]]; then
          # Same-file anchor link
          ANCHOR="${url#\#}"
          if check_anchor "$source_file" "$ANCHOR"; then
            WORKING_COUNT=$((WORKING_COUNT + 1))
            echo "✅ $url (anchor in $source_file)" >> /tmp/gh-aw/agent/link-check-results.md
          else
            BROKEN_COUNT=$((BROKEN_COUNT + 1))
            echo "❌ $url (anchor not found in $source_file)" >> /tmp/gh-aw/agent/link-check-results.md
            echo "❌ $url (anchor not found in $source_file)" >> /tmp/gh-aw/agent/broken-links.md
          fi
        elif [[ "$url" =~ ^[a-zA-Z][a-zA-Z0-9+.-]*: ]]; then
          # Skip absolute URLs (http, https, mailto, etc.) — we only check links to other .md files and anchors
          continue
        else
          # Relative file link, possibly with anchor
          # Split into file path and optional anchor
          REL_PATH="${url%%#*}"
          ANCHOR=""
          if [[ "$url" == *"#"* ]]; then
            ANCHOR="${url#*#}"
          fi
          # Resolve relative to the source file's directory
          SOURCE_DIR=$(dirname "$source_file")
          TARGET_PATH="$SOURCE_DIR/$REL_PATH"
          # Normalize the path
          TARGET_PATH=$(realpath --relative-to=. "$TARGET_PATH" 2>/dev/null || echo "$TARGET_PATH")
          if [[ -z "$REL_PATH" ]] && [[ -n "$ANCHOR" ]]; then
            # Link is just "#anchor" handled above, but in case of edge cases
            WORKING_COUNT=$((WORKING_COUNT + 1))
          elif [[ ! -f "$TARGET_PATH" ]]; then
            BROKEN_COUNT=$((BROKEN_COUNT + 1))
            echo "❌ $url (file not found: $TARGET_PATH) in $source_file" >> /tmp/gh-aw/agent/link-check-results.md
            echo "❌ $url (file not found: $TARGET_PATH) in $source_file" >> /tmp/gh-aw/agent/broken-links.md
          elif [[ -n "$ANCHOR" ]]; then
            # File exists, check the anchor
            if check_anchor "$TARGET_PATH" "$ANCHOR"; then
              WORKING_COUNT=$((WORKING_COUNT + 1))
              echo "✅ $url (file + anchor OK) in $source_file" >> /tmp/gh-aw/agent/link-check-results.md
            else
              BROKEN_COUNT=$((BROKEN_COUNT + 1))
              echo "❌ $url (file exists but anchor '#$ANCHOR' not found in $TARGET_PATH) in $source_file" >> /tmp/gh-aw/agent/link-check-results.md
              echo "❌ $url (file exists but anchor '#$ANCHOR' not found in $TARGET_PATH) in $source_file" >> /tmp/gh-aw/agent/broken-links.md
            fi
          else
            WORKING_COUNT=$((WORKING_COUNT + 1))
            echo "✅ $url (file exists: $TARGET_PATH)" >> /tmp/gh-aw/agent/link-check-results.md
          fi
        fi
      done < /tmp/gh-aw/agent/unique-links.txt

      echo "" >> /tmp/gh-aw/agent/link-check-results.md
      echo "**Summary:** $WORKING_COUNT working, $BROKEN_COUNT broken" >> /tmp/gh-aw/agent/link-check-results.md
      echo "" >> /tmp/gh-aw/agent/broken-links.md
      echo "**Summary:** $BROKEN_COUNT broken links" >> /tmp/gh-aw/agent/broken-links.md
      # Output results
      echo "broken_count=$BROKEN_COUNT" >> $GITHUB_OUTPUT
      echo "working_count=$WORKING_COUNT" >> $GITHUB_OUTPUT

      cat /tmp/gh-aw/agent/link-check-results.md
    shell: bash

tools:
  github:
    toolsets: [default]
  cache-memory: true
  bash: true
  edit:

safe-outputs:
  create-pull-request:
    title-prefix: "[link-checker] "
    labels: ["Area: Documentation", "agentic-workflows"]
    draft: false
    protected-files: fallback-to-issue
    if-no-changes: "warn"
  create-issue:
    max: 1
    title-prefix: "[link-checker] "
    labels: ["Area: Documentation", "agentic-workflows"]
  noop:
    report-as-issue: false
---

# Weekly Relative Link Checker & Fixer

You are an automated link checker and fixer agent. Your job is to find and fix broken links between documentation files in this repository. Only links to other `.md` files and in-file/cross-file heading anchors are checked. Absolute URLs (http/https/mailto/etc.) are intentionally ignored.

## Your Mission

Your workflow has already collected and tested all relative documentation links in the previous step. Use the test results to identify broken links and fix them where possible.

## Step 1: Review Link Check Results

The link check step has already run. Read `/tmp/gh-aw/agent/broken-links.md` to see all broken links that need fixing:
- Each line lists a broken relative link or anchor and the source file where it appears

Use bash to read the file:
```bash
cat /tmp/gh-aw/agent/broken-links.md
```

## Step 2: Load Cache Memory

Check cache memory for previously identified unfixable broken links:
- Load the cache memory to see if there are any broken links we've tried to fix before but couldn't
- These are links whose target file or anchor no longer exists and has no obvious replacement
- Skip these links to avoid repeated attempts

The cache memory should store a JSON object with this structure:
```json
{
  "unfixable_links": [
    {
      "url": "../old/removed-page.md",
      "source_file": "docs/index.md",
      "reason": "Target file removed and no replacement found",
      "first_seen": "2026-02-17"
    }
  ],
  "last_run": "2026-02-17"
}
```

## Step 3: Research and Fix Broken Links

For each broken link found in the test results (but NOT in the unfixable list):

If it's a broken relative link (file not found):
1. Use `find` or `bash` to search for a file with the same basename elsewhere in the repo.
2. If found, update the path in the source markdown file.
3. If not found, mark as unfixable and move on.

If it's a broken anchor (file exists but heading anchor not found):
1. **Do NOT read the target file.** The bash step already confirmed the file exists and the anchor is missing.
2. Use `bash` to extract only the headings: `grep -oP '^#{1,6}\s+\K.*' <file>`
3. Compare the broken anchor against the extracted headings to find a close match (typo, renamed heading, changed casing).
4. If a match is found, update the anchor in the source markdown file.
5. If no match is found, mark as unfixable — do not fetch or read the file further.

**Token budget rule for anchors:** spend at most two `grep` calls per broken anchor. Never read entire files to fix anchors.

If the link cannot be fixed:
- Add it to the `unfixable_links` list in cache memory
- Include the URL, source file, reason, and date
- This prevents future runs from wasting time on the same broken link

## Step 4: Update Cache Memory

After processing all broken links:
- Update the cache memory with any new unfixable links
- Update the "last_run" timestamp
- Save the updated cache memory

## Step 5: Create Pull Request or Noop

Based on your work:

**If you fixed any links:**
- Use the `create-pull-request` safe output to create a PR with your fixes
- In the PR body, include:
  - A summary of how many links were fixed
  - A list of the broken links and their replacements
  - Any links that were added to the unfixable list
- Title format: "Fix broken documentation links"

**If you could not fix anchors**
- Use the `create-issue` safe output to create an issue with broken links
- In the issue description, include:
   - A summary of how many links could not be fixed
   - A list of the broken anchors
- Title format: "Invalid markdown links"

**If no links needed fixing:**
- Use the `noop` safe output with a clear message like:
  - "All documentation links are working correctly" (if no broken links found)
  - "All broken links are in the unfixable list, no new fixes available" (if broken links exist but can't be fixed)

## Important Guidelines

- **Be thorough:** Check every broken link carefully
- **Preserve context:** When replacing links, make sure the new path/anchor points to equivalent content
- **Document everything:** Keep the cache memory up to date with unfixable links
- **Be selective:** Only add links to the unfixable list if you've genuinely tried to find alternatives
- **Scope:** Only relative `.md` file links and heading anchors are in scope. Absolute URLs (http/https/mailto/etc.) are ignored by the checker — do not attempt to validate or fix them.

## Example Cache Memory Update

```json
{
  "unfixable_links": [
    {
      "url": "../old/removed-page.md",
      "source_file": "docs/index.md",
      "reason": "Target file removed and no replacement found in the repo",
      "first_seen": "2026-02-17"
    }
  ],
  "last_run": "2026-02-17"
}
```

## Context

- Repository: `${{ github.repository }}`
- Run weekly on Fridays to catch broken links early
- Link test results are available at `/tmp/gh-aw/agent/link-check-results.md`
