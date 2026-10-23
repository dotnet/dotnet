# VMR QB Daily Round

The Quarterback watches VMR build health and code flow, routes failures to owners, and merges flow
PRs. This guide runs that round and produces the reports. For classifying a single failure see
[triage.md](triage.md); the shape of the reports is [report-contract.md](report-contract.md).

## The rule

Scripts collect build logs and pipeline timelines, build a compact digest, run the model, and save and
send what it wrote. The model writes the reports from that digest by the rules in this folder
([report-contract.md](report-contract.md), [runner-instructions.md](runner-instructions.md),
[triage.md](triage.md)) and may do the read-only Maestro and GitHub checks the reports call for. It
must not re-query Azure DevOps or acquire credentials (the one rule in [SKILL.md](../SKILL.md)).
Nothing validates what the model wrote; a QB reads a report before acting on it. What the scripts could
not read is a run note at the top of the overall report.

Run everything from `.github/skills/vmr-qb-agent/scripts/` in PowerShell 7 on Windows.

## Run it

```powershell
./daily/Invoke-Preflight.ps1     # every day, FIRST. Authentication problems print first.
./daily/Invoke-Daily.ps1         # the round. Add -SkipEmail to look at the reports first.
```

Other switches: `-Date <yyyy-MM-dd>` (names the reports, and picks the saved ones for `-UseExistingReports`; the evidence window is
always the 24 hours before the run); `-CollectOnly` (evidence only); `-UseExistingReports` (send the reports already saved for the
date: no collection and no model. They are not rewritten: the run notes saved with them stay as they are, and the email says it
was re-sent from saved reports instead of claiming a complete run).
There is no resume: after a failed round, run it again.

If `./daily/Invoke-Preflight.ps1` reports an authentication problem, **show it to the user verbatim,
first**. The round still runs: without internal access it reports the internal builds as "unknown",
never "healthy", and the report and the email open with the error and its fix. The usual fix is
`az login --allow-no-subscriptions`. A missing GitHub sign-in (`gh auth login`) is the one
authentication problem that stops the round.

## What stops the round, and what only adds a note

**Four things stop the round before it starts** (exit code 2, with the fix named and, when email is on, a
short notice email): PowerShell older than 7.3, a state folder it cannot write, no GitHub sign-in, and a
config holding your email address that Git tracks or does not ignore (the next commit could publish it). The PowerShell, state
folder and config checks run on every invocation, `-UseExistingReports` and `-SkipEmail` included; the GitHub and Azure
sign-in checks run only when evidence is collected.

**A failed round** (a model run that fails: the time limit, a CLI error, no Copilot CLI; or a crash) stops
the round: exit code 1 and, when email is on, one short email says it failed and at which stage, with
whatever reports were already made attached. The Copilot CLI and its plugins are documented prerequisites
and are not checked. Each model run has a 25-minute limit and a 250 MB output cap; both runs that overlap are held to them,
and one that exceeds either fails the round and is stopped.

**Everything else is a run note**: one line at the top of the overall summary and of the email, with how
to fix it, internal authentication always first.

| Something is wrong | What the round does |
|---|---|
| Internal sign-in (`az login`) fails, or its token expires mid-run | The collector gets a fresh token once on a 401. If internal access is still unavailable, the public builds and code flow still run and the internal builds are **UNKNOWN**, never healthy; the notes open with the error and the fix, and the run is `partial` |
| A source cannot be read (an API, the issue search) | The report says it was not read, as a note |
| A category or redirect in `routing.local.json` names a team the Guide does not list | The file is used as it is and its handle is kept as written; nothing compares it with the Guide, so update your copy when teams change |
| There is no ownership copy yet and the Guide is missing | Owners are shown as unidentified, with a note; nothing is fetched |
| `qb-config.local.json` is not valid JSON or has a bad address | No email, and a note says what to fix |
| `-UseExistingReports` and a saved report is missing | The round fails |
| Outlook is not available | The reports are saved; the send fails the round (exit 1) |

**Exit codes.** `0`: the reports were delivered (and, with email on, sent), whatever the notes say. `1`: the
round failed. `2`: preflight stopped it.

## What the round does

| # | Step | Done by |
|---|---|---|
| 1 | Sign-in, tools, identity | script: `./daily/Invoke-Preflight.ps1` |
| 2 | Evidence for both pipelines: one 24-hour cutoff, pagination, branch scope, rolling vs PR, every problem build's timeline, raw logs from one representative build per matching failure category (see README), and the GitHub issues that mention the failed builds | script: `daily/evidence.ps1`, `triage/issue.ps1` |
| 3 | Which repo timed out | script: `triage/timeout.ps1`, in the digest |
| 4 | Build-health report | **model**, tool-restricted |
| 5 | Code-flow report | **model**, same; started together with build health |
| 6 | Overall summary and actions | **model**, same |
| 7 | Save, run notes at the top, optional email | script |

Each nested stage starts in the current evidence directory. The launcher supplies the unmodified `SKILL.md` text and
identifies the stage, evidence file, and [runner-instructions.md](runner-instructions.md). The model reads those instructions and the
JSON/data paths in the evidence's `runContext` directly. Computer time and timezone are evidence metadata. Read access
is limited to the needed data and shared instructions, not the private email configuration.

"Tool-restricted" is enforced by the CLI's permissions, not by the prompt: file writes are denied, the
only shell commands allowed are read-only `gh` subcommands (`gh pr view`, `gh search issues`, and so on;
`gh api` is excluded because its permission matches the subcommand and would also allow `-X POST`).
A command is allowed only as one plain command: a pipe, `;` or `2>&1` around it makes the call compound
and it is denied, so the model's instructions tell it to run each `gh` command alone. Only
read tools from the Helix, AzDO, Maestro and GitHub servers are granted. Anything else is denied. Pin
the model with `"model"` in `local/qb-config.local.json`: without it the Copilot CLI's default is used
and can change under you, so the email names the model that actually wrote each report.

## Prerequisites

See [README.md](../README.md#prerequisites). Everything local to you (config, reports, logs, evidence,
your notes) lives in `local/` next to this skill, which is **git-ignored**. Set `$env:VMR_QB_HOME` to
use a different folder.

## When a run goes wrong

| Symptom | Do this |
|---|---|
| Preflight stops the run (exit code 2) | Fix what it names, authentication first |
| The round failed (exit code 1) | The console and the failure email name the stage and the error; fix it and run the round again |
| The overall summary opens with "Run notes" | Each note says what was stale, missing or wrong and how to fix it; internal authentication is first. The report is complete for what could be seen: nothing unreadable is shown as healthy |
| Evidence collection was partial | The report says so. Re-run later; do not edit around it |
| Evidence only | `-CollectOnly` (exit 1 when there are coverage alerts) |
| Rehearse without emailing | `-SkipEmail`. Set `VMR_QB_HOME` to a scratch folder to keep everything apart from your real `local/` |

## Teaching it what you learn

When an owner tells you something, append a dated line to `local/owner-notes.md`: the finding name as the
report shows it, the owner, what they said in their words, and a link. The round copies the file where
the model can read it and the instructions say to honor it: it decides New Issue (Y/N) and Monitor-only
placement. To ask "have we seen this before?", search the saved reports:
`Select-String -Path local/reports/overall-summary-*.md -Pattern '<words from the finding>'`.

Repo-to-owner and the list of real teams come from the Repo Ownership table in the VMR QB Guide
(`docs/VMR-QB-Guide.md` in this checkout). The first run copies that table to
`local/knowledge/guide-ownership.json` and uses the copy from then on; nothing is fetched. If there is no
copy and the Guide cannot be read, the report identifies owners as unknown and includes a warning. A repo
name written as a handle (such as `runtime`) resolves to that repo's owner from the same table. Channel
rules, redirects and category routing live in `knowledge/routing.json` (copied once to
`local/knowledge/routing.local.json`).

An optional `local/knowledge/team-contacts.json`, which you edit by hand, names one person beside each
team ping: `{ "contacts": { "@dotnet/<team>": { "github": "<login>", "teams": "<Teams display name>" } } }`.
Without it the round pings the team alone.

## The report contract

See [report-contract.md](report-contract.md). In short: a Top-actions list, a main table of findings that
need action (ordered like the list), a Monitor-only table, and a per-finding appendix. Every action is
`Where: ... <br>Copy/paste: "@dotnet/<team> @<Named Person>, Please check the build failure. <what
failed>: <link>"`. Qualifications such as "cause unconfirmed" must survive into the report.

## Rules

The rules that apply to every task (read-only by default, never force-push a codeflow PR, stable finding
names, no hidden gaps, fatal vs non-fatal) are in [SKILL.md](../SKILL.md#rules-that-apply-to-every-task)
and are not repeated here. One daily-specific note: the nested model cannot merge, approve, comment or
retry (see the permissions note above); *you* can, so read-only is yours to keep.

Merging flow and re-bootstrap PRs is the VMR QB Guide's business, not this skill's, which never merges: see
[Merging Code Flow PRs](https://github.com/dotnet/dotnet/blob/main/docs/VMR-QB-Guide.md#merging-code-flow-prs) and
[Merging Re-bootstrap PRs](https://github.com/dotnet/dotnet/blob/main/docs/VMR-QB-Guide.md#merging-re-bootstrap-prs).
