# VMR QB Triage

How to decide what a failure *is*, who owns it, and whether it needs an action. When structured evidence gives an exact answer
(for example `logs[].repoBuild`), use it rather than re-deriving it from the raw log.

**This page holds only what the sources below do not.** It does not restate their rules, so they cannot go stale here.

| Topic | Source of truth |
|---|---|
| Fatal vs non-fatal, failure categories, artifact cascades, signing failures | the [pipeline-investigation skill](https://github.com/dotnet/arcade-skills/tree/main/plugins/dotnet-helix/skills/pipeline-investigation) |
| Repo to owner, merge rules, branches | the [VMR QB Guide](https://github.com/dotnet/dotnet/blob/main/docs/VMR-QB-Guide.md) |

## 1. Fatal vs non-fatal: use records, not words

Use the definition in the pipeline-investigation skill (Step 1, "Classify every issue as fatal or non-fatal"). Two QB-side rules on top
of it:

- Take the parent Job's result from the timeline, not from the task alone. A job that *succeeded* is exactly the proof that its
  failed task was non-fatal, even when that job's own record was not kept. The collector records it as each task's `jobResult`,
  and the digest's `severity` applies it to every diagnostic, promotion builds included.
- Do not classify by text. 1ES Pipeline Template tasks emit `##[error]` and let the job pass anyway.

## 2. Name the diagnostic that actually fired

Describe a failure by the error that terminated it, not by whichever test or script hosted it.

- A wrapper `MSB3073` / `Bash exited with code 1` / `Darc exit 1` is not a finding. Find the inner diagnostic. If the only retained
  diagnostic *is* the wrapper, say so: name the repo, the exit code, and the inner log that was not collected.
- An installer validation leg failing with `NETSDK1045` is a **target-framework failure**, not a "dnx regression", even when the
  console dump is full of `dotnet dnx ... Usage:` noise.
- **One log can hold two unrelated failures.** A single log can contain both a fast-fail in one repo (an option that was just
  removed, say) and a timeout in other repos. Classify each leg by *its own first terminating diagnostic and its own duration*.
  Never merge them, and never let an error line in the same file stand in for the cause of the timeout.

### Keep the name stable across days

A recurring failure keeps the **same** name, character for character. Renaming "Azure source-index subscription missing" to
"Source-index Azure subscription missing" makes a three-week-old problem read as new. Read recent overall reports and owner notes,
and reuse the exact name when the failure matches.

## 3. Timeouts: which repo was actually running

A leg that "timed out" is a statement about the leg. The owner is whichever repo was running when the time ran out, and that comes
from the log's own markers: `Building <repo>` starts an attempt; `Building <repo>...done` or
`'<repo>' failed during build.` ends it. A failed repo is no longer running, though its error remains
relevant context. A generic error line alone does not end an attempt. Never choose the timeout owner
just because that repo logged an error in the same file. The digest gives the answer as `logs[].repoBuild`, computed by `triage/timeout.ps1`: a
job timeout must be confirmed by the job's explicit timeline evidence, not by the duration of one task's log. Without confirming
evidence the answer is uncertain, even for a long log. For a confirmed timeout, a repo is a suspect only if it was unfinished and
had been running for at least 10 minutes. Use that answer as given. If `reliable` is false, name no repo.

## 4. Routing

- **Repo to owner** comes from the Repo Ownership table in the
  [VMR QB Guide](https://github.com/dotnet/dotnet/blob/main/docs/VMR-QB-Guide.md#repo-ownership) (itself derived from
  `.github/policies/assign_ownership.yml`). Read its local JSON copy at `runContext.guideOwnershipPath`: each `rows` entry maps
  `repo` to `team`. Read `runContext.routingPath` for `vmrTestsOwner` (the owner of the VMR-level tests and packaging under `test/`),
  `preferredRedirects`, any repo-owner overrides or handle aliases, `categories`, `supportDestinations`, `channels` and
  `channelRules`. Use only handles these sources provide. If an owner cannot be established from readable data, write
  "No identified owner".
- **A repo name used as a handle** (`@dotnet/runtime`, `@dotnet/efcore`) is not a team. It resolves to that repo's owner from the
  Guide's table. A name that is not a repo in the table (for example `installer`) has no mapping and is not a routable team.
- **A redirect narrows a real but broad team.** A handle that is a key of `preferredRedirects` is a real team that is too broad
  for this work: name that key's value instead.
- **An unverified handle is a ping to nobody.** Never infer a handle from a repo, directory or component name.
- **There is no default owner.** Do not fall back to a convenient team because it sounds like infrastructure. When no team maps, the
  owner is "No identified owner" and the report says what evidence is missing. A VMR build-orchestration setting (a timeout, node
  reuse, `repo-projects/Directory.Build.targets`) has no team: route to the person who authored the mechanism, found from the
  pull request that introduced it (`gh search prs`, `gh pr view`).
- **Post only where the channel's rule says.** Read `channels` and `channelRules` in the local routing JSON. If the Guide and the owners disagree, raise it with the Guide's
  maintainers instead of silently diverging.
- **Contacts** are optional: read `runContext.contactsPath`. Its `contacts` object maps team handles to `github` logins and
  `teams` display names. Use a recorded person only; with no entry, name the team alone.

> `Author: 1ES Pipeline Templates` on a task header is static boilerplate, not ownership evidence. Use the tool path (`/__t/1espt/...`),
> a failure-time `aka.ms/1espt` link, or an existing 1ES work item.

### Read the owner off the evidence

Ask which component *produced* the thing that failed, not which leg it ran on.

- `Windows_Pgo_x64` failing in `src/roslyn` restore is a roslyn/VMR problem. "PGO" is the pool.
- Missing `AppData\Local\NuGet\v3-cache` is agent disk state, not the NuGet client team.
- A package whose nuspec floor cannot be satisfied belongs to whoever **produces** the package.
- Signing failures (`MacSignFailed`, `FailDoNotRetry`): category and retry guidance are in the pipeline-investigation skill; the owner
  is the signing service, not the repo that was building.

## 5. Do not create duplicates

**Never file a new issue for something already tracked.** Check the issue evidence, the previous reports and the owner notes your
run context identifies. A finding that has a tracker URL or a named owner is filed and someone owns it. **Today's GitHub search returning
nothing proves nothing**: the tracker may live in another repo, be worded differently, or be an IcM GitHub cannot see.

**Already-owned failures are not action items.** Keep the row and its links, but leave it out of the priority list.

**A team member's own PR is monitor-only** (author association `MEMBER`, `OWNER`, `COLLABORATOR`), even when CI is red: the author
owns it. Exceptions that *are* QB work: `dotnet-maestro[bot]` codeflow PRs, outside contributors needing triage, and failures that
also hit rolling builds.

**When an owner tells you something, write it down the same minute** in `local/owner-notes.md`: the finding name exactly as the
reports spell it, who owns it, the tracker link, their exact words and which builds. The daily round hands that file to the model, so
the correction survives; one that lives only in chat is lost tomorrow.

## 6. Separate independent failures

One build often contains several unrelated failures. Two diagnostics in the same job are usually one failure plus a symptom; split
them only when each has its own terminating diagnostic. Do not merge failures that merely share a build. For cascades ("Artifact not
found") find the root leg, as the pipeline-investigation skill describes.

## 7. Code-flow PRs whose files did not match the source diff

Maestro auto-approves a forward-flow PR only when its `src/<repo>` changes reproduce the product repo's own change. The check is
`CodeflowSourceDiffVerifier` in dotnet/arcade-services. It compares the VMR diff `<target>...<PR head>` under `src/<repo>` with the
product repo diff `<S0>...<S1>`, where S0 is the target branch's `src/source-manifest.json` `commitSha` and S1 is the flowed commit.
It skips the mapping's excludes, `eng/common` (except for arcade), `Version.Details.xml`/`.props`, `global.json` and the
`dotnet-tools` files. It checks `eng/Versions.props`. A file is listed when:

- the PR changed it but the source did not;
- both changed it, but the added and removed lines differ; or
- the source changed it, the PR did not, and the VMR copy is not identical to the source.

The check compares changes, not files, so a listed file is not proof of damage: a file the VMR already had (for example a VMR-only
change that later backflowed) is listed too. A missing comment is not proof of safety either: the check runs only for auto-approve
subscriptions on branches that require approval of the last push, and only while every PR commit came from Maestro. After anyone
else pushes, no new comment appears and the last one goes stale.

**A PR with this comment is not merge-ready until every listed file is explained.** For each file, take four versions:

| Name | Version |
|---|---|
| V0 | `src/<repo>/<file>` at `git merge-base <PR head> origin/<target>` |
| V1 | `src/<repo>/<file>` at the PR head |
| S0 | `<file>` in the product repo at the `src/source-manifest.json` `commitSha` of V0 |
| S1 | `<file>` in the product repo at the `commitSha` of V1 |

Read product files with `gh api "repos/<owner>/<repo>/contents/<file>?ref=<sha>" -H "Accept: application/vnd.github.raw"` and use an
empty file for a missing one. Compute what the flow should have produced, which is the VMR's file plus the source's change:
`git merge-file -p V0 S0 S1 > EXPECTED`. Then classify, first match wins:

| Result | Verdict |
|---|---|
| S1 is Maestro's placeholder ("PLEASE READ ... Please remove this file during conflict resolution") | The product repo merged an unresolved backflow. The VMR is right not to have the file; the repo owner must delete it. Check the flow for side effects: Maestro can treat the placeholder as a rename of a real file. |
| V1 == S1 | Matches the source. |
| `merge-file` is clean and V1 == EXPECTED | The source change applied and VMR-only edits were kept. Fine. |
| `merge-file` conflicts | For each conflicting hunk, find the source commit (`git log S0..S1 -- <file>` in the product repo) and the VMR commit (`git log origin/<target> -- src/<repo>/<file>`). Fine only when the source side came from a backflow ("Source code updates from dotnet/dotnet" or "Backflow from") and the VMR side is a later VMR commit, so the VMR is already ahead. Otherwise it is a real conflict the flow did not surface: route it to the repo owner. |
| `merge-file` is clean and V1 != EXPECTED | **The flow damaged the file.** Find the Maestro commit with `git log origin/<target>..<PR head> -- src/<repo>/<file>` and show how V1 differs from EXPECTED. |

A damaged file blocks the PR. Report the file, the culprit commit and the fix: write EXPECTED to the file in a new commit on the PR
branch (this keeps VMR-only edits; never force-push). Send flow defects to @dotnet/prodconsvcs (General VMR infra in the Repo
Ownership table).

## 8. Authentication problems are findings

If the collector could not read internal data, the report is **partial, not healthy**. Keep the explicit error and its sign-in hint
in the report. Do not confuse it with a CI job's own private-feed 401, which is a build failure to triage. Run the preflight first: it
prints authentication problems, with the sign-in command, before anything else.

## 9. Before you report

- [ ] Fatal classification came from records, not message text
- [ ] The named diagnostic is the one that terminated the leg, and the name matches the previous finding names
- [ ] A timeout names its repo from `logs[].repoBuild`, not from reading the log
- [ ] Every owner handle comes from the ownership/routing JSON, and the destination accepts this failure type
- [ ] Not already tracked, not already owned, not a team member's own PR
- [ ] Independent failures are separate rows
- [ ] No code-flow PR with a "did not match the source diff" note is called green or safe before its files are explained
