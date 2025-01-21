[CmdletBinding(PositionalBinding = $false)]
param(
    [string]$flags = ''
)

if ($flags -eq '') {
    # Reads flags from the build name if not provided
    # Example: FeatureFlags - ForceRuntimeCodeGeneration,UseRazorCohostServer
    $flags = $env:Build_CronSchedule_DisplayName).Substring(0, 'FeatureFlags -'.Length)
}

# Matches src/Razor/src/Microsoft.VisualStudio.LanguageServices.Razor/WellKnownFeatureFlagNames.cs
$knownFlags = @{
    ShowAllCSharpCodeActions = "Razor.LSP.ShowAllCSharpCodeActions";
    IncludeProjectKeyInGeneratedFilePath = "Razor.LSP.IncludeProjectKeyInGeneratedFilePath";
    UsePreciseSemanticTokenRanges = "Razor.LSP.UsePreciseSemanticTokenRanges";
    UseRazorCohostServer = "Razor.LSP.UseRazorCohostServer";
    DisableRazorLanguageServer = "Razor.LSP.DisableRazorLanguageServer";
    ForceRuntimeCodeGeneration = "Razor.LSP.ForceRuntimeCodeGeneration";
    UseRoslynTokenizer = "Razor.LSP.UseRoslynTokenizer";
}

Write-Host "Setting flags from $flags"

foreach ($flag in $flags.Split(',')) {
    Write-Host "Searching for '$flag'"

    $match = $knownFlags.Keys | ?{ $_ -ieq $flag } | select -first 1

    if ($match -ne $NULL) {
        $value = $knownFlags[$match]
        Write-Host "$match -> $value"
        Write-Host "Setting $value"
        & "./featureFlag.ps1 -set -enable -flag $value"
    }
}

Write-Host "Done setting flags"