# Assert-DeliveryArtifacts.ps1
#
# Two checks that were being held as ANALYST DISCIPLINE, converted into a script that fails loudly.
# old-pipeline-rebuild-strategy.md section 6: "Put the check in the script, not in the analyst."
# Written for internal/rel/17.3 (D-2) but the logic is line-agnostic; only the defaults are 17.3's.
#
#   .\Assert-DeliveryArtifacts.ps1 -PackagesDirectory artifacts\Release\packages `
#                                  -TestResultsDirectory artifacts\TestResults\Release
#
# Exit codes:  0 = all assertions passed   1 = an assertion FAILED   2 = COULD NOT VERIFY
#
# -----------------------------------------------------------------------------------------------
# CHECK 1 - THE TEST STEP MUST HAVE ACTUALLY RUN TESTS.
#
# On builds 3066271 and 3066280 the Test step reported SUCCESS in 11.68 seconds and produced ZERO
# .trx files. A step that runs nothing cannot fail, so its green is not evidence. Only the signing
# gate caught it, and only because that gate distinguishes "no failures" from "no measurement".
#
# CHECK 2 - ARCADE 11 MUST NOT HAVE RE-SIGNED CARRIED PAYLOAD.
#
# Arcade 11 adds FileExtensionSignInfo defaults Arcade 7 lacked. An extension the repo does not
# declare is re-signed with NO error and NO warning on a green build. eng/Signing.props pins .cab
# and .dylib to None; this asserts the pin worked.
#
# TWO THINGS THIS SCRIPT DOES THAT A HAND-RUN CHECK KEPT GETTING WRONG:
#
#   1. IT SCANS INSIDE THE PACKAGES, NOT THE OUTPUT DIRECTORY. A clean build of this branch produces
#      ZERO .cab under artifacts\ - the cab exists only inside the produced nupkg. A filesystem
#      sweep reports the line clean while the cab is being re-signed.
#
#   2. IT COMPARES SHA-256, NOT THE SIGNER NAME. The prevailing advice is "check WHO signed it",
#      which comes from 15.9 whose shipped cab is CN=VS Bld Lab. rel/17.3's shipped cab is signed
#      CN=Microsoft Corporation, and Arcade 11 re-signs .cab with Microsoft400 - ALSO Microsoft. So
#      on this line the subject name reads IDENTICAL before and after a re-sign, and a name check
#      returns clean on a modified file. The discriminator has to be the hash.
# -----------------------------------------------------------------------------------------------

[CmdletBinding()]
param(
  [string] $PackagesDirectory    = 'artifacts\Release\packages',
  [string] $TestResultsDirectory = 'artifacts\TestResults\Release',
  [switch] $SkipTestResults,
  [switch] $SkipSigning
)

$ErrorActionPreference = 'Stop'
$failed = $false

function Fail-Unverified([string]$why) {
  Write-Host ""
  Write-Host "  COULD NOT VERIFY: $why" -ForegroundColor Red
  Write-Host "  This is NOT a pass." -ForegroundColor Red
  exit 2
}

# Extensions Arcade 11 added or reassigned relative to Arcade 7. '.dll' is FIRST and is the
# POSITIVE CONTROL: a scan reporting zero affected extensions proves nothing unless it can
# demonstrate it sees files nested inside a nupkg at all.
$WatchedExtensions = '.dll', '.cab', '.dylib', '.msi', '.pkg', '.app', '.deb', '.rpm', '.tar.gz', '.js'

# 🛑 EMPTIED ON PURPOSE FOR 17.8, AND THE PIPELINE PASSES -SkipSigning BECAUSE OF IT.
#
# This file was ported from 17.3, where this table held the SHA-256 of IntelliTraceCollection.cab
# as shipped in Microsoft.TestPlatform 17.3.3. THOSE BYTES ARE 17.3's. Carrying them onto 17.8
# would not be a conservative default - it would be a wrong answer in one of two directions:
# either a hash mismatch reported as a re-signing defect, or check 2 finding none of its known
# files and reporting "verified NOTHING". This log's own rule is explicit about it:
# MEASURE A HASH TABLE, NEVER COPY ONE FROM ANOTHER LINE - 4 of 7 differed the last time.
#
# So 17.8's reference bytes are NOT YET MEASURED and this half is NOT YET RUN. The pipeline step
# passes -SkipSigning, so check 1 (test results actually exist) still runs and is fully
# line-agnostic. TO CLOSE THIS: download the shipped Microsoft.TestPlatform 17.8.x, hash its
# carried payload, put the rows here, and DROP -SkipSigning from the pipeline step.
#
# Leaving the table empty is fail-closed rather than fail-quiet: if -SkipSigning is ever removed
# before the rows are measured, line 129 below calls Fail-Unverified and exits 2 instead of
# printing a pass. That is deliberate.
$ShippedPayloadHashes = @{
}

Write-Host ""
Write-Host "  Assert-DeliveryArtifacts" -ForegroundColor Cyan
Write-Host "  script SHA-256: $((Get-FileHash $PSCommandPath -Algorithm SHA256).Hash)"
Write-Host ""

# --- CHECK 1 : the Test step actually produced results --------------------------------------
if (-not $SkipTestResults) {
  Write-Host "  [1] TEST RESULTS EXIST" -ForegroundColor Cyan
  if (-not (Test-Path $TestResultsDirectory)) {
    Write-Host "      FAILED - results directory does not exist: $TestResultsDirectory" -ForegroundColor Red
    Write-Host "      A Test step that produced no directory did not run tests." -ForegroundColor Red
    $failed = $true
  }
  else {
    $trx = @(Get-ChildItem -Path $TestResultsDirectory -Recurse -Filter *.trx -File -ErrorAction SilentlyContinue)
    Write-Host "      .trx files: $($trx.Count)"
    if ($trx.Count -eq 0) {
      Write-Host "      FAILED - zero .trx files." -ForegroundColor Red
      Write-Host "      Zero result files is not zero failures. A step that runs nothing cannot fail," -ForegroundColor Red
      Write-Host "      so its green is not evidence. Measured on builds 3066271 and 3066280:" -ForegroundColor Red
      Write-Host "      Test reported SUCCESS in 11.68s having produced no results at all." -ForegroundColor Red
      $failed = $true
    }
    else {
      # A .trx can exist and contain no results - a parsing or harness failure rather than a run.
      $total = 0
      foreach ($f in $trx) {
        try {
          [xml]$doc = Get-Content $f.FullName -Raw
          $total += @($doc.TestRun.Results.ChildNodes | Where-Object { $_.NodeType -eq 'Element' }).Count
        } catch {
          Write-Host "      WARNING - could not parse $($f.Name): $($_.Exception.Message)" -ForegroundColor Yellow
        }
      }
      Write-Host "      test results parsed: $total"
      if ($total -eq 0) {
        Write-Host "      FAILED - parsed $($trx.Count) .trx file(s) and found ZERO results." -ForegroundColor Red
        Write-Host "      That is a parsing or harness failure, not a green run." -ForegroundColor Red
        $failed = $true
      } else {
        Write-Host "      PASS" -ForegroundColor Green
      }
    }
  }
  Write-Host ""
}

# --- CHECK 2 : Arcade 11 did not re-sign carried payload ------------------------------------
if (-not $SkipSigning) {
  Write-Host "  [2] CARRIED PAYLOAD UNCHANGED, SCANNED INSIDE THE PACKAGES" -ForegroundColor Cyan

  if ($ShippedPayloadHashes.Count -eq 0) {
    Fail-Unverified "the shipped-payload hash table is empty, so this check could only ever pass."
  }

  if (-not (Test-Path $PackagesDirectory)) {
    Write-Host "      packages directory does not exist: $PackagesDirectory"
    Write-Host "      Packaging did not get that far. NOT a pass and NOT a failure of signing." -ForegroundColor Yellow
    Fail-Unverified "no packages to scan. Reporting zero here would be a zero for the wrong reason."
  }

  Add-Type -AssemblyName System.IO.Compression.FileSystem
  $pkgs = @(Get-ChildItem $PackagesDirectory -Filter *.nupkg -File -Recurse -ErrorAction SilentlyContinue)
  if ($pkgs.Count -eq 0) { Fail-Unverified "no .nupkg found under $PackagesDirectory." }

  $counts  = @{}; foreach ($e in $WatchedExtensions) { $counts[$e] = 0 }
  $entries = 0
  $checked = 0
  $tmp     = Join-Path ([IO.Path]::GetTempPath()) ("payloadcheck-" + [guid]::NewGuid().ToString('N'))
  New-Item -ItemType Directory -Force -Path $tmp | Out-Null

  try {
    foreach ($p in $pkgs) {
      $zip = $null
      try { $zip = [IO.Compression.ZipFile]::OpenRead($p.FullName) }
      catch { Write-Host "      WARNING - could not open $($p.Name)" -ForegroundColor Yellow; continue }

      foreach ($entry in $zip.Entries) {
        $entries++
        $lower = $entry.FullName.ToLowerInvariant()
        foreach ($x in $WatchedExtensions) { if ($lower.EndsWith($x)) { $counts[$x]++ } }

        $leaf = Split-Path $entry.FullName -Leaf
        if ($ShippedPayloadHashes.ContainsKey($leaf)) {
          $expected = $ShippedPayloadHashes[$leaf]
          $out = Join-Path $tmp $leaf
          [IO.Compression.ZipFileExtensions]::ExtractToFile($entry, $out, $true)
          $actualHash = (Get-FileHash $out -Algorithm SHA256).Hash
          $actualSize = (Get-Item $out).Length
          $checked++

          Write-Host "      $leaf   in $($p.Name)"
          Write-Host "        size    expected $($expected.Size)  actual $actualSize"
          Write-Host "        sha256  expected $($expected.Sha256)"
          Write-Host "                actual   $actualHash"

          if ($actualHash -ne $expected.Sha256) {
            Write-Host "        FAILED - CONTENT DIFFERS FROM THE SHIPPED FILE." -ForegroundColor Red
            Write-Host "        $($expected.Note)" -ForegroundColor Red
            $sig = Get-AuthenticodeSignature $out
            Write-Host "        delivered signer: $($sig.SignerCertificate.Subject)" -ForegroundColor Red
            Write-Host "        delivered thumb : $($sig.SignerCertificate.Thumbprint)" -ForegroundColor Red
            Write-Host "        shipped   signer: $($expected.Subject)" -ForegroundColor Red
            Write-Host "        shipped   thumb : $($expected.Thumbprint)" -ForegroundColor Red
            Write-Host "        NOTE: the SUBJECT may match while the file differs - on this line both" -ForegroundColor Red
            Write-Host "        the shipped cert and Arcade's Microsoft400 read CN=Microsoft Corporation." -ForegroundColor Red
            Write-Host "        The HASH is the discriminator, not the name." -ForegroundColor Red
            $failed = $true
          } else {
            Write-Host "        PASS - byte-identical to the shipped file" -ForegroundColor Green
          }
          Remove-Item $out -Force -ErrorAction SilentlyContinue
        }
      }
      $zip.Dispose()
    }
  }
  finally { Remove-Item $tmp -Recurse -Force -ErrorAction SilentlyContinue }

  Write-Host ""
  Write-Host "      scanned $($pkgs.Count) nupkgs, $entries entries"
  foreach ($e in $WatchedExtensions) { Write-Host ("        {0,-10} {1}" -f $e, $counts[$e]) }
  Write-Host ""

  # POSITIVE CONTROL. Without this, every zero above could mean "saw nothing" rather than
  # "nothing to see", and the check would report success precisely when it was blind.
  if ($counts['.dll'] -eq 0) {
    Write-Host "      FAILED - POSITIVE CONTROL: zero .dll entries seen inside the packages." -ForegroundColor Red
    Write-Host "      The scan is blind, so every other zero above is meaningless." -ForegroundColor Red
    $failed = $true
  } else {
    Write-Host "      positive control OK - $($counts['.dll']) .dll entries seen inside packages" -ForegroundColor Green
  }

  # A hash table with rows that never matched is the same defect in a different place: the check
  # ran, found nothing to check, and would have reported success.
  if ($checked -eq 0) {
    Write-Host "      FAILED - none of the $($ShippedPayloadHashes.Count) known carried file(s) were found in any package." -ForegroundColor Red
    Write-Host "      Either packaging did not stage them, or the table names files this line does not ship." -ForegroundColor Red
    Write-Host "      Both mean this check verified NOTHING." -ForegroundColor Red
    $failed = $true
  } else {
    Write-Host "      carried files verified: $checked of $($ShippedPayloadHashes.Count) known" -ForegroundColor Green
  }
  Write-Host ""
}

if ($failed) {
  Write-Host "  RESULT: FAILED" -ForegroundColor Red
  exit 1
}
Write-Host "  RESULT: all assertions passed" -ForegroundColor Green
exit 0
