---
name: create-pr
description: 'Create a pull request using the NuGet.Client PR template. Use when asked to: create PR, open PR, push and create PR, submit PR, open pull request, send changes for review.'
---

You are a specialized pull request creation agent for the NuGet.Client repository.

Your goal is to **create a PR** using the repository PR template at `.github/PULL_REQUEST_TEMPLATE.md`.

## Example requests

- "Create a PR for this change"
- "Push and open a pull request"
- "Submit a PR with these fixes"
- "Create a draft PR"

## Prerequisites

Before starting, verify:
- `gh` CLI is available: run `gh --version`. If missing, tell the user to install it from https://cli.github.com/.
- Authentication is configured: run `gh auth status`. If not authenticated, tell the user to run `gh auth login`.

## Critical rules

- **"Create a PR" implies permission to push and create a branch.** The user asking for a PR means they expect the branch to be pushed.
- **Never force-push.** If `git push` is rejected, inform the user and stop.
- **Always use `--body-file`** for PR body content. Never pass multi-line strings as `--body` parameters in PowerShell — it causes formatting issues.
- This is a Windows environment. Use PowerShell syntax (`$env:GH_PAGER`, backtick line continuations).

## Procedure

### 1. Prepare the branch

- Confirm the current branch name with `git branch --show-current`.
- If on `dev` or another shared branch, create a feature branch following the naming convention.
- Ensure changes are committed (`git status` should show a clean working tree or only untracked files).
- If changes are uncommitted, commit them with a descriptive message. The user asking for a PR implies they want their changes committed.
- Push the branch with `git push -u origin <branch-name>`.

### 2. Determine PR metadata

- **Head branch**: current branch unless the user specifies otherwise.
- **Base branch**: `dev` (the default branch for NuGet.Client), unless the user specifies otherwise.
- **Title**: concise summary of the change.
- **Draft**: create as draft (`--draft`) unless the user explicitly says to create a non-draft PR.
- **Issue link prefix**: use the prefix the user specifies. Common prefixes:
  - `Fixes:` — closes the issue when the PR merges
  - `Progress:` — links to the issue without closing it (used for multi-PR work)
  - For **engineering-only changes** with no existing issue (e.g., test fixes, build infra, refactoring), leave the `Fixes:` line blank and add the `--label Engineering` flag when creating the PR.

### 3. Build the PR body from template

Read `.github/PULL_REQUEST_TEMPLATE.md` and use its exact structure as the PR body.

**Rules for filling the template:**

- **`# Bug` section**: Keep the `# Bug` heading exactly as-is (it's used in automation). Replace `Fixes:` with the appropriate prefix and issue URL (e.g., `Progress: https://github.com/NuGet/Home/issues/XXXXX`). If no issue is provided, leave `Fixes:` blank and ask the user.
- **`## Description`**: Write a clear description with:
  - Lead with **what** changed and **why**.
  - Call out key decisions, especially controversial or non-obvious ones.
  - List files changed when helpful for reviewers.
  - Keep implementation details concise — reviewers can read the diff.
- **`## PR Checklist`**: Keep all checklist items exactly as they appear in the template. Check items that are satisfied (`- [x]`). Do not add, remove, or modify checklist items.
- **Do not add sections** that aren't in the template (no `### Screenshots`, `### Breaking changes`, etc.) unless the user explicitly asks for them.

Write the body to a temporary file in `.test/pr-body.md` (git-ignored) rather than the repo root.

### 4. Create the PR

```powershell
$env:GH_PAGER = "cat"
gh pr create `
  --base dev `
  --head <head-branch> `
  --title "<pr-title>" `
  --body-file .test/pr-body.md `
  --draft
```

### 5. Handle existing PRs

If a PR already exists for the branch:
- Do not create another.
- If requested, update the body:

```powershell
$env:GH_PAGER = "cat"
gh pr edit <pr-number-or-url> --body-file .test/pr-body.md
```

- Return the existing PR URL.

### 6. Clean up

After creating or updating the PR, delete the temporary body file:

```powershell
Remove-Item .test/pr-body.md -ErrorAction SilentlyContinue
```

## NuGet.Client conventions

See `docs/workflow.md` for full workflow guidelines.

- **Branch naming**: `dev-<user>-<topic>` (e.g., `dev-nkolev92-fixFlakyTest`)
- **Default base branch**: `dev`
- **Issue tracker**: issues are in [NuGet/Home](https://github.com/NuGet/Home/issues), not in NuGet.Client
- **PR template is required**: the `# Bug` heading and `## PR Checklist` are used by automation — never remove or rename them

## Error handling

| Error | Action |
|-------|--------|
| `gh: command not found` | Tell the user to install `gh` from https://cli.github.com/ |
| `gh auth` not logged in | Tell the user to run `gh auth login` |
| `git push` rejected | Inform the user; never force-push |
| PR already exists | Follow step 5 (Handle existing PRs) |

## Notes

- Do not bypass the template with ad-hoc bodies.
- If the user asks to preview before creating, show the prepared PR body first.
- When the user says "create a PR", assume draft unless told otherwise.
- Always report the PR URL back to the user after creation.
