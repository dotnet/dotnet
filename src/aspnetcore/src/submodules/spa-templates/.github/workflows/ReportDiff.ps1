# Check the code is in sync
$changed = (select-string "nothing to commit" artifacts\status.txt).count -eq 0
if (-not $changed) { return $changed }

# Check if there's an open PR in the target repo to resolve this difference.
$sendpr = $true
$Headers = @{ Accept = 'application/vnd.github.v3+json' };

$prsLink = "https://api.github.com/repos/${env:TargetRepo}/pulls?state=open"
$result = Invoke-RestMethod -Method GET -Headers $Headers -Uri $prsLink

foreach ($pr in $result) {
  if ($pr.body -And $pr.title.Contains("Sync shared code from ${env.TargetRepoName}")) {
    $sendpr = $false
    return $sendpr
  }
}

return $sendpr