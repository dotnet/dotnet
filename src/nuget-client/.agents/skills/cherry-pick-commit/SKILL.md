---
name: cherry-pick-commit
description: >-
  Cherry-pick (backport/forward-port) one or more commits from one branch onto another,
  creating a fresh branch and opening a pull request that follows the repository's conventions.
  Use this whenever the user wants to move, port, backport, or cherry-pick a commit or a merged
  PR to a release/servicing branch (e.g. "cherry-pick the latest dev commit to 7.9.x",
  "backport #7576 to release/8.0.x", "port that fix to the release branch"), even if they don't
  say the exact word "cherry-pick". If the cherry-pick hits conflicts, STOP and report back
  instead of trying to resolve them, unless the user explicitly asked you to resolve conflicts.
---

# Cherry-pick a commit

A fast way to cherry-pick (backport) a commit from one branch onto another — usually a fix from the
development branch to a release/servicing branch. It keeps the happy path quick and the unhappy path
safe: the usual failure modes are branching off a **stale local copy** of the target, silently
resolving a conflict the wrong way, or forgetting to reference the original PR. A conflict should
stop you and surface the problem, never turn into a guess.

## What you need before starting

Figure these two things out first (from the user's request, then by inspecting the repo):

1. **Source** — which commit(s) to cherry-pick. Common phrasings:
   - "the latest commit on dev" → the tip of `origin/<dev-branch>` (after fetching). Do **not** trust
     the local branch; resolve it against `origin`.
   - a PR number (e.g. "#7576") → the squash-merge commit for that PR on the source branch.
   - an explicit SHA or range.
2. **Target** — the branch to cherry-pick *onto* (e.g. "7.9.x"). Users abbreviate release branches;
   map the shorthand to the real ref by listing remote branches (a name like `7.9.x` usually means
   `release/7.9.x` or `release-7.9.x`). Confirm the ref actually exists on `origin` before continuing.

If either is genuinely ambiguous, ask. Otherwise proceed — this is meant to be low-friction.

## The workflow

Run these steps in order. Interpret output at each step; don't blindly chain commands, because a
bad early step (dirty tree, wrong target) makes everything after it wrong.

### 1. Preflight

```bash
git status --porcelain            # must be empty — a dirty tree makes cherry-pick/abort unsafe
git fetch origin --prune          # get the true tips of source and target
git rev-parse --abbrev-ref HEAD   # remember where you started, to return here later
```

If the working tree is dirty, stop and tell the user (offer to stash). Fetching matters:
release branches and dev move independently, and a local `release/*` is frequently behind `origin`.

### 2. Resolve the real refs

```bash
git rev-parse --short origin/<dev-branch>        # -> the source SHA, if "latest on dev"
git branch -r | grep -i <target-shorthand>       # confirm the target ref name on origin
git branch -r --contains <source-sha> | grep <target>   # is it ALREADY on the target?
```

If the commit is already contained in the target branch, don't cherry-pick — report that it's
already there. Cherry-picking an already-present change produces an empty or redundant commit.

### 3. Create the working branch off the *remote* target

Name it using the repository's branch convention. Many repos (including NuGet.Client) use
`dev-<github-handle>-<topic>`; check the repo's instructions/AGENTS.md if unsure, and get the handle
from `gh api user --jq .login`. Make the topic descriptive of the backport, e.g.
`cherrypick-<pr-or-sha>-<target>`.

```bash
git checkout -b dev-<handle>-cherrypick-<pr-or-sha>-<target> origin/<target-ref>
```

Branching off `origin/<target-ref>` (not the local copy) is the single most important correctness
step — it guarantees you build on the real head of the release branch.

### 4. Cherry-pick

Use `-x` so the new commit records "(cherry picked from commit …)". Preserving the original commit
message (including the `(#NNNN)` PR suffix) is what lets a reviewer follow the change back to its
origin — this is an explicit expectation for release-branch commits in some repos, so keep it.

```bash
git cherry-pick -x <source-sha>
```

For a range, use `git cherry-pick -x <from>^..<to>`.

### 5. Branch on the result

- **Success** (exit 0, clean apply): continue to step 6.
- **Conflict / failure** (non-zero exit, or `git status` shows unmerged paths): **do not resolve it
  yourself** unless the user asked you to. Capture which files conflicted, then undo cleanly and
  report:

  ```bash
  git cherry-pick --abort
  git checkout <original-branch>
  git branch -D dev-<handle>-cherrypick-<pr-or-sha>-<target>   # remove the aborted branch
  ```

  Then STOP and report back using the failure template below. A conflict means the target branch has
  diverged in the touched code; that needs a human decision, and silently picking a side is how
  backports introduce regressions.

### 6. Finish the PR (success path only)

Only reformat if you had to resolve conflicts by hand — a clean cherry-pick reproduces
already-formatted, already-reviewed content byte-for-byte, so running a full format is wasted work
and can create spurious diffs. If the repo mandates formatting and you *did* edit files, run its
formatter now.

**Delegate the push and PR creation to the repo's `create-pr` skill**
(`.copilot/skills/create-pr`) rather than re-implementing them here — that skill owns the branch
push, PR template, draft default, `gh` invocation, and error handling, and keeping one source of
truth means backport PRs stay consistent with every other PR in the repo. Hand it these
backport-specific parameters:

- **Base branch**: the target branch (`<target-ref>`) — override its default of `dev`.
- **Title**: the original commit subject, unchanged, so the `(#NNNN)` link is preserved.
- **Body**: the backport content described below.
- Backport PRs don't need review and merge once the build is green (see `docs/workflow.md`), so
  create it ready (non-draft) unless the user says otherwise.

Then report the resulting PR URL.

## PR description (what to feed the `create-pr` skill)

The `create-pr` skill builds the body from the PR template; your job is to give it backport-specific
content that makes it obvious this is a port, not a new change:

- State that it's a cherry-pick of the original PR/commit onto the target branch, and link the
  original PR (`#NNNN`).
- Reuse the original PR's linked issue, keeping the same prefix it used (`Fixes:`/`Progress:`).
- Tick checklist items that genuinely apply (e.g. tests are "added" if the original commit carried
  them; they came along with the pick).

## Failure report template

When a cherry-pick fails, after aborting and cleaning up, report like this so the user can decide
what to do:

```
Cherry-pick FAILED — conflicts, nothing pushed.

- Source: <sha> "<subject>" (from <source-branch>/#<pr>)
- Target: <target-ref>
- Conflicting files:
    - path/one.cs
    - path/two.cs

The target branch has diverged in these files. I aborted the cherry-pick and deleted the temporary
branch, so your tree is back on <original-branch> and unchanged. Want me to retry and resolve the
conflicts, or would you prefer to handle it manually?
```

## Notes and edge cases

- **Never rewrite history or force-push** on shared branches.
- **One commit per PR** is the usual release-branch norm (so a single revert is clean). Only batch
  multiple commits into one PR if the user asks.
- **Merge commits** need `-m 1` to pick the mainline parent; prefer picking the underlying squash
  commit instead when possible.
- **Empty result** ("The previous cherry-pick is now empty"): the change is already present — run
  `git cherry-pick --skip`/`--abort` as appropriate and report it's already in the target.
