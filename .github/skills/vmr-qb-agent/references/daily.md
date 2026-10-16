# VMR QB Daily Round

The Quarterback watches VMR build health and code flow, routes failures to owners, and merges flow
PRs. This guide runs that round and produces the reports. For classifying a single failure see
[triage.md](triage.md); for recurrence lookup see [history.md](history.md).

## The rule

Scripts collect build logs and pipeline timelines, assign finding numbers, validate reports,
record history and send mail. The model writes reports from the supplied inputs and may perform
the read-only Maestro and GitHub checks explicitly required by the reporting workflow.
It must not re-query Azure DevOps or acquire credentials (the one rule in [SKILL.md](../SKILL.md)).
Every item in the table below exists because a hand-run version
of this round got that step wrong.

Run everything from `.github/skills/vmr-qb-agent/scripts/` in PowerShell 7 on Windows.

## Run it

```powershell
./daily/Invoke-Preflight.ps1                           # every day, FIRST. Authentication problems print first.
./daily/Invoke-Daily.ps1                               # the round. Add -SkipEmail to rehearse.
./daily/Invoke-Report.ps1 -ReportPath <file>           # validate a finished overall report
```

Recording what an owner told you is the history workflow ([history.md](history.md)); classifying a
failure is triage ([triage.md](triage.md)). The round's scripts all live in `scripts/daily/`:

```
daily/Invoke-Preflight.ps1   access check            -> preflight.ps1
daily/Invoke-Daily.ps1       the round               -> preflight.ps1, evidence.ps1, report.ps1, gate.ps1,
                                                        prompts.ps1 (what each stage is told), contract.ps1
                                                        (the report format), workspace.ps1 (the nested model's
                                                        run directory), ../triage/ (timeout.ps1, issue.ps1),
                                                        ../history/ (history.ps1, store.ps1), ../common/core.ps1
daily/Invoke-Report.ps1      validate a saved report -> contract.ps1, report.ps1, gate.ps1, ../history/history.ps1
```

If `./daily/Invoke-Preflight.ps1` reports an authentication problem, **show it to the user verbatim,
first**. The round still runs: without internal access it reports the internal builds as "unknown",
never "healthy", and the report and the email open with the error and its fix. The usual fix is
`az login --allow-no-subscriptions`. A missing GitHub sign-in (`gh auth login`) is the one
authentication problem that stops the round.

## What stops the round, and what only adds a note

The round's job is to produce the report. **Only four things stop it** (exit code 2, with the fix named and, when email is on,
a short notice email): PowerShell older than 7.2, a state folder it cannot write, no GitHub sign-in, and a config holding your
email address that Git tracks or does not ignore (the next commit could publish it). Everything else is a
**run note**: one line at the top of the overall summary and of the email, with how to fix it, internal authentication always
first. The notes are also on the console and in the run log (`logs/run-<date>-<time>.json`).

| Something is stale, missing or wrong | What the round does |
|---|---|
| Internal sign-in (`az login`) fails, or its token expires mid-run | The collector gets a fresh token once on a 401 before giving up. If internal access is still unavailable, the public builds and code flow still run and the internal builds are **UNKNOWN**, never healthy; the notes open with the error and the fix, and the run is `partial` |
| `routing.local.json` is behind the shipped file, or a category or redirect names a team the Guide no longer lists | The round uses it. An owner is matched to the closest team (the unique one that shares the most words with the wanted name); with no unique match the handle is kept as written and marked as not matching. Every match is a note. An owner never fails a run, and a finding with no owner says "No identified owner" |
| The Guide is missing, or its `## Repo Ownership` heading changed | The ownership copy saved at first use (`local/knowledge/guide-ownership.json`) stands in. It is made from the Guide in this checkout, reused every day, and renewed only when the Guide file changes (or on `./common/Invoke-Routing.ps1 refresh-ownership`). Only when there is neither a Guide nor a copy is GitHub main fetched, once |
| `qb-config.local.json` is not valid JSON or has a bad address | No email, and a note says what to fix to get email |
| `issue-history.json` or `triage-context.local.json` cannot be read | Skipped for this run and left as it is, with a note. Today's findings are not recorded in a history that cannot be read; the saved reports can rebuild it |
| The Copilot CLI is not installed, or the compact evidence for the model could not be built | The model reports are skipped with the reason, and the scripts' **evidence digest** (`reports/evidence-digest-<date>.md`: the problem builds with links, the failed jobs and tasks, the first error, what each pipeline could and could not see, and what each report did) is saved and emailed instead of the narrative |
| The flow-analysis skill is missing | The code-flow report is skipped with that reason. Build health is still written (without the pipeline-investigation guidance when that skill is missing too) |
| A pinned `model` is refused by the CLI | The run is repeated with the CLI's default model and a note says so. A model run that failed for another reason is tried once more; one that ran out of time is not |
| A report check finds a problem (format, handles, BH numbers, history) | The report is kept as written, after one repair pass, and every problem is its own red note at the top (one bad row never hides another). The repair is taken only when it leaves fewer problems than the original. `./daily/Invoke-Report.ps1` is the manual check and still refuses a report |
| The model run ends abnormally (the time limit, a CLI error on the way out) after it already wrote its final report | That report is used, with a red note saying how the run ended. A run that wrote no report is tried once more (not after a time limit); a second failure makes that one report a failed result and the round goes on |
| One stage (build health, code flow, overall summary) throws while the round is building its prompt or reading its context | That report is a failed result with the reason; the other reports, the digest and the email are unaffected |
| `-UseExistingReports` and a saved report is missing, empty or has another day's title | A missing or empty report is a failed result with the reason (the digest stands in for a missing overall report); another day's title is kept with a note. The round does not stop |
| The email body cannot be formatted, or an attachment is missing | The email goes out anyway: a plain body, or without the missing attachment, and a note says so |
| Outlook is not available | The reports are saved. A send that was refused before anything was submitted is tried again (3 attempts, 30 seconds apart); if it still fails, a note says so and the run is not "delivered" (exit 1) |

**Exit codes.** `0`: a report (or the digest) was delivered, whatever the notes say; with email on, it was also sent. `1`: nothing
was delivered, the email could not be sent, or the run crashed unexpectedly (the log names the stage, and a notice is emailed with
whatever was generated). `2`: preflight stopped the run. `3`: another run for the same day is in progress.

## What is scripted and what is not

| # | Step | Done by | Where | What stops a bad result |
|---|---|---|---|---|
| 1 | Sign-in, tools, identity, plugins | script | `./daily/Invoke-Preflight.ps1` | exits 2 only for PowerShell, the state folder, the GitHub sign-in or a config holding your email address that Git tracks or does not ignore; any other problem is a warning the run turns into a note; auth errors first with the fix command |
| 2 | Evidence for both pipelines: one 24-hour cutoff, pagination, branch scope, rolling vs PR, fatal vs non-fatal from timeline records, root logs, issue and owner evidence. **One raw log per distinct failure, not per build:** every problem build's timeline is read, failures are grouped by task and timeline message (build ids, versions, GUIDs and hashes taken out; within one pipeline and branch, all pull requests counting as one branch), the log is fetched from the first build that has the failure, and every other build keeps its timeline message and names the build whose log stands for it (`sameFailureSummary`). A wrapper message ("Bash exited with code 1") or no message cannot be grouped, so those logs are fetched. Builds, issues, PRs and branches are observed live and timestamped; the collector copies the Guide and the ownership policy from this checkout with their path, SHA-256 and commit, without fetching replacements. Ownership initialization separately supports the saved-copy and GitHub fallbacks described below; standing team context (the source-build contact) comes from the local roster | script | `./daily/Invoke-Daily.ps1` (collector) | coverage alerts; `internal.status`; nothing is "healthy" when unreadable; a Guide or policy missing from the checkout is an explicit error |
| 3 | Which repo timed out | script | `./triage/Invoke-Triage.ps1 timeout` (the triage entry's only action) | refuses truncated logs; separates running repos from error-only repos |
| 4 | Build-health findings | **model**, tool-restricted | nested `copilot` run reading a compact digest | every quote must be verbatim from retained evidence; wrapper errors are not findings; every problem build covered; owner handles must exist |
| 5 | Code-flow report | **model**, tool-restricted | same | stated observation times; commits-behind, not build-delta |
| 6 | Overall summary and actions | **model**, tool-restricted | same | six columns, evidence-first actions, 30-word limit, named contact beside each team |
| 7 | `BH #N` numbering | script | `daily/report.ps1` | derived from the visible Build Health list, never counted by the model |
| 8 | Report checks | script | the same checks at generation, reuse and `./daily/Invoke-Report.ps1` | format, unknown handles, First Responders for non-Helix, BH disagreement, placeholders, novelty and table placement vs history. In the round, what a check finds is a note at the top of the report and the report is kept; `Invoke-Report.ps1`, the manual check, refuses |
| 9 | History replay | script | `./history/Invoke-History.ps1 replay` | idempotent; a report cannot overwrite an operator's facts |
| 10 | Email | script | `./daily/Invoke-Daily.ps1` | matches the Outlook account to one of the recipient addresses, saves to its Sent folder, writes a receipt, will not double-send; retries a send that was not submitted |

Each nested stage starts in its own run directory, `local/run/<timestamp>/` (the newest ten are kept). It holds the
stage's instructions (`AGENTS.md`), copies of the reference documents it is told to read (build health:
`triage.md`; overall summary: `report-contract.md` and `triage.md`, whose routing tables show your effective
routing, local additions included) and its task documents, so nothing it must read depends on an implicit grant and
a second run cannot rewrite the first one's instructions. Build health and the overall summary receive the same
ownership packet, built from the Guide in this checkout; the validators check handles against the same registry.

The model is trusted with steps 4-6 only: choosing words, summarizing, and deciding which finding an
action belongs to. Everything it produces is checked by steps 7-8, and what they find is shown at the top of the report.

"Tool-restricted" is enforced by the CLI's permissions, not by the prompt: file writes are denied, the
only shell commands allowed are read-only `gh` subcommands (`gh pr view`, `gh search issues`, and so
on; `gh api` is excluded because its permission matches the subcommand and would also allow
`-X POST`: checked on Copilot CLI 1.0.89, where `--allow-tool='shell(gh api)'` let the model run
`gh api -X POST markdown` and without it the call was denied). A command is allowed only as one plain command: a pipe, `;` or `2>&1` around it makes the call compound and it is denied (a live run lost its merge-gate checks that way), so the model's instructions tell it to run each `gh` command alone and to retry a denied one as a plain command. Only read tools from the Helix,
AzDO, Maestro and GitHub servers are granted. Anything else is denied. Pin the model with `"model"` in `local/qb-config.local.json`: without it the Copilot CLI's default
is used and can change under you, so the email names the model that actually wrote each report.

## Prerequisites

| Need | Why | Check |
|---|---|---|
| PowerShell 7.2+, Windows | scripts, nested processes | preflight |
| `az login --allow-no-subscriptions` with dnceng/internal access | internal pipeline evidence | preflight (token + a real API read). A warning, not a stop: without it the internal builds are reported UNKNOWN |
| `gh auth login` (authorize the dotnet org for SSO; the `read:org` scope lets it read GitHub team members for the contact roster) | issue/PR evidence, team verification, team roster | preflight. The one sign-in the round cannot do without (exit code 2) |
| The Copilot CLI (`copilot` on PATH) and the `dotnet-arcade-skills` plugin (`dotnet-helix`, `dotnet-codeflow`), found under `COPILOT_HOME` or `~/.copilot/installed-plugins` in any marketplace folder (or at `pluginRoot` in `local/qb-config.local.json`) | the model steps (build health, code flow, overall summary). Not needed for `-CollectOnly`, `-UseExistingReports`, validation, history, triage or routing. Without the CLI the round saves and sends the evidence digest instead of the narrative | preflight |
| Classic Outlook, signed in as one of the recipients | email only: needed when `recipient` is set in `local/qb-config.local.json` (COM; "New Outlook" has no COM). With no recipient nothing is emailed and this is not checked | preflight |

**One execution backend.** `Invoke-Daily.ps1` runs the model stages by starting the Copilot CLI as a nested process, with the per-stage tool restrictions and run directory described above. That is the only backend the script has, and it works the same whichever host started the script. An agent in another host (the Copilot app, say) can follow this guide by hand and write the narrative itself, but then none of those restrictions apply and nothing validates the result except `./daily/Invoke-Report.ps1`, so it is not equivalent and is not offered as a way to run the round.

Everything local to you (config, history, reports, logs, evidence, the team roster, verified findings) lives in
`local/` next to this skill, which is **git-ignored**, so an email address and triage notes are never published.
Set `$env:VMR_QB_HOME` to use a different folder.

## When a run goes wrong

| Symptom | Do this |
|---|---|
| Preflight stops the run (exit code 2) | Only PowerShell, the state folder, the GitHub sign-in or a config holding your email address that Git tracks or does not ignore can. Fix what it names, authentication first |
| The overall summary opens with "Run notes" | Each note says what was stale, missing or wrong and how to fix it; internal authentication is first. The same list is in `logs/run-<date>-<time>.json`. The report is complete for what could be seen: nothing unreadable is shown as healthy |
| Evidence collection failed or was partial | The report says so. Re-run later; do not edit around it. |
| Generation failed (build-health or overall), or an evidence digest arrived instead of the overall report | Fix what the notes name (the Copilot CLI, a skill, the evidence), then `./daily/Invoke-Daily.ps1 -UseCollectedEvidence -SkipCodeFlow` re-uses the dated evidence and the successful code-flow report. Add `-SkipBuildHealth` to regenerate only the overall report, and `-SkipEmail` to look at the result before it is sent; `./daily/Invoke-Daily.ps1 -UseExistingReports -Force` then sends the saved reports |
| A correction from an owner must reach the regenerated report | `-OperatorContextPath <file>`: a short text file of scoped, verified corrections for both generations |
| BH numbers disagree between sections | `./daily/Invoke-Daily.ps1 -UseExistingReports -RefreshReportReferences` repairs them from the visible list without regenerating |
| Rehearse without emailing | `-SkipEmail`. Add `-OutputRoot <dir>` to keep everything apart: the rehearsal writes its own reports, logs, artifacts, run folder and a private copy of the history under that folder, replays only into that copy, and never touches a delivered report, receipt or the real history. It reads (never writes) your settings, local knowledge and reported links, and does not build the roster |
| Evidence only | `-CollectOnly` (exit 1 when there are coverage alerts) |
| Check any report by hand | `./daily/Invoke-Report.ps1 -ReportPath <overall-summary-date.md> -BuildHealthPath <build-health-date.md>` |
| Scheduled run fired twice | A sent receipt makes the second run a no-op unless `-Force` |
| Two runs for the same day overlap (a scheduled run and a manual one) | The second stops at once with exit code 3 and changes nothing: each state and date has one run at a time, and a rehearsal (`-OutputRoot`) has its own state, so it never blocks or is blocked |
| "Not sending: this email already has a claim" | The send is claimed atomically before Outlook is asked, so a day is never mailed twice by accident. If the message says it was already sent, you are done; if it says an earlier send left no receipt, it may have been delivered: check your Sent folder, then `-Force` sends again. A send that fails before anything is submitted is tried again, and then gives its claim back. The claim never ages out: a day is not re-sent until a QB asks (`-Force`) |

## Teaching it what you learn

When an owner tells you something, record it that minute. One command updates history, the
"links the QB reported" column and the model's context, and refuses typos and non-existent teams:

```powershell
./history/Invoke-Record.ps1 -Finding '<exact finding name>' -Status owned -Owner '@dotnet/<team>' `
    -SaidBy '<who said it>' -Note '<what they said, in their words>' `
    -Builds <build id> -Match '<regex for the issue category>' -AddContext -AppliesTo '<when this applies, and when it does not>'
```

Repo-to-owner and the list of real teams are read from the Repo Ownership table in the VMR QB Guide
(`docs/VMR-QB-Guide.md` in this checkout). If the Guide cannot be read, the saved local ownership
copy is used. If neither is available, the script fetches the Guide from GitHub main and saves an
ownership copy for future runs. If all sources fail, the report identifies owners as unknown and
includes a warning.
A repo name written as a handle (such as `runtime`) resolves to that repo's owner from the same table; there is
no alias list. Channel rules, redirects and category routing live in `knowledge/routing.json` and nowhere else. Run `./verify/Invoke-Verify.ps1 -SyncDocs`
after changing it; the model's instructions and [triage.md](triage.md) are generated from the two.

## The report contract

See [report-contract.md](report-contract.md). In short: a Top-actions list, a
main table of findings that need action (ordered like the list), a Monitor-only table for anything
history records as `owned`, `resolved`, `noise` or `wontfix`, and a per-finding appendix. Every action is
`Where: ... <br>Copy/paste: "@dotnet/<team> @<Named Person>, Please check the build failure. <what
failed>: <link>"`. Qualifications such as "cause unconfirmed" must survive into the report.

## Rules

The rules that apply to every task (read-only by default, never force-push a codeflow PR, stable finding names, no hidden
gaps, fatal vs non-fatal) are in [SKILL.md](../SKILL.md#rules-that-apply-to-every-task) and are not repeated here. One
daily-specific note: the nested model cannot merge, approve, comment or retry (see the permissions note above); *you* can,
so read-only is yours to keep.

Merging flow and re-bootstrap PRs is the VMR QB Guide's business, not this skill's, which never merges: see
[Merging Code Flow PRs](https://github.com/dotnet/dotnet/blob/main/docs/VMR-QB-Guide.md#merging-code-flow-prs) and
[Merging Re-bootstrap PRs](https://github.com/dotnet/dotnet/blob/main/docs/VMR-QB-Guide.md#merging-re-bootstrap-prs).

## Knowledge pack

`knowledge/` carries what previous rotations learned. It is a starting point, not scripture:
handles change, issues get fixed, ownership moves. `./verify/Invoke-Verify.ps1 -Online` re-checks
every handle, contact and linked issue against GitHub. Correct what is stale; the next QB inherits
whatever you leave.

| File | Holds |
|---|---|
| `routing.json` | The only definition of aliases, channels and category routing (repo owners come from the Guide) |

The skill ships only `routing.json`. What a QB learns is local and lives in `local/knowledge/` (git-ignored):

| File | Holds |
|---|---|
| `triage-context.local.json` | One QB's verified per-issue findings, each with how it was established. `./history/Invoke-Record.ps1 -AddContext` adds yours, and the handoff file carries them |
| `team-contacts.json` | Names people. `./common/Invoke-Contacts.ps1` builds it from the guide's Repo Ownership table and each GitHub team's maintainers, and every later run reuses it. GitHub is asked again only for a team a pulled guide adds, or with `./common/Invoke-Contacts.ps1 -Refresh`. Entries you edit by hand are kept |
| `routing.local.json` | Your own copy of `routing.json`, made once on first use (never overwritten afterwards), with `routing.base.json` beside it (the shipped text it was copied from). Change routing here with `./common/Invoke-Routing.ps1` (it refuses an edit that would not load): a category with the same id replaces the shipped one, `channelRules` merge by key. Incident links, corrections and open questions go here, never in the shipped file. A pull does not update it; `./daily/Invoke-Preflight.ps1` warns (a warning only: the round still runs) when the shipped file has changed since the copy, and `./common/Invoke-Routing.ps1 diff` / `adopt` / `acknowledge` take in the change without losing your edits |
| `guide-ownership.json` | The Guide's Repo Ownership table as first read, made once and renewed only when the Guide file changes (a pull) or on `./common/Invoke-Routing.ps1 refresh-ownership`. It stands in when the Guide cannot be read, so the round never fetches policy on every run |
| `issue-aliases.json` | Optional bulk list of finding-name aliases; `./history/Invoke-History.ps1 alias` stores them in the history itself |

A history from the previous QB goes in `local/handoff/`; `./common/Invoke-Setup.ps1` points to it (setup is optional), and `./common/Invoke-Setup.ps1 -ImportHandoff <file>` imports it, including the routing they added (yours wins on conflict; see [history.md](history.md)).
