[CmdletBinding()]
param()

# Runs the publish-readiness gate against the real skills tree, so a regression that would block publishing
# (a private path in a test, a Teams permalink in the knowledge pack, a stray binary, state beside the
# scripts, a dangling reference) fails the ordinary test run instead of surfacing on the day of publishing.
# Warnings (size budgets, internal links) are reported but never fail this test; they need a person.

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot '..\verify\publish.ps1')

$skillsRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..\..'))
$ownFolder = Split-Path -Leaf ([IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..')))
$findings = @(Invoke-VmrQBPublishReadiness -Root $skillsRoot -SkillFolder $ownFolder)
$blocking = @($findings | Where-Object severity -eq 'fail')
$warnings = @($findings | Where-Object severity -eq 'warn')

foreach ($finding in $warnings) { Write-Host "WARN: [$($finding.check)] $($finding.path) - $($finding.message)" -ForegroundColor Yellow }
if ($blocking.Count) {
    foreach ($finding in $blocking) { Write-Host "FAIL: [$($finding.check)] $($finding.path):$($finding.line) - $($finding.message)" -ForegroundColor Red }
    Write-Host "$($blocking.Count) blocking publish-readiness finding(s) in $skillsRoot." -ForegroundColor Red
    exit 1
}
Write-Host "PASS: no blocking publish-readiness findings ($($warnings.Count) warning(s) for a person to judge)." -ForegroundColor Green
Write-Host '1 offline tree check. Reads files only; no network, email, or external changes.' -ForegroundColor Green
