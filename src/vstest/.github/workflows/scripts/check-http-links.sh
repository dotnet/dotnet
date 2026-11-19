#!/usr/bin/env bash
# check-http-links.sh — extract and test absolute HTTP(S) links in the documentation.
#
# This is the single, shared implementation of the HTTP link-checking logic used by both:
#   - the `http-link-check-probe` workflow (.github/workflows/http-link-check-probe.yml), and
#   - the `http-link-checker` agentic workflow (.github/workflows/http-link-checker.md).
#
# The probe workflow runs it on a schedule and only wakes the agent when the set of broken
# links differs from eng/agentic-workflows/known-broken-links.txt. Keeping one copy of
# the loop is what stops the gate and the agent disagreeing about what "broken" means.
#
# Links are probed with GET (`curl -L -s -o /dev/null`), never HEAD. Several hosts we link
# to answer HEAD and GET differently — nuget.org answers 404 to HEAD and 200 to GET, and
# github.com/user-attachments answers 403 to HEAD and 200 to GET — so HEAD would invent
# more than a hundred broken links. It is bash rather than python because the probing
# semantics have to stay exactly what they were; a different HTTP stack is a different probe.
#
# Usage:
#   check-http-links.sh [file.md ...]
#
#   With no arguments it scans the default pipeline scope: every *.md under docs/ plus the
#   root README.md. Pass an explicit list of files to scope the check. Run from the repo root.
#
# Environment:
#   OUT_DIR   Directory for output files (default: /tmp/gh-aw/agent). Created if missing.
#
# Outputs (in OUT_DIR):
#   link-check-results.md   Full human-readable report of every link tested (✅ / ❌).
#   broken-links.txt        Only the broken links, sorted, one URL per line.
#   all-links.txt           Raw extracted URLs (intermediate).
#   unique-links.txt        Sorted/deduped URLs (intermediate).
#
# Also writes broken_count / working_count to $GITHUB_OUTPUT when running under Actions.
# Always exits 0 when the scan completes: broken links are data, not a script failure.

set -uo pipefail

if ! printf '\n' | grep -P '' >/dev/null 2>&1; then
  echo "Error: check-http-links.sh requires grep with PCRE (-P) support." >&2
  exit 1
fi

OUT_DIR="${OUT_DIR:-/tmp/gh-aw/agent}"
mkdir -p "$OUT_DIR"

RESULTS="$OUT_DIR/link-check-results.md"
ALL_LINKS="$OUT_DIR/all-links.txt"
UNIQUE_LINKS="$OUT_DIR/unique-links.txt"
BROKEN_LINKS="$OUT_DIR/broken-links.txt"

# Start from a clean slate so repeated runs in one job don't accumulate links.
: > "$ALL_LINKS"
: > "$BROKEN_LINKS"

{
  echo "# Link Check Results"
  echo ""
} > "$RESULTS"

if [ "$#" -gt 0 ]; then
  MARKDOWN_FILES=("$@")
else
  echo "Finding all markdown files..."
  mapfile -d '' -t MARKDOWN_FILES < <(find docs README.md -type f -name "*.md" -print0 2>/dev/null)
fi

if [ "${#MARKDOWN_FILES[@]}" -eq 0 ]; then
  echo "No markdown files found"
  if [ -n "${GITHUB_OUTPUT:-}" ]; then echo "no_files=true" >> "$GITHUB_OUTPUT"; fi
  exit 0
fi

{
  echo "## Links Found"
  echo ""
} >> "$RESULTS"

for file in "${MARKDOWN_FILES[@]}"; do
  echo "Checking $file..."
  # Extract markdown links [text](url)
  grep -oP '\[([^\]]+)\]\(([^\)]+)\)' "$file" | grep -oP '\(([^\)]+)\)' | tr -d '()' >> "$ALL_LINKS" 2>/dev/null || true
  # Extract plain HTTP(S) URLs from non-markdown-link text to avoid duplicates/trailing ')'
  sed -E 's/\[[^]]+\]\(([^)]+)\)/ /g' "$file" | grep -oP 'https?://[^\s<>"]+' | awk '{ if (index($0,"(") == 0) sub(/\)$/, "", $0); print }' >> "$ALL_LINKS" 2>/dev/null || true
done

if [ -s "$ALL_LINKS" ]; then
  sort -u "$ALL_LINKS" > "$UNIQUE_LINKS"
  LINK_COUNT=$(wc -l < "$UNIQUE_LINKS")
  {
    echo "Found $LINK_COUNT unique links"
    echo ""
  } >> "$RESULTS"
else
  echo "No links found" >> "$RESULTS"
  if [ -n "${GITHUB_OUTPUT:-}" ]; then echo "no_links=true" >> "$GITHUB_OUTPUT"; fi
  exit 0
fi

{
  echo "## Link Test Results"
  echo ""
  echo "Testing links..."
} >> "$RESULTS"

BROKEN_COUNT=0
WORKING_COUNT=0

while IFS= read -r url; do
  # Skip relative links and anchors
  if [[ "$url" == "#"* ]] || [[ "$url" != "http"* ]]; then
    continue
  fi

  # Test the link with curl
  HTTP_CODE=$(curl -L -s -o /dev/null -w "%{http_code}" --max-time 10 "$url" 2>/dev/null || echo "000")

  if [[ "$HTTP_CODE" =~ ^2 ]] || [[ "$HTTP_CODE" =~ ^3 ]]; then
    WORKING_COUNT=$((WORKING_COUNT + 1))
    echo "✅ $url (HTTP $HTTP_CODE)" >> "$RESULTS"
  else
    BROKEN_COUNT=$((BROKEN_COUNT + 1))
    echo "❌ $url (HTTP $HTTP_CODE)" >> "$RESULTS"
    echo "$url" >> "$BROKEN_LINKS"
  fi
done < "$UNIQUE_LINKS"

sort -o "$BROKEN_LINKS" "$BROKEN_LINKS"

{
  echo ""
  echo "**Summary:** $WORKING_COUNT working, $BROKEN_COUNT broken"
} >> "$RESULTS"

if [ -n "${GITHUB_OUTPUT:-}" ]; then
  {
    echo "broken_count=$BROKEN_COUNT"
    echo "working_count=$WORKING_COUNT"
  } >> "$GITHUB_OUTPUT"
fi

cat "$RESULTS"
