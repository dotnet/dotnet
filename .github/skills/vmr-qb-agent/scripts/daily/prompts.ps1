Set-StrictMode -Version Latest

# daily/prompts.ps1 - The prompts the nested model receives and the small context helpers that feed them.
# Every input is a parameter (the report date and the ownership packet included), so a stage's prompt is built the same
# way in the runner and in a test. Nothing here reads the runner's state; loading it runs nothing.
. (Join-Path $PSScriptRoot '..\common\core.ps1')
function Get-PreviousFindingNames {
    param([Parameter(Mandatory)][string] $ReportDirectory, [Parameter(Mandatory)][string] $Date, [int] $Days = 5)
    if (-not (Test-Path -LiteralPath $ReportDirectory)) { return $null }
    $names = [ordered]@{}
    $previous = Get-ChildItem -LiteralPath $ReportDirectory -Filter 'overall-summary-*.md' -ErrorAction SilentlyContinue |
        Where-Object { $_.BaseName -notmatch [regex]::Escape($Date) } |
        Sort-Object Name -Descending | Select-Object -First $Days
    foreach ($file in $previous) {
        foreach ($line in (Get-Content -LiteralPath $file.FullName -ErrorAction SilentlyContinue)) {
            if ($line -notmatch '^\s*\|') { continue }
            $cell = ($line -split '\|')[1]
            if (-not $cell) { continue }
            $match = [regex]::Match($cell.Trim(), '^(?:BH #(?:\d+|-)\s*-\s*)?(?<name>.+?)\s*$')
            if (-not $match.Success) { continue }
            $name = $match.Groups['name'].Value.Replace('**', '').Trim()
            if (-not $name -or $name -match '^(?:-+|Issue category)$' -or $name.Length -lt 8) { continue }
            if (-not $names.Contains($name)) { $names[$name] = $file.BaseName -replace '^overall-summary-', '' }
        }
    }
    if ($names.Count -eq 0) { return $null }
    return ((@($names.Keys) | Select-Object -First 80 | ForEach-Object { "- $_ (last used $($names[$_]))" }) -join "`n")
}

function Get-KnownFixContext {
    param([Parameter(Mandatory)][string] $Path)
    if (-not (Test-Path -LiteralPath $Path)) { return $null }
    try { $ctx = Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json } catch { return $null }
    if (-not $ctx.PSObject.Properties['entries'] -or -not $ctx.entries) { return $null }
    $lines = foreach ($e in $ctx.entries) {
        $links = if ($e.PSObject.Properties['links'] -and $e.links) { ($e.links -join '; ') } else { 'no PR/issue link recorded' }
        $note  = if ($e.PSObject.Properties['operatorNote']) { $e.operatorNote } else { '' }
        $appl  = if ($e.PSObject.Properties['applicability']) { $e.applicability } else { '' }
        "- $($e.title) [$($e.operatorStatus)]: $note Applicability: $appl Fix PRs/refs: $links"
    }
    return ($lines -join "`n")
}

function Get-QBNamedContactBlock {
    <#
    .SYNOPSIS
    The "name one person beside the team" instruction plus the roster pairs the model may use.
    #>
    param([hashtable] $TeamContacts = @{})
    $lines = foreach ($team in ($TeamContacts.Keys | Sort-Object)) {
        "$team -> GitHub @$($TeamContacts[$team].github); Teams @$($TeamContacts[$team].teams)"
    }
    $roster = if ($lines) { $lines -join "`n" } else { '(no contact roster available this run)' }
    return @"
NAMED CONTACT - always call out ONE person next to the team, never the team alone.
Put the person immediately after the team handle in the same owners list. On GitHub use
the login: @dotnet/<team> @<github-login>, Please check the build failure. ...
In a Teams channel use the display name: @dotnet/<team> @<Display Name>,
Please check the build failure. ...
Use the GitHub login when the destination is GitHub, and the Teams display name when the
destination is a Teams channel. Use exactly these pairs and invent no one:
$roster
If a team has no entry above, ping the team alone rather than guessing a person.
"@
}

function Get-QBReferenceBlock {
    <#
    .SYNOPSIS
    Names the reference documents staged for a stage, so the stage reads them instead of relying on this prompt alone.
    #>
    param([string[]] $Path = @())
    $documents = @($Path | Where-Object { $_ })
    if ($documents.Count -eq 0) { return '' }
    $purpose = @{
        'triage.md'          = 'who owns a failure, where it is reported, and what is not a destination'
        'report-contract.md' = 'the overall report: its sections, columns, destinations and action wording'
    }
    $lines = foreach ($document in $documents) {
        $leaf = Split-Path -Leaf $document
        $why = if ($purpose.ContainsKey($leaf)) { $purpose[$leaf] } else { 'rules for this stage' }
        "- $document - $why"
    }
    return @"
REFERENCE DOCUMENTS (this skill's own rules, staged for this run; read each once with view before you write anything):
$($lines -join "`n")
"@
}

function Get-BuildHealthPrompt {
    param(
        [Parameter(Mandatory)][ValidatePattern('^\d{4}-\d{2}-\d{2}$')][string] $Date,
        [string] $EvidencePath,
        [string] $InternalStatus = '<exact internal.status from the digest>',
        [string] $KnownFixContext = '',
        [string] $OperatorContext = '',
        # The repo -> owner packet resolved from this checkout (Get-VmrQBOwnershipBlock). Mandatory: the prompt tells the
        # model to name owners from it, so a call that forgets it would leave the model guessing handles.
        [Parameter(Mandatory)][ValidateNotNullOrEmpty()][string] $OwnershipBlock,
        [string[]] $ReferenceDocument = @()
    )
    $references = Get-QBReferenceBlock -Path $ReferenceDocument
    @"
I'd like to understand the health of the public and internal dotnet-unified-build pipelines in the last 24 hours. Make sure to use the pipeline-investigation skill.

Then present the result as a complete, self-contained Markdown report for $Date titled exactly:
# Build Health Monitoring - $Date

MODEL-READY RUN EVIDENCE: $EvidencePath
Read this diagnostic digest ONCE using view with forceReadLargeFiles=true.
Each problemBuilds entry already includes real diagnostic quotes and affected tasks.
Only open its detailPath when diagnostics is empty, additionalDistinctDiagnostics is
nonzero, or a specific independent error needs clarification. Do not routinely open
every per-build packet. The digest contains the current run's authenticated, paginated
collection, distilled OUTSIDE the model. DO NOT open sources.json, failure-tasks.json,
root-logs.json, raw inventories, whole timelines, or artifacts/current-run.json:
those large raw indexes are preserved for audit, not report generation.
Do not re-query AzDO, acquire tokens, or substitute cached reports.
Load the requested pipeline-investigation skill once. Do not restart this workflow,
read Copilot session history/events, or reload already read evidence. Synthesize from
the digest and its exact quotes. Do not reconstruct unavailable checkpoint summaries.
The collector's status, not tool permissions in this child session, determines whether
internal access worked. If internal.status is available, analyze its data; do not claim
authentication was unavailable.

SCOPE - identify and route actual initiating failures:
- Cover EVERY internal failed/partiallySucceeded/canceled build in requiredProblemBuildIds,
  retaining all independent failures. Read the selected root-log excerpts; open the
  relevant retained log LINE RANGES when a wrapper error is insufficient; excerpts give
  the path and line number. Never use forceReadLargeFiles on a raw task log. No new downloads,
  binlogs, rebuilding, or delegated deep investigations are needed.
- Follow the already retained child-promotion evidence. A Darc exit code, SBOM missing
  paths, or generic MSB3073/Bash exit 1 is not a root when its source log has a real error.
- Separate internal/release/* from main/release/* within the internal pipeline. Preserve
  manual cancellations, timeout limits, agent loss, nonblocking warnings, and partial
  evidence. Do not conflate CI-job feed 401 errors with the investigator's authentication.
- Reuse the common cutoff for both pipelines. Active statuses have their own observation
  time and do not enter completed-build pass rates. Aim to finish within about 15 minutes.

Requirements:
- State the observation window and the UTC cutoff.
- Give a pass/fail summary for BOTH the public (dnceng-public/public) and internal (dnceng/internal) dotnet-unified-build pipelines.
- Include a table of failed builds with direct https://dev.azure.com build result links.
- Group failures by root cause and note which are infra vs product.
- If the COLLECTOR reports authentication_required or unavailable, reproduce its specific
  error and sign-in instructions from the compact index. Never label missing internal data healthy or complete.
- Write the public tables normally. The runner renders authoritative internal totals
  and per-build rows from the following required payload; do not invent separate internal
  counts or drop independent root failures from the payload.

Append exactly one machine-readable comment (valid JSON; no Markdown fences inside it):
<!-- VMR-INTERNAL-FINDINGS
{"schemaVersion":1,"reportDate":"$Date","sourceStatus":"$InternalStatus","builds":[{"buildId":123,"roots":[{"id":"stable-diagnostic-key","description":"Plain-language observed failure; distinguish root from downstream effects.","quote":"An exact diagnostic substring from this build's retained root log or timeline issue, at least 12 characters.","ownerTeams":["@dotnet/team"],"nonBlocking":false,"priority":1}]}]}
VMR-INTERNAL-FINDINGS -->
sourceStatus is collector-owned metadata, not an assessment of the build result.
If inventory is accessible but some diagnostic reads were limited, its status is
"partial", not "available"; retain all usable evidence and the recorded coverage gap.
Replace the example with real values. Include exactly requiredProblemBuildIds, one
build entry each, with all independent roots. Reuse the same stable root id when the
same diagnostic recurs across builds; use different ids for independent failures.
SEVERITY - each diagnostic carries severity "terminating" or "nonTerminating", and
terminatingDiagnosticCount says how many terminating ones that build has. A root finding
must quote a TERMINATING diagnostic whenever the build has at least one. Never report a
nonTerminating line as an independent failure: tools log recoverable problems using the
word "error" and keep going. A Roslyn analyzer that fails to load, a missing optional
dependency, or a retried download is noise unless it is the only diagnostic in the build.
Cite nonTerminating text only as supporting context inside a terminating finding.
Two diagnostics in the same job are usually ONE failure with a symptom, not two findings;
split them only when each has its own terminating diagnostic.
DID IT ACTUALLY FAIL THE BUILD - a failed task does not always fail the build. When the
build result is "partiallySucceeded", the failing tasks did NOT block it. Still record
those findings, but mark each one nonBlocking:true in the payload. Non-blocking findings
are monitor-only: the summary row says no action is needed and names no owner to ping,
and they never appear in Top actions. Only a finding on a build whose result is "failed"
or "canceled" is an escalation. Set nonBlocking:false for those.
RECONCILE THE TWO LISTS - the document's "Priority actions" and this payload must describe
the same set. Every non-monitor-only finding in the payload needs a matching priority
action, and every priority action needs a matching finding. Do not cap the priority list
at five and silently drop a real failure; if there are more, list them all.
ALREADY-OWNED FAILURES ARE NOT TOP ACTIONS - when the operator knowledge below records an
existing tracker, IcM, or named owner who has taken a failure, that failure is already
being worked. Keep its summary row and keep its links, but set priority to 0 and keep it
out of "Priority actions". A QB action item is work the QB still has to route; repeating
an item somebody already owns is noise that buries the failures that need attention. This
overrides the reconcile rule above: such a finding needs no matching priority action.
NAME THE DIAGNOSTIC, NOT THE SURROUNDING TEST - describe a failure by the error that
actually fired, not by whichever test or script happened to host it. An installer
validation leg that fails with NETSDK1045 is a target-framework failure, not a "dnx
regression", even though the dump contains `dotnet dnx ... Usage:` console noise. Check
which diagnostic is terminating before naming the finding.
OWNERSHIP PACKET (resolved from this checkout's VMR QB Guide and the routing registry; the only handles you may use):
$OwnershipBlock

MANDATORY: every root needs a non-empty ownerTeams. Use the repo-to-owner table above
when the failure maps to a product repo. When it does not - 1ES/Guardian/SBoM task
tooling, agent or pool problems, container images, or other VMR build infrastructure -
use ["$((Get-VmrQBRouting).UnmappedLabel)"]: it names no team, and no team is a default. Only a manual build
cancellation may omit ownerTeams.
For a manual cancellation, describe and quote the cancellation evidence rather than
inventing a failure. If no problem builds were collected, builds is an empty array.
A build rejected at YAML pre-flight has no timeline; its retained evidence is the
"Pipeline validation" record, so quote that validation message as the initiating
diagnostic instead of reporting the build as undiagnosable.
This is a READ-ONLY report. Do not merge, approve, retry, or modify anything.
RECORDED OPERATOR CONTEXT (apply only to the stated failure and scope; latest corrections take precedence):
$KnownFixContext
$OperatorContext
Keep operator explanations separate from verbatim diagnostic quotes. A diagnostic's
explanation field is not a quote; copy only the quote field into the findings payload.
$references
Return only the final Markdown report as your last message.
"@
}

function Get-CodeFlowPrompt {
    param(
        [Parameter(Mandatory)][ValidatePattern('^\d{4}-\d{2}-\d{2}$')][string] $Date,
        [string] $EvidencePath,
        [string[]] $ReferenceDocument = @()
    )
    $references = Get-QBReferenceBlock -Path $ReferenceDocument
    @"
I'd like to understand the health of dependency flow into the dotnet/dotnet repo. Make sure to use the flow-analysis skill.

Then present the result as a complete, self-contained Markdown report for $Date titled exactly:
# Code Flow Monitoring - $Date

Requirements:
- Read the compact run context at $EvidencePath for pipeline coverage and observation
  cutoff. Do not use a stale artifacts/current-run.json. Label live Maestro/GitHub
  observations with their actual retrieval time.
- Cover forward flow (repo -> VMR) health for the key repos: aspnetcore, efcore, runtime, sdk, windowsdesktop, winforms, wpf, arcade, roslyn - across main and active release/* branches.
- Call out any stale (>7 days) or blocked code flow, with the real "commits behind" number (never the "~builds behind" value).
- MANDATORY before calling a forward-flow subscription stale: a commits-behind count alone
  is NOT staleness. Classify the pending commits first. Commits authored by
  dotnet-maestro[bot] titled "Source code updates from dotnet/dotnet" are BACKFLOW - the
  VMR's own content returning to the product repo - and contain no product-repo work to
  send back. If EVERY pending commit is backflow and the subscription's latest outcome is
  "NoUpdate - no source code updates detected", that subscription is WORKING NORMALLY and
  fully up to date: report it as normal, raise no action, and ping no owner.
  Only report real staleness when at least one pending commit is genuine product-repo work
  (a human/product author, not a backflow title), or the outcome is Failed, or a real
  forward-flow PR is open and blocked. Confirm by comparing the VMR src/source-manifest.json
  commitSha for that repo against the product-repo branch.
- Maestro's cross-validation flags are hints, not proof. "Commit reachable: No" and
  "N PRs merged since last applied" have both produced verified false positives. Before
  repeating such a claim, confirm it with a real commit comparison, and say which branch
  any cited PRs actually targeted.
- State oscillation (MergingPullRequest <-> ApplyingUpdates)
  is a Maestro bookkeeping symptom. When nothing genuine is pending it has no product
  impact - note it at most as dashboard noise, never as blocked product flow.
- List open code flow PRs you find with their CI status and a direct github.com link.
- Note the Maestro dashboard (https://maestro.dot.net/codeflows?branch=main) as the visual reference.
This is a READ-ONLY report. Do not trigger subscriptions, merge, or modify anything.
$references
Return only the final Markdown report as your last message.
"@
}

function Get-OverallPrompt {
    param(
        [Parameter(Mandatory)][ValidatePattern('^\d{4}-\d{2}-\d{2}$')][string] $Date,
        # The same repo -> owner packet the build-health stage received (Get-VmrQBOwnershipBlock), so both stages and the
        # validators resolve owners from one place.
        [Parameter(Mandatory)][ValidateNotNullOrEmpty()][string] $OwnershipBlock,
        [hashtable] $TeamContacts = @{},
        [string[]] $ReferenceDocument = @(),
        [string] $BuildHealthPath,
        [string] $CodeFlowPath,
        [string] $PrevBuildHealthPath,
        [string] $PrevCodeFlowPath,
        [string] $KnownFixContext,
        [string] $PreviousFindingNames,
        [string] $IssueHistoryContext,
        [string] $OperatorContext = '',
        [string] $EvidencePath,
        [string] $IssueEvidencePath,
        [string] $InternalFindingsPath
    )
    $bh   = if ($BuildHealthPath)     { $BuildHealthPath }     else { '(build health report was not produced today)' }
    $cf   = if ($CodeFlowPath)        { $CodeFlowPath }        else { '(code flow report was not produced today)' }
    $pbh  = if ($PrevBuildHealthPath) { $PrevBuildHealthPath } else { 'none' }
    $pcf  = if ($PrevCodeFlowPath)    { $PrevCodeFlowPath }    else { 'none' }
    $knownFix = if ($KnownFixContext) { $KnownFixContext } else { '(no operator-provided known-fix context this run)' }
    $previousNames = if ($PreviousFindingNames) { $PreviousFindingNames } else { '(no earlier overall summaries found; choose clear names and keep them stable from now on)' }
    $issueHistory = if ($IssueHistoryContext) { $IssueHistoryContext } else { '(issue history store is empty; every finding today is effectively unverified history)' }
    $routingGuide = $OwnershipBlock + "`n`n" + (Get-QBNamedContactBlock -TeamContacts $TeamContacts)
    $references = Get-QBReferenceBlock -Path $ReferenceDocument
    @"
You are compiling the VMR Quarterback (QB) daily OVERALL report for $Date. It summarizes steps 1-4 of the VMR QB daily checklist. Work only from the evidence below plus the gh queries requested. This is READ-ONLY: do NOT merge, approve, close, comment, post to Teams, trigger subscriptions, or retry anything.

STEP 1 and STEP 2 - read today's two specialist reports IN FULL using the view tool with forceReadLargeFiles=true:
- Build Health Monitoring (step 1): $bh
- Code Flow Monitoring (step 2): $cf

COMPACT CURRENT RUN EVIDENCE: $EvidencePath
INTERNAL FINDINGS: $InternalFindingsPath
ISSUE / OWNER EVIDENCE: $IssueEvidencePath
Read these compact files ONCE using view with forceReadLargeFiles=true, not sources.json, failure-tasks.json, root-logs.json, raw
inventories, whole comment histories, or artifacts/current-run.json. Internal coverage and statistics
come from the collector and the verified findings. Do not omit the internal failures.
The issue evidence includes open AND closed operational issues, bodies, comments, known
fix PRs, and selectable triage candidates. Check these before recommending new issues.
Read relevant candidate model-issue-*.json files; use line-range reads/text search only
for a specific missing comment, never a full large comment history. An aggregate tracker can already contain this exact build/log
even when its original body describes another failure.

STEP 3 - Merge green code flow PRs. List current open code flow PRs and their CI status:
  gh pr list --repo dotnet/dotnet --search "author:app/dotnet-maestro" --state open --limit 60 --json number,title,headRefName,baseRefName,url,createdAt
  For a few of the most relevant, check CI with: gh pr checks <number> --repo dotnet/dotnet
  Check remaining merge gates with: gh pr view <number> --repo dotnet/dotnet --json url,baseRefName,isDraft,mergeable,mergeStateStatus,reviewDecision,changedFiles,body
  Green CI alone is not merge-ready: report conflicts, required reviews, pending Maestro gates, and unsafe/high-file-count flows separately for each PR. Do not assume every release/* branch is locked; follow the guide and verified branch restrictions.

STEP 4 - Merge re-bootstrap PRs. List open re-bootstrap PRs from dotnet-bot (titles like ".NET X.Y.Z-... Updates"):
  gh pr list --repo dotnet/dotnet --search "author:dotnet-bot" --state open --limit 40 --json number,title,baseRefName,url,createdAt
  Note their CI status.

FIX PR ANALYSIS - for EACH failure/issue you identify (build failures from step 1 and code-flow problems from step 2), determine whether a pull request already exists that fixes it. This is a required part of the report.
1. First consult the KNOWN FIX CONTEXT below (operator-verified fixes and their PRs). Match an issue to a known fix ONLY when the diagnostic, branch, and scope match that entry's applicability. A fix being "in" for one symptom does NOT cover a different symptom the entry explicitly excludes - e.g. the 240-minute compiler-server-shutdown fix does NOT cover unrelated 30/60-minute validation task timeouts.
2. When no known entry clearly covers an issue, do a bounded GitHub search for candidate fix PRs (in dotnet/dotnet and, for root-cause fixes, the owning product repo such as dotnet/roslyn, dotnet/runtime, dotnet/arcade, dotnet/sdk):
     gh search prs --repo dotnet/dotnet "<short error keywords>" --state merged --limit 8 --json number,title,url,state,closedAt
     gh search prs --repo dotnet/dotnet "<short error keywords>" --state open --limit 8 --json number,title,url,state
   Keep it to a few targeted searches per distinct root cause; do not exhaustively search.
3. Record the result in the new "Existing fix PR (status)" column as ONE of:
   - a direct PR link + status, e.g. "dotnet/dotnet#1234 merged 2026-01-15 (rollout unverified)" or "dotnet/roslyn#5678 open";
   - "None found" when a bounded search finds no matching PR;
   - "N/A" when the issue is a transient infra/network blip that a code fix would not address.
   A merged fix PR is NOT proof of deployment/recovery - qualify with "(rollout unverified)" unless post-fix green builds are already evident. Never claim a fix covers an issue that the known-context applicability explicitly excludes.

NEW ISSUE - check recorded issue history first, then yesterday's specialist reports if they exist (read with view if present):
- Yesterday build health: $pbh
- Yesterday code flow: $pcf
Earlier history takes precedence over absence from yesterday's reports.
- Use "New Issue = N" if the finding appears in recorded issue history or any supplied earlier report,
  even if it was absent yesterday. A recurrence is not a new finding.
- Otherwise use "New Issue = Y", including when history or prior-report coverage is incomplete.
  This means not previously reported in the available records, not proof that the failure first occurred today.
  Note any history or coverage gaps without changing the value to "?".

OUTPUT - produce a complete, self-contained Markdown report titled exactly:
# VMR QB Daily Overall Report - $Date

1. Start with "## Top actions for the QB today" - an ordered list, most urgent first, following the action format below.
2. Then "## Summary" with one or more tables using EXACTLY these six columns and this header:

| Issue category | Builds / PRs (links) | New Issue (Y/N) | Existing fix PR (status) | Action as VMR QB (who / where to ping) | Links that QB reported |

CROSS-REFERENCE THE BUILD HEALTH REPORT: the internal findings payload's bhReferences map
comes from the VISIBLE numbered build-health "Priority actions" list. Use that map, never
JSON finding order or a separate counter. Several findings may share one visible action.
Start the
Issue category cell of every internal row with that number as "BH #N - ", for example
"BH #2 - NETSDK1045 target framework failure". Use "BH #-" for internal rows with priority 0
and for code-flow/PR rows, which have no build-health priority. Also start each matching
numbered item in Top actions with the same "BH #N" tag so the two documents line up.

ROW ORDER: sort the summary rows to follow the "## Top actions for the QB today" list exactly.
The row for top action 1 comes first, then top action 2, and so on, so the QB can read the
action list and walk straight down the table in the same order. Rows with no top action -
already-owned failures, monitor-only findings, and code-flow/PR rows - come after all
action-backed rows, most urgent first. Never reorder by "New Issue"; the action order wins.

LINKS THAT QB REPORTED - where this exact failure was ALREADY reported, so the QB can find
the existing thread fast. Use Markdown links to the GitHub issue, PR, or issue-comment
permalink from the collected issue evidence (buildReferences sourceUrl values give the exact
comment permalink). Write "None found" when the bounded evidence shows no existing report.
Teams chat posts have no API and cannot be linked automatically - never invent a Teams URL;
the runner merges any operator-supplied Teams permalinks itself. The runner also appends
verified report links from the collected evidence, so do not guess.

TOP ACTION FORMAT:
- Start EVERY action and sub-bullet directly with Markdown links to the affected builds or PRs, before the description or owner. Put the complete item on one line.
- One issue and one next step per item. Never combine unrelated failures just because they share a pass-rate statistic, branch, owner, or build. Multiple builds with the SAME failure may share an item; each sub-bullet must still start with its own evidence link.
- Keep the pass-rate overview in a separate short paragraph, not an omnibus escalation bullet. Separate EMSDK signing, missing-output cascades, installer/RPM failures, compiler exits, validation timeouts, and socket exhaustion.
- Give each PR its own item when its blocker/action differs: a conflicting green PR, a PR awaiting a Maestro gate, and an iOS NativeAOT clang crash are separate matters. Do not write a blanket "do not merge any PR" action.
- WindowsDesktop and WPF subscription findings get separate linked items. If no PR/build exists, use the verified issue or Maestro subscription link and explicitly say no PR/build exists; never invent one.
- Name the owner and finish each item with an exact destination: "Where: Teams - <channel>." or "Where: GitHub - [owner/repo#N](verified PR or issue URL)." A failure that belongs to an external support destination in the ROUTING GUIDE (for example 1ES support) finishes with "Where: Support - [<name>](<url>)." exactly as the guide gives it.
- When no owner can be identified, say "$((Get-VmrQBRouting).UnmappedLabel)" in the item BEFORE the destination (for example "... Owner: $((Get-VmrQBRouting).UnmappedLabel). Where: None - owner not identified.") and finish with exactly "Where: None - owner not identified." Nothing may follow the Where. Never invent a team or a person to fill the gap.
- For a waiting item with no outreach needed, finish with "Where: None - monitor only." If there are no current actions, write exactly "No QB action required." instead of inventing a ping.
- Top actions holds ONLY actions: every list item in it, numbered or bulleted, starts with an evidence link and finishes with its "Where:" destination, with nothing after it. Status that needs no action goes in the Summary table as code-flow/PR rows ("Where: None - monitor only.<br>Action: <factual status>"), or in at most one short plain paragraph (not a list) after the Top actions list: which PRs are or are not merge-ready (step 3), that no re-bootstrap PR is open (step 4), pass rates. Never write a "Step 3" or "Step 4" heading or bullet list inside Top actions.

SUMMARY ACTION CELL FORMAT:
- Each row covers one issue; preserve the other four columns.
- For outreach, use this exact layout INSIDE the action cell, including the literal <br>:
  Where: Teams - <exact channel>.<br>Copy/paste: "@owner, Please check the build failure. <what failed and the observed error>: <evidence links>"
  or:
  Where: GitHub - [owner/repo#N](verified PR or issue URL).<br>Copy/paste: "@owner, Please check the build failure. <what failed and the observed error>: <evidence links>"
- For an external support destination, use: Where: Support - [<name>](<url>).<br>Copy/paste: "<name>, Please check the build failure. <what failed and the observed error>: <evidence links>"  (the name is the support destination's name, not a @handle).
- When no owner can be identified there is nobody to ping and no copy/paste message. Use: Where: None - owner not identified.<br>Action: $((Get-VmrQBRouting).UnmappedLabel). <factual statement of what failed and what evidence is missing to identify an owner>  Name no team and no person; do not write instructions.
- For a PR/flow issue rather than a build failure, use "Please check this issue. <observed problem or blocker>" with the same destination, owners, and evidence-link format.
- After the brief check request, state ONLY what failed or the observed status. Do not tell the owner to correct, fix, change, update, backport, merge, approve, rerun, verify rollout, or do anything else. A factual "review is required" is fine; "please review/approve this PR" is not.
- The entire message must be enclosed in plain double quotes. Use single quotes for any diagnostic quoted inside it. Address verified @team/@person handles (comma-separated if multiple); use "First Responders" by name when that is the known owner rather than inventing a handle.
- Keep the brief check request AND factual description together to AT MOST 30 words; aim for 25 or fewer when possible. Owner handles and evidence links do not count toward that limit. Use plain language, not proposed remedies or investigation instructions.
- End the quoted message with direct Markdown build/PR links from that row's evidence. Include a matching known-fix link from the fix column when useful. Never output placeholder owners, URLs, or "<paste link>".
- All report hyperlinks must be HTTPS web URLs, never file:// URIs, drive paths, or relative Markdown files. The local specialist files are attached to the email, not usable web links.
- For a monitoring gap without an available build/PR, link the affected service page and explicitly state the missing inventory. For unavailable internal access use https://dev.azure.com/dnceng/internal/_build; for Maestro coverage use the verified subscription/dashboard URL. Do not invent a build ID to satisfy the link requirement.
- State ONE primary reporting destination, not "Teams or GitHub". Prefer the existing GitHub issue/fix PR for an already tracked issue; use the named Teams channel for coordination or an untracked incident. GitHub team handles identify owners; do not claim they are verified Teams mentions or invent Teams chat URLs.
- For a waiting/no-action row, use "Where: None - monitor only.<br>Action: <factual status or pending event>" with no copy/paste message or instruction. For example: "Owner reports a transient restore failure; rerun pending." The owner-assessed transient ASP.NET restore occurrence does not need another escalation or an unverified recovery claim.

Table rules:
- "Builds / PRs (links)" MUST contain direct build/PR links. When none exists, explicitly state that and supply the verified issue/subscription or monitoring-evidence link instead.
- "Existing fix PR (status)" MUST reflect the FIX PR ANALYSIS above (a PR link + status, "None found", or "N/A").
- "Action as VMR QB" MUST follow the quoted-message and destination format above, not prose telling the reader to compose a message.
- Keep known-fix links, merge status, and rollout uncertainty in the Existing fix PR column. Even when a fix is known, the quoted action message reports only the failure/status, not instructions to apply the fix or check its rollout. Respect operator guidance and avoid duplicate escalation. Do not claim version-pin speculation is a verified fix or treat SBOM missing-input cascades as independent ESRP signing failures.
- Use Servicing chat for release/10 servicing coordination. Use Preview/RC chat only for verified preview/RC work, not every release/* branch. VMR-wide incidents go to Unified Build Technical and General Discussions. First Responders is for Helix issues ONLY. Failures with no GitHub team and no Teams channel (the support destinations in the ROUTING GUIDE, for example 1ES Pipeline Templates task failures) use that support destination, never a dnceng Teams channel that will reject them.
- A PULL REQUEST OR ISSUE OPENED BY A TEAM MEMBER IS MONITOR ONLY. When a row's evidence is a
  GitHub PR or issue whose author is a Microsoft/.NET team member - any author_association of
  MEMBER, OWNER or COLLABORATOR, for example dotnet/dotnet#1234 by octocat - that author already
  owns it and is watching their own CI. The QB does not chase them. Set the destination to
  "None - monitor only", state that the author owns it, name no one to ping, and keep it out of
  Top actions. This applies even when the PR's CI is red: a failing check on a member's own PR is
  the author's to fix. Only raise an action when the PR is from dotnet-maestro[bot] (codeflow,
  which has no human author), from an outside contributor needing triage, or when the failure
  also affects rolling/official builds beyond that PR.
- Cover, at minimum: build failures (step 1), code flow staleness/blocked (step 2), green code flow PRs ready to merge (step 3), and re-bootstrap PRs (step 4).
- If a specialist report is missing, add a row noting that gap with the owner "$((Get-VmrQBRouting).UnmappedLabel)".
- Preserve qualifications such as "low disk observed; cause unconfirmed" and "no matching fix found". Do not turn correlations into proven causes to shorten a message.

KNOWN FIX CONTEXT (operator-verified; apply only within each entry's stated applicability):
$knownFix

NAMES USED IN RECENT REPORTS - reuse these EXACTLY for the same failure:
$previousNames
A recurring failure must keep the SAME Issue category text it had before, character for
character. Do not reword, reorder or "improve" it: "Azure source-index subscription missing"
must not become "Source-index Azure subscription missing", and a Bcl.AsyncInterfaces analyzer
load failure must not alternate between "Public API analyzer dependency missing" and
"Roslyn analyzer missing Microsoft.Bcl.AsyncInterfaces". A renamed finding reads as a brand
new problem, hides that it is recurring, and makes the operator re-investigate it. Only
invent a name when nothing in the list above describes the same failure. The BH number may
change between days - the name must not.

ISSUE HISTORY (recorded across previous reports; status, how long it has recurred, owner, tracker):
$issueHistory
Use this to set "New Issue" correctly: a finding listed here is NOT new, however it is worded
today. Prefer the recorded tracker over "None found", and prefer the recorded owner over
re-deriving one. An issue whose status is resolved, noise or wontfix must not become a top
action; report it in the table and move on. A long-recurring issue with no tracker is the
strongest candidate for filing one.

REQUIRED INTERNAL ACTION PLAN:
Append one valid JSON comment, separate from the visible summary table:
<!-- VMR-QB-ACTIONS
{"schemaVersion":1,"reportDate":"$Date","actions":[{"findingId":"exact stable id from INTERNAL FINDINGS","buildIds":[123],"decision":"existing_issue","issueUrl":"https://github.com/owner/repo/issues/123","reason":"Exact match, related historical recurrence, or why monitoring is sufficient.","assignee":null,"assigneeBasis":null,"title":null,"body":null,"searchBasis":null,"comment":null}]}
VMR-QB-ACTIONS -->
- One entry for EACH distinct internal finding id, covering ALL affected internal build
  IDs for that finding. Keep independent failures separate. Do not repeat public cases
  here; this payload is the internal action supplement.
- If INTERNAL FINDINGS is unavailable because build-health generation failed, keep
  actions empty and state the missing analysis prominently. Do not reconstruct findings
  or invent issue recommendations from a failed/missing verified assessment.
- Allowed decisions: new_issue, existing_issue, possible_recurrence, monitor_only,
  needs_triage. An existing issue URL must occur in the collected issue evidence.
- Before new_issue, examine the operational-issue inventory (open AND closed), relevant
  bodies and comments, exact diagnostic and representative build-ID search results.
  An incomplete GitHub collection means needs_triage, not "no issue exists".
- ALREADY-TRACKED FINDINGS ARE NEVER new_issue. Check the ISSUE HISTORY block above FIRST.
  If a finding appears there with a tracker URL, or with status owned/resolved/noise/wontfix,
  it is already filed and someone owns it. Use existing_issue with that URL and write NO
  title/body draft. Today's GitHub search not returning it proves nothing: the tracker may
  live in another repo, be worded differently, or be an IcM that GitHub cannot see. Filing a
  duplicate wastes the owner's time and splits the history. Darc promotion artifact
  download/channel failure is the standing example - it is tracked and must not be re-drafted.
- A closed related issue is possible_recurrence, not proof of a new defect or proof the
  current branch is fixed. State differing branch/test scope and uncertainty.
- If the same build/log is already in an open issue's comments, use existing_issue and
  leave comment null: no duplicate issue or repeated evidence post.
- new_issue requires title, body, and searchBasis. Use the Unified Build Operational
  Issue title prefix and a complete Markdown body with ### Error, ### Links,
  ### Root Cause, ### Blocking, and ### Affected Releases; root cause can be unknown.
  Include actual build/log URLs and owner teams, not a prescribed speculative fix.
- MANDATORY: the ### Links section of every title/body draft must list the internal
  build URL https://dev.azure.com/dnceng/internal/_build/results?buildId=<id> for EVERY
  id in that action's buildIds - all of them, even when there are a dozen or more.
  Do not abbreviate to representative or first/last examples, and do not write "and N
  more". A draft missing any affected build URL is rejected and the run fails.
- If a needs_triage/possible_recurrence case still needs a new tracker decision,
  provide a conditional title/body/searchBasis draft as well, clearly saying the
  existing tracking or scope needs owner confirmation before it is opened. Do not
  invent a definitive root cause. Do not create drafts for monitor-only cases or
  findings already exactly tracked. The conditional draft makes an unresolved
  actionable failure usable rather than just saying "needs triage".
- For existing_issue/possible_recurrence, provide a factual proposed comment only when
  there is genuinely new evidence not already posted. Otherwise comment must be null.
- Suggested assignee is a PERSON from issue-evidence.assigneeCandidates with
  assignable=true and a stated evidence-backed assigneeBasis. Label them initial triage,
  not a confirmed defect owner. Use null when unverified; teams are not user assignees.
- Respect owner-assessed transient/rerun-pending guidance and manual cancellations.
  Do not create tickets for downstream SBOM/artifact symptoms or intentional cancellation.
The runner renders these decisions and copy-ready issue/comment drafts into the
overall report AND the HTML email. Nothing is posted automatically.

ROUTING GUIDE:
$routingGuide

RECORDED OPERATOR CORRECTIONS (apply only to the stated failure and scope):
$OperatorContext
$references
Return only the final Markdown overall report as your last message.
"@
}
