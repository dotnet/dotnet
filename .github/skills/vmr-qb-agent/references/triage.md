# VMR QB Triage

How to decide what a failure *is*, who owns it, and whether it needs an action. Run scripts from
`.github/skills/vmr-qb-agent/scripts/`. Most QB mistakes
are not missed failures. They are correctly-spotted failures that were misclassified, misrouted,
renamed, or duplicated, and almost all of them happened because something was **read off a log by
eye** that a script can answer exactly. Where a script exists below, run it; do not eyeball.

**This page holds only what the sources below do not.** It does not restate their rules, so they
cannot go stale here. Read them at the source:

| Topic | Source of truth |
|---|---|
| Fatal vs non-fatal, failure categories, artifact cascades, signing failures | the [pipeline-investigation skill](https://github.com/dotnet/arcade-skills/tree/main/plugins/dotnet-helix/skills/pipeline-investigation) |
| Repo to owner, channels, merge rules, branches | the [VMR QB Guide](https://github.com/dotnet/dotnet/blob/main/docs/VMR-QB-Guide.md) |

## 1. Fatal vs non-fatal: use records, not words

Use the definition in the pipeline-investigation skill (Step 1, "Classify every issue as fatal or
non-fatal"). Two QB-side rules on top of it:

- Build the job map from the **unfiltered** record set. A job that *succeeded* is exactly the proof
  that its failed task was non-fatal, and a filtered "failed records only" list drops those jobs, so
  the lookup silently finds nothing and the task looks fatal. (The collector does this for you.)
- Do not classify by text. 1ES Pipeline Template tasks emit `##[error]` and let the job pass anyway.

## 2. Name the diagnostic that actually fired

Describe a failure by the error that terminated it, not by whichever test or script hosted it.

- A wrapper `MSB3073` / `Bash exited with code 1` / `Darc exit 1` is not a finding. Find the inner
  diagnostic. If the only retained diagnostic *is* the wrapper, say so: name the repo, the exit
  code, and the inner log that was not collected.
- An installer validation leg failing with `NETSDK1045` is a **target-framework failure**, not a
  "dnx regression", even when the console dump is full of `dotnet dnx ... Usage:` noise.
- **One log can hold two unrelated failures.** A single log can contain both a fast-fail in one repo
  (an option that was just removed, say) and a timeout in other repos. Classify each leg by *its own
  first terminating diagnostic and its own duration*. Never merge them, and never let an error line
  in the same file stand in for the cause of the timeout.

### Keep the name stable across days

A recurring failure keeps the **same** name, character for character. Renaming "Azure source-index
subscription missing" to "Source-index Azure subscription missing" makes a three-week-old problem
read as new. Check history ([history.md](history.md)) before inventing a name; if you find the old wording
under a new name, `./history/Invoke-History.ps1 duplicates` will propose the alias.

## 3. Timeouts: which repo was actually running

A leg that "timed out" is a statement about the leg. The owner is whichever repo was running when
the time ran out, and that must come from the log markers, not from which repo logged an error.
`repo-projects/Directory.Build.targets` writes `[HH:mm:ss.ff] Building <repo>` and
`Building <repo>...done`; a repo with a start and no matching done was still running.

```powershell
./triage/Invoke-Triage.ps1 timeout -LogPath <build-log> -TimeLimitMinutes 150   # a .partial log is treated as truncated; add -Json for scripts
```

It calls `triage/timeout.ps1` (`Get-VmrQBRepoBuildState`, `Get-VmrQBTimeoutAttribution`). The daily round uses
the same functions, so the answer here is the answer in the report. `timeout` is the only action
`Invoke-Triage.ps1` has: the daily round collects issue and owner evidence itself (`triage/issue.ps1`), and
`./history/Invoke-History.ps1 duplicates` answers "is this a rename of something we already track".

What the script enforces, because each was a real wrong answer:

| Rule | Why |
|---|---|
| The log must span at least 90% of the job's own time limit to count as a timeout | Legs that ended far short of the limit failed or were cancelled; they did not time out. The limit comes from the job's own "maximum time of N minutes" message; without it the 150-minute VMR default is assumed and the answer says it is uncertain instead of calling the leg a failure. |
| A repo is a suspect only if unfinished AND running at least 10 minutes | When an upstream failure tears a build down, whichever repo started last is left open for moments. Naming it would be wrong. |
| Repos that merely logged errors are listed separately | A repo can log an unrelated error and still have finished; it is not the cause of a timeout. |
| A truncated log is never used | The download cap keeps the head; the timeout evidence is in the tail. |
| "Where the time went" is reported | The repos that really consumed the limit are the ones with the longest first-start to last-finish spans. |

If `reliable` is false, say so in the report and fetch the tail of the log. Do not guess.

## 4. Routing

Routing is split by who owns the fact, so nothing here is a copy:

- **Repo to owner:** read from the Repo Ownership table in the
  [VMR QB Guide](https://github.com/dotnet/dotnet/blob/main/docs/VMR-QB-Guide.md#repo-ownership)
  (itself derived from `.github/policies/assign_ownership.yml`), by the scripts and the model's
  prompt. It is read from disk in this checkout, so it changes when you `git pull`, and a copy is saved once to
  `local/knowledge/guide-ownership.json` at first use and renewed only when the Guide file changes (never fetched on every
  run). When the Guide changes, the skill follows. If the Guide cannot be read, that copy stands in and a note says so; with
  neither, the Guide is fetched from GitHub main once. If even that fails, no owner is guessed: the owner is "No identified
  owner" and the report opens with a note.
- **Everything the Guide does not say:** **`knowledge/routing.json` is the only place in this skill
  where the `test/` owner, redirects, channel rules and category routing are defined.** The tables
  below are generated from it (`./verify/Invoke-Verify.ps1 -SyncDocs`) and a test fails if they drift. The too-broad
  teams and the channel narrowing below are QB additions recorded from owner feedback. A category names its owners
  with `ownerRepos` (repos whose owner the Guide gives, so a Guide change reaches the category) and/or `owners`
  (teams chosen outright).
- **Changing routing is one of two different edits.**
  - *Changing what the skill ships* (a contributor): edit `knowledge/routing.json`, then run
    `./verify/Invoke-Verify.ps1 -SyncDocs`. Never edit the generated tables by hand.
  - *Changing your own routing* (a QB): use `./common/Invoke-Routing.ps1`. It edits
    `local/knowledge/routing.local.json`, your copy of the shipped file (made once, git-ignored, never overwritten), and
    refuses any edit that would leave a registry that does not load. `show` prints the routing in effect;
    `set-category` and `set-channel-rule` record your change (`-WhatIf` previews it). `refresh-ownership` renews the
    saved copy of the Guide's Repo Ownership table on request (`-FromGitHub` fetches the Guide from main).
  - *After you `git pull` and the shipped file changed*, preflight says your copy is behind. `diff` compares your
    copy with the shipped file one entry at a time and says who changed each: `yours` (keep it), `upstream` (only the
    shipped file changed, so `adopt -All` takes it) or `both` (you decide, `adopt -Section <s> -Key <k>` takes the
    shipped version). `acknowledge` then records that you have reviewed it and keeps every edit you made. The base
    snapshot beside your copy (`routing.base.json`, the shipped text you copied) is what makes `upstream` and `yours`
    distinguishable; a copy made before snapshots existed shows `differs` until you acknowledge once.
- **A repo name used as a handle** (`@dotnet/runtime`, `@dotnet/efcore`) is not a team. It resolves to
  that repo's owner from the Guide's table (runtime is `@dotnet/dnr-codeflow`), so there is no alias
  list to keep. A name that is not a repo in the table (for example `installer`) has no mapping
  and is not a routable team: the daily round matches it to the closest team name when one is unique, otherwise keeps
  it as written, and notes it at the top of the report.

<!-- BEGIN GENERATED: routing (do not edit; run ./verify/Invoke-Verify.ps1 -SyncDocs) -->

#### Owners the Guide does not cover (repo owners are in its Repo Ownership table)

| Area | Owner handle |
|---|---|
| VMR-level tests and packaging (`test/`) | `@dotnet/source-build` |
| Anything else | `No identified owner` - name no team. For VMR build-orchestration config (timeouts, node reuse, `repo-projects/Directory.Build.targets`) name the person who authored the mechanism (`git log` / `gh pr view` on the file). |

#### Real but too broad. Use the narrower owner.

| Broad team | Use instead |
|---|---|
| `@dotnet/roslyn` | `@dotnet/roslyn-infrastructure` |

#### Support destinations (no GitHub team, not a Teams channel)

| Name | Link | When |
|---|---|---|
| 1ES support | https://aka.ms/1esptdocs | 1ES Pipeline Template task failures (SBoM, Guardian, 1ES PT Run Task, binary analysis) |

#### Where to post

| Channel | Rule |
|---|---|
| Unified Build Technical and General Discussions | VMR-specific hot issues: all builds broken, a badly stale PR, snap-blocking problems. |
| First Responders | HELIX ISSUES ONLY (Helix test execution, Helix queues/machines). Never route 1ES pipeline-template tasks, SBoM/Guardian tooling, NuGet feed auth, credential providers or agent-pool problems there. |
| .NET Tactics chat | - |
| Preview/RC chat | Verified preview/RC work only. |
| Servicing chat | Servicing-branch coordination. Do not treat every release branch as preview/RC. |

<!-- END GENERATED: routing -->

**An unverified handle is a ping to nobody.** Invented handles have shipped in real reports.
`./verify/Invoke-Verify.ps1 -Online` checks every handle against GitHub, and the report checks
flag a copy/paste message that names one that is not in the registry (in the daily round the flag is a note at the top of
the report; `./daily/Invoke-Report.ps1` refuses it).

**There is no default owner. Do not fall back to a convenient team because it sounds like infrastructure.**
When no team maps, the owner is "No identified owner" and the report says what evidence is missing.
A VMR build-orchestration setting (a timeout, node reuse, `repo-projects/Directory.Build.targets`) has no
team: check who authored the mechanism with `git log` / `gh pr view` on the file and route to that person.

### Where a failure goes

| Failure | Destination |
|---|---|
| Helix test failures (Helix queues, machines, work items) | Teams: First Responders |
| 1ES Pipeline Template tasks (SBoM, Guardian, 1ES PT Run Task, binary analysis) | The support destination in the generated registry block above (`Support - [<name>](<url>)`). **Not** First Responders. |
| VMR-wide incidents (every build broken, snap-blocking) | Teams: Unified Build Technical and General Discussions |
| release/10 servicing coordination | Teams: Servicing chat |
| Verified preview/RC work only | Teams: Preview/RC chat |
| Anything else | **Not** First Responders |

This is narrower than the Guide's Communication Channels table, which lists First Responders for
non-VMR infra issues such as NuGet feed auth. The narrowing is owner feedback recorded in
`routing.json`; if the Guide and the owners disagree, raise it with the Guide's maintainers instead
of silently diverging.

> `Author: 1ES Pipeline Templates` on a task header is static boilerplate, not ownership evidence.
> Use the tool path (`/__t/1espt/...`), a failure-time `aka.ms/1espt` link, or an existing 1ES work item.

### Read the owner off the evidence

Ask which component *produced* the thing that failed, not which leg it ran on.
- `Windows_Pgo_x64` failing in `src/roslyn` restore is a roslyn/VMR problem. "PGO" is the pool.
- Missing `AppData\Local\NuGet\v3-cache` is agent disk state, not the NuGet client team.
- A package whose nuspec floor cannot be satisfied belongs to whoever **produces** the package.
- Signing failures (`MacSignFailed`, `FailDoNotRetry`): category and retry guidance are in the
  pipeline-investigation skill; the owner is the signing service, not the repo that was building.

## 5. Do not create duplicates

**Never file a new issue for something already tracked.** Check history first ([history.md](history.md)). If
the finding has a tracker URL, or status `owned` / `resolved` / `noise` / `wontfix`, it is filed
and someone owns it. **Today's GitHub search returning nothing proves nothing**: the tracker may
live in another repo, be worded differently, or be an IcM GitHub cannot see.

**Already-owned failures are not action items.** If a tracker, IcM or named owner has taken it, keep
its row and links but leave it out of the priority list.

**A team member's own PR is monitor-only** (author association `MEMBER`, `OWNER`, `COLLABORATOR`),
even when CI is red: the author owns it. Exceptions that *are* QB work: `dotnet-maestro[bot]`
codeflow PRs, outside contributors needing triage, and failures that also hit rolling builds.

**When an owner tells you something, record it the same minute** with `./history/Invoke-Record.ps1`
(status, owner, tracker, the exact words, and which builds). One command updates history, the
"links the QB reported" column and the model's context together. A correction that lives only in
chat is lost tomorrow.

## 6. Separate independent failures

One build often contains several unrelated failures. Two diagnostics in the same job are usually
one failure plus a symptom; split them only when each has its own terminating diagnostic. Do not
merge failures that merely share a build. For cascades ("Artifact not found") find the root leg, as
the pipeline-investigation skill describes.

## 7. Authentication problems are findings

If the collector could not read internal data, the report is **partial, not healthy**. Keep the
explicit error and its sign-in hint in the report. Do not confuse it with a CI job's own
private-feed 401, which is a build failure to triage. Run `./daily/Invoke-Preflight.ps1` first; it prints
authentication problems, with the sign-in command, before anything else. How the internal token is
acquired is described in the pipeline-investigation skill and the Guide.

## 8. Before you report

- [ ] Fatal classification came from records, not message text
- [ ] The named diagnostic is the one that terminated the leg
- [ ] A timeout names the repo from `Get-VmrQBTimeoutAttribution`, not from reading
- [ ] The name matches what history calls it
- [ ] The owner handle is in the registry
- [ ] The destination accepts this failure type (First Responders = Helix only)
- [ ] Not already tracked, not already owned, not a team member's own PR
- [ ] Independent failures are separate rows
