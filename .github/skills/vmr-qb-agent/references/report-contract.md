# VMR QB report contract

The shape of the overall report. The point is comparability: a QB reading Thursday's report after Wednesday's should be able to
tell at a glance what is new, what moved, and what is still waiting on someone. You write the whole report from the evidence
your task names. Nothing is checked or repaired after you, and a QB acts on what you wrote, so every rule here is yours to
follow: a wrong owner, a wrong number or an invented link costs them time.

The runner may put a run-notes block (between `<!-- VMR-QB-RUN-NOTES -->` markers) directly under a saved report's title. It
belongs to the runner: do not write one and do not copy one from an earlier report.

## What the report covers

It summarizes steps 1-4 of the VMR QB daily checklist from today's two specialist reports and the evidence in your task.
Cover at minimum: build failures (step 1), code-flow staleness or blockage (step 2), green code-flow PRs ready to merge
(step 3) and re-bootstrap PRs (step 4). Internal coverage and statistics come from the digest and the Build Health report;
do not omit the internal failures.

```
gh pr list --repo dotnet/dotnet --search "author:app/dotnet-maestro" --state open --limit 60 --json number,title,headRefName,baseRefName,url,createdAt
gh pr checks <number> --repo dotnet/dotnet
gh pr view <number> --repo dotnet/dotnet --json url,baseRefName,isDraft,mergeable,mergeStateStatus,reviewDecision,changedFiles,body
gh pr list --repo dotnet/dotnet --search "author:dotnet-bot" --state open --limit 40 --json number,title,baseRefName,url,createdAt
```

- Step 3: list the open code-flow PRs (first command), check CI for a few of the most relevant (second) and their remaining merge
  gates (third). Green CI alone is not merge-ready: report conflicts, required reviews, pending Maestro gates and unsafe or
  high-file-count flows separately for each PR. Do not assume every release/* branch is locked; follow the VMR QB Guide and
  verified branch restrictions.
- Step 4: the open re-bootstrap PRs from dotnet-bot (fourth command; titles like ".NET X.Y.Z-... Updates") and their CI status.

## Layout

Under the title, in this order: `## Top actions for the QB today`, `## Summary` (findings that need action),
`## Monitor only - no QB action needed` (findings that need none) and `## Internal issue tracking and action drafts`. The
title is `# VMR QB Daily Overall Report - <reportDate>`. Both tables use the six-column header below, and the Summary has at least one row.

## Top actions for the QB today

```markdown
## Top actions for the QB today

1. [9000001](https://dev.azure.com/dnceng/internal/_build/results?buildId=9000001) - **BH #3:** <what failed and why it matters>. Owner: `@dotnet/<team>` / Named Person. Where: Teams - <exact channel>.
```

- Every item (numbered or bulleted, sub-bullets included) starts with a Markdown link to its affected builds or PRs, is one
  line, and ends with its exact destination: `Where: <destination>.` with nothing after it. For an item with no owner,
  `Owner: No identified owner.` comes before the `Where:`. Most urgent first.
- Put the `**BH #N:**` tag after the evidence links, never before them; an item whose finding has no number carries no tag.
  Write a mention of another item inside a sentence in words ("independent of build-health item 2"), not as `BH #2`.
- One issue and one next step per item. Never combine unrelated failures because they share a pass-rate statistic, branch,
  owner or build; builds with the SAME failure may share an item, each sub-bullet still starting with its own link. Separate
  EMSDK signing, missing-output cascades, installer/RPM failures, compiler exits, validation timeouts and socket exhaustion.
  Give each PR its own item when its blocker or action differs (a conflicting green PR, a PR awaiting a Maestro gate and an
  iOS NativeAOT clang crash are separate matters); never write a blanket "do not merge any PR". WindowsDesktop and WPF
  subscription findings get separate items. When no PR or build exists, link the verified issue or Maestro subscription and
  say explicitly that no PR or build exists; never invent one.
- The list holds ONLY actions. Status that needs no action (pass rates, which code-flow PRs are or are not merge-ready for
  step 3, that no re-bootstrap PR is open for step 4) goes in the tables as code-flow or PR rows, or in at most one short plain
  paragraph (not a list) after the list. Never write a "Step 3" or "Step 4" heading or bullet list inside it.
- When there is genuinely nothing: exactly `No QB action required.` Never write it while any table row carries a
  `Copy/paste:` message or `None - owner not identified`: those rows are actions.

## Summary and Monitor only: the six-column table

```markdown
| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |
|---|---|---|---|---|---|
```

| Column | Content |
|---|---|
| Issue category | `BH #N - <stable name>`; `BH #- - <stable name>` when the finding has no build-health priority (code-flow and PR rows too) |
| Builds / PRs | Markdown links to every affected build or PR. When none exists say so and link the verified issue, subscription or monitoring-evidence page instead. Every link the action message uses is here or in the fix column |
| New Issue | `Y` or `N` (below) |
| Existing fix PR | A PR link and its status, `None found`, or `N/A` (below) |
| Action | The action cell (below) |
| Links that QB reported | Where the failure was already reported (below), or `None found` |

- One finding per row, all six cells on every row (a five-column row is wrong); write a literal `|` inside a cell as `\|`.
- Row order follows Top actions: the row of action 1 first, then action 2, so the list and the table read together. Rows with
  no Top action (owned, monitor-only, code-flow and PR rows) come after all action-backed rows, most urgent first. Never sort by
  New Issue.
- **Monitor only** holds a finding that the owner notes (or an earlier report) record as `owned`, `resolved`, `noise` or
  `wontfix`: ownership is confirmed or no outreach is needed, so it is not a Top action. `routed` only means someone was
  notified, not that anyone took responsibility, so an otherwise actionable routed finding stays in the Summary until
  ownership is confirmed. Non-blocking findings (below) land here, and so do code-flow findings while the flow is healthy
  (nothing pending, or only backflow commits, which are the VMR's own content returning). A team member's own PR or issue is
  monitor-only (`triage.md`). The Action names who already owns it but is never a ping:
  `Where: None - monitor only.<br>Action: <factual status>`. Use the recorded links to continue the existing conversation
  rather than treating it as new. When nothing records a finding as handled it belongs in the Summary: "might already be
  handled" is not "recorded as handled".
- A code-flow finding that needs someone is an action like any build failure and belongs in the Summary as `BH #- - <name>`: a
  failed subscription outcome, a blocked or conflicted flow PR, or real product commits waiting. Its message reads
  `Please check this issue. <observed blocker>` and states only what was observed.
- Non-blocking: on a `partiallySucceeded` build the failing tasks did not block it. Record them, say no action is needed, name
  no owner to ping and keep them out of Top actions.

## The action cell

```
Where: Teams - <exact channel>.<br>Copy/paste: "@dotnet/<team> @<Named Person>, Please check the build failure. <what failed and the observed error>: <evidence links>"
```

State ONE primary destination, never "Teams or GitHub", and write the literal `<br>`. The destination is exactly one of:

- `Teams - <channel>`: one channel name exactly as the routing JSON lists it; that data and `triage.md` say which
  failure goes where. First Responders is for Helix failures only: a row or Top action routed there must be about Helix. A
  GitHub team handle identifies an owner; do not present it as a verified Teams mention.
- `GitHub - [owner/repo#N](https://github.com/owner/repo/pull/N)`: a verified PR or issue link. Prefer the existing issue or
  fix PR for an already tracked failure; use the named Teams channel for coordination or an untracked incident.
- `Support - [<name>](<url>)`: an external support destination for a failure with no GitHub team and no Teams channel (for
  example 1ES Pipeline Template tasks), name and link exactly as the routing JSON gives them. The message names the
  support destination, not an `@handle`: `Copy/paste: "1ES support, Please check the build failure. ..."`.
- `None - monitor only`: someone already has it. Use `Action: <factual status or pending event>`: no message, no ping, no
  instruction, for example `Action: Owner reports a transient restore failure; rerun pending.` Do not turn an owner-assessed
  transient into another escalation or an unverified recovery claim.
- `None - owner not identified`: nobody can be named from the evidence, so there is nobody to ping and no message. Write
  `Action: No identified owner. <factual statement of what failed and what evidence is missing>`: no team, no person, no
  instruction. The row still needs the QB, so it is an action. Naming a convenient team instead is the mistake this destination
  exists to prevent.

The message is copy-ready; never write prose telling the reader to compose one.

- Owners: only handles from the ownership/routing JSON (a handle that is not listed reaches nobody; a repo name is not a team),
  comma-separated if several; `First Responders` by name for that channel; the support name for a support destination.
- Name ONE person beside the team in the same owners list, never the team alone: the GitHub login on a GitHub destination
  (`@dotnet/<team> @<github-login>, ...`), the Teams display name on a Teams channel (`@dotnet/<team> @<Display Name>, ...`).
  Use exactly the pairs in the contacts JSON and invent no one; a team with no entry is pinged alone.
- Start with `Please check the build failure.` (`Please check this issue.` for a PR or flow issue), then state ONLY what failed
  or the observed status. The check request and the description together are at most 30 words (aim for 25 or fewer),
  excluding owner handles and links. Plain language: no proposed remedies, no investigation instructions.
- After the opener, never tell the owner to correct, fix, change, update, upgrade, downgrade, backport, apply, bump, pin,
  rerun, retry, requeue, merge, approve, resolve, investigate, review, verify, validate, restore, check, await, wait or monitor
  anything, and write no "please", "kindly", "can you", "could you", "should", "must" or "needs to". A factual "review is
  required" is fine; "please review/approve this PR" is not. A monitor-only or no-owner Action is facts only in the same way.
- The whole message is in plain double quotes, not a code span; diagnostics quoted inside use single quotes. It ends with
  direct Markdown links from that row's evidence (`[11](https://...)`, labeled with the build id or `#PR`): at least one in
  the Builds / PRs cell, none that is not in the Builds / PRs or Existing fix PR cell (add a PR the message links to the Builds
  / PRs cell; a matching known-fix link from the fix cell may be added when useful). A bare URL is not a link. Never output a
  placeholder owner, URL or `<paste link>`.
- Known-fix links, merge status and rollout uncertainty stay in the Existing fix PR cell. Even when a fix is known, the message
  reports only the failure or status, not an instruction to apply the fix or check its rollout.

## BH numbering and names

- The numbered `## Priority actions` list of today's Build Health report is the only source of `BH #N`: item N is `BH #N`.
  Do not count findings, renumber or trust any other priority, and never invent a number for a finding no item covers. Use the
  number of the item that covers the finding (same builds, same failure); findings grouped under one item share it. Findings
  that are not items (monitor-only, owned, non-blocking, canceled by their requester, code-flow and PR rows) are `BH #-` and
  never shift another number.
- Write the number in the first cell, in the finding's Top action and in its appendix heading. One finding has one number and
  one number names one finding, the same one in all three places. A number beyond the end of the list does not exist.
  Numbers may change between days; names must not.
- The name is the finding's stable name: reuse one from recent overall reports, character for character, whenever it
  describes the same failure, and invent a name only when none does (`triage.md`: keep the name stable). A name carries no date
  or build id; new build ids, renamed categories, version changes and recurrences do not make a failure new. Mark an
  ambiguous match uncertain. An old unclassified Darc group cannot prove prior reporting of a newly diagnosed specific
  publishing fault, and an unresolved aggregate identity (a bucket of low-lag subscriptions) is marked uncertain, not named as
  one incident.

## New Issue, Links that QB reported, Existing fix PR

- **New Issue.** `N` when the name is in recent overall reports, yesterday's reports or the owner notes, even if it
  was absent yesterday: a recurrence is not a new finding, and earlier records beat absence from yesterday's reports. Otherwise
  `Y`, including when those records are incomplete (say so; never write `?`). This says "not previously reported in the records
  you have", not that the failure first occurred today. `N` needs a record under that name.
- **Links that QB reported.** Where this exact failure was already reported, so the QB can find the thread fast: Markdown links
  to the GitHub issues the issue evidence lists for the row's builds (`matchedBuildIds`) or that plainly describe the same
  failure, plus the links and remarks the owner notes (`owner-notes.md`) record for it (a remark with no link is plain text).
  A build-id hit says the BUILD is mentioned there, a lead and not proof that this failure category was the one reported; say
  so (`dotnet/dotnet#123 mentions build 456`). Copy a note only onto the rows its stated failure and scope name: one build can
  carry several unrelated failures. Teams chat has no API: never invent a Teams URL. `None found` when the evidence shows no
  existing report.
- **Existing fix PR.** Find whether a PR already fixes each failure or issue. (1) Owner notes first: match a fix only when the
  diagnostic, branch and scope match what the note says it covers. A fix for one symptom does not cover a different symptom
  the note excludes: the compiler-server-shutdown fix covers neither crossgen2 `Text file busy` nor unrelated 30/60-minute
  validation timeouts; a failure seen before an owner's update cannot refute a later reported fix; a fixed-reported issue that
  still occurs needs post-fix verification, not automatic suppression. A version-pin guess is not a verified fix. (2) Otherwise
  a bounded search in dotnet/dotnet and, for root causes, the owning product repo (roslyn, runtime, arcade, sdk): a few
  targeted `gh search prs --repo dotnet/dotnet "<short error keywords>" --merged --limit 8 --json number,title,url,state,closedAt`
  (and `--state open`) per distinct root cause. (3) Record a PR link with its status (`dotnet/dotnet#1234 merged 2026-01-15
  (rollout unverified)`, `dotnet/roslyn#5678 open`), `None found`, or `N/A` for a transient infra or network blip a code fix
  would not address. A merged fix is not proof of deployment: qualify it `(rollout unverified)` unless green builds after it
  are evident.

## Internal issue tracking and action drafts

Start with: `Drafts only: no issues, comments, assignments, messages, or reruns were submitted.` One subsection per internal
finding of the Build Health report (public cases are not repeated here), each finding exactly once, headed
`### BH #N - <stable name>` (`### <stable name>` when it has no number), with a `---` between them. This is where the
copy-ready operational-issue draft lives, and omitting it is the single biggest source of two reports "looking different".

`````markdown
### BH #3 - <stable name>

**Builds:** [11](https://dev.azure.com/dnceng/internal/_build/results?buildId=11), [12](https://dev.azure.com/dnceng/internal/_build/results?buildId=12)
**Disposition:** new_issue
**Existing / related tracker:** <issue URL, only when there is one>
**Evidence / scope:** <exact match, related historical recurrence, or why monitoring is sufficient>
**Individual assignee:** not confirmed; use the owner-team routing in the summary table.
**Proposed title:** <Unified Build Operational Issue title>
**Copy-ready body:**
````markdown
### Error
<the exact terminating diagnostic, quoted>

### Links
<every affected internal build URL>

### Root Cause
<what is known; say "unconfirmed" rather than guessing>

### Blocking
<whether this blocks a specific release, or "not established">

### Affected Releases
<branches/builds affected>
````
**Search basis:** <the searches and evidence behind the decision>
`````

- **Builds:** every build the finding affects and no build from another failure. The tracker line appears only when there is a
  tracker, and its URL comes from the issue evidence or the owner notes, never a guess. The assignee line is always as shown:
  no individual can be verified as selectable from your evidence, and a team is not a user assignee; do not turn team membership
  or an old fix into a claim of defect ownership.
- **Disposition**, one of: `new_issue` (no tracker exists; a failure that keeps recurring across recent reports with none is
  the strongest candidate); `existing_issue` (this failure, or this exact build or log, is tracked: it can sit in a comment of
  an aggregate tracker whose body describes another failure); `possible_recurrence` (a closed related issue: not proof of a new
  defect or that the branch is fixed; state a differing branch or test scope and the uncertainty); `monitor_only`;
  `needs_triage`.
- Before `new_issue`, examine the open AND closed operational issues, the relevant bodies and comments (`gh issue view`), the
  exact diagnostic and the build-id hits. Issue evidence with `status` `partial` means `needs_triage`, never "no issue exists".
- A finding already tracked is NEVER `new_issue` (`triage.md`: do not create duplicates): when the owner notes or an issue show
  it is filed (a tracker URL, or owned, resolved, noise, wontfix), use `existing_issue` with that URL and write no draft. The
  Darc promotion artifact download/channel failure is the standing example.
- A draft (title, body, search basis) is required for `new_issue`; give a conditional draft, saying the existing tracking or
  scope needs owner confirmation before it is opened, for a `needs_triage` or `possible_recurrence` that still needs a tracker
  decision, so an unresolved actionable failure is usable rather than a bare label. No draft for `monitor_only` or a finding
  tracked exactly. The body keeps the five headings above, with the actual build and log URLs and owner teams, a root cause that
  may be unknown, and no prescribed speculative fix. Its Links list EVERY affected build
  (`https://dev.azure.com/dnceng/internal/_build/results?buildId=<id>`): all of them, however many, never representative
  examples and never "and N more".
- A proposed comment (for `existing_issue` or `possible_recurrence`) only when it carries genuinely new evidence not already
  posted; otherwise none. Respect owner-assessed transient or rerun-pending guidance and manual cancellations; open no issue
  for downstream SBOM or artifact symptoms or an intentional cancellation.

## Rules that apply to the whole report

- **Links.** Every hyperlink is an HTTPS web URL, never a file:// URI, drive path or relative Markdown path; local reports are
  email attachments, not web links. For a monitoring gap with no build or PR, link the affected service page and state the
  missing inventory (`https://dev.azure.com/dnceng/internal/_build` for unavailable internal access; the verified
  subscription or dashboard URL for Maestro coverage). Never invent a build id to satisfy a link.
- **Reconciliation.** The Build Health priority list and the Summary describe the same set: every action-backed finding has an
  item, a row and a Top action, and every row has a finding. A finding already owned according to the owner notes has a row
  but no priority action. Never cap the list at five and silently drop a real failure: if there are more, list them all.
- **No placeholders.** No `<team>`, `<owner>`, `<link>`, `<build>`, `<channel>`, `<what failed>`, `<name>`, `TODO`, `TBD`,
  `XXX`, lorem ipsum or template braces left in the text (these examples are shapes, not text to copy).
- **Qualifications survive.** "Low disk observed; cause unconfirmed" and "no matching fix found" reach the final report intact.
  Do not turn a correlation into a proven cause to shorten a sentence. Missing evidence is stated as unknown, never healthy.
