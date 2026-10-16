# VMR QB Issue History

A daily report only shows today. Investigations constantly need yesterday: *have we seen this, what
did we decide, did it get fixed, who said so.* Answering that by grepping old markdown is slow and
misses anything that was renamed. The history store answers it in one command.

The store is a plain JSON file in the skill's git-ignored `local/` folder (`issue-history.json`; override
with `$env:VMR_QB_HOME`). It holds no binaries, can be diffed and reviewed, and is the
format a handoff export uses too. Run everything below from `.github/skills/vmr-qb-agent/scripts/`.
To classify or route a fresh failure, see [triage.md](triage.md); to run the daily report, see
[daily.md](daily.md).

## Ask, don't grep

| Question | Command |
|---|---|
| What do we know about build 1234567? | `./history/Invoke-History.ps1 search -BuildId 1234567` |
| Have we seen this wording before? | `./history/Invoke-History.ps1 search -Text 'socket buffer'` |
| What does a given team have open? | `./history/Invoke-History.ps1 search -Owner '@dotnet/dnceng' -Status owned` |
| Which findings are probably the same failure under different names? | `./history/Invoke-History.ps1 duplicates` |
| An owner just told me something | `./history/Invoke-Record.ps1 -Finding ... -Status ... -Note ...` |
| Hand everything I learned to the next QB (one file: history, confirmed findings, aliases, links, hand-picked contacts, and the routing I added or changed) | `./history/Invoke-History.ps1 export` |
| Load a colleague's handoff file (or a plain history export); additive, yours always wins. `-Preview` reports exactly what would change and writes nothing | `./history/Invoke-History.ps1 import -InputPath <file> [-Preview]` (or `./common/Invoke-Setup.ps1 -ImportHandoff <file>`) |
| Fix up an old or imported history (aliases, owner handles that are not teams) | `./history/Invoke-History.ps1 repair [-Preview]` |

A handoff's routing section holds only what the sender added or changed in their own routing copy (each change with the shipped entry it started
from), never the whole copy and never a stamp, setting or file path. On import an entry is added when you have none; replaced only when you never changed
yours and started from the same shipped entry; kept (and reported as a conflict) when you changed it or started from a different one; refused, with the
reason, when it would not load here or carries an email address or a file path. An old bundle without the section, or a newer section version, is skipped
without touching your routing, and a repeated import changes nothing. Compare and reconcile afterwards with `./common/Invoke-Routing.ps1 diff`.

"Nothing matches" never proves a failure is new. The same problem has been reported as
*"PublicApiAnalyzers dependency-load failure"*, *"Public API analyzer dependency missing"*,
*"Roslyn analyzer missing Microsoft.Bcl.AsyncInterfaces"* and *"Roslyn analyzer/signing dependency
failure"*: four names, one problem. Try a distinctive word from the diagnostic, then run
`./history/Invoke-History.ps1 duplicates`.

## Names are weak evidence, builds are strong

Finding names are written by a model each day, so one failure fragments. In the data this skill was
built from, a single macOS failure was recorded under six names with one occurrence each, which hid
that it kept coming back. `./history/Invoke-History.ps1 duplicates` scores pairs on shared words (at least two),
adds weight for builds both appeared in, and refuses to match different branches
(`release/10.0.1xx` is not `release/11.0.1xx`), different versions (`v2` is not `v1.40.0`), or opposite
states (`No open PRs` is not `Open PRs`; `flow remains red` is not `green flow`). It **suggests**; each result carries the exact
`./history/Invoke-History.ps1 alias` command. Read the pair, then run it only if you agree. Folding keeps the
earliest `first_seen`, every occurrence and the whole status trail.

Fold respellings of one failure, or of one state of one flow. Do not fold different states of a code-flow
PR (green, red, stale, awaiting review) into one finding unless you want the stream, not the state, to be the
thing you track. Record the ones you agree with with `./history/Invoke-History.ps1 alias -Alias '<name to fold>' -Canonical '<name to keep>'`; the alias is stored in the history, so it survives an export and import. Nothing about aliases ships with the skill: finding names are written by a model and drift, so a curated list would go stale.

## Status vocabulary

| Status | Meaning | QB action |
|---|---|---|
| `open` | Seen, not yet routed | Route it |
| `routed` | Reported to an owner, but ownership is not yet confirmed | Follow up until someone takes responsibility |
| `owned` | Owner has taken it, or an IcM exists | **None.** Not an action item |
| `resolved` | Fixed, with a recorded resolution | None |
| `regressed` | Recurred after being marked resolved | **Escalate.** The fix did not hold |
| `noise` | Real but non-blocking; do not raise | None |
| `wontfix` | Accepted, will not be fixed | None |

A `resolved` issue that appears again flips to `regressed` automatically and writes the transition.
That is a signal: the fix did not hold, or did not reach every branch.

## Facts a person recorded are not overwritten by a report

Every owner and tracker carries where it came from. A value recorded with
`./history/Invoke-Record.ps1` is `operator`; one read out of a daily report is `daily-run`. **A daily
replay can fill a blank or refresh a `daily-run` value, but it can never replace an `operator`
value.** (Before this rule, a model-written report could silently overwrite an owner a human had
set.) Owners are canonicalized through `knowledge/routing.json`; a handle that does not exist is
dropped from a report row and refused from a manual update.

## Recording what you were told

```powershell
./history/Invoke-Record.ps1 -Finding '<exact finding name>' -Status owned -Owner '@dotnet/<team>' `
    -SaidBy '<who said it>' -Note '<what they said, in their words>' `
    -Builds <build id> -Match '<regex for the issue category>' -AddContext -AppliesTo '<when this applies, and when it does not>'
```

It validates everything first (exact finding name, real owner, https tracker, a valid `-Match`),
then updates history, the "links the QB reported" column, and the model's context together. Add
`-WhatIf` to preview. A typo in `-Finding` is refused with close matches; it is never turned into a
new finding. Say who said it and keep their words: the next QB reads them.

## Answering the common questions

**"Is this new?"** `./history/Invoke-History.ps1 search`; a hit means no, however it is worded today. Report
`New Issue = N` and cite the recorded tracker, not "None found".

**"Is this the same issue as X?"** Find the one discriminating fact and check it first. Shared
symptoms are weak evidence; a shared *mechanism* is strong. Useful discriminators: the exact error
code, the file path, the `Directory.Build.targets` line number (it dates the code), whether the
affected branch even carries the relevant commit.

**"Should we file an issue?"** A long recurrence with no tracker is the strongest candidate. A single
occurrence usually is not.

**"What did we do last time?"** `./history/Invoke-History.ps1 search -Text '<name>'`, then read the audit
trail (`Get-VmrIssueHistory -Path <history> -Name '<name>'` -> `.statusTrail`, oldest first).

## Keeping it current

The daily run replays today's report into the store automatically (`./history/Invoke-History.ps1 replay`,
idempotent). Everything else is `./history/Invoke-Record.ps1`. When you find a recorded fact is wrong,
correct it with a note saying why: a stale status nobody corrects is worse than none.

If `local/issue-history.json` cannot be read (it is damaged, or was left half-written), the daily round does not stop: it
leaves the file exactly as it is, says so in the run notes, and goes on without comparing with earlier days. Today's findings are
then not recorded. Once the file is fixed or moved aside, `./history/Invoke-History.ps1 replay -Date <day>` records a saved
report again, one day at a time.

> The store records what *was decided*, not what is true forever. Ownership, trackers and fixes
> drift. `./verify/Invoke-Verify.ps1 -Online` re-checks the shipped knowledge against GitHub.
