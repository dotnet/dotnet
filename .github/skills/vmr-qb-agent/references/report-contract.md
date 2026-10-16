# VMR QB report contract

The shape every daily report must follow. The point is comparability: a QB reading Thursday's
report after Wednesday's should be able to tell at a glance what is new, what moved, and what is
still waiting on someone.

## Overall summary

### 1. Top actions

```markdown
## Top actions for the QB today

1. [9000001](https://dev.azure.com/dnceng/internal/_build/results?buildId=9000001) — **BH #3:** <what failed and why it matters>. Owner: `@dotnet/<team>` / Named Person. Where: Teams - Unified Build Technical and General Discussions.
```

Every item starts with an evidence link and ends with an exact destination (nothing follows the
`Where:`; for an item with no owner, `Owner: No identified owner.` comes before it). Ordered most
urgent first. When there is genuinely nothing: `No QB action required.`

The list holds **only actions**: every numbered or bulleted item in it is held to that rule. Status
that needs no action (pass rates, which code-flow PRs are or are not merge-ready for step 3, that no
re-bootstrap PR is open for step 4) goes in the summary table as code-flow/PR rows, or in at most one
short plain paragraph after the list; never as a "Step 3" or "Step 4" bullet list inside it.

### 2. Summary table — exactly six columns, action-backed rows only

```markdown
| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |
```

| Column | Content |
|---|---|
| Issue category | `BH #N - <stable name>`, or `BH #-` when there is no build-health priority |
| Builds / PRs | Markdown links to every affected build or PR |
| New Issue | `N` if recorded history or supplied earlier reports contain the finding; otherwise `Y`, including when history is incomplete. This describes prior reporting, not when the failure first occurred. Older reports may contain `?`. |
| Existing fix PR | Link plus merge status, or `None found` |
| Action | `Where:` destination, then `Copy/paste:` the exact message |
| Links QB reported | Where it was already reported, or `None found` |

**This table holds only rows that need QB action.** A finding recorded as `owned`, `resolved`,
`noise`, or `wontfix` in the issue history store does not belong here — see the monitor-only
table below. **Row order follows the top-actions list.** Action 1's row first, then action 2, so
the list and the table can be read together. Never sort by "New Issue".

### 3. Monitor-only table — same six columns, no action required

```markdown
## Monitor only — no QB action needed

| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |
```

Every finding whose recorded status is `owned`, `resolved`, `noise`, or `wontfix` goes
here instead of the main table — ownership is confirmed or no outreach is needed.
A `routed` status only means someone was notified, not that anyone has taken responsibility;
it does not make a finding monitor-only. An otherwise actionable `routed` finding stays in the
main table for follow-up until ownership is confirmed. Use the recorded reporting links to
continue the existing conversation rather than treating it as a new issue.
The Action column in the monitor-only table still names who owns it (so
the QB knows at a glance), but reads `None - monitor only` as the destination, never a repeated
Teams/GitHub ping. Non-blocking findings also land here, and so do code-flow findings while the flow is healthy (nothing pending, or
only backflow commits, which are the VMR's own content returning). A code-flow finding that needs someone is an action like any
build failure and belongs in the main table as `BH #-`: a failed subscription outcome, a blocked or conflicted flow PR, or real
product commits waiting. Its message reads `Please check this issue. <observed blocker>` and states only what was observed.

When the store has nothing to say about a finding, it belongs in the main table, not here —
"might already be handled" is not the same as "recorded as handled."

### 4. Internal issue tracking and action drafts (appendix)

After both tables, include one `### Build health priority #N — <slug>` subsection per numbered
finding, each with:

```markdown
### Error
<the exact terminating diagnostic, quoted>

### Links
<build/PR URLs>

### Root Cause
<what is known; say "unconfirmed" rather than guessing>

### Blocking
<whether this blocks a specific release, or "not established">

### Affected Releases
<branches/builds affected>
```

This is not optional decoration — it is where the copy-ready operational-issue draft lives, and
omitting it is the single biggest source of two reports "looking different" day to day.

## Consistent numbering across report sources

The **visible numbered Build Health priority-action list is the only source** of `BH #N`. Nobody
counts findings or trusts a model-assigned priority. `daily/report.ps1` derives
`bhReferences` from that list and the same map feeds Top actions, table rows and the issue-draft
appendix. Findings grouped under one visible action share its number. Findings not listed there
remain `BH #-` and do not shift another finding's number.

A scheduled report and an ad-hoc report for the same day therefore agree by construction. If they
ever do not, `./daily/Invoke-Daily.ps1 -UseExistingReports -RefreshReportReferences` rewrites the
numbers from the visible list, and `./daily/Invoke-Report.ps1` fails any report in which one finding
carries two numbers or one number names two findings (an early version of this report did exactly that:
its Top actions and its table used different permutations of the same numbers).

## Validation

One gate (`Invoke-QBOverallReportGate` in `daily/contract.ps1`) runs the strict format validator plus checks that were
each a real defect in a delivered report. It is the same function, with the same inputs, at every boundary: when the
overall report is **generated**, when saved reports are **reused or repaired** (`-UseExistingReports`), and when you check
one **by hand** with `./daily/Invoke-Report.ps1`. The inputs are the report text and its date, the team contacts, the visible
Build Health action list (where every BH number must come from), the history (only days before the report's own date
count, so it gives the same answer before and after today's replay) and the active routing. A report therefore cannot pass
one boundary and fail the next. **FAIL** findings are objective; **warn** findings depend on the history store being
complete, are for a person to judge, and never block at any boundary.

What a FAIL finding does depends on who is asking. In the daily round (generation and reuse) it never costs the report: after
one document-only repair pass (kept only if it leaves fewer problems), each remaining problem becomes its own red run note at
the top of the overall summary ("Report check: ...") and the report is kept and sent. All the format problems of a report are
listed together, not only the first, and a bad handle no longer hides a bad row. `./daily/Invoke-Report.ps1`, the manual check,
exits non-zero for it. The run-notes block
(between `<!-- VMR-QB-RUN-NOTES -->` markers, directly under the title) belongs to the runner and is never checked, so a report
with notes passes the same checks as one without.

**What the runner fixes instead of rejecting.** Mechanical slips are normalized by the scripts (`ConvertTo-QBActionFormat`, the
parsers in `daily/report.ps1`), because a different model formats things differently and none of this is the model's job:
`BH #N` numbers are re-derived from the visible list, and a mention of another item inside a sentence ("independent of BH #2")
is kept as words ("build-health item 2"); a bare URL in a quoted message becomes a Markdown link labelled with its build id or
PR/issue number; build ids in the visible priority list are read whether they are links or plain numbers; the structured JSON
comments (`VMR-INTERNAL-FINDINGS`, `VMR-QB-ACTIONS`) may be on one line or on their own lines; and a contact's Teams display name
may use any letters (Levé, Přemek Vysoký). An untracked actionable finding with only a triage label and no conditional draft is
one run note for that finding; it no longer discards the other findings' drafts. Anything the scripts cannot repair is a run
note, never a lost report.

| Check | Severity | Catches |
|---|---|---|
| format | fail | links, destinations, 30-word limit, named contact beside each team |
| handle | fail / warn | a copy/paste message pinging a team that does not exist (it reaches nobody; a repo name written as a team handle is the usual cause). A real-but-broad team is a warning |
| first-responders | fail | First Responders used for anything that never mentions Helix |
| numbering | fail | a number naming different findings in different sections; one finding with two numbers; a Top action with no row; a number beyond the Build Health list |
| placeholder | fail | `<team>`, `TODO`, `TBD`, `{{...}}` left in the text |
| structure | fail / warn | missing Top actions or table; wrong title date; missing Monitor-only section or appendix |
| novelty | fail / warn | `New Issue = Y` for something history already holds; `N` with no record under that name |
| placement | fail / warn | a finding recorded `owned`/`resolved`/`noise`/`wontfix` in the main table; a `BH` finding in Monitor-only whose recorded status is still `open` |


## Action format

```
Where: Teams - <channel>
Copy/paste: "@dotnet/<team> @<Named Person>, Please check the build failure. <what failed>: <link>"
```

Destinations are exactly one of:
- `Teams - <channel>`: Unified Build Technical and General Discussions, First Responders
  (**Helix only**; the gate rejects it for anything that never mentions Helix), Servicing chat,
  Preview/RC chat, .NET Tactics chat. The channel list and its rules come from
  `knowledge/routing.json`.
- `GitHub - <issue link>`
- `Support - [<name>](<url>)`: an external support destination for a failure that has no GitHub
  team and no Teams channel (for example 1ES Pipeline Template tasks). The registry
  (`supportDestinations` in `knowledge/routing.json`) is the only source of the name and the
  link; the gate rejects any other spelling. The copy/paste message names the support
  destination (`"1ES support, Please check the build failure. ..."`), not an `@handle`.
- `None - monitor only`: someone already has it.
- `None - owner not identified`: nobody can be named from the evidence. There is no owner to ping,
  so there is no copy/paste message. The action reads
  `Action: No identified owner. <what failed and what evidence is missing>`, names no team or person,
  and gives no instruction. The label comes from `unmappedLabel` in `knowledge/routing.json`.
  Naming a convenient team instead is the mistake this destination exists to prevent.

The message is copy-ready. Never write prose instructing the reader to compose one.

## Cross-referencing

Tag each summary row and its matching top action with the same `BH #N`. Numbers may change between
days; **names must not**. A renamed finding reads as new and hides a recurrence.

## Non-blocking findings

On a `partiallySucceeded` build the failing tasks did not block it. Record them, state that no
action is needed, name no owner to ping, and keep them out of top actions.

## Reconciliation

The build-health priority list and the summary table describe the same set: every non-monitor
finding has a row, every row has a finding. Do not cap the priority list and silently drop a real
failure — if there are more than five, list them all.

## Qualifications survive

"Low disk observed; cause unconfirmed" and "no matching fix found" must reach the final report
intact. Do not turn a correlation into a proven cause to shorten a sentence.
