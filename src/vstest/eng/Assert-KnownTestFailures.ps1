<#
.SYNOPSIS
    Gates signing on UNCLASSIFIED test results rather than on zero test failures.

.DESCRIPTION
    This line runs Build -> Package -> Test -> Sign, which is the shipping order: nothing is signed
    that failed its tests. Measured across four runs, every non-passing result on this branch is
    environmental or a harness defect and none is product-shaped - so gating on "zero failures" gates
    on properties of the build agent and the line delivers nothing at all. A gate that cannot open is
    not a gate, it is a stop.

    So the gate is on UNCLASSIFIED results. A committed baseline names every known non-passing test
    together with the failure signature it was classified on, and this script blocks unless every
    non-passing result matches one.

    FOUR PROPERTIES THAT MATTER, each of which exists because this project was bitten by its absence:

    1. IT MATCHES ON A SET, NEVER ON A COUNT. "at most 18 failures" permits substitution: a new
       product regression can take a retired failure's place and the count never moves.

    2. IT MATCHES ON NAME **AND** SIGNATURE. A baseline keyed on the test name alone makes the name a
       proxy for the cause. The same test failing next month for a different reason would be absorbed
       silently, and it would be absorbed in the FALSE-CLEAN direction - the result looks smaller and
       tidier. A name co-occurs with a cause; it is not the cause. Name matches and signature does
       not => BLOCK, reported as its own category.

    3. IT FAILS CLOSED ON AN EMPTY OR ABSENT RESULT SET. No .trx files, or files containing zero
       results, BLOCKS. An absence rendered as a pass is this project's signature failure mode: a
       publish step was once green over zero attached runs, and a results query once returned
       everything because its filter was accepted and ignored. A gate that passes because it found
       nothing to check is the same defect wearing the same colour.

    4. IT REPORTS BASELINE ENTRIES THAT DID NOT FIRE. Every exclusion this project has met rotted
       because nothing noticed when its justification expired. A stale entry does not block - it may
       simply be a test that now passes - but it is printed, so the expiry is observable rather than
       silent. This is the only part of the design that attacks that pattern instead of warning
       about it.

    The full not-passed set is printed on every run, passing or blocking, so it can travel with any
    delivered artifact.

.PARAMETER ResultsDirectory
    Directory searched recursively for .trx files.

.PARAMETER BaselinePath
    The committed baseline JSON.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)][string] $ResultsDirectory,
    [Parameter(Mandatory = $true)][string] $BaselinePath,
    [string] $ExpectedProductVersionPrefix = ''
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Write-Section([string] $Text) {
    Write-Host ''
    Write-Host "=== $Text ==="
}

if (-not (Test-Path $BaselinePath)) {
    Write-Host "##vso[task.logissue type=error]Baseline not found: $BaselinePath"
    exit 1
}

$baselineJson = Get-Content $BaselinePath -Raw | ConvertFrom-Json

# Force enumeration explicitly. Windows PowerShell 5.1's ConvertFrom-Json returns a JSON array as a
# SINGLE object, where PowerShell 7 enumerates it - and the failure that causes is silent and
# plausible. Build 3064083 loaded "1 entries" from a 24-entry file, and every comparison of the form
#   $_.test -eq $name
# then ran against an ARRAY of names, which returns the matching elements rather than a boolean. A
# non-empty array is truthy, so every result appeared to match a baseline entry by name and then
# failed the signature check, and the gate reported 25 results as "known test, NEW failure mode"
# with 0 known. Locally, under PowerShell 7, the same file loaded 24 entries and every case passed.
$baseline = @()
foreach ($item in $baselineJson) { $baseline += $item }
if ($baseline.Count -eq 1 -and $baseline[0] -is [System.Array]) { $baseline = @($baseline[0]) }
Write-Host "Baseline: $BaselinePath ($($baseline.Count) entries)"

# The baseline holds two KINDS of entry, and they are separated by which property they carry rather
# than by position: entries with a 'test' property describe a test result, entries with an
# 'emptyResultFile' property name a .trx that is expected to contain no results at all.
#
# Split on the property, not on the outcome. An entry without an 'outcome' would satisfy
# "$_.outcome -ne 'NotExecuted'" - $null is not equal to that string - and would silently join the
# failure population, where it could never match anything and would be reported stale forever.
$testEntries = @($baseline | Where-Object { $_.PSObject.Properties.Name -contains 'test' })
$baselineEmptyFiles = @($baseline | Where-Object { $_.PSObject.Properties.Name -contains 'emptyResultFile' })

# A test that never ran is not a test that failed. MSTest records both [Ignore] and
# Assert.Inconclusive as the TRX outcome NotExecuted, and this gate used to put them in the same
# population as a genuine failure - so six entries that CANNOT fail were being carried as failures
# and were holding up a signing decision.
#
# They are split out here rather than deleted, because the split is what lets the not-executed
# population be ASSERTED instead of assumed. See the classification section for why that matters.
$baselineFailures = @($testEntries | Where-Object { $_.outcome -ne 'NotExecuted' })
$baselineNotRun = @($testEntries | Where-Object { $_.outcome -eq 'NotExecuted' })
Write-Host "  failures expected    : $($baselineFailures.Count)"
Write-Host "  not-executed expected: $($baselineNotRun.Count)"
Write-Host "  empty result files   : $($baselineEmptyFiles.Count)"
Write-Host "  SUM CHECK  $($baselineFailures.Count) + $($baselineNotRun.Count) + $($baselineEmptyFiles.Count) = $($baselineFailures.Count + $baselineNotRun.Count + $baselineEmptyFiles.Count), entries loaded = $($baseline.Count)"

# --- collect results, failing closed on absence -------------------------------------------------
if (-not (Test-Path $ResultsDirectory)) {
    Write-Host "##vso[task.logissue type=error]Results directory does not exist: $ResultsDirectory"
    Write-Host 'Blocking. A gate that cannot see the results must not pass them.'
    exit 1
}

$trx = @(Get-ChildItem -Path $ResultsDirectory -Recurse -Filter *.trx -File -ErrorAction SilentlyContinue)
Write-Host "TRX files found: $($trx.Count)"
if ($trx.Count -eq 0) {
    Write-Host "##vso[task.logissue type=error]No .trx files under $ResultsDirectory"
    Write-Host 'Blocking. Zero result files is not zero failures.'
    exit 1
}

# --- do these results even belong to THIS servicing line? ----------------------------------------
#
# Added because it caught a real error rather than a hypothetical one. This gate was validated
# against a retained result set that turned out to belong to the 15.9 line, and it produced a
# thoroughly plausible answer: the partition summed, the socket entries matched their signatures,
# and all twelve not-executed results matched expected entries by name. They matched because those
# tests are IDENTICAL on both lines - so the wrong input could not disagree with the baseline.
#
# The only tell was the string "15.9.3-dev" inside one error message, visible solely because this
# gate prints full messages. Nothing about the classification looked wrong.
#
# For a gate that authorises SIGNING, that is the serious version of the mistake: 16.11 artifacts
# could be signed on the strength of another line's test results.
#
# What this check can and cannot do, stated because a control that overstates its reach is worse
# than none: version strings appear only where a test captured runner output, which was 6 of 33
# files. So evidence found and contradicting the expectation BLOCKS; evidence absent is reported
# and does NOT block, because absence of evidence is not evidence of a foreign result set.
#
# A MEASURED LIMITATION, RECORDED BECAUSE IT WEAKENS THE PREMISE ABOVE.
#
# These tokens identify the runner a TEST DROVE, not the build that produced the results. Measured
# on 17.14's own retained set: it carries 17.10.0, 16.11.0, 17.6.2, 15.9.2 and 17.9.0, and ZERO
# 17.14 tokens - every one is a compatibility target, and their own product version never appears.
#
# Two consequences, both real:
#   1. This check PASSES 17.14's results when asked for "16.11.", because their compat tests drive
#      a 16.11 runner. Verified by pointing this script at their directory.
#   2. "Tokens present, none matching" - the condition that blocks - can therefore occur on a
#      GENUINE run, as it would for 17.14 against "17.14.".
#
# It is left blocking anyway, deliberately, because the two errors are not symmetric: wrongly
# blocking a real run costs an investigation, and wrongly authorising signing from another branch's
# tests costs a signed artifact nobody can account for. Failing in the recoverable direction.
#
# FALSIFIER, to be settled by the first genuine 16.11 retained set this pipeline produces: if it
# contains at least one "Tool Version 16.11." token, the premise holds and this stays. If it
# contains none - because this line's tests only ever capture downlevel runners - then the premise
# is false for this line too, and the check must be replaced by one that asserts the SET of test
# projects rather than a version string in captured output.
if (-not [string]::IsNullOrEmpty($ExpectedProductVersionPrefix)) {
    $seen = @{}
    foreach ($file in $trx) {
        $raw = Get-Content $file.FullName -Raw -ErrorAction SilentlyContinue
        if ($null -eq $raw) { continue }
        foreach ($m in [regex]::Matches($raw, 'Tool Version (\d+\.\d+\.\d+[\w\.\-]*)')) {
            $seen[$m.Groups[1].Value] = $true
        }
    }
    $versions = @($seen.Keys | Sort-Object)
    Write-Host "Product version tokens in results: $(if ($versions.Count) { $versions -join ', ' } else { 'none found' })"
    if ($versions.Count -gt 0) {
        $matching = @($versions | Where-Object { $_.StartsWith($ExpectedProductVersionPrefix) })
        if ($matching.Count -eq 0) {
            Write-Host "##vso[task.logissue type=error]These results do not belong to this line."
            Write-Host "Expected a version starting '$ExpectedProductVersionPrefix'; found only: $($versions -join ', ')"
            Write-Host 'Blocking. Signing must not be authorised by another product line''s test results.'
            exit 1
        }
        Write-Host "  matches expected prefix '$ExpectedProductVersionPrefix': $($matching -join ', ')"
        if ($versions.Count -gt 1) {
            Write-Host "  NOTE: $($versions.Count) distinct runner versions are present, so the match above may come"
            Write-Host "        from a compatibility test driving a downlevel runner rather than from this build."
            Write-Host "        This check passed; it did not prove these results are this line's."
        }
    }
    else {
        Write-Host "  no version evidence in these results - NOT blocking on absence, but this check proved nothing."
    }
}

$results = [System.Collections.Generic.List[object]]::new()
$runInfos = [System.Collections.Generic.List[object]]::new()
$totalResults = 0
$declaredExecuted = 0
$declaredTotal = 0
$parentRows = 0
$nestedResults = 0
$perFileDeclaredTotal = @{}
$perFileCounts = @{}
foreach ($file in $trx) {
    [xml] $doc = Get-Content $file.FullName -Raw
    $ns = New-Object System.Xml.XmlNamespaceManager($doc.NameTable)
    $ns.AddNamespace('t', 'http://microsoft.com/schemas/VisualStudio/TeamTest/2010')

    # testId -> fully qualified name, so the gate never keys on a bare method name
    $fqnById = @{}
    foreach ($ut in $doc.SelectNodes('//t:TestDefinitions/t:UnitTest', $ns)) {
        $tm = $ut.SelectSingleNode('t:TestMethod', $ns)
        if ($null -ne $tm) { $fqnById[$ut.id] = "$($tm.className).$($tm.name)" }
    }

    # SELECTS ALL UnitTestResult NODES, NOT ONLY TOP-LEVEL ONES.
    #
    # This was "//t:Results/t:UnitTestResult", which matches only results whose immediate parent is
    # the <Results> element. A data-driven test emits a parent result with its cases nested under
    # <InnerResults>, and those were invisible to this gate. Measured on 3064456:
    #
    #   top-level results parsed        4,898
    #   InnerResults NOT parsed           498
    #   the run's own executed counter   5,273
    #
    # All 498 were passing, so nothing was concealed - but a FAILING data-driven case could not have
    # reached this gate at all, and this gate decides whether signing proceeds.
    #
    # The three numbers do not reconcile: 4,898 + 498 is 5,396, not 5,273. MSTest's counting of a
    # data-driven parent against its cases is not something this script can state a rule for, so it
    # does not assert one - it prints the run's own executed count beside the parsed count so a
    # divergence is visible. Asserting a relationship nobody here can explain would be inventing a
    # justification, which is the error retracted in 88b5b5157.
    $fileCount = 0
    foreach ($r in $doc.SelectNodes('//t:UnitTestResult', $ns)) {
        $totalResults++
        $fileCount++
        if ($r.outcome -eq 'Passed') { continue }

        # A DATA-DRIVEN PARENT IS A PLACEHOLDER, NOT A RESULT. Counted above, excluded here.
        #
        # Its outcome is derived from its children: if one case fails, the parent reads Failed too.
        # Adding both to the not-passed population reports ONE failure TWICE, under two different
        # names - the parent carries the method name, the child carries the parameterised case - and
        # the baseline would then need an entry for each to avoid blocking.
        #
        # This has never fired, because all 498 nested results on this line pass and a parent whose
        # children all pass is itself Passed and skipped above. It is fixed before it fires rather
        # than after: the first failing data-driven case would otherwise arrive as two unclassified
        # results and block signing while looking like two defects.
        #
        # The child is kept because it names WHICH case failed. The parent names only the method.
        if (@($r.SelectNodes('t:InnerResults/t:UnitTestResult', $ns)).Count -gt 0) { continue }

        $msgNode = $r.SelectSingleNode('t:Output/t:ErrorInfo/t:Message', $ns)
        $message = if ($null -ne $msgNode) { ($msgNode.InnerText -replace '\s+', ' ').Trim() } else { '' }
        $fqn = if ($fqnById.ContainsKey($r.testId)) { $fqnById[$r.testId] } else { $r.testName }
        $results.Add([pscustomobject]@{ Fqn = $fqn; Outcome = $r.outcome; Message = $message; File = $file.Name })
    }
    $perFileCounts[$file.Name] = $fileCount

    $nestedResults += @($doc.SelectNodes('//t:InnerResults/t:UnitTestResult', $ns)).Count
    foreach ($top in $doc.SelectNodes('//t:Results/t:UnitTestResult', $ns)) {
        if (@($top.SelectNodes('t:InnerResults/t:UnitTestResult', $ns)).Count -gt 0) { $parentRows++ }
    }

    # The three numbers RECONCILE, and the rule was measured rather than assumed. Over 3064456's 35
    # files:
    #
    #   top-level results                4,898
    #   their nested children              498
    #   all UnitTestResult nodes         5,396
    #   data-driven PARENT rows            111
    #   5,396 - 111                    = 5,285  = Counters/@total       EXACT
    #   5,285 - 12 NotExecuted results = 5,273  = Counters/@executed    EXACT
    #                                    5,273  = passed + failed       EXACT
    #
    # HOLDS ON ONE DATASET THAT ACTUALLY EXERCISES IT. An earlier version of this comment claimed
    # three datasets across two lines, and that was WRONG - 15.9 withdrew their own corroboration
    # after measuring why:
    #
    #   16.11  3064456   5,396 nodes   111 parents   = 5,285 = total    exercises the rule
    #   15.9   3064282   4,434 nodes     0 parents   = 4,434 = total    DEGENERATE
    #   15.9   3064664   4,469 nodes     0 parents   = 4,469 = total    DEGENERATE
    #
    # "0 - 0 = 0" closes for ANY coefficient on the parent term. Those two builds do not confirm the
    # rule in either direction; they are arithmetic, not evidence. Reporting them as confirmations
    # counted a term that was absent as a term that was measured and found to be zero.
    #
    # AND THE REASON THE TERM IS ABSENT IS THE FINDING. It is not that 15.9 has no data-driven
    # tests - they declare 80 [DataRow] and 8 [DataTestMethod] and all of them run. Their MSTest
    # adapter writes each case as its OWN TOP-LEVEL ROW with the arguments in the name, with no
    # parent and no <InnerResults> element at all:
    #
    #   15.9   adapter 14.0.2424.03    ...WithSpecialCharacters (1) / (2)   two flat rows
    #   16.11  adapter 14.0.4503.01    a parent row + <InnerResults> children carrying the same
    #                                  argument-suffixed names
    #
    # So the parent/child TRX shape is a property of the ADAPTER VERSION, not of the test population.
    # Measured on both trees: this line declares 242 [DataRow] and 12 [DataTestMethod].
    #
    # THE CONSEQUENCE FOR THIS GATE: an MSTest upgrade would change which shape this line's .trx
    # files carry, and therefore move the reconciliation and the portal comparison, on a build where
    # no test changed. That is a servicing-infrastructure change of exactly the kind this branch
    # exists to make. It is the strongest argument for PRINTING the reconciliation rather than
    # asserting it - the rule is a property of a tool version, not of this repository, and it can
    # move without anything here moving.
    #
    # Counters/@notExecuted is NOT usable and this is why the subtraction above uses the RESULTS:
    # it reads 0 on this build while 12 results carry outcome="NotExecuted", and the arithmetic only
    # closes if the results are taken as truth and that counter as wrong. 15.9 measured the same on
    # their line, and the VSIX validation session nearly filed a regression against shipped 17.14.1
    # for it before finding the same behaviour there. Any gate asserting "skipped == expected" from
    # that field is reading a number that reports zero while tests are being skipped.
    #
    # THE DECISIVE EVIDENCE IS DYNAMIC, AND IT IS 15.9's. Three static readings of a field that says
    # 0 could be a field with a different meaning, or an offset. This is not:
    #
    #   before the EventLogCollector fix   NotExecuted results 12   Counters/@notExecuted 0
    #   after  the EventLogCollector fix   NotExecuted results 13   Counters/@notExecuted 0
    #
    # The true value MOVED, for a nameable reason - the [Ignore]d test in the unblocked assembly
    # could not report NotExecuted while that assembly produced zero results - and the counter did
    # not move with it. A field wrong by a constant might be an offset; a field that stays at 0 while
    # the true value changes is not tracking the quantity at all.
    #
    # Attribution matters here because this line cannot verify half of it: the before-state, 12
    # results against a counter of 0, is measured locally on 3064456. The after-state of the COUNTER
    # was measured by 15.9 on their own line - this session read 13 results from its gate log while
    # ADO access was live, and never obtained the artifact needed to read the counter beside it.
    $summary = $doc.SelectSingleNode('//t:ResultSummary/t:Counters', $ns)
    if ($null -ne $summary) {
        $declaredExecuted += [int] $summary.executed
        $declaredTotal += [int] $summary.total
        $perFileDeclaredTotal[$file.Name] = [int] $summary.total
    }
    else {
        $perFileDeclaredTotal[$file.Name] = -1
    }

    # RUN-LEVEL DIAGNOSTICS, which this gate read past for its entire existence.
    #
    # A .trx carries more than test results. RunInfos hold what the RUN said about itself, and that
    # is where an empty file explains itself. The EventLogCollector net451 file contains:
    #
    #   [Warning] No test is available in ...EventLogCollector.UnitTests.dll. Make sure that test
    #             discoverer & executors are registered and platform & framework version settings
    #             are appropriate and try again.
    #
    # with ResultSummary outcome="Completed" and every counter at zero. Discovery found nothing,
    # said so as a WARNING, and the run reported success - which is the product behaviour reproduced
    # on this project today: exit 0 having discovered nothing.
    #
    # The explanation was inside the file this gate parses, and the gate selected only
    # Results/UnitTestResult and never looked at it. Collecting a population and ignoring the part
    # of it that says why it is empty is the same error as trusting a total.
    foreach ($info in $doc.SelectNodes('//t:RunInfos/t:RunInfo', $ns)) {
        $txtNode = $info.SelectSingleNode('t:Text', $ns)
        $txt = if ($null -ne $txtNode) { ($txtNode.InnerText -replace '\s+', ' ').Trim() } else { '' }
        $runInfos.Add([pscustomobject]@{ File = $file.Name; Outcome = $info.outcome; Text = $txt })
    }
}

Write-Host "Total results parsed : $totalResults   (ALL UnitTestResult nodes, nested included)"
Write-Host "                       THREE DIFFERENT NUMBERS DESCRIBE THIS BUILD. All are correct under"
Write-Host "                       their own definition, and comparing across them invents regressions:"
Write-Host "                         all nodes, nested included   <- this line, and it is the largest"
Write-Host "                         minus data-driven parents    = Counters/@total, below"
Write-Host "                         minus nested children        = the ADO test API and the Tests tab"
Write-Host "                       On 3064631 those were 5432, 5321 and 4934. The last is also what"
Write-Host "                       this line printed before 6ceb4166e: the LABEL did not change, the"
Write-Host "                       MEANING did. Do not diff this number across that commit, and do not"
Write-Host "                       compare it against the Tests tab - use the decomposition below."
Write-Host "                       All three COLLAPSE to one number on an MSTest adapter that writes"
Write-Host "                       data-driven cases flat, as 15.9's 14.0.2424.03 does. This line's"
Write-Host "                       14.0.4503.01 nests them, so an adapter upgrade would move these"
Write-Host "                       numbers apart with no test having changed."
Write-Host "Run's own 'executed' : $declaredExecuted   (printed, NOT asserted - see the parse loop)"
Write-Host "Not passed           : $($results.Count)"

# RECONCILIATION, printed rather than asserted. If it stops closing, something about how results are
# recorded has changed and a human should look - which is more useful than a gate blocking on a rule
# derived from a single build. The notExecuted COUNTER is deliberately not used; the RESULTS are.
$notExecutedResults = @($results | Where-Object { $_.Outcome -eq 'NotExecuted' }).Count
$step1 = $totalResults - $parentRows
$step2 = $step1 - $notExecutedResults
Write-Host ("Reconciliation       : {0} nodes - {1} data-driven parent row(s) = {2} vs counters total {3}  [{4}]" -f `
    $totalResults, $parentRows, $step1, $declaredTotal, $(if ($step1 -eq $declaredTotal) { 'closes' } else { 'DOES NOT CLOSE' }))
Write-Host ("                       of those nodes, {0} are NESTED cases under {1} parent(s) - un-netted, because a" -f $nestedResults, $parentRows)
Write-Host  "                       nested case can carry a failure and the netted figure hides how many can"
Write-Host ("                       nodes - nested children = {0}, which is what the ADO test API reports" -f ($totalResults - $nestedResults))
Write-Host ("                       {0} - {1} NotExecuted result(s) = {2} vs counters executed {3}  [{4}]" -f `
    $step1, $notExecutedResults, $step2, $declaredExecuted, $(if ($step2 -eq $declaredExecuted) { 'closes' } else { 'DOES NOT CLOSE' }))

# The parse is itself an instrument. A selector that matched nothing must not read as clean.
if ($totalResults -eq 0) {
    Write-Host "##vso[task.logissue type=error]Parsed $($trx.Count) .trx file(s) and found ZERO results."
    Write-Host 'Blocking. That is a parsing failure, not a green run.'
    exit 1
}

# --- A SUM CANNOT REPORT A ZERO IN ONE OF ITS TERMS ----------------------------------------------
#
# The check above asserts the TOTAL is non-zero, and that is not enough. On build 3064456 the total
# was 4,898 across 35 files and one of those files contained NOTHING:
#
#   Microsoft.TestPlatform.Extensions.EventLogCollector.UnitTests_net451_x64.trx    0 results
#
# That project declares 36 [TestMethod] and one [Ignore], and targets net451 only - so roughly 35
# tests did not run, produced a result file anyway, and this gate authorised signing. The identical
# file is empty on the 15.9 line's results too, so it is not a property of one agent.
#
# This is the defect the whole project is named after, re-made inside the control built to prevent
# it: a count that conceals its own terms. The earlier instance concealed 54 missing and 50 added
# behind "3 files wrong", and the fix then was to list the items. This control was written afterwards
# and still looked only at the total.
#
# So every result file must be non-empty, and an empty one must be DECLARED with a cause. Unknown
# empties block. Known empties are printed on every run rather than skipped, because the reason a
# file is empty is a fact about the run and travels with the artifact exactly like a failure does.
# TWO INDEPENDENT SIGNALS FOR EMPTINESS, because one of them can be defeated by how the file is read.
#
# This script counts results with SelectNodes, which returns 0 for an empty <Results/> element. The
# obvious alternative - $doc.TestRun.Results.UnitTestResult - returns a PHANTOM single node for that
# same file, so a per-file "has results" check written that way PASSES on a file containing no tests.
# 15.9 measured exactly that and was one keystroke from reporting their line clean. Measured here on
# the same file: dot-notation 1, SelectNodes 0, Counters/@total 0.
#
# So a file is empty if EITHER the parse found nothing OR the run's own Counters/@total is zero. The
# counters are authoritative for the question "did anything run", and keeping both means a future
# change to the parse cannot silently reintroduce the hole.
#
# The counters are NOT used for anything finer than that. After the csproj fix this same project
# reports counters total=36 executed=35 notExecuted=0 while the results contain a NotExecuted node -
# so any rule relating parsed results to counters would be wrong in at least one direction. This
# script prints both aggregate numbers and asserts nothing between them.
$emptyFiles = @($perFileCounts.Keys | Where-Object { $perFileCounts[$_] -eq 0 -or $perFileDeclaredTotal[$_] -eq 0 } | Sort-Object)
$expectedEmptyNames = @($baselineEmptyFiles | ForEach-Object { $_.emptyResultFile })
$unexpectedEmpty = @($emptyFiles | Where-Object { $expectedEmptyNames -notcontains $_ })
$declaredButNotEmpty = @($expectedEmptyNames | Where-Object { $emptyFiles -notcontains $_ })

Write-Host "Result files         : $($trx.Count)  (empty: $($emptyFiles.Count), expected empty: $($expectedEmptyNames.Count), UNEXPECTED empty: $($unexpectedEmpty.Count))"

if ($emptyFiles.Count -gt 0) {
    Write-Section 'EMPTY RESULT FILES - a .trx that ran and contained no tests'
    foreach ($e in $emptyFiles) {
        $entry = @($baselineEmptyFiles | Where-Object { $_.emptyResultFile -eq $e })
        if ($entry.Count -gt 0) {
            Write-Host "  EXPECTED    $e"
            Write-Host "      cause: $($entry[0].cause)"
        }
        else {
            Write-Host "  UNEXPECTED  $e   <- blocks"
        }
    }
}

if ($declaredButNotEmpty.Count -gt 0) {
    Write-Section 'DECLARED EMPTY BUT NOT EMPTY - the declaration has expired'
    Write-Host '  These files were declared as producing no results and now produce some. That is an'
    Write-Host '  improvement, not a failure: remove the declaration once confirmed.'
    foreach ($d in $declaredButNotEmpty) { Write-Host "  $d   now has $($perFileCounts[$d]) result(s)" }
}

# Printed whether or not anything is empty. A run-level warning on a file that DID produce results
# means a partial run, which is the same family and has no other reporting anywhere.
if ($runInfos.Count -gt 0) {
    Write-Section 'RUN-LEVEL DIAGNOSTICS - what the run said about itself'
    foreach ($info in ($runInfos | Sort-Object File)) {
        Write-Host ("  [{0,-7}] {1}" -f $info.Outcome, $info.File)
        Write-Host ("      {0}" -f $info.Text)
    }
}
else {
    Write-Host ''
    Write-Host "Run-level diagnostics: none across $($trx.Count) file(s)."
}

# --- classify -----------------------------------------------------------------------------------
$known = [System.Collections.Generic.List[object]]::new()
$newMode = [System.Collections.Generic.List[object]]::new()
$unknown = [System.Collections.Generic.List[object]]::new()
$notRun = [System.Collections.Generic.List[object]]::new()
$notRunUnexpected = [System.Collections.Generic.List[object]]::new()
$matchedKeys = [System.Collections.Generic.HashSet[string]]::new()

foreach ($r in $results) {
    # NOT-EXECUTED IS ITS OWN POPULATION, AND IT IS FAIL-CLOSED.
    #
    # It would be easy to write "NotExecuted is never a failure" and stop. That is wrong, and it is
    # wrong in this project's characteristic direction: NotExecuted is ALSO what MSTest records for
    # every test in a fixture whose class initialiser threw. Treating the whole outcome as benign
    # would let a crash that wipes out an entire fixture arrive as "several hundred tests did not
    # run, which is fine" - a large silent no-op, created by the very bucket added to stop one.
    #
    # So a not-executed result is never counted as a FAILURE, and is never silently ACCEPTED either.
    # The six expected names pass; anything else not executed blocks and is printed by name.
    if ($r.Outcome -eq 'NotExecuted') {
        $expected = @($baselineNotRun | Where-Object { $_.test -eq $r.Fqn })
        if ($expected.Count -gt 0) {
            [void] $matchedKeys.Add("$($r.Fqn)|$($r.Outcome)")
            $notRun.Add([pscustomobject]@{ Result = $r; Entry = $expected[0] })
        }
        else {
            $notRunUnexpected.Add($r)
        }
        continue
    }

    $key = "$($r.Fqn)|$($r.Outcome)"

    # ALL entries for this (test, outcome), not the first. The same test can fail differently in
    # different TRX files - it is compiled for several target frameworks and the runs are separate -
    # so an entry may legitimately need more than one signature. Build 3064120 measured exactly that:
    # ProtocolV1Tests.TestCaseDeserialize was baselined on a wall-clock message and the TRX carried
    # an IOException instead. Requirement 4 is unaffected: a genuinely new failure mode matches none
    # of the listed signatures and still blocks.
    # Searched over $baselineFailures, not $baseline. The baseline now also holds entries that declare
    # an empty result FILE and carry no 'test' property at all, and under Set-StrictMode reading
    # $_.test on one of those is a hard error rather than a null - which is how this line was caught.
    $entries = @($baselineFailures | Where-Object { $_.test -eq $r.Fqn -and $_.outcome -eq $r.Outcome })

    if ($entries.Count -eq 0) { $unknown.Add($r); continue }

    $matched = $null
    foreach ($entry in $entries) {
        # An empty baseline signature asserts the result carries NO message. It is not a wildcard.
        $signatureMatches =
            if ([string]::IsNullOrEmpty($entry.signature)) { [string]::IsNullOrEmpty($r.Message) }
            else { $r.Message -like "*$($entry.signature)*" }
        if ($signatureMatches) { $matched = $entry; break }
    }

    if ($null -ne $matched) {
        [void] $matchedKeys.Add($key)
        $known.Add([pscustomobject]@{ Result = $r; Entry = $matched })
    }
    else {
        $newMode.Add([pscustomobject]@{ Result = $r; Entry = $entries[0] })
    }
}

$stale = @($testEntries | Where-Object { -not $matchedKeys.Contains("$($_.test)|$($_.outcome)") })

# --- report -------------------------------------------------------------------------------------
Write-Section 'NOT-PASSED SET (travels with any delivered artifact)'
if ($results.Count -eq 0) {
    Write-Host '  none'
}
else {
    foreach ($r in ($results | Sort-Object Fqn, Outcome)) {
        Write-Host ("  {0,-12} {1}" -f $r.Outcome, $r.Fqn)
    }
}

Write-Section 'CLASSIFICATION'
Write-Host ("  known, name and signature both match : {0}" -f $known.Count)
Write-Host ("  KNOWN TEST, NEW FAILURE MODE         : {0}   <- blocks" -f $newMode.Count)
Write-Host ("  UNKNOWN, not in the baseline         : {0}   <- blocks" -f $unknown.Count)
Write-Host ("  not executed, expected               : {0}   <- not a failure, does not block" -f $notRun.Count)
Write-Host ("  NOT EXECUTED, UNEXPECTED             : {0}   <- blocks" -f $notRunUnexpected.Count)
Write-Host ("  baseline entries that did NOT fire   : {0}   <- reported, does not block" -f $stale.Count)
Write-Host ("  SUM CHECK  {0} + {1} + {2} + {3} + {4} = {5}, not-passed = {6}" -f `
    $known.Count, $newMode.Count, $unknown.Count, $notRun.Count, $notRunUnexpected.Count, `
    ($known.Count + $newMode.Count + $unknown.Count + $notRun.Count + $notRunUnexpected.Count), $results.Count)

# Every count above is followed by the items it counted. A count is a scalar and a scalar cannot be
# inspected; a listing can. This project once reported "27 entries" from an expression that counted
# 27 nulls - the number was correct, the reading was vacuous, and nothing about the number could ever
# have revealed it. The blank listing beside it was the only tell.
if ($known.Count -gt 0) {
    Write-Section 'MATCHED - name and signature, the entries this gate is passing'
    foreach ($k in ($known | Sort-Object { $_.Result.Fqn })) {
        Write-Host ("  {0,-12} {1}" -f $k.Result.Outcome, $k.Result.Fqn)
        Write-Host ("      matched signature : '{0}'" -f $k.Entry.signature)
    }
}

if ($newMode.Count -gt 0) {
    Write-Section 'KNOWN TEST, NEW FAILURE MODE - the baselined cause no longer describes this failure'
    foreach ($n in $newMode) {
        Write-Host "  $($n.Result.Fqn)"
        Write-Host "      baselined cause : $($n.Entry.cause)"
        Write-Host "      expected sig    : '$($n.Entry.signature)'"
        Write-Host "      actual message  : $($n.Result.Message)"
    }
}

if ($unknown.Count -gt 0) {
    Write-Section 'UNKNOWN - not in the baseline'
    foreach ($u in $unknown) {
        Write-Host "  $($u.Outcome)  $($u.Fqn)"
        Write-Host "      $($u.Message)"
    }
}

if ($notRun.Count -gt 0) {
    Write-Section 'NOT EXECUTED, EXPECTED - these did not run, and could not have failed'
    Write-Host '  Kept visible deliberately. Moving a result out of the failure bucket must not move'
    Write-Host '  it into silence - the reason it did not run is a fact about this run and travels'
    Write-Host '  with the artifact exactly like a failure does.'
    foreach ($n in ($notRun | Sort-Object { $_.Result.Fqn })) {
        Write-Host ("  {0,-12} {1}" -f $n.Result.Outcome, $n.Result.Fqn)
        Write-Host ("      cause: {0}" -f $n.Entry.cause)
    }
}

if ($notRunUnexpected.Count -gt 0) {
    Write-Section 'NOT EXECUTED, UNEXPECTED - blocks'
    Write-Host '  A test that stopped running without being declared is not a pass. The usual cause is'
    Write-Host '  a class initialiser that threw, which marks EVERY test in that fixture NotExecuted -'
    Write-Host '  so this is the shape a fixture-wide crash arrives in.'
    foreach ($u in ($notRunUnexpected | Sort-Object Fqn)) {
        Write-Host "  $($u.Fqn)"
        if (-not [string]::IsNullOrEmpty($u.Message)) { Write-Host "      $($u.Message)" }
    }
}

if ($stale.Count -gt 0) {
    Write-Section 'STALE BASELINE ENTRIES - did not fire this run'
    Write-Host '  These are not failures. They are entries whose justification may have expired -'
    Write-Host '  a test that now passes, or an agent image that changed. Remove them once confirmed.'
    foreach ($s in $stale) {
        Write-Host ("  {0,-12} {1}" -f $s.outcome, $s.test)
        Write-Host ("      cause: {0}   classified on run {1}, {2}" -f $s.cause, $s.classifiedOnRun, $s.classifiedOn)
    }
}

$blocking = $newMode.Count + $unknown.Count + $notRunUnexpected.Count + $unexpectedEmpty.Count
Write-Section 'GATE'
if ($blocking -gt 0) {
    Write-Host "##vso[task.logissue type=error]$blocking unclassified test result(s). Signing is blocked."
    Write-Host ("BLOCKED: {0} known test(s) with a new failure mode, {1} unknown, {2} unexpectedly not executed, {3} result file(s) unexpectedly empty." -f `
        $newMode.Count, $unknown.Count, $notRunUnexpected.Count, $unexpectedEmpty.Count)
    exit 1
}

# Stated as the partition rather than as one sentence about all of them, because the two populations
# are asserted to DIFFERENT standards and a single claim would overstate one of them: failures match
# on name AND signature, not-executed entries match on name only - there is no failure message to
# match against, and inventing one would assert something the artifact does not carry.
Write-Host ("PASSED: {0} failure(s) matched a baselined entry by name AND signature; {1} not-executed result(s) matched an expected entry by name." -f `
    $known.Count, $notRun.Count)
Write-Host 'Signing may proceed.'
exit 0
