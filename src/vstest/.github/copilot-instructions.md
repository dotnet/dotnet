This is a .NET based repository that contains the VSTest test platform. Please follow these guidelines when contributing:

## Code Standards

You MUST follow all code-formatting and naming conventions defined in [`.editorconfig`](../.editorconfig).

In addition to the rules enforced by `.editorconfig`, you SHOULD:

- Favor style and conventions that are consistent with the existing codebase.
- Prefer file-scoped namespace declarations and single-line using directives.
- Ensure that the final return statement of a method is on its own line.
- Use pattern matching and switch expressions wherever possible.
- Use `nameof` instead of string literals when referring to member names.
- Always use `is null` or `is not null` instead of `== null` or `!= null`.
- Trust the C# null annotations and don't add null checks when the type system says a value cannot be null.
- Prefer `?.` if applicable (e.g. `scope?.Dispose()`).
- Use `ObjectDisposedException.ThrowIf` where applicable.
- Respect StyleCop.Analyzers rules, in particular:
  - SA1028: Code must not contain trailing whitespace
  - SA1316: Tuple element names should use correct casing
  - SA1518: File is required to end with a single newline character

You MUST minimize adding public API surface area but any newly added public API MUST be declared in the related `PublicAPI.Unshipped.txt` file.

## Working with Git Worktrees

This repository uses git worktrees to work on multiple things at the same time. Worktrees live in `../vstest-tree/<branch>` relative to the main clone at `c:\p\vstest`.

The following global git aliases are configured:

```shell
git config --global alias.wta '!f() { mkdir -p "$(git rev-parse --show-toplevel)/../vstest-tree" 2>/dev/null; git worktree add -b "$1" "../vstest-tree/$1"; }; f'
git config --global alias.wtr '!f() { git worktree remove "../vstest-tree/$1"; }; f'
git config --global alias.wtl '!f() { git worktree list; }; f'
```

Usage:

```shell
git wta my-feature-branch   # creates c:\p\vstest-tree\my-feature-branch
git wtl                     # list all active worktrees
git wtr my-feature-branch   # remove worktree when done
```

The upstream remote `upstream` points to `https://github.com/microsoft/vstest`. To sync with upstream:

```shell
git fetch upstream
git checkout main
git merge upstream/main
```

## Localization Guidelines

Anytime you add a new localization resource, you MUST:
- Add a corresponding entry in the localization resource file.
- Add an entry in all `*.xlf` files related to the modified `.resx` file.
- Do not modify existing entries in '*.xlf' files unless you are also modifying the corresponding `.resx` file.

## Unattended Work Instructions

When working autonomously on issues (e.g. from a milestone), follow this workflow:

### Before Starting

- **Assign the issue** to the user you are working on behalf of before starting work.
- **Skip issues with active PRs** — if an issue already has a PR with recent activity, don't duplicate the work.

### Implement

- Create a feature branch from `main` (never commit to `main` directly).
- Implement the change and write tests where applicable.
- Build locally to validate the change compiles. Debug configuration is fine for local builds.

### Create PR

- Check `git remote -v` to identify which remote is the fork (has the user's name in the URL, e.g. `github.com/<user>/vstest`) and which is the upstream repo (`github.com/microsoft/vstest`).
- Push the branch to the fork remote.
- Create the PR against `microsoft/vstest` (the upstream repo) — **never PR to the fork**.
- Do not create draft PRs — undrafting forces a re-build.

### Monitor PRs

- CI pipeline skips builds for doc-only changes (e.g. `.md` files) — these PRs go green in under a minute.
- For code changes, check PR status within 15–20 minutes — the Windows build and tests finish first, macOS/ubuntu take longer.
- Check **both** CI status (pass/fail) **and** mergeable state — PR checks can show green even when there are merge conflicts. Always verify with `gh pr view <number> --json mergeable`.
- When a build fails, take hints from the automated PR review comments but reason about them — the reviewer is automated and may be wrong.
- If a build fails, investigate the failure, push a fix to the same branch, and wait for the rebuild.
- If a PR becomes CONFLICTING, rebase the branch onto `main` and force-push using `--force-with-lease` (e.g. `git push --force-with-lease`).
- When a PR is fully green (all checks pass, no conflicts), mark it as ready to merge.

### Troubleshooting

- If a build fails with warnings treated as errors (e.g. IDE0005 unnecessary using), and you cannot reproduce locally, try building with `-c Release` to match CI: `./build.cmd -c Release`.
