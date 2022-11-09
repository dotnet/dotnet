
function ReadPropertiesFile {
    Param(
        [Parameter(Mandatory=$True)]
        [string]$Path
    )
    Write-Verbose "Reading properties file '$Path'"
    $properties = @{}
    Get-Content $Path |
        ? { -not $_.StartsWith('#') } |
        % { $_.Trim() } |
        ? { $_ -ne '' } |
        % { $tokens = $_ -split "\s*=\s*"; $properties.Add($tokens[0], $tokens[1]) }
    $properties
}

function ReplaceTextInFiles {
    [CmdletBinding(SupportsShouldProcess=$True)]
    Param(
        [Parameter(ValueFromPipeline=$True, Mandatory=$True, Position=0)]
        [string[]]$Files,
        [Parameter(Mandatory=$True)]
        [Alias('old')]
        [string]$OldText,
        [Parameter(Mandatory=$True)]
        [Alias('new')]
        [string]$NewText,
        [Alias('ef')]
        [string]$ExcludeFilter
    )
    Process {
        $Files |
            ?{ (Get-Content $_ | Out-String) -match $OldText } |
            ?{ $pscmdlet.ShouldProcess($_, 'replace text') } |
            %{
                $lines = Get-Content $_

                $updated = switch ($lines) {
                    { -not $ExcludeFilter -or $_ -notlike $ExcludeFilter } { $_.Replace($OldText, $NewText) }
                    Default { $_ }
                }

                Set-Content $_ $updated | Out-Null
            }
    }
}

Function New-TempDir {
    $TempDir = Join-Path $env:TEMP "nuget_$([System.Guid]::NewGuid())"
    New-Item -ItemType Directory -Force -Path $TempDir | Out-Null
    $TempDir
}
