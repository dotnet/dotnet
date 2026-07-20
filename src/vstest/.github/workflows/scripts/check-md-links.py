#!/usr/bin/env python3
"""check-md-links.py — extract and test relative markdown links and heading anchors.

This is the single, shared implementation of the link-checking logic used by both:
  - the `md-link-checker` agentic workflow (.github/workflows/md-link-checker.md), and
  - the local `@md-link-checker` agent (.github/agents/md-link-checker.md).

Every relative link's target is checked for existence on disk; heading anchors are
validated for in-file links and for links that point at another `.md` file. Absolute
URLs (http/https/mailto/etc.) are intentionally ignored.

It is written in Python (no bash) so it runs unchanged on Windows, macOS, and Linux
— including the GitHub-hosted `ubuntu-latest` pipeline runner, which ships python3.

Usage:
  check-md-links.py [file.md ...]

  With no arguments it scans the default pipeline scope: every *.md under docs/
  plus the root README.md. Pass an explicit list of files (e.g. the changed docs)
  to scope the check — handy for fast local runs.

Environment:
  OUT_DIR   Directory for output files (default: /tmp/gh-aw/agent). Created if missing.

Outputs (in OUT_DIR):
  link-check-results.md   Full human-readable report of every link tested.
  broken-links.md         Only the broken links, one per line, with the source file.
  all-links.txt           Raw "source_file|url" pairs (intermediate).
  unique-links.txt        Sorted/deduped "source_file|url" pairs (intermediate).

Exit code is always 0 (a broken link is a reported result, not a script failure).
Read broken_count / working_count from the printed summary or count broken-links.md.
"""

import os
import re
import sys

# Markdown link [text](url): require non-empty text and url, like the original
# `grep -oP '\[([^\]]+)\]\(([^\)]+)\)'` followed by extracting the `(...)` part.
_LINK_RE = re.compile(r"\[[^\]]+\]\([^\)]+\)")
_PAREN_RE = re.compile(r"\([^\)]+\)")

# HTML anchor: value of a name= or id= attribute inside an <a ...> tag. The tag/attr
# match is case-insensitive; the extracted value is compared case-sensitively (as the
# original `grep -oiP ... \K` + bash `[[ == ]]` did).
_HTML_ANCHOR_RE = re.compile(
    r"<a\b[^>]*\b(?:name|id)\s*=\s*['\"]([^'\"]+)", re.IGNORECASE
)

# Heading line: 1-6 '#' then whitespace, capturing the rest (like `grep -oP '^#{1,6}\s+\K.*'`).
_HEADING_RE = re.compile(r"^#{1,6}\s+(.*)$")

# Absolute URL scheme, like bash `^[a-zA-Z][a-zA-Z0-9+.-]*:`.
_SCHEME_RE = re.compile(r"[A-Za-z][A-Za-z0-9+.-]*:")

_HTML_TAG_RE = re.compile(r"<[^>]*>")
_WS_RE = re.compile(r"\s+")
_SLUG_DROP_RE = re.compile(r"[^a-z0-9_-]")


def write_github_output(broken_count, working_count):
    """Emit step outputs when running inside GitHub Actions (no-op locally)."""
    github_output = os.environ.get("GITHUB_OUTPUT")
    if github_output:
        with open(github_output, "a", encoding="utf-8") as fh:
            fh.write("broken_count={}\n".format(broken_count))
            fh.write("working_count={}\n".format(working_count))


def read_lines(path):
    """Read a file as text lines without trailing newlines, tolerant of bad bytes."""
    try:
        with open(path, "r", encoding="utf-8", errors="replace") as fh:
            return fh.read().splitlines()
    except OSError:
        return []


def slugify(heading):
    """Reproduce the original sed/tr pipeline that turns a heading into an anchor slug."""
    # sed -E 's/<[^>]*>//g'  — strip inline HTML tags.
    s = _HTML_TAG_RE.sub("", heading)
    # sed -E 's/[[:space:]]+/ /g; s/^ //; s/ $//'  — collapse whitespace, trim one space each side.
    s = _WS_RE.sub(" ", s)
    if s.startswith(" "):
        s = s[1:]
    if s.endswith(" "):
        s = s[:-1]
    # tr '[:upper:]' '[:lower:]'  — ASCII-only lowercasing (matches POSIX tr).
    s = "".join(chr(ord(c) + 32) if "A" <= c <= "Z" else c for c in s)
    # sed 's/ /-/g'  — spaces to hyphens.
    s = s.replace(" ", "-")
    # sed 's/[^a-z0-9_-]//g'  — drop everything else.
    s = _SLUG_DROP_RE.sub("", s)
    return s


def check_anchor(path, anchor):
    """True if `anchor` matches an explicit HTML anchor or a heading slug in `path`."""
    lines = read_lines(path)
    for line in lines:
        for m in _HTML_ANCHOR_RE.finditer(line):
            if m.group(1) == anchor:
                return True
    for line in lines:
        m = _HEADING_RE.match(line)
        if m and slugify(m.group(1)) == anchor:
            return True
    return False


def normalize_target(target):
    """Mimic `realpath --relative-to=. "$target" || echo "$target"`.

    GNU realpath's default mode requires every component except the last to exist;
    if a parent component is missing it errors and the bash fallback echoes the raw,
    un-normalized path. We reproduce both the normalized and the fallback outcome so
    the reported paths match the original script byte-for-byte.
    """
    parent = os.path.dirname(target)
    if parent and not os.path.isdir(parent):
        return target  # realpath would fail; bash falls back to the raw path.
    try:
        resolved = os.path.realpath(target)
        return os.path.relpath(resolved, os.getcwd())
    except OSError:
        return target


def collect_default_files():
    """Default scope: every *.md under docs/ plus the root README.md."""
    files = []
    for root, _dirs, names in os.walk("docs"):
        for name in names:
            if name.endswith(".md"):
                files.append(os.path.join(root, name))
    if os.path.isfile("README.md"):
        files.append("README.md")
    return files


def extract_links(path):
    """Yield every url found in markdown `[text](url)` links, reproducing the
    `grep ... | grep ... | tr -d '()'` pipeline (including its multi-paren quirk)."""
    for line in read_lines(path):
        for m1 in _LINK_RE.finditer(line):
            for m2 in _PAREN_RE.finditer(m1.group(0)):
                yield m2.group(0).replace("(", "").replace(")", "")


def main(argv):
    out_dir = os.environ.get("OUT_DIR", "/tmp/gh-aw/agent")
    os.makedirs(out_dir, exist_ok=True)

    results_path = os.path.join(out_dir, "link-check-results.md")
    broken_path = os.path.join(out_dir, "broken-links.md")
    all_links_path = os.path.join(out_dir, "all-links.txt")
    unique_links_path = os.path.join(out_dir, "unique-links.txt")

    # Clear intermediate files from any previous run sharing this OUT_DIR so stale
    # artifacts can't be mistaken for the current run's output.
    for stale in (all_links_path, unique_links_path):
        try:
            os.remove(stale)
        except OSError:
            pass

    results = ["# Link Check Results", ""]
    broken = ["# Broken Links", ""]

    print("Finding all markdown files...")

    if argv:
        files = [f for f in argv if f.endswith(".md") and os.path.isfile(f)]
    else:
        files = collect_default_files()

    if not files:
        print("No markdown files found")
        # Match bash: only the two header files were written before this early exit.
        with open(results_path, "w", encoding="utf-8") as fh:
            fh.write("\n".join(results) + "\n")
        with open(broken_path, "w", encoding="utf-8") as fh:
            fh.write("\n".join(broken) + "\n")
        write_github_output(0, 0)
        return 0

    results.append("## Links Found")
    results.append("")

    all_links = []
    for path in files:
        print("Checking {}...".format(path))
        for url in extract_links(path):
            all_links.append("{}|{}".format(path, url))

    if all_links:
        with open(all_links_path, "w", encoding="utf-8") as fh:
            fh.write("\n".join(all_links) + "\n")
        unique_links = sorted(set(all_links))
        with open(unique_links_path, "w", encoding="utf-8") as fh:
            fh.write("\n".join(unique_links) + "\n")
        results.append("Found {} unique links".format(len(unique_links)))
        results.append("")
    else:
        results.append("No links found")
        with open(results_path, "w", encoding="utf-8") as fh:
            fh.write("\n".join(results) + "\n")
        with open(broken_path, "w", encoding="utf-8") as fh:
            fh.write("\n".join(broken) + "\n")
        write_github_output(0, 0)
        return 0

    results.append("## Link Test Results")
    results.append("")
    results.append("Testing links...")

    broken_count = 0
    working_count = 0
    for entry in unique_links:
        source_file, _, url = entry.partition("|")
        if url.startswith("#"):
            anchor = url[1:]
            if check_anchor(source_file, anchor):
                working_count += 1
                results.append("\u2705 {} (anchor in {})".format(url, source_file))
            else:
                broken_count += 1
                msg = "\u274c {} (anchor not found in {})".format(url, source_file)
                results.append(msg)
                broken.append(msg)
        elif _SCHEME_RE.match(url):
            # Skip absolute URLs (http, https, mailto, etc.).
            continue
        else:
            rel_path = url.split("#", 1)[0]
            anchor = url.split("#", 1)[1] if "#" in url else ""
            # Every relative link's target is checked for existence (matching the
            # original bash, which had no extension filter): broken images,
            # source/script links, and renamed assets are all flagged. Anchor
            # validation only applies to .md targets; for non-.md targets the
            # anchor (if any) is ignored. Same-file "#anchor" links are handled
            # above; absolute URLs were skipped earlier.
            source_dir = os.path.dirname(source_file)
            if source_dir == "":
                source_dir = "."
            target_path = normalize_target("{}/{}".format(source_dir, rel_path))
            if not os.path.isfile(target_path):
                broken_count += 1
                msg = "\u274c {} (file not found: {}) in {}".format(url, target_path, source_file)
                results.append(msg)
                broken.append(msg)
            elif anchor != "" and rel_path.lower().endswith(".md"):
                if check_anchor(target_path, anchor):
                    working_count += 1
                    results.append("\u2705 {} (file + anchor OK) in {}".format(url, source_file))
                else:
                    broken_count += 1
                    msg = "\u274c {} (file exists but anchor '#{}' not found in {}) in {}".format(
                        url, anchor, target_path, source_file
                    )
                    results.append(msg)
                    broken.append(msg)
            else:
                working_count += 1
                results.append("\u2705 {} (file exists: {})".format(url, target_path))

    results.append("")
    results.append("**Summary:** {} working, {} broken".format(working_count, broken_count))
    broken.append("")
    broken.append("**Summary:** {} broken links".format(broken_count))

    with open(results_path, "w", encoding="utf-8") as fh:
        fh.write("\n".join(results) + "\n")
    with open(broken_path, "w", encoding="utf-8") as fh:
        fh.write("\n".join(broken) + "\n")

    write_github_output(broken_count, working_count)

    with open(results_path, "r", encoding="utf-8") as fh:
        sys.stdout.write(fh.read())

    return 0


if __name__ == "__main__":
    sys.exit(main(sys.argv[1:]))
