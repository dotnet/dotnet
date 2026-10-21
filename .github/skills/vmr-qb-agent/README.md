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

Whichever host you type this in, the report-writing stages are run by `scripts/daily/Invoke-Daily.ps1`, which starts the **Copilot CLI** (`copilot`) as a separate, tool-restricted process for each stage. So the CLI must be installed and signed in.

### Get the reports by email (optional)

Email is off until you configure a sender. Email addresses must never be committed, so they live in a git-ignored file that you create from the public template:

1. Copy `qb-config.template.json` to `local/qb-config.local.json`:

   ```powershell
   Copy-Item .github/skills/vmr-qb-agent/qb-config.template.json .github/skills/vmr-qb-agent/local/qb-config.local.json
   ```

   (Create the `local/` folder first if it does not exist yet.)

2. Open `local/qb-config.local.json` and set `sender` to the email address of the classic Outlook account to send from. Put zero or more destination addresses in `recipients`:

   ```json
   {
     "sender": "you@example.com",
     "recipients": []
   }
   ```

   An empty `recipients` list sends to `sender`. To send elsewhere, use a list such as `["teammate@example.com", "another@example.com"]`. The sender does not have to appear in this list and is not automatically copied.

If `sender` is empty or the file is absent, the reports are saved and no email is sent. Email uses classic Outlook; the new Outlook does not work because the skill sends mail through the COM interface.

`qb-config.template.json` is public and always blank. `local/` is in `.gitignore`, so your address stays on your machine. If your config ever sits in a Git repository that does not ignore it, or Git already tracks it, preflight stops the run and tells you how to fix it before anything is committed.

### What happens

- Checks that you are signed in to what the round needs, showing any authentication problem first.
- Collects the evidence for the public and internal `dotnet-unified-build` pipelines, and the GitHub issues that mention the failed builds.
- Has the model write the build-health report, the code-flow report and the overall report (top actions, who owns each failure, whether a fix PR already exists, a message ready to send to each owner), by the rules in `references/`.
- Saves all reports in `local/reports/`.
- Emails the reports if email is turned on.

The skill is read-only. It never merges, approves, comments, retries builds or posts to Teams; the QB does those. Nothing validates what the model writes, so read a report before acting on it.

## Prerequisites

**This skill supports Windows only. Linux and macOS are not supported.**

1. **GitHub Copilot CLI** (`copilot` on your PATH, either the native `copilot.exe` or the PowerShell launcher script that an npm installation creates), signed in, with the `dotnet-arcade-skills` plugin (`dotnet-helix` and `dotnet-codeflow` are required; `dotnet-binlog` is optional).
2. **PowerShell 7.3+** on Windows.
3. **Azure CLI**, for internal pipeline access: `az login --allow-no-subscriptions`
4. **GitHub CLI**, authorized for the dotnet organization: `gh auth login`
5. **Classic Outlook**, only if you want the reports emailed (see [email setup](#get-the-reports-by-email-optional)).

## Authentication

No tokens are stored by the skill.

- **Azure DevOps, public** (`dnceng-public`): no sign-in needed.
- **Azure DevOps, internal** (`dnceng/internal`): your `az login`. If it cannot be read, the round still runs for the public builds and code flow; the report says internal results are **unknown**, never "healthy", and opens with the authentication problem and the command that fixes it.
- **GitHub**: your `gh auth login`, for issue, PR and team evidence. This is the one sign-in the round cannot do without: it stops (exit code 2) and, when email is on, sends a short notice.

## How it works

Copilot follows [SKILL.md](SKILL.md) and the guides in `references/`, and runs two scripts: `scripts/daily/Invoke-Preflight.ps1` and `scripts/daily/Invoke-Daily.ps1`. [references/daily.md](references/daily.md) describes the round, what stops it and what only adds a note, and its exit codes (0 delivered, 1 failed, 2 preflight stop). The rules for the reports are in [report-contract.md](references/report-contract.md), [runner-instructions.md](references/runner-instructions.md) and [triage.md](references/triage.md).

**The collector reads every problem build's timeline, but downloads raw logs from one representative build per matching failure category**, not every failed build. Matching uses the task and normalized timeline diagnostics within a pipeline/branch (PR branch numbers are ignored). Other occurrences retain their own timeline evidence and point to the representative. Wrapper-only or message-less failures and child promotion builds are read separately. Each pipeline has a 1,000-log download budget per run; build counts are not limited.

Generated output (reports, logs, evidence) stays in `local/`, which is git-ignored. Nothing is committed.

## Where your data lives

Everything that is yours alone is in `local/`, which is git-ignored and never published: your email settings, reports, logs, evidence and your `owner-notes.md`. Set `VMR_QB_HOME` to use a different folder.

**Hand-off to the next QB:** copy `local/owner-notes.md`.

## Maintainers

Offline checks in `scripts/tests/` cover evidence collection, timeout attribution, and email/read-only/privacy safeguards. Run a check directly with `pwsh -NoProfile -File <test-path>`. [SKILL.md](SKILL.md) is the entry point and lists every workflow.
