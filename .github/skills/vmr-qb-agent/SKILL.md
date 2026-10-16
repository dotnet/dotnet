---
name: vmr-qb-agent
description: >-
  Supports the .NET VMR Quarterback (QB) rotation for dotnet/dotnet.
  Use when preparing daily QB reports, assessing pipeline health, classifying
  build failures, identifying owners, reviewing recurring issues, or recording
  owner updates. Scripts collect build logs, pipeline timelines, issue details,
  and code-flow status; validate reports; maintain issue history; and optionally
  deliver reports by email. The model generates report narratives from the
  collected information. For detailed investigation of individual CI or Helix
  failures, use ci-analysis, helix-investigation, or pipeline-investigation.
  For source-build failures involving prebuilt dependencies, use
  source-build-investigation.
---

# VMR QB Agent

The Quarterback watches VMR build health and code flow, routes failures to owners, and merges flow
PRs. This skill automates most of that round.

**This file is the entry point.** Read the guide for the workflow you are in; each guide says which
script to run, and that script calls the other scripts of its own folder. Run every script from
`.github/skills/vmr-qb-agent/scripts/` in PowerShell 7 on Windows.

| Workflow | Read | Run | Its scripts |
|---|---|---|---|
| The daily round: check access, collect evidence, write and validate the reports, record, email | [references/daily.md](references/daily.md) | `./daily/Invoke-Preflight.ps1` first, then `./daily/Invoke-Daily.ps1`; `./daily/Invoke-Report.ps1` to validate a finished report | `scripts/daily/` |
| Classify a failure: fatal or not, what to call it, who owns it, which repo timed out, whether to file | [references/triage.md](references/triage.md) | `./triage/Invoke-Triage.ps1 timeout -LogPath <log>` (its one action: which repo was building when a leg timed out). Owner and issue evidence is collected by the daily round; whether it is a duplicate is `./history/Invoke-History.ps1 duplicates` | `scripts/triage/` |
| "Have we seen this before?", record what an owner told you, hand history to the next QB | [references/history.md](references/history.md) | `./history/Invoke-History.ps1 <action>`, `./history/Invoke-Record.ps1` | `scripts/history/` |
| The shape of the reports and the action text | [references/report-contract.md](references/report-contract.md) | checked by `./daily/Invoke-Report.ps1` | `scripts/daily/` |
| Optional setup, the team roster, your own routing | this file; routing in [references/triage.md](references/triage.md) | `./common/Invoke-Setup.ps1`, `./common/Invoke-Contacts.ps1`, `./common/Invoke-Routing.ps1` | `scripts/common/` |
| Maintainers: is this skill fit to publish? | [README.md](README.md) | `./verify/Invoke-Verify.ps1` | `scripts/verify/` |

Nothing has to be set up first. By default reports are saved and **no email is sent** (see
[README.md](README.md) to get them by email).

If `./daily/Invoke-Preflight.ps1` reports an authentication problem, **show it to the user verbatim,
first**. The round still runs: without internal access it reports the internal builds as "unknown",
never "healthy", and the report and the email open with the error and its fix. The usual fix is
`az login --allow-no-subscriptions`. A missing GitHub sign-in (`gh auth login`) is the one
authentication problem that stops the round.

## The one rule

**Scripts collect build logs and pipeline timelines; the model uses those inputs to write reports.**
The model may also perform the read-only Maestro and GitHub checks explicitly required by the
reporting workflow. It must not re-query Azure DevOps or acquire credentials; scripts own that collection.
Scripts assign finding numbers, validate reports, record history and send mail; the model does not.
Each of those has a script and a check, and what a check
finds is written at the top of the report, where it is read, but never costs the report.
Earlier hand-run versions of this round produced confident reports from partial evidence, pinged
teams that do not exist, and numbered the same finding differently in two places.

## Rules that apply to every task

- **Read-only by default.** Do not merge, approve, close, comment, trigger subscriptions or retry
  anything unless explicitly asked.
- **Never force-push a codeflow PR.** PRs titled `Source code updates from ...` belong to
  `dotnet-maestro[bot]`.
- **Routing has two sources, never copies.** Repo-to-owner is read from the Repo Ownership table in
  `docs/VMR-QB-Guide.md` (saved once to `local/knowledge/guide-ownership.json`, which stands in when the Guide cannot be read),
  and so is the real-team list; a repo name written as a handle (such as `runtime`)
  resolves to that repo's owner from the same table, with no alias list. Channel rules, redirects and
  category routing are defined only in `knowledge/routing.json`. An unverified handle is a ping to nobody. Two different edits:
  a contributor changing what the skill ships edits `knowledge/routing.json` and runs `./verify/Invoke-Verify.ps1 -SyncDocs` (the
  tables in `references/triage.md` are generated); a QB changing their own routing uses `./common/Invoke-Routing.ps1`, which edits
  `local/knowledge/routing.local.json`, refuses anything that would not load, never overwrites the file, and reconciles a changed
  shipped file without losing the QB's edits (`diff`, `adopt`, `acknowledge`).
- **Keep finding names stable across days.** A renamed finding reads as new and hides a recurrence.
  Check history before inventing a name.
- **Fatal vs non-fatal** is defined by the pipeline-investigation skill, from timeline records and never from message
  text; [references/triage.md](references/triage.md) adds only the two QB-side rules. A non-fatal finding is monitor-only.
- **Do not hide a gap.** If a source could not be read, the report must say it was not read.
- **Do not lose a report to a stale input.** Only PowerShell older than 7.2, a state folder that cannot be written, a
  missing GitHub sign-in and a config holding your email address that Git tracks or does not ignore stop the daily round
  (exit 2). Everything else is a **run note**, written at the top of the
  overall report and the email with how to fix it, internal authentication first: internal sign-in, a routing copy behind
  the shipped file, an owner the Guide no longer lists, a bad config, a damaged history or triage context, a
  missing Copilot CLI or skill, a pinned model the CLI refused, a failed report check. Without the model, the scripts'
  evidence digest is saved and sent instead. Exit 0 means a report (or the digest) was delivered, whatever the notes say.
- **Everything local to you lives in `local/`, and `local/` is git-ignored.** Settings (your email, if you want email), history, reports, logs, evidence, the team roster, verified findings, aliases and handoff files are one QB's own, so none of it is published. `./verify/Invoke-Verify.ps1` fails if `local/` is not ignored, or if Git tracks or has staged anything in it.

## Layout

| Path | Holds |
|---|---|
| `references/` | The guides above, the report contract, and the nested model's instructions |
| `scripts/` | One folder per workflow, each with its entry scripts (`Invoke-*.ps1`) and the libraries only they use: `daily/` (preflight, evidence collector, prompts, report contract and gate, the per-run workspace for the nested model, the round), `triage/` (timeout attribution, issue evidence), `history/` (issue store, search, record, handoff), `verify/` (knowledge check, publish gate), `common/` (paths, config, routing and its local editing, team roster, setup), and `tests/` (`scripts/tests/Invoke-VmrQBTests.ps1`) |
| `knowledge/` | `routing.json`, the only shipped data |
| `local/` (git-ignored, created on first use) | `qb-config.local.json` (optional: copy `qb-config.template.json` here and set `recipient` to get email), `issue-history.json`, `reports/`, `logs/`, `artifacts/`, `run/`, `handoff/` (handoff files, including a history seed from the previous QB), and `knowledge/`: `team-contacts.json` (the roster; `./common/Invoke-Contacts.ps1` builds it), `triage-context.local.json` (verified findings; `./history/Invoke-Record.ps1 -AddContext`) `routing.local.json` (your copy of `routing.json`, made once on first use; your routing changes and incident notes go here), `guide-ownership.json` (the Guide's Repo Ownership table as first read, renewed only when the Guide file changes or on `./common/Invoke-Routing.ps1 refresh-ownership`) and `issue-aliases.json` (optional). `VMR_QB_HOME` points the skill at a different folder |

`knowledge/` is a starting point, not scripture: handles change, issues get fixed, ownership moves.
`./verify/Invoke-Verify.ps1 -Online` re-checks every handle, contact and linked issue against GitHub.
