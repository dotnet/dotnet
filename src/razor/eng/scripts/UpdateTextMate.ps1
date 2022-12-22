# This script updates our *.tmLanguage.json files from the latest copies int eh VS Code repo.
#
# We pull these files from the VS Code repo, and VS Code in turn pulls them from various repos,
# but rather than pull directly from them, by using the VS Code repo we get the benefit of their
# testing, and their finding the best sources for the various languages.
#
# We download the current files in the VS Code main branch and put them in our repo,
# then update the cgmanifest.json file to point to the commit we pulled them from.

# Need to run this first: Install-Module -Name PowerShellForGitHub
Import-Module -Name PowerShellForGitHub

function DownloadTmLanguageJson {
    param (
        [string]$sha,
        [string]$lang,
        [string]$filename
    )

    if ($filename -eq "") {
        $filename = "$lang.tmLanguage.json"
    }

    # tmLanguage.json
    $url = "https://raw.githubusercontent.com/microsoft/vscode/$sha/extensions/$lang/syntaxes/$filename"
    Write-Host "Downloading $url"

    $content = Invoke-WebRequest -Uri $url

    # Copy to razor extension
    $content.content | Out-File -FilePath "../../src/Razor/src/Microsoft.VisualStudio.RazorExtension/EmbeddedGrammars/$fileName" -Encoding ascii

    # Copy to grammar tests
    $content.content | Out-File -FilePath "../../src/Razor/test/Microsoft.AspNetCore.Razor.VSCode.Grammar.Test/embeddedGrammars/$fileName" -Encoding ascii
}

# Find the current main branch SHA to download from
$branch = Get-GitHubBranch -OwnerName "microsoft" -RepositoryName "vscode" -BranchName "main"
$sha = $branch.commit.sha
Write-Host "VS Code main branch SHA: $sha"

DownloadTmLanguageJson -sha $sha -lang "csharp"
DownloadTmLanguageJson -sha $sha -lang "css"
DownloadTmLanguageJson -sha $sha -lang "html"
# GitHub URLs are case sensetive, and JavaScript is special
DownloadTmLanguageJson -sha $sha -lang "javascript" -filename "JavaScript.tmLanguage.json"

Write-Host "Updating cgmanifest.json"

# Read in the current file
$manifest = Get-Content -Path "../../src/Razor/src/Microsoft.VisualStudio.RazorExtension/cgmanifest.json" | ConvertFrom-Json

# Update commit hash and version URL
$manifest.registrations[0].component.git.commitHash = $sha
$manifest.registrations[0].version = "https://github.com/microsoft/vscode/tree/$sha"

# Write the file back out again
$jsonString = $manifest | ConvertTo-Json -Depth 10
$jsonString | Set-Content -Path "../../src/Razor/src/Microsoft.VisualStudio.RazorExtension/cgmanifest.json" -Encoding ascii
