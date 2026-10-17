# VMR QB Agent

Supports the .NET VMR Quarterback (QB) daily workflow, based on the [VMR Quarterback Guide](https://github.com/dotnet/dotnet/blob/main/docs/VMR-QB-Guide.md). Scripts collect build logs and pipeline timelines, which the model uses to prepare the build-health report. The code-flow stage checks live dependency-flow status. Both reports, together with current GitHub checks, inform an overall QB report with top actions, owners, and ready-to-send messages.

## Usage

### Invoke the skill

In GitHub Copilot (the CLI or the Copilot app), opened on your `dotnet/dotnet` clone, type:

```
/vmr-qb-agent
```

Or ask in natural language. Copilot detects the intent and loads the skill:

```
Run the VMR QB report for today.
```

Whichever host you type this in, the report-writing stages are run by `scripts/daily/Invoke-Daily.ps1`, which starts the **Copilot CLI** (`copilot`) as a separate process for each stage. So the CLI must be installed and signed in for the full round. The other workflows (preflight, evidence-only runs, validating a report, history, triage, your routing) are plain scripts and do not need it.

### Get the reports by email (optional)

Email is off until you give the skill an address. Your address must never be committed, so it lives in a git-ignored file that you create from the public template:

1. Copy `qb-config.template.json` to `local/qb-config.local.json`:

   ```powershell
   Copy-Item .github/skills/vmr-qb-agent/qb-config.template.json .github/skills/vmr-qb-agent/local/qb-config.local.json
   ```

   (Create the `local/` folder first if it does not exist yet.)

2. Open `local/qb-config.local.json` and set `recipient` to your own email address. The reports are sent from the classic Outlook account that matches it.

   ```json
   {
     "recipient": "you@example.com"
   }
   ```

   To send the reports to more people, list several addresses separated by `;`. **At least one must be your own address**, because the reports are sent from your Outlook account that matches one of the recipients:

   ```json
   {
     "recipient": "you@example.com; teammate@example.com"
   }
   ```

If `recipient` is empty or the file is absent, the reports are saved and no email is sent. Email uses classic Outlook; the new Outlook does not work because the skill sends mail through the COM interface.

`qb-config.template.json` is public and always blank. `local/` and `qb-config.local.json` are in `.gitignore`, so your address stays on your machine, and `./verify/Invoke-Verify.ps1` fails if the public template ever contains an address. If your config ever sits in a Git repository that does not ignore it, or Git already tracks it, preflight stops the run and tells you how to fix it before anything is committed.

### What happens

The skill runs the whole process:

- Checks that you are signed in to what the round needs, showing any authentication problem first.
- Collects the evidence for the public and internal `dotnet-unified-build` pipelines.
- Produces the build-health report with the pipeline-investigation skill and the code-flow report with the flow-analysis skill.
- Uses those two reports as the baseline for the overall report: top actions, green code-flow and re-bootstrap PRs, who owns each failure, whether a fix PR already exists, and a message ready to send to each owner.
- Checks the reports and records the findings in your history. What a check finds, or anything stale, missing or wrong along the way, is written as a note at the top of the overall report with how to fix it; it never costs you the report.
- Saves everything in `local/reports/`.
- Emails the reports only if you turned email on.

The skill is read-only. It never merges, approves, comments, retries builds or posts to Teams; the QB does those.

## Prerequisites

**This skill supports Windows only. Linux and macOS are not supported.**

1. **GitHub Copilot CLI** (`copilot` on your PATH, either the native `copilot.exe` or the PowerShell launcher script that an npm installation creates), signed in, with the `dotnet-arcade-skills` plugin (`dotnet-helix` and `dotnet-codeflow` are required; `dotnet-binlog` is optional). These plugins are used to generate the build-health and code-flow reports. They are found under `COPILOT_HOME` or `~/.copilot/installed-plugins`, in any marketplace folder; set `pluginRoot` in `local/qb-config.local.json` if yours are elsewhere. The Copilot app can start the round, but the scripted round writes its reports through the CLI.
2. **PowerShell 7.2+** on Windows.
3. **Azure CLI**, for internal pipeline access: `az login --allow-no-subscriptions`
4. **GitHub CLI**, authorized for the dotnet organization: `gh auth login`
5. **Classic Outlook**, only if you want the reports emailed (see [email setup](#get-the-reports-by-email-optional)).

## Authentication

No tokens are stored by the skill.

- **Azure DevOps, public** (`dnceng-public`): no sign-in needed.
- **Azure DevOps, internal** (`dnceng/internal`): your `az login`. If it cannot be read, the round still runs for the public builds and code flow; the report says internal results are **unknown**, never "healthy", and opens with the authentication problem and the command that fixes it.
- **GitHub**: your `gh auth login`, for issue, PR and team evidence. This is the one sign-in the round cannot do without: it stops (exit code 2) and, when email is on, sends a short notice.

## How it works

This is a high-level overview of the key steps and files, not an exhaustive implementation reference. Copilot follows the workflow guides in `references/` and invokes their entry scripts, which call the supporting scripts in the corresponding workflow folder. Users do not need to follow these steps manually.

`daily.md` describes the whole round. The table maps each step to its reference documents and implementation; script-only steps implement the documented rules rather than reading Markdown at runtime. Steps 3a and 3b run in parallel.

| Step | Workflow reference | Key steps | Key scripts |
|---|---|---|---|
| 1. Preflight | [daily.md](references/daily.md) | Check sign-in, tools, plugins, and writable state; surface authentication problems first | `scripts/daily/Invoke-Preflight.ps1`, `scripts/daily/preflight.ps1` |
| 2. Collect evidence | [daily.md](references/daily.md) | Collect both pipelines over one 24-hour window, timelines, representative failure logs, and issue and owner evidence | `scripts/daily/evidence.ps1`, `scripts/triage/issue.ps1` |
| 3a. Build-health report | [runner-instructions.md](references/runner-instructions.md), [triage.md](references/triage.md) | Classify collected failures, identify owners, and write the build-health report using pipeline-investigation guidance | `scripts/daily/Invoke-Daily.ps1`, `scripts/daily/prompts.ps1`, `scripts/daily/workspace.ps1` |
| 3b. Code-flow report | [runner-instructions.md](references/runner-instructions.md) | Use the flow-analysis skill to check live dependency flow, stale or blocked subscriptions, and open code-flow PRs | `scripts/daily/Invoke-Daily.ps1`, `scripts/daily/prompts.ps1`, `scripts/daily/workspace.ps1` |
| 4. Overall summary | [runner-instructions.md](references/runner-instructions.md), [triage.md](references/triage.md), [report-contract.md](references/report-contract.md) | Combine both reports with GitHub checks into top actions, owners, existing fix PRs, and ready-to-send messages | `scripts/daily/Invoke-Daily.ps1`, `scripts/daily/prompts.ps1` |
| 5. Final checks | [report-contract.md](references/report-contract.md) | Check report shape, owners, numbering, action text, and consistency with history; record problems as run notes | `scripts/daily/contract.ps1`, `scripts/daily/gate.ps1`, `scripts/daily/report.ps1`; `scripts/daily/Invoke-Report.ps1` for a standalone check |
| 6. Record history | [history.md](references/history.md) | Record findings for future rounds while preserving owner updates | `scripts/history/history.ps1`, `scripts/history/store.ps1` |
| 7. Email (optional) | [daily.md](references/daily.md) | Send saved reports through classic Outlook with duplicate-send protection | `scripts/daily/Invoke-Daily.ps1` |

**Logs are sampled by distinct failure, not downloaded for every failed build.** The collector reads every problem build's timeline and groups matching failures by task and normalized diagnostic message within a pipeline and branch. It downloads one representative raw log for each group; other builds with the same failure retain their timeline diagnostics and refer to that representative. A broad category alone is not enough to group failures. Generic wrapper errors (such as "Bash exited with code 1") or missing diagnostic messages cannot establish a match, so their logs are collected separately. See [the daily guide](references/daily.md#what-is-scripted-and-what-is-not) for the detailed collection rules.

Scripts handle deterministic collection, validation, and bookkeeping. Copilot writes the build-health narrative from collected evidence, uses the flow-analysis skill for live code-flow observations, and combines the reports with GitHub checks into the overall summary. Every report is checked before it is recorded or sent; problems found during the daily round become run notes rather than discarding the report.

Generated output (reports, logs, evidence, history) stays in `local/`, which is git-ignored. Nothing is committed.

## When something is stale, missing or wrong

The round is built to produce a report anyway. Only four things stop it, each with exit code 2 and the fix named: PowerShell older than 7.2, a state folder it cannot write, no GitHub sign-in, and a config holding your email address that Git tracks or does not ignore (a commit could publish it). Everything else is a **run note**: one line at the top of the overall report and of the email, with how to fix it, and internal authentication always first.

| Issue encountered | How the round responds |
|---|---|
| Internal sign-in (`az login`) fails | Public builds and code flow still run; the internal builds are reported as UNKNOWN, with the error and the fix |
| Your routing copy is behind the shipped file, or names a team the Guide no longer lists | Uses it. An owner is matched to the closest team name, otherwise the handle is kept as written and marked as not matching. It never fails because of an owner |
| The Guide cannot be read | Uses the saved ownership copy. If no usable copy is available, fetches the Guide from GitHub main and saves an ownership copy for future runs. |
| `qb-config.local.json` is not valid | No email and the defaults are used; the note says how to fix it |
| `issue-history.json` or `triage-context.local.json` cannot be read | Skipped for this run, left as it is, and noted |
| The Copilot CLI or a skill is missing, or the pinned model is refused | The reports that need it are skipped with the reason (a refused model: the CLI's default is used instead). The scripts' evidence digest, `reports/evidence-digest-<date>.md`, is saved and emailed in place of the narrative |
| A report check finds a problem | The report is kept and the problem is a note |

The exit code is 0 when a report (or the digest) was delivered, whatever the notes say; 1 when nothing was delivered or the run crashed (the crash is logged with its stage, and emailed as a notice when email is on); 2 for a preflight stop; 3 when another run for the same day is in progress. A day is never emailed twice unless you ask: `./daily/Invoke-Daily.ps1 -Force`.

## Where your data lives

Everything that is yours alone is in `local/`, which is git-ignored and never published: your email settings, the issue history, reports, logs, the team roster and the findings you record. Set `VMR_QB_HOME` to use a different folder.

## Unattended runs and maintainers

### Schedule a daily run

For automated daily runs, set up Windows Task Scheduler to run `scripts/daily/Invoke-Daily.ps1 -Scheduled` under your authenticated Windows account. The skill does not create or configure the scheduled task automatically.

### Maintainer checks

[SKILL.md](SKILL.md) lists every workflow and its scripts. Maintainers run `./verify/Invoke-Verify.ps1` from `scripts/` to check the skill is fit to publish (no binaries, secrets or addresses, `local/` ignored and not tracked by Git, generated docs in sync); the offline tests are `scripts/tests/Invoke-VmrQBTests.ps1`.
