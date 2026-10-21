---
name: vmr-qb-agent
description: >-
  Supports the .NET VMR Quarterback (QB) rotation for dotnet/dotnet.
  Use when preparing daily QB reports, assessing pipeline health, classifying
  build failures, identifying owners, or reviewing recurring issues. Scripts
  collect build logs, pipeline timelines, issue details, and code-flow status;
  the model writes the reports from that evidence by the rules in references/;
  reports are saved and optionally delivered by email. For detailed
  investigation of individual CI or Helix failures, use ci-analysis,
  helix-investigation, or pipeline-investigation. For source-build failures
  involving prebuilt dependencies, use source-build-investigation.
---

# VMR QB Agent

The Quarterback watches VMR build health and code flow, routes failures to owners, and merges flow
PRs. This skill automates most of that round.

**This file is the entry point.** The rules live in the guides under `references/`; the scripts only
collect, run the model, save and email. Run every script from `.github/skills/vmr-qb-agent/scripts/`
in PowerShell 7 on Windows.

When launched for a specific report stage with an evidence file, follow `references/runner-instructions.md` for that stage.
Collection is already complete; do not start another daily round. Agents and subagents read the skill/reference Markdown
and the named data files directly.

| Workflow | Read | Run |
|---|---|---|
| The daily round: check access, collect evidence, write the reports, email | [references/daily.md](references/daily.md) | `./daily/Invoke-Preflight.ps1` first, then `./daily/Invoke-Daily.ps1` |
| The shape of the reports and the action text | [references/report-contract.md](references/report-contract.md) | written by the model; nothing checks it |
| Classify a failure: fatal or not, what to call it, who owns it, which repo timed out | [references/triage.md](references/triage.md) | done by the round; a timeout names its repo in the digest |
| The nested model's instructions | [references/runner-instructions.md](references/runner-instructions.md) | read directly by each report stage |
| "Have we seen this before?", what an owner told you | the History section below | a search, one appended line |

Nothing has to be set up first. By default reports are saved and **no email is sent** (see
[README.md](README.md) to get them by email).

If `./daily/Invoke-Preflight.ps1` reports an authentication problem, **show it to the user verbatim,
first**. The round still runs: without internal access it reports the internal builds as "unknown",
never "healthy", and the report and the email open with the error and its fix. The usual fix is
`az login --allow-no-subscriptions`. A missing GitHub sign-in (`gh auth login`) is the one
authentication problem that stops the round.

## The one rule

**Scripts collect build logs and pipeline timelines; the model uses those inputs to write the
reports by the rules in `references/`.** The model may also perform the read-only Maestro and GitHub
checks the reports call for. It must not re-query Azure DevOps or acquire credentials; scripts own
that collection. Nothing validates the reports: every rule is written once, in the guide it belongs
to, and a QB reads a report before acting on it. What a script could not read is written at the top of
the report as a run note.

## Rules that apply to every task

- **Read-only by default.** Do not merge, approve, close, comment, trigger subscriptions or retry
  anything unless explicitly asked.
- **Never force-push a codeflow PR.** PRs titled `Source code updates from ...` belong to
  `dotnet-maestro[bot]`.
- **Routing has two official sources.** Repo-to-owner and the list of real teams come from the Repo
  Ownership table in `docs/VMR-QB-Guide.md`; a repo name written as a handle (such as `runtime`)
  resolves to that repo's owner from the same table. Channel rules, redirects and category routing are
  defined only in `knowledge/routing.json`. An unverified handle is a ping to nobody. The first run
  copies each into `local/knowledge/` (`guide-ownership.json`, `routing.local.json`) and uses the
  copies from then on; nothing is fetched or compared. Edit `routing.local.json` to change your own
  routing; after a pull that changes the Guide's table or `routing.json`, delete the copy and the next
  run makes a new one.
- **Keep finding names stable across days.** A renamed finding reads as new and hides a recurrence.
  Read recent overall reports and reuse their exact finding names.
- **Fatal vs non-fatal** is defined by the pipeline-investigation skill, from timeline records and
  never from message text; [references/triage.md](references/triage.md) adds only the QB-side rules.
  A non-fatal finding is monitor-only.
- **Do not hide a gap.** If a source could not be read, the report must say it was not read.
- **Only four things stop the round before it starts** (exit 2): PowerShell older than 7.3, a state
  folder that cannot be written, a missing GitHub sign-in, and a config holding your email address that
  Git tracks or does not ignore. A model run that fails, or a crash, fails the round (exit 1) and, when
  email is on, one short email says so. Everything else is a **run note** at the top of the overall
  report and the email, with how to fix it, internal authentication first.
- **Everything local to you lives in `local/`, and `local/` is git-ignored.** Settings (your email, if
  you want email), reports, logs, evidence, the team roster and your notes are one QB's own, so none of
  it is published.

## History

The saved reports and your owner notes are the history; nothing else is kept.

- **Have we seen this before?** `Select-String -Path local/reports/overall-summary-*.md -Pattern '<words from the finding>'`.
- **What an owner told you:** append a dated line to `local/owner-notes.md` with the finding name as the
  report shows it, the owner, what they said in their words, and a link. The round gives the file to
  the model, which must honor it (it decides New Issue and Monitor-only placement).

## Layout

| Path | Holds |
|---|---|
| `references/` | The guides above, the report contract, and the nested model's instructions |
| `scripts/` | `daily/` (preflight, the round, evidence collector, model digest, email), `triage/` (timeout attribution, issue search), `common/` (paths, config, local data initialization), and `tests/` (offline checks) |
| `knowledge/` | `routing.json`, the only shipped data |
| `local/` (git-ignored, created on first use) | `qb-config.local.json` - optional settings: `sender` selects the Outlook sending account; `recipients` lists destinations, with an empty list sending to `sender`.<br>`reports/` - saved reports.<br>`logs/` - run and model logs.<br>`artifacts/` - collected evidence.<br>`owner-notes.md` - owner updates.<br>`knowledge/routing.local.json` - the routing copy used by runs.<br>`knowledge/guide-ownership.json` - the local ownership copy.<br>`knowledge/team-contacts.json` - optional, hand-edited contacts.<br>Update local routing data when teams or ownership change.<br>`VMR_QB_HOME` points the skill at a different folder. |
